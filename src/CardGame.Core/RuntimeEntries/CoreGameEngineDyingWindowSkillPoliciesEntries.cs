#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime))]
internal sealed partial class CoreGameEngineDyingWindowSkillPoliciesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> _resolve;
    private CoreGameEngineDyingWindowSkillPoliciesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingWindowSkillPoliciesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingWindowSkillPoliciesRuntime> resolve) => new(resolve);
}
