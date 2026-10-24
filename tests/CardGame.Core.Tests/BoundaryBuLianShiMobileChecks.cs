using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryBuLianShiMobileChecks
{
    private const string Hero = "boundary:bu-lian-shi-mobile", Anxu = "boundary:anxu-mobile", Zhuiyi = "boundary:zhuiyi-mobile", Owner = "fixture:boundary-bu-lian-shi-owner",
        Driver = "fixture:boundary-bu-lian-shi-driver", Watch = "fixture:boundary-bu-lian-shi-paid", Empty = "fixture:boundary-bu-lian-shi-empty", Hp = "fixture:boundary-bu-lian-shi-hp", Benefit = "fixture:boundary-bu-lian-shi-benefit", Mode = "identity:classic-boundary-bu-lian-shi";
    private const string Obtain = "skill-program." + Anxu + ".SelectAndMoveOwnedCard";

    public static void RecipientPrivateObtainMovedAwayKeepsOriginalHandRewardCold()
    {
        var s = Scene.Start(emptyRecipient: true); s.Play(); s.Use("draw-two", 2); s.Play();
        var ownerHand = s.V(0).HandCount; var donorHand = s.V(3).HandCount;
        var action = s.G.GetLegalActions(0).Single(a => a.ProgramSkillId == Anxu);
        var pairs = action.SelectableTargetSequences ?? [];
        s.Need(pairs.Any(p => p.SequenceEqual([2, 3])) && s.V(2).HandCount > donorHand && pairs.All(p => p.Count == 2 && p[0] != 0 && p[1] != 0 && p[0] != p[1]),
            "The mobile ordered activation permits a recipient with more hand cards than its donor; both are other distinct roles and the first seat is the recipient.");
        s.BeginAnxu(2, 3); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        s.Need(s.P!.PlayerSeat == 2 && s.P.Choices.Where(c => c.Parameters.GetValueOrDefault("source-zone") == "Hand").All(c => c.Cards.Count == 0) && s.G.CreateSnapshot(0).PendingDecision is null,
            "The recipient, rather than Bu Lian Shi or the donor, owns the genuine private choice; another player's hand is represented by opaque slots.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("source-zone") == "Hand"); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Anxu); var frame = root.Id; var r = root.BoundaryBuLianShiAnxu!; var card = r.CardId;
        s.Need(r is { Delivered: true, From.Zone: CardZoneKind.Hand, RecipientSeat: 2, DonorSeat: 3 } && s.V(2).Hand.Any(c => c.Id == card) && s.V(0).HandCount == ownerHand && s.E<CardsDrawnEvent>().Last().PlayerSeat == 2,
            "The original HE-to-processing and processing-to-recipient invoices freeze the hand source before any owner reward, while its actual payment child remains pending.");
        s.Cold(); s.Answer(_ => true); s.Reach(() => s.P?.SkillPrompt?.SkillId == Empty); s.Answer(_ => true);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "anxu-benefit") == true);
        s.Need(s.V(2).HandCount == 0 && s.V(2).Hand.All(c => c.Id != card) && s.V(0).HandCount == ownerHand + 1 && s.V(3).HandCount == donorHand - 1 &&
            s.E<BoundaryBuLianShiAnxuLeastEvent>().Single() is { RecipientSeat: 2, DonorSeat: 3, RecipientHand: 0, LeastSeat: 2 } &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Anxu).Id == frame,
            "A real gain child may move the obtained entity away and alter the pair's hands; the owner still draws for its committed hand origin, then compares actual post-child hand counts.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "draw"); s.Play();
        s.Need(s.V(2).HandCount == 1 && s.V(0).HandCount == ownerHand + 1 && s.G.GetLegalActions(0).All(a => a.ProgramSkillId != Anxu) &&
            s.G.CardMovements.Count(m => m.CardId == card && m.From == CardLocation.Hand(3) && m.To == CardLocation.Processing && m.Reason.Value == Obtain) == 1,
            "The optional lower-hand reward draws exactly once for the original recipient, retains the original per-Play use and never repeats its paid Obtain after cold restore.");
    }

    public static void EquipmentRecoveryCurrentCountsEqualAndActualDeathBenefitCold()
    {
        var s = Scene.Start(); s.Play(); s.Use("equip", 3); s.Play(); s.Use("hurt", 3); s.Play();
        var lion = s.V(3).Equipment.Single(c => c.Kind == CardKind.SilverLion).Id; var ownerHand = s.V(0).HandCount;
        s.BeginAnxu(2, 3); s.Reach(() => s.P?.Choices.Any(c => c.Cards.SequenceEqual(new[] { lion })) == true); s.Answer(c => c.Cards.SequenceEqual([lion]));
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Anxu);
        s.Need(root.BoundaryBuLianShiAnxu is { From.Zone: CardZoneKind.Equipment, Delivered: true, LeastSelected: false } && s.V(3).Hp == s.V(3).MaxHp && s.V(0).HandCount == ownerHand &&
            s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any(h => h.ResumeFrameId == root.Id && h.Change.ParentFrameId == root.Id),
            "Obtaining a real equipped Silver Lion processes its native recovery on the original frame before source classification rewards or the lower-hand comparison.");
        s.Cold(); s.Answer(_ => true); s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "anxu-benefit") == true);
        s.Need(s.V(0).HandCount == ownerHand && s.E<BoundaryBuLianShiAnxuLeastEvent>().Single() is { LeastSeat: 3 },
            "An equipment-origin Obtain gives no owner draw; only the two original roles' real hand counts decide the optional beneficiary.");
        var donor = s.V(3).HandCount; s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "draw"); s.Play();
        s.Need(s.V(3).HandCount == donor + 1 && s.V(0).HandCount == ownerHand, "The smaller original donor may receive the optional draw even though it did not obtain the equipment.");

        var equal = Scene.Start(); equal.Play(); equal.Use("draw-one", 3); equal.Play(); equal.Use("equip", 3); equal.Play();
        var equalLion = equal.V(3).Equipment.Single(c => c.Kind == CardKind.SilverLion).Id;
        equal.BeginAnxu(2, 3); equal.Reach(() => equal.P?.Choices.Any(c => c.Cards.SequenceEqual(new[] { equalLion })) == true); equal.Answer(c => c.Cards.SequenceEqual([equalLion]));
        equal.Reach(() => equal.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("result-bind") == "anxu-benefit") == true);
        equal.Need(equal.V(2).HandCount == equal.V(3).HandCount && equal.P!.Choices is [{ Parameters: var parameters }] && parameters.GetValueOrDefault("option-id") == "skip",
            "Equal hands contain no strictly smaller original participant, so no optional draw is offered to an arbitrary tied role."); equal.Answer(_ => true); equal.Play();

        var death = Scene.Start(deathOwner: true); death.Play(); death.Use("hurt", 2); death.Play(); var hand = death.V(2).HandCount; var hp = death.V(2).Hp;
        death.Use("kill-owner", 1); death.Reach(() => death.P?.SkillPrompt?.SkillId == Zhuiyi); death.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        death.Reach(() => death.P?.Choices.Any(c => c.Targets.Count == 1) == true);
        death.Need(!death.V(0).IsAlive && death.P!.Choices.Where(c => c.Targets.Count == 1).All(c => c.Targets[0] is not (0 or 1)),
            "The actual dead mobile owner chooses a living beneficiary after death; both self and the actual native damage killer are excluded, without an earlier fixed beneficiary.");
        death.Answer(c => c.Targets.SequenceEqual([2])); death.Reach(() => death.P?.SkillPrompt?.SkillId == Benefit);
        death.Need(death.V(2).HandCount == hand + 3 && death.V(2).Hp == hp && death.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Zhuiyi && f.OwnerSeat == 0),
            "The dead owner's real death-trigger frame issues three actual cards before recovery and waits on that draw's native gain child.");
        death.Cold(); death.Answer(_ => true); death.Reach(() => death.G.ResolutionStack.Count == 0 && death.P is { Kind: DecisionKind.PlayCard });
        death.Need(death.V(2).Hp == hp + 1 && death.V(2).HandCount == hand + 3 && death.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Zhuiyi) == 1,
            "Cold returning the native death-benefit draw gives exactly one real recovery without repeating the three-card benefit or reselecting its recipient.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Use(string id, params int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void BeginAnxu(int recipient, int donor) => Accept(new UseProgramSkillCommand(0, Anxu, "recipient-obtains-donor", [], [recipient, donor], G.Revision, P!.PromptId));
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected mobile Bu Lian Shi fixture command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 220 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } p) { Accept(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : "fixture:boundary-bu-lian-shi-peer-" + p.PlayerSeat, G.Revision, p.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard, PlayerSeat: not 0 } foreign) { Accept(new EndPlayPhaseCommand(foreign.PlayerSeat, G.Revision, foreign.PromptId)); continue; }
                if (P?.SkillPrompt?.SkillId is Watch or Hp or Benefit) { Answer(_ => true); continue; }
                if (P is { Kind: DecisionKind.RescueDying }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "let-die"); continue; }
                if (P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true) { Answer(c => c.Cards.Count == 1); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger }) throw new InvalidOperationException("Unexpected mobile Bu Lian Shi boundary: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The bounded actual mobile Bu Lian Shi fixture missed its expected native boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves recipient-private Obtain, immutable original-source invoices, real equipment recovery and native death-benefit returns."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount, V(i).IsAlive }) })); }
        public static Scene Start(bool emptyRecipient = false, bool deathOwner = false)
        { var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture(emptyRecipient)); var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = deathOwner ? Role.Renegade : Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, r); var s = new Scene(r, g); s.Accept(new StartGameCommand()); return s; }
    }
    private sealed class Fixture(bool emptyRecipient) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-bu-lian-shi", new(1, 0, 0));
        internal static string Rules() => $$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
         {"id":"{{Driver}}","revision":1,"activations":[
          {"id":"draw-two","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"selectedTarget","amount":2}]},
          {"id":"draw-one","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"selectedTarget","amount":1}]},
          {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"equipped"}]},
          {"id":"hurt","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":1}]},
          {"id":"kill-owner","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":7}]}
         ]},
         {"id":"{{Watch}}","revision":1,"triggers":[
          {"id":"actual-hand-obtain","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{Obtain}}"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]},
          {"id":"actual-equipment-obtain","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["{{Obtain}}"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
         ]},
         {"id":"{{Empty}}","revision":1,"triggers":[{"id":"actual-recipient-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{Obtain}}"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"before-empty","options":[{"id":"continue"}]},{"op":"selectOwnedCards","target":"owner","numberExpression":"allOwnedZoneCards","zones":["hand"],"resultBind":"emptied"},{"op":"moveBoundCards","target":"owner","sourceBind":"emptied","destination":"discardPile","awaitMovementTriggers":true}]}]},
         {"id":"{{Hp}}","revision":1,"triggers":[{"id":"actual-recovery","window":"afterHpRecovered","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"recovered","options":[{"id":"continue"}]}]}]},
         {"id":"{{Benefit}}","revision":1,"triggers":[{"id":"actual-death-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:zhuiyi-mobile.Draw"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"death-drawn","options":[{"id":"continue"}]}]}]}
        ]}
        """;
        internal static string Presentation() => JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> { [Driver] = new { name = "真实原生命令", description = "真实摸牌装备失血伤害" }, [Watch] = Pause("原生获得付款"), [Empty] = Pause("获得后真实清手"), [Hp] = Pause("原生回复孩子"), [Benefit] = Pause("原生死后摸牌") } });
        private static object Pause(string name) => new { name, description = "真实原始父帧返回", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero); var c = SkillProgramCatalog.Load(Rules(), Presentation()); foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            b.AddCard(new("classic:silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            b.AddGeneral(new(Owner, "移动界步练师实际场景", "supporter", Anxu, "wu", 3, AdditionalSkillIds: [Zhuiyi, Driver, Watch, Hp, Benefit], Gender: GeneralGender.Female));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:boundary-bu-lian-shi-peer-" + i).ToArray(); foreach (var seat in Enumerable.Range(1, 4)) b.AddGeneral(new(peers[seat - 1], "固定实际角色", "supporter", "standard:none", "wu", 6, AdditionalSkillIds: seat == 2 && emptyRecipient ? [Watch, Hp, Benefit, Empty] : [Watch, Hp, Benefit], Gender: GeneralGender.Male));
            const string deck = "fixture:boundary-bu-lian-shi-deck"; b.AddDeck(new(deck, "真实杀和白银狮子", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 120).Select(i => new ContentDeckPhysicalCard(i % 5 == 0 ? "classic:silver-lion" : "standard:slash", i % 5 == 0 ? Suit.Spade : Suit.Heart, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "移动界步练师原生身份流程", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
