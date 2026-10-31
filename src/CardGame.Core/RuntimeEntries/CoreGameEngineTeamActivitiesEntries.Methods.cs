#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTeamActivitiesEntries
{
    internal bool HasActivitySkill(CharacterState player, string id) => _resolve().HasActivitySkill(player, id);
    internal (CardUseEffectSource Source, int Cap)? JiangeDamageCap(GameEngine.IDamageAttempt attack) => _resolve().JiangeDamageCap(attack);
    internal bool SamePublicTeam(CharacterState a, CharacterState b) => _resolve().SamePublicTeam(a, b);
}
