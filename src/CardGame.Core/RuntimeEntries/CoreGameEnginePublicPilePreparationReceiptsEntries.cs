#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPilePreparationReceiptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPilePreparationReceiptsRuntime> _resolve;
    private CoreGameEnginePublicPilePreparationReceiptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPilePreparationReceiptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPilePreparationReceiptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPilePreparationReceiptsRuntime> resolve) => new(resolve);
}
