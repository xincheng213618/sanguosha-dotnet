using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileHeTaiHouChecks
{
    private const string Owner = "fixture:mobile-he-tai-hou-owner", Driver = "fixture:mobile-he-tai-hou-driver", Observer = "fixture:mobile-he-tai-hou-paid", Mode = "identity:mobile-he-tai-hou";
    public static void OwnerKillsExcludeOtherDeathsAndKeepOlDefault()
    {
        foreach (var mobile in new[] { true, false })
        {
            var s = Scene.Start(mobile, wine: false); s.Play(0);
            s.Use("kill", 1); s.Play(0); s.Use("source-less-death", 2); s.Play(0);
            s.Need(s.G.Events.Select(e => e.Payload).OfType<PlayerDiedEvent>().Count(e => e.KillerSeat == 0) == 1 &&
                s.G.Events.Select(e => e.Payload).OfType<PlayerDiedEvent>().Count(e => e.KillerSeat != 0) == 1,
                "The same actual turn has one real owner kill and one source-less HP-loss death.");
            s.End(); s.Reach(() => s.Offer(s.Qiluan)); s.Cold(); s.Activate();
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Observer); s.Cold();
            s.Need(s.Draws == (mobile ? 3 : 4), "Mobile counts only its one owner kill times three; absent optional flags preserve OL's deaths plus twice owner kills.");
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play(3);
            s.End(); s.Play(0);
            s.Need(s.Draws == (mobile ? 3 : 4), "The next actual turn cannot reuse the earlier kill or source-less death for another draw.");
        }
    }
    public static void ForeignPhaseWineKillRewardsAtDeadActorsOriginalTurnEndCold()
    {
        var s = Scene.Start(mobile: true, wine: true); s.Play(0);
        s.Need(s.OwnWineOffers == 0, "Mobile Zhendu has no self Play-phase offer.");
        s.Use("weaken", 1); s.Play(0); s.End();
        s.Reach(() => s.Offer("mobile:zhendu") && s.G.State.CurrentSeat == 1); s.Activate();
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true);
        var cost = s.P!.Choices.First(c => c.Cards.Count == 1).Cards.Single(); s.Cold(); s.Answer(c => c.Cards.SequenceEqual([cost]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Observer && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "wine"));
        var committedWine = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardKind == CardKind.Alcohol);
        s.Need(committedWine is { CardId: 0, PhysicalCardIds.Count: 0, SourceSeat: 1, Action: { ActorSeat: 1, ProviderSeat: 0, Type: CardActionType.Use, PhysicalCards.Count: 0 } },
            "The fixture observes the actual committed zero-entity Wine before its original native return.");
        var wine = committedWine.Action!;
        var wineParent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == "mobile:zhendu");
        var paidWine = wineParent.PhaseAlcohol!;
        s.Need(paidWine is { Stage: ProgramPhaseAlcoholStage.WineIssued, ActorSeat: 1, PaymentInstruction: 2, WineInstruction: 3 } &&
            paidWine.CardId == cost && paidWine.WineFrameId == committedWine.Id && committedWine.VirtualBasicReturn is { } returned &&
            returned.ParentFrameId == wineParent.Id && returned.InstructionIndex == paidWine.WineInstruction &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.Sequence > paidWine.PaymentSequenceBefore && m.Sequence <= paidWine.PaymentSequenceAfter &&
                m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == paidWine.PaymentReason) == 1,
            "The formal PhaseAlcohol payment receipt and typed virtual-basic return bind this exact zero-entity child to the original once-paid foreign phase.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.Offer(s.Qiluan));
        s.Need(s.G.State.CurrentSeat == 1 && !s.G.CreateSnapshot(0).Players[1].IsAlive &&
            s.G.Events.Select(e => e.Payload).OfType<PlayerDiedEvent>().Single().KillerSeat == 0 &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.Reason.Value == "skill-program.mobile:zhendu.MoveBoundCards" && m.To == CardLocation.DiscardPile) == 1,
            "The paid foreign phase actor uses its real zero-entity Wine, then owner damage kills it inside its own original actual turn.");
        s.Need(wine is { ActorSeat: 1, ProviderSeat: 0, Type: CardActionType.Use, PhysicalCards.Count: 0 } && wine.TargetSeats.SequenceEqual([1]),
            "The frozen native action keeps the actual Wine user, provider and self-target separate without physical materials.");
        var facts = s.G.Events.Select(e => e.Payload).ToArray();
        var declared = Array.FindIndex(facts, e => e is CardUseDeclaredEvent { CardId: 0, CardKind: CardKind.Alcohol, SourceSeat: 1 } declaration && declaration.ResolutionId == committedWine.Id);
        var applied = Array.FindIndex(facts, e => e is AlcoholAppliedEvent { SourceSeat: 1 } application && application.ResolutionId == committedWine.Id);
        var finished = Array.FindIndex(facts, e => e is CardUseFinishedEvent { CardId: 0, CardKind: CardKind.Alcohol } completion && completion.ResolutionId == committedWine.Id);
        var damage = Array.FindIndex(facts, e => e is DamageAppliedEvent { SourceSeat: 0, TargetSeat: 1, Amount: 1, SourceLess: false });
        var death = Array.FindIndex(facts, e => e is PlayerDiedEvent { VictimSeat: 1, KillerSeat: 0 });
        var ended = Array.FindIndex(facts, e => e is TurnEndedEvent { ActorSeat: 1 } turn && turn.TurnNumber == paidWine.TurnNumber);
        s.Need(declared >= 0 && declared < applied && applied < finished && finished < damage && damage < death && death < ended &&
            facts.OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == committedWine.Id) == 1,
            "The original Wine declares, applies and finishes exactly once before its independent skill damage, real owner-attributed death and original actual turn-end fact.");
        s.Activate(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Observer); s.Cold();
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == s.Qiluan);
        s.Need(s.Draws == 3 && root.WindowContext is { SourceSeat: 1, TargetSeat: 1, Facts: { ActualTurnDeathCount: 1, OwnerActualTurnKillCount: 1 } },
            "The paid original turn-end draw retains one frozen owner kill from the dead actor's actual turn and pays three cards once.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play(2);
        s.Need(s.OwnWineOffers == 0 && s.Draws == 3, "The dead actor's completion returns before the next live turn without a duplicate draw.");
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry, bool mobile)
    {
        public GameEngine G = game;
        public string Qiluan => mobile ? "mobile:qiluan" : "ol:qiluan";
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public int Draws => G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program." + Qiluan + ".Draw");
        public int OwnWineOffers;
        public bool Offer(string skill) => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile He Tai Hou command."); }
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End() { var p = P!; Accept(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); }
        public void Use(string activation, int target) => Accept(new UseProgramSkillCommand(0, Driver, activation, [], [target], G.Revision, P!.PromptId));
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 220 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P is { Kind: DecisionKind.DiscardCards } discard) { Accept(new DiscardCardsCommand(discard.PlayerSeat, discard.ValidCardIds.Take(discard.RequiredCardCount).ToArray(), discard.PromptId, G.Revision)); continue; }
                if (Offer("mobile:zhendu") && G.State.CurrentSeat == 0) OwnWineOffers++;
                if (P is { } p)
                {
                    var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        p.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0) ?? p.Choices.First();
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded mobile He Tai Hou boundary.");
        }
        public void Cold()
        {
            var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(Enumerable.Range(0, 4).All(i => SnapshotJson.Serialize(G.CreateSnapshot(i)) == SnapshotJson.Serialize(copy.CreateSnapshot(i))) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(copy.ResolutionStack) && G.CardMovements.SequenceEqual(copy.CardMovements),
                "Cold restoration preserves the frozen original actual-turn deaths, paid cards and native draw frames."); G = copy;
        }
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { P, Turn = G.State.TurnNumber, Current = G.State.CurrentSeat, Draws })); }
        public static Scene Start(bool mobile, bool wine)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(mobile, wine));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r), r, mobile);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-he-tai-hou-peer-" + seat;
    private sealed class Fixture(bool mobile, bool wine) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mobile-he-tai-hou", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, mobile ? "mobile:he-tai-hou" : "ol:he-tai-hou");
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
             {"id":"{{Driver}}","revision":1,"activations":[
              {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damageParticipants","target":"selectedTarget","amount":20}]},
              {"id":"source-less-death","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":20}]},
              {"id":"weaken","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":3}]}]},
             {"id":"{{Observer}}","revision":1,"triggers":[{"id":"paid-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.{{(mobile ? "mobile:qiluan" : "ol:qiluan")}}.Draw"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]},
              {"id":"committed-wine-child","window":"cardUseCommitted","ownerRelation":"observer","cardKinds":["alcohol"],"optional":false,
               "effects":[{"op":"chooseOption","target":"owner","resultBind":"wine","options":[{"id":"continue"}]}]}]}
            ]}
            """;
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实击杀驱动", description = "实际伤害与无来源体力流失" }, [Observer] = new { name = "已付款摸牌暂停", description = "原生摸牌孩子", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, id, id) { Program = p });
            var additional = new List<string> { Driver, Observer }; if (wine) additional.Add("mobile:zhendu");
            b.AddGeneral(new(Owner, "移动何太后实际回合", "supporter", mobile ? "mobile:qiluan" : "ol:qiluan", "qun", 8, additional));
            for (var i = 1; i < 4; i++) b.AddGeneral(new(Peer(i), "固定真实同桌", "supporter", "standard:none", "wei", 4));
            const string deck = "fixture:mobile-he-tai-hou-deck";
            b.AddDeck(new(deck, "无救援实体闪", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 80).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 2)).ToArray() });
            b.AddMode(new(Mode, "移动何太后实际回合", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
