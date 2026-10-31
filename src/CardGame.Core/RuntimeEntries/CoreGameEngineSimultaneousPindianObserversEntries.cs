#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianObserversRuntime))]
internal sealed partial class CoreGameEngineSimultaneousPindianObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianObserversRuntime> _resolve;
    private CoreGameEngineSimultaneousPindianObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSimultaneousPindianObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianObserversRuntime> resolve) => new(resolve);
}
