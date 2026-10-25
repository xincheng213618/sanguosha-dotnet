using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryXiahouDunMobileChecks
{
    private const string Hero = "boundary:xiahou-dun-mobile", Skill = "boundary:qingjian-mobile", Owner = "fixture:mobile-xiahou-dun-owner", Peer = "fixture:mobile-xiahou-dun-peer";
    private const string Driver = "fixture:mobile-xiahou-dun-driver", Gate = "fixture:mobile-xiahou-dun-gate", Watch = "fixture:mobile-xiahou-dun-watch", Deck = "fixture:mobile-xiahou-dun-deck";

    public static void OldHandStorageDefersAllGiftsAndRewardsActualCountCold()
    {
        var s = Scene.Start(); var oldHand = s.V(0).Hand.Select(c => c.Id).ToArray();
        s.Accept(new UseProgramSkillCommand(0, Driver, "gain-one", [], [], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        var gained = s.V(0).Hand.Single(c => !oldHand.Contains(c.Id)).Id;
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        foreach (var id in oldHand) s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "toggle" && c.Cards.SequenceEqual([id]));
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "store");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue-store"));
        var deposit = s.E<BoundaryXiahouDunStoredEvent>().Single();
        Need(oldHand.Length == 2 && deposit.Count == 2 && s.V(0).Hand.Select(c => c.Id).SequenceEqual([gained]) &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == deposit.FrameId).BoundaryXiahouDun is { Stage: BoundaryXiahouDunStage.StoreChildren, Payment.Count: 2 } &&
            oldHand.All(id => s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location == deposit.Location) &&
            s.V(0).Hp == 3 && s.V(0).Skills?.Any(k => k.ContentId == Skill) != true,
            "The actual gained card remains in hand while two older hand entities are paid into a private general pile; a genuine storage observer removes the skill source after payment.");
        var ownPile = s.V(0).PrivateWhirlPiles!.Single(p => p.SkillId == Skill);
        var foreignPile = s.G.CreateSnapshot(2).Players.Single(p => p.Seat == 0).PrivateWhirlPiles!.Single(p => p.SkillId == Skill);
        Need(ownPile.Count == 2 && ownPile.Cards!.Select(c => c.Id).Order().SequenceEqual(oldHand.Order()) && foreignPile.Count == 2 && foreignPile.Cards is null,
            "The persistent pile exposes its count and only its actual owner sees the concealed entities.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue-store"); s.Play();
        Need(s.E<BoundaryXiahouDunGivenEvent>().Count() == 0 && s.E<BoundaryXiahouDunRewardEvent>().Count() == 0 &&
            oldHand.All(id => s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location == deposit.Location),
            "Finishing the gain and payment child does not distribute early or award a storage bonus.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "give"));
        var distribution = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.BoundaryXiahouDun is { Stage: BoundaryXiahouDunStage.Distributing });
        Need(s.G.ResolutionStack.OfType<TurnEndingBoundaryFrame>().Any(f => f.OwnerSeat == 0) && !s.E<TurnEndedEvent>().Any() &&
            distribution.SkillInstanceId == deposit.Source.SkillInstanceId && distribution.BoundaryXiahouDun!.Deposits.SequenceEqual([deposit.FrameId]),
            "The next actual ending phase issues the original paid private debt even after losing its skill, before the actual TurnEnded event.");
        s.Answer(c => c.Cards.SequenceEqual([oldHand[0]]) && c.Targets.SequenceEqual([1]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue-give"));
        Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == distribution.Id).BoundaryXiahouDun is { Stage: BoundaryXiahouDunStage.GiveChildren, Given: 1 } &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(f => f.Batch.ParentFrameId == distribution.Id) &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == oldHand[0]).Location == CardLocation.Hand(1) &&
            s.G.CreateCardZoneDiagnostics().Single(c => c.CardId == oldHand[1]).Location == deposit.Location && !s.E<BoundaryXiahouDunRewardEvent>().Any(),
            "The first real gift interrupts on its exact original movement child; the other card remains held and one gift earns no reward.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue-give");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "give"));
        s.Answer(c => c.Cards.SequenceEqual([oldHand[1]]) && c.Targets.SequenceEqual([2]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue-give"));
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue-give");
        s.Reach(() => s.E<BoundaryXiahouDunRewardEvent>().Any() && s.G.ResolutionStack.All(f => f.Id != distribution.Id));
        Need(s.E<BoundaryXiahouDunStoredEvent>().Count() == 1 && s.E<BoundaryXiahouDunGivenEvent>().Count() == 2 &&
            s.E<BoundaryXiahouDunRewardEvent>().Single() is { Given: 2, Actual: 1, Owner: 0 } && s.V(0).HandCount == 2 &&
            oldHand.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == deposit.Location) == 1 &&
                s.G.CardMovements.Count(m => m.CardId == id && m.From == deposit.Location && m.To.Zone == CardZoneKind.Hand && m.To.OwnerSeat != 0) == 1) &&
            s.G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == "skill.boundary-xiahou-dun.qingjian.reward") == 1 &&
            s.V(0).PrivateWhirlPiles?.Any(p => p.SkillId == Skill) != true,
            "Two actually delivered original entities empty the pile and draw exactly one native reward; cold return neither repays storage, repeats a gift nor rewards twice.");
        s.Cold();
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual mobile Xiahou Dun command."); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 180; step++)
            {
                if (goal()) return;
                if (P is { PlayerSeat: 0 } p) throw new InvalidOperationException("Unexpected mobile Xiahou Dun prompt: " + JsonSerializer.Serialize(new { p.Kind, Skill = p.SkillPrompt?.SkillId }));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded actual mobile Qingjian boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold retains the private store, lost source, exact movement children and once-only distributed reward."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture-mobile-xiahou-dun-qingjian", Owner, 4, 0, 0, 0, 0, [],
                [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Club, 3)], [], [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
                [new(Peer) { StartingHand = [] }], new string('a', 64));
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r)); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-xiahou-dun-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:xiahou-dun");
            object Choice(string id) => new { op = "chooseOption", target = "owner", resultBind = id, options = new[] { new { id } } };
            var skills = new object[] {
                new { id = Driver, revision = 1, activations = new[] { new { id = "gain-one", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                    targetKind = "otherLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "draw", target = "owner", amount = 1 } } } } },
                new { id = Gate, revision = 1, hpSkillGrants = new[] { new { id = "four-hp-qingjian", skillId = Skill, minimumHp = 4 } } },
                new { id = Watch, revision = 1, triggers = new object[] {
                    new { id = "store-paid", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" },
                        movementOccurrence = "perBatch", movementReasons = new[] { "skill.boundary-xiahou-dun.qingjian.store" }, optional = false,
                        effects = new object[] { new { op = "loseHp", target = "owner", amount = 1 }, Choice("continue-store") } },
                    new { id = "gift-paid", window = "cardsMoved", subject = "owner", sourceZones = new[] { "privatePersistentPile" },
                        movementOccurrence = "perBatch", movementReasons = new[] { "skill.boundary-xiahou-dun.qingjian.give" }, optional = false, effects = new[] { Choice("continue-give") } }
                } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实获得牌", description = "原生摸一张" }, [Gate] = new { name = "真实体力来源", description = "四体力授予移动清俭" },
                [Watch] = new { name = "实际扣置与给牌孩子", description = "真实付款后暂停", optionLabels = new Dictionary<string, string> { ["continue-store"] = "继续扣置返回", ["continue-give"] = "继续给牌返回" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动清俭真实拥有者", "", Gate, "wei", 4, [Driver, Watch])); b.AddGeneral(new(Peer, "移动清俭真实其他角色", "", "standard:none", "wu", 4));
            b.AddDeck(new(Deck, "固定真实实体", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动清俭原生流程", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
