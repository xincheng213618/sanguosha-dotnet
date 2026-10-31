#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime))]
internal sealed partial class CoreGameEngineProgramEquipmentRecastRecoveriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime> _resolve;
    private CoreGameEngineProgramEquipmentRecastRecoveriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramEquipmentRecastRecoveriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime> resolve) => new(resolve);
}
