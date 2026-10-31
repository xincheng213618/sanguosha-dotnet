#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5YuanshuRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQualification, 5744)]
    internal bool ShanheS5TongjiAllowsSlash(ProgramSlashTargetQuery query) => !ShanheS5TongjiProhibitsSlashTarget(new(query.Actor.Seat, query.Target.Seat, query.Kind));
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 5744)]
    internal bool ShanheS5TongjiProhibitsSlashTarget(ProgramDirectedTargetQuery query)
    {
        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(query.Kind) || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.SlashMustTargetOverHpHandOwner) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(query.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(query.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[query.ActorSeat].IsAlive)
            return false;
        return _coreMatchParticipantsPlayersAccess._players.Any(owner => owner.IsAlive && owner.HasHpValue && owner.Seat != query.ActorSeat && owner.Seat != query.TargetSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > owner.Hp && _coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.SlashMustTargetOverHpHandOwner) && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(query.ActorSeat, owner.Seat));
    }
}
