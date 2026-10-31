#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiBoundaryMobileRuntime
{
    internal const string BoundaryZhangFeiPaoxiao = "boundary:paoxiao-mobile", BoundaryZhangFeiLiyong = "boundary:liyong-mobile", BoundaryZhangFeiReserve = "reserve-after-real-dodge", BoundaryZhangFeiIssue = "issue-next-real-use", BoundaryZhangFeiTargets = "issued-target-rights", BoundaryZhangFeiPenalty = "issued-surviving-damage";
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 58600)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 58600)]
    internal bool BoundaryZhangFeiPaoxiaoDistance(ProgramSlashDistanceQuery query) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && query.Actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == query.Actor.Seat && _coreNationalSkillsOperations.HasRuntimeSkill(query.Actor, BoundaryZhangFeiPaoxiao) && _corePlayCardHistoryOperations.EventsSinceLastBoundary(fact => fact is PhaseChangedEvent or TurnStartedEvent).OfType<CardUseDeclaredEvent>().Any(fact => fact.SourceSeat == query.Actor.Seat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(fact.CardKind) && fact.ResolutionId != query.ExistingUseFrameId);
    internal CardConversionSource BoundaryZhangFeiSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    internal bool BoundaryZhangFeiOwnPlay(int owner) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == owner && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == owner && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;
    internal BoundaryLiyongReserve? BoundaryZhangFeiReady(int owner, string instance)
    {
        if (!BoundaryZhangFeiOwnPlay(owner))
            return null;
        var reserve = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiyongReservedEvent>().Select(fact => fact.Reserve).LastOrDefault(item => item.Source.OwnerSeat == owner && item.Source.SkillInstanceId == instance && item.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && item.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        return reserve is not null && !_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiyongUseIssuedEvent>().Any(fact => fact.Use.Reserve == reserve) ? reserve : null;
    }

    internal CardUseFrame? BoundaryZhangFeiWindowUse(ProgramSkillWindowContext? context) => context?.CardUse is { } card && _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } action } use && action.ActionId == card.CardActionId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) ? use : null;
    internal bool BoundaryZhangFeiSourceMatches(BoundaryLiyongUse right, ProgramTriggerCandidate candidate) => right.Reserve.Source.OwnerSeat == candidate.OwnerSeat && right.Reserve.Source.SkillId == candidate.SkillId && right.Reserve.Source.SkillInstanceId == candidate.SkillInstanceId && right.Reserve.GameplayHash == candidate.GameplayHash;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 58600)]
    internal bool CanOfferBoundaryZhangFei(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != BoundaryZhangFeiLiyong)
            return true;
        if (candidate.BindingId == BoundaryZhangFeiReserve)
            return BoundaryZhangFeiOwnPlay(candidate.OwnerSeat) && BoundaryZhangFeiWindowUse(context)is { CardAttack: { } state } use && use.Action!.ActorSeat == candidate.OwnerSeat && state.SuccessfulDodgeResponses >= state.RequiredDodgeResponses && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.ResolutionId == use.Id && !_coreSlashSuitDiscardOperations.IsSlashDodgeCancellationPrevented(attack);
        if (candidate.BindingId == BoundaryZhangFeiIssue)
            return BoundaryZhangFeiOwnPlay(candidate.OwnerSeat) && BoundaryZhangFeiReady(candidate.OwnerSeat, candidate.SkillInstanceId)is not null && BoundaryZhangFeiWindowUse(context)is { BoundaryLiyong: null, Action: { } action } && action.ActorSeat == candidate.OwnerSeat;
        if (candidate.BindingId == BoundaryZhangFeiTargets)
            return BoundaryZhangFeiWindowUse(context)is { BoundaryLiyong: { } right, Action: { } action } && BoundaryZhangFeiSourceMatches(right, candidate) && context.TargetSeat is { } target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && action.EffectiveDesignatedTargetSeats.Contains(target);
        return candidate.BindingId == BoundaryZhangFeiPenalty && BoundaryZhangFeiPenaltyUse(candidate, context)is not null;
    }

    internal SkillProgramStepOutcome ResolveBoundaryZhangFei(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var context = frame.WindowContext ?? throw new InvalidOperationException("Liyong lost its native window.");
        if (operation == SkillProgramEffectOp.PayBoundaryZhangFeiLiyong)
            return PayBoundaryZhangFeiLiyong(frame);
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.SkillInstanceId, frame.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(frame).Priority);
        if (!CanOfferBoundaryZhangFei(candidate, context))
            throw new InvalidOperationException("Liyong lost its original actual use or phase qualification.");
        var use = BoundaryZhangFeiWindowUse(context)!;
        if (operation == SkillProgramEffectOp.ReserveBoundaryZhangFeiLiyong)
        {
            var target = use.CardAttack!.TargetSeat;
            var cancellation = new NamedSlashCancellation(frame.SkillId, frame.OwnerSeat, target);
            if (use.NamedSlashCancellations?.Contains(cancellation) == true)
                return SkillProgramStepOutcome.Continue;
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { NamedSlashCancellations = Array.AsReadOnly((use.NamedSlashCancellations ?? []).Append(cancellation).ToArray()) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiyongReservedEvent(new(frame.Id, BoundaryZhangFeiSource(frame), frame.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, use.Id, use.Action!.ActionId, target)));
        }
        else if (context.Window == SkillProgramTriggerWindow.CardUseCommitted)
        {
            var reserve = BoundaryZhangFeiReady(frame.OwnerSeat, frame.SkillInstanceId)!;
            var right = new BoundaryLiyongUse(reserve, use.Id, use.Action!.ActionId);
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { BoundaryLiyong = right });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiyongUseIssuedEvent(right));
        }
        else
        {
            var target = context.TargetSeat!.Value;
            _coreCurrentTurnSkillSuppressionsOperations.IssueCurrentTurnNonLockedSkillSuppression(frame, target);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiyongTargetIssuedEvent(frame.Id, use.Id, use.Action!.ActionId, target));
        }

        return SkillProgramStepOutcome.Continue;
    }

    internal bool IsBoundaryZhangFeiLiyongUnrespondable(long frameId) => _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)is { Action: { } action, BoundaryLiyong: { } right } && right.CardUseFrameId == frameId && right.CardActionId == action.ActionId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind);
    internal int BoundaryZhangFeiLiyongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is global::CardGame.Core.NativeOwners.CardAttackHandle && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) ? _coreFinalTargetSlashOperations.FinalTargetSlashReceipts(attack.ResolutionId, attack.TargetSeat).Where(receipt => receipt.SkillId == BoundaryZhangFeiLiyong).Sum(receipt => receipt.DamageBonus) : 0;
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 58600)]
    internal void IncludeBoundaryZhangFeiTargets(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || action.Type != CardActionType.Use || window is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardUseTargetsFinalized))
            return;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(item => item.Action?.ActionId == action.ActionId);
        var reserve = window == SkillProgramTriggerWindow.CardUseCommitted ? _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiyongReservedEvent>().Select(item => item.Reserve).LastOrDefault(item => item.Source.OwnerSeat == action.ActorSeat && BoundaryZhangFeiReady(action.ActorSeat, item.Source.SkillInstanceId) == item) : use?.BoundaryLiyong?.Reserve;
        if (reserve is null)
            return;
        var binding = window == SkillProgramTriggerWindow.CardUseCommitted ? BoundaryZhangFeiIssue : BoundaryZhangFeiTargets;
        result.RemoveAll(candidate => candidate.SkillId == BoundaryZhangFeiLiyong && candidate.TriggerId == binding);
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(BoundaryZhangFeiLiyong).Program!.Triggers.Single(item => item.Id == binding);
        var targets = window == SkillProgramTriggerWindow.CardUseCommitted ? new[]
        {
            action.ActorSeat
        }

        : action.EffectiveDesignatedTargetSeats.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
        foreach (var target in targets)
        {
            var owner = reserve.Source.OwnerSeat;
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, owner, target, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[owner], action));
            result.Add(new(owner, target, BoundaryZhangFeiLiyong, trigger.Id, reserve.GameplayHash, reserve.Source.SkillInstanceId, trigger.Priority, context));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 58600)]
    internal bool IsIssuedBoundaryZhangFei(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId == BoundaryZhangFeiLiyong && (candidate.BindingId == BoundaryZhangFeiIssue && context.Window == SkillProgramTriggerWindow.CardUseCommitted && BoundaryZhangFeiReady(candidate.OwnerSeat, candidate.SkillInstanceId)is { } reserve && reserve.GameplayHash == candidate.GameplayHash && BoundaryZhangFeiWindowUse(context)?.Action?.ActorSeat == candidate.OwnerSeat || candidate.BindingId == BoundaryZhangFeiTargets && BoundaryZhangFeiWindowUse(context)?.BoundaryLiyong is { } right && BoundaryZhangFeiSourceMatches(right, candidate) || candidate.BindingId == BoundaryZhangFeiPenalty && BoundaryZhangFeiPenaltyUse(candidate, context)is not null);
    [ProgramPipelineHook(ProgramPipelinePhase.AdmitTurnSuppression, 58600)]
    internal bool AdmitBoundaryZhangFeiSuppression(ProgramSkillFrame frame, int target) => frame.SkillId == BoundaryZhangFeiLiyong && frame.TriggerId == BoundaryZhangFeiTargets && frame.WindowContext?.TargetSeat == target && BoundaryZhangFeiWindowUse(frame.WindowContext)?.BoundaryLiyong is { } right && right.Reserve.Source.SkillInstanceId == frame.SkillInstanceId;
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 58600)]
    internal void AssertBoundaryZhangFeiUses()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(item => item.BoundaryLiyong is not null))
        {
            var right = use.BoundaryLiyong!;
            var reserve = right.Reserve;
            if (use.Action is not { Type: CardActionType.Use } action || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || right.CardUseFrameId != use.Id || right.CardActionId != action.ActionId || reserve.Source.SkillId != BoundaryZhangFeiLiyong || reserve.Source.OwnerSeat != action.ActorSeat || reserve.Source.BindingId != BoundaryZhangFeiReserve || reserve.ProducerFrameId <= 0 || reserve.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || reserve.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || reserve.CancelledUseId == use.Id || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiyongReservedEvent>().Count(fact => fact.Reserve == reserve) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiyongUseIssuedEvent>().Count(fact => fact.Use == right) != 1)
                throw new InvalidOperationException("Liyong lost the original phase cancellation or exact next actual Use receipt.");
        }
    }

    SkillProgramStepOutcome global::CardGame.Core.IBoundaryZhangFeiHost.ResolveBoundaryZhangFei(ProgramSkillFrame frame, SkillProgramEffectOp operation) => ResolveBoundaryZhangFei(frame, operation);
}
