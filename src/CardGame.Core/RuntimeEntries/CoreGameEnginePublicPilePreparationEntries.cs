#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPilePreparationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPilePreparationRuntime> _resolve;
    private CoreGameEnginePublicPilePreparationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPilePreparationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPilePreparationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPilePreparationRuntime> resolve) => new(resolve);
}
