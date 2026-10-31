#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineMatchingRecastsRuntime))]
internal sealed partial class CoreGameEngineMatchingRecastsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineMatchingRecastsRuntime> _resolve;
    private CoreGameEngineMatchingRecastsEntries(Func<EngineOwners.CoreGameEngineMatchingRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMatchingRecastsEntries Create(Func<EngineOwners.CoreGameEngineMatchingRecastsRuntime> resolve) => new(resolve);
}
