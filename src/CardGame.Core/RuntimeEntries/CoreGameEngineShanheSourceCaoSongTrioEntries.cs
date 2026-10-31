#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime))]
internal sealed partial class CoreGameEngineShanheSourceCaoSongTrioEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime> _resolve;
    private CoreGameEngineShanheSourceCaoSongTrioEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceCaoSongTrioEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceCaoSongTrioRuntime> resolve) => new(resolve);
}
