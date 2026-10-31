#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSkillInvocationNativeRuntime
{
    internal void ResumeNativeSkillInvocation(SkillInvocationFrame frame)
    {
        if (frame.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || frame.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || frame.ParentFrameId != 0 && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.ParentFrameId)
            throw new InvalidOperationException("A skill invocation lost its original native turn or source frame.");
        switch (frame.ReturnKind)
        {
            case SkillInvocationReturnKind.RuntimeFrame:
                _coreRuntimeOperations.AdvanceRuntimeFrame(frame.ParentFrameId);
                return;
            case SkillInvocationReturnKind.SkipDiscard:
                if (frame.ParentFrameId != 0 || frame.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Discard || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardPolicyResolvedEvent>().Any(e => e.OwnerSeat == frame.ActorSeat && e.SkillId == frame.SkillKey && e.Applied))
                    throw new InvalidOperationException("A discard policy invocation lost its accepted native choice.");
                if (_coreMatchOutcomeWinnerAccess._winner == Winner.None)
                    _coreActualTurnsOperations.EndTurn();
                return;
            case SkillInvocationReturnKind.FactionDefense:
                if (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not { } defense || defense.OwnerSeat != frame.ActorSeat || defense.Source.SkillId != frame.SkillKey || defense.Source.SkillInstanceId != frame.SkillInstanceId || defense.InvocationOccurrenceKey != frame.OccurrenceKey)
                    throw new InvalidOperationException("A faction defense invocation lost its exact provider request.");
                ValidateNativeFactionInvocationReturn(frame, defense.OwnerFrameId);
                if (!_coreMatchParticipantsPlayersAccess._players[frame.ActorSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
                {
                    RecordNativeFactionInvocationCancellation(frame, defense.OwnerFrameId);
                    CancelInvokedFactionDefense(defense);
                    return;
                }

                _coreNativeFactionDefenseOperations.AdvanceFactionDefenseCandidate();
                return;
            case SkillInvocationReturnKind.FactionSlash:
                if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not { AwaitingProviders: true } slash || slash.OwnerSeat != frame.ActorSeat || slash.PolicySource is not { } source || source.SkillId != frame.SkillKey || source.SkillInstanceId != frame.SkillInstanceId || slash.InvocationOccurrenceKey != frame.OccurrenceKey || slash.IsProgramSkillUse)
                    throw new InvalidOperationException("A faction Slash invocation lost its exact provider request.");
                ValidateNativeFactionInvocationReturn(frame, slash.OwnerFrameId);
                if (!_coreMatchParticipantsPlayersAccess._players[frame.ActorSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
                {
                    RecordNativeFactionInvocationCancellation(frame, slash.OwnerFrameId);
                    CancelInvokedFactionSlash(slash);
                    return;
                }

                _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
                return;
            case SkillInvocationReturnKind.NativeEquipment:
                _coreNativeEquipmentInvocationsOperations.ContinueNativeEquipmentInvocation(frame);
                return;
            default:
                throw new InvalidOperationException("The skill invocation has no native return contract.");
        }
    }

    internal void ValidateNativeFactionInvocationReturn(SkillInvocationFrame frame, long requestOwnerId)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not { } parent || parent.Id != frame.ParentFrameId || parent.Id != requestOwnerId && (parent is not ResponseWindowFrame response || response.ParentFrameId != requestOwnerId) || parent.SkillInvocations?.Issued.Contains(global::CardGame.Core.EngineOwners.CharacterHuaTuoAnniversaryGodRuntime.InvocationSourceKey(frame.ActorSeat, frame.SkillKey, frame.OccurrenceKey), StringComparer.Ordinal) != true || _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationStartedEvent>().Count(e => e == new SkillInvocationStartedEvent(frame.Id, frame.ParentFrameId, frame.ActorSeat, frame.SkillKey, frame.OccurrenceKey, frame.SkillInstanceId, frame.SkillName, frame.GameplayHash, frame.RoundNumber, frame.TurnNumber, frame.TurnOwnerSeat, frame.ReturnKind)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationCompletedEvent>().Count(e => e.FrameId == frame.Id && e.ParentFrameId == frame.ParentFrameId && e.OccurrenceKey == frame.OccurrenceKey) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<NativeFactionSkillInvocationCanceledEvent>().Any(e => e.InvocationId == frame.Id))
            throw new InvalidOperationException("A native faction activation lost its original accepted source and exact completed typed return.");
    }

    internal void RecordNativeFactionInvocationCancellation(SkillInvocationFrame frame, long requestOwnerId) => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeFactionSkillInvocationCanceledEvent(frame.Id, frame.ParentFrameId, requestOwnerId, frame.ReturnKind, frame.ActorSeat, frame.SkillKey, frame.SkillInstanceId, frame.OccurrenceKey));
    internal void CancelInvokedFactionDefense(global::CardGame.Core.NativeOwners.FactionDefenseHandle defense)
    {
        var attack = defense.Attack;
        _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense = null;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionDefenseResolvedEvent(defense.OwnerFrameId, defense.OwnerSeat, false, null, null));
        FinishCanceledNativeFactionResponse(attack);
    }

    internal void CancelInvokedFactionSlash(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request)
    {
        var purpose = request.Purpose;
        var attack = request.ResponseAttack;
        var borrowed = request.BorrowedSword;
        var qinglong = request.QinglongCrescentBlade;
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(request.ResolutionId, request.OwnerSeat, false, null, null, null, request.IsBorrowedSwordUse || request.IsQinglongCrescentBladeUse, request.TargetSeat));
        switch (purpose)
        {
            case FactionCardRequestPurpose.BorrowedSwordUse:
                // The native dead-weapon-owner branch also finishes without a
                // weapon transfer or a newly published owner Slash decision.
                _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordWithoutSlash(borrowed ?? throw new InvalidOperationException("Canceled faction use lost Borrowed Sword."), transferWeapon: false);
                return;
            case FactionCardRequestPurpose.QinglongCrescentBladeUse:
            {
                var original = qinglong?.Attack ?? throw new InvalidOperationException("Canceled faction use lost Qinglong.");
                _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
                _coreNativeCardUseFramesOperations.SetCardUseStep(original.ResolutionId, ResolutionFrameStep.ResolvingEffect);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(original.ResolutionId, original.SourceSeat, original.TargetSeat, false, [], null));
                _coreQinglongInsertionOperations.RestoreFactionRequestAfterQinglongChoice(original.ResolutionId);
                _coreNativeAttackCompletionOperations.CompleteAttack(original);
                return;
            }

            case FactionCardRequestPurpose.DuelResponse:
            case FactionCardRequestPurpose.GroupResponse:
                FinishCanceledNativeFactionResponse(attack ?? throw new InvalidOperationException("Canceled faction response lost its attack."));
                return;
            default:
                throw new InvalidOperationException("No native cancellation contract exists for this faction request purpose.");
        }
    }

    internal void FinishCanceledNativeFactionResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ResponseWindowFrame response || response.ParentFrameId != attack.ResolutionId || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack))
            throw new InvalidOperationException("Canceled native faction response lost its original response host.");
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeResponseWindowsOperations.SetResponseParentStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        // Native completion skips this unavailable target and retains any real
        // group/halberd cursor and typed card-use parent return.
        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }
}
