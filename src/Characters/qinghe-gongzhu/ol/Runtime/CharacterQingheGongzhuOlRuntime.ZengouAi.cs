#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQingheGongzhuOlRuntime
{
    internal (ProgramAiEstimate Estimate, int? TargetSeat) EstimateActualDodgeZengouForAi(CharacterState owner, ProgramSkillWindowContext? context)
    {
        var window = _coreWushuangZengouOperations.WushuangZengouWindow(context);
        if (window is null)
            return (new(new(0, 0, 0, 0, 0, 0, false, false), 0d, false, "没有可令其无效的真实闪。"), null);
        var candidate = _coreProgramLifecycleOperations.GetPendingProgramTriggerCandidate().Candidate;
        var hpCost = ActualDodgeZengouHpCostForAi(owner);
        var cost = _coreWushuangZengouOperations.WushuangZengouCosts2(owner.Seat, candidate.SkillId, candidate.SkillInstanceId).Length > 0 ? Math.Min(8d, hpCost) : hpCost;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat);
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains.GetValueOrDefault(owner.Seat) ?? new SimpleAiBrain(owner.Seat, unchecked(_coreMatchDefinitionOptionsAccess._options.Seed * 397) ^ (owner.Seat + 1), _coreMatchDefinitionOptionsAccess._options.AiPolicyVersion);
        // Canceling this Dodge lets precisely its current attack target take the
        // hit. Reuse the existing public harmful-card relationship estimate.
        var hit = -brain.ScoreCardEffectIntervention(view, CardKind.Duel, window.Action.ActorSeat);
        var gain = window.Action.PhysicalCards.Count(card => window.ResponseCompletion!.NativeCosts.Any(move => move.CardId == card.CardId && move.To == CardLocation.DiscardPile)) * 8d;
        var lethal = double.IsPositiveInfinity(cost);
        var score = lethal ? -1000d : hit + gain - cost;
        return (new(new(0, 0, 0, 0, 0, 0, false, false) { ValueAdjustment = score }, score, lethal, $"真实闪无效的公开收益 {(hit):0.#}，获得实体牌 {(gain):0.#}，最小支付代价 {(cost):0.#}；失血计入强制摸牌及低体力风险。"), null);
    }

    internal double ActualDodgeZengouHpCostForAi(CharacterState owner)
    {
        // Do not assume an unknown rescue. Mandatory, unconditional owner draws
        // after HP loss are real benefits (for example Zhaxiang), not a name-based
        // companion bonus. Other effects retain the ordinary conservative cost.
        if (owner.Hp <= 1)
            return double.PositiveInfinity;
        var draws = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.AfterHpLost).Select(_coreProgramLifecycleOperations.GetProgramTrigger2).Where(trigger => !trigger.Optional && trigger.Subject == SkillProgramTriggerSubject.Owner && trigger.Condition.Kind == SkillProgramTriggerConditionKind.Always && trigger.UsageLimit is null).SelectMany(trigger => trigger.Effects).Where(effect => effect is { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }).ToArray();
        var benefit = ProgramCompositionAi.Estimate(draws, _coreNativeSkillContextQueriesOperations.CreateSkillContext(owner), owner.IsFaceDown, _coreProgramLifecycleOperations.CreateProgramAiPublicContext(owner)).Score;
        return 22d + (owner.Hp == 2 ? 8d : 0d) - benefit;
    }
}
