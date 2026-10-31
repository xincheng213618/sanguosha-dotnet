#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime))]
internal sealed partial class CoreGameEngineAlcoholSlashSkillReplacementsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime> _resolve;
    private CoreGameEngineAlcoholSlashSkillReplacementsEntries(Func<EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlcoholSlashSkillReplacementsEntries Create(Func<EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime> resolve) => new(resolve);
}
