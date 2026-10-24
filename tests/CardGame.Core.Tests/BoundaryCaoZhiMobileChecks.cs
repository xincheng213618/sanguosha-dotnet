using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.Tests;

internal static class BoundaryCaoZhiMobileChecks
{
    private const string Hero = "boundary:cao-zhi-mobile", Luoying = "boundary:luoying-mobile", Jiushi = "boundary:jiushi-mobile",
        Upgraded = "boundary:jiushi-mobile-upgraded", Chengzhang = "boundary:chengzhang-mobile", Peer = "fixture:mobile-cz-peer",
        Quiet = "fixture:mobile-cz-quiet", Driver = "fixture:mobile-cz-driver", Gain = "fixture:mobile-cz-gain";

    public static void ProvenanceClaimsBaseWineNativeDamageAndDyingFaceSnapshotCold()
    {
        var s = Scene.Start(); s.Play(); s.Wine(Jiushi); s.Play();
        s.Need(s.V().IsFaceDown && s.Facts<AlcoholAppliedEvent>().Length == 1 && s.Selections().Length == 0 &&
            s.Facts<CardUseFinishedEvent>().Any(e => e.CardKind == CardKind.Alcohol && e.CardId == 0),
            "Base Jiushi really pays its native face cost and uses a zero-material Wine; this initial flip earns no trick.");
        var club = s.V(3).Hand.Single().Id;
        s.Drive("discard-other", [3]); s.Offer(Luoying); s.Activate(); s.Gain(); s.Continue(); s.Play();
        var first = s.Facts<ProgramDiscardedEntityClaimedEvent>().Single();
        s.Need(first.CardId == club && first.Origin.SkillId == Luoying && s.Zone(club) == CardLocation.Hand(0) && s.V().IsFaceDown &&
            s.Facts<ProgramDiscardedEntityOriginEvent>().Any(e => e.MovementSequence == first.DiscardMovementSequence && e.SourceSeat == 3),
            "Mobile Luoying claims another role's real club discard with its native origin, without OL outside-claim face-up or claimed-card benefits.");
        s.Drive("judge", [3]); s.Offer(Luoying); s.Activate(); s.Gain(); s.Continue(); s.Play();
        var judged = s.Facts<JudgmentResolvedEvent>().Single(e => e.Reason == "fixture:mobile-cz-judge");
        s.Need(s.Facts<ProgramDiscardedEntityClaimedEvent>().Length == 2 && judged.CardId is { } judgedId && s.Zone(judgedId) == CardLocation.Hand(0) &&
            s.Facts<ProgramDiscardedEntityOriginEvent>().Any(e => e.JudgmentFrameId == judged.ResolutionId && e.SourceSeat == 3) && s.V().IsFaceDown,
            "The judged target, not the draw pile or the judging skill's owner, owns the final club judgment origin for Luoying.");
        s.Drive("duel", [3, 0]); s.Offer(Jiushi); s.Activate(); s.Gain();
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Jiushi && f.TriggerId is not null);
        var selected = s.Selections().Single();
        s.Need(!s.V().IsFaceDown && s.V().Hp == 7 && root.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied } context &&
            s.Facts<BoundaryCaoZhiDamageFaceEvent>().Any(e => e.DamageFrameId == context.DamageFrameId && e.OwnerSeat == 0 && e.WasFaceDown) &&
            selected.CardId is { } card && s.V().Hand.Single(c => c.Id == card).Kind == CardKind.Indulgence &&
            s.G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)) == 1,
            "Real native Duel damage sees the original back face, flips up and actually obtains a delayed trick from the random eligible deck pool.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.Selections().Length == 1 && selected.CardId is { } gained && s.G.CardMovements.Count(m => m.CardId == gained && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0)) == 1,
            "Cold returns through the original bound-card gain once without sampling the random pool or obtaining the same invoice twice.");
        s.Drive("hurt-seven", [3]); s.Reach(() => s.P?.SkillPrompt?.SkillId == Jiushi && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "flip"));
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "flip"); s.Play();
        s.Need(s.V().Hp == 1 && s.V().IsFaceDown && s.Facts<AlcoholAppliedEvent>().Length == 1 &&
            s.Facts<CardUseDeclaredEvent>().Count(e => e.CardKind == CardKind.Alcohol && e.CardId == 0) == 2 &&
            s.Facts<ProgramDyingRescueEvent>() is [{ SkillId: Jiushi, OwnerSeat: 0, VictimSeat: 0, CardId: 0, RecoveredHp: 1, VictimHp: 1 }] &&
            s.Selections().Length == 1 &&
            s.Facts<BoundaryCaoZhiDamageFaceEvent>().Last(e => e.OwnerSeat == 0).WasFaceDown == false,
            "A genuine lethal program-damage attempt began face up; self-dying Jiushi rescues with Wine and flips down, but that later cost cannot create a false after-damage flip or extra trick. " +
            JsonSerializer.Serialize(new { WineBonuses = s.Facts<AlcoholAppliedEvent>().Length,
                WineDeclarations = s.Facts<CardUseDeclaredEvent>().Count(e => e.CardKind == CardKind.Alcohol && e.CardId == 0),
                Rescues = s.Facts<ProgramDyingRescueEvent>(), TrickSelections = s.Selections().Length,
                OriginalFace = s.Facts<BoundaryCaoZhiDamageFaceEvent>().Last(e => e.OwnerSeat == 0) }));
    }

    public static void ChengzhangNativeUpgradeAndEveryTurnoverWineChildrenCold()
    {
        var s = Scene.Start(upgrade: true); s.Play(); s.Drive("hurt-four", [3]); s.Play(); s.Drive("hit-three", [3]); s.Play();
        s.Need(s.GameDamage() == 7 && s.V().Hp == 4 && s.Has(Jiushi) && !s.Has(Upgraded),
            "Actual caused and received damage values sum to seven, without upgrading during the current Play.");
        var before = s.V().Hand.Count; s.End(); s.Gain();
        var awakening = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Chengzhang);
        s.Need(awakening.InstructionIndex == 2 && s.G.State.CurrentSeat == 0 && s.V().Hp == 5 && s.V().Hand.Count == before + 1 &&
            s.Has(Jiushi) && !s.Has(Upgraded) && s.Facts<BoundaryCaoZhiJiushiUpgradedEvent>().Length == 0,
            "At the next real preparation the original once-game Chengzhang frame recovers one and draws one, awaiting its real draw child before shared skill replacement.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(!s.Has(Jiushi) && s.Has(Upgraded) && s.Has(Luoying) && s.Has(Chengzhang) && s.V().MaxHp == 8 &&
            s.Facts<BoundaryCaoZhiJiushiUpgradedEvent>() is [{ GameDamageTotal: 7 }] && s.Facts<SkillAwakenedEvent>().Count(e => e.SkillId == Chengzhang) == 1,
            "Native skill mutation replaces only Jiushi, keeps both other printed skills, records one awakening and does not reduce maximum HP.");
        s.Wine(Upgraded); s.Gain();
        var wine = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Upgraded && f.ActivationId == "virtual-alcohol");
        s.Need(wine.ProvenanceAlcohol is { Stage: ProgramProvenanceAlcoholStage.FaceCost, ChildFrameId: null } && s.V().IsFaceDown &&
            s.Facts<AlcoholAppliedEvent>().Length == 0 && s.Selections() is [{ CardId: { } first }] &&
            s.V().Hand.Single(c => c.Id == first).Kind == CardKind.Dismantlement,
            "Upgraded Jiushi's real Wine face cost opens the generic turnover trick gain before the original zero-material Wine is issued.");
        s.Cold(); s.Continue(); s.Play();
        s.Need(s.V().IsFaceDown && s.Selections().Length == 1 && s.Facts<AlcoholAppliedEvent>().Length == 1,
            "Cold resumes the original paid face child, then uses one Wine without another flip, random sample or acquisition.");
        s.Drive("flip", [0]); s.Gain(); s.Continue(); s.Play();
        s.Need(!s.V().IsFaceDown && s.Selections().Length == 2, "An unrelated native face-up turnover also grants the upgraded trick.");
        s.Drive("flip", [0]); s.Gain(); s.Continue(); s.Play();
        s.Drive("hurt-one", [3]); s.Offer(Upgraded); s.Activate(); s.Gain(); s.Continue(); s.Play();
        s.Need(!s.V().IsFaceDown && s.Selections().Length == 4 && s.V().Hp == 4,
            "A native back-face damage flip invokes the same upgraded turnover observer exactly once, with no extra base-version duplicate trick.");
        s.End(); s.Play();
        s.Need(s.Facts<BoundaryCaoZhiJiushiUpgradedEvent>().Length == 1 && s.Facts<SkillAwakenedEvent>().Count(e => e.SkillId == Chengzhang) == 1,
            "Further actual preparations cannot awaken or reissue the permanent upgrade again.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] Facts<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public BoundaryCaoZhiTrickSelectedEvent[] Selections() => Facts<BoundaryCaoZhiTrickSelectedEvent>();
        public int GameDamage() => Facts<DamageAppliedEvent>().Sum(e => (e.TargetSeat == 0 ? e.Amount : 0) + (!e.SourceLess && e.SourceSeat == 0 ? e.Amount : 0));
        public bool Has(string skill) => V().Skills!.Any(s => s.ContentId == skill);
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Offer(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Gain() => Reach(() => P?.SkillPrompt?.SkillId == BoundaryCaoZhiMobileChecks.Gain);
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Wine(string skill) => Accept(new UseProgramSkillCommand(0, skill, "virtual-alcohol", [], [], G.Revision, P!.PromptId));
        public void Drive(string id, IReadOnlyList<int> targets) => Accept(new UseProgramSkillCommand(0, BoundaryCaoZhiMobileChecks.Driver, id, [], targets, G.Revision, P!.PromptId));
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> pick) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, G.Revision)); }
        public void Reach(Func<bool> goal)
        {
            for (var i = 0; i < 240; i++)
            {
                if (goal()) return;
                Need(P?.PlayerSeat != 0, "Unexpected human prompt interrupted the native mobile Cao Zhi scene.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Native mobile Cao Zhi scene exceeded its fixed boundary steps.");
        }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == json && G.CardMovements.SequenceEqual(restored.CardMovements) &&
                JsonSerializer.Serialize(G.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
                "Cold preserves the original skill frame, exact random binding, native Wine face cost, gain child and shared upgrade sources."); G = restored;
        }
        private void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected mobile Cao Zhi command."); }
        public void Need(bool value, string why)
        {
            if (!value) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { G.State.CurrentSeat, G.State.TurnNumber, G.State.Phase, Hp = V().Hp, FaceDown = V().IsFaceDown,
                PendingKind = P?.Kind, PendingSeat = P?.PlayerSeat, PendingSkill = P?.SkillPrompt?.SkillId, Stack = G.ResolutionStack.Select(f => f.Kind), Last = G.Events.TakeLast(6).Select(e => e.Payload.GetType().Name) }));
        }
        public static Scene Start(bool upgrade = false)
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(upgrade));
            var setup = new ShanheBattleSetup(83, 1, "mobile-boundary-cao-zhi", Hero, 8, 5, 0, 0, 0, [Quiet, Driver, BoundaryCaoZhiMobileChecks.Gain],
                [new(CardKind.Dodge, Suit.Heart, 1), new(CardKind.Dodge, Suit.Spade, 2)], [],
                [new(Peer, Hp: 16) { StartingHand = [] }, new(Peer, Hp: 16) { StartingHand = [new(CardKind.Dodge, Suit.Club, 7)] }],
                [new(Peer, Hp: 16) { StartingHand = [] }], new string('f', 64));
            var g = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                AdvanceAfterHumanCommands = false, UseInteractiveSetup = false, UseInteractiveDiscard = false, MaxTurns = 10, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r);
            var scene = new Scene(g, r); scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(bool upgrade) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-boundary-cao-zhi", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            object Activation(string id, int targets, params object[] effects) => new { id, minCards = 0, maxCards = 0, minTargets = targets, maxTargets = targets, targetKind = "anyLiving", usesPerTurn = (int?)null, effects };
            object Hurt(int amount) => new { op = "damage", target = "owner", amount, sourceRef = new { kind = "selectedTarget" } };
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[]
            {
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false,
                    effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Driver, revision = 1, activations = new[]
                {
                    Activation("discard-other", 1, new { op = "selectAndMoveOwnedCard", target = "owner", chooserRef = new { kind = "selectedTarget" }, cardOwnerRef = new { kind = "selectedTarget" }, zones = new[] { "hand", "equipment" }, count = 1, destination = "discardPile" }),
                    Activation("judge", 1, new { op = "startJudgment", target = "selectedTarget", judgmentReason = "fixture:mobile-cz-judge", resultBind = "judged", visibility = "public" },
                        new { op = "moveBoundCards", target = "owner", sourceBind = "judged", destination = "discardPile", awaitMovementTriggers = true }),
                    Activation("duel", 2, new { op = "startVirtualDuel", target = "owner" }),
                    Activation("flip", 1, new { op = "turnOver", target = "selectedTarget" }),
                    Activation("hurt-seven", 1, Hurt(7)), Activation("hurt-four", 1, Hurt(4)), Activation("hurt-one", 1, Hurt(1)),
                    Activation("hit-three", 1, new { op = "damage", target = "selectedTarget", amount = 3 })
                } },
                new { id = Gain, revision = 1, triggers = new[] { new { id = "gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch",
                    movementReasons = new[] { "skill-program." + Luoying + ".provenance-claim", "skill-program." + Jiushi + ".MoveBoundCards", "skill-program." + Upgraded + ".MoveBoundCards", "skill-program." + Chengzhang + ".Draw" },
                    optional = false, isSkillInvocation = false, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "gain-return", options = new[] { new { id = "continue" } } } } } } }
            } });
            var names = new Dictionary<string, object>
            {
                [Quiet] = new { name = "安静摸牌", description = "省去无关普通摸牌。" }, [Driver] = new { name = "原生路径", description = "真实弃置、判定、卡牌伤害、程序伤害和翻面。" },
                [Gain] = new { name = "获取子窗", description = "原生获取与成章摸牌的真实子返回。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = names }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Peer, "酒诗安静同桌", "", Quiet, "wei", 16));
            const string deck = "fixture:mobile-cz-deck";
            var trick = upgrade ? "standard:dismantlement" : "standard:indulgence";
            b.AddDeck(new(deck, "真实普通或延时锦囊库存", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 40).Select(i => new ContentDeckPhysicalCard(i % 4 == 0 ? trick : "standard:dodge", Suit.Club, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动界曹植真实翻面与觉醒", 4, 4, new Dictionary<string, int>(), deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
