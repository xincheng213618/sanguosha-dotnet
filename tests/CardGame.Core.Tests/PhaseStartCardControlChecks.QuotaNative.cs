using System.Text.Json.Nodes;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static partial class PhaseStartCardControlChecks
{
    private static void FireQuotaWaitsForRealDrawChildren()
    {
        var (g, registry) = StartDirected("quota-fire"); Use(g, "fire-hit", [2]);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
        Need(P(g)!.PlayerSeat == 2, "The actual fire recipient owns the real gained-card child.");
        Answer(g, c => c.Targets.SequenceEqual([0]));
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var fire = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.FireTargetBenefit is not null);
        var receipt = fire.FireTargetBenefit!;
        Need(!TargetQuota(g, 0, 2) && fire.PendingMovementContinuation is not null &&
            g.ResolutionStack.OfType<DamageTriggerWindowFrame>().Any(f => f.Id == receipt.DamageWindowId && f.ParentFrameId == receipt.DamageFrameId) &&
            E<ProgramFireTargetDrawIssuedEvent>(g).Single() is { ActualCount: 1 } drawn && drawn.ProgramFrameId == fire.Id && drawn.DamageWindowId == receipt.DamageWindowId &&
            g.CardMovements.Count(m => m.Sequence > receipt.SequenceBefore && m.Sequence <= receipt.SequenceAfter && m.To == CardLocation.Hand(2) && m.Reason.Value == "program.fire-target-benefit.draw") == 1,
            "The real fire-damage frame pays one target draw, but no quota exists while its child is suspended.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        Need(!V(g, 0).Skills!.Any(s => s.ContentId == "ol:huxiao") && !TargetQuota(g, 0, 2),
            "Source loss inside the actual draw child neither cancels the paid promise nor grants it early.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue"); Play(g);
        var policy = E<DirectedTurnCardPolicyGrantedEvent>(g).Single(e => e.Policy.Source.SkillId == "ol:huxiao").Policy;
        Need(TargetQuota(g, 0, 2) && !TargetQuota(g, 2, 0) && policy.ParentFrameId == fire.Id &&
            policy.EffectIndex == receipt.InstructionIndex - 1 && policy.Source == receipt.Source &&
            !policy.RequiresEffectiveSource && !policy.CaptureOnUse,
            "Only the true parent return issues one independent directed quota with its original instruction and source.");
        g = Cold(g, registry);
        Need(E<ProgramFireTargetDrawIssuedEvent>(g).Count() == 1 && E<DirectedTurnCardPolicyGrantedEvent>(g).Count(e => e.Policy.Source.SkillId == "ol:huxiao") == 1,
            "Cold recovery does not pay the target draw or fulfil the original promise twice.");
        ExpireQuotaAtNativeEnd(g, registry, policy);
    }

    private static void ChosenAssaultIssuesBothUndamagedTargets()
    {
        var (g, registry) = StartDirected("quota-bao"); Activate(g);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "bao2025" && c.Cards.Count == 2));
        Answer(g, c => c.Cards.Count == 2);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "add-2")); Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "add-2");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "add-3");
        Answer(g, c => c.Parameters.GetValueOrDefault("branch") == "confirm"); Play(g);
        var issued = E<ChosenAssaultUseIssuedEvent>(g).Single();
        var outcomes = E<ChosenAssaultTargetOutcomeEvent>(g).Where(e => e.FrameId == issued.FrameId).ToArray();
        var rows = E<DirectedTurnCardPolicyGrantedEvent>(g).Select(e => e.Policy).Where(p => p.ParentFrameId == issued.FrameId).ToArray();
        Need(issued.SelectedTargets.SequenceEqual([2, 3]) && outcomes.Length == 2 && outcomes.All(e => e.UseId == issued.UseId && !e.OwnerDealtDamage) &&
            rows.Length == 2 && rows.Select(p => p.TargetSeat).Order().SequenceEqual([2, 3]) && rows.Select(p => p.IssuanceSlot).Distinct().Count() == 2 &&
            rows.Select(p => p.EffectIndex).Distinct().Count() == 1 && TargetQuota(g, 0, 2) && TargetQuota(g, 0, 3),
            "Both genuinely undamaged chosen-assault targets receive distinct slots from one original instruction after the actual Use returns.");
        g = Cold(g, registry);
        Need(E<DirectedTurnCardPolicyGrantedEvent>(g).Count(e => e.Policy.ParentFrameId == issued.FrameId) == 2,
            "Cold recovery preserves both targets without replaying either outcome.");
        ExpireQuotaAtNativeEnd(g, registry, rows[0]);
        Need(!TargetQuota(g, 0, 3), "The second outcome expires with the same real turn as the first.");
    }

    private static void GuosiUsesPaidBranchAndNativeExpiry(int branch)
    {
        var (g, registry) = StartDirected("quota-guosi", obtained: 1);
        Accept(g, new UseProgramSkillCommand(0, "ol:tanbei", "choose-target-pact", [], [2], g.Revision, P(g)!.PromptId));
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "guo-si"));
        Need(P(g)!.PlayerSeat == 2, "The real pact target chooses its branch.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option") == branch.ToString()); Play(g);
        var paid = E<GuoSiPactPaidEvent>(g).Single(); var policy = E<DirectedTurnCardPolicyGrantedEvent>(g).Single().Policy;
        var expected = branch == 0 ? DirectedTurnCardPolicyEffect.ForbidTarget : DirectedTurnCardPolicyEffect.IgnoreDistance | DirectedTurnCardPolicyEffect.BypassUseLimit;
        Need(paid.Pact.Branch == branch && paid.Pact.Actor == 0 && paid.Pact.Target == 2 && paid.ActualCount == (branch == 0 ? 1 : 0) &&
            g.CardMovements.Count(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.From == CardLocation.Hand(2) && m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program.tanbei.gain") == paid.ActualCount &&
            policy.ParentFrameId == paid.Pact.FrameId && policy.Effects == expected && !policy.RequiresEffectiveSource && !policy.CaptureOnUse &&
            policy.Until.Any(b => b.Event == SkillStateBoundaryEvent.TurnEnded && b.Timing == SkillStateBoundaryTiming.Before) &&
            (branch == 0 ? DirectedHas(g, 2, DirectedTurnCardPolicyEffect.ForbidTarget) && !TargetQuota(g, 0, 2)
                : DirectedHas(g, 2, DirectedTurnCardPolicyEffect.IgnoreDistance) && TargetQuota(g, 0, 2) && !TargetQuota(g, 2, 0)),
            "The true paid branch and movement invoice survive; only its directed effect is issued with the native Before-End boundary.");
        g = Cold(g, registry); ExpireQuotaAtNativeEnd(g, registry, policy);
    }

    private static void ExpireQuotaAtNativeEnd(GameEngine g, ContentRegistry registry, DirectedTurnCardPolicy policy)
    {
        End(g); Reach(g, _ => E<TurnEndedEvent>(g).Any(e => e.TurnNumber == policy.TurnNumber && e.ActorSeat == policy.TurnSeat));
        Need(!g.GetRuntimeModule<CoreDirectedTurnCardPoliciesRuntime>().ActiveDirectedPolicies().Any(p => p.ParentFrameId == policy.ParentFrameId),
            "The native actual turn endpoint retires every slot of the original applied policy.");
        _ = Cold(g, registry);
    }

    private static void ConfigureNativeQuotaFixture(JsonNode rules, string scenario)
    {
        if (scenario == "quota-fire")
        {
            rules["skills"]![1]!["activations"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"fire-hit","minTargets":1,"maxTargets":1,"usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1,"nature":"fire"}]}
            """));
            rules["skills"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"fixture:phase-control-watch","revision":1,"triggers":[{"id":"fire-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"effects":[
              {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},
              {"op":"chooseOption","target":"owner","resultBind":"before-loss","options":[{"id":"continue"}]},
              {"op":"skillState","target":"selectedTarget","action":"lose","match":{"skillIds":["ol:huxiao"]},"until":[]},
              {"op":"chooseOption","target":"owner","resultBind":"after-loss","options":[{"id":"continue"}]}]}]}
            """));
        }
        if (scenario == "quota-bao")
            rules["skills"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"fixture:quota-prevent","revision":1,"triggers":[{"id":"prevent","window":"beforeDamageApplied","subject":"damageTarget","optional":false,"effects":[{"op":"preventCurrentDamage","target":"owner"}]}]}
            """));
    }
}
