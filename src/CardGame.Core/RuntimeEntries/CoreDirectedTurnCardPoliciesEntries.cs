#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed partial class CoreDirectedTurnCardPoliciesEntries
{
    private readonly Func<EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _resolve;
    private CoreDirectedTurnCardPoliciesEntries(Func<EngineOwners.CoreDirectedTurnCardPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDirectedTurnCardPoliciesEntries Create(Func<EngineOwners.CoreDirectedTurnCardPoliciesRuntime> resolve) => new(resolve);
}
