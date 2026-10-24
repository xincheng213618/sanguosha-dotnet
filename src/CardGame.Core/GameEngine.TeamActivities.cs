namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool SamePublicTeam(CharacterState a, CharacterState b) =>
        ModeAllegiancePolicy.SamePublicTeam(CaptureAllegianceSeat(a), CaptureAllegianceSeat(b));
    private bool HasActivitySkill(CharacterState player, string id) => player.IsAlive && GetSkillBindingShard(player).HasSkill(id);

    private (CardUseEffectSource Source, int Cap)? JiangeDamageCap(IDamageAttempt attack)
    {
        if (_modeRuntime.Find<IJiangeBattleFeature>()?.AlliedDamageCap(attack.TargetSeat) is not { } plan) return null;
        var owner = _players[plan.OwnerSeat];
        var grant = GetSkillBindingShard(owner).ActiveGrants.First(g => g.SkillId == plan.SkillId);
        return (new CardUseEffectSource(grant.SkillId, plan.BindingId, owner.Seat, grant.SkillInstanceId), plan.Maximum);
    }

}
