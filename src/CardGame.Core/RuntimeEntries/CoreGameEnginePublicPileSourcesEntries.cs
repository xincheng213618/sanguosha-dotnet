#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePublicPileSourcesRuntime))]
internal sealed partial class CoreGameEnginePublicPileSourcesEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicPileSourcesRuntime> _resolve;
    private CoreGameEnginePublicPileSourcesEntries(Func<EngineOwners.CoreGameEnginePublicPileSourcesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileSourcesEntries Create(Func<EngineOwners.CoreGameEnginePublicPileSourcesRuntime> resolve) => new(resolve);
}
