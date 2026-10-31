#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundGainedEquipmentDrawsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentDrawsRuntime> _resolve;
    private CoreGameEngineRoundGainedEquipmentDrawsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentDrawsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundGainedEquipmentDrawsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundGainedEquipmentDrawsRuntime> resolve) => new(resolve);
}
