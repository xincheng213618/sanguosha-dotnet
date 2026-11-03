using CardGame.Core.EngineOwners;

namespace CardGame.Core;

// Being in an attack range is a participant relation, independent of a card's
// distance exemptions, target prohibitions and use quotas.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class AttackRangeRuntime(IMatchParticipantStateAccess participants,
    IKernelSkillBindingIndexAccess bindings,
    EngineStates.Contracts.ICoreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess directed,
    RuntimeEntries.CoreGameEngineActionQueriesEntries arithmetic,
    RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries roles,
    IAttackRangeExpressionWorlds worlds) : IAttackRangeQueries
{
    public bool HasRelation(int actor, int target)
    {
        if (!HasLivingParticipants(actor, target)) return false;
        foreach (var relation in directed._directedTurnCardPolicies)
        {
            if (relation.ActorSeat != actor || relation.TargetSeat != target ||
                !relation.Effects.HasFlag(DirectedTurnCardPolicyEffect.InAttackRange)) continue;
            if (relation.RequiresEffectiveSource &&
                !bindings._skillBindingIndex.GetShard(participants.At(relation.Source.OwnerSeat))
                    .HasInstance(relation.Source.SkillId, relation.Source.SkillInstanceId)) continue;
            if (relation.While is null || ProgramExpressions.EvaluateBool(relation.While,
                    worlds.Create(relation.Source.OwnerSeat, actor, target)) == true)
                return true;
        }

        foreach (var owner in participants.Players.Where(player => player.IsAlive))
        {
            IProgramExpressionWorld? world = null;
            foreach (var source in bindings._skillBindingIndex.GetShard(owner).ProgramInstances)
            foreach (var declaration in source.Program.AttackRangeRelations)
            {
                if (declaration.OwnerRole is { } role &&
                    !roles.HasSkillRoleQualification(owner, source.SkillId, source.SkillInstanceId, role)) continue;
                if (ProgramExpressions.EvaluateBool(declaration.While,
                        world ??= worlds.Create(owner.Seat, actor, target)) == true)
                    return true;
            }
        }
        return false;
    }

    public bool IsWithinAttackRange(int actor, int target, int? excludedEquipmentId = null,
        EquipmentSlot? excludedEquipmentSlot = null) =>
        HasLivingParticipants(actor, target) && (HasRelation(actor, target) ||
            EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(arithmetic.EvaluateDistance(
                participants.At(actor), participants.At(target), excludedEquipmentId, excludedEquipmentSlot)) <=
            EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(arithmetic.EvaluateAttackRange(
                participants.At(actor), excludedEquipmentId, excludedEquipmentSlot)));

    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 900010)]
    internal bool RelationAllowsBorrowedWeaponSlash(ProgramSlashDistanceQuery query) =>
        HasRelation(query.Actor.Seat, query.Target.Seat);

    private bool HasLivingParticipants(int actor, int target) => actor != target &&
        actor >= 0 && actor < participants.Players.Count && target >= 0 && target < participants.Players.Count &&
        participants.At(actor).IsAlive && participants.At(target).IsAlive;
}
