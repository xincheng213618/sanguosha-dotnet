#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEquipmentSuitHandLimitEntries
{
    internal RuleQueryEvaluation ApplyEquipmentSuitHandLimit(CharacterState player, RuleQueryEvaluation ordinary) => _resolve().ApplyEquipmentSuitHandLimit(player, ordinary);
}
