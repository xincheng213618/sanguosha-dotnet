#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNationalSkillsEntries
{
    internal bool CanOwnPrintedSkill(CharacterState player, GeneralDefinition general, GeneralSkillDefinition skill) => _resolve().CanOwnPrintedSkill(player, general, skill);
    internal IReadOnlyList<string> EnabledContentSkillIds(CharacterState player) => _resolve().EnabledContentSkillIds(player);
    internal IEnumerable<SkillGrant> EnabledNonTemplateSkillGrants(CharacterState player) => _resolve().EnabledNonTemplateSkillGrants(player);
    internal IReadOnlyList<string> EnabledPrintedContentSkillIds(CharacterState player) => _resolve().EnabledPrintedContentSkillIds(player);
    internal IEnumerable<SkillGrant> EnabledRuntimeSkillGrants(CharacterState player) => _resolve().EnabledRuntimeSkillGrants(player);
    internal string EnabledSkillNames(CharacterState player) => _resolve().EnabledSkillNames(player);
    internal string GetRuntimeSkillInstanceId(CharacterState player, string skillId) => _resolve().GetRuntimeSkillInstanceId(player, skillId);
    internal SkillBindingShard GetSkillBindingShard(CharacterState player) => _resolve().GetSkillBindingShard(player);
    internal bool HasRuntimeSkill(CharacterState player, string skillId) => _resolve().HasRuntimeSkill(player, skillId);
    internal bool HasRuntimeSkillInstance(CharacterState player, string skillId, string skillInstanceId) => _resolve().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
    internal void InitializeDualGeneralHealth() => _resolve().InitializeDualGeneralHealth();
    internal bool IsEnabledTemplateSkill(CharacterState player, GeneralDefinition general, string? skillId) => _resolve().IsEnabledTemplateSkill(player, general, skillId);
    internal IEnumerable<GeneralSkillDefinition> OwnedPrintedSkills(CharacterState player, GeneralDefinition general) => _resolve().OwnedPrintedSkills(player, general);
    internal IEnumerable<GeneralSkillDefinition> OwnedRuntimeSkills(CharacterState player, GeneralDefinition general, bool includeAcquired) => _resolve().OwnedRuntimeSkills(player, general, includeAcquired);
    internal IEnumerable<GeneralSkillDefinition> RuntimePassiveSkillDefinitions(CharacterState player, GeneralDefinition general) => _resolve().RuntimePassiveSkillDefinitions(player, general);
    internal GeneralSkillDefinition ToRuntimeSkillDefinition(string skillId) => _resolve().ToRuntimeSkillDefinition(skillId);
}
