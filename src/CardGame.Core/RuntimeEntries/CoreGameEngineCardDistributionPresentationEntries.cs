#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardDistributionPresentationRuntime))]
internal sealed partial class CoreGameEngineCardDistributionPresentationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> _resolve;
    private CoreGameEngineCardDistributionPresentationEntries(Func<EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardDistributionPresentationEntries Create(Func<EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> resolve) => new(resolve);
}
