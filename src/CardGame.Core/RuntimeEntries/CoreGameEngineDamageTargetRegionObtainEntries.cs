#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime))]
internal sealed partial class CoreGameEngineDamageTargetRegionObtainEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime> _resolve;
    private CoreGameEngineDamageTargetRegionObtainEntries(Func<EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetRegionObtainEntries Create(Func<EngineOwners.CoreGameEngineDamageTargetRegionObtainRuntime> resolve) => new(resolve);
}
