#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFactionRequestCostRecoveriesRuntime
{
    internal bool TryPausePaidFactionRequestCost(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, Card card, CardLocation from)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not { PendingRecoveryAttempts.Count: > 0 } producer)
            return false;
        if (!request.CostPaid || !request.AwaitingProviders || request.CandidateIndex != 0 || request.PolicySource is not { DiscardCost: 1 } policy || producer.PaidFactionRequestCostRecovery is not null)
            throw new InvalidOperationException("Faction request recovery lost its exact newly paid cost.");
        _coreRuntimeOperations.ReplaceRuntimeTop(producer with { PaidFactionRequestCostRecovery = new(request.OwnerFrameId, request.OwnerSeat, request.Purpose, policy, card.Id, from) });
        if (!_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(producer.Id, PostEventContinuation.FactionRequestCost))
            ContinuePaidFactionRequestCost(producer.Id);
        return true;
    }

    internal void ContinuePaidFactionRequestCost(long producerId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not { } producer || producer.Id != producerId || producer.PaidFactionRequestCostRecovery is not { } receipt || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not { } request || !PaidFactionRequestMatches(request, receipt))
            throw new InvalidOperationException("Paid faction request lost its exact producer and frozen request.");
        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(producerId, PostEventContinuation.FactionRequestCost) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(producerId))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(producer with { PaidFactionRequestCostRecovery = null });
        var owner = _coreMatchParticipantsPlayersAccess._players[request.OwnerSeat];
        var sourceLive = _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.FactionResponseRequest, requiredKind: request.RequiredKind).Any(item => item.Source.SkillId == receipt.PolicySource.SkillId && item.Source.SkillInstanceId == receipt.PolicySource.SkillInstanceId && item.Policy.Id == receipt.PolicySource.PolicyId);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive)
        {
            CancelPaidFactionRequestAfterRecovery(request);
            return;
        }

        if (request.TargetSeat is { } targetSeat && !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive && (request.IsProgramSkillUse || request.IsQinglongCrescentBladeUse || request.IsBorrowedSwordUse))
        {
            if (request.IsBorrowedSwordUse)
                _coreNativeFactionSlashOperations.CompleteFactionSlashFailure(request);
            else
                CancelPaidFactionRequestAfterRecovery(request);
            return;
        }

        if (!sourceLive)
            _coreNativeFactionSlashOperations.CompleteFactionSlashFailure(request);
        else
            _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal static bool PaidFactionRequestMatches(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, PaidFactionRequestCostRecovery receipt) => request.OwnerFrameId == receipt.RequestOwnerFrameId && request.OwnerSeat == receipt.RequestOwnerSeat && request.Purpose == receipt.Purpose && request.PolicySource == receipt.PolicySource && request.CostPaid && !request.ProviderRewarded && request.AwaitingProviders && request.CandidateIndex == 0 && request.ActiveAttack is null && !request.AwaitingZhuqueFanChoice;
    internal bool HasPaidFactionRequestCostRecovery(global::CardGame.Core.NativeOwners.FactionCardRequestHandle? request)
    {
        if (request is null)
            return false;
        var producerIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.PaidFactionRequestCostRecovery is { } receipt && PaidFactionRequestMatches(request, receipt));
        if (producerIndex < 0)
            return false;
        for (var index = producerIndex + 1; index < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            var child = _resolutionStackOperations._resolutionStack[index];
            var parent = _resolutionStackOperations._resolutionStack[index - 1];
            if (child is RecoveryReplacementFrame recovery && recovery.ParentFrameId == parent.Id && recovery.Return.ResumeFrameId == parent.Id && recovery.Return.Continuation is PostEventContinuation.FactionRequestCost or PostEventContinuation.RecoveryReplacement)
                continue;
            if (child is HpChangedTriggerWindowFrame hp && hp.ResumeFrameId == parent.Id && hp.Continuation is PostEventContinuation.FactionRequestCost or PostEventContinuation.RecoveryReplacement)
                continue;
            if (child is CardsMovedTriggerWindowFrame movement && movement.Batch.ParentFrameId == parent.Id && (movement.ResumeFactionRequestCostFrameId == parent.Id || movement.ResumeRecoveryReplacementFrameId == parent.Id))
                continue;
            if (_coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(child, parent) || _corePublicPileEquipmentSequencesOperations.PileEquipmentFrameRidesOn(child, parent) || _coreDeckEquipmentProgramsOperations.RandomEquipmentFrameRidesOn(child, parent))
                continue;
            if (child is DyingFrame dying && parent is ProgramSkillFrame program && dying.ParentFrameId == program.Id && dying.ResumesProgramSkill)
                continue;
            if (child is ProgramSkillFrame { WindowContext: { } response } && parent is DyingFrame dyingParent && response.ParentFrameId == dyingParent.Id && response.Window is SkillProgramTriggerWindow.DyingResponse or SkillProgramTriggerWindow.SelfDyingResponse)
                continue;
            return false;
        }

        return true;
    }

    internal void CancelPaidFactionRequestAfterRecovery(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request)
    {
        var purpose = request.Purpose;
        var responseAttack = request.ResponseAttack;
        var borrowed = request.BorrowedSword;
        var qinglong = request.QinglongCrescentBlade;
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
        if (request.IsProgramSkillUse)
        {
            if (request.IsAssistedProgramUse)
                _coreProgramSlashRequestOperations.CommitProgramChoiceResult(request.ProgramSkillFrameId!.Value, request.AssistedResultBind!, "declined", request.OwnerSeat, "已付请求在回复子链后结束。");
            _coreRuntimeOperations.AdvanceRuntimeProgram(request.ProgramSkillFrameId!.Value);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(request.ResolutionId, request.OwnerSeat, false, null, null, null, request.IsBorrowedSwordUse || request.IsQinglongCrescentBladeUse, request.TargetSeat));
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response)
            _coreNativeResponseWindowsOperations.PopResponseWindow(response.ParentFrameId);
        if (purpose == FactionCardRequestPurpose.BorrowedSwordUse)
        {
            _coreNativeBorrowedSwordOperations.FinishBorrowedSword(borrowed!);
            return;
        }

        if (purpose == FactionCardRequestPurpose.QinglongCrescentBladeUse)
        {
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativeCardUseFramesOperations.SetCardUseStep(qinglong!.Attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(qinglong.Attack.ResolutionId, qinglong.Attack.SourceSeat, qinglong.Attack.TargetSeat, false, [], null));
            _coreQinglongInsertionOperations.RestoreFactionRequestAfterQinglongChoice(qinglong.Attack.ResolutionId);
            _coreNativeAttackCompletionOperations.CompleteAttack(qinglong.Attack);
            return;
        }

        _coreNativeAttackCompletionOperations.CompleteAttack(responseAttack!);
    }

    internal void AssertPaidFactionRequestCostRecoveries()
    {
        foreach (var producer in _resolutionStackOperations._resolutionStack.Where(frame => frame.PaidFactionRequestCostRecovery is not null))
        {
            var receipt = producer.PaidFactionRequestCostRecovery!;
            if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not { } request || !PaidFactionRequestMatches(request, receipt) || receipt.CostLocation != CardLocation.Equipment(receipt.RequestOwnerSeat) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(move => move.CardId == receipt.CostCardId && move.From == receipt.CostLocation && move.To == CardLocation.DiscardPile && move.Reason.Value == "program.faction-request.cost") || !HasPaidFactionRequestCostRecovery(request))
                throw new InvalidOperationException("Paid faction request recovery changed its committed cost or exact descendant chain.");
        }
    }
}
