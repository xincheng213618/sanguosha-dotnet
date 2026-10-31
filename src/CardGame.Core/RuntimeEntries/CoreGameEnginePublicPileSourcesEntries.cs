#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime))]
internal sealed partial class CoreGameEnginePublicPileSourcesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime> _resolve;
    private CoreGameEnginePublicPileSourcesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileSourcesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime> resolve) => new(resolve);
}
