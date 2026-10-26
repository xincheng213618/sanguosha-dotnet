namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 82300)]
    private void AssertMobileDuanfa(ProgramSkillFrame f)
    {
        if (f.MobileDuanfa is not { } r) return;
        if (f.SkillId != MobileDuanfaId || f.ActivationId != "phase-black-recycle" || f.InstructionIndex != 1 || f.WindowContext is not null ||
            r.Source != MobileDuanfaSource(f) || r.Hash != f.GameplayHash || r.Phase.PhaseActorSeat != f.OwnerSeat || !Enum.IsDefined(r.Stage) ||
            r.Cards.Distinct().Count() != r.Cards.Count || r.ActualCount < 0 || r.ActualCount > r.Cards.Count || r.Before > r.After || r.After > _movementSequence ||
            ProgramEventHistory<MobileDuanfaIssuedEvent>().Count(e => e == new MobileDuanfaIssuedEvent(f.Id, r.Source, r.Hash, r.Phase)) != 1)
            throw new InvalidOperationException("Mobile Duanfa lost its exact original source, actual Play phase or payment cursor.");
        if (r.Stage == MobileDuanfaStage.Cards) return;
        if (r.Cards.Count == 0 || r.Limit < 1 || r.SpentBefore < 0 || r.SpentBefore + r.Cards.Count > r.Limit ||
            ProgramEventHistory<MobileDuanfaPaidEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var paid] ||
            paid != new MobileDuanfaPaidEvent(f.Id, r.Source, r.Hash, r.Phase, r.Limit, r.SpentBefore, r.ActualCount, r.Before, r.After) ||
            _cardMovements.Count(m => r.Cards.Contains(m.CardId) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == MobileDuanfaDiscard &&
                m.From.OwnerSeat == f.OwnerSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame)) != r.ActualCount)
            throw new InvalidOperationException("Duanfa lost its exact paid black HE discard and phase budget invoice.");
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 82300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 82300)]
    private bool KeepPaidMobileDuanfa(ProgramSkillFrame f)
    { if (f.MobileDuanfa is not { Stage: not MobileDuanfaStage.Cards, ActualCount: > 0 }) return false; AssertMobileDuanfa(f); return true; }
    private bool MobileDuanfaObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileDuanfa is not { Stage: MobileDuanfaStage.CostChildren } r) return false; AssertMobileDuanfa(f);
        if (child is CardsMovedTriggerWindowFrame moved) return ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId &&
            moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId &&
            moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && r.Cards.Contains(m.CardId));
        if (child is HpChangedTriggerWindowFrame hp) return ProgramHpReturnMatches(f, hp) && hp.Change.Kind == HpChangeKind.Recovery;
        if (child is RecoveryReplacementFrame recovery) return RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion &&
            recovery.Attempt.Completion.MoveReason?.Value == MobileDuanfaDiscard;
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id &&
            (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }
    private ProgramSkillFrame? MobileDuanfaObserverRoot() => FindProgramObserverRoot(static (e, f, child) => e.MobileDuanfaObserverChild(f, child),
        static (e, index) => e.HasObserverSuffix(index, static (g, i) => g.IsMovementObserverEdge(i) || g.IsSameNameHandObserverEdge(i) || g.IsYanPuDyingExitEdge(i) ||
            g.TypedDamagePostEventEdge(g._resolutionStack[i - 1], g._resolutionStack[i]) || g.RecoveryReplacementFrameRidesOn(g._resolutionStack[i], g._resolutionStack[i - 1]),
            static (g, i, dying) => g.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 82300)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 82300)]
    private bool MobileDuanfaDyingProof() => ActiveDying is { } dying && MobileDuanfaObserverRoot() is { } root &&
        _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 82300)]
    private bool AdvanceMobileDuanfaObserver() => _pendingDecision is null && MobileDuanfaObserverRoot() is not null && TryAdvanceObserverSubtree(true);
}
