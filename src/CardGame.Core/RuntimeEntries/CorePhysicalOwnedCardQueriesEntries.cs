#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CorePhysicalOwnedCardQueriesRuntime))]
internal sealed partial class CorePhysicalOwnedCardQueriesEntries
{
    private readonly Func<EngineOwners.CorePhysicalOwnedCardQueriesRuntime> _resolve;
    private CorePhysicalOwnedCardQueriesEntries(Func<EngineOwners.CorePhysicalOwnedCardQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePhysicalOwnedCardQueriesEntries Create(Func<EngineOwners.CorePhysicalOwnedCardQueriesRuntime> resolve) => new(resolve);
}
