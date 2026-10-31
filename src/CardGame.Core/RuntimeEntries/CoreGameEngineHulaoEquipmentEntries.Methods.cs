#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHulaoEquipmentEntries
{
    internal bool AllowsHulaoEquipmentTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsHulaoEquipmentTrigger(gate);
    internal string? HulaoEquipmentSkillGrantId(Card card) => _resolve().HulaoEquipmentSkillGrantId(card);
    internal string? HulaoEquipmentSkillId(CardKind kind) => _resolve().HulaoEquipmentSkillId(kind);
}
