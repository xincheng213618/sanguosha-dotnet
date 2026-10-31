#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramStateProjectionEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnCardActionProhibition> CreateNonemptyActionProhibitionSnapshots(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().CreateNonemptyActionProhibitionSnapshots(owner, skillId);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DirectedTurnCardPolicy> CreateNonemptyDirectedPolicySnapshots(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().CreateNonemptyDirectedPolicySnapshots(owner, skillId);
    internal global::CardGame.Core.SkillRuntimeStateSnapshot CreateProgramAwareSkillStateSnapshot(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().CreateProgramAwareSkillStateSnapshot(owner, skillId);
    internal bool HasProgramPersistentZone(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardZoneKind zone) => _resolve().HasProgramPersistentZone(owner, zone);
    internal bool IsRuntimeAcquiredSkill(global::CardGame.Core.CharacterState player, string skillId) => _resolve().IsRuntimeAcquiredSkill(player, skillId);
}
