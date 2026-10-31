#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> _resolve;
    private CoreGameEngineGuanduCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduCardsRuntime> resolve) => new(resolve);
}
