
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_da_qiao_le_LeDaQiaoZixiPlacement
{
    internal static CardConversionSource ZixiPlacementSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    internal static string ZixiSlotName(CardKind kind) => kind switch { CardKind.Indulgence => "indulgence", CardKind.SupplyShortage => "supplyShortage", CardKind.Lightning => "lightning", _ => throw new InvalidOperationException("Unknown Qin judgment slot.") };
}
