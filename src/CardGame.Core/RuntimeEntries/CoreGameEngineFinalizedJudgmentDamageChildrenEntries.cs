#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime))]
internal sealed partial class CoreGameEngineFinalizedJudgmentDamageChildrenEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> _resolve;
    private CoreGameEngineFinalizedJudgmentDamageChildrenEntries(Func<EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalizedJudgmentDamageChildrenEntries Create(Func<EngineOwners.CoreGameEngineFinalizedJudgmentDamageChildrenRuntime> resolve) => new(resolve);
}
