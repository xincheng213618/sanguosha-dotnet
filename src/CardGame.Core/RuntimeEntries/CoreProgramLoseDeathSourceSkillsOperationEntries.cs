#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramLoseDeathSourceSkillsOperationEntries
{
    private readonly Func<EngineOwners.CoreProgramLoseDeathSourceSkillsOperationRuntime> _resolve;
    private CoreProgramLoseDeathSourceSkillsOperationEntries(Func<EngineOwners.CoreProgramLoseDeathSourceSkillsOperationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramLoseDeathSourceSkillsOperationEntries Create(Func<EngineOwners.CoreProgramLoseDeathSourceSkillsOperationRuntime> resolve) => new(resolve);
}
