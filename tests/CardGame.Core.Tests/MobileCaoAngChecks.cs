using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileCaoAngChecks
{
    private const string Hero = "mobile:cao-ang", Skill = "mobile:kangkai", Owner = "fixture:mobile-cao-ang-owner", Attacker = "fixture:mobile-cao-ang-attacker", Mode = "identity:classic-mobile-cao-ang",
        Quiet = "fixture:mobile-cao-ang-quiet", Driver = "fixture:mobile-cao-ang-multi", Paid = "fixture:mobile-cao-ang-gift", Prepare = "fixture:mobile-cao-ang-prepare";
    private const string GiftReason = "skill-program.mobile:kangkai.SelectAndMoveOwnedCard";

    public static void MultiTargetColdSlashSelfDrawPublicGiftAndRecipientEquipmentReturnCold()
    {
        var s = Scene.Start(); s.AtPlay(0); var equipment = s.V().Hand.Single(c => c.Kind == CardKind.Crossbow).Id;
        s.PrepareMaterials(); s.AtPlay(0); var ownerHp = s.V().Hp; var allyHp = s.V(1).Hp;
        var openingDraws = s.E<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count);
        s.Need(s.V().Hand.Count == 1 && s.Zone(equipment) == CardLocation.Hand(0) && s.V(3).Hand is [{ Kind: CardKind.ColdSlash }] && ownerHp == 5 && allyHp == 4,
            "The supported four-human identity table deals four fixed entities to its Lord, then native commands stock two future draws and give the real Cold Slash to its actual attacker, leaving only Cao Ang's hand equipment.");
        s.End(); s.AtPlay(3); var slash = s.V(3).Hand.Single(c => c.Kind == CardKind.ColdSlash).Id;
        var action = s.G.GetLegalActions(3).Single(a => a.CardId == slash && a.Kind == LegalActionKind.Slash && a.TargetSeats.SequenceEqual([0, 1]));
        s.Accept(new PlayCardCommand(3, slash, action.TargetSeats, s.G.Revision, s.P!.PromptId, action.PlayedCardKind));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Paid, equipment);
        var root = s.Root; var id = root.Id; var use = s.G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == slash);
        var gift = root.CardSetBindings.Single(b => b.Name == "gift");
        s.Need(root.TriggerId == "nearby-trigger" && root.OwnerSeat == 0 && root.InstructionIndex == 3 && root.SelectedTargetSeats.SequenceEqual([1]) &&
            root.WindowContext is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, TargetSeat: 1 } && root.PendingMovementContinuation is not null &&
            use is { SourceSeat: 3, CardKind: CardKind.ColdSlash } && use.TargetSeats.SequenceEqual([0, 1]) &&
            gift.Visibility == SkillProgramCardSetVisibility.Public && gift.CardIds.SequenceEqual([equipment]) && s.Zone(equipment) == CardLocation.Hand(1) && s.PaidGiftOnce(equipment) &&
            s.E<ProgramCardsRevealedEvent>().Count(e => e.FrameId == id && e.SkillId == Skill && e.Bind == "gift" && e.Cards.Any(c => c.Id == equipment)) == 1 &&
            !s.E<CardUseDeclaredEvent>().Any(e => e.CardId == equipment) && !s.E<DamageAppliedEvent>().Any() && s.V().Hp == ownerHp && s.V(1).Hp == allyHp,
            "The actual multi-target Cold Slash has a distinct nearby ally offer despite also targeting Cao Ang: the original frame draws, publicly gives its real hand equipment, then waits on that paid gift child before equipment use or Slash damage.");
        s.Cold(); s.Continue();
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill && s.P.PlayerSeat == 1 && s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "use-equipment"), equipment);
        s.Need(s.Root.Id == id && s.Zone(equipment) == CardLocation.Hand(1) && s.G.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Id == use.Id) &&
            s.PaidGiftOnce(equipment),
            "Cold gift return retains the same public entity and original multi-target parent; the actual recipient owns the optional equipment-use decision without another gift payment.");
        s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "use-equipment"); s.AtPlay(3, equipment);
        s.Need(s.V().Hp == ownerHp - 1 && s.V(1).Hp == allyHp - 1 && s.V().HandCount == 2 && s.V(1).Equipment.Any(c => c.Id == equipment && c.Kind == CardKind.Crossbow) &&
            s.E<CardsDrawnEvent>().Where(e => e.PlayerSeat == 0).Sum(e => e.Count) - openingDraws == 2 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.SkillId == Skill && e.Activated && e.Completed) == 2 &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == id && e.Completed) == 1 &&
            s.E<ProgramCardsRevealedEvent>().Count(e => e.SkillId == Skill && e.Cards.Any(c => c.Id == equipment)) == 1 &&
            s.PaidGiftOnce(equipment) &&
            !s.G.CardMovements.Any(m => m.From == CardLocation.Hand(0) && m.To == CardLocation.Hand(0)) &&
            s.E<CardUseDeclaredEvent>().Count(e => e.SourceSeat == 1 && e.CardId == equipment && e.CardKind == CardKind.Crossbow) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.CardId == equipment && e.CardKind == CardKind.Crossbow) == 1 &&
            s.E<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == slash && e.CardKind == CardKind.ColdSlash) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(3) && m.To == CardLocation.Processing) == 1 &&
            s.E<DamageAppliedEvent>().Where(e => e.SourceSeat == 3 && e.Nature == DamageNature.Cold).Select(e => (e.TargetSeat, e.Amount)).OrderBy(e => e.TargetSeat).SequenceEqual([(0, 1), (1, 1)]) &&
            s.G.ResolutionStack.All(f => f.Id != id && f.Id != use.Id),
            "The recipient truly uses the gifted equipment once before the original Cold Slash completes once against both actual targets; Cao Ang draws for self and ally exactly once, never self-gifts or repeats the paid public transfer.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat = 0) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public ProgramSkillFrame Root => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int card) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location;
        public bool PaidGiftOnce(int card)
        {
            var moves = G.CardMovements.Where(m => m.CardId == card && m.Reason.Value == GiftReason).OrderBy(m => m.Sequence).ToArray();
            return moves is [var release, var receive] && release.From == CardLocation.Hand(0) && release.To == CardLocation.Processing &&
                receive.From == CardLocation.Processing && receive.To == CardLocation.Hand(1) && release.Sequence < receive.Sequence;
        }
        public void AtPlay(int seat, int equipment = -1) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat, equipment);
        public void End() { var p = P!; Accept(new EndPlayPhaseCommand(p.PlayerSeat, G.Revision, p.PromptId)); }
        public void PrepareMaterials()
        {
            Accept(new UseProgramSkillCommand(0, Prepare, "stock-draws", V().Hand.Where(c => c.Kind == CardKind.Slash).Select(c => c.Id).ToArray(), [], G.Revision, P!.PromptId));
            AtPlay(0);
            Accept(new UseProgramSkillCommand(0, Prepare, "supply-cold", [V().Hand.Single(c => c.Kind == CardKind.ColdSlash).Id], [3], G.Revision, P!.PromptId));
        }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Answer(Func<PromptChoice, bool> match) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(match).Id, G.Revision)); }
        public void Reach(Func<bool> ready, int equipment = -1)
        {
            for (var i = 0; i < 220; i++)
            {
                if (ready()) return;
                if (P is { Kind: DecisionKind.SelectGeneral } select) { Accept(new SelectGeneralCommand(select.PlayerSeat, select.PlayerSeat == 0 ? Owner : select.PlayerSeat == 3 ? Attacker : Peer(select.PlayerSeat), G.Revision, select.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard }) { End(); continue; }
                if (P?.SkillPrompt?.SkillId == Skill)
                {
                    if (P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate")) { Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); continue; }
                    if (P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target")) { Answer(c => c.Targets.SequenceEqual([1])); continue; }
                    if (P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card")) { Answer(c => c.Cards.SequenceEqual([equipment])); continue; }
                }
                if (P is { Kind: DecisionKind.RespondDodge }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage"); continue; }
                if (P?.Choices.Any(c => c.Parameters.GetValueOrDefault("action") == "cold-damage") == true) { Answer(c => c.Parameters.GetValueOrDefault("action") == "cold-damage"); continue; }
                Need(P is null, "Unexpected mobile Cao Ang native boundary."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded actual Cao Ang scene missed its original multi-target child boundary.");
        }
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves the original public gift, already-paid movement, private recipient and actual multi-target Cold Slash frames.");
        }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Cao Ang command."); }
        public void Need(bool ok, string why) { if (!ok) throw new InvalidOperationException(why + " " + JsonSerializer.Serialize(new { P, G.State.CurrentSeat, G.State.Phase, Players = Enumerable.Range(0, 4).Select(V).Select(p => new { p.Seat, p.Hp, p.HandCount }), Frames = G.ResolutionStack.Select(f => new { f.Id, f.Kind }) })); }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var engine = GameEngine.CreateStandard(new() { Seed = 59, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var s = new Scene(engine, registry); s.Accept(new StartGameCommand()); return s;
        }
    }

    private static string Peer(int seat) => "fixture:mobile-cao-ang-peer-" + seat;

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-cao-ang", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            object Pause() => new { op = "chooseOption", target = "owner", resultBind = "paid", options = new[] { new { id = "continue" } } };
            var skills = new object[] {
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "quiet", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false, effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Prepare, revision = 1, modifiers = new[] { new { id = "fixed-four-opening-entities", query = "initialHandSize", operation = "add", value = 4, priority = 0 } },
                    activations = new object[] {
                        new { id = "stock-draws", minCards = 2, maxCards = 2, sourceZones = new[] { "hand" }, cardKinds = new[] { "slash" }, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null, usesPerGame = (int?)null,
                            effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "prepared" }, new { op = "moveBoundCards", target = "owner", sourceBind = "prepared", destination = "drawPileTop", awaitMovementTriggers = true } } },
                        new { id = "supply-cold", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, cardKinds = new[] { "coldSlash" }, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null, usesPerGame = (int?)null,
                            effects = new[] { new { op = "giveSelected", target = "selectedTarget", amount = 1 } } } } },
                new { id = Driver, revision = 1, modifiers = new object[] {
                    new { id = "actual-two-targets", query = "cardTargetCount", operation = "add", value = 1, priority = 0, cardKinds = new[] { "coldSlash" } },
                    new { id = "actual-target-distance", query = "slashDistanceLimit", operation = "unlimited", value = 0, priority = 0 } } },
                new { id = Paid, revision = 1, triggers = new[] { new { id = "actual-public-gift", window = "cardsGained", subject = "owner", optional = false, isSkillInvocation = false,
                    destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { GiftReason }, effects = new[] { Pause() } } } }
            };
            var labels = new Dictionary<string, object> { [Quiet] = new { name = "固定开局", description = "不改变初始手牌" }, [Prepare] = new { name = "原生准备实体", description = "真实发牌后放回两张杀，并将冰杀交给实际用牌者" }, [Driver] = new { name = "真实双目标冰杀", description = "原生目标与距离政策" },
                [Paid] = new { name = "原慷忾交牌孩子", description = "真实公开交付后暂停", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Owner, "移动曹昂固定身份", "supporter", Skill, "wei", 4, AdditionalSkillIds: [Quiet, Prepare], Gender: GeneralGender.Male));
            foreach (var seat in new[] { 1, 2 }) b.AddGeneral(new(Peer(seat), "固定邻近同桌", "supporter", Quiet, "wei", 4, [Paid]));
            b.AddGeneral(new(Attacker, "原生双目标用牌者", "supporter", Driver, "shu", 4, [Quiet]));
            b.AddCard(new("fixture:mobile-cao-ang-cold", "冰杀", "基本牌", CardCatalog.Get(CardKind.ColdSlash).Description, CardKind.ColdSlash));
            const string deck = "fixture:mobile-cao-ang-deck";
            b.AddDeck(new(deck, "固定慷忾摸牌", 0, 2, []) { PhysicalCards = [new("standard:crossbow", Suit.Club, 5), new("fixture:mobile-cao-ang-cold", Suit.Spade, 7), new("standard:slash", Suit.Heart, 7), new("standard:slash", Suit.Heart, 8)] });
            b.AddMode(new(Mode, "移动曹昂逐目标原生慷忾", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Attacker]));
        }
    }
}
