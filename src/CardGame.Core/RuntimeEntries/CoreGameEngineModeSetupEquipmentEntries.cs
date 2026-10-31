#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupEquipmentRuntime))]
internal sealed partial class CoreGameEngineModeSetupEquipmentEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupEquipmentRuntime> _resolve;
    private CoreGameEngineModeSetupEquipmentEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupEquipmentRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeSetupEquipmentEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeSetupEquipmentRuntime> resolve) => new(resolve);
}
