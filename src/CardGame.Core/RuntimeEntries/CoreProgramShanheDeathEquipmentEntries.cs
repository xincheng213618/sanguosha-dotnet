#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramShanheDeathEquipmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheDeathEquipmentRuntime> _resolve;
    private CoreProgramShanheDeathEquipmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheDeathEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramShanheDeathEquipmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramShanheDeathEquipmentRuntime> resolve) => new(resolve);
}
