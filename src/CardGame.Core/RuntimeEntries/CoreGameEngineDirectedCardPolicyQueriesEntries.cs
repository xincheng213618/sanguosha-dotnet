#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime))]
internal sealed partial class CoreGameEngineDirectedCardPolicyQueriesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> _resolve;
    private CoreGameEngineDirectedCardPolicyQueriesEntries(Func<EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDirectedCardPolicyQueriesEntries Create(Func<EngineOwners.CoreGameEngineDirectedCardPolicyQueriesRuntime> resolve) => new(resolve);
}
