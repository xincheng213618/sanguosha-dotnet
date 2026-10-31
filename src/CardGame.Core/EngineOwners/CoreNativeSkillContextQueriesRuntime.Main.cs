#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeSkillContextQueriesRuntime
{
    internal bool HasProgramSkill(CharacterState player, string skillId) => _coreNationalSkillsOperations.GetSkillBindingShard(player).ProgramInstances.Any(instance => string.Equals(instance.SkillId, skillId, StringComparison.Ordinal));
    internal PlayerSkillContext CreateSkillContext(CharacterState player, bool includeHandLimit = false)
    {
        var context = new PlayerSkillContext(player.Seat, player.HappyQiaoHealth is null ? player.Hp : 0, player.MaxHp, _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count, _coreActualTurnProgressCurrentSeatPhaseAccess._phase, player.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat, player.IsFaceDown, player.IsChained, _coreNativeDamageHandlesOperations.UsesClassicBattleRules, _coreStrategicProgramsOperations.GetPublicProgramBooleanStates(player), _coreStrategicProgramsOperations.GetProgramPublicCounters(player), includeHandLimit ? _coreNativeRuleQueriesOperations.GetHandLimit(player) : null, includeHandLimit ? _coreProgramParticipantPhaseOperationsOperations.HasActuallyUsableHandCard(player) : null, _corePlayCardHistoryOperations.TracksPlayCardHistory ? _corePlayCardHistoryOperations.PreviousPlayCardIsBasicFor(player.Seat) : null)
        {
            HpValueProvider = player.HappyQiaoHealth is null ? null : () => player.Hp,
            Armor = player.Armor,
            HpUndefined = !player.HasHpValue,
            RuleWounded = _coreTreatedWoundedOperations.UsesTreatedWounded ? _coreTreatedWoundedOperations.IsRuleWounded(player) : null,
            HasWeapon = _characterHuCheErMouOperations.KongwuEffectiveEquipment(player).Any(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon)
        };
        context = context with
        {
            IncomingDamageCap = _corePassiveDamageCapsOperations.GetIncomingFixedDamageCap(player),
            DamageBecomesHpLoss = _corePublicDamagePredictionOperations.HasPredictionHpPolicy(player, SkillProgramCardPolicyKind.DamageBecomesHpLoss, context),
            OtherParticipantDamageBecomesHpLoss = _corePublicDamagePredictionOperations.HasPredictionHpPolicy(player, SkillProgramCardPolicyKind.OtherParticipantDamageBecomesHpLoss, context)
        };
        if (_contentRegistryOperations._contentRegistry.ProgramDependencies.EquipmentPresenceSkillIds.Count == 0 || !_coreNationalSkillsOperations.GetSkillBindingShard(player).ProgramInstances.Any(instance => _contentRegistryOperations._contentRegistry.ProgramDependencies.EquipmentPresenceSkillIds.Contains(instance.SkillId)))
            return context;
        var slots = _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Select(card => _coreFieldEquipmentOperations.FieldEquipmentSlot(card)).ToArray();
        return context with
        {
            ArmorPresent = slots.Contains(EquipmentSlot.Armor),
            MountPresent = slots.Any(slot => slot is EquipmentSlot.OffensiveHorse or EquipmentSlot.DefensiveHorse),
            TreasurePresent = slots.Contains(EquipmentSlot.Treasure)
        };
    }
}
