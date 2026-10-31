#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime))]
internal sealed partial class CoreGameEngineWenheChaosEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> _resolve;
    private CoreGameEngineWenheChaosEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWenheChaosEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWenheChaosRuntime> resolve) => new(resolve);
}
