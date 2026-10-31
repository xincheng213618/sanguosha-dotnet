#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoTuOlRuntime
{
    internal bool WeijieNativeNeedCurrent(WeijieReceipt r)
    {
        var original = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == r.Need.RequestFrameId);
        return original is not null && CaptureWeijieNeed(r.OriginalDecision, original)is { } n && SameWeijieNeed(n, r.Need);
    }

    internal bool ValidWeijiePayment(WeijiePaidOrigin p)
    {
        if (p.AttemptId <= p.Need.RequestFrameId || p.Source.SkillId != "ol:weijie" || p.Source.BindingId != "native-basic-request" || p.Source.OwnerSeat != p.Need.ProviderSeat || string.IsNullOrWhiteSpace(p.Source.SkillInstanceId) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(p.Source.SkillId)?.Program is not { } program || program.GameplayHash != p.GameplayHash || p.ActualTurn <= 0 || p.ActualTurnOwner == p.Source.OwnerSeat || p.From != CardLocation.Hand(p.VictimSeat) || p.PaymentSequence <= 0 || p.PaymentBatchId <= 0)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieCostPaidEvent>().Count(e => e == new WeijieCostPaidEvent(p.AttemptId, p.Source, p.GameplayHash, p.ActualTurn, p.ActualTurnOwner, p.VictimSeat, p.CardId, p.PrintedKind, p.EffectiveName, p.From, p.PaymentSequence, p.PaymentBatchId)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieCostPaidEvent>().Count(e => e.Source.OwnerSeat == p.Source.OwnerSeat && e.ActualTurn == p.ActualTurn && e.ActualTurnOwner == p.ActualTurnOwner) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == p.PaymentSequence && m.CardId == p.CardId && m.From == p.From && m.To == CardLocation.DiscardPile && m.Reason.Value == WeijieDiscardReason) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieDeclaredEvent>().Count(e => e.FrameId == p.AttemptId && e.OutputKind == p.OutputKind && e.TargetSeat == p.TargetSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieVictimSelectedEvent>().Count(e => e.FrameId == p.AttemptId && e.VictimSeat == p.VictimSeat) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieStartedEvent>().Count(e => e.FrameId == p.AttemptId && e.Source == p.Source && e.GameplayHash == p.GameplayHash && e.ActualTurn == p.ActualTurn && e.ActualTurnOwner == p.ActualTurnOwner && e.Intent == p.Need.Intent && e.OwnerFrameId == p.Need.OwnerFrameId && e.RequestFrameId == p.Need.RequestFrameId) == 1;
    }

    internal bool WeijieParent(ProgramSkillFrame f)
    {
        if (f.Weijie is not { } r)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != r.Need.RequestFrameId)
            return false;
        var native = _resolutionStackOperations._resolutionStack[index - 1];
        return native is ResponseWindowFrame response ? response.ParentFrameId == r.Need.OwnerFrameId : native.Id == r.Need.OwnerFrameId && native is CardUseFrame or ProgramSkillFrame or DyingFrame;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27462)]
    internal void AssertWeijie(ProgramSkillFrame f)
    {
        if (f.Weijie is not { } r)
            return;
        if (f.InstructionIndex is < 0 or > 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.CardSetBindings.Count != 0 || f.ChoiceBindings.Count != 0 || f.NumberBindings.Count != 0 || f.PindianResultBindings.Count != 0 || f.WeijieReturn is not null || r.Source != WeijieSource(f) || r.Source.OwnerSeat != r.Need.ProviderSeat || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 0 && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveWeijieRequiredBasicRequest || !WeijieParent(f) || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber || r.ActualTurnOwner != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !WeijieTurnOpen(f.OwnerSeat) || r.OriginalDecision.PlayerSeat != f.OwnerSeat || r.OriginalDecision.PromptId.Value <= 0 || r.OriginalDecision.Revision < 0 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieStartedEvent>().Count(e => e == new WeijieStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.ActualTurnOwner, r.Need.Intent, r.Need.OwnerFrameId, r.Need.RequestFrameId, r.OriginalDecision.PromptId, r.OriginalDecision.Revision, WeijieDecisionHash(r.OriginalDecision))) != 1 || !r.OriginalDecision.Choices.Any(c => c.Parameters.GetValueOrDefault(WeijieOpen) == "open" && global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(c.Parameters, out var s) && s == r.Source))
            throw new InvalidOperationException("Weijie lost its registered owning request, exact original native prompt, source hash or actual foreign turn.");
        if (r.Stage != WeijieStage.Declare && (r.OutputKind is not { } kind || r.TargetSeat is not { } target || _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieDeclaredEvent>().Count(e => e == new WeijieDeclaredEvent(f.Id, kind, target)) != 1))
            throw new InvalidOperationException("Weijie changed its once-declared basic name or target.");
        if (r.Stage is WeijieStage.Card or WeijieStage.CostChildren && (r.VictimSeat is not { } victim || _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieVictimSelectedEvent>().Count(e => e == new WeijieVictimSelectedEvent(f.Id, victim)) != 1))
            throw new InvalidOperationException("Weijie changed its selected foreign hand owner.");
        if (r.Stage == WeijieStage.CostChildren)
        {
            if (r.Paid is not { } paid || !ValidWeijiePayment(paid) || paid.AttemptId != f.Id || !SameWeijieNeed(paid.Need, r.Need) || paid.Source != r.Source || paid.GameplayHash != r.GameplayHash || paid.OutputKind != r.OutputKind || paid.TargetSeat != r.TargetSeat || paid.VictimSeat != r.VictimSeat || paid.PaymentSequence <= r.Before || paid.PaymentSequence > r.After || f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != f.OwnerSeat || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).Any(m => m.Sequence != paid.PaymentSequence))
                throw new InvalidOperationException("Weijie lost its one actual opaque Hand discard, once-paid source or exact movement interval.");
        }
        else if (r.Paid is not null || f.PendingMovementContinuation is not null || r.Before != 0 || r.After != 0)
            throw new InvalidOperationException("Unpaid Weijie cannot claim a payment or pending movement.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } p && (p.PlayerSeat != f.OwnerSeat || p.SkillPrompt?.SkillId != f.SkillId || !p.IsPrivate || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, WeijieChoices(f))))
            throw new InvalidOperationException("Weijie changed its private native request frontier.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 400)]
    internal bool IsWeijieMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.Weijie is { Stage: WeijieStage.CostChildren, Paid: not null } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && effect?.Op == SkillProgramEffectOp.ResolveWeijieRequiredBasicRequest;
    internal bool WeijieFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Weijie is not { Stage: WeijieStage.CostChildren, Paid: { } p } r)
            return false;
        AssertWeijie(f);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed)
            return changed.ResumeProgramFrameId == f.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Id == p.PaymentBatchId && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements is [var m] && m.Sequence == p.PaymentSequence && m.CardId == p.CardId && m.From == p.From && m.To == CardLocation.DiscardPile && m.Reason.Value == WeijieDiscardReason;
    }

    internal ProgramSkillFrame? WeijieObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterGuoTuOlOperations.WeijieFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 910)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1020)]
    internal bool HasWeijieDamageObserver(long id) => WeijieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1050)]
    internal bool HasWeijieBeforeDamageObserver(long id) => WeijieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 880)]
    internal bool IsWeijieDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && (ActiveWeijieDraft()is { Weijie: { Need: { Intent: WeijieIntent.Dying } need } } draft && need.OwnerFrameId == d.Id && need.RequestFrameId == d.Id && WeijieParent(draft) || WeijieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27462)]
    internal bool AdvanceWeijieChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WeijieObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal ProgramSkillFrame? ActiveWeijieDraft() => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f => f.Weijie is not null);
    internal PendingDecision? WeijieInvariantDecision(PendingDecision? ordinary)
    {
        if (ActiveWeijieDraft()is not { } f)
            return ordinary is { } p ? WeijieNativeDecision(p) : null;
        AssertWeijie(f);
        return WeijieNativeDecision(f.Weijie!.OriginalDecision);
    }

    internal ResolutionFrame? WeijieInvariantTop(ResolutionFrame? ordinary)
    {
        if (ActiveWeijieDraft()is not { } f)
            return ordinary;
        AssertWeijie(f);
        return _resolutionStackOperations._resolutionStack.Single(x => x.Id == f.Weijie!.Need.RequestFrameId);
    }

    internal bool HasWeijieNativeRequestContinuation(long owner) => ActiveWeijieDraft()is { } f && f.Weijie!.Need.OwnerFrameId == owner && WeijieParent(f) && (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id || WeijieObserverRoot()?.Id == f.Id);
    internal bool HasWeijieQinglongPaymentRide(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        if (!_coreNativeDamageHandlesOperations.UsesFormalQinglongCrescentBlade || ActiveWeijieDraft()is not { Weijie: { Stage: WeijieStage.CostChildren, Paid: { } paid } r } f || r.Need.Intent != WeijieIntent.Qinglong || !ValidWeijiePayment(paid) || !SameWeijieNeed(paid.Need, r.Need) || paid.AttemptId != f.Id || !WeijieParent(f) || !WeijieNativeNeedCurrent(r) || _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not { } blade || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(blade.Attack, attack) || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || attack.SuccessfulDodgeResponses != attack.RequiredDodgeResponses || r.Need.OwnerFrameId != attack.ResolutionId || r.Need.RequestFrameId != attack.ResolutionId || r.Need.ActorSeat != attack.SourceSeat || r.Need.ProviderSeat != attack.SourceSeat || r.Need.Cursor != 0 || r.Source.OwnerSeat != attack.SourceSeat || r.TargetSeat != attack.TargetSeat || !r.Need.Targets.SequenceEqual([attack.TargetSeat]))
            return false;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Step: ResolutionFrameStep.AwaitingResponse, Action: { } action } use || use.SourceSeat != attack.SourceSeat || use.CardKind != attack.EffectiveCardKind || action.ActorSeat != attack.SourceSeat || action.EffectiveKind != use.CardKind || r.Need.ParentActionId != action.ActionId || r.OriginalDecision is not { Kind: DecisionKind.QinglongCrescentBlade, IsPrivate: true } original || original.PlayerSeat != attack.SourceSeat || original.SourceSeat != attack.SourceSeat || original.TargetSeat != attack.TargetSeat || original.IncomingCard != attack.EffectiveCardKind || !original.ValidTargetSeats.SequenceEqual([attack.TargetSeat]))
            return false;
        AssertWeijie(f);
        return _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id || WeijieObserverRoot()?.Id == f.Id;
    }

    internal void AssertWeijieRequests()
    {
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.Weijie is not null))
            AssertWeijie(f);
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.WeijieOrigin is not null))
            if (!IsIssuedWeijieUse(use))
                throw new InvalidOperationException("Weijie native use lost its exact once-paid request issuance.");
    }
}
