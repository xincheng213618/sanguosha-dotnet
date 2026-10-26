namespace CardGame.Core;

public sealed partial class GameEngine
{
    private void AssertMobileQirangGain(ProgramSkillFrame f)
    {
        var r = f.MobileQirangGain ?? throw new InvalidOperationException("Mobile Qirang lost its paid receipt.");
        if (f.SkillId != MobileQirang || f.TriggerId != "equipment-entry" || f.InstructionIndex != 1 || r.Source != MobileZhugeGuoSource(f) || r.Hash != f.GameplayHash ||
            f.WindowContext?.MovementBatch is not { } batch || batch.Id != r.EquipmentBatchId ||
            !batch.Movements.Any(m => m.Sequence == r.EquipmentSequence && m.CardId == r.EquipmentCardId && m.To == CardLocation.Equipment(f.OwnerSeat) && m.From != m.To) ||
            r.Before < r.EquipmentSequence || r.Before >= r.After || r.After > _movementSequence || r.GainSequence <= r.Before || r.GainSequence > r.After ||
            GetProgramCardCategory(r.Kind) != SkillProgramCardCategory.Trick ||
            ProgramEventHistory<MobileQirangGainPaidEvent>().Count(e => e.FrameId == f.Id && e.Receipt == r) != 1 ||
            _cardMovements.Count(m => m.Sequence == r.GainSequence && m.CardId == r.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MobileQirangGainReason) != 1)
            throw new InvalidOperationException("Mobile Qirang lost its actual entering equipment, exact source or once-paid deck trick movement.");
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 81800)]
    private void AssertMobileZhugeGuo(ProgramSkillFrame f)
    {
        if (f.MobileQirangGain is not null) AssertMobileQirangGain(f);
        if (f.SkillId != MobileYuhua || f.TopReorder is not { } top) return;
        if (f.TriggerId != "ending-category-top" || f.InstructionIndex != 1 || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } c ||
            ProgramEventHistory<MobileYuhuaPeekStartedEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var peek] ||
            peek.Source != MobileZhugeGuoSource(f) || peek.Hash != f.GameplayHash || peek.EndingId != c.ParentFrameId || peek.HandCount <= peek.Hp ||
            peek.Requested is < 1 or > 3 || peek.ActualCount > peek.Requested || top.ViewedCardIds.Count != peek.ActualCount ||
            top.ViewedCardIds.Distinct().Count() != top.ViewedCardIds.Count || top.TopCardIds.Concat(top.BottomCardIds).Distinct().Count() != top.TopCardIds.Count + top.BottomCardIds.Count ||
            top.TopCardIds.Concat(top.BottomCardIds).Any(id => !top.ViewedCardIds.Contains(id)))
            throw new InvalidOperationException("Mobile Yuhua lost its original hand-category quota or private native top/bottom partition.");
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 81800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 81800)]
    private bool KeepMobileQirangGain(ProgramSkillFrame f)
    { if (f.MobileQirangGain is null) return false; AssertMobileQirangGain(f); return true; }
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 81800)]
    private CardSnapshot[] MobileYuhuaPrivateView(int viewer) => _resolutionStack.LastOrDefault() is ProgramSkillFrame
        { SkillId: MobileYuhua, TopReorder: { } top } f && f.OwnerSeat == viewer && _pendingDecision is { Kind: DecisionKind.ProgramTopReorder, PlayerSeat: var chooser } && chooser == viewer
            ? top.ViewedCardIds.Select(id => ToSnapshot(_cardZones.CardsAt(CardLocation.DrawPile).Single(c => c.Id == id))).ToArray() : [];
    private bool MobileQirangObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileQirangGain is not { } r) return false; AssertMobileQirangGain(f);
        if (child is CardsMovedTriggerWindowFrame moved) return ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat &&
            moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId &&
            moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.CardId && m.Reason.Value == MobileQirangGainReason);
        if (child is HpChangedTriggerWindowFrame hp) return ProgramHpReturnMatches(f, hp);
        if (child is RecoveryReplacementFrame recovery) return RecoveryReplacementFrameRidesOn(recovery, f);
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id &&
            (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }
    private ProgramSkillFrame? MobileQirangObserverRoot() => FindProgramObserverRoot(static (e, f, child) => e.MobileQirangObserverChild(f, child),
        static (e, index) => e.HasObserverSuffix(index, static (g, i) => g.IsMovementObserverEdge(i) || g.IsSameNameHandObserverEdge(i) || g.IsYanPuDyingExitEdge(i) ||
            g.TypedDamagePostEventEdge(g._resolutionStack[i - 1], g._resolutionStack[i]) || g.RecoveryReplacementFrameRidesOn(g._resolutionStack[i], g._resolutionStack[i - 1]),
            static (g, i, dying) => g.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 81800)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 81800)]
    private bool MobileQirangDyingProof() => ActiveDying is { } dying && MobileQirangObserverRoot() is { } root &&
        _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 81800)]
    private bool AdvanceMobileQirangObserver() => _pendingDecision is null && MobileQirangObserverRoot() is not null && TryAdvanceObserverSubtree(true);
}
