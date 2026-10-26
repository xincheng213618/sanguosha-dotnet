using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileCaiWenJiChecks
{
    private const string Hero = "mobile:cai-wen-ji", Beige = "mobile:beige", Reason = "skill.mobile.beige",
        Peer = "fixture:ordinary-mobile-cwj-peer", Quiet = "fixture:ordinary-mobile-cwj-quiet", Final = "fixture:ordinary-mobile-cwj-final",
        Payment = "skill-program.mobile:beige.SelectAndMoveOwnedCard";

    public static void EmptyHandEquipmentPaymentJudgmentReturnsCold()
    {
        var s = Scene.Start(); s.Play(); var equipment = s.V(0).Equipment.Single().Id;
        s.Native(s.V(0).Hand.Single(c => c.Kind == CardKind.Alcohol).Id, 0); s.Play();
        var slash = s.V(0).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        s.Native(slash, 3);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Beige && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        s.Need(s.V(0).HandCount == 0 && s.V(3).Hp == 6 && s.Zone(equipment) == CardLocation.Equipment(0) &&
            s.Facts<DamageAppliedEvent>().Any(e => e.SourceSeat == 0 && e.TargetSeat == 3 && e.Amount == 2) &&
            !s.Facts<JudgmentRequestedEvent>().Any(e => e.Reason == Reason),
            "A real boosted Slash leaves Cai Wen Ji empty-handed; her remaining equipment still qualifies for optional ordinary mobile Beige before any payment or judgment.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(c => c.Cards.SequenceEqual([equipment])) == true);
        s.Answer(c => c.Cards.SequenceEqual([equipment]));
        s.Reach(() => s.P?.Choices.Any(c => c.Targets.SequenceEqual([3])) == true);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Beige);
        var boundCost = root.CardSetBindings.Single(b => b.Name == "beige-cost");
        var paid = s.G.CardMovements.Single(m => m.CardId == equipment && m.From == CardLocation.Equipment(0) && m.To == CardLocation.DiscardPile);
        s.Need(paid.Reason.Value == Payment && !s.Facts<JudgmentRequestedEvent>().Any(e => e.Reason == Reason) &&
            root.WindowContext is { SourceSeat: 0, TargetSeat: 3, Amount: 2 } &&
            s.V(0).HandCount == 0 && s.V(0).Equipment.Count == 0 && boundCost.CardIds.SequenceEqual([equipment]) &&
            boundCost.SourceLocations.SequenceEqual([CardLocation.DiscardPile]) &&
            s.Facts<CardMovedEvent>().Count(e => e.CardId == equipment && e.From == paid.From && e.To == paid.To && e.Reason == paid.Reason) == 1,
            "The real equipment cost is one logical discard, paid before the original injured target can judge; no hand card or synthetic equipment loss substitutes for it.");
        s.Answer(c => c.Targets.SequenceEqual([3]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Final);
        var judgment = s.G.ResolutionStack.OfType<JudgmentFrame>().Single(j => j.Reason == Reason);
        s.Need(judgment.ParentFrameId == root.Id && judgment.TargetSeat == 3 && judgment.SourceSeat == 0 && judgment.Suit == Suit.Heart &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Beige).Id == root.Id && s.V(3).Hp == 6,
            "The native finalized-judgment child retains the exact paid damage parent, injured target, source and physical heart result before any recovery.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        var result = s.Facts<JudgmentResolvedEvent>().Single(e => e.Reason == Reason);
        s.Need(result.ParentResolutionId == root.Id && result.ResolutionId == judgment.Id && result.TargetSeat == 3 && result.Suit == Suit.Heart &&
            result.CardId is { } judgedCard && s.Zone(judgedCard) == CardLocation.DiscardPile && s.V(3).Hp == 7 &&
            s.Facts<ProgramRecoveryAppliedEvent>().Where(e => e.ProgramFrameId == root.Id).ToArray() is
                [{ Receipt: { RequestedAmount: 1, SourceSeat: 0, TargetSeat: 3, HpBefore: 6, HpAfter: 7 } }] &&
            s.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Beige && e.Activated && e.Completed) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Equipment(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == Payment) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1,
            "Cold returns the same real judgment exactly once: ordinary Beige recovers one after two damage, cleans the actual judgment entity, and repeats neither equipment payment nor physical Slash use.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Native(int id, int target)
        {
            var a = G.GetLegalActions(0).First(a => a.CardId == id && (a.TargetSeats.SequenceEqual([target]) ||
                target == 0 && a.Kind == LegalActionKind.Alcohol && a.TargetSeats.Count == 0));
            Accept(new PlayCardCommand(0, id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)
                { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> select)
        { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(select).Id, G.Revision)); }
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 140; step++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "Unexpected human boundary in the fixed ordinary mobile Beige scene.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Ordinary mobile Beige did not reach its next native boundary.");
        }
        public void Cold()
        {
            string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
                Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray() });
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves all views, the paid original frame, native finalized judgment, event facts and physical entities.");
        }
        public void Need(bool condition, string why)
        { if (!condition) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.Phase,
            Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId, Choices = p.Choices.Count } : null,
            HandCounts = Enumerable.Range(0, 4).Select(seat => V(seat).HandCount), Frames = G.ResolutionStack.Select(f => f.Kind) })); }
        private void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected ordinary mobile Beige command."); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "ordinary-mobile-cai-wen-ji", Hero, 8, 5, 0, 0, 0, [Quiet, Final],
                [new(CardKind.Alcohol, Suit.Heart, 4), new(CardKind.Slash, Suit.Club, 6)], [new(CardKind.Crossbow, Suit.Club, 5)],
                [new(Peer, Hp: 8) { StartingHand = [] }, new(Peer, Hp: 8) { StartingHand = [] }],
                [new(Peer, Hp: 8) { StartingHand = [] }], new string('e', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                AdvanceAfterHumanCommands = false, UseInteractiveSetup = false, UseInteractiveDiscard = false, MaxTurns = 4 }, r);
            var scene = new Scene(g, r); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:ordinary-mobile-cai-wen-ji", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "classic:cai-wen-ji");
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                 {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,
                  "effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                 {"id":"{{Final}}","revision":1,"triggers":[{"id":"real-finalized-child","window":"judgmentFinalized","subject":"any","judgmentReasons":["{{Reason}}"],
                  "suits":["heart"],"excludedReasons":[],"minimumRank":1,"maximumRank":13,"optional":false,"isSkillInvocation":false,
                  "effects":[{"op":"chooseOption","target":"owner","resultBind":"returned","options":[{"id":"continue"}]}]}]}
                ]}
                """;
            var c = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Quiet] = new { name = "安静摸牌", description = "固定真实手牌。" },
                    [Final] = new { name = "原生判定子返回", description = "付款后的真实判定终值。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description)
                { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Peer, "普通悲歌安静同桌", "supporter", Quiet, "wei", 8));
            const string deck = "fixture:ordinary-mobile-cwj-deck";
            b.AddDeck(new(deck, "固定红桃判定", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 7)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "普通移动蔡文姬真实装备付款与判定", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
