#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuShiOlRuntime
{
    internal bool HasOlLuShiJudgmentProof(ProgramSkillFrame f) => f.OlLuShi is { Operation: SkillProgramEffectOp.ResolveOlLuShiLeijie, Item: "slashes" } r && f.JudgmentOutcomeBindings is [var outcome] && outcome is { Name: "leijie-result", Suit: Suit.Spade, Rank: >= 2 and <= 9 } && outcome.TargetSeat == r.TargetSeat && _coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(f, outcome);
    internal bool OlLuShiMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.OlLuShi is not { Stage: OlLuShiStage.Children } r || effect?.Op != r.Operation || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            return false;
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<OlLuShiMovementEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid.Before != r.Before || paid.After != r.After || paid.TargetSeat != r.TargetSeat)
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).ToArray();
        return paid.Draw ? r.Draw is { } draw && draw.ActualDrawIds.Count == paid.Count && draw.SequenceBefore == r.Before && draw.SequenceAfter == r.After && draw.ActualDrawIds.All(id => moves.Count(m => m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.TargetSeat) && m.Reason.Value == "skill.ol.lushi.draw") == 1) : r.Cards.Count == r.RequiredCount && paid.Count == r.Cards.Count && r.Cards.Distinct().Count() == r.Cards.Count && r.Cards.All(id => moves.Count(m => m.CardId == id && (m.From == CardLocation.Hand(r.TargetSeat) || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) == CardLocation.Hand(r.TargetSeat)) && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m)) && m.Reason.Value == "skill.ol.zhuyan.discard") == 1);
    }

    internal bool OlLuShiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.OlLuShi is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == OlLuShiStage.NativeUse && use.OlLuShiLeijie is not null && IsOlLuShiLeijieNativeSlash(use);
        if (child is HappyQiaoHealthChoiceFrame health)
            return _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(health, f);
        if (child is IdentityAppearanceFrame appearance)
            return r.Item == "hp" && r.Stage is OlLuShiStage.Loss or OlLuShiStage.Children && appearance.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && appearance.Receipt.ParentFrameId == f.Id && appearance.Receipt.OwnerSeat == r.TargetSeat && appearance.Receipt.RequestedDeduction == r.Amount && appearance.Receipt.OriginalInstructionIndex == f.InstructionIndex;
        if (r.Stage != OlLuShiStage.Children)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } pending && OlLuShiMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return r.Item == "hp" && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == r.TargetSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program;
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Item == "hp" && hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == r.TargetSeat && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind is HpChangeKind.Loss or HpChangeKind.Recovery;
        if (child is DyingFrame dying)
            return r.Item == "hp" && dying.ParentFrameId == f.Id && dying.VictimSeat == r.TargetSeat && dying.Continuation == DyingContinuationKind.ProgramSkill && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.TargetSeat == r.TargetSeat && e.Amount == r.Amount && e.RemainingHp <= 0);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? OlLuShiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.OlLuShiFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 42700)]
    internal bool OlLuShiDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && OlLuShiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 42700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 42700)]
    internal bool OlLuShiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.OlLuShi is null)
            return false;
        var root = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return root >= 0 && child > root && OlLuShiFirstChild(f, _resolutionStackOperations._resolutionStack[root + 1]) && (child == root + 1 || Enumerable.Range(root + 2, child - root - 1).All(i => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i))) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 42700)]
    internal bool AdvanceOlLuShiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && OlLuShiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
