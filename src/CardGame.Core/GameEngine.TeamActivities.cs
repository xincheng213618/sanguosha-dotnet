namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool SamePublicTeam(CharacterState a, CharacterState b) => a.TeamId is not null && a.TeamId == b.TeamId;
    private bool HasActivitySkill(CharacterState player, string id) => player.IsAlive && GetSkillBindingShard(player).HasSkill(id);

    private (CardUseEffectSource Source, int Cap)? JiangeDamageCap(IDamageAttempt attack)
    {
        if (_modeDefinition.TeamActivity is null) return null;
        var target = _players[attack.TargetSeat];
        var owner = _players.FirstOrDefault(p => SamePublicTeam(p, target) && HasActivitySkill(p, "jiange:chiying"));
        if (owner is null) return null;
        var grant = GetSkillBindingShard(owner).ActiveGrants.First(g => g.SkillId == "jiange:chiying");
        return (new CardUseEffectSource(grant.SkillId, "allied-damage-cap", owner.Seat, grant.SkillInstanceId), 1);
    }

}
