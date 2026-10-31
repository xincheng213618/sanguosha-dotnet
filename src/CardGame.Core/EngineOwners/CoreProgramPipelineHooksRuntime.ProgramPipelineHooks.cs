#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPipelineHooksRuntime
{
    internal CardKind? ProgramVirtualEquipment(int seat, EquipmentSlot slot) => _programPipelineHooksOperations._programPipelineHooks.VirtualEquipment(seat, slot);
}
