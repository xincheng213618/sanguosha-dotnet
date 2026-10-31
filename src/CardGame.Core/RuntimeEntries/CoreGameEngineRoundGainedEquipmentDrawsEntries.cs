#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundGainedEquipmentDrawsRuntime))]
internal sealed partial class CoreGameEngineRoundGainedEquipmentDrawsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundGainedEquipmentDrawsRuntime> _resolve;
    private CoreGameEngineRoundGainedEquipmentDrawsEntries(Func<EngineOwners.CoreGameEngineRoundGainedEquipmentDrawsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundGainedEquipmentDrawsEntries Create(Func<EngineOwners.CoreGameEngineRoundGainedEquipmentDrawsRuntime> resolve) => new(resolve);
}
