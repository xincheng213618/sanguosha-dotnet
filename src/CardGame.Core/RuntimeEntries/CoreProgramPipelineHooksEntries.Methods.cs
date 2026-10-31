#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPipelineHooksEntries
{
    internal global::CardGame.Core.CardKind? ProgramVirtualEquipment(int seat, global::CardGame.Core.EquipmentSlot slot) => _resolve().ProgramVirtualEquipment(seat, slot);
}
