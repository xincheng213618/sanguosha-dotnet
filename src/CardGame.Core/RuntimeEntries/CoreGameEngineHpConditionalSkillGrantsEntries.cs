#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime))]
internal sealed partial class CoreGameEngineHpConditionalSkillGrantsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> _resolve;
    private CoreGameEngineHpConditionalSkillGrantsEntries(Func<EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHpConditionalSkillGrantsEntries Create(Func<EngineOwners.CoreGameEngineHpConditionalSkillGrantsRuntime> resolve) => new(resolve);
}
