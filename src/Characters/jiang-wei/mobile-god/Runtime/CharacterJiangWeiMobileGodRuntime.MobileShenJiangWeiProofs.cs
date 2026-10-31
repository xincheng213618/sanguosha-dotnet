#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMobileGodRuntime
{
    internal bool MobileJwParent(ProgramSkillFrame f)
    {
        var r = f.MobileJiangWei!;
        if (r.Operation == SkillProgramEffectOp.ResolveMobileJiangWeiXinghun)
            return f.TriggerId is null && f.WindowContext is null && r.ParentId == 0 && f.ActivationId == "five-stars";
        if (f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || w.ParentFrameId != r.ParentId || f.ActivationId != f.TriggerId)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1].Id != r.ParentId)
            return false;
        var candidate = _resolutionStackOperations._resolutionStack[i - 1] switch
        {
            ProgramLifecycleTriggerWindowFrame p when p.Window == w.Window && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count => p.Candidates[p.CandidateIndex],
            TurnEndingBoundaryFrame p when p.ItemIndex >= 0 && p.ItemIndex < p.Items.Count => p.Items[p.ItemIndex].Candidate,
            _ => null
        };
        return candidate is not null && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
    }
    internal bool MobileJwMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (f.MobileJiangWei is not { } r || effect?.Op != r.Operation || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null || r.Stage is not (MobileJiangWeiStage.ExchangeChildren or MobileJiangWeiStage.ReserveChildren or MobileJiangWeiStage.ReturnChildren or MobileJiangWeiStage.DiscardChildren or MobileJiangWeiStage.RecoveryChildren or MobileJiangWeiStage.GrantChildren) || !r.Paid.SequenceEqual(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After)) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileJiangWeiPaidEvent>().Count(e => e == new MobileJiangWeiPaidEvent(f.Id, r.PaymentIndex, r.Before, r.After, r.Paid.Count)) != 1)
            return false;
        return r.Paid.All(m => r.Stage switch
        {
            MobileJiangWeiStage.ExchangeChildren => m.Reason.Value == "mobile.xinghun.exchange" && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.DrawPile),
            MobileJiangWeiStage.ReserveChildren => m.Reason.Value == "mobile.xinghun.show" && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && r.Shown.Any(s => s.Id == m.CardId && s.Kind == m.CardKind && s.From == m.From),
            MobileJiangWeiStage.ReturnChildren => m.Reason.Value == "mobile.xinghun.return" && m.From == CardLocation.Processing && m.To == CardLocation.DrawPile && r.Shown.Any(s => s.Id == m.CardId && s.From == CardLocation.DrawPile),
            MobileJiangWeiStage.DiscardChildren => m.Reason.Value == "mobile.tiantao.discard" && m.From == new CardLocation(r.Region, r.Target) || m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(r.Target) && r.Paid.Any(x => x.From == CardLocation.Equipment(r.Target) && x.CardKind == CardKind.WoodenOx),
            _ => false
        });
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 40600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 40600)]
    internal bool MobileJwRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.MobileJiangWei is not { } r || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement)
            return false;
        if (r.Stage == MobileJiangWeiStage.RecoveryChildren)
            return recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == r.Amount && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == f.InstructionIndex;
        return r.Stage == MobileJiangWeiStage.DiscardChildren && r.Paid.Any(m => m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(r.Target)) && recovery.Attempt.TargetSeat == r.Target && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion;
    }

    internal bool IsMobileJwShenpeiDamage(ProgramSkillFrame f, int target, int amount, ProgramParticipantReference? source, DamageNature? nature, bool sourceLess, int? explicitSource)
    {
        if (f.MobileJiangWei is not { Operation: SkillProgramEffectOp.ResolveMobileJiangWeiShenpei, Stage: MobileJiangWeiStage.ThunderIssued } r || f.AttackAttempt is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || target != r.Target || amount != r.Amount || source is not null || nature != DamageNature.Thunder || sourceLess || explicitSource is not null || _coreDyingCursorRuntimeOperations.Value.ActiveDying?.Id != r.DyingId || f.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering } w || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame { Continuation: ProgramLifecycleContinuation.ResumeDyingEntry } entry || entry.ResumeDyingFrameId != r.DyingId)
            return false;
        return true;
    }

    internal bool IsMobileJwSuspendedDying(long attackId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileJiangWei: { Operation: SkillProgramEffectOp.ResolveMobileJiangWeiShenpei, Stage: MobileJiangWeiStage.ThunderIssued } r, AttackAttempt: { Nature: DamageNature.Thunder, SourceLess: false } a } f || f.Id != attackId || a.SourceSeat != f.OwnerSeat || _coreDyingCursorRuntimeOperations.Value.ActiveDying?.Id != r.DyingId || f.AttackReturn is null)
            return false;
        return true;
    }

    internal bool MobileJwFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileJiangWei is not { } r)
            return false;
        if (child is CardUseFrame use)
            return use.MobileJiangWeiReturn is { } ret && r.Use == ret && ret.ParentId == f.Id && r.Stage == MobileJiangWeiStage.SlashIssued && use.Action?.ActionId == ret.ActionId;
        if (child is RecoveryReplacementFrame recovery)
            return MobileJwRecovery(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return r.Stage == MobileJiangWeiStage.GrantChildren && changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && MobileJwMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(r.Paid.Contains);
        return r.Stage == MobileJiangWeiStage.ThunderIssued && f.AttackAttempt is not null && f.AttackReturn is not null && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
    }

    internal ProgramSkillFrame? MobileJwObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.MobileJwFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 40600)]
    internal bool MobileJwDyingProof()
    {
        if (_coreDyingCursorRuntimeOperations.Value.ActiveDying is not { } dying)
            return false;
        var root = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.MobileJiangWei is { } r && r.DyingId == dying.Id);
        if (root is not null)
        {
            return true;
        }

        return MobileJwObserverRoot()is { } observer && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == observer.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 40600)]
    internal bool AdvanceMobileJwSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MobileJwObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
