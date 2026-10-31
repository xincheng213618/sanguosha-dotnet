#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed partial class CoreGameEngineDyingWindowSkillPoliciesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _resolve;
    private CoreGameEngineDyingWindowSkillPoliciesEntries(Func<EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingWindowSkillPoliciesEntries Create(Func<EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> resolve) => new(resolve);
}
