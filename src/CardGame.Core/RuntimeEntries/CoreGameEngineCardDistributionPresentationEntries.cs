#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime))]
internal sealed partial class CoreGameEngineCardDistributionPresentationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> _resolve;
    private CoreGameEngineCardDistributionPresentationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardDistributionPresentationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardDistributionPresentationRuntime> resolve) => new(resolve);
}
