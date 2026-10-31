#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndRefillNextTurnEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> _resolve;
    private CoreGameEngineEndRefillNextTurnEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEndRefillNextTurnEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEndRefillNextTurnRuntime> resolve) => new(resolve);
}
