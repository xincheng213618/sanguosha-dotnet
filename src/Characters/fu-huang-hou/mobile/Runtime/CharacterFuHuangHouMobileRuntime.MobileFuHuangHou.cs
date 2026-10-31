#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFuHuangHouMobileRuntime
{
    [ProgramDistanceRule(ProgramDistanceStage.DistanceProjection, 73910)]
    internal RuleQueryEvaluation MobileFuHuangHouZhuikongDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original)
    {
        if (source.Seat == target.Seat)
            return original;
        var grants = _coreDirectedTurnCardPoliciesDirectedTurnCardPoliciesAccess._directedTurnCardPolicies.Where(p => p.Source.SkillId == "mobile:zhuikong" && p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && p.ActorSeat == source.Seat && p.TargetSeat == target.Seat && p.Effects == DirectedTurnCardPolicyEffect.IgnoreDistance).ToArray();
        if (grants.Length == 0)
            return original;
        return RuleQueryService.Evaluate(SkillRuleQuery.OutgoingDistance, new(1, int.MaxValue), [new RuleQueryBaseTerm("distance:before-mobile-zhuikong", global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(original))], grants.Select(g => (RuleQueryContribution)new FiniteRuleQueryContribution($"mobile-zhuikong:{(g.ParentFrameId)}:{(g.EffectIndex)}", SkillRuleOperation.Set, 1)).ToArray());
    }
}
