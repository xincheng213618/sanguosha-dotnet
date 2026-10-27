using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheLiyuChecks
{
    private const string Skill = "shanhe:s5-liyu-14128", Boss = "shanhe:s5-lvbu-baji", Wushuang = "shanhe:shengui-wushuang";
    private const string Driver = "fixture:liyu-driver", Gain = "fixture:liyu-gain", Paid = "fixture:liyu-paid";
    private const string HpGate = "fixture:liyu-hp-grant", Maker = "fixture:liyu-native-weapon";
    private const string ConditionalOwner = "fixture:liyu-conditional-owner", Victim = "fixture:liyu-victim", Peer = "fixture:liyu-peer", Reserve = "fixture:liyu-reserve";
    private const string Deck = "fixture:liyu-deck", Obtain = "skill-program.damage-target-obtain.obtain", Benefit = "skill-program.damage-target-obtain.benefit-draw";
    private const int VictimSeat = 2, ThirdSeat = 3;
    private static readonly Lazy<ContentRegistry> Production = new(() => StandardShanheCampaignCatalog.CreateBattleRegistry(
        new(83, 1, "fixture-liyu-production", "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [], [new(Boss)], [new("classic:sun-jian")], new string('a', 64))));
    private static readonly Lazy<ContentRegistry> Base = new(() => ContentRegistry.Build(new StandardContentPackage()));

    public static void WholeRegionsUseActualObtainReceiptsAndPrintedCategoryCold()
    {
        VerifyActualCatalogAndLoaderBoundaries();
        foreach (var scene in new[] { "hand", "equipment", "judgment", "mixed-discard" })
        {
            var s = Scene.Start(scene); var zone = scene == "equipment" ? CardZoneKind.Equipment : scene == "judgment" ? CardZoneKind.Judgment : CardZoneKind.Hand;
            if (zone == CardZoneKind.Equipment) s.PrepareEquipment();
            if (zone == CardZoneKind.Judgment) s.PlayCard(CardKind.Indulgence, [VictimSeat]);
            var originals = s.ZoneCards(zone).ToArray();
            var originalHandCount = s.V(VictimSeat).HandCount;
            Need(originals.Length == (scene == "judgment" ? 1 : 2), "The original victim's chosen zone is populated by actual starting, equipment-placement or delayed-use entities.");
            var slash = s.BeginCard(CardKind.Slash, [VictimSeat]); s.AtOffer(); s.Activate(); s.AtRegion();
            Need(s.P is { PlayerSeat: 0, IsPrivate: true } && s.P.Choices.All(choice => choice.Cards.Count == 0) &&
                s.P.Choices.All(choice => choice.Parameters.GetValueOrDefault("program-action") == "damage-target-obtain-region") &&
                Enumerable.Range(1, 3).All(viewer => s.G.CreateSnapshot(viewer).PendingDecision is null),
                "The actual chooser selects one whole HEJ region without receiving hidden Hand entity IDs or individual-card slots.");
            s.Reject(); s.Cold(); s.SelectRegion(zone);
            if (scene == "mixed-discard")
            {
                s.AtOwnedSelection(); var equipment = originals.Single(card => EquipmentCatalog.IsEquipment(card.Kind)).Id;
                var issued = s.RegionPaid(); Need(issued.Receipt.ActualObtainedCount == 2 && issued.Receipt.ContainsEquipment,
                    "The mixed original Hand atomically pays both actual gains before a genuine gain child can move either entity again.");
                s.Cold(); s.Answer(choice => choice.Cards.Contains(equipment));
                if (s.P?.SkillPrompt?.SkillId == Gain && s.P.Choices.Any(choice => Action(choice) == "finish-owned-cards"))
                    s.Answer(choice => Action(choice) == "finish-owned-cards");
                s.AtGain();
                Need(s.G.CreateCardZoneDiagnostics().Single(card => card.CardId == equipment).Location == CardLocation.DiscardPile &&
                    s.RegionPaid().Receipt.ActualObtainedCount == 2 && s.RegionPaid().Receipt.ContainsEquipment,
                    "The real gain child discards an actually obtained equipment card, while the paid count and printed-equipment branch stay frozen.");
            }
            else s.AtGain();
            var fact = s.RegionPaid(); var receipt = fact.Receipt;
            Need(receipt is { Paid: true, RecipientSeat: 0 } && receipt.SelectedZone == zone && receipt.MovementBatchId > 0 &&
                receipt.Materials.Select(material => material.CardId).Order().SequenceEqual(originals.Select(card => card.Id).Order()) &&
                receipt.ActualObtainedCount == originals.Length && receipt.Movements.Count == originals.Length &&
                receipt.Movements.All(move => move.From == new CardLocation(zone, VictimSeat) && move.To == CardLocation.Hand(0) && move.Reason == Obtain) &&
                receipt.Movements.All(move => s.G.CardMovements.Count(actual => actual.Sequence == move.MovementSequence && actual.CardId == move.CardId &&
                    actual.From == move.From && actual.To == move.To && actual.Reason.Value == Obtain) == 1) &&
                receipt.Materials is IList<DamageTargetRegionMaterial> { IsReadOnly: true } &&
                receipt.Movements is IList<ProgramDamageTargetObtainReceipt> { IsReadOnly: true },
                "One owning atomic region receipt freezes the selected original entities and every actual movement before all gain children: " + s.Diagnostic());
            var root = s.Root();
            Need(root.DamageTargetObtain?.OuterCardUseFrameId == fact.OuterCardUseFrameId &&
                root.DamageTargetObtain?.OuterActionId == fact.OuterActionId && fact.Source is { SkillId: Skill, OwnerSeat: 0 } &&
                fact.ProgramFrameId == root.Id && fact.Receipt.ActualObtainedCount == root.DamageTargetObtain?.RegionObtain?.ActualObtainedCount,
                "The real paid event and owning compound retain the same original Slash, actual source binding and paid gain child.");
            s.Cold(); s.Continue();
            if (scene is "equipment" or "mixed-discard")
            {
                s.ChooseDuelTarget(); s.AtDamage(); var duel = s.NestedDuel();
                Need(duel.DamageTargetDuelOrigin is { InitialActorSeat: 0, VictimChooserSeat: VictimSeat, InitialTargetSeat: ThirdSeat, BaseDamage: 2 } origin &&
                    origin.ParentProgramFrameId == fact.ProgramFrameId && origin.OuterCardUseFrameId == fact.OuterCardUseFrameId &&
                    duel.Action is { Type: CardActionType.Use, PhysicalCards.Count: 0 } && duel.CardId == 0 && duel.Id != fact.OuterCardUseFrameId &&
                    s.Events<DamageAppliedEvent>().Last() is { SourceSeat: 0, TargetSeat: ThirdSeat, Amount: 2 },
                    "The original victim selects a third character for one independent native zero-entity Duel; the local mixed branch and its base damage use the frozen two-card actual gain.");
                s.Cold(); s.Continue(); s.Play();
                Need(s.BenefitDraws().Length == 0 && s.Events<CardUseFinishedEvent>().Count(done => done.ResolutionId == duel.Id) == 1,
                    "The equipment branch neither draws the non-equipment compensation nor finishes its independent Duel twice.");
            }
            else
            {
                s.Play();
                Need(s.BenefitDraws() is [{ From: var from, To: var to }] && from == CardLocation.DrawPile && to == CardLocation.Hand(VictimSeat) &&
                    s.Events<CardUseDeclaredEvent>().All(use => use.CardKind != CardKind.Duel) &&
                    s.V(VictimSeat).HandCount == (zone == CardZoneKind.Hand ? 1 : originalHandCount + 1),
                    "An entirely non-equipment Hand or Judgment gain compensates the original victim with exactly one genuine draw, regardless of how many cards were obtained.");
            }
            s.AssertFinishedSlash(slash); s.AssertRegionOnce(fact); s.Cold();
        }
    }

    private static void VerifyActualCatalogAndLoaderBoundaries()
    {
        foreach (var difficulty in new[] { 1, 2, 3 })
        {
            var definition = StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);
            var encounter = definition.Nodes.Single(node => node.Id == "luoyang").Encounters!.Single(item => item.Id == "luoyang:lvbu-baji-s5");
            var enemy = encounter.Enemies.Single();
            Need(enemy.GeneralId == Boss && enemy.MaxHpBonus == difficulty - 1 && enemy.InitialHandBonus == difficulty - 1 &&
                enemy.DrawBonus == (difficulty == 3 ? 1 : 0) && encounter.Allies is { Count: 1 } &&
                StandardShanheCampaignCatalog.CreateDefinition(difficulty).Nodes.SelectMany(node => node.Encounters ?? []).All(item => item.Id != encounter.Id),
                "The actual current-edition candidate uses only its explicit local growth and real third-character companion; the old edition has no Baji candidate.");
            var setup = new ShanheBattleSetup(83, 1, "luoyang", "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [],
                encounter.Enemies, encounter.Allies ?? [], ShanheCampaignContent.GetContentHash(definition));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup); var general = registry.Generals[Boss];
            Need(general.BaseHp == 10 && general.FactionId == "qun" && general.Gender == GeneralGender.Male &&
                general.SkillIds.Order().SequenceEqual(new[] { Skill, Wushuang }.Order()) && setup.StartingEquipment.Count == 0 &&
                registry.GetDeck(registry.Modes[ShanheBattleSetup.BattleModeId].DeckId!).InitialHandSize == 4,
                "The real registry prints only dynamic Wushuang and regional Liyu on the raw 10HP form, with the ordinary four-card deal and no fabricated Shenqu, Jiwu or opening gear.");
            var game = StandardShanheCampaignCatalog.CreateBattle(setup); var started = game.Submit(new StartGameCommand()); Need(started.Accepted, started.Error?.Message ?? "Actual Baji encounter failed.");
            var bossSeat = 1 + setup.AllyRoster.Count; var boss = game.CreateSnapshot(bossSeat).Players[bossSeat];
            Need(boss.GeneralId == Boss && boss.MaxHp == 10 + difficulty - 1 && boss.Hp == boss.MaxHp && boss.Armor == 0 &&
                boss.Skills!.Select(skill => skill.ContentId).Order().SequenceEqual(new[] { Skill, Wushuang }.Order()) && boss.Equipment.Count == 0,
                "The actual catalogue recipe really starts that two-skill native form with its local HP growth and no initial Armor or fixed equipment."); ColdActual(game, registry);
        }
        var old = StandardShanheCampaignCatalog.CreateDefinition(); var original = old.Nodes.Single(node => node.Id == "luoyang").Encounters!.Single();
        var oldRegistry = StandardShanheCampaignCatalog.CreateBattleRegistry(new(83, 1, "luoyang", "classic:guan-yu", 4, 0, 0, 0, 0, [], [], [], original.Enemies, original.Allies ?? [], ShanheCampaignContent.GetContentHash(old)));
        Need(!oldRegistry.Skills.ContainsKey(Skill) && !oldRegistry.Generals.ContainsKey(Boss), "An actual old-edition encounter does not import the dedicated current Liyu form or skill bundle.");

        var assembly = typeof(StandardShanheS5BossPackage).Assembly;
        string Read(string suffix) { var name = assembly.GetManifestResourceNames().Single(resource => resource.EndsWith(suffix, StringComparison.Ordinal)); using var stream = assembly.GetManifestResourceStream(name)!; using var reader = new StreamReader(stream); return reader.ReadToEnd(); }
        var rules = Read("shanhe-s5-liyu.rules.json"); var presentation = Read("shanhe-s5-liyu.presentation.json");
        foreach (var change in new Action<JsonObject>[] { effect => effect.Remove("obtainMode"), effect => effect.Remove("duelDamageExpression"),
            effect => effect["obtainMode"] = "allZones", effect => effect["duelDamageExpression"] = "selectedCardCount", effect => effect["target"] = "selectedTarget" })
        {
            var root = JsonNode.Parse(rules)!; change(root["skills"]![0]!["triggers"]![0]!["effects"]![0]!.AsObject());
            var rejected = false; try { _ = SkillProgramCatalog.Load(root.ToJsonString(), presentation); } catch (Exception error) when (error is ArgumentException or InvalidOperationException) { rejected = true; }
            Need(rejected, "The paired opt-in region/acquired-count fields reject incomplete, invented or wrong-participant instructions.");
        }
    }

    private static string State(GameEngine game) => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, game.PlayerCount).Select(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat))),
        Frames = game.ResolutionStack.Select(frame => JsonSerializer.Serialize(frame, frame.GetType())), Facts = game.Events.Select(item => JsonSerializer.Serialize(item.Payload, item.Payload.GetType())),
        game.CardMovements, Zones = game.CreateCardZoneDiagnostics(), Checkpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint()) });
    private static GameEngine ColdActual(GameEngine game, ContentRegistry registry) { var before = State(game); var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry); Need(State(cold) == before, "Cold replay preserves the actual catalogue or fixture's exact typed uses, frozen region receipt, native responses and physical entities."); return cold; }
    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game, ContentRegistry registry, string scene)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(seat => G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => G.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public CardMovementRecord[] BenefitDraws() => G.CardMovements.Where(move => move.Reason.Value == Benefit && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(VictimSeat)).ToArray();
        public ProgramSkillFrame Root() => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Skill && frame.DamageTargetObtain is not null);
        public DamageTargetRegionObtainPaidEvent RegionPaid() => Events<DamageTargetRegionObtainPaidEvent>().Single();
        public CardUseFrame NestedDuel() => G.ResolutionStack.OfType<CardUseFrame>().Single(use => use.DamageTargetDuelOrigin is not null);
        public IReadOnlyList<CardSnapshot> ZoneCards(CardZoneKind zone) => zone == CardZoneKind.Hand ? V(VictimSeat).Hand : zone == CardZoneKind.Equipment ? V(VictimSeat).Equipment : V(VictimSeat).Judgment;
        public void Accept(GameCommand command) { var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Actual regional Liyu command rejected."); }
        public void Answer(Func<PromptChoice, bool> predicate) { var prompt = P!; Accept(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, G.Revision)); }
        public void Continue() => Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Use(string id, int[] targets) { Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId)); Play(); }
        public int BeginCard(CardKind kind, int[] targets)
        {
            var action = G.GetHumanLegalActions().First(action => action.CardId is { } id && V().Hand.Any(card => card.Id == id && card.Kind == kind) &&
                action.ConversionSource is null && action.Kind != LegalActionKind.Recast && action.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId)); return action.CardId.Value;
        }
        public int PlayCard(CardKind kind, int[] targets) { var card = BeginCard(kind, targets); Play(); return card; }
        public void Place(CardKind kind, int target)
        {
            var card = V().Hand.Single(card => card.Kind == kind).Id;
            Accept(new UseProgramSkillCommand(0, Driver, "place", [card], [target], G.Revision, P!.PromptId)); Play();
        }
        public void PrepareEquipment() { Place(CardKind.Crossbow, VictimSeat); Place(CardKind.OffensiveHorse, VictimSeat); }
        public void AtOffer() => Reach(() => P?.PlayerSeat == 0 && P.SkillPrompt?.SkillId == Skill && P.Choices.Any(choice => Action(choice) == "activate"));
        public void Activate() => Answer(choice => Action(choice) == "activate");
        public void AtRegion() => Reach(() => P?.PlayerSeat == 0 && P.Choices.Any(choice => Action(choice) == "damage-target-obtain-region"));
        public void SelectRegion(CardZoneKind zone) => Answer(choice => Action(choice) == "damage-target-obtain-region" && choice.Parameters.GetValueOrDefault("source-zone") == zone.ToString());
        public void AtGain() => Reach(() => P?.PlayerSeat == 0 && P.SkillPrompt?.SkillId == Gain && P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"));
        public void AtOwnedSelection() => Reach(() => P?.PlayerSeat == 0 && P.SkillPrompt?.SkillId == Gain && P.Choices.Any(choice => Action(choice) == "select-owned-cards"));
        public void AtTargetSelection() => Reach(() => P?.PlayerSeat == 0 && P.SkillPrompt?.SkillId == Gain && P.Choices.Any(choice => Action(choice) == "select-target"));
        public void AtDamage() => Reach(() => P?.PlayerSeat == 0 && P.SkillPrompt?.SkillId == Paid);
        public void AtResponse(int seat) => Reach(() => P is { Kind: DecisionKind.RespondSlash } prompt && prompt.PlayerSeat == seat);
        public void AwaitDuelResponses(long resolutionId, int responderSeat, int count)
        {
            for (var step = 0; step < 80; step++)
            {
                if (Events<DuelResponseEvent>().Count(item => item.ResolutionId == resolutionId && item.ResponderSeat == responderSeat && item.UsedSlash) >= count &&
                    Events<RequiredResponseProgressEvent>().Any(progress => progress.ResolutionId == resolutionId && progress.ResponderSeat == responderSeat && progress.ResponseCount >= count)) return;
                Need(P?.PlayerSeat != 0 && G.State.Winner == Winner.None,
                    "The native NPC response must be paid before another human decision or the battle ends: " + Diagnostic());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing actual NPC Duel response count " + count + ": " + Diagnostic());
        }
        public void AtDeathBoundary(int seat) => Reach(() => G.ResolutionStack.OfType<DyingFrame>().Any(frame => frame.VictimSeat == seat) || !V(seat).IsAlive);
        public void AtNullification() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.Nullification } && P.Choices.Any(choice => choice.Cards.Count == 1));
        public void ChooseDuelTarget()
        {
            Reach(() => P?.Choices.Any(choice => Action(choice) == "damage-target-duel") == true);
            Need(P!.PlayerSeat == VictimSeat && P.Choices.All(choice => !choice.Targets.Contains(0) && !choice.Targets.Contains(VictimSeat)),
                "Only the original victim owns the actual third-character choice, excluding the original actor and itself.");
            Cold(); Answer(choice => choice.Targets.SequenceEqual([ThirdSeat]));
        }
        public void Play() => Reach(() => P is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void FinishOriginal(int card) => Reach(() => Events<CardUseFinishedEvent>().Any(done => done.CardId == card));
        private void Reach(Func<bool> done)
        {
            for (var step = 0; step < 210; step++)
            {
                if (done()) return;
                if (P is { PlayerSeat: 0 } prompt)
                {
                    if (prompt.SkillPrompt?.SkillId == Paid || prompt.SkillPrompt?.SkillId == Gain && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue")) { Continue(); continue; }
                    if (prompt.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge or DecisionKind.RescueDying or DecisionKind.Nullification)
                    { Answer(choice => choice.Parameters.GetValueOrDefault("response") is "take-damage" or "pass" or "let-die"); continue; }
                    throw new InvalidOperationException("Unexpected real Liyu boundary: " + Diagnostic());
                }
                Need(G.State.Winner == Winner.None, "The bounded actual Liyu fixture ended before its typed return: " + Diagnostic());
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded real Liyu boundary: " + Diagnostic());
        }
        public void Reject() { var before = State(G); Need(!G.Submit(new AnswerPromptCommand(0, P!.PromptId, new("not-published-region"), G.Revision)).Accepted && State(G) == before, "Unpublished region choices preserve the exact original use, private choices and every unpaid entity."); }
        public void AssertPhysicalPaidOnce(int card) => Need(G.CardMovements.Count(move => move.CardId == card && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
            G.CardMovements.Count(move => move.CardId == card && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) == 1,
            "An original Slash, real response or Nullification is physically paid and cleaned up exactly once through native child returns.");
        public void AssertFinishedSlash(int card) { AssertPhysicalPaidOnce(card); Need(Events<CardUseDeclaredEvent>().Count(use => use.CardId == card && use.CardKind == CardKind.Slash) == 1 &&
            Events<CardUseFinishedEvent>().Count(use => use.CardId == card) == 1, "The original paid Slash declares and finishes once after its entire independent benefit."); }
        public void AssertRegionOnce(DamageTargetRegionObtainPaidEvent paid) => Need(Events<DamageTargetRegionObtainPaidEvent>().Length == 1 &&
            paid.Receipt.Movements.All(receipt => G.CardMovements.Count(move => move.CardId == receipt.CardId && move.Sequence == receipt.MovementSequence && move.Reason.Value == Obtain) == 1),
            "Cold restoration never reissues or repays the already-committed selected-region batch.");
        public void Cold() { G = ColdActual(G, registry); var zones = G.CreateCardZoneDiagnostics(); Need(zones.Select(card => card.CardId).Distinct().Count() == zones.Count, "Every actual entity remains in exactly one native zone."); }
        public string Diagnostic() => JsonSerializer.Serialize(new { Scene = scene, Prompt = P is null ? null : new { P.Kind, P.PlayerSeat, P.RequiredCardKind, Skill = P.SkillPrompt?.SkillId, Actions = P.Choices.Take(4).Select(choice => new { Action = Action(choice), choice.Cards, Response = choice.Parameters.GetValueOrDefault("response") }) },
            Paid = Events<DamageTargetRegionObtainPaidEvent>().Select(item => new { item.ProgramFrameId, item.VictimSeat, item.Receipt.SelectedZone, item.Receipt.ActualObtainedCount, item.Receipt.ContainsEquipment }),
            Damage = Events<DamageAppliedEvent>().Select(hit => new { hit.SourceSeat, hit.TargetSeat, hit.Amount }),
            Responses = Events<DuelResponseEvent>(), Progress = Events<RequiredResponseProgressEvent>(), LastThought = G.AiThoughts.LastOrDefault(),
            Uses = Events<CardUseDeclaredEvent>().Select(use => new { use.ResolutionId, use.SourceSeat, use.CardId, use.CardKind }),
            Frames = G.ResolutionStack.Select(frame => new { frame.Id, frame.Kind }) });
        public static Scene Start(string scene)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(scene));
            var own = new List<string> { Driver, Gain, Paid }; if (scene == "loss") own.AddRange([HpGate, Wushuang]);
            var cards = new List<ShanheStartingCard> { new(CardKind.Slash, Suit.Spade, 3), new(CardKind.Slash, Suit.Club, 4), new(CardKind.Slash, Suit.Spade, 5),
                new(CardKind.Crossbow, Suit.Club, 2), new(CardKind.OffensiveHorse, Suit.Heart, 6), new(CardKind.Indulgence, Suit.Heart, 7) };
            if (scene == "nullification") cards.Add(new(CardKind.Nullification, Suit.Spade, 11));
            ShanheStartingCard[] victimHand = scene == "hand" ? [new(CardKind.Peach, Suit.Heart, 2), new(CardKind.Alcohol, Suit.Heart, 3)] :
                scene == "mixed-discard" ? [new(CardKind.Peach, Suit.Heart, 2), new(CardKind.DefensiveHorse, Suit.Club, 3)] : [];
            var setup = new ShanheBattleSetup(83, 1, "fixture-liyu-" + scene, scene == "loss" ? ConditionalOwner : Boss, 10, 0, 0, 0, 0, own, cards, [],
                [new(Victim, ExtraSkillIds: scene.StartsWith("destroyed-", StringComparison.Ordinal) ? [Maker] : []) { StartingHand = victimHand },
                 new(Peer, Hp: scene == "dynamic" ? 18 : null) { StartingHand = scene == "dynamic" ? [new(CardKind.Slash, Suit.Club, 9), new(CardKind.Slash, Suit.Spade, 10)] : [] }],
                [new(Peer) { StartingHand = [] }], new string('a', 64));
            var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
            var s = new Scene(game, registry, scene); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shanhe-liyu", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            foreach (var id in new[] { Skill, Wushuang }) builder.AddSkill(Production.Value.GetSkill(id));
            builder.AddGeneral(Production.Value.Generals[Boss]);
            object Pause(string name) => new { op = "chooseOption", target = "owner", resultBind = name, options = new[] { new { id = "continue" } } };
            object[] gainEffects = scene switch
            {
                "loss" => [new { op = "loseHp", target = "owner", amount = 1 }, Pause("lost")],
                "owner-death" => [new { op = "loseHp", target = "owner", amount = 10 }, Pause("dead")],
                "victim-death" => [new { op = "selectTarget", target = "owner", targetKind = "otherLiving" }, new { op = "loseHp", target = "selectedTarget", amount = 20 }, Pause("victim-died")],
                "mixed-discard" => [new { op = "selectOwnedCards", target = "owner", zones = new[] { "hand" }, amount = 1, resultBind = "re-gained" },
                    new { op = "moveBoundCards", target = "owner", sourceBind = "re-gained", destination = "discardPile", awaitMovementTriggers = true }, Pause("discarded")],
                _ => [Pause("gained")]
            };
            var definitions = new object[] {
                new { id = Driver, revision = 1, modifiers = new[] { new { id = "range", query = "attackRange", operation = "add", value = 3, priority = 0 },
                    new { id = "slashes", query = "slashLimit", operation = "add", value = 10, priority = 0 }, new { id = "keep", query = "handLimit", operation = "add", value = 30, priority = 0 } },
                    activations = new object[] {
                        new { id = "place", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, cardCategories = new[] { "equipment" }, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                            effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "gear" }, new { op = "placeSelectedEquipment", target = "selectedTarget", sourceBind = "gear" } } },
                        new { id = "armor-one", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "anyLiving", usesPerTurn = (int?)null,
                            effects = new[] { new { op = "grantArmor", target = "selectedTarget", amount = 1 } } } } },
                new { id = Gain, revision = 1, triggers = new[] { new { id = "actual-region-gain-child", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                    movementOccurrence = "perBatch", movementReasons = new[] { Obtain }, optional = false, effects = gainEffects } } },
                new { id = Paid, revision = 1, triggers = new[] { new { id = "actual-damage-child", window = "afterDamageApplied", subject = "damageSource", damageOccurrence = "perDamage",
                    optional = false, priority = -100, effects = new[] { Pause("paid") } } } },
                new { id = HpGate, revision = 1, hpSkillGrants = new[] { new { id = "full-health-liyu", skillId = Skill, minimumHp = 10 } } },
                new { id = Maker, revision = 1, triggers = new[] { new { id = "real-native-weapon", window = "gameStarting", subject = "owner", optional = false,
                    effects = new[] { new { op = "equipSampledGenerals", target = "owner", amount = 1 } } } } }
            };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "真实利驭操作", description = "实体装备、杀和护甲，无已付状态注入" },
                [HpGate] = new { name = "真实体力条件来源", description = "HP10时授予生产利驭，失血后自然撤销" }, [Maker] = new { name = "真实备用武将武器", description = "调用原生武将武器生成和装备选择" } };
            foreach (var id in new[] { Gain, Paid }) labels[id] = new { name = "真实已付子窗口", description = "在真实获得牌或伤害付款后的原生子流程暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = definitions }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(ConditionalOwner, "真实条件来源", "", Driver, "qun", 10));
            builder.AddGeneral(new(Victim, "真实原受害者", "", "standard:none", "wei", 20)); builder.AddGeneral(new(Peer, "真实独立第三人", "", "standard:none", "wei", 20));
            builder.AddGeneral(new(Reserve, "真正未出场的武将武器来源", "", "standard:none", "wei", 4));
            foreach (var kind in new[] { CardKind.Indulgence, CardKind.GeneralWeapon })
                if (Base.Value.Cards.Values.All(card => card.LegacyKind != kind))
                { var card = CardCatalog.Get(kind); builder.AddCard(new("fixture:liyu-card-" + kind, card.DisplayName, card.CategoryName, card.Description, kind)); }
            builder.AddDeck(new(Deck, "零自然摸牌的实际牌堆", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(index => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, index % 13 + 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "真实区域利驭与独立决斗", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [scene == "loss" ? ConditionalOwner : Boss, Victim, Peer, Reserve], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
