using System.Text.Json.Nodes;
using CardGame.Core;

internal static partial class PhaseStartCardControlChecks
{
    private static void ConfirmedNativeTargetsStayFrozenWhileLiveTargetsChange()
    {
        var (g, registry) = StartDirected("history-confirmed-add");
        var card = SouyingPlay(g, CardKind.IronChain, [1]);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"));
        Answer(g, c => c.Targets.SequenceEqual([2]));
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "confirmed-added"));
        var use = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == card);
        var point = new CardUseHistoryPoint(g.State.TurnNumber, 0, History(g).CardUseHistoryCursor, use.Id, use.Action!.ActionId);
        var confirmed = History(g).ReadCardUseFacts(point, CardUseHistoryView.Confirmations).Single(f => f.NativeUseId == use.Id);
        Need(use.Action!.EffectiveDesignatedTargetSeats.SequenceEqual([1, 2]) && confirmed.ConfirmedTargets!.SequenceEqual([1]) &&
            confirmed.PrimaryTargets!.SequenceEqual([1]) && confirmed.Phase == TurnPhase.Play &&
            History(g).ReadCardUseFacts(point, CardUseHistoryView.Latest).Single(f => f.NativeUseId == use.Id).Targets.SequenceEqual([1, 2]),
            "A real post-confirmation added target updates the live use without rewriting its original confirmation or principal targets.");
        g = Cold(g, registry);
        Need(History(g).ReadCardUseFacts(point, CardUseHistoryView.Confirmations).Single(f => f.NativeUseId == use.Id).ConfirmedTargets!.SequenceEqual([1]),
            "The same native identity and confirmation cutoff survive command replay while the target-adding instruction waits.");
        Answer(g, c => c.Parameters.GetValueOrDefault("option-id") == "continue"); Play(g);
        Need(E<CardUseFinishedEvent>(g).Count(e => e.ResolutionId == use.Id) == 1 &&
            History(g).FindCompletedUse(use.Id)!.Action.EffectiveDesignatedTargetSeats.SequenceEqual([1, 2]) &&
            confirmed.ConfirmedTargets!.SequenceEqual([1]),
            "The actually expanded card completes once and leaves the previously exposed confirmation unchanged.");
        _ = Cold(g, registry);
        ConfirmedHistoryFeedsRealTunjiang();
        ConfirmedHistoryFeedsRealMeihunAcrossActors();
        foreach (var used in new[] { false, true }) ConfirmedHistoryFeedsCurrentBoundaryQieting(used);
    }

    private static void ConfirmedHistoryFeedsRealTunjiang()
    {
        foreach (var targeted in new[] { false, true })
        {
            var (g, registry) = StartDirected("history-tunjiang");
            var turn = g.State.TurnNumber;
            if (targeted) { HistoryUse(g, CardKind.IronChain, [1]); HistoryContinue(g); }
            End(g);
            if (!targeted)
            {
                Reach(g, p => Optional(p, "ol:tunjiang"));
                g = Cold(g, registry); Activate(g);
                var root = E<ProgramBindingStartedEvent>(g).Single(e => e.SkillId == "ol:tunjiang");
                Need(E<ProgramDrawCompletedEvent>(g).Where(e => e.FrameId == root.FrameId).ToArray() is [{ RequestedCount: > 0, ActualDrawCount: > 0 }],
                    "The real untouched Play phase qualifies the actual Tunjiang definition and pays its faction-count draw.");
                g = Cold(g, registry);
            }
            Reach(g, p =>
            {
                if (g.State.TurnNumber > turn) return true;
                Need(!Optional(p, "ol:tunjiang"), "A real confirmed other-player target forbids Tunjiang, and an accepted reward cannot be offered again.");
                return false;
            });
            var roots = E<ProgramBindingStartedEvent>(g).Where(e => e.SkillId == "ol:tunjiang").Select(e => e.FrameId).ToHashSet();
            var draws = E<ProgramDrawCompletedEvent>(g).Where(e => roots.Contains(e.FrameId)).ToArray();
            Need(draws.Length == (targeted ? 0 : 1) &&
                g.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program.ol:tunjiang.Draw") == draws.Sum(e => e.ActualDrawCount),
                "Tunjiang's real draw occurs once only for the eligible unskipped Play.");
            _ = Cold(g, registry);
        }
    }

    private static void ConfirmedHistoryFeedsRealMeihunAcrossActors()
    {
        var (g, registry) = StartDirected("history-meihun");
        Use(g, "history-poke", [1]); Reach(g, p => Optional(p, "happy:meihun"));
        var first = g.ResolutionStack.OfType<CardUseFrame>().Single(f => f.SourceSeat == 1 && f.CardKind == CardKind.Slash);
        Need(first.TargetSeats.SequenceEqual([0]) && P(g)!.SkillPrompt!.SkillId == "happy:meihun",
            "The actual Meihun definition offers for the first real other actor's Slash confirmation targeting its owner.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        Reach(g, p =>
        {
            Need(!Optional(p, "happy:meihun"), "Declining the original first-use opportunity must not produce a duplicate Meihun prompt.");
            return p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 };
        });
        Use(g, "history-poke", [2]);
        Reach(g, p =>
        {
            Need(!Optional(p, "happy:meihun"), "A second actor's later Slash cannot borrow another first-target Meihun opportunity.");
            return p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 };
        });
        var slashes = E<CardUseDeclaredEvent>(g).Where(e => e.CardKind == CardKind.Slash).ToArray();
        Need(slashes.Select(e => e.SourceSeat).SequenceEqual([1, 2]) &&
            slashes.All(e => History(g).FindNativeUse(e.ResolutionId)!.DesignatedTargets.SequenceEqual([0])) &&
            slashes.All(e => E<CardUseFinishedEvent>(g).Count(done => done.ResolutionId == e.ResolutionId) == 1),
            "Both genuine foreign actors finish their native Slashes, while the declined first confirmation stays consumed across cold recovery.");
        _ = Cold(g, registry);
    }

    private static void ConfirmedHistoryFeedsCurrentBoundaryQieting(bool used)
    {
        const string skill = "boundary:qieting-current";
        var (g, registry) = StartDirected("qieting-confirmed-current");
        var turn = g.State.TurnNumber;
        if (used) { SouyingPlay(g, CardKind.IronChain, [1]); Play(g); }
        End(g); Reach(g, p => Optional(p, skill)); Activate(g);
        Reach(g, p => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "actual-ended-equipment"));
        var frame = g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.ActualEndedEquipment is not null);
        var qualification = frame.ActualEndedEquipment!.Qualification;
        Need(qualification.TurnNumber == turn && qualification.TurnOwnerSeat == 0 &&
            qualification.UsedOnAnother == used && !qualification.DamagedAnother && qualification.MaximumDistinctOptions == (used ? 1 : 2),
            "The actual Boundary Qieting expression uses pre-End confirmations even when the live IronChain targets were subsequently cancelled.");
        g = Cold(g, registry); Answer(g, c => c.Parameters.GetValueOrDefault("option") == "draw");
        Reach(g, p =>
        {
            if (g.State.TurnNumber > turn) return true;
            Need(!Optional(p, skill), "The original current Boundary Qieting option budget cannot generate another optional invocation.");
            return false;
        });
        Need(E<ActualEndedTurnEquipmentOptionIssuedEvent>(g).Count(e => e.ProgramFrameId == frame.Id && e.Option == "draw" && e.ActualDrawCount == 1) == 1,
            "The real current Boundary Qieting draw executes once under its original frozen option budget after cold recovery.");
        _ = Cold(g, registry);
    }

    private static void RegisterConfirmedConsumerFixture(IContentRegistryBuilder builder, string scenario, List<string> skills)
    {
        var source = scenario switch
        {
            "history-tunjiang" => (Bundle: "ol-liu-qi", Skill: "ol:tunjiang"),
            "history-meihun" => (Bundle: "happy-shen-diao-chan", Skill: "happy:meihun"),
            _ => default
        };
        if (source.Bundle is null) return;
        var catalog = CharacterFixtureContent.EmbeddedProgramBundle(source.Bundle);
        var id = source.Skill;
        builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] });
        skills.Add(id);
    }

    private static void ConfigureConfirmedHistoryFixture(JsonNode rules, string scenario)
    {
        if (scenario != "history-confirmed-add") return;
        rules["skills"]!.AsArray().Single(node => node!["id"]!.GetValue<string>() == Watch)!["triggers"] = JsonNode.Parse("""
        [{"id":"confirmed-then-add","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["ironChain"],"priority":100,"optional":false,"usageScope":"game","usageLimit":1,
          "effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLegalCurrentCardTarget"},
            {"op":"addCurrentCardUseTarget","target":"selectedTarget"},
            {"op":"chooseOption","target":"owner","resultBind":"confirmed-added","options":[{"id":"continue"}]}]}]
        """);
    }
}
