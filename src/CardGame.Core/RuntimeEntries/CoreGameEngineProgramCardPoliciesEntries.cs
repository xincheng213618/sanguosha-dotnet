#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramCardPoliciesRuntime))]
internal sealed partial class CoreGameEngineProgramCardPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> _resolve;
    private CoreGameEngineProgramCardPoliciesEntries(Func<EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramCardPoliciesEntries Create(Func<EngineOwners.CoreGameEngineProgramCardPoliciesRuntime> resolve) => new(resolve);
}
