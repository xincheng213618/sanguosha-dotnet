#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime))]
internal sealed partial class CoreGameEngineMultiplayerEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> _resolve;
    private CoreGameEngineMultiplayerEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMultiplayerEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMultiplayerRuntime> resolve) => new(resolve);
}
