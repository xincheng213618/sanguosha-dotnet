#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime))]
internal sealed partial class CoreGameEngineModeCardRecastsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime> _resolve;
    private CoreGameEngineModeCardRecastsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeCardRecastsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineModeCardRecastsRuntime> resolve) => new(resolve);
}
