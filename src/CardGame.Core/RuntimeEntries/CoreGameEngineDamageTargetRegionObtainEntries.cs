#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime))]
internal sealed partial class CoreGameEngineDamageTargetRegionObtainEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime> _resolve;
    private CoreGameEngineDamageTargetRegionObtainEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetRegionObtainEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime> resolve) => new(resolve);
}
