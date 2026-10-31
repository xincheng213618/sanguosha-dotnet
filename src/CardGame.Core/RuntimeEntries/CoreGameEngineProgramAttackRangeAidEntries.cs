#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime))]
internal sealed partial class CoreGameEngineProgramAttackRangeAidEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> _resolve;
    private CoreGameEngineProgramAttackRangeAidEntries(Func<EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAttackRangeAidEntries Create(Func<EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> resolve) => new(resolve);
}
