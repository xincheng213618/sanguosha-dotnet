#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime))]
internal sealed partial class CoreGameEngineCapturedEquipmentDrawsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> _resolve;
    private CoreGameEngineCapturedEquipmentDrawsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCapturedEquipmentDrawsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> resolve) => new(resolve);
}
