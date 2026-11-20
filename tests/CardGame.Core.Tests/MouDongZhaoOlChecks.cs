using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Core.EngineOwners;

namespace CardGame.Core.Tests;

internal static class MouDongZhaoOlChecks
{
    private const string Hero = "mou:dong-zhao-ol", Shunji = "mou-ol:shunji", Yishi = "mou-ol:yishi",
        Driver = "fixture:mdzo-driver", Quiet = "fixture:mdzo-quiet", Owner = "fixture:mdzo-owner", Ally = "fixture:mdzo-ally",
        Enemy = "fixture:mdzo-enemy", Far = "fixture:mdzo-far", Deck = "fixture:mdzo-deck";
    private const string GainWatch = "fixture:mdzo-shunji-gain", DrawReason = "skill-program.mou-ol:shunji.draw";

    public static void RealDamageGiftAndNativeSuitLossObligationCold()
    {
        var s = Scene.Start(); s.Play();
        var crossbow = s.V().Equipment.Single(c => c.Kind == CardKind.Crossbow).Id;
        var dodge = s.V().Hand.Single(c => c.Kind == CardKind.Dodge).Id;
        var armor = s.V().Hand.Single(c => c.Kind == CardKind.SilverLion).Id;
        s.Accept(new PlayCardCommand(0, armor, [], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.V().Equipment.Any(c => c.Id == armor), "The original Silver Lion enters Equipment through a real accepted Use.");
        s.UseDriver("damage-one", targets: [1]);
        s.Reach(() => s.OfferShunji()); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.Verb("draw-2")); s.Answer(c => c.Parameters.GetValueOrDefault("verb") == "draw-2");
        s.Reach(() => s.P is { SkillPrompt.SkillId: GainWatch } prompt && prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));

