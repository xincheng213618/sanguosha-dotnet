#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShenwuExpandedPoliciesEntries
{
    internal long CaptureShenwuFengdongStamp() => _resolve().CaptureShenwuFengdongStamp();
    internal bool IsShenwuHonglianHandExempt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsShenwuHonglianHandExempt(owner, card);
    internal bool ShenwuFengdongQualifiesGrant(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().ShenwuFengdongQualifiesGrant(owner, grant);
    internal bool ShenwuJuyuanExtraTarget(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind) => _resolve().ShenwuJuyuanExtraTarget(player, kind);
    internal global::CardGame.Core.RuleQueryContribution? ShenwuJuyuanTargetCountContribution(global::CardGame.Core.ProgramTargetCountQuery query) => _resolve().ShenwuJuyuanTargetCountContribution(query);
    internal int ShenwuReplacementDrawBonus(int seat) => _resolve().ShenwuReplacementDrawBonus(seat);
    internal bool ShenwuSipuCardUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().ShenwuSipuCardUseForbidden(query);
    internal bool ShenwuSipuForbidsAction(int actorSeat) => _resolve().ShenwuSipuForbidsAction(actorSeat);
    internal bool ShenwuZhaohuoDisablesArmor(int targetSeat) => _resolve().ShenwuZhaohuoDisablesArmor(targetSeat);
}
