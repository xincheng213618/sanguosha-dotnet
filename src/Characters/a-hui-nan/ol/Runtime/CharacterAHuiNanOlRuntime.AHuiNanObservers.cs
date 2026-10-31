#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterAHuiNanOlRuntime
{
    internal bool AHuiNanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AHuiNanJueman is not { } r || !ValidAHuiNan(f))
            return false;
        if (child is CardUseFrame use)
            return r.Stage == AHuiNanJuemanStage.NativeUse && use.AHuiNanJueman is { } o && MatchesAHuiNanJueman(f, o) && IsAHuiNanJuemanUse(use.Id);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == AHuiNanJuemanStage.DrawChildren && moved.ResumeProgramFrameId == f.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == AHuiNanDrawReason || m.Reason == CardMoveReasons.Reshuffle));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? AHuiNanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.AHuiNanFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27200)]
    internal bool AdvanceAHuiNanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && AHuiNanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveJuemanFirstBasicTurnEnd)]
    internal PromptChoice AiAHuiNan(PendingDecision p, ProgramSkillFrame f)
    {
        var friendly = f.AHuiNanJueman!.History[2].Kind is CardKind.Peach or CardKind.Alcohol;
        var hint = friendly ? new SkillProgramAiHint(0, 0, 0, 1, 0, 0, false, false) : new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false);
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat);
        return p.Choices.OrderByDescending(c => c.Targets.Sum(t => _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(view, t, hint))).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }
}
