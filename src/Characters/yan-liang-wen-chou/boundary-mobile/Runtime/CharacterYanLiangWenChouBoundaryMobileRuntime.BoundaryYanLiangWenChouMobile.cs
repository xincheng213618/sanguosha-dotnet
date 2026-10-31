#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanLiangWenChouBoundaryMobileRuntime
{
    internal const string BoundaryYanLiangWenChouSkill = "boundary:shuangxiong-mobile";
    internal const string BoundaryYanLiangWenChouClaimReason = "skill.boundary-yan-liang-wen-chou.shuangxiong.claim";
    SkillProgramStepOutcome global::CardGame.Core.IBoundaryYanLiangWenChouHost.Claim(ProgramSkillFrame f) => BeginBoundaryYanLiangWenChouClaim(f);
    internal bool BoundaryYanLiangWenChouOriginalDuel(int owner, string instance, ProgramSkillWindowContext context, out CardUseFrame use)
    {
        use = null !;
        if (context is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } || context.TargetSeat != owner || _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } damage || damage.TriggerWindow != context.Window || damage.TargetSeat != owner || damage.ParentFrameId != context.DamageFrameId || _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(d => d.Id == damage.ParentFrameId)is not { } applied || _coreLastDamageSourceReciprocityObserversOperations.LastDamageSourceOriginalAttack(applied.ParentFrameId)is not { } attack)
            return false;
        if (attack.IsSourceLess || attack.IsChainPropagation || attack.TargetSeat != owner || attack.SourceSeat == owner || attack.EffectiveCardKind != CardKind.Duel || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { CardKind: CardKind.Duel, Action: { } action } current || current.SourceSeat != owner || action.ActorSeat != owner || action.EffectiveKind != CardKind.Duel || !action.ConversionChain.Any(s => s.SkillId == BoundaryYanLiangWenChouSkill && s.OwnerSeat == owner && s.SkillInstanceId == instance && s.BindingId == "reveal-two-instead-of-drawing.turn-2") || !_coreProgramEventHistoryOperations.ProgramEventHistory<DuelResponseEvent>().Any(e => e.ResolutionId == current.Id && e.ResponderSeat == owner && !e.UsedSlash))
            return false;
        use = current;
        return true;
    }

    internal IReadOnlyList<int> BoundaryYanLiangWenChouResponseEntities(CardUseFrame use, int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Select(e => e.Action).Where(a => a.Type == CardActionType.Response && a.ParentActionId == use.Action!.ActionId && a.ActorSeat != owner && a.ResponderSeat == a.ActorSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && _coreProgramEventHistoryOperations.ProgramEventHistory<DuelResponseEvent>().Any(e => e.ResolutionId == use.Id && e.ResponderSeat == a.ActorSeat && e.UsedSlash)).SelectMany(a => a.PhysicalCards).Select(c => c.CardId).Distinct().Where(id => id > 0 && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DiscardPile).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 65710, SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses)]
    internal bool OfferBoundaryYanLiangWenChouClaim(ProgramTriggerLegalityContext c) => BoundaryYanLiangWenChouOriginalDuel(c.OwnerSeat, c.Candidate.SkillInstanceId, c.Context, out var use) && BoundaryYanLiangWenChouResponseEntities(use, c.OwnerSeat).Count > 0;
    internal SkillProgramStepOutcome BeginBoundaryYanLiangWenChouClaim(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != BoundaryYanLiangWenChouSkill || f.InstructionIndex != 1 || f.BoundaryYanLiangWenChouClaim is not null || f.WindowContext is not { } context || !BoundaryYanLiangWenChouOriginalDuel(f.OwnerSeat, f.SkillInstanceId, context, out var use))
            throw new InvalidOperationException("Shuangxiong lost its original converted Duel and actual recipient-damage window.");
        var ids = BoundaryYanLiangWenChouResponseEntities(use, f.OwnerSeat);
        if (ids.Count == 0)
            return SkillProgramStepOutcome.Continue;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var r = new BoundaryYanLiangWenChouClaimReceipt(new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, context.ParentFrameId, context.DamageFrameId!.Value, use.Id, use.Action!.ActionId, f.InstructionIndex, context.OccurrenceIndex, ids, before, before + ids.Count);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryYanLiangWenChouClaim = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var cards = ids.Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(c => c.Id == id)).ToArray();
        var index = 0;
        _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new(BoundaryYanLiangWenChouClaimReason), beforeFact: m =>
        {
            var i = index++;
            if (m.CardId != r.CardIds[i])
                throw new InvalidOperationException("Shuangxiong's frozen response gain order changed.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryYanLiangWenChouResponseClaimedEvent(f.Id, use.Id, use.Action.ActionId, f.OwnerSeat, m.CardId, i, m.Sequence));
        }, emitCardMovedEvents: false);
        ContinueBoundaryYanLiangWenChouClaim(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ValidBoundaryYanLiangWenChouClaim(ProgramSkillFrame f)
    {
        if (f.BoundaryYanLiangWenChouClaim is not { } r || f.SkillId != BoundaryYanLiangWenChouSkill || f.InstructionIndex != 1 || r.InstructionIndex != 1 || f.WindowContext is not { } c || c.ParentFrameId != r.WindowId || c.DamageFrameId != r.DamageId || c.OccurrenceIndex != r.OccurrenceIndex || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || !BoundaryYanLiangWenChouOriginalDuel(f.OwnerSeat, f.SkillInstanceId, c, out var use) || use.Id != r.UseId || use.Action!.ActionId != r.ActionId || r.CardIds.Count == 0 || r.CardIds.Distinct().Count() != r.CardIds.Count || r.CardIds.Any(id => id <= 0) || r.Before < 0 || r.After != r.Before + r.CardIds.Count)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Single(w => w.Id == r.WindowId);
        if (parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex].ToProgramCandidate()))
            return false;
        var claims = _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryYanLiangWenChouResponseClaimedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        return claims.Length == r.CardIds.Count && claims.Select(e => e.CardId).SequenceEqual(r.CardIds) && !claims.Where((e, i) => e.UseId != r.UseId || e.ActionId != r.ActionId || e.OwnerSeat != f.OwnerSeat || e.ClaimIndex != i || e.MovementSequence != r.Before + i + 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BoundaryYanLiangWenChouClaimReason) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(a => a.Action.Type == CardActionType.Response && a.Action.ParentActionId == r.ActionId && a.Action.ActorSeat != f.OwnerSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.Action.EffectiveKind) && a.Action.PhysicalCards.Any(p => p.CardId == e.CardId))).Any();
    }

    internal void ContinueBoundaryYanLiangWenChouClaim(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (!ValidBoundaryYanLiangWenChouClaim(f))
            throw new InvalidOperationException("Shuangxiong lost its original response entities and committed gain receipt.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed: true);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 65710)]
    internal bool ProveBoundaryYanLiangWenChouClaimMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op != SkillProgramEffectOp.ClaimBoundaryYanLiangWenChouDuelResponses || !ValidBoundaryYanLiangWenChouClaim(f) || f.BoundaryYanLiangWenChouClaim is not { } r || pending != f.PendingMovementContinuation || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            return false;
        // The native batch is the paid gain, even if a later observer moves an
        // entity again. Retain the original Duel/response invoice on this frame.
        return _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Concat(_resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(w => w.Batch)).Any(batch => batch.ParentFrameId == f.Id && batch.OriginOwnerSeat == f.OwnerSeat && batch.OriginSkillId == f.SkillId && batch.OriginSkillInstanceId == f.SkillInstanceId && batch.Movements.Select(m => m.CardId).SequenceEqual(r.CardIds) && batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DiscardPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BoundaryYanLiangWenChouClaimReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 65710)]
    internal bool ResumeBoundaryYanLiangWenChouClaim(long id)
    {
        if (_coreProgramLifecycleOperations.GetActiveProgramFrame(id).BoundaryYanLiangWenChouClaim is null)
            return false;
        ContinueBoundaryYanLiangWenChouClaim(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 65710)]
    internal bool ReturnBoundaryYanLiangWenChouClaim(ProgramSkillFrame f)
    {
        if (f.BoundaryYanLiangWenChouClaim is null)
            return false;
        if (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != f.OwnerSeat)
            throw new InvalidOperationException("Shuangxiong's actual gain child lost its typed original-frame return.");
        ContinueBoundaryYanLiangWenChouClaim(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 65710)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 65710)]
    internal bool KeepPaidBoundaryYanLiangWenChouClaim(ProgramSkillFrame f) => f.BoundaryYanLiangWenChouClaim is not null && ValidBoundaryYanLiangWenChouClaim(f);
}
