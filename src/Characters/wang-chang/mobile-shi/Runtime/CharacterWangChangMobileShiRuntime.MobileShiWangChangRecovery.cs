#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangChangMobileShiRuntime
{
    internal void RequireShiWangChang(ProgramSkillFrame f)
    {
        if (f.MobileShiWangChang is not { } r || r.Source != ShiWangChangSource(f) || r.GameplayHash != f.GameplayHash || f.WindowContext is not { } c || c.ParentFrameId != r.WindowId || c.OwnerSeat != f.OwnerSeat || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiWangChangStartedEvent>().Count(e => e == new MobileShiWangChangStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.ActualTurn, r.TurnOwner, r.Maximum, r.UseId, r.ActionId, r.Actor, r.Kind)) != 1)
            throw new InvalidOperationException("Shi Wang Chang lost its exact source, native boundary or issued invoice.");
        if (f.SkillId == ShiKaiji)
        {
            if (f.TriggerId != "kaiji-prepare" || c.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || c.SourceSeat != f.OwnerSeat || r.Maximum < 1 || r.Maximum > _coreMatchParticipantsPlayersAccess._players.Count || r.Targets.Count > r.Maximum || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.Index < 0 || r.Index > r.Targets.Count || r.UseId != 0 || r.ActionId != 0 || r.Actor != -1 || r.Stage is MobileShiWangChangStage.Mode or MobileShiWangChangStage.Resolving || f.InstructionIndex is < 1 or > 3)
                throw new InvalidOperationException("Kaiji changed its preparation selection or paid recipient cursor.");
            if (r.Stage != MobileShiWangChangStage.Targets && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiKaijiTargetCommittedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Target).SequenceEqual(r.Targets))
                throw new InvalidOperationException("Kaiji changed its frozen original recipients after payment began.");
            if (r.Draw is { } draw && (r.Stage != MobileShiWangChangStage.DrawChildren || r.Index >= r.Targets.Count || draw.RecipientSeat != r.Targets[r.Index] || draw.RequestedCount != 1 || draw.SequenceBefore != r.Before || draw.SequenceAfter != r.After || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiKaijiDrawReturnedEvent>().Count(e => e.FrameId == f.Id && e.Index == r.Index && e.Target == draw.RecipientSeat && e.Actual == draw.ActualDrawIds.Count && e.Replacement == draw.ReplacementGainedIds.Count && e.Before == r.Before && e.After == r.After) != 1))
                throw new InvalidOperationException("Kaiji cannot repeat or redirect a completed native draw.");
        }
        else if (f.SkillId != ShiShepan || f.TriggerId != "shepan-adjust" || c.Window != SkillProgramTriggerWindow.CardUseBeforeTargetEffects || ShiShepanUse(c, f.OwnerSeat)is not { } use || use.Id != r.UseId || use.Action!.ActionId != r.ActionId || use.SourceSeat != r.Actor || use.CardKind != r.Kind || r.Maximum != 0 || r.Targets.Count != 0 || r.Index != 0 || r.Draw is not null || r.Stage is not (MobileShiWangChangStage.Mode or MobileShiWangChangStage.Resolving or MobileShiWangChangStage.Complete) || r.Stage == MobileShiWangChangStage.Mode && r.Mode is not null || r.Stage != MobileShiWangChangStage.Mode && r.Mode is not ("draw" or "top"))
            throw new InvalidOperationException("Shepan lost its original current damage-card target, branch or adjustment.");
    }

    internal bool ShiKaijiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.SkillId != ShiKaiji || f.InstructionIndex != 1 || f.MobileShiWangChang is not { } r)
            return false;
        RequireShiWangChang(f);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MobileShiWangChangStage.Drawing && f.NativeDraw is not null;
        if (r.Stage != MobileShiWangChangStage.DrawChildren || r.Draw is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal ProgramSkillFrame? ShiKaijiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterWangChangMobileShiOperations.ShiKaijiFirstChild(f, child), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 47400)]
    internal bool ShiKaijiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ShiKaijiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 47400)]
    internal bool AdvanceShiKaijiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiKaijiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 47400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 47400)]
    internal bool ShiKaijiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.SkillId != ShiKaiji || f.InstructionIndex != 1 || f.MobileShiWangChang is null)
            return false;
        RequireShiWangChang(f);
        if (_coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var root = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == recovery.Id);
        return root >= 0 && child > root + 1 && ShiKaijiFirstChild(f, _resolutionStackOperations._resolutionStack[root + 1]) && ShiKaijiObserverRoot()?.Id == f.Id && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 47400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 47400)]
    internal bool KeepPaidShiWangChang(ProgramSkillFrame f)
    {
        if (f.MobileShiWangChang is not { } r || !(f.SkillId == ShiKaiji && r.Stage != MobileShiWangChangStage.Targets || f.SkillId == ShiShepan && r.Stage is MobileShiWangChangStage.Resolving or MobileShiWangChangStage.Complete))
            return false;
        RequireShiWangChang(f);
        return true;
    }
}
