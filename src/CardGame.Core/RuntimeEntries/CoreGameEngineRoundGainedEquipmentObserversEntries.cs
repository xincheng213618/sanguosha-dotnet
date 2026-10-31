#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime))]
internal sealed partial class CoreGameEngineRoundGainedEquipmentObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> _resolve;
    private CoreGameEngineRoundGainedEquipmentObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundGainedEquipmentObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentObserversRuntime> resolve) => new(resolve);
}
