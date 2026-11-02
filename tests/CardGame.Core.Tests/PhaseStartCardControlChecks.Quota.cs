using System.Reflection;
using System.Text.Json.Nodes;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static partial class PhaseStartCardControlChecks
{
    private static void TargetQuotaAllowsSelfAndUsesOriginalIssuanceSlots()
    {
        var (g, registry) = StartDirected("quota-self");
        var wine = V(g, 0).Hand.Where(c => c.Kind == CardKind.Alcohol).Select(c => c.Id).ToArray();
        DirectedPlay(g, g.GetHumanLegalActions().First(a => a.CardId == wine[0] && a.ConversionSource is null)); Play(g);
        Accept(g, new UseProgramSkillCommand(0, DirectedSource, "self-quota", [], [], g.Revision, P(g)!.PromptId)); Play(g);
        Need(TargetQuota(g, 0, 0) && !TargetQuota(g, 0, 2) &&
            !g.GetHumanLegalActions().Any(a => a.CardId == wine[1] && a.ConversionSource is null),
            "Pure self quota bypasses only the count; an existing Alcohol effect still forbids another real Wine.");
        var slash = V(g, 0).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        DirectedPlay(g, g.GetHumanLegalActions().First(a => a.CardId == slash && a.ConversionSource is null && a.TargetSeats.SequenceEqual([1]))); Play(g);
        Need(E<DamageAppliedEvent>(g).Any(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 2), "The real Slash consumes the actual Wine effect.");
        g = Cold(g, registry);
        DirectedPlay(g, g.GetHumanLegalActions().First(a => a.CardId == wine[1] && a.ConversionSource is null)); Play(g);
        Need(wine.All(id => E<CardUseFinishedEvent>(g).Count(e => e.CardId == id && e.CardKind == CardKind.Alcohol) == 1),
            "After the Wine effect is consumed, the same-turn second entity pays and completes once through the common quota query.");
        g = Cold(g, registry);
        OriginalIssuanceSlotIsIdempotent();
        FireQuotaWaitsForRealDrawChildren();
        ChosenAssaultIssuesBothUndamagedTargets();
        foreach (var branch in new[] { 0, 1 }) GuosiUsesPaidBranchAndNativeExpiry(branch);
    }

    private static void OriginalIssuanceSlotIsIdempotent()
    {
        var (g, _) = StartDirected("quota-slots"); Use(g, "quota-hold");
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var frame = g.ResolutionStack.OfType<ProgramSkillFrame>().Last(f => f.SkillId == Driver && f.ActivationId == "quota-hold");
        var runtime = g.GetRuntimeModule<CoreDirectedTurnCardPoliciesRuntime>();
        var options = new DirectedCardPolicyOptions { RequiresEffectiveSource = false, CaptureOnUse = false, Until = [] };
        // This small issuer-contract check uses a real paused frame, not synthetic frames or a command-replay claim.
        runtime.GrantDirectedCardPolicy(frame, 0, 2, [], DirectedTurnCardPolicyEffect.BypassUseLimit, options, issuanceSlot: 0);
        var first = runtime.ActiveDirectedPolicies().Single(p => p.ParentFrameId == frame.Id);
        runtime.GrantDirectedCardPolicy(frame, 0, 3, [], DirectedTurnCardPolicyEffect.BypassUseLimit, options, issuanceSlot: 1);
        runtime.GrantDirectedCardPolicy(frame, 0, 2, [], DirectedTurnCardPolicyEffect.BypassUseLimit, options, issuanceSlot: 0);
        var rows = runtime.ActiveDirectedPolicies().Where(p => p.ParentFrameId == frame.Id).ToArray();
        Need(rows.Length == 2 && rows.All(p => p.EffectIndex == frame.InstructionIndex - 1) &&
            rows.Select(p => p.IssuanceSlot).Distinct().Count() == 2 && ReferenceEquals(first, rows.Single(p => p.IssuanceSlot == 0)),
            "One original instruction may issue to two targets while repeating its first slot is an exact no-op.");
        var rejected = false;
        try { runtime.GrantDirectedCardPolicy(frame, 0, 0, [], DirectedTurnCardPolicyEffect.IgnoreDistance, options, issuanceSlot: 2); }
        catch (InvalidOperationException) { rejected = true; }
        Need(rejected && runtime.ActiveDirectedPolicies().Count(p => p.ParentFrameId == frame.Id) == 2,
            "Self does not permit unrelated distance effects or mutate existing slots.");
        var started = E<TurnStartedEvent>(g).Last();
        var boundaries = g.GetRuntimeModule<CoreGameEngineProgramSkillChangesRuntime>();
        boundaries.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.TurnCleanup, started.ActorSeat,
            SkillStateBoundaryTiming.Before, started.TurnNumber));
        runtime.GrantDirectedCardPolicy(frame, 0, 1, [], DirectedTurnCardPolicyEffect.BypassUseLimit,
            new() { RequiresEffectiveSource = false, CaptureOnUse = false }, issuanceSlot: 2);
        Need(TargetQuota(g, 0, 1), "Default actual-turn rights issued after cleanup remain valid until the next actual start.");
        boundaries.ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.TurnStarted, (started.ActorSeat + 1) % 4,
            SkillStateBoundaryTiming.Before, started.TurnNumber + 1));
        Need(!TargetQuota(g, 0, 1) && TargetQuota(g, 0, 2) && TargetQuota(g, 0, 3) &&
            runtime.ActiveDirectedPolicies().Count(p => p.ParentFrameId == frame.Id) == 2,
            "The next any-seat start removes the late default slot while both explicitly permanent slots remain.");
    }

    private static bool TargetQuota(GameEngine g, int actor, int target) =>
        ((ProgramCapabilityRegistry)typeof(GameEngine).GetProperty("ProgramCapabilities", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(g)!).Get<IProgramTargetQuotaQueries>().HasTargetCardQuotaAllowance(actor, target);

    private static void ConfigureQuotaFixture(JsonNode rules, string scenario)
    {
        ConfigureNativeQuotaFixture(rules, scenario);
        if (scenario == "quota-self")
            rules["skills"]![0]!["activations"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"self-quota","usesPerTurn":null,"effects":[{"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"owner"},"effects":["bypassUseLimit"],"requiresEffectiveSource":false,"captureOnUse":false}]}
            """));
        if (scenario == "quota-slots")
            rules["skills"]![1]!["activations"]!.AsArray().Add(JsonNode.Parse("""
            {"id":"quota-hold","usesPerTurn":null,"effects":[{"op":"chooseOption","target":"owner","resultBind":"slot-host","options":[{"id":"continue"}]}]}
            """));
    }
}
