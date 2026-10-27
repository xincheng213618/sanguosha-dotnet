using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class WuLuKangDixianChecks
{
    private const string Draw = "skill-program.wumiao:dixian.draw", Obtain = "skill-program.wumiao:dixian.obtain";
    private static ShanheStartingCard C(CardKind kind, int rank = 7, Suit suit = Suit.Spade) => WuLuKangScene.C(kind, rank, suit);
    private static void Finished(WuLuKangScene s, long id) => s.Reach(() => s.E<WuLuKangDixianCompletedEvent>().Any(e => e.FrameId == id));
    private static void Spent(WuLuKangScene s)
    {
        s.Need(!s.Actions().Any(a => a.ProgramSkillId == WuLuKangScene.Dixian), "The real game-limited activation is already consumed.");
        s.Reject(new UseProgramSkillCommand(0, WuLuKangScene.Dixian, "declare-rank", [], [], s.G.Revision, s.P!.PromptId));
    }
    public static void SuccessfulGuessEffectiveRankNativeQuotaAndLifetimeCold()
    {
        var s = WuLuKangScene.Start(new("success-rank-six-permanent") { Dixian = true, ConditionalSources = true, PauseUse = true, WatchDraw = true,
            Hand = [C(CardKind.ZhangbaSerpentSpear), C(CardKind.Slash, 7), C(CardKind.Slash, 6), C(CardKind.Slash, 6), C(CardKind.Slash, 6), C(CardKind.Slash, 7),
                C(CardKind.Alcohol, 7), C(CardKind.Alcohol, 6), C(CardKind.Alcohol, 6), C(CardKind.Alcohol, 7), C(CardKind.Peach, 1), C(CardKind.IronChain, 2), C(CardKind.Dodge, 4), C(CardKind.Dodge, 4)],
            Stock = Enumerable.Range(0, 18).Select(_ => C(CardKind.Dodge, 7)).ToArray() });
        s.Native(CardKind.Slash, [1], 7); s.Play(); s.Native(CardKind.Alcohol, [], 7); s.Play();
        s.Need(!s.Actions().Any(a => a.Kind == LegalActionKind.Slash && a.TargetSeats.Contains(2)) &&
            !s.Actions().Any(a => a.Kind == LegalActionKind.Alcohol), "Ordinary Slash and Alcohol quotas are actually spent before the limited skill.");
        s.BeginDixian(); var started = s.E<WuLuKangDixianStartedEvent>().Single();
        s.Need(s.P!.Choices.Count == 13 && s.P.Choices.All(c => c.Cards.Count == 0 && c.Targets.Count == 0) &&
            s.P.Choices.Select(c => c.Parameters.GetValueOrDefault("rank")).ToHashSet().SetEquals(Enumerable.Range(1, 13).Select(i => i.ToString())),
            "A genuine accepted limited skill privately publishes exactly thirteen rank declarations without revealing stock identities.");
        s.Private(); s.RejectPrompt(); s.Cold(); s.ChooseRank(6); s.Reach(() => s.Hold());
        s.Need(s.E<WuLuKangDixianRankChosenEvent>().Single() is { Rank: 6, AllAtLeast: true, StockCount: 18 } &&
            s.E<WuLuKangDixianDrawReturnedEvent>().Single() is { RequestedCount: 6, ActualCount: 6, ReplacementCount: 0 } &&
            s.E<WuLuKangDixianGrantedEvent>().Length == 0 && s.G.CardMovements.Count(m => m.Reason.Value == Draw && m.To == CardLocation.Hand(0)) == 6,
            "The entire accepted stock predicate is frozen; all six real draws and their gain child precede the permanent grant.");
        s.Private(); s.Cold(); s.Continue(); Finished(s, started.FrameId); s.Play();
        var grant = s.E<WuLuKangDixianGrantedEvent>().Single().Grant;
        s.Need(grant.Rank == 6 && grant.Source.OwnerSeat == 0 && grant.ProgramFrameId == started.FrameId, "One exact rank-six permanent grant is issued only after real draw descendants return."); Spent(s);
        foreach (var expected in new[] { 1, 2, 3 })
        {
            var card = s.V().Hand.First(c => c.Kind == CardKind.Slash && c.Rank == 6).Id;
            s.Native(CardKind.Slash, [2], 6); s.Reach(() => s.Hold());
            var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(u => u.CardId == card);
            s.Need(use.Action is { Type: CardActionType.Use, EffectiveRank: 6 } && use.WuLuKangDixianUse is { Applies: true, EffectiveRank: 6 } receipt && receipt.Grant == grant &&
                s.G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
                "Each equality-bound real Slash uses its original paid entity and frozen effective rank, reaches distance two and exceeds only the ordinary count quota.");
            s.Private(); s.Cold(); s.Continue(); s.Play();
            s.Need(s.E<WuLuKangDixianUseAdmittedEvent>().Count(e => e.Receipt.Applies && e.Receipt.Kind == CardKind.Slash && e.Receipt.EffectiveRank == 6) == expected,
                "Each actual rank-six Use receives one admission fact, never a synthetic free action or duplicated return.");
            if (expected < 3) { s.Native(CardKind.Alcohol, [], 6); s.Play(); }
        }
        s.Need(!s.Actions().Any(a => a.Kind == LegalActionKind.Slash && a.CardId is { } id && s.V().Hand.Any(c => c.Id == id && c.Rank == 7)),
            "A printed/effective seven above the bound cannot borrow unlimited Slash usage even at a near target.");
        s.Need(!s.Actions().Any(a => a.Kind == LegalActionKind.Alcohol && a.CardId is { } id && s.V().Hand.Any(c => c.Id == id && c.Rank == 7)),
            "Two eligible Alcohol Uses bypass their count without refunding the already spent ordinary Alcohol quota to high ranks.");
        s.Use("lose-one"); s.Play(); s.Need(!s.Has(WuLuKangScene.Dixian), "A genuine HP change removes the original conditional source.");
        s.Need(Enumerable.Range(0, 4).All(v => s.G.CreateSnapshot(v).Players[0].ActiveEffects?.Any(e => e.SkillId == WuLuKangScene.Dixian && e.SourceSeat == 0 &&
            e.Expiry == ActivePlayerEffectExpiry.RuleCleared && e.Description.Contains("6", StringComparison.Ordinal)) == true),
            "The issued permanent rank bound remains public to every safe viewer after its original skill source is lost.");
        s.Native(CardKind.ZhangbaSerpentSpear, []); s.Play();
        var pair = s.V().Hand.Where(c => c.Rank is 1 or 2).Take(2).Select(c => c.Id).ToArray();
        s.Need(pair.Length == 2, "The original two exact low-rank materials remain available for native Zhangba.");
        s.Accept(new UseEquipmentEffectCommand(0, CardKind.ZhangbaSerpentSpear, pair, [2], s.G.Revision, s.P!.PromptId)); s.Reach(() => s.Hold());
        var spear = s.G.ResolutionStack.OfType<CardUseFrame>().Last();
        s.Need(spear.Action is { PhysicalCards.Count: 2, EffectiveRank: 3 } && spear.WuLuKangDixianUse is { Applies: true, EffectiveRank: 3 },
            "A genuine two-material Zhangba uses its actual effective rank sum three, retains both real payments and preserves the issued privilege after source loss."); s.Cold(); s.Continue(); s.Play();
        var tooHighPair = s.V().Hand.Where(c => c.Kind == CardKind.Dodge && c.Rank == 4).Take(2).Select(c => c.Id).ToArray();
        s.Reject(new UseEquipmentEffectCommand(0, CardKind.ZhangbaSerpentSpear, tooHighPair, [2], s.G.Revision, s.P!.PromptId));
        s.Use("recover-one"); s.Play(); s.Need(s.Has(WuLuKangScene.Dixian) && s.E<WuLuKangDixianGrantedEvent>().Length == 1, "Real source regrant retains one issued rank bound without resetting the limited skill."); Spent(s); s.Cold();

        var replaced = WuLuKangScene.Start(new("success-real-draw-replacement") { Dixian = true, Zenrun = true, Stock = Enumerable.Range(0, 8).Select(_ => C(CardKind.Dodge, 7)).ToArray() });
        replaced.BeginDixian(); replaced.ChooseRank(1); replaced.Reach(() => replaced.Optional("ol:zenrun")); replaced.Private(); replaced.Cold(); replaced.Activate();
        replaced.Reach(() => replaced.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "target") == true); replaced.Answer(c => c.Parameters.GetValueOrDefault("branch") == "target" && c.Targets.SequenceEqual([2]));
        replaced.Reach(() => replaced.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "pick") == true); replaced.Private();
        replaced.Answer(c => c.Parameters.GetValueOrDefault("branch") == "pick" && c.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand)); replaced.Answer(c => c.Parameters.GetValueOrDefault("branch") == "take");
        Finished(replaced, replaced.E<WuLuKangDixianStartedEvent>().Single().FrameId); replaced.Play();
        replaced.Need(replaced.E<WuLuKangDixianDrawReturnedEvent>().Single() is { RequestedCount: 1, ActualCount: 0, ReplacementCount: 1 } && replaced.E<WuLuKangDixianGrantedEvent>().Single().Grant.Rank == 1 &&
            replaced.G.CardMovements.All(m => m.Reason.Value != Draw), "An actual native replacement is not falsely recorded as drawing the minimum/top entity, and requested Draw1 still completes the permanent grant."); replaced.Cold();

        foreach (var childLoss in new[] { 1, 4 })
        {
            var paid = WuLuKangScene.Start(new("successful-draw-child-source-" + childLoss) { Dixian = true, ConditionalSources = true, WatchDraw = true, DrawChildLoss = childLoss,
                Stock = Enumerable.Range(0, 4).Select(_ => C(CardKind.Dodge, 7)).ToArray() });
            paid.BeginDixian(); paid.ChooseRank(1); paid.Reach(() => paid.Hold()); var parent = paid.E<WuLuKangDixianStartedEvent>().Single().FrameId;
            paid.Need(paid.E<WuLuKangDixianDrawReturnedEvent>().Single() is { RequestedCount: 1, ActualCount: 1 } && paid.E<WuLuKangDixianGrantedEvent>().Length == 0,
                "The accepted successful branch holds its actual returned draw while the genuine gain child is still unresolved.");
            paid.Private(); paid.Cold(); paid.Continue(); Finished(paid, parent);
            paid.Need(paid.E<WuLuKangDixianDrawReturnedEvent>().Length == 1 && paid.E<WuLuKangDixianCompletedEvent>().Count(e => e.FrameId == parent && e.Granted == (childLoss == 1)) == 1 &&
                paid.E<WuLuKangDixianGrantedEvent>().Length == (childLoss == 1 ? 1 : 0) && !paid.Has(WuLuKangScene.Dixian),
                "A paid draw child that removes the live source still grants once for its living owner, while actual nonterminal owner death stops the permanent grant and preserves the once-returned draw.");
            if (childLoss == 1) { paid.Play(); Spent(paid); } else paid.Need(!paid.V().IsAlive && paid.V(1).IsAlive && paid.G.State.Winner == Winner.None, "The success death branch is an actual nonterminal native death, not a terminal shortcut."); paid.Cold();
        }

        var empty = WuLuKangScene.Start(new("empty-stock-vacuous-success") { Dixian = true, Stock = [C(CardKind.Dodge, 7)] }); empty.Use("draw20"); empty.Play();
        empty.Need(empty.Stock().Length == 0, "A real native draw exhausts the one-card stock before the declaration."); empty.BeginDixian(); empty.ChooseRank(13); Finished(empty, empty.E<WuLuKangDixianStartedEvent>().Single().FrameId); empty.Play();
        empty.Need(empty.E<WuLuKangDixianRankChosenEvent>().Single() is { Rank: 13, AllAtLeast: true, StockCount: 0 } &&
            empty.E<WuLuKangDixianDrawReturnedEvent>().Single() is { RequestedCount: 13, ActualCount: 0 } && empty.E<WuLuKangDixianGrantedEvent>().Single().Grant.Rank == 13,
            "An empty current stock freezes vacuous success before its genuine short requested Draw13, rather than inventing thirteen actual cards."); Spent(empty); empty.Cold();
    }
}
