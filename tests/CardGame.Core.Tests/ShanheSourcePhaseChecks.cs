using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Standard;

internal static class ShanheSourcePhaseChecks
{
    private const string Jushou = StandardShanheSourceYulinGuardPackage.JushouSkillId;
    private const string Shengxi = StandardShanheSourceBuHengPackage.ShengxiSkillId;
    private const string Deck = "fixture:source-phase-deck", Watch = "fixture:source-phase-equipment-child", Driver = "fixture:source-phase-damage";

    public static void JushouRunsAfterHandLimitDiscardAndUsesTheOriginalEquipmentAtTurnEnding()
    {
        var s = Scene.Start(jushou: true);
        Need(s.V.HandCount == 6 && s.V.HandLimit == 4 && !s.V.IsFaceDown, "Fresh source370 starts over its real four-card hand limit.");
        s.End(); s.Until(() => s.P is { Kind: DecisionKind.DiscardCards });
        var discard = s.G.ResolutionStack.OfType<ManualDiscardFrame>().Single();
        Need(s.P!.RequiredCardCount == 2 && !s.Events<ProgramBindingStartedEvent>().Any(e => e.SkillId == Jushou),
            "Source370 does not activate at PlayEnding: the actual excess-hand discard must run first.");
        var paid = s.P.ValidCardIds.Take(2).ToArray();
        s.Accept(new DiscardCardsCommand(0, paid, s.P.PromptId, s.G.Revision));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Jushou);
        var boundary = s.G.ResolutionStack.OfType<TurnEndingBoundaryFrame>().Single();
        Need(s.V.HandCount == 4 && !s.G.ResolutionStack.Any(f => f.Id == discard.Id) &&
            paid.All(id => s.G.CardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.HandLimitDiscard) == 1),
            "Both real excess cards finish their original discard before the one native TurnEnding offer.");
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Until(() => s.P?.SkillPrompt?.SkillId == Jushou && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "strategic-choice"));
        var producer = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Jushou);
        var equipment = s.V.Hand.First(c => c.Kind == CardKind.Crossbow).Id;
        Need(producer.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } context && context.ParentFrameId == boundary.Id &&
            s.V.IsFaceDown && s.V.HandCount == 8 && s.V.HandLimit == 4 && producer.StrategicDraft?.Op == SkillProgramEffectOp.DiscardHandOrUseEquipment &&
            s.P!.Choices.All(c => c.Cards.Count == 1),
            "The original ending frame owns the flip, native four-card draw and mandatory hand-card selection; no new discard phase begins.");
        s.Answer(c => c.Cards.SequenceEqual([equipment]));
        s.Until(() => s.P?.SkillPrompt?.SkillId == Watch);
        var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == equipment);
        Need(s.G.ResolutionStack.Any(f => f.Id == boundary.Id) && s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == producer.Id) &&
            use.Action is { Type: CardActionType.Use, ActorSeat: 0, PhysicalCards.Count: 1 } action && action.PhysicalCards.Single().CardId == equipment &&
            action.ConversionChain.Any(c => c.SkillId == Jushou && c.SkillInstanceId == producer.SkillInstanceId) &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "Choosing real equipment starts its original native Use and holds the same ending producer while its actual target-program child runs.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.NextHuman();
        Need(s.V.IsFaceDown && s.V.HandCount == 7 && s.V.HandLimit is null && s.V.Equipment.Single(c => c.Kind == CardKind.Crossbow).Id == equipment &&
            s.Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == producer.Id && e.SkillId == Jushou && e.Window == SkillProgramTriggerWindow.TurnEnding && e.Completed) == 1 &&
            s.Events<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == equipment) == 1 &&
            s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == equipment) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == equipment && m.From == CardLocation.Processing && m.To == CardLocation.Equipment(0)) == 1 &&
            s.G.CardMovements.Count(m => m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.HandLimitDiscard) == 2 &&
            s.Events<TurnEndedEvent>().Count(e => e.TurnNumber == 1 && e.ActorSeat == 0) == 1 && !s.G.ResolutionStack.Any(f => f.Id == producer.Id || f.Id == boundary.Id || f.Id == use.Id),
            "The mandatory original equipment is used and installed once; the original ending completes once with seven hand cards and never discards again after Jushou's draw. " +
            JsonSerializer.Serialize(new { s.V.IsFaceDown, s.V.HandCount, s.V.HandLimit, s.V.Equipment,
                ProducerId = producer.Id, BoundaryId = boundary.Id, UseId = use.Id,
                Bindings = s.Events<ProgramBindingResolvedEvent>().Where(e => e.SkillId == Jushou),
                Declared = s.Events<CardUseDeclaredEvent>().Where(e => e.CardId == equipment),
                Finished = s.Events<CardUseFinishedEvent>().Where(e => e.CardId == equipment),
                EquipmentMoves = s.G.CardMovements.Where(m => m.CardId == equipment),
                HandLimitMoves = s.G.CardMovements.Where(m => m.Reason == CardMoveReasons.HandLimitDiscard),
                TurnsEnded = s.Events<TurnEndedEvent>(), Stack = s.G.ResolutionStack.Select(f => new { Type = f.GetType().Name, f.Id }) }));

        var hell = Scene.Start(jushou: true, hell: true);
        Need(hell.V.IsEquipmentAreaAbolished && hell.V.Equipment.Count == 0 && hell.V.HandCount == 10 && hell.V.HandLimit == 8,
            "The real Hell2498 opening Qiongshou abolishes every slot, draws four and adds four to the hand limit before the first human Play.");
        hell.End(); hell.Until(() => hell.P is { Kind: DecisionKind.DiscardCards });
        var hellDiscard = hell.V.Hand.Where(c => c.Kind == CardKind.Dodge).Take(2).Select(c => c.Id).ToArray();
        Need(hell.P!.RequiredCardCount == 2, "The real Hell opening still owes its original two excess hand cards.");
        hell.Accept(new DiscardCardsCommand(0, hellDiscard, hell.P.PromptId, hell.G.Revision));
        hell.Until(() => hell.P?.SkillPrompt?.SkillId == Jushou);
        var hellBoundary = hell.G.ResolutionStack.OfType<TurnEndingBoundaryFrame>().Single().Id;
        hell.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        hell.Until(() => hell.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "strategic-choice") == true);
        var hellProducer = hell.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Jushou);
        Need(hell.V.IsFaceDown && hell.V.HandCount == 12 && hell.V.HandLimit == 8 &&
            hellProducer.WindowContext?.ParentFrameId == hellBoundary && hell.V.Hand.Count(c => c.Kind == CardKind.Crossbow) == 8 &&
            hell.P!.Choices.Count > 0 && hell.P.Choices.All(c => c.Cards.Count == 1 && hell.V.Hand.Single(h => h.Id == c.Cards.Single()).Kind == CardKind.Dodge),
            "After Qiongshou every drawn equipment is truly unusable: Jushou offers the remaining legal non-equipment hand cost and never declares a forbidden equipment Use.");
        var nonEquipment = hell.P.Choices.First().Cards.Single();
        hell.Answer(c => c.Cards.SequenceEqual([nonEquipment])); hell.NextHuman();
        Need(hell.V.IsEquipmentAreaAbolished && hell.V.Equipment.Count == 0 && hell.V.IsFaceDown && hell.V.HandCount == 11 &&
            hell.Events<CardUseDeclaredEvent>().All(e => e.SourceSeat != 0 || e.CardKind != CardKind.Crossbow) &&
            hell.Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == hellProducer.Id && e.Window == SkillProgramTriggerWindow.TurnEnding && e.Completed) == 1 &&
            hell.G.CardMovements.Count(m => m.CardId == nonEquipment && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile &&
                m.Reason.Value == "skill-program." + Jushou + ".jushou-discard") == 1 &&
            hell.G.CardMovements.Count(m => m.Reason == CardMoveReasons.HandLimitDiscard) == 2 &&
            !hell.G.ResolutionStack.Any(f => f.Id == hellBoundary || f.Id == hellProducer.Id),
            "The Hell ending pays one genuine legal non-equipment discard, keeps all eight unusable equipment entities in hand, avoids an illegal Use and returns from the original ending once.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 2).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public T[] Events<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, (r.Error?.Message ?? "Source phase native command rejected.") + " " + Diagnostic()); }
        public void Answer(Func<PromptChoice, bool> choose)
        { var p = P!; var c = p.Choices.FirstOrDefault(choose); Need(c is not null, "Missing exact source phase choice. " + Diagnostic()); Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision)); }
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && G.ResolutionStack.Count == 0);
        public void NextHuman() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 } && G.ResolutionStack.Count == 0);
        public void Until(Func<bool> stop)
        {
            for (var step = 0; step < 220; step++)
            {
                if (stop()) return;
                Need(G.State.Winner == Winner.None, "Source phase checks stop at the next human boundary. " + Diagnostic());
                if (P is { } p)
                {
                    var c = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip" || c.Parameters.GetValueOrDefault("response") is "pass" or "take-damage");
                    Need(c is not null, "Unexpected native source phase boundary. " + Diagnostic());
                    Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, c!.Id, G.Revision));
                }
                else Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Source phase exceeded bounded native progression. " + Diagnostic());
        }
        private string Diagnostic() => JsonSerializer.Serialize(new { G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase, G.State.Winner,
            Pending = P, Stack = G.ResolutionStack.Select(f => new { Type = f.GetType().Name, f.Id }),
            Recent = G.Events.TakeLast(8).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) });
        public static Scene Start(bool jushou, bool hell = false)
        {
            string[] skills = jushou ? [Watch] : [Shengxi, Driver];
            var setup = new ShanheBattleSetup(223, 1, "fixture:source-phase-" + jushou,
                jushou ? hell ? StandardShanheSourceYulinGuardPackage.HellId : StandardShanheSourceYulinGuardPackage.BaseId : "classic:guan-yu", 4, 0, 0, 0, 0, skills,
                Enumerable.Range(0, jushou ? 6 : 4).Select(i => new ShanheStartingCard(CardKind.Dodge, Suit.Heart, i + 1)).ToArray(), [],
                [new("classic:guan-yu", MaxHpBonus: 16, Hp: 20) { StartingHand = [] }], [], "fixture:source-phase");
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = jushou ? [] : [Shengxi] }).WithExtension(new Fixture(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanSeats = [0, 1],
                HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, registry));
            s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-phase", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            b.AddModeFeatureFactory(new HumanSetupFactory()); b.AddModeFeatureFactory(new HumanProfileFactory());
            var skills = new object[]
            {
                new { id = Watch, revision = 1, triggers = new[] { new { id = "real-equipment-use", window = "cardUseTargetsFinalized", ownerRelation = "actor", optional = false,
                    cardCategories = new[] { "equipment" }, effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "return", options = new[] { new { id = "continue" } } } } } } },
                new { id = Driver, revision = 1, activations = new[] { new { id = "hurt", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1,
                    targetKind = "otherLiving", usesPerTurn = 1, effects = new object[] { new { op = "damage", target = "selectedTarget", amount = 1 } } } } }
            };
            var labels = new Dictionary<string, object>
            { [Watch] = new { name = "实际装备使用孩子", description = "原生装备使用时返回同一据守结束帧。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } },
                [Driver] = new { name = "当前回合真实伤害", description = "造成一点原生技能伤害。" } };
            var programs = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in programs.Programs) b.AddSkill(new(id, programs.Presentations[id].Name, programs.Presentations[id].Description)
            { Program = p, ProgramPresentation = programs.Presentations[id] });
            b.AddDeck(new(Deck, "零自然起手摸牌的真实装备牌堆", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(i => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, i % 13 + 1)).ToArray() });
        }
    }
    private sealed class HumanSetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:phase-human-setup"; public string ModeId => ShanheBattleSetup.BattleModeId;
        public void ValidateInitialOptions(GameOptions o, ModeConfigurationValidationPhase p) => new StandardModeSetupFactory().ValidateInitialOptions(o, p);
        public IGameModeFeature Create(ModeFeatureCreationContext c) => new HumanSetup((IMatchModeSetupFeature)new StandardModeSetupFactory().Create(c));
    }
    private sealed class HumanProfileFactory : IGameModeProfileFeatureFactory
    {
        public string Id => "fixture:phase-human-profile"; public string ModeId => ShanheBattleSetup.BattleModeId;
        public IGameModeFeature Create(ModeFeatureCreationContext c) => new StandardModeProfileFactory().Create(new ModeFeatureCreationContext(c.Options! with { HumanSeats = null },
            c.ModeDefinition!, c.ContentRegistry, new ModeHostRegistry().Register<IModeSetupRandom>(() => c.GetHost<IModeSetupRandom>())
                .Register<IModeNationalRevealHost>(() => c.GetHost<IModeNationalRevealHost>()).Register<IModeSetupEquipmentHost>(() => c.GetHost<IModeSetupEquipmentHost>())));
    }
    private sealed class HumanSetup(IMatchModeSetupFeature inner) : IMatchModeSetupFeature
    {
        public ModeSetupPlan BuildPlayers() { var p = inner.BuildPlayers(); return new(p.Seats.Select(s => s with { IsHuman = true }), p.AvailableGeneralIds,
            p.SelectionOrder, p.ShenwuSharedGodIds, p.ShenwuLeaderGeneralId, p.GuanduConfiguration, p.InitialActorSeat); }
        public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest r) => inner.GetGeneralOffers(r);
        public IReadOnlyList<string> RemoveGeneralIdsAfterSelection(string id) => inner.RemoveGeneralIdsAfterSelection(id);
        public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> d) => inner.PrepareDeck(d);
        public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> d) => inner.ProjectDeckCardNames(d);
        public IReadOnlyList<Type> InitialEquipmentCompletionFacts => inner.InitialEquipmentCompletionFacts;
        public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> d, IReadOnlyList<ModeSeatSetupFacts> s) => inner.PrepareSelectedEquipment(d, s);
        public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts s, string id) => inner.PrepareSelectedGeneral(s, id);
    }
    private static void Need(bool yes, string message) { if (!yes) throw new InvalidOperationException(message); }
}
