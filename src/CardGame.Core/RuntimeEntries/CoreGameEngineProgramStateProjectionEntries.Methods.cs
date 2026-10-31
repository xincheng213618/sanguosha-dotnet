#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramStateProjectionEntries
{
    internal IReadOnlyList<TurnCardActionProhibition> CreateNonemptyActionProhibitionSnapshots(CharacterState owner, string skillId) => _resolve().CreateNonemptyActionProhibitionSnapshots(owner, skillId);
    internal IReadOnlyList<DirectedTurnCardPolicy> CreateNonemptyDirectedPolicySnapshots(CharacterState owner, string skillId) => _resolve().CreateNonemptyDirectedPolicySnapshots(owner, skillId);
    internal SkillRuntimeStateSnapshot CreateProgramAwareSkillStateSnapshot(CharacterState owner, string skillId) => _resolve().CreateProgramAwareSkillStateSnapshot(owner, skillId);
    internal bool HasProgramPersistentZone(CharacterState owner, CardZoneKind zone) => _resolve().HasProgramPersistentZone(owner, zone);
    internal bool IsRuntimeAcquiredSkill(CharacterState player, string skillId) => _resolve().IsRuntimeAcquiredSkill(player, skillId);
}
