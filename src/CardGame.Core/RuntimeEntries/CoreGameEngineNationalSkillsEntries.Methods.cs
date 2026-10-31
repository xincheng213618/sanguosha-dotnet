#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNationalSkillsEntries
{
    internal bool CanOwnPrintedSkill(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition general, global::CardGame.Core.GeneralSkillDefinition skill) => _resolve().CanOwnPrintedSkill(player, general, skill);
    internal global::System.Collections.Generic.IReadOnlyList<string> EnabledContentSkillIds(global::CardGame.Core.CharacterState player) => _resolve().EnabledContentSkillIds(player);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.SkillGrant> EnabledNonTemplateSkillGrants(global::CardGame.Core.CharacterState player) => _resolve().EnabledNonTemplateSkillGrants(player);
    internal global::System.Collections.Generic.IReadOnlyList<string> EnabledPrintedContentSkillIds(global::CardGame.Core.CharacterState player) => _resolve().EnabledPrintedContentSkillIds(player);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.SkillGrant> EnabledRuntimeSkillGrants(global::CardGame.Core.CharacterState player) => _resolve().EnabledRuntimeSkillGrants(player);
    internal string EnabledSkillNames(global::CardGame.Core.CharacterState player) => _resolve().EnabledSkillNames(player);
    internal string GetRuntimeSkillInstanceId(global::CardGame.Core.CharacterState player, string skillId) => _resolve().GetRuntimeSkillInstanceId(player, skillId);
    internal global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player) => _resolve().GetSkillBindingShard(player);
    internal bool HasRuntimeSkill(global::CardGame.Core.CharacterState player, string skillId) => _resolve().HasRuntimeSkill(player, skillId);
    internal bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId) => _resolve().HasRuntimeSkillInstance(player, skillId, skillInstanceId);
    internal void InitializeDualGeneralHealth() => _resolve().InitializeDualGeneralHealth();
    internal bool IsEnabledTemplateSkill(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition general, string? skillId) => _resolve().IsEnabledTemplateSkill(player, general, skillId);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.GeneralSkillDefinition> OwnedPrintedSkills(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition general) => _resolve().OwnedPrintedSkills(player, general);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.GeneralSkillDefinition> OwnedRuntimeSkills(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition general, bool includeAcquired) => _resolve().OwnedRuntimeSkills(player, general, includeAcquired);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.GeneralSkillDefinition> RuntimePassiveSkillDefinitions(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralDefinition general) => _resolve().RuntimePassiveSkillDefinitions(player, general);
    internal global::CardGame.Core.GeneralSkillDefinition ToRuntimeSkillDefinition(string skillId) => _resolve().ToRuntimeSkillDefinition(skillId);
}
