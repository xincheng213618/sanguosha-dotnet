#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheDeathEquipmentEntries
{
    private readonly Func<EngineOwners.CoreProgramShanheDeathEquipmentRuntime> _resolve;
    private CoreProgramShanheDeathEquipmentEntries(Func<EngineOwners.CoreProgramShanheDeathEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramShanheDeathEquipmentEntries Create(Func<EngineOwners.CoreProgramShanheDeathEquipmentRuntime> resolve) => new(resolve);
}
