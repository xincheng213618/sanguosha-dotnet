#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiYanHappyGodRuntime
{
    internal bool ValidHappyWeiYanDraw(NativeDrawOutcome draw, int owner, long frameId)
    {
        if (draw.RecipientSeat != owner || draw.RequestedCount != 2 || draw.SequenceBefore < 0 || draw.SequenceAfter < draw.SequenceBefore || draw.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || draw.ActualDrawIds.Count > 2 || draw.ActualDrawIds.Distinct().Count() != draw.ActualDrawIds.Count || draw.ActualDrawIds.Any(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(owner) && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && m.Reason.Value == HappyEzhiDraw) != 1))
            return false;
        if (draw.AttemptFrameId is { } attempt)
            return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawAttemptStartedEvent>().Count(e => e.Context.AttemptFrameId == attempt && e.Context.ParentFrameId == frameId && e.Context.RecipientSeat == owner && e.Context.RequestedCount == 2 && e.Context.Reason.Value == HappyEzhiDraw && e.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation && e.Return.InstructionIndex == 1) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == frameId && e.RecipientSeat == owner && e.RequestedCount == 2 && e.Replaced == (draw.Replacement is not null)) == 1;
        return draw.Replacement is null;
    }

    internal bool ValidHappyWeiYanCancellation(HappyWeiYanCancellation proof, CardActionContext action, long parent)
    {
        if (proof.ParentId != parent || proof.Source.SkillId != HappyWeiYanComposition.Ezhi || proof.Source.BindingId != "bone-use" || proof.Source.OwnerSeat == action.ActorSeat || proof.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || proof.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(proof.Source.SkillId).Program!.GameplayHash || !CharacterHuangZhongAnniversaryGodRuntime.SameAnniversaryHzAction(proof.Action, action) || action.PhysicalCards.Count == 0 || proof.Draw is not { } draw || !ValidHappyWeiYanDraw(draw, proof.Source.OwnerSeat, proof.FrameId) || draw.SequenceAfter > proof.Before || proof.After <= proof.Before || proof.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || proof.Bone.Holder != action.ActorSeat || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(proof.Bone.PrintedKind) != CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(action.EffectiveKind) || HappyBoneAt(proof.Bone.CardId, CardLocation.Equipment(action.ActorSeat), proof.Before)is not { } original || original.Sequence != proof.Bone.Sequence || original.Source != proof.Bone.Source || original.PrintedKind != proof.Bone.PrintedKind || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == proof.Bone.CardId && m.CardKind == proof.Bone.PrintedKind && m.From == CardLocation.Equipment(action.ActorSeat) && m.Reason.Value == HappyEzhiDiscard && m.Sequence > proof.Before && m.Sequence <= proof.After) != 1)
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == proof.FrameId && e.OwnerSeat == proof.Source.OwnerSeat && e.SkillId == proof.Source.SkillId && e.BindingId == proof.Source.BindingId && e.SkillInstanceId == proof.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseCommitted) == 1 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyWeiYanEzhiPaidEvent>().Count(e => e == new HappyWeiYanEzhiPaidEvent(proof.FrameId, proof.Source, proof.GameplayHash, proof.ActualTurn, proof.WindowId, parent, action.ActionId, action.ActorSeat, action.EffectiveKind, proof.Bone.CardId, proof.Bone.PrintedKind, proof.Before, proof.After, true)) == 1;
    }

    internal bool IsHappyWeiYanUseIneffective(CardUseFrame use)
    {
        if (use.HappyWeiYanCancellation is not { } proof)
            return false;
        if (use.Action is not { Type: CardActionType.Use } action || !ValidHappyWeiYanCancellation(proof, action, use.Id))
            throw new InvalidOperationException("Ezhi lost the original paid actual whole-Use cancellation.");
        return true;
    }

    internal bool ContinueHappyWeiYanIneffectiveSimple(long id, ProgramSimpleCardContinuation continuation)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { } use || !IsHappyWeiYanUseIneffective(use))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id)
            throw new InvalidOperationException("Ezhi lost the original ineffective native simple Use.");
        foreach (var card in _coreNativeTrickUseOperations.GetCardUsePhysicalCards(id).Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.Processing))
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, new("skill-program.happy:ezhi.cleanup"));
        var original = use.CardId == 0 ? new Card(0, use.CardKind, Suit.None, 0) : _coreAdvancedLifecycleOperations.GetAdvancedCard(use.CardId);
        _coreNativeCardUseCompletionOperations.FinishCardUse(id, original, use.CardKind);
        return true;
    }

    internal bool HasHappyWeiYanIneffectiveResponse(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if (continuation != ProgramCardContinuation.Dodge || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.HappyWeiYanResponses is not { } proofs)
            return false;
        var matches = proofs.Where(p => p.Action.ActionId == action.ActionId).ToArray();
        if (matches.Length == 0)
            return false;
        if (action.ParentActionId != _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.Action?.ActionId || action.EffectiveKind != CardKind.Dodge || action.ActorSeat != attack.TargetSeat || matches.Any(p => !ValidHappyWeiYanCancellation(p, action, attack.ResolutionId)))
            throw new InvalidOperationException("Ezhi's canceled Dodge lost its paid native Slash-response owner.");
        return true;
    }

    internal bool ContinueHappyWeiYanIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if (!HasHappyWeiYanIneffectiveResponse(attack, action, continuation))
            return false;
        _coreRoundOnceCardlessNativeResponsesOperations.ClearRoundOnceCardlessDodgeResponse(attack);
        _coreTieredRoundZeroResponsesOperations.ClearTieredRoundDodgeResponse(attack);
        _coreChainedStateBasicReturnsOperations.ClearChainedStateBasicDodgeResponse(attack);
        _coreDrawFundedDistinctBasicResponsesOperations.Value.ClearDrawFundedDistinctBasicDodgeResponse(attack);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        return true;
    }

    internal bool HappyWeiYanCounterspellInvalidated(NullificationWindowFrame pending, CardActionContext action)
    {
        if (pending.HappyWeiYanCancellation is not { } proof)
            return false;
        if (pending.NamedAmbushResponse?.ActionId != action.ActionId || !ValidHappyWeiYanCancellation(proof, action, pending.Id))
            throw new InvalidOperationException("Ezhi's canceled Nullification lost its original paid chain node.");
        return true;
    }
}
