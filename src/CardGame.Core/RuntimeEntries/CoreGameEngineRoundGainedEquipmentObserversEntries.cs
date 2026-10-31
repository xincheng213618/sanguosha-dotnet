#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime))]
internal sealed partial class CoreGameEngineRoundGainedEquipmentObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> _resolve;
    private CoreGameEngineRoundGainedEquipmentObserversEntries(Func<EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundGainedEquipmentObserversEntries Create(Func<EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> resolve) => new(resolve);
}
