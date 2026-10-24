using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryZhangNiMobileChecks
{
    private const string Hero = "boundary:zhang-ni-mobile", Skill = "boundary:wurong-mobile", Peer = "fixture:boundary-zhang-ni-peer",
        Quiet = "fixture:boundary-zhang-ni-quiet", Skip = "fixture:boundary-zhang-ni-skip", Hold = "fixture:boundary-zhang-ni-paid", Drop = "fixture:boundary-zhang-ni-drop";
    public static void FourPrivateStrategyConsequences()
    {
        foreach (var suppress in new[] { true, false }) foreach (var resist in new[] { true, false })
        {
            var s = Scene.Start(); s.Play(); var ownerHp = s.V().Hp; var targetHp = s.V(1).Hp; var ownerHand = s.V().Hand.Count; var targetHand = s.V(1).Hand.Count;
            s.Begin(); s.Strategy(suppress ? "suppress" : "appease");
            s.Need(s.P?.PlayerSeat == 1 && s.Facts<BoundaryWurongCommittedEvent>() is [{ First: true }] && s.Facts<BoundaryWurongRevealedEvent>().Length == 0,
                "The second strategy participant receives the same private pair after only a public commitment, before either result is revealed.");
            s.Strategy(resist ? "resist" : "submit"); s.Settle();
            var reveal = s.Facts<BoundaryWurongRevealedEvent>();
            s.Need(reveal is [var outcome] && outcome.Suppress == suppress && outcome.Resist == resist &&
                !s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Skill), "The original Play resolves exactly one four-way result and spends the per-phase activation.");
            if (resist)
            {
                var damage = s.Facts<DamageAppliedEvent>();
                s.Need(s.V().Hand.Count == ownerHand + 1 && damage is [var applied] && applied.Amount == 1 &&
                    (suppress ? applied.SourceSeat == 0 && applied.TargetSeat == 1 && !applied.SourceLess && s.V(1).Hp == targetHp - 1 && s.V().Hp == ownerHp :
                        applied.TargetSeat == 0 && applied.SourceLess && s.V().Hp == ownerHp - 1 && s.V(1).Hp == targetHp),
                    "Each resistance result follows its real native damage before one draw; appeasement damage has no invented opponent source.");
            }
            else
            {
                var gift = s.Facts<BoundaryWurongGiftPaidEvent>();
                s.Need(gift is [{ Count: 2 }] && gift[0].Giver == (suppress ? 0 : 1) && gift[0].Recipient == (suppress ? 1 : 0) &&
                    s.V().Hand.Count == ownerHand + (suppress ? -1 : 2) && s.V(1).Hand.Count == targetHand + (suppress ? 1 : -2) &&
                    s.Facts<DamageAppliedEvent>().Length == 0 && s.Facts<BoundaryWurongSkipIssuedEvent>().Length == 0,
                    "Suppression first obtains one then returns two; full appeasement submission transfers two in the opposite direction without damage or a phase debt.");
            }
        }
    }
    public static void WholeHeGiftColdAndIssuedNextDrawSkip()
    {
        var s = Scene.Start(); s.Play(); s.Begin(); s.Strategy("suppress"); s.Cold(); s.Strategy("submit"); s.TakeOne(); s.Gift();
        var root = s.Root!.Id; var armor = s.V().Equipment.Single(c => c.Kind == CardKind.SilverLion).Id; var hand = s.V().Hand.First(c => c.Kind == CardKind.Peach).Id;
        s.Strategy("card", armor); s.Cold();
        s.Need(s.Zone(armor) == CardLocation.Equipment(0) && s.Zone(hand) == CardLocation.Hand(0) && s.Facts<BoundaryWurongGiftPaidEvent>().Length == 0,
            "Selecting one real HE entity remains an unpaid draft across cold restoration.");
        s.Strategy("card", hand); s.Strategy("pay"); s.Held();
        s.Need(s.Root?.Id == root && s.Root.BoundaryWurong is { Stage: BoundaryWurongStage.GiftChildren, Required: 2 } &&
            s.Zone(armor) == CardLocation.Hand(1) && s.Zone(hand) == CardLocation.Hand(1) && s.V().Hp == 7 &&
            s.Facts<BoundaryWurongGiftPaidEvent>() is [{ Count: 2 }] && s.Facts<BoundaryWurongGiftEntityEvent>().Length == 2,
            "The original frame pays both exact HE entities together, including native Silver Lion recovery, before its movement observer returns.");
        s.Cold(); s.Release(); s.Settle();
        s.Need(s.Facts<BoundaryWurongGiftPaidEvent>().Length == 1 && s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.boundary-zhang-ni.wurong.gift") == 2,
            "The paid child's cold return never recommits its strategy, transfers again or recovers a second time.");

        s = Scene.Start(targetCards: 1); s.Play(); var retained = s.V(1).Hand.Single().Id; s.Begin(); s.Strategy("appease"); s.Strategy("submit"); s.Play();
        s.Need(s.Facts<BoundaryWurongSkipIssuedEvent>() is [{ Target: 1 }] && s.Facts<BoundaryWurongGiftPaidEvent>().Length == 0 && s.Zone(retained) == CardLocation.Hand(1),
            "An insufficient submission replaces the whole transfer; its one owned card stays in place and one next-Draw obligation is issued.");
        s.LoseSkill(); s.Play(); s.Need(s.Facts<ProgramOwnerSkillsReplacedEvent>().Any(e => e.OwnerSeat == 0 && e.LostSkillIds.Contains(Skill)), "The original skill is genuinely replaced after its obligation was issued.");
        s.Cold(); s.End(); s.Reach(() => s.Facts<DrawPhaseSkippedEvent>().Any(e => e.OwnerSeat == 1));
        s.Need(s.Facts<BoundaryWurongSkipConsumedEvent>() is [{ Target: 1 }] && s.Facts<DrawPhaseSkippedEvent>().Count(e => e.OwnerSeat == 1) == 1,
            "Only the actual next Draw boundary consumes the issued obligation, even after skill loss; the native phase emits its true skipped fact.");
        s.Cold(); s.Play(); s.End(); s.Play();
        s.Need(s.Facts<BoundaryWurongSkipConsumedEvent>().Length == 1 && s.Facts<DrawPhaseSkippedEvent>().Count(e => e.OwnerSeat == 1) == 1,
            "A later actual turn no longer skips that role's Draw and cannot consume or replay the same obligation twice.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame? Root => G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill && f.TriggerId is null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Begin() => Accept(new UseProgramSkillCommand(0, Skill, "private-mouyi", [], [1], G.Revision, P!.PromptId));
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void LoseSkill() => Accept(new UseProgramSkillCommand(0, Drop, "lose", [], [], G.Revision, P!.PromptId));
        public void Strategy(string branch, int? card = null) => Answer(c => c.Parameters.GetValueOrDefault("branch") == branch && (card is null || c.Cards.SequenceEqual([card.Value])));
        public void Gift() => Reach(() => Root?.BoundaryWurong?.Stage == BoundaryWurongStage.GiftChoice && P?.SkillPrompt?.SkillId == Skill);
        public void Held() => Reach(() => P?.SkillPrompt?.SkillId == Hold);
        public void Release() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void TakeOne()
        {
            Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"));
            var id = P!.Choices.First(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card").Id;
            Answer(c => c.Id == id);
        }
        public void Settle()
        {
            for (var i = 0; i < 200; i++)
            {
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }) return;
                if (P?.SkillPrompt?.SkillId == Hold) { Release(); continue; }
                if (P?.SkillPrompt?.SkillId == Skill)
                {
                    var choice = P.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pay") ??
                        P.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") ??
                        P.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "card");
                    Need(choice is not null, "A native strategy consequence retains its actual card/payment choice."); Answer(c => c.Id == choice!.Id); continue;
                }
                Need(P is null, "Unexpected decision while settling native Wurong consequences."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "Wurong consequences missed their bounded native Play return.");
        }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; var c = p.Choices.Single(predicate); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c.Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 300; i++) { if (done()) return; Need(P is null, "Wurong missed a native boundary and reached an unexpected private prompt."); Accept(new AdvanceOneStepCommand(G.Revision)); }
            Need(false, "Wurong scene exceeded its fixed native phase bound.");
        }
        public void Cold()
        {
            var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "Cold preserves the exact original private strategies, staged HE batch, movement child and persistent phase debt.");
        }
        private string State() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements });
        private void Accept(GameCommand c) { var result = G.Submit(c); Need(result.Accepted, result.Error?.Message ?? "Rejected native Wurong command."); }
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, P, Root = Root?.BoundaryWurong,
            Issued = Facts<BoundaryWurongSkipIssuedEvent>(), Consumed = Facts<BoundaryWurongSkipConsumedEvent>(), Substituted = Facts<ProgramTurnPhaseSubstitutedEvent>(), Last = G.Events.TakeLast(10).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) })); }
        public static Scene Start(int targetCards = 2)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] own = [new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Peach, Suit.Heart, 3), new(CardKind.Peach, Suit.Heart, 4)];
            var setup = new ShanheBattleSetup(29, 1, "boundary-zhang-ni", Hero, 6, 4, 0, 0, 0, [Quiet, Hold, Drop], own, [new(CardKind.SilverLion, Suit.Club, 1)],
                [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }],
                [new(Peer, Hp: 8) { StartingHand = Enumerable.Range(0, targetCards).Select(i => new ShanheStartingCard(CardKind.Dodge, Suit.Club, i + 5)).ToArray() }], new string('d', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, r);
            var s = new Scene(g, r); s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-zhang-ni-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            b.AddCard(new("fixture:boundary-zhang-ni-silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
             {"id":"{{Quiet}}","revision":1,"triggers":[
              {"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
              {"id":"observe-real-draw-skip","window":"drawPhaseSkipped","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"setChainedState","target":"owner","chained":false}]}]},
             {"id":"{{Skip}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
             {"id":"{{Hold}}","revision":1,"triggers":[{"id":"paid","window":"cardsMoved","subject":"owner","sourceZones":["hand","equipment"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill-program.boundary-zhang-ni.wurong.gift"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
             {"id":"{{Drop}}","revision":1,"activations":[{"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["{{Skill}}"],"sourceBind":"{{Quiet}}"}]}]}
            ]}
            """;
            var names = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Skip, Drop }) names[id] = new { name = id, description = "固定真实阶段和原生技能替换。" };
            names[Hold] = new { name = Hold, description = "两张原生HE赠牌后的真实子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Peer, "怃戎固定同桌", "", Quiet, "wei", 8, [Skip]));
            const string deck = "fixture:boundary-zhang-ni-deck";
            b.AddDeck(new(deck, "怃戎固定库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "界张嶷真实谋弈", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
