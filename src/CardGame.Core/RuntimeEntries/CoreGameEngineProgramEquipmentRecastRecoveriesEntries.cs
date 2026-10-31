#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime))]
internal sealed partial class CoreGameEngineProgramEquipmentRecastRecoveriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime> _resolve;
    private CoreGameEngineProgramEquipmentRecastRecoveriesEntries(Func<EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramEquipmentRecastRecoveriesEntries Create(Func<EngineOwners.CoreGameEngineProgramEquipmentRecastRecoveriesRuntime> resolve) => new(resolve);
}
