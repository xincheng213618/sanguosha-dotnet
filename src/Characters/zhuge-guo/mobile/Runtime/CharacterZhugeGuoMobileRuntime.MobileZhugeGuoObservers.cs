#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhugeGuoMobileRuntime
{
    internal void AssertMobileQirangGain(ProgramSkillFrame f)
    {
        var r = f.MobileQirangGain ?? throw new InvalidOperationException("Mobile Qirang lost its paid receipt.");
        if (f.SkillId != MobileQirang || f.TriggerId != "equipment-entry" || f.InstructionIndex != 1 || r.Source != MobileZhugeGuoSource(f) || r.Hash != f.GameplayHash || f.WindowContext?.MovementBatch is not { } batch || batch.Id != r.EquipmentBatchId || !batch.Movements.Any(m => m.Sequence == r.EquipmentSequence && m.CardId == r.EquipmentCardId && m.To == CardLocation.Equipment(f.OwnerSeat) && m.From != m.To) || r.Before < r.EquipmentSequence || r.Before >= r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.GainSequence <= r.Before || r.GainSequence > r.After || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(r.Kind) != SkillProgramCardCategory.Trick || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileQirangGainPaidEvent>().Count(e => e.FrameId == f.Id && e.Receipt == r) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == r.GainSequence && m.CardId == r.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MobileQirangGainReason) != 1)
            throw new InvalidOperationException("Mobile Qirang lost its actual entering equipment, exact source or once-paid deck trick movement.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 81800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 81800)]
    internal bool KeepMobileQirangGain(ProgramSkillFrame f)
    {
        if (f.MobileQirangGain is null)
            return false;
        AssertMobileQirangGain(f);
        return true;
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 81800)]
    internal CardSnapshot[] MobileYuhuaPrivateView(int viewer) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { SkillId: MobileYuhua, TopReorder: { } top } f && f.OwnerSeat == viewer && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTopReorder, PlayerSeat: var chooser } && chooser == viewer ? top.ViewedCardIds.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(c => c.Id == id))).ToArray() : [];
    internal bool MobileQirangObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileQirangGain is not { } r)
            return false;
        AssertMobileQirangGain(f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.CardId && m.Reason.Value == MobileQirangGainReason);
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f);
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id && (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }

    internal ProgramSkillFrame? MobileQirangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MobileQirangObserverChild(f, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 81800)]
    internal bool MobileQirangDyingProof() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MobileQirangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 81800)]
    internal bool AdvanceMobileQirangObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MobileQirangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
