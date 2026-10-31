#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeferredBenefitsProgramsRuntime
{
    internal bool TryBeginFactionRecoveryDebts(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying, bool survived)
    {
        var debts = _coreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programFactionRecoveryDebts.Where(item => item.DyingFrameId == dying.FrameId).ToArray();
        if (debts.Length == 0)
            return false;
        _coreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programFactionRecoveryDebts.RemoveAll(item => item.DyingFrameId == dying.FrameId);
        var first = debts[0];
        var program = _contentRegistryOperations._contentRegistry.GetSkill(first.SkillId).Program!;
        var trigger = ProgramInstructionResolver.Default.Resolve(program, ProgramInstructionSourceKind.Trigger, first.BindingId).Trigger!;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var debtReturn = new FactionRecoveryDebtReturn(dying.FrameId, dying.ParentFrameId, dying.VictimSeat, dying.Continuation, survived, _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId, _coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id);
        _coreRuntimeOperations.PushRuntimeFrame(new ProgramSkillFrame(id, first.OwnerSeat, first.SkillId, first.BindingId, program.GameplayHash, trigger.Effects.Count, [], []) { TriggerId = first.BindingId, SkillInstanceId = first.SkillInstanceId, FactionRecoveryDebtReturn = debtReturn, WindowContext = new(trigger.Window, dying.FrameId, first.OwnerSeat), FactionRecoveryDraft = new(dying.FrameId, debts.Select(item => item.ResponderSeat).ToArray(), 0, debts.Select(item => item.ResponderSeat).ToArray(), SettlingDebts: true) });
        TryContinueFactionRecoveryDebts(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 1030)]
    internal bool TryContinueFactionRecoveryDebts(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FactionRecoveryDraft.SettlingDebts: true } frame || frame.Id != frameId)
            return false;
        var continuation = frame.FactionRecoveryDebtReturn ?? throw new InvalidOperationException("Faction damage debts lost their original dying continuation.");
        var draft = frame.FactionRecoveryDraft!;
        var cursor = draft.ResponderIndex;
        while (cursor < draft.ResponderSeats.Count && !_coreMatchParticipantsPlayersAccess._players[draft.ResponderSeats[cursor]].IsAlive)
            cursor++;
        if (cursor >= draft.ResponderSeats.Count)
        {
            _coreRuntimeOperations.PopResolutionFrame(frameId, ResolutionFrameKind.ProgramSkill);
            if (frame.AttackAttempt is not null || _coreDyingCursorRuntimeOperations.ActiveDying is not null)
                throw new InvalidOperationException("Settled faction damage retained an unfinished child resolution.");
            if (_coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId != continuation.ParentAttackOwnerFrameId || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id != continuation.ParentDamageWindowFrameId)
                throw new InvalidOperationException("Settled faction damage lost its outer damage frame.");
            _coreNativeDyingOperations.ContinueDyingAfterFactionRecoveryDebts(new global::CardGame.Core.GameEngine.DyingCompletionReceipt(continuation.DyingFrameId, continuation.DyingParentFrameId, continuation.DyingVictimSeat, continuation.DyingContinuation), continuation.Survived);
            return true;
        }

        var seat = draft.ResponderSeats[cursor];
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { FactionRecoveryDraft = draft with { ResponderIndex = cursor + 1 }, WindowContext = frame.WindowContext!with { TargetSeat = seat } });
        // The internal seat anchors resolution; attribution is explicitly absent.
        _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), seat, 1, new ProgramParticipantReference(ProgramParticipantRef.EventTarget), sourceLess: true);
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.IDeferredBenefitsProgramHost.RequestFactionRecovery(ProgramSkillFrame frame, string factionId) => RequestProgramFactionRecovery(frame, factionId);
    void global::CardGame.Core.IDeferredBenefitsProgramHost.SetNextTurnRuleModifier(ProgramSkillFrame frame, int targetSeat, SkillRuleQuery query, int amount) => QueueProgramNextTurnRuleModifier(frame, targetSeat, query, amount);
    SkillProgramStepOutcome global::CardGame.Core.IDeferredBenefitsProgramHost.WeaponDiscardOrDamageBonus(ProgramSkillFrame frame, int amount) => BeginProgramWeaponDamageChoice(frame, amount);
    void global::CardGame.Core.IDeferredBenefitsProgramHost.ResolveJudgmentColorBenefit(ProgramSkillFrame frame, int targetSeat, string sourceBind, int recovery) => ResolveProgramJudgmentColorBenefit(frame, targetSeat, sourceBind, recovery);
    internal int CountPlayPhaseDamageTakenByAny() => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play ? 0 : _corePlayCardHistoryOperations.EventsSinceLastBoundary(item => item is TurnStartedEvent or PhaseChangedEvent { Phase: TurnPhase.Play }).OfType<DamageAppliedEvent>().Count();
    internal void QueueProgramNextTurnRuleModifier(ProgramSkillFrame frame, int targetSeat, SkillRuleQuery query, int amount)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetSeat == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || query is not (SkillRuleQuery.HandLimit or SkillRuleQuery.SlashLimit or SkillRuleQuery.DrawCount) || amount is < 1 or > 20)
            throw new InvalidOperationException("A next-turn grant needs a living other target and a bounded hand or Slash allowance.");
        var grant = new ProgramNextTurnRuleModifier(frame.Id, frame.InstructionIndex - 1, targetSeat, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame), query, amount);
        if (_coreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programNextTurnRuleModifiers.Any(item => item.ParentFrameId == grant.ParentFrameId && item.EffectIndex == grant.EffectIndex))
            throw new InvalidOperationException("A next-turn grant cannot be scheduled twice.");
        _coreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programNextTurnRuleModifiers.Add(grant);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramNextTurnRuleModifierQueuedEvent(grant));
    }

    internal void ApplyQueuedNextTurnRuleModifiers(int targetSeat)
    {
        foreach (var scheduled in _coreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programNextTurnRuleModifiers.Where(item => item.TargetSeat == targetSeat).ToArray())
        {
            _coreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programNextTurnRuleModifiers.Remove(scheduled);
            if (!_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
                continue;
            var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantRuleModifier(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, scheduled.ParentFrameId, scheduled.EffectIndex, scheduled.Source, scheduled.Query, SkillRuleOperation.Add, scheduled.Amount, affectedSeat: targetSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnRuleModifierGrantedEvent(granted));
        }
    }

    internal void ResolveProgramJudgmentColorBenefit(ProgramSkillFrame frame, int targetSeat, string sourceBind, int recovery)
    {
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), sourceBind);
        var suit = binding.FrozenRevealedSuit ?? throw new InvalidOperationException("A judgment benefit requires its final frozen suit.");
        if (suit is Suit.Heart or Suit.Diamond)
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frame.Id, frame.OwnerSeat, targetSeat, recovery, null, null);
        else if (suit is Suit.Spade or Suit.Club)
            _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, targetSeat, frame.WindowContext?.Amount ?? 0, null, null, SkillProgramCardSetVisibility.Private, new CardMoveReason("skill-program.judgment-color.draw"));
    }

    internal SkillProgramStepOutcome RequestProgramFactionRecovery(ProgramSkillFrame frame, string factionId)
    {
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("Faction recovery has no dying occurrence.");
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering } context || context.ParentFrameId != _resolutionStackOperations._resolutionStack[^2].Id || dying.VictimSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Faction recovery lost its entering dying owner.");
        var responders = Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount - 1).Select(offset => _coreTableOrderOperations.TableStepSeat(frame.OwnerSeat, offset)).Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[seat]) == factionId).ToArray();
        if (responders.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { FactionRecoveryDraft = new(dying.FrameId, responders, 0, []) });
        PublishFactionRecoveryPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishFactionRecoveryPrompt(ProgramSkillFrame frame)
    {
        var draft = frame.FactionRecoveryDraft!;
        var chooser = draft.ResponderSeats[draft.ResponderIndex];
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var choices = new[]
        {
            new PromptChoice(new ChoiceId("faction-recovery.accept"), "令其回复1点体力，濒死结算结束后你受到1点伤害", [], [frame.OwnerSeat], new Dictionary<string, string> { ["program-action"] = "faction-recovery", ["accept"] = "true" }),
            new PromptChoice(new ChoiceId("faction-recovery.decline"), "不响应", [], [], new Dictionary<string, string> { ["program-action"] = "faction-recovery", ["accept"] = "false" })
        };
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"【{(skill.Name)}】是否令 {(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 回复1点体力？", [], [frame.OwnerSeat], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("faction-recovery")]
    internal void ResolveFactionRecoveryChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Faction recovery lost its frame.");
        var draft = frame.FactionRecoveryDraft ?? throw new InvalidOperationException("Faction recovery lost its draft.");
        var chooser = draft.ResponderSeats[draft.ResponderIndex];
        var accepted = choice.Parameters.GetValueOrDefault("accept") == "true";
        if (choice.Parameters.GetValueOrDefault("program-action") != "faction-recovery" || choice.Parameters.GetValueOrDefault("accept")is not ("true" or "false") || choice.Cards.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != chooser || accepted && !choice.Targets.SequenceEqual([frame.OwnerSeat]) || !accepted && choice.Targets.Count != 0)
            throw new InvalidOperationException("Invalid faction recovery choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (accepted && _coreMatchParticipantsPlayersAccess._players[chooser].IsAlive && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
        {
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frame.Id, chooser, frame.OwnerSeat, 1, null, null);
            _coreGameEngineDeferredBenefitsProgramsProgramFactionRecoveryDebtsProgramNextTurnRuleModifiersAccess._programFactionRecoveryDebts.Add(new(draft.DyingFrameId, frame.OwnerSeat, chooser, frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.SkillInstanceId));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramFactionRecoveryChoiceEvent(frame.Id, frame.SkillId, frame.OwnerSeat, chooser, accepted, draft.DyingFrameId));
        var next = draft.ResponderIndex + 1;
        while (next < draft.ResponderSeats.Count && !_coreMatchParticipantsPlayersAccess._players[draft.ResponderSeats[next]].IsAlive)
            next++;
        var updated = draft with
        {
            ResponderIndex = next,
            AcceptedSeats = accepted ? draft.AcceptedSeats.Append(chooser).ToArray() : draft.AcceptedSeats
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { FactionRecoveryDraft = next < draft.ResponderSeats.Count ? updated : null });
        if (next < draft.ResponderSeats.Count)
            PublishFactionRecoveryPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.RequestFactionRecovery)]
    internal PromptChoice SelectAiFactionRecovery(PendingDecision decision, ProgramSkillFrame frame)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var self = view.Players.Single(player => player.Seat == decision.PlayerSeat);
        // Public relationship alone determines aid; private hands do not enter the decision.
        var accept = self.Role == Role.Loyalist || self.Role == Role.Renegade && view.Players.Count(player => player.IsAlive) > 2;
        return decision.Choices.Single(choice => choice.Parameters["accept"] == (accept ? "true" : "false"));
    }

    internal SkillProgramStepOutcome BeginProgramWeaponDamageChoice(ProgramSkillFrame frame, int amount)
    {
        var context = frame.WindowContext ?? throw new InvalidOperationException("Weapon damage choice has no damage context.");
        if (context.Window != SkillProgramTriggerWindow.BeforeDamageApplied || context.SourceSeat != frame.OwnerSeat || context.TargetSeat is not { } target || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { IsChainPropagation: false } attack || attack.EffectiveCardKind is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash))
            throw new InvalidOperationException("Weapon damage choice requires the owner's direct Slash damage.");
        var weapon = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Where(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon).OrderByDescending(card => _coreAdvancedLifecycleOperations.GetWeaponAttackRange(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], card)).ThenBy(card => card.Id).FirstOrDefault();
        if (weapon is null)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { WeaponDamageDraft = new(target, weapon.Id, Math.Max(0, _coreAdvancedLifecycleOperations.GetWeaponAttackRange(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], weapon)), []) });
        PublishWeaponDamagePrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), amount);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishWeaponDamagePrompt(ProgramSkillFrame frame, int amount)
    {
        var draft = frame.WeaponDamageDraft!;
        var choices = new List<PromptChoice>();
        if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]).Count >= draft.RequiredCount)
        {
            if (draft.SelectedIds.Count < draft.RequiredCount)
                choices.AddRange(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]).Where(card => !draft.SelectedIds.Contains(card.Id)).Select(card => new PromptChoice(new ChoiceId("weapon-damage.card-" + card.Id), "选择弃置【" + card.DisplayName + "】", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "weapon-damage", ["branch"] = "card" })));
            else
                choices.Add(new(new ChoiceId("weapon-damage.finish"), $"弃置所选{(draft.RequiredCount)}张手牌及其武器", [], [], new Dictionary<string, string> { ["program-action"] = "weapon-damage", ["branch"] = "finish" }));
        }

        choices.Add(new(new ChoiceId("weapon-damage.bonus"), $"令此杀伤害+{(amount)}", [], [], new Dictionary<string, string> { ["program-action"] = "weapon-damage", ["branch"] = "bonus" }));
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, draft.TargetSeat, $"【{(skill.Name)}】弃置{(draft.RequiredCount)}张手牌并弃置其武器，或令此杀伤害+{(amount)}。", choices.SelectMany(choice => choice.Cards).ToArray(), [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices.ToArray(),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("weapon-damage")]
    internal void ResolveWeaponDamageChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Weapon choice lost its frame.");
        var draft = frame.WeaponDamageDraft ?? throw new InvalidOperationException("Weapon choice lost its draft.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var branch = choice.Parameters.GetValueOrDefault("branch");
        if (choice.Parameters.GetValueOrDefault("program-action") != "weapon-damage" || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != draft.TargetSeat || effect.Op != SkillProgramEffectOp.WeaponDiscardOrDamageBonus || choice.Targets.Count != 0 || branch is not ("card" or "finish" or "bonus"))
            throw new InvalidOperationException("Invalid weapon damage choice.");
        if (branch == "card")
        {
            if (choice.Cards.Count != 1 || draft.SelectedIds.Contains(choice.Cards[0]) || draft.SelectedIds.Count >= draft.RequiredCount || _cardZonesOperations._cardZones.GetLocation(choice.Cards[0]) != CardLocation.Hand(draft.TargetSeat))
                throw new InvalidOperationException("Illegal staged weapon discard.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { WeaponDamageDraft = draft with { SelectedIds = draft.SelectedIds.Append(choice.Cards[0]).ToArray() } });
            PublishWeaponDamagePrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), effect.Amount);
            return;
        }

        if (choice.Cards.Count != 0 || branch == "finish" && (draft.SelectedIds.Count != draft.RequiredCount || draft.SelectedIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(draft.TargetSeat)) || _cardZonesOperations._cardZones.GetLocation(draft.WeaponCardId) != CardLocation.Equipment(frame.OwnerSeat)))
            throw new InvalidOperationException("Weapon discard costs changed before commitment.");
        var context = frame.WindowContext!;
        if (_resolutionStackOperations._resolutionStack[^2] is not BeforeDamageProgramWindowFrame window || window.Id != context.ParentFrameId || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || attack.SourceSeat != frame.OwnerSeat || attack.TargetSeat != draft.TargetSeat)
            throw new InvalidOperationException("Weapon choice lost its active damage.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { WeaponDamageDraft = null });
        var after = window.Amount;
        if (branch == "bonus")
        {
            attack.IncreaseFinalizedDamageAmount(effect.Amount);
            after = checked(window.Amount + effect.Amount);
            _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[^2].Id, window with { Amount = after });
        }
        else
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, draft.TargetSeat, draft.SelectedIds.Append(draft.WeaponCardId).ToArray(), new CardMoveReason("skill-program.weapon-discard-cost"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramWeaponDamageChoiceEvent(frame.Id, frame.SkillId, frame.OwnerSeat, draft.TargetSeat, draft.WeaponCardId, branch == "finish", branch == "finish" ? draft.SelectedIds : [], window.Amount, after));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
