#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed partial class CorePhysicalOwnedCardQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _resolve;
    private CorePhysicalOwnedCardQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePhysicalOwnedCardQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePhysicalOwnedCardQueriesRuntime> resolve) => new(resolve);
}
