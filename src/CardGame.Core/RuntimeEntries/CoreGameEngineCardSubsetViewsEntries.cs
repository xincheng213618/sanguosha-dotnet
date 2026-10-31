#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardSubsetViewsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardSubsetViewsRuntime> _resolve;
    private CoreGameEngineCardSubsetViewsEntries(Func<EngineOwners.CoreGameEngineCardSubsetViewsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardSubsetViewsEntries Create(Func<EngineOwners.CoreGameEngineCardSubsetViewsRuntime> resolve) => new(resolve);
}
