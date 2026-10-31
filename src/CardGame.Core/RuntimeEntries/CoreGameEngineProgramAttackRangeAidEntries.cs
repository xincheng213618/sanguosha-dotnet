#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime))]
internal sealed partial class CoreGameEngineProgramAttackRangeAidEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> _resolve;
    private CoreGameEngineProgramAttackRangeAidEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramAttackRangeAidEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramAttackRangeAidRuntime> resolve) => new(resolve);
}
