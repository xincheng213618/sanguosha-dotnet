using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryDiaoChanMobileChecks
{
    private const string Hero = "boundary:diao-chan-mobile", Skill = "boundary:lijian-mobile", Binding = "discard-and-issue-ordered-duel";
    private const string Owner = "fixture:mobile-diao-chan-owner", Actor = "fixture:mobile-diao-chan-actor", Target = "fixture:mobile-diao-chan-target";
    private const string Gate = "fixture:mobile-diao-chan-gate", Watch = "fixture:mobile-diao-chan-discard-watch", Deck = "fixture:mobile-diao-chan-deck";

    public static void PaidSourceLossReturnsThroughRealNullificationCold()
    {
        var s = Scene.Start();
        var cost = s.V(0).Hand.Single(card => card.Kind == CardKind.Dodge).Id;
        var counter = s.V(0).Hand.Single(card => card.Kind == CardKind.Nullification).Id;
        s.Accept(new UseProgramSkillCommand(0, Skill, Binding, [cost], [1, 2], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        var paid = s.E<OrderedSelectedActorDuelPaidEvent>().Single().Payment;
        Need(paid.CardId == cost && paid.OwnerSeat == 0 && paid.ActorSeat == 1 && paid.TargetSeat == 2 &&
            paid.From == CardLocation.Hand(0) && paid.To == CardLocation.DiscardPile &&
            !s.E<CardUseDeclaredEvent>().Any(e => e.CardKind == CardKind.Duel),
            "The actual HE discard is paid on its original program before the discard observer or independent Duel.");
        s.AssertCost(cost); s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.V(0).Hp == 2 && s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        Need(s.V(0).Skills?.Any(skill => skill.ContentId == Skill) != true &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid.ProgramFrameId && f.OrderedSelectedActorDuel?.Payment == paid),
            "Actual HP loss removes the granted source while the paid original parent retains its exact discard receipt.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P is { Kind: DecisionKind.Nullification, PlayerSeat: 0 });
        var duel = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.SelectedActorDuelOrigin?.OrderedPairPayment is not null);
        Need(duel.SourceSeat == 1 && duel.TargetSeats.SequenceEqual([2]) && duel.CardId == 0 && duel.PhysicalCardIds?.Count == 0 &&
            duel.Action is { Type: CardActionType.Use, ActorSeat: 1, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0 } &&
            duel.SelectedActorDuelOrigin is { OwnerSeat: 0, InitialActorSeat: 1, OrderedPairPayment: var original } && original == paid &&
            s.P!.Choices.Any(c => c.Cards.SequenceEqual([counter])),
            "The selected male genuinely uses a zero-material native Duel after source loss, and the owner can pay its real Nullification.");
        s.Cold(); s.Answer(c => c.Cards.SequenceEqual([counter]));
        s.Reach(() => !s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == paid.ProgramFrameId) &&
            !s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Id == duel.Id));
        Need(s.E<NullificationRespondedEvent>().Count(e => e.ResolutionId == duel.Id && e.ResponderSeat == 0 && e.NullificationCardId == counter && e.EffectNullified) == 1 &&
            s.E<NullificationResolvedEvent>().Count(e => e.ResolutionId == duel.Id && e.EffectNullified) == 1 &&
            !s.E<DuelResponseEvent>().Any(e => e.ResolutionId == duel.Id) && !s.E<DamageAppliedEvent>().Any() &&
            s.V(1).Hp == 4 && s.V(2).Hp == 4,
            "An actual physical Nullification cancels the native Duel before either Slash response or damage.");
        Need(s.E<OrderedSelectedActorDuelPaidEvent>().Count() == 1 && s.E<OrderedSelectedActorDuelIssuedEvent>().Count() == 1 &&
            s.E<CardUseDeclaredEvent>().Count(e => e.ResolutionId == duel.Id && e.SourceSeat == 1 && e.CardKind == CardKind.Duel) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == duel.Id) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == counter && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == counter && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "The paid outer instruction, real response entity and native child complete once across both restored child boundaries.");
        s.AssertCost(cost); s.Cold();
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual mobile Diao Chan command."); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } p) throw new InvalidOperationException("Unexpected mobile Diao Chan prompt: " + JsonSerializer.Serialize(new { p.Kind, Skill = p.SkillPrompt?.SkillId }));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded mobile Diao Chan native boundary.");
        }
        public void AssertCost(int id) => Need(G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) &&
            m.To == CardLocation.DiscardPile && m.Reason.Value == $"skill-program.{Skill}.{SkillProgramEffectOp.DiscardSelected}") == 1 &&
            G.CardMovements.All(m => m.CardId != id || m.To != CardLocation.Processing),
            "The original selected discard is paid once and never becomes Duel or response material.");
        public void Cold()
        {
            var before = Dump();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold retains every viewer, the original typed paid/child frames and actual physical movements.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture-mobile-diao-chan-nullification", Owner, 3, 0, 0, 0, 0, [],
                [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Nullification, Suit.Club, 11)], [],
                [new(Target) { StartingHand = [new(CardKind.Slash, Suit.Spade, 5)] }, new(Target) { StartingHand = [] }],
                [new(Actor) { StartingHand = [new(CardKind.Slash, Suit.Club, 4)] }], new string('a', 64));
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r));
            s.Accept(new StartGameCommand()); s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }); return s;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-diao-chan-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:diao-chan");
            var skills = new object[] {
                new { id = Gate, revision = 1, hpSkillGrants = new[] { new { id = "full-health-lijian", skillId = Skill, minimumHp = 3 } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "paid-original-discard", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" },
                    movementReasons = new[] { $"skill-program.{Skill}.{SkillProgramEffectOp.DiscardSelected}" }, movementOccurrence = "perBatch", optional = true,
                    effects = new object[] { new { op = "loseHp", target = "owner", amount = 1 },
                        new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object> { [Gate] = new { name = "真实体力来源", description = "三体力授予移动离间" },
                [Watch] = new { name = "实际弃牌子流程", description = "实际弃牌后失去原技能来源", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动离间原拥有者", "", Gate, "qun", 3, [Watch], GeneralGender.Female));
            b.AddGeneral(new(Actor, "移动离间真实男性使用者", "", "standard:none", "qun", 4, Gender: GeneralGender.Male));
            b.AddGeneral(new(Target, "移动离间真实男性目标", "", "standard:none", "wei", 4, Gender: GeneralGender.Male));
            b.AddDeck(new(Deck, "固定真实实体", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动离间真实原生决斗", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Actor, Target], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
