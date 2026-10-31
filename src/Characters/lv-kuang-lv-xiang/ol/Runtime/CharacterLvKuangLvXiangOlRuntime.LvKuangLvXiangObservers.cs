#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKuangLvXiangOlRuntime
{
    internal bool QigongPaidAdapterChild(ProgramSkillFrame f, ResolutionFrame child) => child switch
    {
        DrawFundedDistinctBasicFrame draw => QigongAdapterActor(f, draw.Payment.Source.OwnerSeat, draw.Payment.TargetSeat, draw.Payment.Cursor, draw.Payment.ParentFrameId, draw.Payment.RequestFrameId) && _coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(draw.Payment) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DrawFundedDistinctBasicPaidEvent>().Count(e => e.PaymentFrameId == draw.Id) == 1,
        ChainedStateBasicFrame chain => QigongAdapterActor(f, chain.Payment.Source.OwnerSeat, chain.Payment.TargetSeat, chain.Payment.Cursor, chain.Payment.ParentFrameId, chain.Payment.RequestFrameId) && _coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(chain.Payment) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChainedStateBasicPaidEvent>().Count(e => e.Payment == chain.Payment) == 1,
        _ => false
    };
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 360)]
    internal void AssertLvKuangLvXiang(ProgramSkillFrame f)
    {
        if (f.LvKuangLvXiang is not { } r)
            return;
        if (r.Source != LvKuangLvXiangSource(f) || r.GameplayHash != f.GameplayHash || r.Instruction != f.InstructionIndex || r.Turn != _coreActualTurnProgressTurnNumberAccess._turnNumber || r.Stage is not ("receive" or "receive-children" or "give" or "give-children" or "actor" or "slash" or "issuing" or "native" or "declaration") || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LvKuangLvXiangStartedEvent>().Count(e => e == new LvKuangLvXiangStartedEvent(f.Id, r.Source, r.GameplayHash, r.Instruction, r.Turn, r.Target, r.OriginalUseId, r.OriginalActionId)) != 1)
            throw new InvalidOperationException("The relay lost its exact source, current instruction or original native occurrence.");
        if (r.OriginalUseId is { } original)
        {
            if (f.TriggerId is not { } trigger || f.WindowContext is not { } w || !QigongOriginal(new(f.OwnerSeat, f.SkillId, trigger, f.SkillInstanceId, f.GameplayHash, 0), w, out var native, false) || native.Id != original || native.Action?.ActionId != r.OriginalActionId || r.Payments.Count != 0 || r.Actor >= 0 && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Actor) || r.Actor == r.Target))
                throw new InvalidOperationException("Qigong no longer owns the actual sole target's completed real Dodge occurrence.");
        }
        else if (f.TriggerId is not null || f.WindowContext is not null || !f.SelectedTargetSeats.SequenceEqual([r.Target]) || r.Target == f.OwnerSeat || r.Payments.Count > 2)
            throw new InvalidOperationException("Liehou lost its original giver and actual activation.");
        for (var i = 0; i < r.Payments.Count; i++)
        {
            var handPayment = r.Payments[i];
            if (handPayment.From != CardLocation.Hand(i == 0 ? r.Target : f.OwnerSeat) || handPayment.Recipient == (i == 0 ? r.Target : f.OwnerSeat) || i == 0 && handPayment.Recipient != f.OwnerSeat || i == 1 && (handPayment.Recipient == r.Target || handPayment.Recipient == f.OwnerSeat) || handPayment.Before > handPayment.After || handPayment.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || handPayment.BatchId <= 0 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == handPayment.CardId && m.From == handPayment.From && m.To == CardLocation.Hand(handPayment.Recipient) && m.Sequence > handPayment.Before && m.Sequence <= handPayment.After && m.Reason.Value == LiehouReason) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LiehouHandPaidEvent>().Count(e => e == new LiehouHandPaidEvent(f.Id, r.Source, r.GameplayHash, i + 1, handPayment.From.OwnerSeat!.Value, handPayment.Recipient, handPayment.Before, handPayment.After, handPayment.BatchId)) != 1)
                throw new InvalidOperationException("Liehou needs each exact actual once-paid hand transfer.");
        }

        if (r.Stage is "receive-children" or "give-children" && f.PendingMovementContinuation is not { SubjectSeat: var seat, BeforeCount: 0, CoverageResultBind: null } || r.Stage is "receive-children" or "give-children" && f.PendingMovementContinuation!.SubjectSeat != f.OwnerSeat || r.Stage == "receive-children" && r.Payments.Count != 1 || r.Stage == "give-children" && r.Payments.Count != 2)
            throw new InvalidOperationException("Liehou's child lost its native hand payment return.");
        if (r.Native is { } issued)
        {
            var live = _coreCardUseLifecycleOperations.LifecycleCardUse(issued.UseId);
            if (!r.Returned ? (live is null || !ValidQigongNativeUse(live)) : live is not null || !issued.Paid || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QigongNativeReturnedEvent>().Count(e => e.ProgramId == f.Id && e.UseId == issued.UseId && e.ActionId == issued.ActionId) != 1)
                throw new InvalidOperationException("Qigong lost its typed native card-use return.");
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } p && r.Stage is "receive" or "give" or "actor" or "slash" && !LvKuangLvXiangDecisionMatches(f, p))
            throw new InvalidOperationException($"The relay's exact chooser or candidate list changed (stage={(r.Stage)}, actor={(r.Actor)}, prompt={(p.Kind)}, chooser={(p.PlayerSeat)}, faction={(r.Faction is not null)}).");
    }

    internal bool LvKuangLvXiangDecisionMatches(ProgramSkillFrame f, PendingDecision prompt)
    {
        var native = _coreRoundOnceCardlessProgramRequestsOperations.RoundOnceCardlessProgramNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineChainedStateBasicConversionsRuntime.ChainedStateBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineDrawFundedDistinctBasicOffersRuntime.DrawFundedDistinctBasicNativeDecision(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicOwnershipRuntime.RequestedDeckBasicNativeDecision(global::CardGame.Core.EngineOwners.CharacterGuoTuOlRuntime.WeijieNativeDecision(prompt)))));
        if (f.LvKuangLvXiang is { Stage: "slash", Faction: not null } && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true } request)
        {
            if (!ValidateQigongFactionSlashParent(request, f) || prompt.Kind != DecisionKind.RespondSlash || prompt.PlayerSeat != request.CurrentCandidateSeat || prompt.SourceSeat != request.OwnerSeat || prompt.TargetSeat != request.OwnerSeat || prompt.IncomingCard != CardKind.Slash || prompt.RequiredCardKind != CardKind.Slash || !prompt.IsPrivate || prompt.ValidTargetSeats.Count != 0)
                return false;
            var provider = _coreMatchParticipantsPlayersAccess._players[request.CurrentCandidateSeat];
            return provider.IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider) == request.ProviderFactionId && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(native.Choices, _coreNativeFactionSlashOperations.CreateFactionSlashResponseChoices(provider, request, _coreCardConversionsOperations.GetFactionSlashSlashCards(request, provider)));
        }

        var choices = LvKuangLvXiangChoices(f);
        return prompt.Kind == DecisionKind.ProgramTrigger && prompt.PlayerSeat == LvChooser(f) && prompt.IsPrivate && prompt.ValidTargetSeats.SequenceEqual(LvKuangLvXiangValidTargets(f, choices)) && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(native.Choices, choices);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 320)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 320)]
    internal bool CanContinueLvKuangLvXiang(ProgramSkillFrame f)
    {
        if (f.LvKuangLvXiang is not { } r)
            return false;
        var paid = r.Payments.Count > 0 || r.Native is { Paid: true } || r.Declaration is not null && _resolutionStackOperations._resolutionStack.OfType<CardDeclarationFrame>().Any(d => MatchesQigongDeclaration(f, d)) || _resolutionStackOperations._resolutionStack.Any(child => QigongPaidAdapterChild(f, child));
        if (!paid)
            return false;
        AssertLvKuangLvXiang(f);
        return true;
    }

    internal bool LvKuangLvXiangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.LvKuangLvXiang is not { } r)
            return false;
        if (_characterLiuFengBoundaryOperations.BoundaryLiuFengRequestChild(f, child))
            return true;
        if (child is CardUseFrame use)
            return QigongCardUseRidesOn(use, f);
        if (IsQigongDeclarationChild(f, child) || QigongPaidAdapterChild(f, child))
            return true;
        if (!CanContinueLvKuangLvXiang(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved && r.Payments.LastOrDefault()is { } p)
            return moved.Batch.Id == p.BatchId && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements is [var m] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.CardId == p.CardId && m.From == p.From && m.To == CardLocation.Hand(p.Recipient) && m.Sequence > p.Before && m.Sequence <= p.After && m.Reason.Value == LiehouReason;
        return child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool LvKuangLvXiangObserverSuffix(int index)
    {
        if (index + 1 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[index + 1] is DrawFundedDistinctBasicFrame)
            return _coreDrawFundedDistinctBasicObserversOperations.DrawFundedDistinctBasicPaidSuffix(index + 1);
        if (index + 1 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[index + 1] is ChainedStateBasicFrame)
            return _coreChainedStateBasicReturnsOperations.ChainedStateBasicPaidSuffix(index + 1);
        return _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterLvKuangLvXiangOlOperations.QigongDeclarationObserverEdge(i), (i, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, d) || (_coreDrawFundedDistinctBasicReturnsOperations2.DrawFundedDistinctBasicDyingRescueRide(i, d) || _characterHuanShuJunOlOperations2.HuanpeiDyingRescueRide(i, d)) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(i, d));
    }

    internal ProgramSkillFrame? LvKuangLvXiangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLvKuangLvXiangOlOperations.LvKuangLvXiangFirstChild(f, c), (i) => _characterLvKuangLvXiangOlOperations.LvKuangLvXiangObserverSuffix(i));
    internal bool HasLvKuangLvXiangCardObserver(long originalUseId)
    {
        if (LvKuangLvXiangObserverRoot()is not { LvKuangLvXiang: { OriginalUseId: { } id, OriginalActionId: { } action }, WindowContext: { } w, TriggerId: { } binding } root || id != originalUseId || !QigongOriginal(new(root.OwnerSeat, root.SkillId, binding, root.SkillInstanceId, root.GameplayHash, 0), w, out var original, false) || original.Id != id || original.Action?.ActionId != action)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
        return index > 1 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramCardTriggerWindowFrame parent && parent.Id == w.ParentFrameId && parent.ParentFrameId == originalUseId && _resolutionStackOperations._resolutionStack[index - 2].Id == originalUseId;
    }

    internal bool IsQigongCompletedAttackProcessing(global::CardGame.Core.NativeOwners.CardAttackHandle attack, IReadOnlyList<Card> processing)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Step: ResolutionFrameStep.Completed, Qigong: { Paid: true } receipt, Action: { } action } use || !ValidQigongNativeUse(use) || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault()is not { Continuation: ProgramCardContinuation.CompletedSlash } window || window.ParentFrameId != use.Id || !global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(window.Action, action) || !attack.PhysicalCards.Select(c => c.Id).SequenceEqual(action.PhysicalCards.Select(c => c.CardId)) || attack.PhysicalCards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Processing) || !HasLvKuangLvXiangCardObserver(receipt.OriginalUseId) || _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.OriginalUseId)is not { Action: { } original })
            return false;
        var retained = original.PhysicalCards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing).Select(c => c.CardId).ToHashSet();
        return processing.Select(c => c.Id).ToHashSet().SetEquals(retained) && processing.Count == retained.Count;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 280)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 390)]
    internal bool HasLvKuangLvXiangDamageObserver(long id) => LvKuangLvXiangObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 480)]
    internal bool HasLvKuangLvXiangBeforeDamageObserver(long id) => LvKuangLvXiangObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 250)]
    internal bool IsLvKuangLvXiangDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && LvKuangLvXiangObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 390)]
    internal bool TryAdvanceLvKuangLvXiangSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LvKuangLvXiangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
