#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeQiMobileShiRuntime
{
    internal const string ShiHqShanxi = "mobile:shi-shanxi", ShiHqQizhou = "mobile:shi-qizhou", ShiHqView = "yingji-duel";
    internal const string ShiHqDrawReason = "skill.mobile-shi-he-qi.yingji-draw", ShiHqCleanupReason = "skill.mobile-shi-he-qi.yingji-cleanup";
    internal bool HasShiHqCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShiHeQiQizhouMark);

    internal static CardConversionSource ShiHqSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal ShiHqYingjiMarkedEvent? ShiHqMark(int id) => !HasShiHqCapability ? null : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHqYingjiMarkedEvent>().LastOrDefault(m => m.CardId == id && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHqYingjiClearedEvent>().Any(c => c.MarkId == m.MarkId));
    internal bool HasShiHqYingji(int id) => ShiHqMark(id)is not null;
    [ProgramPipelineHook(ProgramPipelinePhase.CardEntitySkillLabel, 49000)]
    internal string? ShiHqYingjiLabel(int id) => HasShiHqYingji(id) ? "应机" : null;
    internal bool ShiHqTargetUsed(int owner, int target) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHqShanxiUsedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.PhaseInstance == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && e.Target == target);
    [ProgramCardLegality(ProgramCardLegalityStage.ConversionSourceBan, 49000)]
    internal bool BanShiHqUnmarkedConversion(ProgramViewAsSourceQuery q) => q.Source.SkillId == ShiHqShanxi && (q.Source.BindingId != ShiHqView || q.ForResponse || q.DyingUse || q.Source.OwnerSeat != q.Owner.Seat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != q.Owner.Seat || _cardZonesOperations._cardZones.GetLocation(q.Card.Id) != CardLocation.Hand(q.Owner.Seat) && !_characterLuSuMobileShiOperations.IsShiLsHandAccess2(q.Owner.Seat, q.Card.Id) || !HasShiHqYingji(q.Card.Id));
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 49000)]
    internal bool BanShiHqRepeatedTarget(ProgramLegalActionQuery q) => q.Action.ConversionSource is { SkillId: ShiHqShanxi } && (q.Action.TargetSeats.Count == 0 || q.Action.TargetSeats.Any(s => s == q.Actor.Seat || ShiHqTargetUsed(q.Actor.Seat, s)));
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 49000)]
    internal void ObserveShiHqDuel(IGameEvent payload)
    {
        if (!HasShiHqCapability || payload is not CardUseDeclaredEvent d || _coreCardUseLifecycleOperations.LifecycleCardUse(d.ResolutionId)is not { Action: { } a } use || !a.ConversionChain.Any(s => s.SkillId == ShiHqShanxi))
            return;
        var source = a.ConversionChain.Single(s => s.SkillId == ShiHqShanxi);
        if (source.BindingId != ShiHqView || a.Type != CardActionType.Use || a.EffectiveKind != CardKind.Duel || use.CardKind != CardKind.Duel || a.ActorSeat != source.OwnerSeat || a.PhysicalCards is not [{ CardId: var id, From: var from }] || !IsShiHqOriginalHandPayment(use, a, source.OwnerSeat, id, from) || !HasShiHqYingji(id) || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != source.OwnerSeat || use.TargetSeats.Count == 0 || use.TargetSeats.Any(s => s == source.OwnerSeat || ShiHqTargetUsed(source.OwnerSeat, s)))
            throw new InvalidOperationException("Shanxi requires one original authorized marked hand entity and unused actual-Play targets on its native Duel.");
        foreach (var target in use.TargetSeats.Distinct())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHqShanxiUsedEvent(use.Id, a.ActionId, source, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, target, id));
    }

    internal bool IsShiHqOriginalHandPayment(CardUseFrame use, CardActionContext action, int actor, int cardId, CardLocation from) => from == CardLocation.Hand(actor) && action.ProviderSeat == actor || from.Zone == CardZoneKind.Hand && from.OwnerSeat == action.ProviderSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiLsMaterialAuthorizedEvent>().Any(e => e.UseId == use.Id && e.ActionId == action.ActionId && e.Actor == actor && e.CardId == cardId && e.From == from);
    internal static bool IsShiHqMarkTrigger(SkillProgramTrigger t) => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveMobileShiHeQiQizhouMark);
    internal static bool ShiHqEntersField(CardMovementRecord m, int owner) => m.To.OwnerSeat == owner && m.To.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment && m.From != m.To;
    internal IEnumerable<ProgramTriggerCandidate> CollectShiHqMovementCandidates(CardMovementBatchContext batch)
    {
        if (!HasShiHqCapability)
            yield break;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && batch.Movements.Any(m => ShiHqEntersField(m, p.Seat))))
            foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved).Where(c => c.SkillId == ShiHqQizhou && c.BindingId == "field-entry-mark"))
                yield return c;
    }

    internal ProgramSkillWindowContext CreateShiHqMovementContext(CardsMovedTriggerWindowFrame w, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.CardsMoved, w.Id, c.OwnerSeat, SourceSeat: w.Batch.OriginOwnerSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]), MovementBatch: w.Batch);
    internal bool IsCurrentShiHqFieldEntry(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != ShiHqQizhou || c.BindingId != "field-entry-mark" || context.Window != SkillProgramTriggerWindow.CardsMoved || context.OwnerSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || context.OccurrenceIndex != c.OccurrenceIndex || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId) || _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash != c.GameplayHash || context.MovementBatch is not { } batch || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()is not { } window || window.Id != context.ParentFrameId || window.Batch.Id != batch.Id || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != c || context.SourceSeat != window.Batch.OriginOwnerSeat || !window.Batch.Movements.SequenceEqual(batch.Movements))
            return false;
        // Native equipment and delayed-trick entry often comes from ownerless
        // Processing. Qualification belongs to the actual destination batch,
        // never to an unrelated card loss by the observing skill holder.
        return batch.Movements.Any(m => ShiHqEntersField(m, c.OwnerSeat) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 49000)]
    internal bool CanOfferShiHq(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (c.SkillId != ShiHqQizhou)
            return true;
        if (t.Id == "field-entry-mark")
            return IsCurrentShiHqFieldEntry(c, w);
        if (t.Id == "global-yingji-cleanup")
            return IsIssuedShiHqCleanupCandidate(c, w);
        return t.Id == "draw-extra-yingji" && w.Window == SkillProgramTriggerWindow.DrawPhaseStarting && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == c.OwnerSeat;
    }

    internal ShiHqYingjiMarkedEvent? ShiHqCleanupSource() => _coreMatchParticipantsPlayersAccess._players.SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p)).Select(c => ShiHqMark(c.Id)).Where(m => m is not null).Cast<ShiHqYingjiMarkedEvent>().OrderBy(m => m.MarkId).FirstOrDefault();
    internal IReadOnlyList<TurnEndingBoundaryItem> IncludeShiHqAfterTurnEndedItems(DeferredTurnEndFrame parent, IReadOnlyList<TurnEndingBoundaryItem> ordinary)
    {
        if (!HasShiHqCapability)
            return ordinary;
        var items = parent.AfterTurnEnded is null ? ordinary.Where(i => i.Candidate is not { SkillId: ShiHqQizhou, BindingId: "global-yingji-cleanup" }).ToList() : ordinary.ToList();
        if (!items.Any(i => i.Candidate is { SkillId: ShiHqQizhou, BindingId: "global-yingji-cleanup" }) && ShiHqCleanupSource()is { } mark && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHqCleanupIssuedEvent>().Any(e => e.Turn == parent.TurnNumber && e.TurnOwner == parent.OwnerSeat))
        {
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(ShiHqQizhou).Program!.Triggers.Single(t => t.Id == "global-yingji-cleanup");
            var c = new ProgramTriggerCandidate(mark.Source.OwnerSeat, ShiHqQizhou, trigger.Id, mark.Source.SkillInstanceId, mark.Hash, trigger.Priority);
            items.Add(new(TurnEndingBoundaryItemKind.Program, trigger.Priority, $"shi-he-qi.cleanup:{(parent.TurnNumber)}:{(parent.OwnerSeat)}", c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
        }

        return items.SequenceEqual(ordinary) ? ordinary : Array.AsReadOnly(items.ToArray());
    }

    [ProgramIssuedTrigger(350)]
    internal bool IsIssuedShiHqCleanupCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (!HasShiHqCapability || c.SkillId != ShiHqQizhou || c.BindingId != "global-yingji-cleanup" || w.Window != SkillProgramTriggerWindow.AfterTurnEnded || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not DeferredTurnEndFrame p || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(p) || _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(p) != c || w.OwnerSeat != c.OwnerSeat || w.SourceSeat != p.OwnerSeat || w.TargetSeat != p.OwnerSeat || w.OccurrenceIndex != c.OccurrenceIndex || w.Facts != p.AfterTurnEnded!.Items[p.AfterTurnEnded.ItemIndex].Facts || _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program?.GameplayHash != c.GameplayHash)
            return false;
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.ShiHeQi is { Operation: SkillProgramEffectOp.ResolveMobileShiHeQiYingjiCleanup } r && r.ParentId == p.Id && r.Source.OwnerSeat == c.OwnerSeat && r.Source.SkillInstanceId == c.SkillInstanceId && r.Hash == c.GameplayHash && f.WindowContext == w))
            return true;
        return ShiHqCleanupSource()is { } mark && mark.Source.OwnerSeat == c.OwnerSeat && mark.Source.SkillInstanceId == c.SkillInstanceId && mark.Hash == c.GameplayHash && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiHqCleanupIssuedEvent>().Any(e => e.Turn == p.TurnNumber && e.TurnOwner == p.OwnerSeat);
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiHeQiPublicStates(CharacterState owner, string skillId)
    {
        if (skillId != ShiHqShanxi)
            return[];
        var used = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner.Seat ? _coreMatchParticipantsPlayersAccess._players.Count(p => p.Seat != owner.Seat && ShiHqTargetUsed(owner.Seat, p.Seat)) : 0;
        var remaining = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner.Seat ? _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive && p.Seat != owner.Seat && !ShiHqTargetUsed(owner.Seat, p.Seat)) : 0;
        return _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances.Where(i => i.SkillId == skillId).SelectMany(i => new[] { new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "shanxi-used-targets", ProgramPublicRuleStateKind.GameActivationProgress, used), new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "shanxi-remaining-targets", ProgramPublicRuleStateKind.GameActivationProgress, remaining) }).ToArray();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 4200)]
    internal void CollectWindow_CollectShiHqMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectShiHqMovementCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 400)]
    internal bool ExcludeOrdinaryMovement_IsShiHqMarkTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsShiHqMarkTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 600)]
    internal ProgramSkillWindowContext? TryCreateShiHqMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsShiHqMarkTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateShiHqMovementContext(frame, candidate);
        return null;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 20, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveShiHqMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsShiHqMarkTrigger(trigger) ? IsCurrentShiHqFieldEntry(candidate, context) : null;
}
