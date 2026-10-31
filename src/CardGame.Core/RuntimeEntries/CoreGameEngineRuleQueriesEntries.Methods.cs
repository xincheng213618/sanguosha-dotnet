#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRuleQueriesEntries
{
    internal RuleQueryEvaluation CalculateAttackRange(CharacterState player, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().CalculateAttackRange(player, excludedEquipmentId, excludedEquipmentSlot);
    internal RuleQueryEvaluation CalculateDistance(CharacterState source, CharacterState target, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().CalculateDistance(source, target, excludedEquipmentId, excludedEquipmentSlot);
    internal RuleQueryEvaluation CalculateSlashUseLimit(CharacterState player, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().CalculateSlashUseLimit(player, excludedEquipmentId, excludedEquipmentSlot);
    internal bool CanSpendSlashUse(CharacterState player, CharacterState target, bool ignoresCount, CardKind effectiveKind = (CardKind)0, Card? physicalCard = default, int? effectiveRank = default, int? excludedEquipmentId = default, IReadOnlyList<int>? physicalCardIds = default, EquipmentSlot? excludedEquipmentSlot = default, bool ignoreMoQuanyu = false, Suit? effectiveSuit = default) => _resolve().CanSpendSlashUse(player, target, ignoresCount, effectiveKind, physicalCard, effectiveRank, excludedEquipmentId, physicalCardIds, excludedEquipmentSlot, ignoreMoQuanyu, effectiveSuit);
    internal IReadOnlyList<RuleQueryContribution> CollectNumericRuleContributions(CharacterState player, SkillRuleQuery query, CardKind? effectiveCardKind = default) => _resolve().CollectNumericRuleContributions(player, query, effectiveCardKind);
    internal RuleQueryEvaluation EvaluateCardTargetCount(CharacterState player, CardKind effectiveCardKind, long? declaredUseFrameId = default) => _resolve().EvaluateCardTargetCount(player, effectiveCardKind, declaredUseFrameId);
    internal RuleQueryEvaluation EvaluateDrawCount(CharacterState player) => _resolve().EvaluateDrawCount(player);
    internal RuleQueryEvaluation EvaluateHandLimit(CharacterState player, bool suppressHanSongYinbi = false) => _resolve().EvaluateHandLimit(player, suppressHanSongYinbi);
    internal int GetCardUseDistanceLimit(CharacterState player, CardKind effectiveCardKind) => _resolve().GetCardUseDistanceLimit(player, effectiveCardKind);
    internal int GetLivingFactionCount() => _resolve().GetLivingFactionCount();
    internal int GetLivingPlayersMinHp() => _resolve().GetLivingPlayersMinHp();
    internal int GetSlashUseLimit(CharacterState player, int? excludedEquipmentId = default, EquipmentSlot? excludedEquipmentSlot = default) => _resolve().GetSlashUseLimit(player, excludedEquipmentId, excludedEquipmentSlot);
}
