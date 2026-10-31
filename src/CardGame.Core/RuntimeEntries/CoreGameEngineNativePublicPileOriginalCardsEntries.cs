#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed partial class CoreGameEngineNativePublicPileOriginalCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _resolve;
    private CoreGameEngineNativePublicPileOriginalCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativePublicPileOriginalCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> resolve) => new(resolve);
}
