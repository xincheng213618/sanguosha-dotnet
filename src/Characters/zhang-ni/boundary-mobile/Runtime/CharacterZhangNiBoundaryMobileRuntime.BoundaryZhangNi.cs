#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangNiBoundaryMobileRuntime
{
    internal const string BoundaryWurongGift = "skill-program.boundary-zhang-ni.wurong.gift";
    internal static CardConversionSource BoundaryWurongSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IBoundaryZhangNiHost.Resolve(ProgramSkillFrame f, SkillProgramEffect e) => e.Op == SkillProgramEffectOp.ConsumeBoundaryZhangNiDrawSkip ? ConsumeBoundaryWurongSkip(f) : BeginBoundaryWurong(f, e);
    internal SkillProgramStepOutcome BeginBoundaryWurong(ProgramSkillFrame supplied, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (e.StateId == "gifts")
            return BeginBoundaryWurongGift(f);
        if (f.BoundaryWurong is not null || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.ActivationId != BoundaryZhangNiComposition.Activation || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat)
            throw new InvalidOperationException("Wurong requires one other living role in the original own actual Play activation.");
        var r = new BoundaryWurongReceipt(BoundaryWurongSource(f), f.GameplayHash, phase, target);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWurong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWurongStartedEvent(f.Id, r.Source, r.Hash, phase, target));
        PublishBoundaryWurong(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BoundaryWurongChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryWurong!;
        PromptChoice Choice(string branch, string text, IReadOnlyList<int>? cards = null) => new(new($"boundary-wurong.{(f.Id)}.{(r.Stage)}.{(branch)}.{(string.Join('-', cards ?? []))}"), text, cards ?? [], [], new Dictionary<string, string> { ["program-action"] = "boundary-wurong", ["branch"] = branch });
        if (r.Stage == BoundaryWurongStage.OwnerChoice)
            return[Choice("suppress", "镇压"), Choice("appease", "安抚")];
        if (r.Stage == BoundaryWurongStage.TargetChoice)
            return[Choice("resist", "反抗"), Choice("submit", "归顺")];
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Giver]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[r.Giver])).ToArray();
        var choices = new List<PromptChoice>();
        foreach (var card in cards.Where(c => r.Selected.Contains(c.Id)))
            choices.Add(Choice("unpick", "取消交出【" + card.DisplayName + "】", [card.Id]));
        if (r.Selected.Count < r.Required)
            foreach (var card in cards.Where(c => !r.Selected.Contains(c.Id)))
                choices.Add(Choice("card", "交出【" + card.DisplayName + "】", [card.Id]));
        if (r.Selected.Count == r.Required)
            choices.Add(Choice("pay", $"同时交给{(_coreMatchParticipantsPlayersAccess._players[r.Recipient].Name)}{(r.Required)}张牌"));
        return choices;
    }

    internal void PublishBoundaryWurong(ProgramSkillFrame f)
    {
        var r = f.BoundaryWurong!;
        var chooser = r.Stage == BoundaryWurongStage.OwnerChoice ? f.OwnerSeat : r.Stage == BoundaryWurongStage.TargetChoice ? r.Target : r.Giver;
        var choices = BoundaryWurongChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var prompt = new PendingDecision(DecisionKind.ProgramTrigger, chooser, r.Stage == BoundaryWurongStage.GiftChoice ? $"怃戎：选择{(r.Required)}张手牌或装备牌，确认后同时交出。" : "怃戎：秘密选择谋弈策略，双方提交后共同揭晓。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = r.Stage == BoundaryWurongStage.GiftChoice ? _corePlayerViewNativeCardSelectionsOperations.NativeOwnedSelection(prompt, f, "wurong-gift", "branch", ["card"], ["pay"], r.Selected, r.Required - r.Selected.Count, r.Required - r.Selected.Count) : prompt;
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("boundary-wurong")]
    internal void AnswerBoundaryWurong(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Wurong lost its original private commitment frame.");
        var r = f.BoundaryWurong!;
        if (!BoundaryWurongChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets)))
            throw new InvalidOperationException("Wurong rejects a stale strategy or payment entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (r.Stage == BoundaryWurongStage.OwnerChoice)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Suppress = branch == "suppress", Stage = BoundaryWurongStage.TargetChoice } });
            // Only the existence of a commitment is public before reveal.
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWurongCommittedEvent(f.Id, f.OwnerSeat, true));
            PublishBoundaryWurong(f);
            return;
        }

        if (r.Stage == BoundaryWurongStage.TargetChoice)
        {
            var resist = branch == "resist";
            var outcome = (r.Suppress!.Value ? "suppress" : "appease") + (resist ? "-resist" : "-submit");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Resist = resist, Outcome = outcome, Stage = BoundaryWurongStage.Revealed }, ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding(BoundaryZhangNiComposition.Result, outcome, f.OwnerSeat)).ToArray()) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWurongCommittedEvent(f.Id, r.Target, false));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWurongRevealedEvent(f.Id, r.Suppress.Value, resist, outcome));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        switch (branch)
        {
            case "card":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Selected = [..r.Selected, choice.Cards.Single()] } });
                PublishBoundaryWurong(f);
                return;
            case "unpick":
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Selected = r.Selected.Where(id => id != choice.Cards.Single()).ToArray() } });
                PublishBoundaryWurong(f);
                return;
            case "pay":
                PayBoundaryWurongGift(f);
                return;
            default:
                throw new InvalidOperationException("Unknown Wurong private commitment.");
        }
    }

    internal SkillProgramStepOutcome BeginBoundaryWurongGift(ProgramSkillFrame f)
    {
        if (f.BoundaryWurong is not { Stage: BoundaryWurongStage.Revealed } r || f.InstructionIndex != 5)
            throw new InvalidOperationException("Wurong gifts require the original revealed consequence.");
        if (r.Resist == true || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.Complete } });
            return SkillProgramStepOutcome.Continue;
        }

        var giver = r.Suppress == true ? f.OwnerSeat : r.Target;
        var recipient = giver == f.OwnerSeat ? r.Target : f.OwnerSeat;
        var available = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[giver]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[giver]).Count;
        if (r.Suppress == false && available < 2)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWurongSkipIssuedEvent(f.Id, r.Source, r.Hash, r.Target));
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.Complete } });
            return SkillProgramStepOutcome.Continue;
        }

        if (available == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.Complete } });
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.GiftChoice, Giver = giver, Recipient = recipient, Required = Math.Min(2, available) } });
        PublishBoundaryWurong(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PayBoundaryWurongGift(ProgramSkillFrame f)
    {
        var r = f.BoundaryWurong!;
        if (r.Stage != BoundaryWurongStage.GiftChoice || r.Selected.Count != r.Required || r.Selected.Any(id => _cardZonesOperations._cardZones.GetLocation(id)is not { OwnerSeat: { } holder, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } || holder != r.Giver))
            throw new InvalidOperationException("Wurong must pay its complete current owned HE set once.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWurong = r = r with { Stage = BoundaryWurongStage.GiftChildren, Before = before, After = before }, PendingMovementContinuation = new(r.Giver, 0, null) });
        foreach (var group in r.Selected.GroupBy(id => _cardZonesOperations._cardZones.GetLocation(id)))
            _corePhysicalMovementFacadesOperations.MoveCards(group.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray(), group.Key, CardLocation.Hand(r.Recipient), new(BoundaryWurongGift));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var movement in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && r.Selected.Contains(m.CardId) && m.From.OwnerSeat == r.Giver && m.To == CardLocation.Hand(r.Recipient) && m.Reason.Value == BoundaryWurongGift).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWurongGiftEntityEvent(f.Id, movement.CardId, movement.From, movement.To, movement.Sequence));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWurongGiftPaidEvent(f.Id, r.Giver, r.Recipient, r.Selected.Count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueBoundaryWurongGift(f.Id);
    }

    internal void ContinueBoundaryWurongGift(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryWurong: { Stage: BoundaryWurongStage.GiftChildren } r } f || f.Id != id)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.Complete }, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 54200)]
    internal bool ResumeBoundaryWurong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryWurong.Stage: BoundaryWurongStage.GiftChildren } f || f.Id != id)
            return false;
        ContinueBoundaryWurongGift(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 54200)]
    internal bool ReturnBoundaryWurongMovement(ProgramSkillFrame f)
    {
        if (f.BoundaryWurong?.Stage != BoundaryWurongStage.GiftChildren)
            return false;
        ContinueBoundaryWurongGift(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 54200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 54200)]
    internal bool KeepBoundaryWurongPaid(ProgramSkillFrame f) => f.BoundaryWurong is { Stage: >= BoundaryWurongStage.Revealed } || f.TriggerId == BoundaryZhangNiComposition.Skip && f.InstructionIndex is 0 or 1 && f.WindowContext is { } context && AdmitBoundaryWurongSkip(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 1000, context.OccurrenceIndex), context);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryZhangNiWurong)]
    internal static PromptChoice BoundaryWurongAi(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pay") ?? p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "card") ?? p.Choices.First();
    internal BoundaryWurongSkipIssuedEvent[] PendingBoundaryWurongSkips(int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWurongSkipIssuedEvent>().Where(e => e.Target == target && _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(e.Source.SkillId)?.Program?.GameplayHash == e.Hash && !_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWurongSkipConsumedEvent>().Any(c => c.IssuerFrameId == e.FrameId)).ToArray();
    internal bool HasPendingBoundaryWurongDrawSkip(int target) => PendingBoundaryWurongSkips(target).Length > 0;
    internal static string BoundaryWurongSkipInstance(BoundaryWurongSkipIssuedEvent e) => "issued:boundary-wurong-skip:" + e.FrameId;
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 54200)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeBoundaryWurongSkip(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> original)
    {
        if (window != SkillProgramTriggerWindow.DrawPhaseStarting || !_contentRegistryOperations._contentRegistry.Skills.ContainsKey(BoundaryZhangNiComposition.Skill))
            return original;
        var rest = original.Where(c => c.SkillId != BoundaryZhangNiComposition.Skill || c.BindingId != BoundaryZhangNiComposition.Skip).ToArray();
        var debt = PendingBoundaryWurongSkips(owner.Seat).FirstOrDefault();
        return debt is null ? rest : [..rest, new ProgramTriggerCandidate(owner.Seat, BoundaryZhangNiComposition.Skill, BoundaryZhangNiComposition.Skip, BoundaryWurongSkipInstance(debt), debt.Hash, 1000)];
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 54200)]
    internal bool AdmitBoundaryWurongSkip(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != BoundaryZhangNiComposition.Skill || candidate.BindingId != BoundaryZhangNiComposition.Skip || context.Window != SkillProgramTriggerWindow.DrawPhaseStarting || context.OwnerSeat != candidate.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DrawPhaseStarting } parent || parent.OwnerSeat != candidate.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != candidate)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWurongSkipIssuedEvent>().Any(e => e.Target == candidate.OwnerSeat && e.Hash == candidate.GameplayHash && BoundaryWurongSkipInstance(e) == candidate.SkillInstanceId && !_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryWurongSkipConsumedEvent>().Any(c => c.IssuerFrameId == e.FrameId && c.DrawWindowId != parent.Id));
    }

    internal SkillProgramStepOutcome ConsumeBoundaryWurongSkip(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } context || !AdmitBoundaryWurongSkip(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 1000), context))
            throw new InvalidOperationException("Wurong skip requires its issued next actual Draw window.");
        foreach (var debt in PendingBoundaryWurongSkips(f.OwnerSeat))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryWurongSkipConsumedEvent(debt.FrameId, f.Id, context.ParentFrameId, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber));
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(p => p.Id == context.ParentFrameId);
        if (parent.ActualPhaseSubstitution is null)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { ActualPhaseSubstitution = new(parent.Id, f.Id, f.OwnerSeat, f.SkillId, f.SkillInstanceId, _coreActualTurnProgressTurnNumberAccess._turnNumber, SkillProgramTurnPhase.Draw) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(f.Id, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, context.Window, SkillProgramTurnPhase.Draw));
        }

        return _coreProgramTurnPhaseOperations.SkipProgramTurnPhases(f, [SkillProgramTurnPhase.Draw]);
    }
}
