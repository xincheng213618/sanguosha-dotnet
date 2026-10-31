#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedPrerequisitesEntries
{
    internal bool AdvancedTriggerPrerequisites(global::CardGame.Core.CharacterState owner, string skillId, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().AdvancedTriggerPrerequisites(owner, skillId, trigger);
    internal bool CanReclaimNamedWeapon(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramEffect effect) => _resolve().CanReclaimNamedWeapon(owner, effect);
}
