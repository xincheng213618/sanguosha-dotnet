using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhugeGuoChecks
{
    private const string Qirang = "mobile:qirang", Yuhua = "mobile:yuhua", DriverId = "fixture:mobile-zhuge-guo-driver", WatchId = "fixture:mobile-zhuge-guo-watch";
    private const string Mode = "identity:classic-mobile-zhuge-guo", GainReason = "skill-program.mobile-qirang.deck-trick";
    public static void NonUseEquipmentEntryGrantsOnlyExactSnatchDistanceAndCommittedDrawCold()
    {
        var s = Scene.Start(); var equipment = s.Find(CardKind.Crossbow, seat: 1); s.PlaceAndAcquire(equipment);
        var paid = s.E<MobileQirangGainPaidEvent>().Single();
        s.Need(paid.Receipt.EquipmentCardId == equipment && paid.Receipt.Kind == CardKind.Snatch && s.V(0).Equipment.Any(c => c.Id == equipment) &&
            s.G.CardMovements.Any(m => m.CardId == equipment && m.From == CardLocation.Hand(1) && m.To == CardLocation.Equipment(0)) &&
            !s.G.CardMovements.Any(m => m.CardId == equipment && m.To == CardLocation.Processing) &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.MobileQirangGain == paid.Receipt),
            "A genuine non-Use equipment placement obtains one actual deck Snatch and pauses on its original paid gain frame.");
        s.Cold(); s.ContinueWatch(); s.Play(0);
        var acquired = paid.Receipt.CardId; var other = s.Find(CardKind.Snatch, acquired);
        s.Need(s.V(0).Hand.Single(c => c.Id == other).Kind == CardKind.Snatch &&
            !s.G.GetHumanLegalActions().Any(a => a.CardId == other && a.ConversionSource is null && a.TargetSeats.SequenceEqual([2])) &&
            s.G.GetHumanLegalActions().Any(a => a.CardId == acquired && a.ConversionSource is null && a.TargetSeats.SequenceEqual([2])),
            "Only the exact acquired entity permits native distance-two Snatch; an otherwise identical hand Snatch receives no permission.");
        var draws = s.DrawCount(0); s.Native(acquired, [2]); s.Reach(() => s.IsRewardWatch || s.P is { Kind: DecisionKind.PlayCard });
        s.Need(s.IsRewardWatch && s.DrawCount(0) == draws + 1 && s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>()
            .Any(w => s.IsQirangCommittedWindow(w) && w.Action.PhysicalCards is [var c] && c.CardId == acquired),
            "The exact physical Snatch pays natively and draws one at committed use, before its ordinary trick finishes.");
        s.ContinueWatch(); s.Play(0);
        s.Need(s.E<MobileQirangUseIssuedEvent>() is [{ CardId: var used }] && used == acquired &&
            s.G.CardMovements.Count(m => m.CardId == acquired && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.E<ProgramCardTriggerResolvedEvent>().Count(e => e.SkillId == Qirang && e.TriggerId == "acquired-trick-use" && e.OwnerSeat == 0 && e.Activated) == 1,
            "The acquired physical entity completes its one native payment and one reward without replaying equipment acquisition.");
    }
    public static void AcquiredNullificationResponseUsesOriginalCommittedDrawCold()
    {
        var s = Scene.Start(counter: true); s.PlaceAndAcquire(s.Find(CardKind.Crossbow, seat: 1));
        var acquired = s.E<MobileQirangGainPaidEvent>().Single().Receipt.CardId; s.ContinueWatch(); s.Play(0);
        var material = s.Find(CardKind.Dodge); var action = s.G.GetHumanLegalActions().FirstOrDefault(a => a.CardId == material && a.PlayedCardKind == CardKind.Duel &&
            a.ConversionSource?.SkillId == DriverId && a.TargetSeats.SequenceEqual([1]));
        s.Need(action is not null, "The existing conversion fixture exposes a genuine Duel with a different physical material.");
        s.NativeAction(action!); s.Reach(() => s.P is { Kind: DecisionKind.Nullification, PlayerSeat: 0 });
        var before = s.DrawCount(0); s.Answer(c => c.Cards.SequenceEqual([acquired]) && c.Parameters.GetValueOrDefault("response") == "nullification");
        s.Reach(() => s.IsRewardWatch || s.P is { Kind: DecisionKind.PlayCard });
        s.Need(s.IsRewardWatch && s.DrawCount(0) == before + 1 && s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w =>
            s.IsQirangCommittedWindow(w) && w.Action.Type == CardActionType.Response && w.Action.ActorSeat == 0 &&
            w.Action.ProviderSeat == 0 && w.Action.EffectiveKind == CardKind.Nullification && w.Action.PhysicalCards is [var p] && p.CardId == acquired &&
            w.CompletedResponseReturn is { IsCommitted: true, Kind: ProgramCompletedResponseKind.Nullification }),
            "An acquired real Nullification is an actual response Use and receives its draw on the same committed action nonce, despite having no ordinary designated target.");
        s.Cold(); s.ContinueWatch(); s.Play(0);
        s.Need(s.DrawCount(0) == before + 1 && s.G.CardMovements.Count(m => m.CardId == acquired && m.From == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Nullification) == 1 &&
            s.E<ProgramCardTriggerResolvedEvent>().Count(e => e.SkillId == Qirang && e.TriggerId == "acquired-trick-use" && e.OwnerSeat == 0 && e.Activated) == 1,
            "The response draw resumes its original native counterspell exactly once and does not reward the separate basic-to-Duel conversion.");
    }
    public static void EndingHandThresholdTwoThreeCategoriesAndNativeTopReorderCold()
    {
        foreach (var categories in new[] { 2, 3 })
        {
            var s = Scene.Start(categories: categories);
            s.Need(s.E<MobileYuhuaPeekStartedEvent>().Length == 0, "Mobile Yuhua has no preparation-phase top viewing.");
            var basic = s.V(0).Hand.Where(c => c.Kind == CardKind.Dodge).Select(c => c.Id).ToArray(); var excess = Math.Max(0, basic.Length - s.V(0).Hp);
            s.End(); s.Reach(() => s.P is { Kind: DecisionKind.ProgramTopReorder } || s.P is { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 });
            if (excess > 0)
            {
                s.Need(s.P is { Kind: DecisionKind.DiscardCards } discard && discard.RequiredCardCount == excess && discard.ValidCardIds.All(basic.Contains),
                    "Only actual basic cards count against the discard limit; nonbasic cards remain exempt.");
                var d = s.P!; s.Accept(new DiscardCardsCommand(0, d.ValidCardIds.Take(d.RequiredCardCount).ToArray(), d.PromptId, s.G.Revision));
                s.Reach(() => s.P is { Kind: DecisionKind.ProgramTopReorder });
            }
            s.Need(s.P is { Kind: DecisionKind.ProgramTopReorder, PlayerSeat: 0 } && s.P.DeckReorder!.Cards.Count == categories &&
                s.E<MobileYuhuaPeekStartedEvent>() is [{ Requested: var requested, HandCount: var count, Hp: var hp }] && requested == categories && count > hp &&
                Enumerable.Range(1, 3).All(i => s.G.CreateSnapshot(i).PendingDecision?.DeckReorder is null),
                "Qualified own ending privately freezes the actual two/three hand categories, rather than OL's one-card or delayed-debt quota.");
            var top = s.P!.DeckReorder!.Cards.First().Id; s.Answer(c => c.Cards.SequenceEqual([top]) && c.Parameters.GetValueOrDefault("action") == "top");
            s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("action") == "finish-top");
            while (s.P is { Kind: DecisionKind.ProgramTopReorder }) s.Answer(c => c.Parameters.GetValueOrDefault("action") == "bottom");
            s.Play(1); s.Driver("draw-one", []); s.Play(1);
            s.Need(s.V(1).Hand.Any(c => c.Id == top) && s.E<MobileYuhuaPeekStartedEvent>().Length == 1,
                "The existing native top/bottom partition really places the chosen first card on top after cold return, without repeating viewing.");
        }
        var equal = Scene.Start(categories: 2, initialHand: 4); equal.End(); equal.Play(1);
        equal.Need(equal.E<MobileYuhuaPeekStartedEvent>().Length == 0, "Hand count equal to current HP does not satisfy the strictly-greater ending condition.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public int DrawCount(int seat) => E<CardsDrawnEvent>().Where(e => e.PlayerSeat == seat).Sum(e => e.Count);
        public bool IsRewardWatch => P?.SkillPrompt?.SkillId == WatchId && P.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "reward-pause");
        public bool IsQirangCommittedWindow(ProgramCardTriggerWindowFrame w) => G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f =>
            f.SkillId == Qirang && f.TriggerId == "acquired-trick-use" && f.WindowContext is { Window: SkillProgramTriggerWindow.CardUseCommitted } c &&
            c.ParentFrameId == w.Id && c.CardUse?.CardActionId == w.Action.ActionId);
        public void ContinueWatch() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Driver(string id, int[] targets) => Accept(new UseProgramSkillCommand(P!.PlayerSeat, DriverId, id, [], targets, G.Revision, P.PromptId));
        public int Find(CardKind kind, int except = -1, int seat = 0)
        {
            for (var i = 0; i < 5; i++) { var card = V(seat).Hand.FirstOrDefault(c => c.Kind == kind && c.Id != except); if (card is not null) return card.Id; Driver(seat == 0 ? "draw-three" : "draw-three-other", seat == 0 ? [] : [seat]); Play(0); }
            throw new InvalidOperationException("The bounded real mixed deck did not provide the fixture material.");
        }
        public void PlaceAndAcquire(int card)
        {
            Driver("place-equipment", [1]); Reach(() => P!.Choices.Any(c => c.Cards.SequenceEqual([card])));
            Answer(c => c.Cards.SequenceEqual([card]));
            Reach(() => P?.SkillPrompt?.SkillId == Qirang && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(() => P?.SkillPrompt?.SkillId == WatchId || P is { Kind: DecisionKind.PlayCard });
            Need(P?.SkillPrompt?.SkillId == WatchId, "The real Qirang deck-to-hand gain must stop in its native paid gain child.");
        }
        public void Native(int card, int[] targets) => NativeAction(G.GetHumanLegalActions().First(a => a.CardId == card && a.ConversionSource is null && a.TargetSeats.SequenceEqual(targets)));
        public void NativeAction(LegalAction a) => Accept(new PlayCardCommand(P!.PlayerSeat, a.CardId!.Value, a.TargetSeats, G.Revision, P.PromptId, a.PlayedCardKind, a.TargetCardId)
            { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources ?? [] });
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End() => Accept(new EndPlayPhaseCommand(P!.PlayerSeat, G.Revision, P.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var choice = p.Choices.FirstOrDefault(predicate); Need(choice is not null, "Required native Zhuge Guo prompt choice is missing."); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Actual mobile Zhuge Guo command rejected."); }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 220 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } general) { Accept(new SelectGeneralCommand(general.PlayerSeat, General(general.PlayerSeat), G.Revision, general.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.ProgramTopReorder }) { Answer(c => c.Parameters.GetValueOrDefault("action") == "top"); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } d) { Accept(new DiscardCardsCommand(d.PlayerSeat, d.ValidCardIds.Take(d.RequiredCardCount).ToArray(), d.PromptId, G.Revision)); continue; }
                if (P is { } p)
                {
                    var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == "continue") ??
                        p.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0) ?? p.Choices.First();
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded actual mobile Zhuge Guo boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                G.CardMovements.SequenceEqual(copy.CardMovements) && G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).SequenceEqual(copy.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType()))),
                "Cold recovery preserves the same paid entity, actual committed use and private native reorder cursor."); G = copy;
        }
        public void Need(bool ok, string text)
        {
            if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { Prompt = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId,
                    Choices = p.Choices.Take(8).Select(c => new { c.Cards, c.Targets, c.Parameters }).ToArray(), ChoiceCount = p.Choices.Count } : null, G.State.TurnNumber, G.State.CurrentSeat,
                Facts = G.Events.Select(e => e.Payload).Where(e => e is MobileQirangGainPaidEvent or MobileQirangUseIssuedEvent or MobileYuhuaPeekStartedEvent or ProgramBindingResolvedEvent or ProgramCardTriggerResolvedEvent)
                    .TakeLast(10).Select(e => JsonSerializer.Serialize(e, e.GetType())).ToArray() }));
        }
        public static Scene Start(bool counter = false, int categories = 3, int initialHand = 12)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(counter, categories, initialHand));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 9 }, registry), registry);
            s.Accept(new StartGameCommand()); s.Play(0); return s;
        }
    }
    private static string General(int seat) => "fixture:mobile-zhuge-guo-general-" + seat;
    private sealed class Fixture(bool counter, int categories, int initialHand) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-zhuge-guo", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, "mobile:zhuge-guo");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{DriverId}}","revision":1,"activations":[
                {"id":"place-equipment","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"selectedTarget"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"cardCategories":["equipment"],"count":1,"destination":"selectedTargetEquipment","targetRef":{"kind":"owner"},"awaitMovementTriggers":true}]},
                {"id":"draw-three","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":3}]},
                {"id":"draw-three-other","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"selectedTarget","amount":3}]},
                {"id":"draw-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1}]}
              ],"viewAs":[{"id":"duel","inputKinds":[],"inputSuits":[],"outputKind":"duel","forPlay":true,"forResponse":false,"useOnly":true,"singleCardTrickUse":true}]},
              {"id":"{{WatchId}}","revision":1,"triggers":[
                {"id":"paid-gain-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["{{GainReason}}"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"gain-pause","options":[{"id":"continue"}]}]},
                {"id":"committed-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.mobile:qirang.Draw"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"reward-pause","options":[{"id":"continue"}]}]}
              ]}
            ]}
            """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [DriverId] = new { name = "真实移动诸葛果准备", description = "实际非使用装备放置与摸牌。" },
                    [WatchId] = new { name = "真实祈禳付款子窗", description = "原生得牌暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            for (var seat = 0; seat < 4; seat++) b.AddGeneral(new(General(seat), "固定真实参与者", "supporter", seat == 0 ? Qirang : "standard:none", "shu", seat == 0 ? 3 : 4,
                seat == 0 ? [Yuhua, DriverId, WatchId] : [DriverId], GeneralGender.Female));
            var kinds = new List<ContentDeckPhysicalCard> { new(counter ? "standard:nullification" : "standard:snatch", Suit.Heart, 7), new("standard:dodge", Suit.Club, 2) };
            if (categories == 3) kinds.Add(new("standard:crossbow", Suit.Club, 1));
            const string deck = "fixture:mobile-zhuge-guo-deck";
            b.AddDeck(new(deck, "固定真实三类材料", initialHand, 0, []) { PhysicalCards = Enumerable.Range(0, 48).SelectMany(_ => kinds).ToArray() });
            b.AddMode(new(Mode, "移动诸葛果原生流程", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Enumerable.Range(0, 4).Select(General).ToArray()));
        }
    }
}
