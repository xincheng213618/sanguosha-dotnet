#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaMobileGodRuntime
{
    internal bool CanBeginShenGuoJiaJudgment(ProgramSkillFrame frame, int targetSeat, string reason, string resultBind, SkillProgramCardSetVisibility visibility) => ValidShenGuoJia(frame) && frame.ShenGuoJia is { Stage: ShenGuoJiaStage.Judging, Operation: SkillProgramEffectOp.ResolveShenGuoJiaHuishi } receipt && receipt.JudgmentId > 0 && targetSeat == frame.OwnerSeat && reason == "mobile-huishi" && resultBind == $"huishi-{(receipt.Judgments.Count)}" && visibility == SkillProgramCardSetVisibility.Public;
    internal bool IsShenGuoJiaJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => CanBeginShenGuoJiaJudgment(frame, judgment.TargetSeat, judgment.Reason, judgment.ProgramResultBind ?? "", judgment.ProgramResultVisibility ?? SkillProgramCardSetVisibility.Private) && judgment.Id == frame.ShenGuoJia!.JudgmentId && judgment.ParentFrameId == frame.Id && judgment.SourceSeat == frame.OwnerSeat && judgment.Continuation == JudgmentContinuationKind.ProgramSkill;
    internal void BeginShenGuoJiaJudgment(ProgramSkillFrame frame)
    {
        var receipt = frame.ShenGuoJia!;
        var judgmentId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt with { Stage = ShenGuoJiaStage.Judging, JudgmentId = judgmentId }, PendingMovementContinuation = null });
        _ = _coreNativeJudgmentOperations.BeginJudgment(null, frame.OwnerSeat, "mobile-huishi", frame.Id, null, JudgmentContinuationKind.ProgramSkill, sourceSeat: frame.OwnerSeat, programResultBind: $"huishi-{(receipt.Judgments.Count)}", programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    internal bool TryReturnShenGuoJiaJudgment(JudgmentFrame judgment)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenGuoJia: { Stage: ShenGuoJiaStage.Judging } receipt } frame || judgment.ParentFrameId != frame.Id)
            return false;
        if (!IsShenGuoJiaJudgment(judgment, frame) || _resolutionStackOperations._resolutionStack.Any(value => value.Id == judgment.Id))
            throw new InvalidOperationException("Huishi requires its completed own native judgment.");
        var result = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JudgmentResolvedEvent>().Single(fact => fact.ResolutionId == judgment.Id && fact.ParentResolutionId == frame.Id && fact.Reason == "mobile-huishi" && fact.TargetSeat == frame.OwnerSeat);
        var retained = result.CardId is { } id && _coreProgramJudgmentOutcomesOperations.IsUnclaimedJudgmentOutcomeCard(judgment, id) ? id : (int? )null;
        var handoff = retained is { } card ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(movement => movement.CardId == card).Sequence : (long? )null;
        var outcome = new ShenGuoJiaJudgment(judgment.Id, result.CardId, result.Suit, result.Rank, handoff);
        var mayRepeat = result.Suit is { } suit && !receipt.Judgments.Any(previous => previous.Suit == suit) && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp < 10;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt with { Stage = ShenGuoJiaStage.JudgmentChildren, Judgments = receipt.Judgments.Append(outcome).ToArray(), Cards = retained is { } owned ? receipt.Cards.Append(owned).ToArray() : receipt.Cards, MayRepeat = mayRepeat, Before = handoff is { } sequence ? sequence - 1 : _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaJudgmentReturnedEvent(frame.Id, receipt.Judgments.Count, outcome));
        _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, $"huishi-{(receipt.Judgments.Count)}", retained is { } bound ? [bound] : [], SkillProgramCardSetVisibility.Public, retained is not null ? [CardLocation.Processing] : [], result.Suit);
        ContinueShenGuoJia(frame.Id);
        return true;
    }

    internal IReadOnlyList<ShenGuoJiaPlan> ShenGuoJiaPlans(int owner)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        var options = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, physicalSuit: Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: []);
        return Array.AsReadOnly(_coreNextActualUseTargetAdjustmentsOperations.NextActualUseOrdinaryTrickOptions(actor, options, Suit.None, null, Suit.None, false).Where(option => CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(option.EffectiveCardKind)).Select(option => new ShenGuoJiaPlan(option.EffectiveCardKind, option.ActionKind, option.TargetCardId, option.RequiredCardKind, option.NextActualUseAdjusted) { Targets = _coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(option.EffectiveCardKind, owner, option.TargetSeats) }).ToArray());
    }

    internal static bool SameShenGuoJiaPlan(ShenGuoJiaPlan left, ShenGuoJiaPlan right) => (left.Kind, left.ActionKind, left.TargetCardId, left.RequiredKind, left.Adjusted) == (right.Kind, right.ActionKind, right.TargetCardId, right.RequiredKind, right.Adjusted) && left.Targets.SequenceEqual(right.Targets);
    internal void IssueShenGuoJiaVirtualUse(ProgramSkillFrame frame)
    {
        var receipt = frame.ShenGuoJia!;
        if (!ValidShenGuoJia(frame) || receipt is not { Stage: ShenGuoJiaStage.CostChildren, Operation: SkillProgramEffectOp.ResolveShenGuoJiaZuoxing, Plan: { } plan } || !ShenGuoJiaPlans(frame.OwnerSeat).Any(option => SameShenGuoJiaPlan(option, plan)))
        {
            FinishShenGuoJia(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt with { Stage = ShenGuoJiaStage.IssuingUse } });
        if (plan.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = plan.Targets;
        var origin = new ProgramVirtualOrdinaryTrickOrigin(frame.Id, frame.InstructionIndex, 0, frame.OwnerSeat, frame.OwnerSeat, frame.OwnerSeat, frame.SkillId, frame.ActivationId, frame.SkillInstanceId, frame.GameplayHash, receipt.Turn, plan.Kind, SkillProgramConditionKind.Always, null, null);
        var card = new Card(0, plan.Kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, frame.OwnerSeat, plan.Targets, plan.Kind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        frame = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(value => value.Id == frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame = frame with { ShenGuoJia = frame.ShenGuoJia!with { Stage = ShenGuoJiaStage.NativeUse, UseId = id, ActionId = use.Action!.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaNativeIssuedEvent(frame.Id, id, use.Action!.ActionId, plan.Kind, receipt.Benefactor));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, frame.OwnerSeat, use.TargetSeats, plan.ActionKind, plan.TargetCardId, plan.RequiredKind, plan.Kind);
    }

    internal bool IsShenGuoJiaVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: ShenGuoJiaZuoxing } origin || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == origin.ParentProgramFrameId)is not { ShenGuoJia: { Plan: { } plan } receipt } frame || !ValidShenGuoJia(frame) || receipt.Stage is not (ShenGuoJiaStage.IssuingUse or ShenGuoJiaStage.NativeUse) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, RequesterSeat: null, ResponderSeat: null, OpponentSeat: null } action || action.ProviderSeat != frame.OwnerSeat || use.SourceSeat != action.ActorSeat || action.ActionId != origin.CardActionId || action.EffectiveKind != plan.Kind || use.CardKind != plan.Kind || action.EffectiveSuit is not (null or Suit.None) || action.EffectiveRank is not (null or 0) || action.EffectiveIsRed == true || !action.TargetSeats.SequenceEqual(use.TargetSeats) || origin.OwnerSeat != frame.OwnerSeat || origin.InitialActorSeat != frame.OwnerSeat || origin.InitialTargetSeat != frame.OwnerSeat || origin.BindingId != frame.ActivationId || origin.SkillInstanceId != frame.SkillInstanceId || origin.GameplayHash != frame.GameplayHash || origin.InstructionIndex != frame.InstructionIndex || origin.LifecycleFrameId != 0 || origin.TurnNumber != receipt.Turn || receipt.UseId != 0 && (receipt.UseId != use.Id || receipt.ActionId != action.ActionId))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaMaximumPaidEvent>().Count(fact => fact.FrameId == frame.Id && fact.Purpose == "zuoxing" && fact.Seat == receipt.Benefactor && fact.After == fact.Before - 1) == 1 && (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(fact => fact.ResolutionId == use.Id) ? _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, frame.OwnerSeat, frame.OwnerSeat) : action.ActorSeat == frame.OwnerSeat);
    }

    internal bool TryReturnShenGuoJiaVirtualUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: ShenGuoJiaZuoxing } origin)
            return false;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(origin.ParentProgramFrameId);
        var receipt = frame.ShenGuoJia!;
        if (!ValidShenGuoJia(frame) || receipt.Stage != ShenGuoJiaStage.NativeUse || receipt.UseId != use.Id || receipt.ActionId != use.Action?.ActionId || use.Step != ResolutionFrameStep.Completed || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaNativeReturnedEvent>().Any(fact => fact.FrameId == frame.Id) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == use.Id && fact.CardId == 0 && fact.CardKind == receipt.Plan!.Kind) != 1)
            throw new InvalidOperationException("Zuoxing requires the same complete native zero-material Use once.");
        var hadAttack = use.CardAttack is { Active: true };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ShenGuoJia = receipt with { Stage = ShenGuoJiaStage.ReturnedUse } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenGuoJiaNativeReturnedEvent(frame.Id, use.Id, receipt.ActionId, hadAttack));
        if (!hadAttack)
            FinishShenGuoJia(frame);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 38000)]
    internal bool ReturnShenGuoJiaAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenGuoJia: { Stage: ShenGuoJiaStage.ReturnedUse } receipt } frame)
            return false;
        if (!ValidShenGuoJia(frame) || _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.UseId)is not null || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShenGuoJiaNativeReturnedEvent>().Any(fact => fact.FrameId == frame.Id && fact.HadAttack))
            throw new InvalidOperationException("Zuoxing cannot resume before the native damage tail returns.");
        FinishShenGuoJia(frame);
        return true;
    }
}
