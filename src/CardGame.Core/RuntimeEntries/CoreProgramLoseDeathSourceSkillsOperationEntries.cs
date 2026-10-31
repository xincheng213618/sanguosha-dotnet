#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramLoseDeathSourceSkillsOperationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLoseDeathSourceSkillsOperationRuntime> _resolve;
    private CoreProgramLoseDeathSourceSkillsOperationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLoseDeathSourceSkillsOperationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreProgramLoseDeathSourceSkillsOperationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramLoseDeathSourceSkillsOperationRuntime> resolve) => new(resolve);
}
