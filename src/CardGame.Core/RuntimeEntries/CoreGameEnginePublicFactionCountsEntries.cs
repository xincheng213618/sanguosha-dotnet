#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicFactionCountsRuntime))]
internal sealed partial class CoreGameEnginePublicFactionCountsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicFactionCountsRuntime> _resolve;
    private CoreGameEnginePublicFactionCountsEntries(Func<EngineOwners.CoreGameEnginePublicFactionCountsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicFactionCountsEntries Create(Func<EngineOwners.CoreGameEnginePublicFactionCountsRuntime> resolve) => new(resolve);
}
