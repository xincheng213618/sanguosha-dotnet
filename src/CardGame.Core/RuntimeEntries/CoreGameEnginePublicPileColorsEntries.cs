#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileColorsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime> _resolve;
    private CoreGameEnginePublicPileColorsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileColorsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime> resolve) => new(resolve);
}
