#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 160)]
    internal bool CompletedUndamagedFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => TracksCompletedUndamagedUseDamage && IsCompletedUndamagedDamageUse(query.Kind) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(query.Kind);
    internal const string CompletedUndamagedRevealBind = "completed-undamaged-target-reveal";
    internal long CompletedUndamagedMovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
    internal bool TracksCompletedUndamagedUseDamage => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMiuyanCompletedUse) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RevealUndamagedUseTargetHandAndDiscardSameColor) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InvalidateRemainingActualDamageCardEffects) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ClaimCompletedUndamagedIncomingCardEntities) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RequestFulueZeroDamageCounterSlash);

    internal static bool IsCompletedUndamagedDamageUse(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CompletedUndamagedTargetReveal.IsCompletedUndamagedDamageUse(kind);
    internal static string CompletedUndamagedDiscardReason(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CompletedUndamagedTargetReveal.CompletedUndamagedDiscardReason(f);
    internal bool HasExactCompletedUndamagedPublicPileCost(CardUseFrame use, CardActionContext action)
    {
        // Xiansi pays two public Authority entities to issue a zero-material
        // Slash. Those activation costs are recorded on Action, but they are
        // deliberately absent from the native Slash's physical materials.
        if (!_coreForeignPublicPileSlashesOperations.IsForeignPublicPileSlashUse(use.Id) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || action.PhysicalCards is not [var first, var second] || first.CardId <= 0 || second.CardId <= 0 || first.CardId == second.CardId || first.From != second.From || first.From is not { Zone: CardZoneKind.Authority, OwnerSeat: { } owner } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || owner == action.ProviderSeat || action.RequesterSeat is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || action.ParentActionId is not null)
            return false;
        var sources = action.ConversionChain.Where(s => s.OwnerSeat == owner && !string.IsNullOrWhiteSpace(s.SkillInstanceId) && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(s.SkillId)?.Program?.CardPolicies.Any(p => p.Id == s.BindingId && p.Kind == SkillProgramCardPolicyKind.ForeignPublicPileSlash) == true).ToArray();
        return sources.Length == 1 && action.PhysicalCards.All(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == c.CardId && m.CardKind == c.CardKind && m.From == c.From && m.To == CardLocation.DiscardPile && m.Reason.Value == "program.public-pile.slash-payment"));
    }

    internal bool HasExactAcceptedCompletedUndamagedUse(CardUseFrame use, CardActionContext action)
    {
        if (!_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action))
            return false;
        if (HasExactCompletedUndamagedPublicPileCost(use, action))
            return true;
        if (use.PhysicalCardIds is not { } physical || !physical.SequenceEqual(action.PhysicalCards.Select(c => c.CardId)) || physical.Distinct().Count() != physical.Count || physical.Any(id => id <= 0) || (physical.Count == 0 ? use.CardId != 0 : !physical.Contains(use.CardId)))
            return false;
        // This ledger proves which accepted native Use caused the damage.
        // Payment remains the responsibility of that Use's original producer:
        // legal top-deck, already-revealed and equipment conversions need not
        // share the ordinary hand-to-Processing movement reason.
        return true;
    }

    internal bool HasExactCompletedUndamagedTunanVirtualAttack(CardUseFrame use, CardActionContext action, global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        // Tunan retains its one publicly revealed input on CardUse/Action, but
        // its converted native Slash intentionally has no physical materials.
        // Only that exact suspended producer can explain the difference.
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || attack.PhysicalCards.Count != 0 || attack.ProgramSkillCardUseFrameId is not { } parentId || use.PhysicalCardIds is not [var cardId] || use.CardId != cardId || action.PhysicalCards is not [var cost] || cost.CardId != cardId || cost.From != CardLocation.Processing)
            return false;
        var useIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (useIndex < 1 || _resolutionStackOperations._resolutionStack[useIndex - 1] is not ProgramSkillFrame parent || parent.Id != parentId || parent.SelectedTargetSeats is not [var user] || user == parent.OwnerSeat || action.ProviderSeat != user || parent.WindowContext?.Window != SkillProgramTriggerWindow.PlayPhaseStarting || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } program || program.GameplayHash != parent.GameplayHash)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(parent, program);
        if (parent.InstructionIndex < 1 || parent.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(parent.InstructionIndex).Effect is not { Op: SkillProgramEffectOp.TunanUseRevealedCard, SourceBind: { } bind } || parent.CardSetBindings.SingleOrDefault(b => b.Name == bind)is not { Visibility: SkillProgramCardSetVisibility.Public, CardIds: [var boundCard] } || boundCard != cardId || !action.ConversionChain.Contains(new(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), user, parent.SkillInstanceId)))
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == parent.Id && e.SkillId == parent.SkillId && e.BindingId == global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent) && e.SkillInstanceId == parent.SkillInstanceId && e.OwnerSeat == parent.OwnerSeat && e.Window == SkillProgramTriggerWindow.PlayPhaseStarting) == 1 && history.OfType<ProgramCardsRevealedEvent>().Where(e => e.FrameId == parent.Id && e.SkillId == parent.SkillId && e.BindingId == global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent) && e.OwnerSeat == parent.OwnerSeat && e.Bind == bind).ToArray()is [{ Cards: [var revealed] }] && revealed.Id == cardId && revealed.Kind == cost.CardKind && history.OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == cardId && e.CardKind == CardKind.Slash && e.SourceSeat == user) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cardId && m.CardKind == cost.CardKind && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == $"skill-program.{(parent.SkillId)}.{(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent))}.reveal");
    }

    internal bool CompletedUndamagedNativeUseActorMatches(CardUseFrame use, global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (use.SourceSeat == attack.CardUserSeat)
            return true;
        // Huoshou changes the native global attack's damage source, while the
        // same CardUse still belongs to its original actor. Prove that producer
        // directly; a damage child beneath an unrelated Use is not sufficient.
        return use.CardKind == CardKind.BarbarianAssault && _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } group && group.ResolutionId == use.Id && group.SourceSeat == use.SourceSeat && group.Effect == GroupCardEffect.ResponseAttack && group.RequiredCardKind == CardKind.Slash && group.Card.Kind == use.CardKind && group.DamageSourceSeat == attack.SourceSeat && attack.CardUserSeat == group.DamageSourceSeat && group.TargetSeats.SequenceEqual(use.TargetSeats) && group.CurrentAttack is { } current && current.ResolutionId == attack.ResolutionId && current.SourceSeat == attack.SourceSeat && current.TargetSeat == attack.TargetSeat && current.EffectiveCardKind == use.CardKind && group.PhysicalCards.Select(c => c.Id).SequenceEqual(attack.PhysicalCards.Select(c => c.Id)) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HuoshouAttributedEvent>().Where(e => e.ResolutionId == use.Id).ToArray()is [var attributed] && attributed.CardUserSeat == use.SourceSeat && attributed.DamageSourceSeat == attack.SourceSeat;
    }

    internal void RecordCompletedUndamagedTargetDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, int amount)
    {
        if (!TracksCompletedUndamagedUseDamage || amount <= 0 || attack is not global::CardGame.Core.NativeOwners.CardAttackHandle card || card.IsDelayedJudgmentDamage || card.IsProgramJudgmentDamage || card.IsProgramSkillDamage || !card.DamageWasApplied || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.Id != damageFrameId || damage.ParentFrameId != card.ResolutionId || damage.SourceSeat != card.SourceSeat || damage.TargetSeat != card.TargetSeat || damage.Amount != amount || damage.Nature != _coreNativeCardClassificationsOperations.GetDamageNature(card) || _coreCardUseLifecycleOperations.LifecycleCardUse(card.ResolutionId)is not { } use || !CompletedUndamagedNativeUseActorMatches(use, card) || use.CardKind != card.EffectiveCardKind || !IsCompletedUndamagedDamageUse(use.CardKind))
            return;
        if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageRequestedEvent>().Any(e => e.ResolutionId == damageFrameId && e.SourceSeat == damage.SourceSeat && e.TargetSeat == damage.TargetSeat && e.Amount == amount && e.SourceCard == use.CardKind && e.Nature == damage.Nature && e.SourceLess == card.IsSourceLess))
            throw new InvalidOperationException("Whole-use damage lost its native declared attack or real requested damage.");
        if (use.Action is { } action)
        {
            if (action.Type != CardActionType.Use || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || !HasExactAcceptedCompletedUndamagedUse(use, action) || !card.PhysicalCards.Select(c => c.Id).SequenceEqual(use.PhysicalCardIds!) && !HasExactCompletedUndamagedTunanVirtualAttack(use, action, card) || card.PhysicalCards.Any(c => !action.PhysicalCards.Any(cost => cost.CardId == c.Id && cost.CardKind == c.Kind)))
                throw new InvalidOperationException($"Whole-use damage cannot borrow an unaccepted action identity. " + $"Use={(use.Id)}; actor={(action.ActorSeat)}/{(use.SourceSeat)}; kind={(action.EffectiveKind)}/{(use.CardKind)}; " + $"accepted={(HasExactAcceptedCompletedUndamagedUse(use, action))}; " + $"nativeMaterials={(card.PhysicalCards.Select(c => c.Id).SequenceEqual(use.PhysicalCardIds!))}; " + $"tunan={(HasExactCompletedUndamagedTunanVirtualAttack(use, action, card))}; " + $"declaredMaterials={(card.PhysicalCards.All(c => action.PhysicalCards.Any(cost => cost.CardId == c.Id && cost.CardKind == c.Kind)))}.");
        }
        else if (use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use) || !_coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, use.SourceSeat, use.SourceSeat))
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CompletedUndamagedUseDamageRecordedEvent>().Any(e => e.DamageFrameId == damageFrameId))
            throw new InvalidOperationException("A real native damage application cannot be recorded twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CompletedUndamagedUseDamageRecordedEvent(use.Id, use.Action?.ActionId, damageFrameId, use.SourceSeat, card.SourceSeat, card.TargetSeat, use.CardKind, amount, damage.Nature, card.IsSourceLess, card.IsChainPropagation, card.DamageRedirected));
    }

    internal CardUseFrame? CompletedUndamagedUse(ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } card })
            return null;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == card.CardActionId);
        if (use is not { Step: ResolutionFrameStep.Completed, Action: { Type: CardActionType.Use } action } || use.SourceSeat != action.ActorSeat || card.ActorSeat != action.ActorSeat || card.EffectiveKind != use.CardKind || action.EffectiveKind != use.CardKind || !IsCompletedUndamagedDamageUse(use.CardKind) || use.TargetSeats.Count == 0 || use.TargetSeats.Distinct().Count() != use.TargetSeats.Count || use.TargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || context.ParentFrameId > 0 && card.ParentCardUseFrameId != use.Id || !HasExactAcceptedCompletedUndamagedUse(use, action) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == use.CardId && e.CardKind == use.CardKind) != 1)
            return null;
        return use;
    }

    internal int[] CompletedUndamagedEligibleTargets(CardUseFrame use) => use.TargetSeats.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CompletedUndamagedUseDamageRecordedEvent>().Any(e => e.CardUseFrameId == use.Id && e.TargetSeat == seat && e.Amount > 0)).ToArray();
    internal bool CompletedUndamagedSourceUseQualifies(string skillId, CardUseFrame use) => skillId != CompletedUndamagedTargetRevealComposition.ShanheSourceYiduSkillId || _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null && use.TargetSeats.Count == 1 && use.Action is { } action && action.EffectiveDesignatedTargetSeats.Distinct().Count() == 1;
    internal static int CompletedUndamagedDiscardActor(ProgramSkillFrame frame, int target) => frame.SkillId == CompletedUndamagedTargetRevealComposition.ShanheSourceYiduSkillId ? target : frame.OwnerSeat;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1060)]
    internal bool CanOfferCompletedUndamagedTargetReveal(ProgramTriggerLegalityContext gate)
    {
        var trigger = gate.Trigger;
        if (!trigger.Effects.Any(e => CompletedUndamagedTargetRevealComposition.IsOperation(e.Op)))
            return true;
        var owner = gate.OwnerSeat;
        return _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchOutcomeStatusWinnerAccess._status != EngineStatus.Completed && _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && CompletedUndamagedUse(gate.Context)is { } use && use.SourceSeat == owner && CompletedUndamagedSourceUseQualifies(gate.SkillId, use) && CompletedUndamagedEligibleTargets(use).Length > 0;
    }

    internal SkillProgramStepOutcome BeginCompletedUndamagedTargetReveal(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.CompletedUndamagedTargetReveal is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || CompletedUndamagedUse(context)is not { Action: { } action } use || use.SourceSeat != f.OwnerSeat || !_coreCompletedUndamagedTargetRevealObserversOperations.ExactCompletedUndamagedParent(f, context, use) || !CompletedUndamagedSourceUseQualifies(f.SkillId, use))
            throw new InvalidOperationException("Completed-target reveal lost its original completed Use and current optional candidate.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var targets = CompletedUndamagedEligibleTargets(use);
        if (targets.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var r = new ProgramCompletedUndamagedTargetRevealReceipt
        {
            InstructionIndex = f.InstructionIndex,
            Source = new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId),
            GameplayHash = f.GameplayHash,
            WindowFrameId = context.ParentFrameId,
            CardUseFrameId = use.Id,
            CardActionId = action.ActionId,
            EffectiveKind = use.CardKind,
            FinalTargets = use.TargetSeats,
            EligibleTargets = targets,
            Stage = CompletedUndamagedTargetRevealStage.ChoosingTarget
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CompletedUndamagedTargetReveal = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CompletedUndamagedTargetRevealStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowFrameId, r.CardUseFrameId, r.CardActionId));
        PublishCompletedUndamagedTargetRevealChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> CompletedUndamagedTargetRevealChoices(ProgramSkillFrame f)
    {
        var r = f.CompletedUndamagedTargetReveal!;
        Dictionary<string, string> Parameters(string branch) => new()
        {
            ["program-action"] = CompletedUndamagedRevealBind,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        if (r.Stage == CompletedUndamagedTargetRevealStage.ChoosingTarget)
            return Array.AsReadOnly(r.EligibleTargets.Select(seat => new PromptChoice(new($"completed-undamaged.{(f.Id)}.target.{(seat)}"), $"展示 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 的至多三张手牌。", [], [seat], Parameters("target"))).ToArray());
        var selected = r.SelectedSlots.ToHashSet();
        var choices = r.TargetHand.Where(m => !selected.Contains(m.Slot)).Select(m =>
        {
            var p = Parameters("slot");
            p["slot-index"] = m.Slot.ToString(CultureInfo.InvariantCulture);
            return new PromptChoice(new($"completed-undamaged.{(f.Id)}.{(selected.Count)}.slot.{(m.Slot)}"), $"选择第 {(m.Slot + 1)} 个暗置手牌牌位（已选 {(selected.Count)} 张）。", [], [r.TargetSeat!.Value], p);
        }).ToList();
        choices.Add(new(new($"completed-undamaged.{(f.Id)}.{(selected.Count)}.finish"), $"选毕，展示已选的 {(selected.Count)} 张手牌。", [], [r.TargetSeat!.Value], Parameters("finish")));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishCompletedUndamagedTargetRevealChoice(ProgramSkillFrame f)
    {
        var r = f.CompletedUndamagedTargetReveal!;
        var choices = CompletedUndamagedTargetRevealChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage == CompletedUndamagedTargetRevealStage.ChoosingTarget ? "选择本次使用未受到此牌伤害的一名存活目标。" : "盲选其至多三张手牌；选毕后公开所选牌。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("completed-undamaged-target-reveal")]
    internal void ResolveCompletedUndamagedTargetRevealChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Completed-target reveal lost its chooser.");
        var r = f.CompletedUndamagedTargetReveal!;
        if (r.Stage is not (CompletedUndamagedTargetRevealStage.ChoosingTarget or CompletedUndamagedTargetRevealStage.ChoosingCards) || !CompletedUndamagedTargetRevealChoices(f).Any(c => CompletedUndamagedChoiceEquals(c, choice)))
            throw new InvalidOperationException("Completed-target reveal changed its exact frozen target or opaque slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "遗毒在发行展示前失去原技能资格或发动者。");
            return;
        }

        if (r.Stage == CompletedUndamagedTargetRevealStage.ChoosingTarget)
        {
            var seat = choice.Targets.Single();
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            {
                _coreSkillProgramsOperations.FinishProgramSkill(f, true);
                return;
            }

            r = r with
            {
                TargetSeat = seat,
                TargetHand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select((c, slot) => new CompletedUndamagedHandMaterial(c.Id, c.Kind, slot)).ToArray(),
                Stage = CompletedUndamagedTargetRevealStage.ChoosingCards
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CompletedUndamagedTargetReveal = r });
            PublishCompletedUndamagedTargetRevealChoice(f);
            return;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat!.Value].IsAlive)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        if (choice.Parameters["branch"] == "slot")
        {
            var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
            r = r with
            {
                SelectedSlots = r.SelectedSlots.Append(slot).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CompletedUndamagedTargetReveal = r });
            if (r.SelectedSlots.Count < Math.Min(3, r.TargetHand.Count))
            {
                PublishCompletedUndamagedTargetRevealChoice(f);
                return;
            }
        }

        IssueCompletedUndamagedTargetReveal(f);
    }

    internal static bool CompletedUndamagedChoiceEquals(PromptChoice a, PromptChoice b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CompletedUndamagedTargetReveal.CompletedUndamagedChoiceEquals(a, b);
    internal static CardColor? CompletedUndamagedColor(Suit suit) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CompletedUndamagedTargetReveal.CompletedUndamagedColor(suit);
    internal void IssueCompletedUndamagedTargetReveal(ProgramSkillFrame f)
    {
        var r = f.CompletedUndamagedTargetReveal!;
        var target = r.TargetSeat!.Value;
        var from = CardLocation.Hand(target);
        var hand = _cardZonesOperations._cardZones.CardsAt(from);
        var selected = r.SelectedSlots.Select(slot => r.TargetHand.Single(m => m.Slot == slot)).ToArray();
        var cards = selected.Select(m => hand.SingleOrDefault(c => c.Id == m.CardId && c.Kind == m.PrintedKind)).Where(c => c is not null).Select(c => c!).ToArray();
        var revealed = cards.Select(c =>
        {
            var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[target], c);
            return new CompletedUndamagedRevealedMaterial(c.Id, c.Kind, selected.Single(m => m.CardId == c.Id).Slot, suit, CompletedUndamagedColor(suit));
        }).ToArray();
        var same = revealed.Length > 0 && revealed.All(m => m.EffectiveColor == revealed[0].EffectiveColor);
        // Source3113 says the target discards its own cards. OL Yidu retains
        // its original effect-owner discard; qualification uses the real actor.
        var actor = CompletedUndamagedDiscardActor(f, target);
        var paid = same ? revealed.Where(m => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, hand.Single(c => c.Id == m.CardId), from, OwnedCardMoveIntent.Discard)).ToArray() : [];
        var before = CompletedUndamagedMovementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CompletedUndamagedTargetReveal = r with { SelectionIssued = true, RevealedMaterials = revealed, SameColor = same, PaidMaterials = paid, SequenceBefore = before, SequenceAfter = before, Stage = paid.Length > 0 ? CompletedUndamagedTargetRevealStage.MovementChildren : CompletedUndamagedTargetRevealStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CompletedUndamagedTargetRevealSelectionIssuedEvent(f.Id, target, selected.Length, revealed.Length, same));
        if (cards.Length > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(f.Id, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, CompletedUndamagedRevealBind, Array.AsReadOnly(cards.Select(c => _coreSnapshotValueProjectionOperations.ToSnapshot(c)with { Suit = revealed.Single(m => m.CardId == c.Id).EffectiveSuit }).ToArray())));
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "completed-target-hand-reveal"))
            return;
        ContinueCompletedTargetPublicReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void CompleteCompletedUndamagedTargetReveal(ProgramSkillFrame f)
    {
        var r = f.CompletedUndamagedTargetReveal!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CompletedUndamagedTargetReveal = r with { Stage = CompletedUndamagedTargetRevealStage.Complete }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CompletedUndamagedTargetRevealCompletedEvent(f.Id, r.SelectedSlots.Count, r.PaidMaterials.Count));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 180)]
    internal bool ResumeCompletedUndamagedTargetReveal(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.CompletedUndamagedTargetReveal is not { } r)
            return false;
        if (r.Stage is CompletedUndamagedTargetRevealStage.ChoosingTarget or CompletedUndamagedTargetRevealStage.ChoosingCards)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishCompletedUndamagedTargetRevealChoice(f);
            return true;
        }

        if (r.Stage == CompletedUndamagedTargetRevealStage.MovementChildren && (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id)))
            return true;
        CompleteCompletedUndamagedTargetReveal(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 160)]
    internal bool ReturnCompletedUndamagedTargetRevealMovement(ProgramSkillFrame f)
    {
        if (f.CompletedUndamagedTargetReveal is not { MovementIssued: true, Stage: CompletedUndamagedTargetRevealStage.MovementChildren })
            return false;
        if (f.PendingMovementContinuation is null)
            throw new InvalidOperationException("Completed-target reveal lost its native child return.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 540)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 560)]
    internal bool CanContinueCompletedUndamagedTargetReveal(ProgramSkillFrame f) => f.CompletedUndamagedTargetReveal is { SelectionIssued: true } && _coreCompletedUndamagedTargetRevealObserversOperations.ValidCompletedUndamagedTargetRevealReceipt(f);
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 150)]
    internal bool IsCompletedUndamagedTargetRevealMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.RevealUndamagedUseTargetHandAndDiscardSameColor && f.CompletedUndamagedTargetReveal is { MovementIssued: true, Stage: CompletedUndamagedTargetRevealStage.MovementChildren } r && pending.SubjectSeat == r.TargetSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && f.PendingMovementContinuation == pending && _coreCompletedUndamagedTargetRevealObserversOperations.ValidCompletedUndamagedTargetRevealReceipt(f);
    SkillProgramStepOutcome global::CardGame.Core.ICompletedUndamagedTargetRevealProgramHost.CompletedUndamagedTargetReveal(ProgramSkillFrame f) => BeginCompletedUndamagedTargetReveal(f);
    [PublicHandRevealReturn("completed-target-hand-reveal")]
    internal void ContinueCompletedTargetPublicReveal(ProgramSkillFrame f)
    {
        var r = f.CompletedUndamagedTargetReveal ?? throw new InvalidOperationException("Completed-target reveal lost its original native roster.");
        var target = r.TargetSeat!.Value;
        var from = CardLocation.Hand(target);
        var actor = CompletedUndamagedDiscardActor(f, target);
        var paid = r.PaidMaterials.Where(m => _cardZonesOperations._cardZones.GetLocation(m.CardId) == from && (f.SkillId != CompletedUndamagedTargetRevealComposition.ShanheSourceYiduSkillId || !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId), from, OwnedCardMoveIntent.Discard))).ToArray();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CompletedUndamagedTargetReveal = r with { PaidMaterials = paid, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = paid.Length > 0 ? new(target, 0, null) : null });
        if (paid.Length == 0)
        {
            CompleteCompletedUndamagedTargetReveal(f);
            return;
        }

        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, actor, paid.Select(m => m.CardId).ToArray(), new(CompletedUndamagedDiscardReason(f)), (batch, records) =>
        {
            foreach (var movement in records)
                _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, actor, f.Id);
            if (records.Count != paid.Length || !records.Select(m => m.CardId).SequenceEqual(paid.Select(m => m.CardId)) || records.Any(m => m.From != from || m.To != CardLocation.DiscardPile || paid.Single(c => c.CardId == m.CardId).PrintedKind != m.CardKind))
                throw new InvalidOperationException("Completed-target reveal did not discard its exact revealed original Hand batch.");
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { CompletedUndamagedTargetReveal = current.CompletedUndamagedTargetReveal!with { MovementIssued = true, SequenceAfter = records[^1].Sequence, BatchId = batch } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CompletedUndamagedTargetRevealMovementIssuedEvent(f.Id, target, records.Count, before, records[^1].Sequence, batch));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
}
