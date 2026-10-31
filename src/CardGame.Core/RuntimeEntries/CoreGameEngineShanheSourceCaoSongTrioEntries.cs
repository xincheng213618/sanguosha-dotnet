#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime))]
internal sealed partial class CoreGameEngineShanheSourceCaoSongTrioEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime> _resolve;
    private CoreGameEngineShanheSourceCaoSongTrioEntries(Func<EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceCaoSongTrioEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime> resolve) => new(resolve);
}
