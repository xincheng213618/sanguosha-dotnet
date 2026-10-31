#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangGuanOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27210)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27210)]
    internal bool ContinueIssuedWangGuan(ProgramSkillFrame f)
    {
        if (f.WangGuan is not null)
        {
            return true;
        }

        if (f.InstructionIndex is not (0 or 1) || f.SkillId != "ol:miuyan" || f.TriggerId is null || f.WindowContext is not { } context)
            return false;
        return CanRunIssuedMiuyanCandidate(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), context);
    }

    internal bool WangGuanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WangGuan is not { Stage: WangGuanStage.MiuyanGainChildren or WangGuanStage.ShiluDrawChildren } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (r.Op == SkillProgramEffectOp.ResolveShiluDrawRevealIdentity ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ShiluDrawReason || m.Reason == CardMoveReasons.Reshuffle : r.Entities.Any(e => e.CardId == m.CardId && m.From == CardLocation.Hand(e.HolderSeat)) && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MiuyanGainReason));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? WangGuanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.WangGuanFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27210)]
    internal bool AdvanceWangGuanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WangGuanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
