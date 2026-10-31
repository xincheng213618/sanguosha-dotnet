#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed partial class CoreGameEngineProgramCardPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _resolve;
    private CoreGameEngineProgramCardPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> resolve) => new(resolve);
}
