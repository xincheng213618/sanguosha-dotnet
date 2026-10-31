#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime))]
internal sealed partial class CoreGameEngineCapturedEquipmentDrawsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> _resolve;
    private CoreGameEngineCapturedEquipmentDrawsEntries(Func<EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCapturedEquipmentDrawsEntries Create(Func<EngineOwners.CoreGameEngineCapturedEquipmentDrawsRuntime> resolve) => new(resolve);
}
