#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLiushiEffectsRuntime))]
internal sealed partial class CoreGameEngineLiushiEffectsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLiushiEffectsRuntime> _resolve;
    private CoreGameEngineLiushiEffectsEntries(Func<EngineOwners.CoreGameEngineLiushiEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLiushiEffectsEntries Create(Func<EngineOwners.CoreGameEngineLiushiEffectsRuntime> resolve) => new(resolve);
}
