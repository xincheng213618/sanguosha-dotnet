#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTieredRoundZeroReturnsRuntime
{
    internal bool TieredRoundZeroDyingRescueRide(int dyingIndex, DyingFrame dying)
    {
        if (dyingIndex < 0 || dyingIndex + 1 >= _resolutionStackOperations._resolutionStack.Count || _coreDyingCursorRuntimeOperations.Value.ActiveDying?.FrameId != dying.Id || _resolutionStackOperations._resolutionStack[dyingIndex] is not DyingFrame original || original != dying || _resolutionStackOperations._resolutionStack[dyingIndex + 1] is not CardUseFrame rescue || !_coreTieredRoundZeroUsesOperations.IsIssuedTieredRoundUse(rescue, true) || rescue.TieredRoundConversionUse!.DyingFrameId != dying.Id || rescue.DyingResponse is not { } token || token.ResolutionId != dying.Id || token.ResponderSeat != dying.ResponderSeat || rescue.SourceSeat != dying.ResponderSeat || rescue.CardKind is not (CardKind.Peach or CardKind.Alcohol) || !rescue.TargetSeats.SequenceEqual([dying.VictimSeat]) || rescue.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0 } action || action.ActorSeat != token.ResponderSeat || !action.EffectiveDesignatedTargetSeats.SequenceEqual([dying.VictimSeat]) || (rescue.CardKind == CardKind.Peach ? !token.UsedPeach || token.PeachCardId != 0 || token.UsedAlcohol || token.AlcoholCardId is not null : !token.UsedAlcohol || token.AlcoholCardId != 0 || token.UsedPeach || token.PeachCardId is not null || rescue.SourceSeat != dying.VictimSeat) || token.UsedPeachPhysicalCardKind is not null)
            return false;
        for (var child = dyingIndex + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
            if (!_coreParticipantHandDamageRidesOperations.ParticipantHandRescueObserverRide(_resolutionStackOperations._resolutionStack[child], _resolutionStackOperations._resolutionStack[child - 1], rescue))
                return false;
        return true;
    }

    internal bool HasTieredRoundZeroDyingRide(long dyingId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dyingId && f is DyingFrame);
        return index >= 0 && TieredRoundZeroDyingRescueRide(index, (DyingFrame)_resolutionStackOperations._resolutionStack[index]);
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 700)]
    internal bool IsTieredRoundZeroRescueProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && _resolutionStackOperations._resolutionStack.LastOrDefault() is ProgramSkillFrame && HasTieredRoundZeroDyingRide(dying.FrameId);

    internal bool IsTieredRoundZeroFireAttackUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0, CardKind: CardKind.FireAttack, PhysicalCardIds.Count: 0 } use && _coreTieredRoundZeroUsesOperations.IsIssuedTieredRoundUse(use, true);
    internal bool MatchesTieredRoundZeroFireAttackAttack(NativeOwners.CardAttackHandle attack) => IsTieredRoundZeroFireAttackUse(attack.ResolutionId) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { } use && attack.Card is null && attack.PhysicalCards.Count == 0 && attack.EffectiveCardKind == CardKind.FireAttack && attack.SourceSeat == use.SourceSeat && attack.CardUserSeat == use.SourceSeat && attack.ProgramSkillFrameId is null && attack.ProgramSkillCardUseFrameId is null && attack.ProgramJudgmentFrameId is null && !attack.IsDelayedJudgmentDamage && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FireAttackResolvedEvent>().LastOrDefault(fact => fact.ResolutionId == use.Id)is { CausedDamage: true, MatchingDiscardCardId: > 0 } paid && paid.SourceSeat == use.SourceSeat && use.TargetSeats.Contains(paid.TargetSeat);
    internal bool IsTieredRoundZeroProcessingParent(long id, int cardId) => cardId == 0 && (_characterHuanShuJunOlOperations.IsHuanpeiUse(id) || _characterLiuZhangOlOperations.IsZonghuUse(id) || _characterGuoTuOlOperations.IsWeijieUse(id) || (_characterGuanHaiOlOperations.IsYouqueUse(id) || _characterLuKangWumiaoOperations.IsWuLuKangKegouUse(id) || _characterHuCheErMouOperations.IsMouHuCheErUse(id) || _characterZhangXiuMouOperations.IsMouZhangXiuAnniversaryUse(id) || _characterHuLieMouOperations.IsMouHuLieUse(id)) || _characterTaoQianOlOperations.IsZongluanUse(id) || _coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(id) || _coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(id));
    internal bool MatchesTieredRoundZeroUseAction(long id, CardActionContext action, int cardId) => cardId == 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && _coreTieredRoundZeroUsesOperations.IsIssuedTieredRoundUse(use, true) && TieredRoundActionsStructurallyMatch(use.Action, action) && action.Type == CardActionType.Use && action.EffectiveKind == use.CardKind && action.PhysicalCards.Count == 0;

    internal static bool TieredRoundActionsStructurallyMatch(CardActionContext? left, CardActionContext right) => RuntimeSkills.RuntimeRules_Core_TieredRoundZeroReturns.TieredRoundActionsStructurallyMatch(left, right);
    internal bool SkipTieredRoundZeroFinishedMovement(long id, Card card)
    {
        if (card.Id != 0 || !_coreTieredRoundZeroUsesOperations.IsTieredRoundZeroUse(id) && !_characterQinLangOlOperations.IsQinLangXianyingUse(id) && !_characterAHuiNanOlOperations.Value.IsAHuiNanJuemanUse(id))
            return false;
        if (card.Kind != _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind)
            throw new InvalidOperationException("The logical completed card changed its issued card name.");
        return true; // No physical entity was issued; there is no cleanup move.
    }

    internal bool IsCappedConversionAwaitedMovement(ProgramSkillFrame frame, ProgramMovementContinuation pending) => frame.CappedConversionBenefit is { AwaitingMovement: true } && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && _coreCappedConversionBenefitsOperations.Value.ValidCappedConversionBenefit(frame);
}
