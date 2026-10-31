#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceCurseObserversRuntime))]
internal sealed partial class CoreGameEngineSourceCurseObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCurseObserversRuntime> _resolve;
    private CoreGameEngineSourceCurseObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCurseObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceCurseObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCurseObserversRuntime> resolve) => new(resolve);
}
