#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime))]
internal sealed partial class CoreGameEngineOriginalHandAwakeningEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> _resolve;
    private CoreGameEngineOriginalHandAwakeningEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalHandAwakeningEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> resolve) => new(resolve);
}
