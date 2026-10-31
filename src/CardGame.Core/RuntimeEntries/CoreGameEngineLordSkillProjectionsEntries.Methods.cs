#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLordSkillProjectionsEntries
{
    internal IReadOnlyList<string> AdvancedOwnedSkillIds(CharacterState owner) => _resolve().AdvancedOwnedSkillIds(owner);
    internal long CaptureLordProjectionDependencyStamp() => _resolve().CaptureLordProjectionDependencyStamp();
    internal IEnumerable<SkillGrant> EffectiveUnprojectedGrants(CharacterState owner) => _resolve().EffectiveUnprojectedGrants(owner);
    internal bool HasSkillRoleQualification(CharacterState owner, string skillId, string? instance, Role role) => _resolve().HasSkillRoleQualification(owner, skillId, instance, role);
    internal bool IsProjectedGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsProjectedGrantQualified(owner, grant);
    internal void SynchronizeLordSkillProjections() => _resolve().SynchronizeLordSkillProjections();
    internal void SynchronizeOwnerLordSkillProjections(CharacterState owner, CharacterState? lord) => _resolve().SynchronizeOwnerLordSkillProjections(owner, lord);
    internal bool HasLordProjectionCapability { get => _resolve().HasLordProjectionCapability; }
}
