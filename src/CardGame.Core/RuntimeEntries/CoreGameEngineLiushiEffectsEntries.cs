#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLiushiEffectsRuntime))]
internal sealed partial class CoreGameEngineLiushiEffectsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiushiEffectsRuntime> _resolve;
    private CoreGameEngineLiushiEffectsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiushiEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLiushiEffectsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiushiEffectsRuntime> resolve) => new(resolve);
}
