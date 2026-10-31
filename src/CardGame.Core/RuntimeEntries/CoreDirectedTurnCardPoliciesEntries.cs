#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime))]
internal sealed partial class CoreDirectedTurnCardPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> _resolve;
    private CoreDirectedTurnCardPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDirectedTurnCardPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreDirectedTurnCardPoliciesRuntime> resolve) => new(resolve);
}
