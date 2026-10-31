#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardDamageMaximumHpReplacementRuntime
{
    internal int CalculateCardDamageLayer(GameEngine.IDamageAttempt attack)
    {
        if (attack is not NativeOwners.CardAttackHandle card || attack.IsChainPropagation || attack.IsDelayedJudgmentDamage || attack.IsProgramJudgmentDamage || attack.EffectiveCardKind is not { } kind)
            return attack.DamageAmount;
        if (_coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).CardDamageLayerAmount is { } issued)
            return issued;
        if (attack.DamageAmountFinalized)
            throw new InvalidOperationException("Card damage must be selected before damage-only modifiers and prevention.");
        var turn = _turnCardUseEffectsOperations._turnCardUseEffects.GetDamageModifiers(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, attack.SourceSeat, attack.CardUserSeat, kind, false).Sum(m => m.Amount);
        var passive = _coreNativeDamagePoliciesOperations.GetPassiveProgramDamageModifiers(attack, cardDamageOnly: true).Sum(m => m.Amount);
        var amount = checked(attack.DamageAmount + _coreExtendedViewAsOperations.ProgramConversionDamageBonus(card) + turn + passive + _characterPanZhangMaZhongBoundaryMobileOperations.Value.BoundaryAnjianDamage(attack) + _characterChengPuBoundaryMobileOperations.Value.BoundaryChengPuLihuoDamage(attack) + _characterHuangZhongBoundaryMobileOperations.Value.BoundaryHuangZhongCardDamage(attack) + _characterZhangFeiBoundaryMobileOperations.Value.BoundaryZhangFeiLiyongCardDamage(attack));
        _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { CardDamageLayerAmount = amount });
        return amount;
    }

    internal static string MaximumHpReplacementUsage(string policyId) => RuntimeSkills.RuntimeRules_Core_CardDamageMaximumHpReplacement.MaximumHpReplacementUsage(policyId);
    internal bool TryReplaceCardDamageWithMaximumHpLoss(GameEngine.IDamageAttempt attack, out bool pending)
    {
        pending = false;
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.FirstRoundCardDamageBecomesMaximumHpLoss) || attack is not NativeOwners.CardAttackHandle card || attack.IsSourceLess || attack.IsDelayedJudgmentDamage || attack.IsProgramJudgmentDamage || attack.IsChainPropagation || attack.DamageAmountFinalized || attack.CardUserSeat != attack.SourceSeat || attack.EffectiveCardKind is not { } kind || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].HasHpValue || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1)
            return false;
        foreach (var binding in _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], SkillProgramCardPolicyKind.FirstRoundCardDamageBecomesMaximumHpLoss, kind))
        {
            var usage = MaximumHpReplacementUsage(binding.Policy.Id);
            if (_skillRuntimeStateOperations._skillRuntimeState.GetUsage(attack.SourceSeat, binding.Source.SkillId, usage, SkillUsageScope.Round) != 0)
                continue;
            var amount = CalculateCardDamageLayer(card);
            if (amount <= 0)
                return false;
            if (_coreIdentityConcealmentOperations.TryBeginIdentityAppearanceForDamage(attack, amount))
            {
                pending = true;
                return true;
            }

            var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
            var afterMaximum = target.HasShenPangTongKunyu ? 1 : Math.Max(0, target.MaxHp - amount);
            var qiaoMaximum = _characterDaQiaoXiaoQiaoHappyGodOperations.PrepareHappyQiaoCardMaximumHp(attack, afterMaximum);
            int? hpBefore = qiaoMaximum is null ? target.Hp : null;
            var receipt = new CardDamageMaximumHpReplacementReceipt(new(binding.Source.SkillId, binding.Policy.Id, attack.SourceSeat, binding.Source.SkillInstanceId), _coreActualRoundProgressRoundNumberAccess._roundNumber, attack.SourceSeat, target.Seat, kind, amount, target.MaxHp, afterMaximum, hpBefore, hpBefore is { } beforeHp ? Math.Min(beforeHp, afterMaximum) : null)
            {
                HappyQiaoMaximumProof = qiaoMaximum
            };
            if (_coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).MaximumHpReplacement is not null || !_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(attack.SourceSeat, binding.Source.SkillId, usage, SkillUsageScope.Round, 1))
                throw new InvalidOperationException("The card damage replacement was already issued.");
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, state => state!with { MaximumHpReplacement = receipt });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(attack.SourceSeat, binding.Source.SkillId, usage, SkillUsageScope.Round, 1));
            if (!_characterDaQiaoXiaoQiaoHappyGodOperations.ApplyHappyQiaoCardMaximumHp(attack.ResolutionId, receipt))
            {
                target.MaxHp = receipt.MaximumHpAfter;
                target.Hp = receipt.HpAfter ?? throw new InvalidOperationException("Ordinary maximum-HP replacement lost its HP payment.");
            }

            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardDamageReplacedWithMaximumHpLossEvent(attack.ResolutionId, receipt));
            _coreMatchLogOperations.AddLog("MaximumHpLost", $"{(target.Name)} 的体力上限减少 {(receipt.MaximumHpBefore - receipt.MaximumHpAfter)} 点，剩余 {(target.MaxHp)}。", attack.SourceSeat, target.Seat);
            if (target.MaxHp == 0)
            {
                // Death owns its cleanup and normal death windows. It marks the
                // victim dead before publishing the max-HP fact, so a threshold
                // observer cannot transform a zero-maximum living character.
                pending = true;
                _coreNativePlayerDeathOperations.BeginPlayerDeath(attack.ResolutionId, target, null, attack, null, causingCardDamageReplacementId: attack.ResolutionId);
            }
            else
            {
                PublishMaximumHpDamageReplacement(attack.ResolutionId);
                pending = _characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoCardMaximumHealth(attack.ResolutionId);
            }

            return true;
        }

        return false;
    }

    internal void PublishMaximumHpDamageReplacement(long ownerId)
    {
        var receipt = _coreCardAttackStateOperations.GetCardAttackState(ownerId).MaximumHpReplacement ?? throw new InvalidOperationException("Maximum-HP replacement lost its owning attack receipt.");
        if (_characterDaQiaoXiaoQiaoHappyGodOperations.PublishHappyQiaoCardMaximumHp(ownerId, receipt))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(receipt.TargetSeat, receipt.MaximumHpAfter - receipt.MaximumHpBefore, receipt.MaximumHpAfter, receipt.Source.SkillId) { HpBeforeClamp = receipt.HpBefore });
    }

    internal bool IsExactMaximumHpReplacementDeathParent(DeathFrame death, ResolutionFrame parent)
    {
        if (parent.Id != death.ParentFrameId || parent is not (CardUseFrame or ProgramSkillFrame) || _coreCardAttackStateOperations.GetCardAttackState(parent.Id)is not { MaximumHpReplacement: { } receipt } state)
            return false;
        return receipt.MaximumHpAfter == 0 && receipt.TargetSeat == death.VictimSeat && death.KillerSeat is null && state.SourceSeat == receipt.SourceSeat && state.TargetSeat == receipt.TargetSeat && !state.DamageWasApplied && !state.DamageAmountFinalized && !state.IsChainPropagation && _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].MaxHp == 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardDamageReplacedWithMaximumHpLossEvent>().Count(e => e.ParentResolutionId == parent.Id && e.Receipt == receipt) == 1;
    }
}
