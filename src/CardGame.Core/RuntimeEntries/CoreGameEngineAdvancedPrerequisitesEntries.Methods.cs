#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedPrerequisitesEntries
{
    internal bool AdvancedTriggerPrerequisites(CharacterState owner, string skillId, SkillProgramTrigger trigger) => _resolve().AdvancedTriggerPrerequisites(owner, skillId, trigger);
    internal bool CanReclaimNamedWeapon(CharacterState owner, SkillProgramEffect effect) => _resolve().CanReclaimNamedWeapon(owner, effect);
}
