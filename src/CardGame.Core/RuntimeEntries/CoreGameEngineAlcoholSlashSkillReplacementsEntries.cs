#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime))]
internal sealed partial class CoreGameEngineAlcoholSlashSkillReplacementsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime> _resolve;
    private CoreGameEngineAlcoholSlashSkillReplacementsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAlcoholSlashSkillReplacementsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAlcoholSlashSkillReplacementsRuntime> resolve) => new(resolve);
}