        var shunji = s.E<MouDongZhaoShunjiAcceptedEvent>().Single();
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == shunji.FrameId);
        var movement = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.ParentFrameId == parent.Id);
        var drawReceipt = parent.NativeOperationDrawReturns.Single();
        var drawn = s.G.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray();
        s.Need(parent.MouDongZhao is { Stage: MouDongZhaoStage.DrawChildren, TargetSeat: 1, Draw: null, RequestedDraw: 0 } && parent.NativeDraw is null &&
            drawReceipt.Operation == SkillProgramEffectOp.ResolveMouDongZhaoShunji && drawReceipt.Reason.Value == DrawReason &&
            drawReceipt.Outcome is { RecipientSeat: 0, RequestedCount: 2, ActualDrawIds.Count: 2, Replacement: null } &&
            drawReceipt.Outcome.ActualDrawIds.SequenceEqual(drawn.Select(move => move.CardId)) &&
            drawn.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)) &&
            s.E<MouDongZhaoGiftSignedEvent>().Length == 0 && s.E<MouDongZhaoNameSpentEvent>().Length == 0,
            "Dong Zhao's actual gain observer suspends the original draw with its shared receipt and both cards already moved.");

        var gainPromptId = s.P!.PromptId;
        s.Cold();
        var restoredParent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id);
        var restoredMovement = s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Id == movement.Id);
        var runtime = s.G.GetRuntimeModule<CharacterDongZhaoMouOlRuntime>();
        s.Need(s.P?.PromptId == gainPromptId &&
            JsonSerializer.Serialize(restoredParent.NativeOperationDrawReturns.Single()) == JsonSerializer.Serialize(drawReceipt) &&
            runtime.MouDongZhaoFirstChild(restoredParent, restoredMovement) &&
            !runtime.MouDongZhaoFirstChild(restoredParent with { NativeOperationDrawReturns = [] }, restoredMovement),
            "Cold return preserves the real gain child and its original native outcome.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.Verb("gift-toggle")); s.Cold();
        s.Need(!s.Verb("gift-confirm"), "The gift cannot confirm before paying the actual draw count.");
        s.Answer(c => c.Parameters.GetValueOrDefault("verb") == "gift-toggle" && c.Cards.SequenceEqual([dodge]));
        s.Need(!s.Verb("gift-confirm"), "One original Hand entity does not satisfy the two-card gift.");
        s.Answer(c => c.Parameters.GetValueOrDefault("verb") == "gift-toggle" && c.Cards.SequenceEqual([armor]));
        s.Answer(c => c.Parameters.GetValueOrDefault("verb") == "gift-confirm"); s.Play();
        var gifts = s.E<MouDongZhaoGiftSignedEvent>().Where(e => e.FrameId == shunji.FrameId).ToArray();
        var names = s.E<MouDongZhaoNameSpentEvent>().Where(e => e.FrameId == shunji.FrameId).ToArray();
        var history = s.G.Events.Select(item => item.Payload).ToArray();
        var damage = s.E<DamageAppliedEvent>().Where(e => e.SourceSeat == 0 && e.TargetSeat == 1).ToArray();
        s.Need(s.E<MouDongZhaoDrawReturnedEvent>().Single(e => e.FrameId == shunji.FrameId) is { Requested: 2, Actual: 2, Replacement: 0 } &&
            s.G.CardMovements.Where(move => move.Reason.Value == DrawReason).SequenceEqual(drawn) &&
            gifts.Length == 2 && gifts.Any(e => e.CardId == dodge && e.From == CardLocation.Hand(0)) &&
            gifts.Any(e => e.CardId == armor && e.From == CardLocation.Equipment(0)) &&
            gifts.All(gift => gift.To == CardLocation.Hand(1) && s.V(1).Hand.Any(c => c.Id == gift.CardId) &&
                s.G.CardMovements.Count(move => move.Sequence == gift.Sequence && move.CardId == gift.CardId && move.From == gift.From && move.To == gift.To) == 1) &&
            names.Select(e => e.Kind).Order().SequenceEqual(new[] { CardKind.Dodge, CardKind.SilverLion }.Order()) &&
            damage.Length == 2 && s.V(1).Hp == 10 &&
            gifts.Max(e => Array.IndexOf(history, e)) < names.Min(e => Array.IndexOf(history, e)) &&
            names.Max(e => Array.IndexOf(history, e)) < Array.IndexOf(history, damage[1]) &&
            s.E<MouDongZhaoCompletedEvent>().Count(e => e.FrameId == shunji.FrameId) == 1,
            "After both actual gifts arrive from Hand and Equipment, Shunji spends their first lost names, deals one additional damage and completes once.");

        s.UseYishi(); s.KeepOnePerSuit(); s.Reach(() => s.Verb("field"));
        s.Answer(c => c.Parameters.GetValueOrDefault("verb") == "field" && c.Cards.SequenceEqual([crossbow]) && c.Targets.SequenceEqual([1])); s.Play();
        var first = s.E<MouDongZhaoYishiStartedEvent>().Single();
        var placed = s.G.CardMovements.Where(m => m.Reason.Value == "skill-program.mou-ol:yishi.place-rest").ToArray();
        s.Need(placed.Length == 2 && placed.All(m => m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile) &&
            !s.E<NativeLogicalDiscardInvocationRecordedEvent>().Any(e => e.Reason.Value == "skill-program.mou-ol:yishi.place-rest") &&
            s.V(1).Equipment.Any(c => c.Id == crossbow) && s.E<MouDongZhaoYishiGrantedEvent>().Length == 0 &&
            s.E<CardsRevealedEvent>().Count(e => e.ResolutionId == first.FrameId && e.Cards.Count == 2) == 1,
            "Yishi really shows one entity per suit, places the remainder without inventing a logical discard, moves native equipment, and grants no suit effect after losing Hand.");

        s.NextPlay(); s.UseYishi(); s.KeepOnePerSuit(); s.Reach(() => s.Verb("field"));
        s.Answer(c => c.Parameters.GetValueOrDefault("verb") == "field" && c.Cards.SequenceEqual([crossbow]) && c.Targets.SequenceEqual([0])); s.Play();
        var grant = s.E<MouDongZhaoYishiGrantedEvent>().Single();
        s.Need(grant.Suit == Suit.Spade && s.V().Equipment.Any(c => c.Id == crossbow) &&
            s.G.CardMovements.Count(m => m.CardId == crossbow && m.From == CardLocation.Equipment(1) && m.To == CardLocation.Equipment(0)) == 1 &&
            !s.G.GetHumanLegalActions().Any(a => a.ProgramSkillId == Yishi),
            "The next actual Play retains every current Hand entity and grants the actual moved card's Spade effect once, independently of the prior phase.");
        s.UseDriver("lose-yishi"); s.Play();
        var cost = s.V().Hand.Single(c => c.Kind == CardKind.Slash).Id;
        s.UseDriver("discard-one", cards: [cost]); s.Play();
        s.Need(s.E<MouDongZhaoYishiLossDrawIssuedEvent>() is [{ GrantFrameId: var issuedGrant }] && issuedGrant == grant.FrameId &&
            s.E<MouDongZhaoDrawReturnedEvent>().Count(e => e.Requested == 1 && e.Actual == 1) == 1,
            "An already paid suit grant survives true source loss and draws one for the real matching-suit loss.");
        var discard = s.E<ProgramSkillStartedEvent>().Where(e => e.OwnerSeat == 0 && e.SkillId == Driver && e.ActivationId == "discard-one").ToArray();
        var discardReason = $"skill-program.{Driver}.{SkillProgramEffectOp.DiscardSelected}";
        s.Need(s.G.AcceptedCommands.OfType<UseProgramSkillCommand>().Count(c => c.ActorSeat == 0 && c.SkillId == Driver &&
                c.ActivationId == "discard-one" && c.CardIds.SequenceEqual([cost])) == 1 &&
            discard.Length == 1 && s.E<ProgramSkillResolvedEvent>().Count(e => e.FrameId == discard[0].FrameId && e.Completed) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == cost && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == discardReason) == 1 &&
            !s.V().Hand.Any(c => c.Id == cost) && s.E<MouDongZhaoFieldMovedEvent>().Length == 2,
            "The accepted native DiscardSelected activation pays its one real Hand material and returns once, without repaying the already completed field move.");
        s.UseDriver("draw-two"); s.Play();
        var slash = s.V().Hand.First(c => c.Kind == CardKind.Slash).Id;
        s.NativeSlash(slash, 3); s.Play();
        s.Need(s.E<MouDongZhaoYishiLossDrawIssuedEvent>().Length == 1 &&
            s.E<CardUseDeclaredEvent>().Any(e => e.CardId == slash && e.SourceSeat == 0 && e.CardKind == CardKind.Slash),
            "A real matching-suit Slash Use pays its material without triggering the non-Use loss reward.");
        var beforePlayed = s.G.CardMovements.Count;
        s.UseDriver("direct-duel", targets: [2, 0]);
        s.Reach(() => s.P?.Kind == DecisionKind.RespondSlash);
        var response = s.P!.Choices.First(c => c.Parameters.GetValueOrDefault("response") == "slash");
        var responseId = response.Cards.Single(); s.Answer(c => c.Id == response.Id); s.Play();
        s.Need(s.E<MouDongZhaoYishiLossDrawIssuedEvent>().Length == 2 && s.E<MouDongZhaoYishiLossDrawIssuedEvent>().All(e => e.GrantFrameId == grant.FrameId) &&
            s.E<MouDongZhaoDrawReturnedEvent>().Count(e => e.Requested == 1 && e.Actual == 1) == 2 &&
            s.G.CardMovements.Skip(beforePlayed).Count(m => m.CardId == responseId && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.ResolutionStack.Count == 0,
            "The same original grant distinguishes one real Duel Played Slash from Use, performs its native Draw1 child, and returns the original Duel without repeating payment.");
        s.NextPlay();
        s.UseDriver("discard-one", cards: [s.V().Hand.First(c => c.Kind == CardKind.Slash).Id]); s.Play();
        s.Need(s.E<MouDongZhaoYishiLossDrawIssuedEvent>().Length == 2 &&
            s.E<MouDongZhaoYishiGrantedEvent>().Length == 1,
            "The next own actual TurnStart expires the paid suit effect even though the old original grant remains in history.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mou-dong-zhao-ol", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var definitions = new object[] {
                new { id = Driver, revision = 1, activations = new object[] {
                    new { id = "damage-one", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "damage", target = "selectedTarget", amount = 1 } } },
                    new { id = "discard-one", minCards = 1, maxCards = 1, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "discardSelected", target = "owner", amount = 1 } } },
                    new { id = "draw-two", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "draw", target = "owner", amount = 2 } } },
                    new { id = "direct-duel", minCards = 0, maxCards = 0, minTargets = 2, maxTargets = 2, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "startVirtualDuel", target = "owner" } } },
                    new { id = "lose-yishi", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, effects = new[] { new { op = "loseOwnerSkillsAndGrant", target = "owner", skillIds = new[] { Yishi }, sourceBind = "standard:none" } } }
                } },
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "afterNormalDraw", subject = "owner", optional = false,
                    effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } },
                new { id = GainWatch, revision = 1, triggers = new[] { new { id = "shunji-actual-gain", window = "cardsGained", subject = "owner",
                    optional = false, isSkillInvocation = false, usageScope = "game", usageLimit = 1, destinationZones = new[] { "hand" },
                    movementOccurrence = "perBatch", movementReasons = new[] { DrawReason }, effects = new[] {
                        new { op = "chooseOption", target = "owner", resultBind = "gain", options = new[] { new { id = "continue" } } } } } } }
            };
            var labels = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实原生请求", description = "公开支付和效果" },
                [Quiet] = new { name = "静默", description = "跳过无关出牌阶段" },
                [GainWatch] = new { name = "顺机摸牌观察", description = "在顺机实际得牌后暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            };
            var programs = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in programs.Programs) b.AddSkill(new(id, id, "原生小局") { Program = program, ProgramPresentation = programs.Presentations[id] });
            foreach (var kind in new[] { CardKind.Slash, CardKind.Dodge, CardKind.Peach, CardKind.SilverLion, CardKind.Crossbow })
            { var c = CardCatalog.Get(kind); b.AddCard(new("fixture:mdzo-card-" + kind, c.DisplayName, c.CategoryName, c.Description, kind)); }
            b.AddGeneral(new(Owner, "谋董昭真实主链", "", "standard:none", "wei", 9, [Shunji, Yishi, Driver, GainWatch]));
            foreach (var id in new[] { Ally, Enemy, Far }) b.AddGeneral(new(id, "静默同行", "", Quiet, "wei", 12));
            b.AddDeck(new(Deck, "固定四十八张真实黑杀", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(i => new ContentDeckPhysicalCard("fixture:mdzo-card-Slash", Suit.Spade, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "谋董昭OL原生主链", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Ally, Enemy, Far], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Verb(string verb) => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "mou-dong-zhao" && c.Parameters.GetValueOrDefault("verb") == verb) == true;
        public bool OfferShunji() => P?.SkillPrompt?.SkillId == Shunji && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void UseDriver(string id, int[]? cards = null, int[]? targets = null) => Accept(new UseProgramSkillCommand(0, Driver, id, cards ?? [], targets ?? [], G.Revision, P!.PromptId));
        public void UseYishi() => Accept(new UseProgramSkillCommand(0, Yishi, "keep-suits-and-move", [], [], G.Revision, P!.PromptId));
        public void KeepOnePerSuit()
        {
            Reach(() => Verb("keep-toggle") || Verb("keep-confirm"));
            foreach (var id in V().Hand.Where(c => c.Suit != Suit.None).GroupBy(c => c.Suit).Select(group => group.First().Id).ToArray())
                Answer(c => c.Parameters.GetValueOrDefault("verb") == "keep-toggle" && c.Cards.SequenceEqual([id]));
            Answer(c => c.Parameters.GetValueOrDefault("verb") == "keep-confirm");
        }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void NextPlay() { var before = G.State.TurnNumber; Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId)); Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.State.TurnNumber > before); }
        public void NativeSlash(int id, int target)
        {
            var action = G.GetHumanLegalActions().First(a => a.CardId == id && a.ConversionSource is null && a.TargetSeats.SequenceEqual([target]));
            Accept(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
        }
        public void Answer(Func<PromptChoice, bool> match)
        {
            var pending = P!; var choice = pending.Choices.FirstOrDefault(match); Need(choice is not null, "The real published choice must exist.");
            Accept(new AnswerPromptCommand(0, pending.PromptId, choice!.Id, G.Revision));
        }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 240; i++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "Unexpected terminal cutoff.");
                if (P is not { PlayerSeat: 0 } pending) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (pending.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification)
                    Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                else if (OfferShunji()) Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else throw new InvalidOperationException("Unexpected real Dong Zhao prompt: " + JsonSerializer.Serialize(pending));
            }
            throw new InvalidOperationException("Missing bounded native Dong Zhao boundary.");
        }
        public void Cold()
        {
            var view = SnapshotJson.Serialize(G.CreateSnapshot(0)); var movements = JsonSerializer.Serialize(G.CardMovements);
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(view == SnapshotJson.Serialize(G.CreateSnapshot(0)) && movements == JsonSerializer.Serialize(G.CardMovements), "The new private gift pending survives one real Cold without repeating the draw or paying cards twice.");
        }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, "Rejected " + command.GetType().Name + ": " + result.Error?.Message); }
        public void Need(bool ok, string message)
        {
            if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.Phase, G.State.Winner, P,
                Players = G.CreateSnapshot(0).Players.Select(p => new { p.Seat, p.Hp, p.HandCount }),
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
                Facts = G.Events.Where(e => e.Payload.GetType().Name.StartsWith("MouDongZhao", StringComparison.Ordinal)).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) }));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            static ShanheStartingCard C(CardKind kind, Suit suit) => new(kind, suit, 7);
            var setup = new ShanheBattleSetup(31, 1, "mdzo-native", Owner, 9, 0, 0, 0, 0, [],
                [C(CardKind.Slash, Suit.Spade), C(CardKind.Dodge, Suit.Heart), C(CardKind.Peach, Suit.Club), C(CardKind.SilverLion, Suit.Diamond)],
                [C(CardKind.Crossbow, Suit.Spade)],
                [new(Enemy) { StartingHand = [] }, new(Far) { StartingHand = [] }], [new(Ally) { StartingHand = [] }], new string('d', 64));
            var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 14 }, registry);
            var scene = new Scene(game, registry); scene.Accept(new StartGameCommand()); return scene;
        }
    }
}
