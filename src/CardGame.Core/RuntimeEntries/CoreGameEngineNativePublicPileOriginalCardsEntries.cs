#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime))]
internal sealed partial class CoreGameEngineNativePublicPileOriginalCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> _resolve;
    private CoreGameEngineNativePublicPileOriginalCardsEntries(Func<EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativePublicPileOriginalCardsEntries Create(Func<EngineOwners.CoreGameEngineNativePublicPileOriginalCardsRuntime> resolve) => new(resolve);
}
