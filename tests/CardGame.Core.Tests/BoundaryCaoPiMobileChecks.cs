using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;
internal static class BoundaryCaoPiMobileChecks
{
    private const string Hero = "boundary:cao-pi-mobile", Xing = "boundary:xingshang-mobile", Fang = "boundary:fangzhu-mobile", Song = "boundary:songwei-mobile",
        Wei = "fixture:boundary-cao-pi-wei", Judger = "fixture:boundary-cao-pi-judger", Shu = "fixture:boundary-cao-pi-shu", Quiet = "fixture:boundary-cao-pi-quiet", Skip = "fixture:boundary-cao-pi-skip", Driver = "fixture:boundary-cao-pi-driver",
        Paid = "fixture:boundary-cao-pi-paid", Gained = "fixture:boundary-cao-pi-gained";
    public static void DeathChoiceAndForeignJudgmentChooser()
    {
        var s = Scene.Start(); s.Play(); s.Equip(2); s.Play(); var armor = s.V(2).Equipment.Single().Id;
        var delayed = s.V().Hand.Single(c => c.Kind == CardKind.Indulgence).Id; s.Native(delayed, 2); s.Play();
        var corpse = s.V(2).Hand.Select(c => c.Id).Append(armor).ToArray(); s.Kill(2); s.Offer(Xing); s.Activate(); s.Choice(Xing);
        s.Need(s.P!.Choices.Select(c => c.Parameters.GetValueOrDefault("branch")).Order().SequenceEqual(new[] { "claim", "recover" }), "A wounded Cao Pi receives both real death alternatives after cleanup.");
        s.Cold(); s.Branch("claim"); s.Play();
        s.Need(corpse.All(id => s.Zone(id) == CardLocation.Hand(0)) && s.Zone(delayed) == CardLocation.DiscardPile && s.V().Hp == 7 &&
            s.Facts<BoundaryCaoPiChoiceResultEvent>().Single(e => e.SkillId == Xing).Outcome == "claim",
            "Native source-filtered Xingshang takes every actual HE corpse entity, preserves the judgment discard, and does not also recover.");
        s = Scene.Start(targetCards: 0); s.Play(); s.Kill(2); s.Offer(Xing); s.Activate(); s.Choice(Xing);
        s.Need(s.P!.Choices is [{ Parameters: var options }] && options.GetValueOrDefault("branch") == "recover", "An empty corpse still permits the independent recovery alternative without offering an empty claim.");
        s.Branch("recover"); s.Play(); s.Need(s.V().Hp == 8 && s.Facts<BoundaryCaoPiChoiceResultEvent>().Single(e => e.SkillId == Xing).Outcome == "recover", "The death recovery branch genuinely recovers one HP without requiring any corpse cards.");

        s = Scene.Start(songwei: true); s.Play(); var before = s.V(1).Hand.Count; s.Judge(0); s.Option(Song);
        s.Need(s.P!.PlayerSeat == 0 && s.P.SkillPrompt!.SkillId == Song && (s.G.CreateSnapshot(1).PendingDecision?.Choices.Count ?? 0) == 0, "The real Wei judgment subject chooses whether the qualified printed lord draws; the lord has no competing answer.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "draw"); s.Play(); s.Need(s.V(1).Hand.Count == before + 1, "Accepting the actual black judgment's Songwei draws one native card for the lord.");
        s.Judge(0); s.Option(Song); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "decline"); s.Play();
        s.Judge(1); s.Play(); s.Judge(2); s.Play();
        s.Need(s.V(1).Hand.Count == before + 1, "Declining, the lord's own black judgment and a foreign non-Wei judgment issue no extra Songwei draw.");
    }
    public static void TargetChoiceFrozenDrawAndExactHePaidCold()
    {
        var s = Scene.Start(); s.Play(); s.Hurt(); s.Offer(Fang); s.Activate(); s.Target(1); s.Choice(Fang);
        s.Need(s.P!.PlayerSeat == 1 && s.Root?.BoundaryFangzhu is { Target: 1, X: 2 }, "After one real native damage the selected role itself, rather than Cao Pi, owns the frozen-two Fangzhu choice.");
        var before = s.V(1).Hand.Count; s.Branch("draw"); s.GainHold();
        s.Need(!s.V(1).IsFaceDown && s.V(1).Hand.Count == before + 2 && s.Root?.BoundaryFangzhu is { Stage: BoundaryFangzhuStage.DrawChildren, X: 2, Actual: 2 }, "The native draw is complete and its original child remains before the promised turn-over.");
        s.Cold(); s.Release(); s.Target(0); s.Play();
        s.Need(s.V(1).IsFaceDown && s.V(1).Hand.Count == before + 2 && s.V().Hp == 7 && s.Facts<BoundaryFangzhuDrawReturnedEvent>() is [{ Requested: 2, Actual: 2 }] &&
            s.Facts<BoundaryCaoPiChoiceResultEvent>().Single(e => e.SkillId == Fang) is { Chooser: 1, Outcome: "drawn", Count: 2 },
            "Cold finishes the draw child once, allows its actual recovery of Cao Pi without rewriting X, then flips the selected role.");

        s = Scene.Start(); s.Play(); s.Equip(1); s.Play(); var armor = s.V(1).Equipment.Single().Id; var hand = s.V(1).Hand.First().Id;
        s.Hurt(); s.Offer(Fang); s.Activate(); s.Target(1); s.Choice(Fang); s.Branch("card", armor); s.Cold();
        s.Need(s.Zone(armor) == CardLocation.Equipment(1) && s.Zone(hand) == CardLocation.Hand(1) && s.Facts<BoundaryFangzhuPaidEvent>().Length == 0, "One staged HE entity survives cold without prematurely paying the target's two-card cost.");
        s.AiPaysOriginalDraft(); s.PaidHold();
        s.Need(s.Zone(armor) == CardLocation.DiscardPile && s.Zone(hand) == CardLocation.DiscardPile && s.V(1).Hp == 7 && !s.V(1).IsFaceDown &&
            s.Root?.BoundaryFangzhu is { Stage: BoundaryFangzhuStage.DiscardChildren, X: 2, Actual: 2 } && s.Facts<BoundaryFangzhuPaidEvent>() is [{ X: 2, Actual: 2 }],
            "The target pays its complete native HE batch once, including Silver Lion removal recovery, before HP loss or any turn-over.");
        s.Cold(); s.Release(); s.Settle();
        s.Need(s.V(1).Hp == 6 && !s.V(1).IsFaceDown && s.Facts<BoundaryFangzhuDrawReturnedEvent>().Length == 0 && s.Facts<BoundaryFangzhuPaidEvent>().Length == 1 &&
            s.Facts<DamageAppliedEvent>().All(e => e.TargetSeat != 1) && s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.boundary-cao-pi.fangzhu.discard") == 2,
            "The original paid cold return loses exactly one HP without creating damage, flipping, drawing or paying a second time.");
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Fang);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Offer(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Choice(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "boundary-cao-pi"));
        public void Option(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "choose-option"));
        public void Branch(string branch, int? card = null) => Answer(c => c.Parameters.GetValueOrDefault("branch") == branch && (card is null || c.Cards.SequenceEqual([card.Value])));
        public void GainHold() => Option(Gained);
        public void PaidHold() => Option(Paid);
        public void AiPaysOriginalDraft()
        {
            for (var i = 0; i < 20; i++)
            {
                if (P?.SkillPrompt?.SkillId == Paid) return;
                Need(P is { PlayerSeat: 1 } && P.SkillPrompt?.SkillId == Fang, "The real nonhuman recipient retains the original staged Fangzhu draft while its native AI completes payment.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The recipient's real AI must complete the staged cost rather than toggling its already selected entity.");
        }
        public void Release() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Target(int seat) { Reach(() => P?.Choices.Any(c => c.Targets.SequenceEqual([seat])) == true); Answer(c => c.Targets.SequenceEqual([seat])); }
        public void Hurt() => DriverUse("hurt");
        public void Kill(int seat) => DriverUse("kill", seat);
        public void Judge(int seat) => DriverUse("judge", seat);
        public void Equip(int seat)
        {
            DriverUse("equip", seat); Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            var id = P!.Choices.First(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card").Id; Answer(c => c.Id == id);
        }
        private void DriverUse(string action, params int[] targets) => Accept(new UseProgramSkillCommand(0, BoundaryCaoPiMobileChecks.Driver, action, [], targets, G.Revision, P!.PromptId));
        public void Native(int card, int target)
        {
            var a = G.GetHumanLegalActions().Single(a => a.CardId == card && a.TargetSeats.SequenceEqual([target]));
            Accept(new PlayCardCommand(0, card, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; var c = p.Choices.Single(predicate); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c.Id, G.Revision)); }
        public void Settle()
        {
            for (var i = 0; i < 100; i++)
            {
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                if (P?.SkillPrompt?.SkillId == Paid) { Release(); continue; }
                Need(P is null, "Unexpected choice while completing the original Cao Pi movement child."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Cao Pi original movement child missed its bounded return.");
        }
        public void Reach(Func<bool> done)
        { for (var i = 0; i < 300; i++) { if (done()) return; Need(P is null, "Cao Pi fixture reached an unexpected native decision."); Accept(new AdvanceOneStepCommand(G.Revision)); } Need(false, "Cao Pi fixture missed its bounded native boundary."); }
        public void Cold() { var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(State() == before, "Cold preserves Cao Pi's original native death/damage/judgment frames, actual chooser, frozen X and once-paid entities."); }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements });
        private void Accept(GameCommand c) { var result = G.Submit(c); Need(result.Accepted, result.Error?.Message ?? "Rejected native Cao Pi command."); }
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, P, Root = Root?.BoundaryFangzhu, Last = G.Events.TakeLast(10).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) })); }
        public static Scene Start(bool songwei = false, int targetCards = 2)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] hand = songwei ? [] : [new(CardKind.SilverLion, Suit.Club, 1), new(CardKind.Indulgence, Suit.Heart, 4), new(CardKind.Dodge, Suit.Club, 5)];
            var friend = songwei ? new ShanheCombatantDefinition(Hero, MaxHpBonus: 5, Hp: 8, ExtraSkillIds: [Quiet, Skip]) { StartingHand = [], QualifiedPrintedLordSkillIds = [Song] } :
                new(Wei, Hp: 6, ExtraSkillIds: [Paid, Gained]) { StartingHand = [new(CardKind.Dodge, Suit.Club, 6), new(CardKind.Dodge, Suit.Club, 7)] };
            var setup = new ShanheBattleSetup(29, 1, "boundary-cao-pi", songwei ? Judger : Hero, songwei ? 8 : 7, songwei ? 0 : 5, 0, 0, 0, songwei ? [] : [Quiet, Driver], hand, [],
                [new(Shu, Hp: 8) { StartingHand = Enumerable.Range(0, targetCards).Select(i => new ShanheStartingCard(CardKind.Dodge, Suit.Club, i + 8)).ToArray() }, new(Shu, Hp: 8) { StartingHand = [] }], [friend], new string('d', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-cao-pi-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            b.AddCard(new("fixture:boundary-cao-pi-silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
             {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
             {"id":"{{Skip}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
             {"id":"{{Driver}}","revision":1,"activations":[
              {"id":"hurt","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","amount":1}]},
              {"id":"kill","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":20}]},
              {"id":"judge","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"startJudgment","target":"selectedTarget","judgmentReason":"fixture.boundary-cao-pi","resultBind":"judged","visibility":"public"},{"op":"moveBoundCards","target":"owner","sourceBind":"judged","destination":"discardPile"}]},
              {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"zones":["hand"],"cardCategories":["equipment"],"count":1,"destination":"selectedTargetEquipment"}]}
             ]},
             {"id":"{{Paid}}","revision":1,"triggers":[{"id":"paid","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill-program.boundary-cao-pi.fangzhu.discard"],"usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
             {"id":"{{Gained}}","revision":1,"triggers":[{"id":"gained","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary-cao-pi.fangzhu.draw"],"usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"gained","options":[{"id":"continue"}]},{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"recover","target":"selectedTarget","amount":1}]}]}
            ]}
            """;
            var names = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Skip, Driver }) names[id] = new { name = id, description = "固定合法原生阶段及死亡/伤害/判定流程。" };
            foreach (var id in new[] { Paid, Gained }) names[id] = new { name = id, description = "原生付款或摸牌后的真实子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Wei, "曹丕魏同桌", "", Quiet, "wei", 8, [Skip, Driver])); b.AddGeneral(new(Shu, "曹丕蜀同桌", "", Quiet, "shu", 8, [Skip]));
            b.AddGeneral(new(Judger, "曹丕真实魏判定者", "", Quiet, "wei", 8, [Driver]));
            const string deck = "fixture:boundary-cao-pi-deck";
            b.AddDeck(new(deck, "曹丕固定黑色判定库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界曹丕真实结算", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Wei, Shu, Judger], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
