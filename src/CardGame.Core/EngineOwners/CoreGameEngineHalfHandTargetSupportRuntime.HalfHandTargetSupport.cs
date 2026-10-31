#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHalfHandTargetSupportRuntime
{
    internal bool IsHalfHandImplicitSelfTarget(CardUseFrame use, int target) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferHalfHandRecipientSupport) && use.SourceSeat == target && use.CardKind == CardKind.DrawTwo && use.CardId > 0 && use.TargetSeats.Count == 0 && use.PhysicalCardIds is { Count: > 0 } ids && use.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.DrawTwo } action && action.ActorSeat == target && action.TargetSeats.Count == 0 && action.PhysicalCards.Select(c => c.CardId).SequenceEqual(ids) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id && e.CardId == use.CardId && e.SourceSeat == target && e.CardKind == CardKind.DrawTwo) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().Any(e => e.ResolutionId == use.Id && e.TargetSeats.Count == 0);
    internal IReadOnlyList<int> HalfHandActualTrickTargets(CardUseFrame use, IReadOnlyList<int> targets) => targets.Count == 0 && IsHalfHandImplicitSelfTarget(use, use.SourceSeat) && HasHalfHandSelfTargetSupport(use.SourceSeat) ? [use.SourceSeat] : targets;
    internal bool HasHalfHandSelfTargetSupport(int target) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferHalfHandRecipientSupport) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[target]).Any(p => p.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.OfferHalfHandRecipientSupport && _coreHalfHandPhaseDebtProgramsOperations.CurrentHalfHandSupport(target, p.Id, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[target], p.Id), p.GameplayHash, e.StateId!)is not null)));
    internal bool IsHalfHandSupportCandidate(ProgramTriggerCandidate candidate) => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects.Any(e => e.Op == SkillProgramEffectOp.OfferHalfHandRecipientSupport);
    internal bool HalfHandSupportPaymentMatches(ProgramSkillFrame f, ActualUseTargetWindowFrame parent, bool requirePaid)
    {
        if (f.HalfHandSupport is not { } paid || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.OtherActualUseTargeted, ActualUseTarget: { } use } context || context.ParentFrameId != parent.Id || parent.ParentFrameId != use.CardUseFrameId || use != paid.Use || use.TargetSeat != f.OwnerSeat || paid.Source != _coreHalfHandPhaseDebtProgramsOperations.HalfHandDebtSource(f) || paid.GameplayHash != f.GameplayHash || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) || !_coreActualUseTargetProgramsOperations.MatchesActualUseTarget(use) || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.OfferHalfHandRecipientSupport, StateId: { } state }] || paid.StateId != state || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HalfHandTargetSupportIssuedEvent>().SingleOrDefault(e => e.ProgramFrameId == paid.SupportProgramFrameId)is not { } issued || issued.OwnerSeat != f.OwnerSeat || issued.Source.SkillId != f.SkillId || issued.Source.SkillInstanceId != f.SkillInstanceId || issued.GameplayHash != f.GameplayHash || issued.StateId != state || issued.RecipientSeat != paid.RecipientSeat || issued.ActualDeliveredCount <= 0)
            return false;
        if (!requirePaid)
            return true;
        if (!paid.Paid || paid.Declined || paid.CardId is not { } cardId || paid.SequenceAfter <= paid.SequenceBefore)
            return false;
        var reason = $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.OfferHalfHandRecipientSupport)}";
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > paid.SequenceBefore && m.Sequence <= paid.SequenceAfter && m.CardId == cardId && m.From == CardLocation.Hand(paid.RecipientSeat) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == reason) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HalfHandSupportPaymentEvent>().Count(e => e.ProgramFrameId == f.Id && e.SupportProgramFrameId == issued.ProgramFrameId && e.CardUseFrameId == use.CardUseFrameId && e.CardActionId == use.ActionId && e.Source == paid.Source && e.OwnerSeat == f.OwnerSeat && e.RecipientSeat == paid.RecipientSeat && e.Paid && !e.Declined && e.SequenceBefore == paid.SequenceBefore && e.SequenceAfter == paid.SequenceAfter) == 1;
    }

    internal SkillProgramStepOutcome OfferHalfHandSupport(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.HalfHandSupport is not null || frame.InstructionIndex != 1 || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ActualUseTargetWindowFrame parent || frame.WindowContext?.ActualUseTarget is not { } use || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]) || !_coreActualUseTargetProgramsOperations.CanRunActualUseTarget(parent.Candidates[parent.CandidateIndex], frame.WindowContext) || _coreHalfHandPhaseDebtProgramsOperations.CurrentHalfHandSupport(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, effect.StateId!)is not { } issued || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[issued.RecipientSeat]).Count == 0)
            throw new InvalidOperationException("A support offer requires its actual original target and real current half-hand recipient.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { HalfHandSupport = new(0, use, issued.ProgramFrameId, _coreHalfHandPhaseDebtProgramsOperations.HalfHandDebtSource(frame), frame.GameplayHash, effect.StateId!, issued.RecipientSeat) });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var chooser = issued.RecipientSeat;
        var choices = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[chooser]).OrderBy(c => c.Id).Select(card => new PromptChoice(new ChoiceId($"half-hand-support.frame-{(frame.Id)}.give-{(card.Id)}"), $"交给{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)}一张手牌【{(card.DisplayName)}】。", [card.Id], [], HalfHandSupportParameters(frame, "give"))).Append(new PromptChoice(new ChoiceId($"half-hand-support.frame-{(frame.Id)}.pass"), "不交出手牌。", [], [], HalfHandSupportParameters(frame, "pass"))).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, "原好施受赠者可以交给当前目标一张手牌。", choices.SelectMany(c => c.Cards).ToArray(), [], SourceSeat: frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = Array.AsReadOnly(choices),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal Dictionary<string, string> HalfHandSupportParameters(ProgramSkillFrame f, string option) => new()
    {
        ["program-action"] = "half-hand-support",
        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
        ["support-option"] = option
    };
    [ProgramAnswer("half-hand-support")]
    internal void ResolveHalfHandSupportChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing support owning frame.");
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ActualUseTargetWindowFrame parent || f.HalfHandSupport is not { Paid: false, Declined: false } paid || !HalfHandSupportPaymentMatches(f, parent, false) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } decision || decision.PlayerSeat != paid.RecipientSeat || !decision.Choices.Any(c => c.Id == choice.Id) || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || choice.Targets.Count != 0)
            throw new InvalidOperationException("The support choice lost its exact private actual recipient.");
        if (!_coreHalfHandPhaseDebtProgramsOperations.HalfHandDebtSourceCurrent(f) || !_coreMatchParticipantsPlayersAccess._players[paid.RecipientSeat].IsAlive || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(paid.Use.CardUseFrameId, f.OwnerSeat) || _coreHalfHandPhaseDebtProgramsOperations.CurrentHalfHandSupport(f.OwnerSeat, f.SkillId, f.SkillInstanceId, f.GameplayHash, paid.StateId)?.ProgramFrameId != paid.SupportProgramFrameId)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "援助来源、原受赠者或真实用牌目标失效，未付牌。");
            return;
        }

        var option = choice.Parameters.GetValueOrDefault("support-option");
        if (option == "pass" && choice.Cards.Count == 0)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HalfHandSupport = paid with { Declined = true } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HalfHandSupportPaymentEvent(f.Id, paid.SupportProgramFrameId, paid.Use.CardUseFrameId, paid.Use.ActionId, paid.Source, f.OwnerSeat, paid.RecipientSeat, false, true, 0, 0));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (option != "give" || choice.Cards is not [var cardId] || _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(paid.RecipientSeat))
            throw new InvalidOperationException("The recipient must pay one actual current own hand entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var before = _coreHalfHandPhaseDebtProgramsOperations.HalfHandDebtSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HalfHandSupport = paid with { CardId = cardId, SequenceBefore = before, Paid = true }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var reason = new CardMoveReason($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.OfferHalfHandRecipientSupport)}");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([cardId], CardLocation.Hand(f.OwnerSeat), reason, _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, paid.RecipientSeat, f.OwnerSeat, [cardId], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, records) =>
        {
            if (records is not [var movement] || movement.From != CardLocation.Hand(paid.RecipientSeat) || movement.CardId != cardId || movement.To != CardLocation.Hand(f.OwnerSeat))
                throw new InvalidOperationException("Support cannot complete without its one real hand delivery.");
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HalfHandSupport = paid with { CardId = cardId, Paid = true, SequenceBefore = before, SequenceAfter = movement.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HalfHandSupportPaymentEvent(f.Id, paid.SupportProgramFrameId, paid.Use.CardUseFrameId, paid.Use.ActionId, paid.Source, f.OwnerSeat, paid.RecipientSeat, true, false, before, movement.Sequence));
        }));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferHalfHandRecipientSupport)]
    internal PromptChoice SelectAiHalfHandSupport(PendingDecision decision, ProgramSkillFrame frame)
    {
        var donor = frame.HalfHandSupport!.RecipientSeat;
        var gift = decision.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[donor]).Single(card => card.Id == c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[donor])).FirstOrDefault();
        var value = _coreAiBrainsAiBrainsAccess._aiBrains[donor].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(donor), frame.OwnerSeat, new(0, 0, 0, 1, 0, 0, false, false));
        return gift is not null && value > 0 ? gift : decision.Choices.Single(c => c.Parameters.GetValueOrDefault("support-option") == "pass");
    }
}
