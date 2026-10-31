#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShenwuExpandedPoliciesEntries
{
    internal bool IsShenwuHonglianHandExempt(CharacterState owner, Card card) => _resolve().IsShenwuHonglianHandExempt(owner, card);
    internal bool ShenwuJuyuanExtraTarget(CharacterState player, CardKind kind) => _resolve().ShenwuJuyuanExtraTarget(player, kind);
    internal RuleQueryContribution? ShenwuJuyuanTargetCountContribution(ProgramTargetCountQuery query) => _resolve().ShenwuJuyuanTargetCountContribution(query);
    internal int ShenwuReplacementDrawBonus(int seat) => _resolve().ShenwuReplacementDrawBonus(seat);
    internal bool ShenwuSipuCardUseForbidden(ProgramCardUseQuery query) => _resolve().ShenwuSipuCardUseForbidden(query);
    internal bool ShenwuSipuForbidsAction(int actorSeat) => _resolve().ShenwuSipuForbidsAction(actorSeat);
    internal bool ShenwuZhaohuoDisablesArmor(int targetSeat) => _resolve().ShenwuZhaohuoDisablesArmor(targetSeat);
}
