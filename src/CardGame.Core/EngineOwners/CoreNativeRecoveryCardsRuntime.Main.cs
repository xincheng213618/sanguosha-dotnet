#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeRecoveryCardsRuntime
{
    internal void ResolvePeach(CharacterState player, Card peach)
    {
        ResolvePeach(player, player, peach, allowDying: false);
    }

    internal void ResolvePeach(CharacterState source, CharacterState target, Card peach, bool allowDying, CardConversionSource? conversionSource = null, DyingResponseEvent? dyingResponse = null)
    {
        if ((!allowDying && !_coreTreatedWoundedOperations.IsRuleWounded(source)) || (allowDying && target.Hp > 0) || !target.IsAlive)
        {
            throw new InvalidOperationException("This Peach target cannot currently be recovered.");
        }

        var recoveryPolicies = allowDying && source.Seat != target.Seat ? _coreProgramCardPoliciesOperations.CardPolicies(target, SkillProgramCardPolicyKind.RescueRecoveryBonus, CardKind.Peach).Where(item => string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(source), item.Policy.FactionId, StringComparison.Ordinal)).ToArray() : [];
        ResolveRecoveryCard(source, target, peach, "桃", playedCardKind: CardKind.Peach, recoveryAmount: 1 + recoveryPolicies.Sum(item => item.Policy.Value), recoveryPolicySources: recoveryPolicies.Select(item => (item.Source.SkillId, item.Policy.Id)).ToArray(), conversionSource: conversionSource, dyingResponse: dyingResponse);
    }

    internal void ResolveDyingAlcohol(CharacterState source, CharacterState target, Card alcohol, CardConversionSource? conversionSource = null, DyingResponseEvent? dyingResponse = null)
    {
        if ((source.Seat != target.Seat) || target.Hp > 0 || !target.IsAlive)
        {
            throw new InvalidOperationException("Alcohol can only rescue a living dying player.");
        }

        ResolveRecoveryCard(source, target, alcohol, "酒", playedCardKind: CardKind.Alcohol, conversionSource: conversionSource, dyingResponse: dyingResponse);
    }

    internal void ResolveRecoveryCard(CharacterState source, CharacterState target, Card card, string cardName, CardKind? playedCardKind = null, int recoveryAmount = 1, IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = null, CardConversionSource? conversionSource = null, DyingResponseEvent? dyingResponse = null, IReadOnlyList<Card>? physicalCards = null, DrawFundedDistinctBasicPayment? drawFundedPayment = null, HuanpeiPaidOrigin? huanpeiOrigin = null)
    {
        _coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._selectedUseConversion = conversionSource;
        _coreGameEngineCardConversionsHasSelectedUseConversionChoiceSelectedUseConversionAccess._hasSelectedUseConversionChoice = true;
        if (_coreCardDeclarationsOperations.TryBeginCardDeclaration(source, card, playedCardKind ?? card.Kind, _coreCardDeclarationsOperations.PeekDeclarationConversion(source, card, playedCardKind ?? card.Kind, true, conversionSource), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.Recovery, source.Seat, target: target.Seat, recovery: recoveryAmount, policies: recoveryPolicySources?.Select(policy => new ProgramRecoveryPolicySource(policy.SkillId, policy.PolicyId)).ToArray(), dying: dyingResponse), [target.Seat]))
            return;
        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(card, source.Seat, [target.Seat], playedCardKind: playedCardKind, conversionSource: conversionSource, physicalCardIds: physicalCards?.Select(item => item.Id).ToArray(), drawFundedPayment: drawFundedPayment, huanpeiOrigin: huanpeiOrigin);
        var useIndex = _resolutionStackOperations._resolutionStack.FindLastIndex(frame => frame.Id == resolutionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[useIndex].Id, ((CardUseFrame)_resolutionStackOperations._resolutionStack[useIndex])with { DyingResponse = dyingResponse });
        foreach (var physical in physicalCards ?? [card])
            _corePhysicalMovementFacadesOperations.MoveCard(physical, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, physical), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(resolutionId, new(card.Id, SimpleCardUseEffect.Recovery, recoveryAmount, recoveryPolicySources?.Select(policy => new ProgramRecoveryPolicySource(policy.SkillId, policy.PolicyId)).ToArray()));
    }

    internal void CompleteRecoveryCardUse(CharacterState source, CharacterState target, Card card, long resolutionId, CardKind playedCardKind, int recoveryAmount, IReadOnlyList<ProgramRecoveryPolicySource> recoveryPolicySources)
    {
        var cardName = CardCatalog.Get(playedCardKind).DisplayName;
        if (!target.IsAlive || !target.HasHpValue || !_characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoAnyWounded(target.Seat) || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(resolutionId, target.Seat))
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(resolutionId, card, playedCardKind);
            return;
        }

        _characterSunQuanMobileOperations.Value.ApplyNativePeachRecoveryScope(resolutionId, source, target, playedCardKind, ref recoveryAmount, ref recoveryPolicySources);
        recoveryAmount = _coreCardRecoveryModifiersOperations.ApplyIssuedCardRecoveryModifiers(resolutionId, source.Seat, target.Seat, recoveryAmount);
        if (_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(resolutionId, source.Seat, target.Seat, recoveryAmount, new(RecoveryAttemptProducer.CardUse, CardId: card.Id, CardKind: playedCardKind, Policies: recoveryPolicySources)))
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(resolutionId, card, playedCardKind);
            return;
        }

        var recoveryFrameId = _coreNativeRecoveryFramesOperations.BeginRecovery(resolutionId, source.Seat, target.Seat, recoveryAmount);
        try
        {
            var hpBefore = target.Hp;
            target.Hp = Math.Min(target.MaxHp, target.Hp + recoveryAmount);
            _coreNativeRecoveryDamageOperations.RecordNativeCardRecovery(resolutionId, recoveryFrameId, source.Seat, target.Seat, playedCardKind, recoveryAmount, hpBefore, target.Hp);
            foreach (var policy in recoveryPolicySources ?? [])
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramRecoveryPolicyAppliedEvent(resolutionId, target.Seat, source.Seat, card.Id, recoveryAmount, policy.SkillId, policy.PolicyId));
            }

            _coreMatchLogOperations.AddLog("Recovered", $"{(source.Name)} 使用【{(cardName)}】使 {(target.Name)} 回复 {(recoveryAmount)} 点体力，至 {(target.Hp)}/{(target.MaxHp)}。", source.Seat, target.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(source.Seat, target.Seat, recoveryAmount, target.Hp));
        }
        finally
        {
            _coreRuntimeOperations.PopResolutionFrame(recoveryFrameId, ResolutionFrameKind.Recovery);
        }

        var recoveryUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == resolutionId);
        var physicalIds = recoveryUse.PhysicalCardIds ?? [card.Id];
        var retainMaterials = _coreNextCardTargetAdjustmentsOperations.HasRemainingAdjustedSimpleTargets(resolutionId) || _coreExtraCardEffectRoundsOperations.HasUnstartedExtraCardEffect(resolutionId);
        if (!retainMaterials)
            _characterXunXuAnniversaryOperations.TryFinishAnniversaryXunXuUseMaterials(recoveryUse);
        foreach (var id in retainMaterials ? [] : physicalIds)
            if (_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).SingleOrDefault(item => item.Id == id)is { } physical)
                _corePhysicalMovementFacadesOperations.MoveCard(physical, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
        _coreNativeCardUseCompletionOperations.FinishCardUse(resolutionId, card, playedCardKind);
    }
}
