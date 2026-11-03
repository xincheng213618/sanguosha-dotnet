using System.Text.Json.Nodes;
using CardGame.Core;

internal static partial class PhaseStartCardControlChecks
{
    private const string Lianpian = "ol:lianpian", LianpianDrawReason = "skill-program.ol:lianpian.Draw",
        LianpianGiveReason = "skill-program.ol:lianpian.GiveBoundCard";

    private static void SuFeiGivesOriginalDrawAfterNestedHistoryAndSourceLossCold()
    {
        foreach (var scenario in new[] { "history-sufei-give", "history-sufei-reentry", "history-sufei-gift-death" })
        {
            var (g, registry) = StartDirected(scenario);
            HistoryUse(g, CardKind.IronChain, [1]); HistoryContinue(g);
            var second = HistoryUse(g, CardKind.IronChain, [1]);
            Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(g, p => Optional(p, Lianpian)); Activate(g);
            Reach(g, p => LianpianWatchOption(p, "lianpian-draw"));
            var frame = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Lianpian);
            var frameId = frame.Id;
            var drawn = frame.CardSetBindings.Single(binding => binding.Name == "lianpian-drawn");
            var cardId = drawn.CardIds.Single();
            var entry = g.CardMovements.Single(move => move.CardId == cardId && move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Hand(0) && move.Reason.Value == LianpianDrawReason).Sequence;
            Need(E<ProgramDrawCompletedEvent>(g).Single(e => e.FrameId == frameId) is { RequestedCount: 1, ActualDrawCount: 1, RecipientSeat: 0 } &&
                drawn.SourceLocations.SequenceEqual([CardLocation.Hand(0)]) && frame.InvocationAuthorization is { RetainOwnerDeath: false } &&
                frame.WindowContext?.CardUse?.ParentCardUseFrameId == second.Id,
                "The accepted real card-action window owns its one actual drawn entity and source-loss permission, separately from death permission.");
            g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            if (scenario == "history-sufei-reentry")
            {
                Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards"));
                Answer(g, c => c.Cards.SequenceEqual([cardId]));
            }
            Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
            Answer(g, c => c.Targets.SequenceEqual([2]));
            Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "history-finalized"));
            var nested = g.ResolutionStack.OfType<CardUseFrame>().Last();
            Need(nested.Id != second.Id && History(g).FindFinalizedUse(nested.Id)!.PrimaryTargets!.SequenceEqual([2]),
                "A genuine nested Slash finalizes a different target while the original paid Draw is suspended.");
            Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(g, p => LianpianWatchOption(p, "lianpian-return"));
            Need(!V(g, 0).Skills!.Any(skill => skill.ContentId == Lianpian) && V(g, 0).Hand.Any(card => card.Id == cardId) &&
                History(g).ReadCardUseFacts(new(g.State.TurnNumber, 0, History(g).CardUseHistoryCursor), CardUseHistoryView.Finalizations)
                    .Last().PrimaryTargets!.SequenceEqual([2]),
                "The draw child really loses the source and changes live finalized history before the original gift resumes.");
            if (scenario == "history-sufei-reentry")
                Need(g.CardMovements.Last(move => move.CardId == cardId && move.To == CardLocation.Hand(0)).Sequence > entry,
                    "The exact original drawn entity leaves and reenters Hand; this gift rule retains entity identity rather than requiring its original residency.");
            g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "give-bound-card"));
            Need(P(g)!.Choices.Where(c => c.Parameters.GetValueOrDefault("program-action") == "give-bound-card")
                    .All(c => c.Cards.SequenceEqual([cardId]) && c.Targets.SequenceEqual([1])) &&
                P(g)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "keep-bound-cards"),
                "After loss and nested use, only the original drawn entity and the original window's common target remain gift choices.");
            g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "give-bound-card");
            Reach(g, p => LianpianWatchOption(p, "lianpian-gift"));
            Need(E<ProgramBoundCardGivenEvent>(g).Single(e => e.FrameId == frameId) is { OwnerSeat: 0, TargetSeat: 1 } given && given.CardId == cardId &&
                V(g, 1).Hand.Any(card => card.Id == cardId) && !E<ProgramBindingResolvedEvent>(g).Any(e => e.FrameId == frameId),
                "The actual Give moves the one original entity before awaiting its real movement child on the same unfinished program.");
            g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
            if (scenario == "history-sufei-gift-death")
            {
                for (var step = 0; step < 240 && !E<ProgramBindingResolvedEvent>(g).Any(e => e.FrameId == frameId); step++)
                {
                    if (P(g) is { PlayerSeat: 0, Kind: DecisionKind.RescueDying })
                        Answer(g, c => c.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
                    else Accept(g, new AdvanceOneStepCommand(g.Revision));
                }
                Need(!V(g, 0).IsAlive && E<PlayerDiedEvent>(g).Any(e => e.VictimSeat == 0) &&
                    E<ProgramBindingResolvedEvent>(g).Single(e => e.FrameId == frameId).Completed,
                    "Death in the actual gift child drains the already delivered terminal Give without requiring ongoing source or owner eligibility.");
                g = Cold(g, registry);
            }
            else
            {
                Play(g);
                Need(History(g).FindFinalizedUse(second.Id) is { Completed: true },
                    "The original native use returns once after its paid gift child and source loss.");
            }
            Need(E<ProgramDrawCompletedEvent>(g).Count(e => e.FrameId == frameId) == 1 &&
                E<NativeDrawAttemptStartedEvent>(g).Count(e => e.Context.Reason.Value == LianpianDrawReason) == 1 &&
                E<ProgramBoundCardGivenEvent>(g).Count(e => e.FrameId == frameId) == 1 &&
                g.CardMovements.Count(move => move.CardId == cardId && move.From == CardLocation.Hand(0) && move.To == CardLocation.Hand(1) &&
                    move.Reason.Value == LianpianGiveReason) == 1,
                "Every cold boundary preserves exactly one original Draw, one actual gift and no new reward request after source loss or death.");
        }
    }

    private static bool LianpianWatchOption(PendingDecision prompt, string binding) => prompt.SkillPrompt?.SkillId == Watch &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("result-bind") == binding);

    private static void ConfigureLianpianGiftFixture(JsonNode rules, string scenario)
    {
        if (scenario is not ("history-sufei-give" or "history-sufei-reentry" or "history-sufei-gift-death")) return;
        var triggers = rules["skills"]!.AsArray().Single(skill => skill!["id"]!.GetValue<string>() == Watch)!["triggers"]!.AsArray();
        var reentry = scenario == "history-sufei-reentry" ? """
            {"op":"selectOwnedCards","target":"owner","zones":["hand"],"minimumCards":1,"maximumCards":1,"resultBind":"away"},
            {"op":"moveBoundCards","target":"owner","sourceBind":"away","destination":"drawPileTop","awaitMovementTriggers":true},
            {"op":"draw","target":"owner","amount":1,"awaitMovementTriggers":true},
            """ : "";
        triggers.Add(JsonNode.Parse($$$"""
            {"id":"lianpian-paid-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["{{{LianpianDrawReason}}}"],"movementOccurrence":"perBatch","optional":false,"usageScope":"game","usageLimit":1,
              "effects":[{"op":"chooseOption","target":"owner","resultBind":"lianpian-draw","options":[{"id":"continue"}]},
                {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{Lianpian}}}"]}},
                {{{reentry}}}
                {"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"useVirtualSlash","target":"selectedTarget"},
                {"op":"chooseOption","target":"owner","resultBind":"lianpian-return","options":[{"id":"continue"}]}]}
            """));
        var death = scenario == "history-sufei-gift-death" ? ",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":20}" : "";
        triggers.Add(JsonNode.Parse($$$"""
            {"id":"lianpian-paid-gift","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementReasons":["{{{LianpianGiveReason}}}"],"movementOccurrence":"perBatch","optional":false,"usageScope":"game","usageLimit":1,
              "effects":[{"op":"chooseOption","target":"owner","resultBind":"lianpian-gift","options":[{"id":"continue"}]}{{{death}}}]}
            """));
        triggers.Add(JsonNode.Parse("""
            {"id":"lianpian-native-request","window":"nativeDrawStarting","subject":"owner","optional":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}
            """));
    }
}
