#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLordSkillProjectionsEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<string> AdvancedOwnedSkillIds(global::CardGame.Core.CharacterState owner) => _resolve().AdvancedOwnedSkillIds(owner);
    internal long CaptureLordProjectionDependencyStamp() => _resolve().CaptureLordProjectionDependencyStamp();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.SkillGrant> EffectiveUnprojectedGrants(global::CardGame.Core.CharacterState owner) => _resolve().EffectiveUnprojectedGrants(owner);
    internal bool HasSkillRoleQualification(global::CardGame.Core.CharacterState owner, string skillId, string? instance, global::CardGame.Core.Role role) => _resolve().HasSkillRoleQualification(owner, skillId, instance, role);
    internal bool IsProjectedGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsProjectedGrantQualified(owner, grant);
    internal void SynchronizeLordSkillProjections() => _resolve().SynchronizeLordSkillProjections();
    internal void SynchronizeOwnerLordSkillProjections(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CharacterState? lord) => _resolve().SynchronizeOwnerLordSkillProjections(owner, lord);
    internal bool HasLordProjectionCapability { get => _resolve().HasLordProjectionCapability; }
}
