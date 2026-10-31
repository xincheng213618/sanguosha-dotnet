#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime))]
internal sealed partial class CoreGameEngineGuanduExtraEffectsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime> _resolve;
    private CoreGameEngineGuanduExtraEffectsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduExtraEffectsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduExtraEffectsRuntime> resolve) => new(resolve);
}
