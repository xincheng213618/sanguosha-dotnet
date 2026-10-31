#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime))]
internal sealed partial class CoreGameEngineOriginalHandAwakeningEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> _resolve;
    private CoreGameEngineOriginalHandAwakeningEntries(Func<EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOriginalHandAwakeningEntries Create(Func<EngineOwners.CoreGameEngineOriginalHandAwakeningRuntime> resolve) => new(resolve);
}
