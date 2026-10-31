using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheDongJiChecks
{
    private const string Skill = StandardShanheS5DongshiPackage.LianzhuId, Boss = StandardShanheS5DongshiPackage.DongJiQiaoxiaoId;
    private const string Dong = StandardShanheS5DongshiPackage.DongZhuoBaxinId, Xiahui = "ol:xiahui", Encounter = "luoyang:dongshi-s5";
    private const string Owner = "fixture:dongji-owner", Driver = "fixture:dongji-driver", Deck = "fixture:dongji-deck";
    private const string GiftWatch = "fixture:dongji-gift", DrawWatch = "fixture:dongji-draw", DiscardWatch = "fixture:dongji-discard";
    private const string Health = "fixture:dongji-health", RecipientGain = "fixture:dongji-recipient-gain", Gate = "fixture:dongji-gate", Quiet = "fixture:dongji-quiet";
    private const string Reserve = "fixture:dongji-unused-general";
    private const int Recipient = 2;
    private const string GiftReason = "skill-program.lianzhu.GiftChildren", DiscardReason = "skill-program.lianzhu.DiscardChildren", DrawReason = "skill-program.lianzhu.DrawChildren";
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new(83, 1, "fixture:dongji-content", Boss, 8, 0, 0, 0, 0, [], [], [], [new("classic:cao-cao")], [], new string('c', 64))));

    public static void FrozenRecipientHpSurvivesEquipmentChildrenAndPaidSourceInvalidationCold()
    {
        var equipment = Scene.Start("equipment"); equipment.Play();
        var lion = equipment.V().Equipment.Single(card => card.Kind == CardKind.SilverLion).Id;
        Need(equipment.G.GetHumanLegalActions().Any(action => action.ProgramSkillId == Skill && action.SelectableCardIds.Contains(lion)),
            "The real new activation publishes a transferable equipped Silver Lion, rather than silently treating all gifts as hand cards.");
        equipment.Gift(lion); equipment.Until(() => equipment.At(Health));
        Need(equipment.V().Hp == 4 && equipment.Root.LianZhu is { GiftFrom: { } from, CountPolicy: null } && from == CardLocation.Equipment(0) &&
            equipment.E<LianZhuChoiceAmountCapturedEvent>().Length == 0 && equipment.E<RecoveryAppliedEvent>() is [{ TargetSeat: 0, Amount: 1 }],
            "A real black armor gift leaves equipment, pays the exact gift and drains the source's Silver Lion recovery before capturing X.");
        equipment.AssertGift(lion, CardLocation.Equipment(0)); equipment.Cold(); equipment.Continue();
        equipment.Until(equipment.AtChoice); equipment.AssertAmount(4);
        Need(equipment.V(Recipient).Hp == 4 && equipment.E<RecoveryAppliedEvent>().Any(fact => fact.TargetSeat == Recipient && fact.Amount == 1),
            "The recipient's genuine gained-card child recovers from three to four; X is captured only after this real gift child too.");
        equipment.AssertRecipientAuthority(); equipment.Answer(choice => Branch(choice) == "draw");
        equipment.Until(() => equipment.At(DrawWatch) && equipment.P!.Choices.Any(choice => choice.Targets.SequenceEqual([Recipient])));
        equipment.Answer(choice => choice.Targets.SequenceEqual([Recipient])); equipment.Until(() => equipment.At(DrawWatch) && equipment.P!.Choices.Any(IsContinue));
        Need(equipment.V(Recipient).Hp == 3 && equipment.Root.LianZhu is { CountPolicy: { RecipientHp: 4, Amount: 4 }, Count: 4 } &&
            equipment.E<LianZhuChoiceAmountCapturedEvent>() is [{ RecipientHp: 4, Amount: 4 }] &&
            equipment.E<XiaHuiRestrictionReleasedEvent>().Any(fact => fact.RecipientSeat == Recipient && fact.CardId == lion && fact.Reason == "recipient-hp-decreased") &&
            equipment.G.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.From == CardLocation.DrawPile && move.Reason.Value == DrawReason) == 4,
            "After choosing, an actual paid-draw child lowers the recipient's HP and releases Xiahui, while the original frozen X and four paid draw entities remain unchanged.");
        equipment.Cold(); var equipmentFrame = equipment.Root.Id; equipment.Play(); equipment.AssertFinished(equipmentFrame); equipment.Cold();

        foreach (var scene in new[] { "paid-loss", "paid-death", "recipient-death" })
        {
            var s = Scene.Start(scene); s.Play(); var gift = s.V().Hand.Single(card => card.Suit == Suit.Spade).Id;
            s.Gift(gift); s.Until(() => s.At(GiftWatch)); var root = s.Root.Id;
            s.AssertGift(gift, CardLocation.Hand(0)); s.Cold(); s.Continue();
            if (scene == "paid-loss")
            {
                s.Until(() => s.At(GiftWatch) && s.P!.Choices.Any(choice => Option(choice) == "lost"));
                Need(s.V().Hp == 7 && s.V().Skills?.Any(skill => skill.ContentId == Skill) != true,
                    "Actual HP loss withdraws the conditional source after the gift has already paid.");
                s.Cold(); s.Continue();
            }
            if (scene == "recipient-death")
            {
                s.Until(() => !s.V(Recipient).IsAlive && s.E<ProgramSkillResolvedEvent>().Any(fact => fact.FrameId == root));
                Need(s.E<LianZhuChoiceAmountCapturedEvent>().Length == 0 && s.E<LianZhuSegmentPaidEvent>().Length == 1 &&
                    s.G.State.Winner == Winner.None && s.V(3).IsAlive,
                    "The recipient's real gained-card child dies before choosing; the original paid gift completes without inventing a zero-HP choice or another payment.");
            }
            else
            {
                s.Until(s.AtChoice); s.AssertAmount(3); s.AssertRecipientAuthority(); s.Cold(); s.Answer(choice => Branch(choice) == "draw");
                if (scene == "paid-loss") { s.Until(() => s.At(DrawWatch)); s.Cold(); s.Play(); }
                else s.Until(() => s.E<ProgramSkillResolvedEvent>().Any(fact => fact.FrameId == root));
                var draw = s.E<LianZhuSegmentPaidEvent>().Single(fact => fact.Stage == LianZhuStage.DrawChildren);
                Need(draw.Count == (scene == "paid-death" ? 0 : 3) && s.E<LianZhuChoiceAmountCapturedEvent>().Length == 1 &&
                    s.G.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason.Value == DrawReason) == draw.Count &&
                    (scene != "paid-death" || !s.V().IsAlive && s.V(1).IsAlive && s.G.State.Winner == Winner.None),
                    "Paid-source loss preserves the original benefit; true source death pays no new cards and still returns with a living ally, without hiding under game-over cleanup.");
            }
            s.AssertFinished(root); s.AssertGift(gift, CardLocation.Hand(0)); s.Cold();
        }
        VerifyNonTransferableWeapon();
        VerifyNativeNpcDecisions();
    }

    private static void VerifyNonTransferableWeapon()
    {
        var s = Scene.Start("weapon"); s.Play(); s.UseDriver("weapon");
        var weapon = s.V().Equipment.Single(card => card.Kind == CardKind.GeneralWeapon).Id;
        var before = s.State();
        Need(s.G.GetHumanLegalActions().Where(action => action.ProgramSkillId == Skill).All(action => !action.SelectableCardIds.Contains(weapon)),
            "A real native sampled-general weapon would be destroyed on departure and is absent from the published HE gift candidates.");
        s.Reject(new UseProgramSkillCommand(0, Skill, "reveal-owned-gift", [weapon], [Recipient], s.G.Revision, s.P!.PromptId));
        Need(s.State() == before && s.V().Equipment.Any(card => card.Id == weapon) && s.E<LianZhuSegmentPaidEvent>().Length == 0,
            "Forcing the same nontransferable original entity is atomically rejected and cannot manufacture a gifted OutsideGame weapon.");
        s.Cold();
    }

    private static void VerifyNativeNpcDecisions()
    {
        var choice = Scene.Start("npc-choice"); choice.Play(); var gift = choice.V().Hand.Single(card => card.Suit == Suit.Spade).Id;
        choice.Gift(gift); choice.Until(choice.AtChoice); var root = choice.Root.Id; choice.AssertAmount(3); choice.Cold();
        choice.Play();
        var segment = choice.E<LianZhuSegmentPaidEvent>().Single(fact => fact.Stage is LianZhuStage.DrawChildren or LianZhuStage.DiscardChildren);
        Need(segment.Count == 3 && choice.E<LianZhuChoiceAmountCapturedEvent>().Length == 1 &&
            choice.G.CardMovements.Count(move => move.Reason.Value == (segment.Stage == LianZhuStage.DrawChildren ? DrawReason : DiscardReason)) == 3,
            "AdvanceOneStep drives the real NPC recipient's dynamic branch and all exact-X entities; the test does not answer its choice.");
        choice.AssertFinished(root); choice.Cold();

        var npc = Scene.Start("npc-source"); npc.Play(); npc.EndPlay();
        npc.Until(() => npc.E<ProgramSkillResolvedEvent>().Any(fact => fact.OwnerSeat == 1 && fact.SkillId == Skill));
        var giftFact = npc.E<LianZhuSegmentPaidEvent>().Single(fact => fact.Stage == LianZhuStage.GiftChildren && fact.Source.OwnerSeat == 1);
        var captured = npc.E<LianZhuChoiceAmountCapturedEvent>().Single(fact => fact.FrameId == giftFact.FrameId);
        var paid = npc.E<LianZhuSegmentPaidEvent>().Single(fact => fact.FrameId == giftFact.FrameId && fact.Stage is LianZhuStage.DrawChildren or LianZhuStage.DiscardChildren);
        Need(npc.E<ProgramSkillResolvedEvent>().Single(fact => fact.FrameId == giftFact.FrameId) is { OwnerSeat: 1, Completed: true } && captured.RecipientSeat == 0 &&
            captured.Amount == captured.RecipientHp && captured.Amount > 0 && paid.Count == captured.Amount &&
            npc.G.CardMovements.Any(move => move.From == CardLocation.Hand(1) && move.Reason.Value == GiftReason) &&
            npc.G.AiThoughts.Any(thought => thought.ActorSeat == 1),
            "The real native NPC uses the actual production 14212 activation, transfers an original black entity, chooses its dynamic follow-up and finishes without a forged NPC UseProgramSkill command.");
        npc.Cold();
    }

    private static string Peer(int seat) => "fixture:dongji-peer-" + seat;
    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");
    private static string? Option(PromptChoice choice) => choice.Parameters.GetValueOrDefault("option-id");
    private static bool IsContinue(PromptChoice choice) => Option(choice) == "continue";
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static string StateActual(GameEngine game) => JsonSerializer.Serialize(new
    {
        Views = Enumerable.Range(0, game.PlayerCount).Select(viewer => SnapshotJson.Serialize(game.CreateSnapshot(viewer))),
        Frames = game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())),
        Facts = game.Events.Select(fact => JsonSerializer.Serialize(fact.Payload, fact.Payload.GetType())),
        game.CardMovements, Zones = game.CreateCardZoneDiagnostics(), Commands = CommandJson.Serialize(game.AcceptedCommands)
    });
    private static GameEngine ColdActual(GameEngine game, ContentRegistry registry)
    {
        var before = StateActual(game); var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(before == StateActual(restored), "Cold replay preserves exact gift locations, frozen HP count, selected HE entities, paid receipts, private authority and actual child frames.");
        return restored;
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry, string scene)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, G.PlayerCount).Select(viewer => G.CreateSnapshot(viewer).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] E<T>() => G.Events.Select(fact => fact.Payload).OfType<T>().ToArray();
        public ProgramSkillFrame Root => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill);
        public bool At(string skill) => P?.PlayerSeat == 0 && P.SkillPrompt?.SkillId == skill;
        public bool AtChoice() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(choice => Branch(choice) == "draw");
        public string State() => StateActual(G);
        public void Accept(GameCommand command)
        { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, (result.Error?.Message ?? "Actual Dong Ji command rejected.") + " " + Diagnostic()); }
        public void Reject(GameCommand command)
        { var before = State(); Need(!G.Submit(command).Accepted && before == State(), "An invalid actor, entity or already-used activation cannot mutate any actual private view, frame, zone or accepted journal."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = P ?? throw new InvalidOperationException("Missing actual Dong Ji prompt.");
            var published = G.CreateSnapshot(prompt.PlayerSeat).PendingDecision ?? throw new InvalidOperationException("The real recipient did not receive its private prompt.");
            Accept(new AnswerPromptCommand(published.PlayerSeat, published.PromptId, published.Choices.First(predicate).Id, G.Revision));
        }
        public void Continue() => Answer(choice => Option(choice) is "continue" or "lost");
        public void Gift(int id) => Accept(new UseProgramSkillCommand(0, Skill, "reveal-owned-gift", [id], [Recipient], G.Revision, P!.PromptId));
        public void UseDriver(string activation, int[]? targets = null)
        { Accept(new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], G.Revision, P!.PromptId)); Play(); }
        public void PlaceLion()
        { var id = V().Hand.Single(card => card.Kind == CardKind.SilverLion).Id; Accept(new UseProgramSkillCommand(0, Driver, "place", [id], [Recipient], G.Revision, P!.PromptId)); Play(); }
        public void EndPlay() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Play() => Until(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 320; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.SkillPrompt?.SkillId is GiftWatch or DrawWatch or DiscardWatch or Health && prompt.Choices.Any(choice => Option(choice) is "continue" or "lost")) { Continue(); continue; }
                    if (prompt.SkillPrompt?.SkillId == Driver && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "advanced-lifecycle"))
                    { var select = prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("advanced-value") != "finish");
                      Answer(choice => (choice.Parameters.GetValueOrDefault("advanced-value") != "finish") == select); continue; }
                    if (scene == "npc-source" && prompt.SkillPrompt?.SkillId == Skill && prompt.Choices.Any(choice => Branch(choice) == "draw"))
                    { Answer(choice => Branch(choice) == "draw"); continue; }
                    if (prompt.Kind == DecisionKind.RescueDying) { Answer(choice => choice.Parameters.GetValueOrDefault("response") == "let-die"); continue; }
                    throw new InvalidOperationException("Unexpected actual Dong Ji boundary: " + Diagnostic());
                }
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded actual Dong Ji boundary: " + Diagnostic());
        }
        public void AssertAmount(int amount)
        {
            var r = Root.LianZhu!; var fact = E<LianZhuChoiceAmountCapturedEvent>().Single();
            Need(r.CountPolicy is { } policy && policy.RecipientHp == amount && policy.Amount == amount &&
                fact.FrameId == Root.Id && fact.Source == r.Source && fact.GameplayHash == Root.GameplayHash &&
                fact.RecipientSeat == Recipient && fact.RecipientHp == amount && fact.Amount == amount && fact.GiftAfter == r.GiftAfter,
                "One exact scalar amount is issued from the live recipient HP only after all genuine gift children have drained: " + Diagnostic());
        }
        public void AssertRecipientAuthority()
        {
            var prompt = G.CreateSnapshot(Recipient).PendingDecision;
            Need(prompt is { PlayerSeat: Recipient, IsPrivate: true } && G.CreateSnapshot(0).PendingDecision is null &&
                G.CreateSnapshot(0).Players[Recipient].Hand.Count == 0,
                "Only the actual recipient view publishes its private branch/cards; the source learns neither private choices nor the recipient's full hand.");
            Reject(new AnswerPromptCommand(0, prompt!.PromptId, prompt.Choices[0].Id, G.Revision));
        }
        public void AssertGift(int id, CardLocation from) => Need(G.CardMovements.Count(move => move.CardId == id && move.From == from &&
            move.To == CardLocation.Hand(Recipient) && move.Reason.Value == GiftReason) == 1 &&
            E<LianZhuSegmentPaidEvent>().Count(fact => fact.Stage == LianZhuStage.GiftChildren) == 1,
            "Exactly one original hand/equipment entity moves into the recipient's real hand and issues one native gift receipt.");
        public void AssertFinished(long frame) => Need(E<ProgramSkillResolvedEvent>().Where(fact => fact.FrameId == frame).ToArray() is [{ SkillId: Skill, Completed: true }] &&
            !G.ResolutionStack.Any(owner => owner.Id == frame), "The actual active native program resolves once and removes its owner only after every real child has returned.");
        public void Cold() { G = ColdActual(G, registry); var zones = G.CreateCardZoneDiagnostics(); Need(zones.Select(card => card.CardId).Distinct().Count() == zones.Count, "Every real entity occupies exactly one native card zone."); }
        public string Diagnostic() => JsonSerializer.Serialize(new
        {
            scene, G.State.TurnNumber, Players = G.CreateSnapshot(0).Players.Select(player => new { player.Seat, player.Hp, player.IsAlive, player.HandCount }),
            Prompt = P is null ? null : new { P.Kind, P.PlayerSeat, Skill = P.SkillPrompt?.SkillId, Ids = P.Choices.Select(choice => choice.Id.Value) },
            Captured = E<LianZhuChoiceAmountCapturedEvent>(), Paid = E<LianZhuSegmentPaidEvent>().Select(fact => new { fact.FrameId, fact.Stage, fact.ActorSeat, fact.Count }),
            Frames = G.ResolutionStack.Select(frame => new { frame.Id, frame.Kind }), LastThought = G.AiThoughts.LastOrDefault()
        });
        public static Scene Start(string scene)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene));
            var hand = new List<ShanheStartingCard>();
            if (scene == "npc-source") hand.AddRange([new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Heart, 3), new(CardKind.Dodge, Suit.Heart, 4)]);
            else hand.AddRange([new(CardKind.Dodge, scene == "red" ? Suit.Heart : Suit.Spade, 7), new(CardKind.Dodge, Suit.Diamond, 9)]);
            if (scene == "discard") hand.Add(new(CardKind.SilverLion, Suit.Club, 1));
            ShanheStartingCard[] recipient = scene == "npc-source" ? [new(CardKind.Dodge, Suit.Spade, 3)] :
                Enumerable.Range(0, scene is "draw" or "equipment" or "npc-choice" ? 3 : 2).Select(index => new ShanheStartingCard(CardKind.Dodge, Suit.Heart, index + 2)).ToArray();
            var enemies = scene == "npc-source" ? new ShanheCombatantDefinition[] { new(Peer(2), Hp: 1) { StartingHand = [] } } :
                [new(Peer(Recipient), Hp: 3) { StartingHand = recipient }, new(Peer(3)) { StartingHand = [] }];
            var setup = new ShanheBattleSetup(83, 1, "fixture:dongji-" + scene, Owner, scene is "equipment" or "npc-source" ? 3 : 8,
                0, 0, 0, 0, [], hand, scene == "equipment" ? [new(CardKind.SilverLion, Suit.Club, 1)] : [],
                enemies, scene == "npc-source" ? [new(Peer(1)) { StartingHand = recipient }] : [new(Peer(1)) { StartingHand = [] }], new string('b', 64));
            var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = scene == "npc-source" ? 3 : 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var result = new Scene(game, registry, scene); result.Accept(new StartGameCommand()); return result;
        }
    }

    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-dongji", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            foreach (var id in new[] { Skill, Xiahui }) b.AddSkill(Production.Value.GetSkill(id));
            b.AddCard(Production.Value.GetCard("classic:silver-lion"));
            var generalWeapon = CardCatalog.Get(CardKind.GeneralWeapon);
            b.AddCard(new("fixture:dongji-general-weapon", generalWeapon.DisplayName, generalWeapon.CategoryName, generalWeapon.Description, CardKind.GeneralWeapon));
            var definitions = new List<Dictionary<string, object>>(); var labels = new Dictionary<string, object>();
            object Pause(string option) => new { op = "chooseOption", target = "owner", resultBind = option, options = new[] { new { id = option } } };
            void Add(string id, Dictionary<string, object> body, params string[] options)
            { body["id"] = id; body["revision"] = 1; definitions.Add(body); labels[id] = new { name = id, description = "原牌赠予、原生HE弃牌与已付子流程", optionLabels = options.ToDictionary(option => option, option => option) }; }
            Add(Driver, new() { ["activations"] = new object[]
            {
                new { id = "place", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, cardCategories = new[] { "equipment" }, minTargets = 1, maxTargets = 1,
                    targetKind = "otherLiving", usesPerTurn = (int?)null, effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "gear" }, new { op = "placeSelectedEquipment", target = "selectedTarget", sourceBind = "gear" } } },
                new { id = "weapon", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "equipSampledGenerals", target = "owner", amount = 1 } } },
                new { id = "armor", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "grantArmor", target = "selectedTarget", amount = 2 } } },
                new { id = "hurt", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "damage", target = "selectedTarget", amount = 2 } } },
                new { id = "lose", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                    effects = new[] { new { op = "loseHp", target = "selectedTarget", amount = 1 } } }
            } });
            var giftEffects = new List<object> { Pause("continue") };
            if (scene is "paid-loss" or "paid-death") giftEffects.Add(new { op = "loseHp", target = "owner", amount = scene == "paid-loss" ? 1 : 20 });
            if (scene == "paid-loss") giftEffects.Add(Pause("lost"));
            Add(GiftWatch, new() { ["triggers"] = new[] { new { id = "actual-paid-hand-gift", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" },
                movementOccurrence = "perBatch", movementReasons = new[] { GiftReason }, optional = false, usageScope = "game", usageLimit = 1, effects = giftEffects.ToArray() } } },
                scene == "paid-loss" ? ["continue", "lost"] : ["continue"]);
            var drawEffects = new List<object>();
            if (scene == "equipment") drawEffects.AddRange([new { op = "selectTarget", target = "owner", targetKind = "otherLiving" }, new { op = "loseHp", target = "selectedTarget", amount = 1 }]);
            drawEffects.Add(Pause("continue"));
            Add(DrawWatch, new() { ["triggers"] = new[] { new { id = "actual-paid-draw-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                movementOccurrence = "perBatch", movementReasons = new[] { DrawReason }, optional = false, usageScope = "game", usageLimit = 1, effects = drawEffects.ToArray() } } }, "continue");
            Add(DiscardWatch, new() { ["triggers"] = new[] { new { id = "actual-paid-discard-child", window = "discardPileReceived", subject = "owner", optional = false,
                movementReasons = new[] { DiscardReason }, usageScope = "game", usageLimit = 1, effects = new[] { Pause("continue") } } } }, "continue");
            Add(Health, new() { ["triggers"] = new[] { new { id = "native-armor-recovery", window = "afterHpRecovered", subject = "owner", hpChangeOccurrence = "perEvent", optional = false,
                effects = new[] { Pause("continue") } } } }, "continue");
            Add(RecipientGain, new() { ["triggers"] = new[] { new { id = "actual-gift-recipient-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                movementOccurrence = "perBatch", movementReasons = new[] { GiftReason }, optional = false, usageScope = "game", usageLimit = 1,
                effects = new[] { new { op = scene == "recipient-death" ? "loseHp" : "recover", target = "owner", amount = scene == "recipient-death" ? 20 : 1 } } } } });
            Add(Gate, new() { ["hpSkillGrants"] = new[] { new { id = "actual-full-health-source", skillId = Skill, minimumHp = 8 } } });
            Add(Quiet, new() { ["triggers"] = new[] { new { id = "quiet-real-turn", window = "afterNormalDraw", subject = "owner", optional = false,
                effects = new[] { new { op = "skipTurnPhases", target = "owner", phases = new[] { "play" } } } } } });
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                { Program = program, ProgramPresentation = catalog.Presentations[id] });
            var own = new List<string> { Driver, Xiahui, GiftWatch, DrawWatch, DiscardWatch, Health };
            if (scene == "paid-loss") own.Add(Gate);
            b.AddGeneral(new(Owner, "真实董姬技能拥有者", "", scene is "paid-loss" or "npc-source" ? Driver : Skill, "qun", 8,
                own.Where(id => id != (scene is "paid-loss" or "npc-source" ? Driver : Skill)).ToArray(), GeneralGender.Female));
            var count = scene == "npc-source" ? 3 : 4;
            for (var seat = 1; seat < count; seat++)
            {
                var extra = new List<string>();
                if (seat == Recipient && (scene is "equipment" or "recipient-death")) extra.Add(RecipientGain);
                if (scene == "npc-source" && seat == 1) extra.Add(Xiahui);
                b.AddGeneral(new(Peer(seat), "实际受赠者或同行", "", scene == "npc-source" && seat == 1 ? Skill : Quiet, "qun", 8, extra));
            }
            b.AddGeneral(new(Reserve, "真实未上场武将武器材料", "", "standard:none", "wei", 4));
            b.AddDeck(new(Deck, "固定真实闪牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 96).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, index % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "真实董姬赠牌与动态选择", count, count, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: Enumerable.Range(1, count - 1).Select(Peer).Prepend(Owner).Append(Reserve).ToArray(), ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = count - 2 }));
        }
    }
}
