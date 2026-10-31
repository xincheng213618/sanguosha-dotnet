#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTeamActivitiesEntries
{
    internal bool HasActivitySkill(global::CardGame.Core.CharacterState player, string id) => _resolve().HasActivitySkill(player, id);
    internal (global::CardGame.Core.CardUseEffectSource Source, int Cap)? JiangeDamageCap(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().JiangeDamageCap(attack);
    internal bool SamePublicTeam(global::CardGame.Core.CharacterState a, global::CardGame.Core.CharacterState b) => _resolve().SamePublicTeam(a, b);
}
