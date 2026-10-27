using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceGrainChecks
{
    private const string Cangchu = StandardShanheSourceCoalitionTrioPackage.CangchuSkillId,
        Sushou = StandardShanheSourceCoalitionTrioPackage.SushouSkillId,
        Driver = "fixture:source-grain-fire", Watch = "fixture:source-grain-equipment-child", Deck = "fixture:source-grain-deck";
    private const string GiftReason = "activity.guandu.shanhe:source-sushou-1183.gift";

    public static void ActualFireDamageConsumesSourceGrainAndSushouSharesEquippedCardsOnlyWithTrueTeammates()
    {
        var s = Scene.Start();
        var owner = s.V(0);
        var ally = s.G.CreateSnapshot(0).Players.Single(p => p.Seat != 0 && p.TeamId == owner.TeamId);
        var enemy = s.G.CreateSnapshot(0).Players.Single(p => p.TeamId != owner.TeamId);
        var equipment = owner.Hand.Single(c => c.Kind == CardKind.Crossbow).Id;
        Need(owner.GeneralId == StandardShanheSourceCoalitionTrioPackage.QuartermasterHardId && owner.Hp == 3 && owner.MaxHp == 3 &&
            owner.Equipment.Count == 0 && s.Grain == 3 &&
            s.G.ContentRegistry.Generals[owner.GeneralId!].FactionId == "qun" &&
            s.G.ContentRegistry.Generals[ally.GeneralId!].FactionId == "shu" &&
            s.G.ContentRegistry.Generals[enemy.GeneralId!].FactionId == "qun" &&
            s.Events<PlayerMarkerChangedEvent>().Count(e => e.PlayerSeat == 0 && e.SkillOwnerSeat == 0 &&
                e.Marker == PlayerMarkerKind.GuanduGrain && e.Delta == 3 && e.Count == 3) == 1 &&
            s.Events<GuanduGeneralIssuedEvent>().Count(e => e.OwnerSeat == 0 && e.SkillId == Cangchu &&
                e.Operation == SkillProgramEffectOp.ResolveGuanduCangchu) == 1,
            "The real source2465 hard quartermaster receives three grain through its actual opening program; a different-faction ally and same-faction enemy retain their real teams.");
        var equip = s.G.GetHumanLegalActions().Single(a => a.CardId == equipment && a.Kind == LegalActionKind.Equip && a.TargetSeat is null);
        s.Accept(new PlayCardCommand(0, equipment, equip.TargetSeats, s.G.Revision, s.P!.PromptId, equip.PlayedCardKind)); s.Play();
        Need(s.V(0).Equipment.Single(c => c.Kind == CardKind.Crossbow).Id == equipment &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Processing && m.To == CardLocation.Equipment(0)) == 1,
            "The future gift is first equipped by its genuine physical card use, with its original entity passing through Processing.");
        s.Accept(new UseProgramSkillCommand(0, Driver, "fire-two", [], [], s.G.Revision, s.P!.PromptId)); s.Play();
        Need(s.V(0).Hp == 1 && s.Grain == 1 &&
            s.Events<DamageAppliedEvent>().Single() is { SourceSeat: 0, TargetSeat: 0, Amount: 2, Nature: DamageNature.Fire } &&
            s.Events<PlayerMarkerChangedEvent>().Count(e => e.PlayerSeat == 0 && e.Marker == PlayerMarkerKind.GuanduGrain && e.Delta == -1) == 2 &&
            s.Events<GuanduGeneralIssuedEvent>().Count(e => e.OwnerSeat == 0 && e.SkillId == Cangchu) == 3,
            "One real nonlethal two-point fire hit runs both per-damage-point Cangchu occurrences and removes exactly two grain.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Sushou);
        Need(s.G.State.Phase == TurnPhase.Discard && s.Events<PhaseChangedEvent>().Count(e => e.ActorSeat == 0 && e.Phase == TurnPhase.Discard) == 1,
            "Sushou is offered on this actual owner's first discard entry.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Sushou && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option") == "give"));
        var frame = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Sushou);
        var draws = s.Events<GuanduGeneralDrawEvent>().Single(e => e.FrameId == frame.Id);
        Need(draws is { Requested: 2, Actual: 2, RecipientSeat: 0 } && s.V(0).HandCount == 2 &&
            s.P!.Choices.Where(c => c.Parameters.GetValueOrDefault("option") == "give").All(c => c.Targets.SequenceEqual(new[] { ally.Seat })) &&
            s.P.Choices.Any(c => c.Cards.SequenceEqual(new[] { equipment }) && c.Targets.SequenceEqual(new[] { ally.Seat })) &&
            s.P.Choices.All(c => !c.Targets.Contains(enemy.Seat) && !c.Targets.Contains(0)),
            "With one grain the original Sushou frame draws two real cards and offers the equipped entity only to the actual teammate, despite opposing faction labels.");
        s.Answer(c => c.Cards.SequenceEqual(new[] { equipment }) && c.Targets.SequenceEqual(new[] { ally.Seat }));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == frame.Id && f.SkillId == Sushou &&
                f.GuanduGeneral is { Stage: "sushou-give" } r && !r.Seats!.Contains(ally.Seat)) &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(w => w.Batch.ParentFrameId == frame.Id) &&
            s.V(0).Equipment.All(c => c.Id != equipment) && s.V(ally.Seat).Hand.Any(c => c.Id == equipment) &&
            s.Events<GuanduGeneralCompletedEvent>().All(e => e.FrameId != frame.Id) &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Equipment(0) &&
                m.To == CardLocation.Hand(ally.Seat) && m.Reason.Value == GiftReason) == 1,
            "The equipped original entity is really given once, and its equipment-loss movement child holds the same Sushou frame before the original discard phase resumes.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Sushou);
        Need(s.P!.Choices.Count == 1 && s.P.Choices.Single().Parameters.GetValueOrDefault("option") == "skip" &&
            s.P.Choices.Single().Targets.Count == 0,
            "Returning from the real gift child cannot offer the already-served teammate again or substitute a same-faction enemy.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option") == "skip");
        s.Until(() => s.P is { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 });
        Need(s.G.State.Phase == TurnPhase.Discard && s.G.State.CurrentSeat == 0 && s.Grain == 1 &&
            s.Events<PhaseChangedEvent>().Count(e => e.ActorSeat == 0 && e.Phase == TurnPhase.Discard) == 1 &&
            s.Events<GuanduGeneralDrawEvent>().Count(e => e.FrameId == frame.Id && e.Requested == 2 && e.Actual == 2) == 1 &&
            s.Events<GuanduGeneralCompletedEvent>().Count(e => e.FrameId == frame.Id && e.Completed) == 1 &&
            s.Events<TurnEndedEvent>().All(e => e.ActorSeat != 0) &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.To == CardLocation.Hand(ally.Seat) && m.Reason.Value == GiftReason) == 1,
            "The one paid Sushou returns to the original actual discard decision exactly once, without another draw, gift, phase entry or completed turn.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public int Grain => V(0).Markers?.SingleOrDefault(m => m.Kind == PlayerMarkerKind.GuanduGrain)?.Count ?? 0;
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected source-grain command."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        { var p = P!; var c = p.Choices.Single(predicate); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c.Id, G.Revision)); }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> ready)
        {
            for (var i = 0; i < 240; i++)
            {
                if (ready()) return;
                Need(G.State.Winner == Winner.None, "The grain fixture stays within one bounded live rule flow.");
                if (P is { } p && !(p.Kind == DecisionKind.PlayCard && G.ResolutionStack.Count != 0))
                    throw new InvalidOperationException("Unexpected source-grain prompt: " + JsonSerializer.Serialize(new { p.Kind, p.PlayerSeat, p.SkillPrompt?.SkillId }));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source grain exceeded the bounded native flow.");
        }
        public static Scene Start()
        {
            var setup = new ShanheBattleSetup(257, 1, "fixture:source-grain", StandardShanheSourceCoalitionTrioPackage.QuartermasterHardId,
                3, 0, 0, 0, 0, [Driver, Watch], [new(CardKind.Crossbow, Suit.Club, 1)], [],
                [new("classic:lu-bu") { StartingHand = [] }], [new("classic:guan-yu") { StartingHand = [] }], "fixture:source-grain")
            { ExtraSkillSlots = 2 };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] }).WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var s = new Scene(GameEngine.CreateStandard(new()
            { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, ConfirmOpeningHand = false,
                UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 2 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-grain", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[]
            {
                new { id = Driver, revision = 1, activations = new[] { new { id = "fire-two", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0,
                    targetKind = "anyLiving", usesPerTurn = 1, effects = new[] { new { op = "damage", target = "owner", amount = 2, nature = "fire" } } } } },
                new { id = Watch, revision = 1, triggers = new[] { new { id = "original-equipment-gift", window = "cardsMoved", subject = "owner",
                    sourceZones = new[] { "equipment" }, movementOccurrence = "perBatch", movementReasons = new[] { GiftReason }, optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "return", options = new[] { new { id = "continue" } } } } } } }
            } }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
            {
                [Driver] = new { name = "真实两点火伤", description = "在原伤害帧内实际受到两点火焰伤害。" },
                [Watch] = new { name = "实际装备给牌孩子", description = "宿守的真实装备离场后返回原分配流程。",
                    optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
            } }));
            foreach (var (id, p) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
            { Program = p, ProgramPresentation = catalog.Presentations[id] });
            b.AddDeck(new(Deck, "零自然起手摸牌的实际牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 5)).ToArray() });
        }
    }
    private static void Need(bool yes, string message) { if (!yes) throw new InvalidOperationException(message); }
}
