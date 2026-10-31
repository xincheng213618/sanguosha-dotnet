#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed partial class CoreGameEngineRuntimeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> _resolve;
    private CoreGameEngineRuntimeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRuntimeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRuntimeRuntime> resolve) => new(resolve);
}
