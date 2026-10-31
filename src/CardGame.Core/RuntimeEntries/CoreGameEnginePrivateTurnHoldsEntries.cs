#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateTurnHoldsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> _resolve;
    private CoreGameEnginePrivateTurnHoldsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateTurnHoldsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateTurnHoldsRuntime> resolve) => new(resolve);
}
