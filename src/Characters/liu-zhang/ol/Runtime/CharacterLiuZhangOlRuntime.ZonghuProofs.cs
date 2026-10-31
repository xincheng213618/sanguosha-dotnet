#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZhangOlRuntime
{
    internal bool ZonghuNativeNeedCurrent(ZonghuReceipt r)
    {
        if (r.Need.Intent == ZonghuIntent.Play)
            return r.Need.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && r.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber && r.ActualTurnOwner == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;
        var original = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.Need.RequestFrameId);
        if (original is null || CaptureZonghuNeed(r.OriginalDecision, original)is not { } n)
            return false;
        if (SameZonghuNeed(n, r.Need))
            return true;
        return r.Paid is not null && r.TargetSeat is { } target && r.Need.Targets.Contains(target) && n.Targets.Contains(target) && SameZonghuNeed(n with { Targets = r.Need.Targets }, r.Need);
    }

    internal bool ValidZonghuPayment(ZonghuPaidOrigin p)
    {
        if (p.AttemptId <= p.Need.RequestFrameId || p.Source.SkillId != "ol:zonghu" || p.Source.BindingId != "native-use-request" || p.Source.OwnerSeat != p.Need.ActorSeat || p.Need.ProviderSeat != p.Need.ActorSeat || p.Need.RequesterSeat is not null || string.IsNullOrWhiteSpace(p.Source.SkillInstanceId) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(p.Source.SkillId)?.Program is not { } program || program.GameplayHash != p.GameplayHash || p.ActualTurn <= 0 || p.RequiredCount is < 1 or > 3 || p.Gifts.Count != p.RequiredCount || p.Gifts.Select(g => g.CardId).Distinct().Count() != p.Gifts.Count || p.Before < 0 || p.After <= p.Before)
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuCostPaidEvent>().Where(e => e.Source.OwnerSeat == p.Source.OwnerSeat && e.ActualTurn == p.ActualTurn && e.ActualTurnOwner == p.ActualTurnOwner).ToArray()is not [var paid] || paid != new ZonghuCostPaidEvent(p.AttemptId, p.Source, p.GameplayHash, p.ActualTurn, p.ActualTurnOwner, p.RecipientSeat, p.RequiredCount, p.Before, p.After))
            return false;
        for (var i = 0; i < p.Gifts.Count; i++)
        {
            var g = p.Gifts[i];
            if (g.From != CardLocation.Hand(p.Source.OwnerSeat) && g.From != CardLocation.Equipment(p.Source.OwnerSeat) || g.Sequence <= p.Before || g.Sequence > p.After || g.BatchId <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuGiftSelectedEvent>().Count(e => e == new ZonghuGiftSelectedEvent(p.AttemptId, i, g.CardId)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuGiftPaidEvent>().Count(e => e == new ZonghuGiftPaidEvent(p.AttemptId, i, g.CardId, g.Kind, g.From, p.RecipientSeat, g.Sequence, g.BatchId)) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == g.Sequence && m.CardId == g.CardId && m.From == g.From && m.To == CardLocation.Hand(p.RecipientSeat) && m.Reason.Value == ZonghuGiftReason) != 1)
                return false;
        }

        return _coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuDeclaredEvent>().Count(e => e == new ZonghuDeclaredEvent(p.AttemptId, p.OutputKind, p.TargetSeat)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuRecipientSelectedEvent>().Count(e => e == new ZonghuRecipientSelectedEvent(p.AttemptId, p.RecipientSeat)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuStartedEvent>().Count(e => e.FrameId == p.AttemptId && e.Source == p.Source && e.GameplayHash == p.GameplayHash && e.ActualTurn == p.ActualTurn && e.ActualTurnOwner == p.ActualTurnOwner && e.Intent == p.Need.Intent && e.OwnerFrameId == p.Need.OwnerFrameId && e.RequestFrameId == p.Need.RequestFrameId) == 1;
    }

    internal bool ZonghuParent(ProgramSkillFrame f)
    {
        if (f.Zonghu is not { } r)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (r.Need.Intent == ZonghuIntent.Play)
            return index == 0 && r.Need.OwnerFrameId == 0 && r.Need.RequestFrameId == 0 && r.Need.ActorSeat == f.OwnerSeat;
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != r.Need.RequestFrameId)
            return false;
        var native = _resolutionStackOperations._resolutionStack[index - 1];
        return native is ResponseWindowFrame response ? response.ParentFrameId == r.Need.OwnerFrameId : native.Id == r.Need.OwnerFrameId && native is CardUseFrame or ProgramSkillFrame or DyingFrame;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27512)]
    internal void AssertZonghu(ProgramSkillFrame f)
    {
        if (f.Zonghu is not { } r)
            return;
        if (f.InstructionIndex is < 0 or > 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.CardSetBindings.Count != 0 || f.ChoiceBindings.Count != 0 || f.NumberBindings.Count != 0 || f.PindianResultBindings.Count != 0 || f.ZonghuReturn is not null || r.Source != ZonghuSource(f) || r.Source.OwnerSeat != r.Need.ProviderSeat || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 0 && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveZonghuRequiredUseRequest || !ZonghuParent(f) || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber || r.ActualTurnOwner != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !ZonghuTurnOpen(f.OwnerSeat) || r.OriginalDecision.PlayerSeat != f.OwnerSeat || r.OriginalDecision.PromptId.Value <= 0 || r.OriginalDecision.Revision < 0 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuStartedEvent>().Count(e => e == new ZonghuStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.ActualTurnOwner, r.Need.Intent, r.Need.OwnerFrameId, r.Need.RequestFrameId, r.OriginalDecision.PromptId, r.OriginalDecision.Revision, ZonghuDecisionHash(r.OriginalDecision))) != 1 || r.Need.Intent != ZonghuIntent.Play && !r.OriginalDecision.Choices.Any(c => c.Parameters.GetValueOrDefault(ZonghuOpen) == "open" && global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(c.Parameters, out var s) && s == r.Source))
            throw new InvalidOperationException("Zonghu lost its registered owning request, exact original native prompt, source hash or actual foreign turn.");
        if (r.Stage != ZonghuStage.Declare && (r.OutputKind is not { } kind || r.TargetSeat is not { } target || _coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuDeclaredEvent>().Count(e => e == new ZonghuDeclaredEvent(f.Id, kind, target)) != 1))
            throw new InvalidOperationException("Zonghu changed its once-declared basic name or target.");
        if (r.Stage is ZonghuStage.Card or ZonghuStage.CostChildren && (r.RecipientSeat is not { } victim || _coreProgramEventHistoryOperations.ProgramEventHistory<ZonghuRecipientSelectedEvent>().Count(e => e == new ZonghuRecipientSelectedEvent(f.Id, victim)) != 1))
            throw new InvalidOperationException("Zonghu changed its selected foreign hand owner.");
        if (r.Stage == ZonghuStage.CostChildren)
        {
            if (r.Paid is not { } paid || !ValidZonghuPayment(paid) || paid.AttemptId != f.Id || !SameZonghuNeed(paid.Need, r.Need) || paid.Source != r.Source || paid.GameplayHash != r.GameplayHash || paid.OutputKind != r.OutputKind || paid.TargetSeat != r.TargetSeat || paid.RecipientSeat != r.RecipientSeat || paid.Before != r.Before || paid.After > r.After || !paid.Gifts.Select(g => g.CardId).SequenceEqual(r.Selected) || f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != f.OwnerSeat || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).Any(m => !IsZonghuGiftMovement(f, m)))
                throw new InvalidOperationException("Zonghu lost its exact whole gift and independent native spill interval.");
        }
        else if (r.Paid is not null || f.PendingMovementContinuation is not null || r.Before != 0 || r.After != 0)
            throw new InvalidOperationException("Unpaid Zonghu cannot claim a native movement.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } p && (p.PlayerSeat != f.OwnerSeat || p.SkillPrompt?.SkillId != f.SkillId || !p.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, ZonghuChoices(f))))
            throw new InvalidOperationException("Zonghu changed its private native request frontier.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 450)]
    internal bool IsLiuZhangMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.Fengwei is { Stage: FengweiStage.DrawChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && effect?.Op == SkillProgramEffectOp.ResolveFengweiRoundStartDraw || f.Zonghu is { Stage: ZonghuStage.CostChildren, Paid: not null } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && effect?.Op == SkillProgramEffectOp.ResolveZonghuRequiredUseRequest;
    internal bool ZonghuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Zonghu is not { Stage: ZonghuStage.CostChildren, Paid: { } p } r)
            return false;
        AssertZonghu(f);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        if (child is RecoveryReplacementFrame recovery)
            return IsLiuZhangRecoveryParent(f, recovery);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsZonghuGiftMovement(f, m));
    }

    internal bool IsZonghuGiftMovement(ProgramSkillFrame f, CardMovementRecord m) => f.Zonghu is { Stage: ZonghuStage.CostChildren, Paid: { } paid } r && m.Sequence > r.Before && m.Sequence <= r.After && (paid.Gifts.Any(g => g.Sequence == m.Sequence && g.CardId == m.CardId && g.From == m.From && m.To == CardLocation.Hand(paid.RecipientSeat) && m.Reason.Value == ZonghuGiftReason) || r.GrainIds.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && paid.Gifts.Any(g => g.Kind == CardKind.WoodenOx && g.From == CardLocation.Equipment(f.OwnerSeat)));
    internal ProgramSkillFrame? ZonghuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLiuZhangOlOperations.ZonghuFirstChild(f, c) || _characterLiuZhangOlOperations.FengweiFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 960)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1070)]
    internal bool HasLiuZhangDamageObserver(long id) => ZonghuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1100)]
    internal bool HasLiuZhangBeforeDamageObserver(long id) => ZonghuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 930)]
    internal bool IsLiuZhangDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && (ActiveZonghuDraft()is { Zonghu: { Need: { Intent: ZonghuIntent.Dying } need } } draft && need.OwnerFrameId == d.Id && need.RequestFrameId == d.Id && ZonghuParent(draft) || ZonghuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27512)]
    internal bool AdvanceZonghuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZonghuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal ProgramSkillFrame? ActiveZonghuDraft() => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.Zonghu is not null);
    internal PendingDecision? ZonghuInvariantDecision(PendingDecision? ordinary)
    {
        if (ActiveZonghuDraft()is not { } f)
            return ordinary is { } p ? ZonghuNativeDecision(p) : null;
        AssertZonghu(f);
        return ZonghuNativeDecision(f.Zonghu!.OriginalDecision);
    }

    internal ResolutionFrame? ZonghuInvariantTop(ResolutionFrame? ordinary)
    {
        if (ActiveZonghuDraft()is not { } f)
            return ordinary;
        AssertZonghu(f);
        return f.Zonghu!.Need.Intent == ZonghuIntent.Play ? ordinary : _resolutionStackOperations._resolutionStack.Single(x => x.Id == f.Zonghu!.Need.RequestFrameId);
    }

    internal bool HasZonghuNativeRequestContinuation(long owner) => ActiveZonghuDraft()is { } f && f.Zonghu!.Need.OwnerFrameId == owner && ZonghuParent(f) && (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id || ZonghuObserverRoot()?.Id == f.Id);
    internal bool HasZonghuQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (!_coreNativeDamageHandlesOperations.UsesFormalQinglongCrescentBlade || ActiveZonghuDraft()is not { Zonghu: { Stage: ZonghuStage.CostChildren, Paid: { } paid } r } f || r.Need.Intent != ZonghuIntent.Qinglong || !ValidZonghuPayment(paid) || !SameZonghuNeed(paid.Need, r.Need) || paid.AttemptId != f.Id || !ZonghuParent(f) || !ZonghuNativeNeedCurrent(r) || _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not { } blade || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(blade.Attack, attack) || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || attack.SuccessfulDodgeResponses != attack.RequiredDodgeResponses || r.Need.OwnerFrameId != attack.ResolutionId || r.Need.RequestFrameId != attack.ResolutionId || r.Need.ActorSeat != attack.SourceSeat || r.Need.ProviderSeat != attack.SourceSeat || r.Need.Cursor != 0 || r.Source.OwnerSeat != attack.SourceSeat || r.TargetSeat != attack.TargetSeat || !r.Need.Targets.SequenceEqual([attack.TargetSeat]))
            return false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Step: ResolutionFrameStep.AwaitingResponse, Action: { } action } use || use.SourceSeat != attack.SourceSeat || use.CardKind != attack.EffectiveCardKind || action.ActorSeat != attack.SourceSeat || action.EffectiveKind != use.CardKind || r.Need.ParentActionId != action.ActionId || r.OriginalDecision is not { Kind: DecisionKind.QinglongCrescentBlade, IsPrivate: true } original || original.PlayerSeat != attack.SourceSeat || original.SourceSeat != attack.SourceSeat || original.TargetSeat != attack.TargetSeat || original.IncomingCard != attack.EffectiveCardKind || !original.ValidTargetSeats.SequenceEqual([attack.TargetSeat]))
            return false;
        AssertZonghu(f);
        return _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id || ZonghuObserverRoot()?.Id == f.Id;
    }

    internal void AssertZonghuRequests()
    {
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.Zonghu is not null))
            AssertZonghu(f);
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.ZonghuOrigin is not null))
            if (!IsIssuedZonghuUse(use))
                throw new InvalidOperationException("Zonghu native use lost its exact once-paid request issuance.");
    }
}
