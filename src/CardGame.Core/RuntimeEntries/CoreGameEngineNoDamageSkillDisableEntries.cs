#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime))]
internal sealed partial class CoreGameEngineNoDamageSkillDisableEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> _resolve;
    private CoreGameEngineNoDamageSkillDisableEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNoDamageSkillDisableEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> resolve) => new(resolve);
}
