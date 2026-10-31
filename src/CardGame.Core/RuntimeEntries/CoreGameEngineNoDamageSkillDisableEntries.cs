#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime))]
internal sealed partial class CoreGameEngineNoDamageSkillDisableEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> _resolve;
    private CoreGameEngineNoDamageSkillDisableEntries(Func<EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNoDamageSkillDisableEntries Create(Func<EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> resolve) => new(resolve);
}
