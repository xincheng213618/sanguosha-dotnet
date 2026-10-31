#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineModeSetupEquipmentRuntime))]
internal sealed partial class CoreGameEngineModeSetupEquipmentEntries
{
    private readonly Func<EngineOwners.CoreGameEngineModeSetupEquipmentRuntime> _resolve;
    private CoreGameEngineModeSetupEquipmentEntries(Func<EngineOwners.CoreGameEngineModeSetupEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeSetupEquipmentEntries Create(Func<EngineOwners.CoreGameEngineModeSetupEquipmentRuntime> resolve) => new(resolve);
}
