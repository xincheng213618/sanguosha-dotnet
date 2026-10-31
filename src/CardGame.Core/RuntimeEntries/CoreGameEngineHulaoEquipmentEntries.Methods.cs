#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHulaoEquipmentEntries
{
    internal bool AllowsHulaoEquipmentTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsHulaoEquipmentTrigger(gate);
    internal string? HulaoEquipmentSkillGrantId(global::CardGame.Core.Card card) => _resolve().HulaoEquipmentSkillGrantId(card);
    internal string? HulaoEquipmentSkillId(global::CardGame.Core.CardKind kind) => _resolve().HulaoEquipmentSkillId(kind);
}
