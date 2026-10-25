using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryXiaoQiaoMobileChecks
{
    private const string Hero = "boundary:xiao-qiao-mobile", Driver = "fixture:mobile-xiao-qiao-driver", Watch = "fixture:mobile-xiao-qiao-watch", Peer = "fixture:mobile-xiao-qiao-peer", Deck = "fixture:mobile-xiao-qiao-deck";
    public static void OtherHeartJudgmentAndHandOnlyTianxiangPaidCold()
    {
        var s = Scene.Start(); s.Play();
        var equip = s.G.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Equip);
        s.Accept(new PlayCardCommand(0, equip.CardId!.Value, equip.TargetSeats, s.G.Revision, s.P!.PromptId, equip.PlayedCardKind)); s.Play(); var equipment = equip.CardId.Value;
        s.Use("judge", [1]); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "boundary-xiao-qiao-judgment") == true);
        var judgment = s.G.ResolutionStack.OfType<JudgmentFrame>().Single(); var physical = judgment.CardId!.Value;
        s.Need(judgment.TargetSeat == 1 && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Last().WindowContext?.Judgment?.Suit == Suit.Heart,
            "Mobile Hongyan observes another actual character's heart judgment before the native result takes effect.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("suit") == "Club"); s.Play();
        s.Need(s.E<BoundaryXiaoQiaoJudgmentRewrittenEvent>().Single().Receipt is { Suit: Suit.Club, CardId: var id } && id == physical &&
            s.E<JudgmentResolvedEvent>().Any(e => e.ResolutionId == judgment.Id && e.CardId == physical && e.Suit == Suit.Club) &&
            s.G.CardMovements.Count(m => m.CardId == physical && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "Cold changes only the same original judgment's effective suit, then the native judgment and its original cleanup finish once.");
        var cost = s.V(0).Hand.Single(c => c.Suit == Suit.Spade).Id; var hp = s.V(0).Hp; var recipientHp = s.V(1).Hp;
        s.Use("incoming", [2]); s.Reach(() => s.P?.SkillPrompt?.SkillId == "boundary:tianxiang-mobile" && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "suit-prevention-benefit") == true);
        s.Need(s.P!.Choices.Any(c => c.Cards.SequenceEqual([cost])) && s.P.Choices.All(c => !c.Cards.Contains(equipment)),
            "Spade hand material is effectively Heart, while the real equipped spade is excluded from mobile Tianxiang payment.");
        s.Answer(c => c.Cards.SequenceEqual([cost])); s.Answer(c => c.Targets.SequenceEqual([1]) && c.Parameters.GetValueOrDefault("branch") == "lose-hp-gift");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var payment = s.E<ProgramSuitPreventionPaymentEvent>().Single();
        s.Need(payment.CostCardId == cost && payment.EffectiveSuit == Suit.Heart && s.V(0).Hp == hp && s.V(1).Hp == recipientHp,
            "The native heart-hand discard already prevents the original damage and awaits its real movement child before issuing recipient HP loss or gift.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(s.V(0).Hp == hp && s.V(1).Hp == recipientHp - 1 && s.V(1).Hand.Any(c => c.Id == cost) &&
            s.E<ProgramSuitPreventionPaymentEvent>().Count() == 1 && !s.E<DamageAppliedEvent>().Any(e => e.TargetSeat == 0) &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) == 1,
            "The original paid child returns after cold exactly once: one HP loss, the exact discarded entity gifted, and no original incoming damage or repeated payment.");
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game; public PendingDecision? P => G.PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Mobile Xiao Qiao command rejected."); }
        public void Answer(Func<PromptChoice, bool> test) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(test).Id, G.Revision)); }
        public void Use(string id, IReadOnlyList<int> seats) => Accept(new UseProgramSkillCommand(0, Driver, id, [], seats, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> done) { for (var n = 0; n < 140 && !done(); n++) { Need(G.State.Winner == Winner.None && P is null, "Unexpected mobile Xiao Qiao boundary."); Accept(new AdvanceOneStepCommand(G.Revision)); } Need(done(), "Missing mobile Xiao Qiao native boundary."); }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Xiao Qiao cold preserves native judgment and paid damage frames plus all four private views."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, Skill = p.SkillPrompt?.SkillId } : null, V(0).Hp, V(0).HandCount })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(107, 1, "mobile-xiao-qiao", Hero, 3, 0, 0, 0, 0, [Driver, Watch],
                [new(CardKind.Dodge, Suit.Spade, 1), new(CardKind.BaguaFormation, Suit.Spade, 2), new(CardKind.Dodge, Suit.Heart, 3)], [],
                Enumerable.Range(0, 3).Select(_ => new ShanheCombatantDefinition(Peer) { StartingHand = [] }).ToArray(), [], new string('b', 64));
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, r), r);
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-xiao-qiao", new(1, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new object[] {
                    new { id = "judge", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new object[] { new { op = "startJudgment", target = "selectedTarget", judgmentReason = "fixture.mobile-hongyan", resultBind = "original-judgment", visibility = "public" }, new { op = "moveBoundCards", target = "owner", sourceBind = "original-judgment", destination = "discardPile" } } },
                    new { id = "incoming", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                        effects = new object[] { new { op = "damage", target = "owner", sourceRef = new { kind = "selectedTarget" }, amount = 2 } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "real-heart-hand-cost", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.boundary:tianxiang-mobile.DiscardSuitPreventDamageAndBenefit" }, optional = false, usageScope = "game", usageLimit = 1,
                    effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } } } } } } };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实判定与伤害", description = "使用原生判定和有来源伤害。" }, [Watch] = new { name = "真实天香付款孩子", description = "原红桃手牌弃置后的实际移动窗口。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Peer, "真实判定受益人", "supporter", "standard:none", "wei", 5));
            b.AddDeck(new(Deck, "固定红桃判定", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动小乔真实边界", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
