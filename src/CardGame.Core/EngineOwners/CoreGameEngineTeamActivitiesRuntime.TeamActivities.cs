#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTeamActivitiesRuntime
{
    internal bool SamePublicTeam(CharacterState a, CharacterState b) => _coreModeAllegianceOperations.ModeAllegiancePolicy.SamePublicTeam(CoreGameEngineModeAllegianceRuntime.CaptureAllegianceSeat(a), CoreGameEngineModeAllegianceRuntime.CaptureAllegianceSeat(b));
    internal bool HasActivitySkill(CharacterState player, string id) => player.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(player).HasSkill(id);
    internal (CardUseEffectSource Source, int Cap)? JiangeDamageCap(GameEngine.IDamageAttempt attack)
    {
        if (_modeRuntimeOperations._modeRuntime.Find<IJiangeBattleFeature>()?.AlliedDamageCap(attack.TargetSeat)is not { } plan)
            return null;
        var owner = _coreMatchParticipantsPlayersAccess._players[plan.OwnerSeat];
        var grant = _coreNationalSkillsOperations.GetSkillBindingShard(owner).ActiveGrants.First(g => g.SkillId == plan.SkillId);
        return (new CardUseEffectSource(grant.SkillId, plan.BindingId, owner.Seat, grant.SkillInstanceId), plan.Maximum);
    }
}
