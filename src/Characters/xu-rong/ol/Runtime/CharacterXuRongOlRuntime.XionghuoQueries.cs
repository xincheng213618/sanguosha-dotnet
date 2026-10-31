#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuRongOlRuntime
{
    internal bool LiveXionghuoSource(XionghuoGrant grant) => _coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[grant.Source.OwnerSeat], grant.Source.SkillId, grant.Source.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(grant.Source.SkillId).Program?.GameplayHash == grant.GameplayHash;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 10)]
    internal int XionghuoActualDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (attack.IsSourceLess || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) || _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].XionghuoGrants.Count == 0)
            return 0;
        return XuRongSubjectGrants(attack.SourceSeat, attack.TargetSeat).Any() ? 1 : 0;
    }
}
