using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class MobileCaoPiChecks
{
    private const string Hero = "mobile:cao-pi", Xing = "mobile:xingshang", Driver = "fixture:mobile-cao-pi-driver",
        Quiet = "fixture:mobile-cao-pi-quiet", Skip = "fixture:mobile-cao-pi-skip", Peer = "fixture:mobile-cao-pi-peer",
        Watch = "fixture:mobile-cao-pi-claimed", ClaimReason = "skill-program.mobile:xingshang.claim-died-cards.claim-death-cleanup";

    public static void OnlyActualHeCorpseEntitiesAndPaidClaimCold()
    {
        var s = Scene.Start(onlyJudgment: true); s.Play();
        var delayed = s.V(0).Hand.Single(c => c.Kind == CardKind.Indulgence).Id;
        s.Native(delayed, 2); s.Play(); s.Need(s.Zone(delayed) == CardLocation.Judgment(2), "The empty victim genuinely owns only a native delayed-trick placement, not fabricated cleanup data.");
        s.Use("kill", 2); s.Play();
        s.Need(s.Zone(delayed) == CardLocation.DiscardPile && s.Facts<ProgramBindingStartedEvent>().All(e => e.SkillId != Xing) &&
            s.G.CardMovements.All(m => m.CardId != delayed || m.Reason.Value != ClaimReason),
            "A corpse containing only Judgment entities never offers or starts the explicitly HE-filtered mobile Xingshang.");

        s = Scene.Start(onlyJudgment: false); s.Play(); s.Use("equip", 2);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"); s.Play();
        var equipment = s.V(2).Equipment.Single().Id;
        delayed = s.V(0).Hand.Single(c => c.Kind == CardKind.Indulgence).Id; s.Native(delayed, 2); s.Play();
        var corpse = s.V(2).Hand.Select(c => c.Id).Append(equipment).ToArray();
        var before = s.V(0).Hand.Count;
        s.Need(corpse.Length == 3 && s.Zone(delayed) == CardLocation.Judgment(2), "The real victim has two physical hand cards, a genuinely equipped weapon, and a separate native Judgment entity.");
        s.Use("kill", 2); s.Reach(() => s.P?.SkillPrompt?.SkillId == Xing);
        var death = s.G.ResolutionStack.OfType<DeathFrame>().Single(d => d.VictimSeat == 2);
        s.Need(corpse.All(id => s.Zone(id) == CardLocation.DiscardPile) && s.Zone(delayed) == CardLocation.DiscardPile,
            "Xingshang is offered after its exact native death cleanup, while every original corpse entity remains in Discard.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var claimant = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Xing);
        var claimantId = claimant.Id;
        s.Need(claimant.PendingMovementContinuation is not null && claimant.InstructionIndex == 1 &&
            claimant.WindowContext is { Window: SkillProgramTriggerWindow.CharacterDied, TargetSeat: 2 } context &&
            s.G.ResolutionStack.OfType<ProgramKillTriggerWindowFrame>().Any(w => w.Id == context.ParentFrameId && w.DeathFrameId == death.Id) &&
            corpse.All(id => s.Zone(id) == CardLocation.Hand(0)) && s.V(0).Hand.Count == before + corpse.Length &&
            s.Zone(delayed) == CardLocation.DiscardPile && s.G.CardMovements.Count(m => m.Reason.Value == ClaimReason) == corpse.Length &&
            s.Facts<ProgramBindingResolvedEvent>().All(e => e.SkillId != Xing || !e.Completed),
            "The committed one-node claim takes exactly the original HE once, excludes Judgment, and awaits its actual gain child on the same death-owned frame.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
        s.Need(corpse.All(id => s.Zone(id) == CardLocation.Hand(0)) && s.Zone(delayed) == CardLocation.DiscardPile &&
            s.V(0).Hand.Count == before + corpse.Length && s.G.CardMovements.Count(m => m.Reason.Value == ClaimReason) == corpse.Length &&
            s.Facts<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Xing && e.Activated && e.Completed) == 1 &&
            s.G.ResolutionStack.All(f => f.Id != claimantId && f.Id != death.Id),
            "Cold drains the original paid claim child, completes that single Xingshang and native death, and neither obtains Judgment nor claims a second time.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string activation, int target) => Submit(new UseProgramSkillCommand(0, Driver, activation, [], [target], G.Revision, P!.PromptId));
        public void Native(int card, int target)
        {
            var action = G.GetHumanLegalActions().Single(a => a.CardId == card && a.TargetSeats.SequenceEqual([target]));
            Submit(new PlayCardCommand(0, card, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)
                { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!; var choice = p.Choices.Single(predicate);
            Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice.Id, G.Revision));
        }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 100; i++)
            {
                if (done()) return;
                Need(P is null, "The actual Cao Pi flow reached an unexpected native decision (including an ineligible empty-corpse offer).");
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The actual Cao Pi flow missed its named native boundary.");
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        {
            var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "Cold preserves the same original death, committed claimant and physical HE gain child for all four viewers.");
        }
        public void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Cao Pi native command rejected.");
        }
        public void Need(bool ok, string why)
        {
            if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new
            {
                G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, Pending = P?.Kind, Skill = P?.SkillPrompt?.SkillId,
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), Movements = G.CardMovements.TakeLast(8).ToArray()
            }));
        }
        public static Scene Start(bool onlyJudgment)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            ShanheStartingCard[] hand = onlyJudgment ? [new(CardKind.Indulgence, Suit.Heart, 4)] :
                [new(CardKind.Crossbow, Suit.Club, 1), new(CardKind.Indulgence, Suit.Heart, 4)];
            var setup = new ShanheBattleSetup(29, 1, "mobile-cao-pi", Hero, 7, 5, 0, 0, 0, [Quiet, Driver, Watch], hand, [],
                [new(Peer, Hp: 8) { StartingHand = onlyJudgment ? [] : [new(CardKind.Dodge, Suit.Club, 8), new(CardKind.Dodge, Suit.Club, 9)] },
                 new(Peer, Hp: 8) { StartingHand = [] }], [new(Peer, Hp: 8) { StartingHand = [] }], new string('e', 64));
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry);
            var scene = new Scene(registry, game); scene.Submit(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-cao-pi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "classic:cao-pi", "boundary:cao-pi-mobile");
            var rules = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new object[]
                {
                    new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false,
                        effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                    new { id = Skip, revision = 1, triggers = new[] { new { id = "skip", window = "afterNormalDraw", subject = "owner", optional = false, isSkillInvocation = false,
                        effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } },
                    new { id = Driver, revision = 1, activations = new object[]
                    {
                        new { id = "kill", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                            effects = new[] { new { op = "loseHp", target = "selectedTarget", amount = 20 } } },
                        new { id = "equip", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                            effects = new[] { new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "owner" },
                                targetRef = new { kind = "selectedTarget" }, zones = new[] { "hand" }, cardCategories = new[] { "equipment" }, count = 1, destination = "selectedTargetEquipment" } } }
                    } },
                    new { id = Watch, revision = 1, triggers = new[] { new { id = "claimed", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                        movementOccurrence = "perBatch", movementReasons = new[] { ClaimReason }, optional = false, isSkillInvocation = false,
                        effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "hold", options = new[] { new { id = "continue" } } } } } } }
                }
            });
            var names = new Dictionary<string, object>();
            foreach (var id in new[] { Quiet, Skip, Driver }) names[id] = new { name = id, description = "合法原生死亡与装备准备。" };
            names[Watch] = new { name = Watch, description = "原行殇领取子窗。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Peer, "曹丕实际死亡对象", "", Quiet, "shu", 8, [Skip]));
            const string deck = "fixture:mobile-cao-pi-deck";
            builder.AddDeck(new(deck, "固定原生判定及死亡库存", 0, 2, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "普通移动曹丕真实行殇", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
