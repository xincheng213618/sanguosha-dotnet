#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineMultiplayerRuntime))]
internal sealed partial class CoreGameEngineMultiplayerEntries
{
    private readonly Func<EngineOwners.CoreGameEngineMultiplayerRuntime> _resolve;
    private CoreGameEngineMultiplayerEntries(Func<EngineOwners.CoreGameEngineMultiplayerRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMultiplayerEntries Create(Func<EngineOwners.CoreGameEngineMultiplayerRuntime> resolve) => new(resolve);
}
