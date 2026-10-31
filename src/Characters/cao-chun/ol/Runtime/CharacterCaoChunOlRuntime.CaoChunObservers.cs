#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoChunOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 40)]
    internal bool CaoChunFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasCaoChunCapability;
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 50)]
    internal bool CaoChunNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => HasCaoChunCapability;
    internal bool HasCaoChunCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.DrawAndRequestShanjiaSlash);

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 24500)]
    internal void ObserveAcquiredCaoChun(SkillsAcquiredEvent acquired)
    {
        foreach (var skillId in acquired.SkillIds)
            InitializeAcquiredCaoChun(acquired.PlayerSeat, skillId);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 24501)]
    internal void ObservePurchasedCaoChun(WushuangOfferBoughtEvent bought)
    {
        if (bought.SkillId is { } skillId)
            InitializeAcquiredCaoChun(bought.Seat, skillId);
    }

    internal void InitializeAcquiredCaoChun(int seat, string skillId)
    {
        if (!HasCaoChunCapability || _coreMatchParticipantsPlayersAccess._players[seat] is not { IsAlive: true, Shanjia: null } owner || _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program is not { } program)
            return;
        var trigger = program.Triggers.SingleOrDefault(t => ProgramInstructionResolver.Default.Features(t).HasOperation(SkillProgramEffectOp.InitializeShanjiaLossMarkers));
        if (trigger is null)
            return;
        var source = new CardConversionSource(skillId, trigger.Id, seat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, skillId));
        owner.Shanjia = new(source, program.GameplayHash, 3);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanjiaMarksChangedEvent(source, program.GameplayHash, 0, 3, 0));
    }

    internal bool HasIssuedCaoChunDebt
    {
        get
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                if (p.Shanjia?.Debt is not null)
                    return true;
            return false;
        }
    }

    internal static bool ShanjiaHeld(CardLocation location, int owner) => global::CardGame.Core.RuntimeSkills.RuntimeRules_cao_chun_ol_CaoChunObservers.ShanjiaHeld(location, owner);
    internal void ObserveCaoChunMovement(CardMovementBatchContext batch)
    {
        if (!HasCaoChunCapability)
            return;
        foreach (var group in batch.Movements.Where(m => m.From.OwnerSeat is { } owner && ShanjiaHeld(m.From, owner) && !ShanjiaHeld(m.To, owner) && CardCatalog.Get(m.CardKind).CategoryName == "装备牌").GroupBy(m => m.From.OwnerSeat!.Value))
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[group.Key];
            if (owner.Shanjia is not { } state || state.LossMarks == 0 || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, state.Source.SkillId, state.Source.SkillInstanceId) || _contentRegistryOperations._contentRegistry.GetSkill(state.Source.SkillId).Program?.GameplayHash != state.GameplayHash)
                continue;
            foreach (var movement in group.OrderBy(m => m.Sequence))
            {
                if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanjiaMarksChangedEvent>().Any(e => e.Source == state.Source && e.MovementSequence == movement.Sequence))
                    continue;
                var next = Math.Max(0, state.LossMarks - 1);
                owner.Shanjia = state = state with
                {
                    LossMarks = next
                };
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanjiaMarksChangedEvent(state.Source, state.GameplayHash, next + 1, next, movement.Sequence));
                if (next == 0)
                    break;
            }
        }
    }

    internal CaoChunHandUsePayment? CaptureCaoChunPaidHandUse(long nativeParentId, CardActionContext action, long before, long after, ProgramCompletedResponseKind? responseKind = null)
    {
        if ((!HasCaoChunCapability) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || action.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || action.ProviderSeat != action.ActorSeat || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[action.ActorSeat].Shanjia?.Debt is not { } debt || debt.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || debt.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || debt.Remaining <= 0 || before > after || after > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return null;
        if (action.Type == CardActionType.Use)
        {
            if (responseKind is not null || _coreCardUseLifecycleOperations.LifecycleCardUse(nativeParentId)is not { Action: { } actual } use || !global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(actual, action) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action))
                return null;
        }
        else if (responseKind is not { } kind || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(action, kind) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(e.Action, action)))
            return null;
        var hand = action.PhysicalCards.Where(c => c.From.OwnerSeat == action.ActorSeat && c.From.Zone is CardZoneKind.Hand or CardZoneKind.WoodenOxGrain).ToArray();
        if (hand.Length == 0 || hand.Select(c => c.CardId).Distinct().Count() != hand.Length)
            return null;
        var reason = action.Type == CardActionType.Response ? (responseKind == ProgramCompletedResponseKind.Nullification && action.PhysicalCards.Count == 1 ? CardMoveReasons.Nullification : CardMoveReasons.Respond) : action.EffectiveKind == CardKind.IronChain ? CardMoveReasons.IronChainUse : CardMoveReasons.Use;
        // Some native uses (equipment, delayed placement) retain their exact producer reason.
        bool Payment(CardMovementRecord m, CardActionCost c) => m.Sequence > before && m.Sequence <= after && m.CardId == c.CardId && m.CardKind == c.CardKind && m.From == c.From && (m.To == CardLocation.Processing || _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, c.CardId) && m.To == CardLocation.DrawPile) && (m.Reason == reason || action.Type == CardActionType.Use && CardCatalog.Get(action.EffectiveKind).CategoryName == "装备牌" && m.Reason == CardMoveReasons.EquipmentUse);
        if (hand.Any(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => Payment(m, c)) != 1))
            return null;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanjiaHandUseDebitedEvent>().Any(e => e.Source == debt.Source && e.ActionId == action.ActionId))
            return null;
        var proof = new CaoChunHandUsePayment(debt, nativeParentId, action, before, after, responseKind)
        {
            HandMaterials = hand,
            Payments = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => hand.Any(c => Payment(m, c))).ToArray()
        };
        if (action.Type == CardActionType.Use && _coreCardUseLifecycleOperations.LifecycleCardUse(nativeParentId)is { } native)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(nativeParentId, f => f with { CaoChunDebtQualification = proof });
        return proof;
    }

    internal bool ValidCaoChunPayment(CaoChunHandUsePayment p)
    {
        var d = p.Debt;
        if (d.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || d.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || d.Source.OwnerSeat != p.Action.ActorSeat || p.Action.ActorSeat != p.Action.ProviderSeat || p.HandMaterials.Count == 0 || p.Before > p.After || p.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(e.Action, p.Action)) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanjiaDebtArmedEvent>().Count(e => e.Source == d.Source && e.GameplayHash == d.GameplayHash && e.ActivationFrameId == d.ActivationFrameId && e.ActualTurn == d.ActualTurn && e.PhaseInstance == d.PhaseInstance && e.FrozenX == d.FrozenX) != 1)
            return false;
        if (p.Action.Type == CardActionType.Use && (_coreCardUseLifecycleOperations.LifecycleCardUse(p.NativeParentId)is not { Action: { } a } || !global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(a, p.Action)))
            return false;
        if (p.Action.Type == CardActionType.Response && (p.ResponseKind is not { } kind || !_coreResponseUseCompletionOperations.IsCompletedResponseUse2(p.Action, kind)))
            return false;
        return p.Payments.Count == p.HandMaterials.Count && p.Payments.Select(m => m.CardId).Distinct().Count() == p.Payments.Count && p.HandMaterials.All(c => p.Action.PhysicalCards.Contains(c) && ShanjiaHeld(c.From, d.Source.OwnerSeat) && c.From.Zone != CardZoneKind.Equipment && p.Payments.Count(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > p.Before && m.Sequence <= p.After && m.CardId == c.CardId && m.CardKind == c.CardKind && m.From == c.From) == 1);
    }

    internal ProgramTriggerCandidate? CaoChunDebtCandidate(CaoChunHandUsePayment p) => !ValidCaoChunPayment(p) || !_coreMatchParticipantsPlayersAccess._players[p.Debt.Source.OwnerSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[p.Debt.Source.OwnerSeat].Shanjia?.Debt != p.Debt ? null : new(p.Debt.Source.OwnerSeat, p.Debt.Source.SkillId, "hand-use-debt", p.Debt.Source.SkillInstanceId, p.Debt.GameplayHash, 0);
    internal ProgramSkillWindowContext CaoChunDebtContext(CaoChunHandUsePayment p, long windowId) => new(SkillProgramTriggerWindow.CardUseBeforeTargetEffects, windowId, p.Debt.Source.OwnerSeat, CardUse: new(p.Action.ActionId, p.NativeParentId, p.Action.ActorSeat, null, p.Action.EffectiveKind, p.Action.EffectiveSuit, p.Action.EffectiveIsRed, false, null))
    {
        CaoChunPayment = p
    };
    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 90)]
    internal bool HasIssuedCaoChunDebtCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseBeforeTargetEffects || context.CaoChunPayment is not { } p || CaoChunDebtCandidate(p) != candidate || _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program?.GameplayHash != candidate.GameplayHash || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != SkillProgramTriggerWindow.CardUseBeforeTargetEffects || !global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(window.Action, p.Action) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        var current = window.Candidates[window.CandidateIndex];
        return global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(current) == candidate && current.FrozenContext?.CaoChunPayment == p && context.CardUse?.CardActionId == p.Action.ActionId && window.ParentFrameId == p.NativeParentId;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 490)]
    internal bool CanOfferCaoChun(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        var features = ProgramInstructionResolver.Default.Features(t);
        if (features.HasOperation(SkillProgramEffectOp.PayShanjiaHandUseDebt))
            return w.CaoChunPayment is { } p && CaoChunDebtCandidate(p) == c;
        if (features.HasOperation(SkillProgramEffectOp.RequestShanjiaEndingSlash))
            return w.Window == SkillProgramTriggerWindow.PlayEnding && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Shanjia?.Debt is { } d && d.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && d.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && !d.EndingConsumed && (!d.DiscardedAny || d.OnlyEquipment);
        if (features.HasOperation(SkillProgramEffectOp.InitializeShanjiaLossMarkers))
            return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Shanjia is null;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 270)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 270)]
    internal bool CanContinueCaoChun(ProgramSkillFrame f)
    {
        if (f.CaoChun is { } r)
        {
            if (r.Stage == CaoChunStage.InitialSlash)
                return ValidCaoChunPaidActivation(f);
            if (r.Stage is CaoChunStage.DrawChildren or CaoChunStage.DiscardChildren or CaoChunStage.NativeSlash)
            {
                RequireCaoChun(f);
                return true;
            }

            return r.Payment is { } p && ValidCaoChunPayment(p) && r.Stage == CaoChunStage.DebtChoice;
        }

        return f.InstructionIndex == 0 && f.WindowContext?.CaoChunPayment is { } issued && ValidCaoChunPayment(issued) && CaoChunDebtCandidate(issued)is { } c && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }

    internal bool CaoChunFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!CanContinueCaoChun(root) || root.CaoChun is not { } r)
            return false;
        if (_characterLiuFengBoundaryOperations.BoundaryLiuFengRequestChild(root, child))
            return true;
        if (child is CardUseFrame native)
            return CaoChunNativeRidesOn(root, native);
        if (child is CardsMovedTriggerWindowFrame w)
        {
            if (w.Batch.Id != w.Id || w.Batch.ParentFrameId != root.Id || w.Batch.AwaitingProgramFrameId != root.Id || w.ResumeProgramFrameId is not null || w.Batch.OriginOwnerSeat != root.OwnerSeat || w.Batch.OriginSkillId != root.SkillId || w.Batch.OriginSkillInstanceId != root.SkillInstanceId || w.Batch.Movements.Count == 0)
                return false;
            return w.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (m.Sequence > r.Before && m.Sequence <= r.After && (r.Stage == CaoChunStage.DrawChildren ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == ShanjiaDrawReason : w.Batch.Id == r.PaidBatchId && m.CardId == r.DiscardId && m.From == r.DiscardFrom && m.To == CardLocation.DiscardPile && m.Reason.Value == ShanjiaDiscardReason) || r.Stage == CaoChunStage.DiscardChildren && r.DiscardKind == CardKind.WoodenOx && r.DiscardFrom == CardLocation.Equipment(root.OwnerSeat) && r.PaidBatchId is> 0 && w.Batch.ParentBatchId == r.PaidBatchId && m.Sequence > r.After && m.From == CardLocation.WoodenOxGrain(root.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard));
        }

        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (r.Stage != CaoChunStage.DiscardChildren || r.DiscardKind != CardKind.SilverLion || r.DiscardFrom != CardLocation.Equipment(root.OwnerSeat))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason is { Value: ShanjiaDiscardReason };
    }

    internal ProgramSkillFrame? CaoChunObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterCaoChunOlOperations.CaoChunFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 230)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 340)]
    internal bool HasCaoChunDamageObserver(long id) => CaoChunObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 430)]
    internal bool HasCaoChunBeforeDamageObserver(long id) => CaoChunObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 200)]
    internal bool IsCaoChunDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && CaoChunObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 340)]
    internal bool TryAdvanceCaoChunSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CaoChunObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    internal void ExpireCaoChunPlayDebt(int owner, int phaseInstance)
    {
        if (_coreMatchParticipantsPlayersAccess._players[owner].Shanjia is { } s && s.Debt?.PhaseInstance == phaseInstance)
            _coreMatchParticipantsPlayersAccess._players[owner].Shanjia = s with
            {
                Debt = null
            };
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 500)]
    internal CardMovementBatchContext CompleteMovementBatchObserveCaoChunMovement(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        ObserveCaoChunMovement(completed);
        return completed;
    }
}
