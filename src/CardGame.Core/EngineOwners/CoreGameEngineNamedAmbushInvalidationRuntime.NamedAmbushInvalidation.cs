#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNamedAmbushInvalidationRuntime
{
    internal bool IsExactNamedAmbushInvalidation(NamedAmbushUseInvalidatedEvent fact, CardActionContext action, long parent)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var offers = history.Select((e, i) => (e, i)).Where(p => p.e is NamedAmbushOfferedEvent issued && issued.ProgramFrameId == fact.ProgramFrameId && issued.Receipt.Removing && issued.Receipt.Use == fact.Use).ToArray();
        if (offers is not [var offer] || offer.e is not NamedAmbushOfferedEvent { Receipt: var receipt } || receipt.OriginalAction is not { } original || receipt.Source.SkillId != fact.Deposit.Source.SkillId || receipt.Source.OwnerSeat != fact.Deposit.Source.OwnerSeat || receipt.Source.SkillInstanceId != fact.Deposit.Source.SkillInstanceId || receipt.InstructionIndex != 1 || _contentRegistryOperations._contentRegistry.GetSkill(receipt.Source.SkillId).Program is not { } program || receipt.GameplayHash != program.GameplayHash || ProgramInstructionResolver.Default.FindTrigger(program, receipt.Source.BindingId)?.Effects is not [{ Op: SkillProgramEffectOp.NullifyMatchingHandUse }] || history.OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.ProgramFrameId && e.SkillId == receipt.Source.SkillId && e.OwnerSeat == receipt.Source.OwnerSeat && e.BindingId == receipt.Source.BindingId && e.SkillInstanceId == receipt.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseCommitted) != 1 || fact.Use.WindowFrameId != receipt.WindowFrameId || fact.Use.ParentFrameId != parent || fact.Use.ActionId != original.ActionId || fact.Use.ActorSeat != original.ActorSeat || fact.Use.ProviderSeat != original.ProviderSeat || fact.Use.EffectiveKind != original.EffectiveKind || original.ActorSeat != original.ProviderSeat || fact.Use.ResponseUse != (original.Type == CardActionType.Response) || original.PhysicalCards.Count == 0 || original.PhysicalCards.Any(c => c.From != CardLocation.Hand(original.ActorSeat)) || original.ActionId != action.ActionId || original.ParentActionId != action.ParentActionId || original.Type != action.Type || original.ProviderSeat != action.ProviderSeat || original.RequesterSeat != action.RequesterSeat || original.ResponderSeat != action.ResponderSeat || original.OpponentSeat != action.OpponentSeat || original.EffectiveSuit != action.EffectiveSuit || original.EffectiveRank != action.EffectiveRank || !original.PhysicalCards.SequenceEqual(action.PhysicalCards) || fact.Deposit.RecordedKind != CoreGameEngineNamedAmbushRuntime.AmbushCardName(original.EffectiveKind) || fact.SequenceAfter <= fact.SequenceBefore || history.OfType<NamedAmbushDepositedEvent>().Count(e => e.Deposit == fact.Deposit) != 1 || history.OfType<NamedAmbushRemovedEvent>().Count(e => e.ProgramFrameId == fact.ProgramFrameId && e.Deposit == fact.Deposit && e.Use == fact.Use && e.SequenceBefore == fact.SequenceBefore && e.SequenceAfter == fact.SequenceAfter && e.Reason == "used") != 1 || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.CardId == fact.Deposit.CardId && m.From == fact.Deposit.Location && m.To == CardLocation.DiscardPile && m.Sequence > fact.SequenceBefore && m.Sequence <= fact.SequenceAfter && m.Reason.Value == "skill-program.named-ambush.remove") != 1 || history.OfType<NamedAmbushUseInvalidatedEvent>().Count(e => e == fact) != 1)
            return false;
        if (original.ActorSeat == action.ActorSeat && original.EffectiveKind == action.EffectiveKind && original.ConversionChain.SequenceEqual(action.ConversionChain))
            return true;
        // A paid ineffective state follows the same real use through later
        // proven role/kind changes; its hand qualification stays at issuance.
        return !fact.Use.ResponseUse && _coreCardUseLifecycleOperations.LifecycleCardUse(parent)is { Action: { } current } use && CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(current, action) && _coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action) && _coreCompletedCategoryUseProofOperations.RebuildAcceptedActualHandGainUse(use, original, action, history, offer.i);
    }

    internal void InvalidateNamedAmbushUse(ProgramSkillFrame f)
    {
        var r = f.NamedAmbush!;
        var use = r.Use!;
        if (!_coreNamedAmbushOperations.TryGetAmbushUse(f.WindowContext!, out var live, out var action) || live != use)
            throw new InvalidOperationException("Paid ambush cannot invalidate a different actual use.");
        var fact = new NamedAmbushUseInvalidatedEvent(f.Id, r.Deposit!, use, r.MovementBefore, r.MovementAfter);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        if (!IsExactNamedAmbushInvalidation(fact, action, use.ParentFrameId))
            throw new InvalidOperationException("Invalidation lost exact private-card payment proof.");
        if (!use.ResponseUse)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.ParentFrameId, parent => parent with { NamedAmbushInvalidation = fact });
        else if (action.EffectiveKind == CardKind.Dodge)
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.ParentFrameId, parent => parent with { NamedAmbushResponseInvalidations = (parent.NamedAmbushResponseInvalidations ?? []).Append(fact).ToArray() });
        else if (action.EffectiveKind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } counter && counter.Id == use.ParentFrameId && counter.NamedAmbushResponse is { } response && response.ActionId == action.ActionId)
            _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(counter with { NamedAmbushResponse = response with { Invalidated = fact } });
        else
            throw new InvalidOperationException("The invalidated response lost its exact typed native owner.");
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name)}】，令【{(CardCatalog.Get(action.EffectiveKind).DisplayName)}】无效。", f.OwnerSeat, action.ActorSeat);
    }

    internal bool IsNamedAmbushUseIneffective(CardUseFrame use)
    {
        if (_characterWeiYanHappyGodOperations.IsHappyWeiYanUseIneffective(use) || _characterChengYuMouOperations.IsMouCyUseIneffective(use) || _characterHuangZhongAnniversaryGodOperations.IsAnniversaryHzUseIneffective(use))
            return true;
        if (use.NamedAmbushInvalidation is not { } proof)
            return false;
        if (use.Action is not { } action || !IsExactNamedAmbushInvalidation(proof, action, use.Id))
            throw new InvalidOperationException("A globally invalidated use lost its exact paid ambush proof.");
        return true;
    }

    internal bool ContinueNamedAmbushIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if (_characterWeiYanHappyGodOperations.ContinueHappyWeiYanIneffectiveDodge(attack, action, continuation) || _characterChengYuMouOperations.ContinueMouCyIneffectiveDodge(attack, action, continuation) || _characterHuangZhongAnniversaryGodOperations.ContinueAnniversaryHzIneffectiveDodge(attack, action, continuation))
            return true;
        if (continuation != ProgramCardContinuation.Dodge || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.NamedAmbushResponseInvalidations is not { } proofs)
            return false;
        var matches = proofs.Where(p => p.Use.ActionId == action.ActionId).ToArray();
        if (matches.Length == 0)
            return false;
        if (action.ParentActionId != _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action?.ActionId || attack.TargetSeat != action.ActorSeat || action.EffectiveKind != CardKind.Dodge || matches.Any(p => !IsExactNamedAmbushInvalidation(p, action, attack.ResolutionId)))
            throw new InvalidOperationException("An ineffective Dodge changed its Slash or committed response identity.");
        _coreRoundOnceCardlessNativeResponsesOperations.ClearRoundOnceCardlessDodgeResponse(attack);
        _coreTieredRoundZeroResponsesOperations.ClearTieredRoundDodgeResponse(attack);
        _coreChainedStateBasicReturnsOperations.ClearChainedStateBasicDodgeResponse(attack);
        _coreDrawFundedDistinctBasicResponsesOperations.Value.ClearDrawFundedDistinctBasicDodgeResponse(attack);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        return true;
    }

    internal NullificationWindowFrame PrepareNamedAmbushCounterspell(NullificationWindowFrame pending, int responder, int cardId)
    {
        if (!_coreNamedAmbushOperations.HasNamedAmbushCapability && !_characterChengYuMouOperations.HasMouChengYuAnniversary && !_characterHuangZhongAnniversaryGodOperations.HasAnniversaryHuangZhongCapability && !_characterWeiYanHappyGodOperations.HasHappyWeiYanEzhiCapability)
            return pending;
        if (pending.NamedAmbushResponse is not null)
            throw new InvalidOperationException("A counterspell already awaits its actual response use.");
        return _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { NamedAmbushResponse = new(0, responder, cardId, pending.EffectNullified, pending.ChainDepth), ChainDepth = pending.ChainDepth + 1, CandidateSeats = _coreNativeNullificationOperations.BuildNullificationCandidateSeats(responder), CandidateIndex = 0 });
    }

    internal bool HasEnabledMatchingNamedAmbush(int actor, CardKind kind) => _coreNamedAmbushOperations.HasNamedAmbushCapability && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != actor && p.Seat != _coreActualTurnProgressCurrentSeatAccess._currentSeat).Any(owner => _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.CardUseCommitted).Any(b => b.Trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.NullifyMatchingHandUse) && _coreNamedAmbushOperations.LiveAmbushDeposits(owner.Seat, b.SkillId, b.SkillInstanceId).Any(d => d.RecordedKind == CoreGameEngineNamedAmbushRuntime.AmbushCardName(kind))));
    internal void BindNamedAmbushCounterspell(long id, CardActionContext action)
    {
        if (!_coreNamedAmbushOperations.HasNamedAmbushCapability && !_characterChengYuMouOperations.HasMouChengYuAnniversary && !_characterHuangZhongAnniversaryGodOperations.HasAnniversaryHuangZhongCapability && !_characterWeiYanHappyGodOperations.HasHappyWeiYanEzhiCapability)
            return;
        var pending = _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().Single(f => f.Id == id);
        if (pending.NamedAmbushResponse is not { ActionId: 0 } r || action.EffectiveKind != CardKind.Nullification || action.Type != CardActionType.Response || action.ActorSeat != r.ResponderSeat || (!action.PhysicalCards.Any(c => c.CardId == r.CardId) && !IsDeferredZeroCounterspell(pending, action, r)))
            throw new InvalidOperationException("A deferred counterspell lost its committed paid response.");
        _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { NamedAmbushResponse = r with { ActionId = action.ActionId } });
        _characterLiJueOlOperations.CaptureLiJueCompletedNullificationUse(pending.Id, action);
    }

    internal bool IsDeferredZeroCounterspell(NullificationWindowFrame pending, CardActionContext action, NamedAmbushCounterspellResponse response) => response.CardId == 0 && action.PhysicalCards.Count == 0 && pending.TieredRoundResponseUse is { } receipt && receipt.OwnerFrameId == pending.Id && receipt.CardActionId == action.ActionId && receipt.ActorSeat == action.ActorSeat && receipt.EffectiveKind == CardKind.Nullification && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TieredRoundConversionUseIssuedEvent>().Count(e => e.Receipt == receipt) == 1;
    internal NullificationWindowFrame CompleteNamedAmbushCounterspell(NullificationWindowFrame pending)
    {
        if (pending.NamedAmbushResponse is not { } r)
            return pending;
        if (r.ActionId <= 0 || pending.ChainDepth != r.PreviousChainDepth + 1 || pending.EffectNullified != r.PreviousEffectNullified || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != pending.Id)
            throw new InvalidOperationException("Deferred counterspell completion changed the original chain state.");
        var action = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Select(e => e.Action).LastOrDefault(a => a.ActionId == r.ActionId) ?? throw new InvalidOperationException("Deferred counterspell lost its real response use.");
        if (r.Invalidated is { } invalidation && !IsExactNamedAmbushInvalidation(invalidation, action, pending.Id))
            throw new InvalidOperationException("Counterspell cancellation lost paid ambush provenance.");
        var ineffective = r.Invalidated is not null || _characterChengYuMouOperations.MouCyCounterspellInvalidated(pending, action) || _characterHuangZhongAnniversaryGodOperations.AnniversaryHzCounterspellInvalidated(pending, action) || _characterWeiYanHappyGodOperations.HappyWeiYanCounterspellInvalidated(pending, action);
        pending = _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { NamedAmbushResponse = null, MouCyResponseInvalidation = null, AnniversaryHzInvalidation = null, HappyWeiYanCancellation = null, EffectNullified = !ineffective ? !r.PreviousEffectNullified : r.PreviousEffectNullified, ChainDepth = r.PreviousChainDepth + 1, CandidateSeats = _coreNativeNullificationOperations.BuildNullificationCandidateSeats(r.ResponderSeat), CandidateIndex = 0 });
        _characterCaoXiOlOperations.CaptureCaoXiNativeCounterspellOffset(pending, action, invalidated: ineffective);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NullificationRespondedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, r.ResponderSeat, r.CardId, pending.EffectNullified, pending.ChainDepth) { WindowFrameId = pending.Id, TargetSeat = CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) });
        _coreMatchLogOperations.AddLog("NullificationResponded", ineffective ? $"{(_coreMatchParticipantsPlayersAccess._players[r.ResponderSeat].Name)} 使用的【无懈可击】无效。" : pending.EffectNullified ? $"{(_coreMatchParticipantsPlayersAccess._players[r.ResponderSeat].Name)} 使用【无懈可击】，使【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】失效。" : $"{(_coreMatchParticipantsPlayersAccess._players[r.ResponderSeat].Name)} 使用【无懈可击】，使【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】恢复。", r.ResponderSeat, pending.SourceSeat);
        return pending;
    }
}
