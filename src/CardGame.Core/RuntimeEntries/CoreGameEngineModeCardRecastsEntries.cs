#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineModeCardRecastsRuntime))]
internal sealed partial class CoreGameEngineModeCardRecastsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineModeCardRecastsRuntime> _resolve;
    private CoreGameEngineModeCardRecastsEntries(Func<EngineOwners.CoreGameEngineModeCardRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineModeCardRecastsEntries Create(Func<EngineOwners.CoreGameEngineModeCardRecastsRuntime> resolve) => new(resolve);
}
