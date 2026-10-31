#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPipelineHooksEntries
{
    internal CardKind? ProgramVirtualEquipment(int seat, EquipmentSlot slot) => _resolve().ProgramVirtualEquipment(seat, slot);
}
