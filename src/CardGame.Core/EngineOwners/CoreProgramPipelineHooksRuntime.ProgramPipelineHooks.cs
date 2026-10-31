#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPipelineHooksRuntime
{
    internal CardKind? ProgramVirtualEquipment(int seat, EquipmentSlot slot) => _programPipelineHooksOperations._programPipelineHooks.VirtualEquipment(seat, slot);
}
