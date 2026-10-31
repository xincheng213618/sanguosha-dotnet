#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHpOrderedDistancePoliciesRuntime
{
    private readonly RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    internal CoreGameEngineHpOrderedDistancePoliciesRuntime(RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations)
    {
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
    }
}
