using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryLiRuMobileChecks
{
    private const string Hero = "boundary:li-ru-mobile", Mieji = "boundary:mieji-mobile", Juece = "boundary:juece-mobile", Fencheng = "boundary:fencheng-mobile",
        Owner = "fixture:boundary-li-ru-mobile-owner", Driver = "fixture:boundary-li-ru-mobile-driver", Watch = "fixture:boundary-li-ru-mobile-paid", Hp = "fixture:boundary-li-ru-mobile-hp", Top = "fixture:boundary-li-ru-mobile-top", Mode = "identity:classic-boundary-li-ru-mobile";
    private const string GiftReason = "skill.boundary-li-ru.mieji.give", DiscardReason = "skill.boundary-li-ru.mieji.discard", FireReason = "skill-program." + Fencheng + ".EscalatingDiscardOrDamage";

    public static void BlackTrickTopRealGiftAndActualTurnLostCardJueceCold()
    {
        var s = Scene.Start(); s.Play(0); s.Search("trick", 0); s.Search("trick", 1);
        var top = s.V(0).Hand.First(c => c.Kind == CardKind.Dismantlement && c.Suit == Suit.Spade).Id;
        var gift = s.V(1).Hand.First(c => c.Kind == CardKind.Dismantlement).Id;
        s.StartMieji(top, 1); s.Reach(() => s.P?.SkillPrompt?.SkillId == Top);
        var original = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Mieji).Id;
        s.Need(s.Moves("skill-program." + Mieji + ".MoveBoundCards") is [{ CardId: var paid, From.Zone: CardZoneKind.Hand, To.Zone: CardZoneKind.DrawPile }] && paid == top &&
            s.E<CardUseDeclaredEvent>().All(e => e.CardId != top) && s.E<BoundaryLiRuMiejiStartedEvent>().Length == 0,
            "Mieji shows and pays the actual selected black hand Trick to the real deck top, without using it or opening the target's branch before its paid movement child returns.");
        s.Cold(); s.Answer(_ => true); s.Branch("gift"); s.Card(gift); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        s.Need(s.V(0).Hand.Any(c => c.Id == gift) && s.V(1).Hand.All(c => c.Id != gift) && s.Moves(GiftReason) is [{ From: var from, To: var to }] && from == CardLocation.Hand(1) && to == CardLocation.Hand(0) &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Mieji).Id == original,
            "The selected target really gives its own chosen Trick entity into Li Ru's hand; this is not the OL discard-a-Trick branch and keeps the original paid frame.");
        s.Cold(); s.Answer(_ => true); s.Play(0); s.Need(s.G.GetLegalActions(0).All(a => a.ProgramSkillId != Mieji) && s.Moves(GiftReason).Length == 1,
            "Returning the real gift child keeps the per-Play use limit and cannot top-deck or give the same entity twice.");
        s.End(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Juece); s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        s.Reach(() => s.P?.Choices.Any(c => c.Targets.Count == 1) == true);
        s.Need(s.P!.Choices.Where(c => c.Targets.Count == 1).SelectMany(c => c.Targets).Distinct().SequenceEqual([1]) && s.V(1).HandCount > 0,
            "Only the target's actual hand-card loss this turn qualifies Juece; a nonempty hand is eligible, while untouched players cannot qualify from empty-hand or hand-comparison approximations.");
        var hp = s.V(1).Hp; s.Answer(c => c.Targets.SequenceEqual([1])); s.Play(1);
        s.Need(s.V(1).Hp == hp - 1 && s.E<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == 1 && e.Amount == 1) == 1,
            "The genuine ending-stage lost-card target receives one native damage.");
        foreach (var seat in new[] { 1, 2, 3, 4 }) { s.Play(seat); s.End(); } s.Play(0); s.End(); s.Play(1);
        s.Need(s.E<ProgramBindingStartedEvent>().Count(e => e.SkillId == Juece) == 1,
            "Actual losses from the previous owner turn cannot qualify a later ending with no new foreign hand/equipment loss.");
    }

    public static void SequentialNonTrickChildrenShortageAndFixedClockwiseFenchengCold()
    {
        var s = Scene.Start(); s.Play(0); s.Search("trick", 0); s.Search("equipment", 1); s.Use("equip", 1); s.Play(0); s.Use("hurt", 1); s.Play(0);
        var top = s.V(0).Hand.First(c => c.Kind == CardKind.Dismantlement).Id; var lion = s.V(1).Equipment.Single(c => c.Kind == CardKind.SilverLion).Id;
        var second = s.V(1).Hand.First(c => c.Kind == CardKind.SilverLion).Id;
        s.StartMieji(top, 1); s.Branch("discard"); s.Card(lion); s.Reach(() => s.P?.SkillPrompt?.SkillId == Hp);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Mieji); var original = root.Id;
        s.Need(root.BoundaryLiRuMieji is { Stage: BoundaryLiRuMiejiStage.Movement, Payments.Count: 1 } && s.Moves(DiscardReason) is [{ CardId: var first }] && first == lion &&
            s.V(1).Hand.Any(c => c.Id == second) && s.V(1).Hp == s.V(1).MaxHp && s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any(h => h.ResumeFrameId == root.Id && h.Change.ParentFrameId == root.Id),
            "Only the first non-Trick has paid. Its discarded Silver Lion's real recovery child stays on the original Mieji frame while the second hand entity remains unpaid.");
        s.Cold(); s.Answer(_ => true); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch); s.Cold(); s.Answer(_ => true);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "boundary-li-ru-mieji" && c.Cards.Count == 1) == true);
        s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Mieji).Id == original && s.Moves(DiscardReason).Length == 1,
            "The second private discard is issued only after the first payment's recovery and cards-moved children actually finish, including cold recovery.");
        s.Card(second); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch); s.Answer(_ => true); s.Play(0);
        s.Need(s.Moves(DiscardReason).Select(m => m.CardId).SequenceEqual([lion, second]) && s.E<BoundaryLiRuMiejiPaidEvent>().Select(e => e.Ordinal).SequenceEqual([1, 2]) &&
            s.E<BoundaryLiRuMiejiPaidEvent>().All(e => e.Count == 1), "Both non-Tricks pay once in separate native movement invoices, retaining the sequential one-card child boundary.");

        var fire = s.G.GetLegalActions(0).Single(a => a.ProgramSkillId == Fencheng);
        s.Need(fire.TargetSeats.Count == 0, "Mobile Fencheng does not offer the OL edition's freely chosen starting character.");
        s.Accept(new UseProgramSkillCommand(0, Fencheng, fire.ProgramActivationId!, [], [], s.G.Revision, s.P!.PromptId));
        s.FirePrompt(1); s.FireDiscardOne(); s.FirePrompt(2);
        s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Fencheng).DiscardChallenge!.PreviousCount == 1 && s.P!.Prompt.Contains("2", StringComparison.Ordinal),
            "The real fixed walk starts at the owner's next seat; after its one-card payment, the next player must discard at least two.");
        s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "damage"); s.FirePrompt(3);
        s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Fencheng).DiscardChallenge!.PreviousCount == 0,
            "Taking the native two fire damage resets the next player's minimum to one rather than carrying an earlier player's discard count.");
        s.Cold(); s.FireDiscardOne(); s.FirePrompt(4); s.Answer(c => c.Parameters.GetValueOrDefault("branch") == "damage"); s.Play(0);
        s.Need(s.E<DamageAppliedEvent>().Where(e => e.Nature == DamageNature.Fire).Select(e => e.TargetSeat).SequenceEqual([2, 4]) &&
            s.E<DamageAppliedEvent>().Where(e => e.Nature == DamageNature.Fire).All(e => e.Amount == 2 && e.SourceSeat == 0) &&
            s.Moves(FireReason).Select(m => m.From.OwnerSeat).SequenceEqual(new int?[] { 1, 3 }) && s.G.GetLegalActions(0).All(a => a.ProgramSkillId != Fencheng),
            "The limited native walk completes seats1,2,3,4 exactly once, retains both real fire damage returns and real discard payments, and never includes its own actor.");

        var shortScene = Scene.Start(); shortScene.Play(0); shortScene.Search("trick", 0); shortScene.Use("clear", 1); shortScene.Play(0);
        shortScene.Need(shortScene.G.GetLegalActions(0).Where(a => a.ProgramSkillId == Mieji).SelectMany(a => a.TargetSeats).All(t => t != 1),
            "An actually empty hand/equipment target cannot be selected for Mieji; target qualification uses public ownership, not the hidden categories of another hand.");
        shortScene.Search("equipment", 1);
        shortScene.Need(shortScene.V(1).HandCount == 1 && shortScene.V(1).Equipment.Count == 0, "The shortage uses one actual owned non-Trick entity, not a forged configured card count.");
        var only = shortScene.V(1).Hand.Single().Id; shortScene.StartMieji(shortScene.V(0).Hand.First(c => c.Kind == CardKind.Dismantlement).Id, 1); shortScene.Branch("discard"); shortScene.Card(only);
        shortScene.Reach(() => shortScene.P?.SkillPrompt?.SkillId == Watch); shortScene.Cold(); shortScene.Answer(_ => true); shortScene.Play(0);
        shortScene.Need(shortScene.V(1).HandCount == 0 && shortScene.Moves(DiscardReason) is [{ CardId: var single }] && single == only && shortScene.E<BoundaryLiRuMiejiPaidEvent>().Length == 1,
            "With no second legal non-Trick after the first real payment returns, the original Mieji completes with only that one discard.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 5).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public T[] E<T>() => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardMovementRecord[] Moves(string reason) => G.CardMovements.Where(m => m.Reason.Value == reason).ToArray();
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } && P.PlayerSeat == seat);
        public void End() => Accept(new EndPlayPhaseCommand(P!.PlayerSeat, G.Revision, P.PromptId));
        public void Use(string id, params int[] targets) => Accept(new UseProgramSkillCommand(0, Driver, id, [], targets, G.Revision, P!.PromptId));
        public void StartMieji(int card, int target) => Accept(new UseProgramSkillCommand(0, Mieji, "black-trick-top", [card], [target], G.Revision, P!.PromptId));
        public void Branch(string branch) { Reach(() => P?.SkillPrompt?.SkillId == Mieji && P.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == branch)); Answer(c => c.Parameters.GetValueOrDefault("branch") == branch); }
        public void Card(int card) { Reach(() => P?.SkillPrompt?.SkillId == Mieji && P.Choices.Any(c => c.Cards.SequenceEqual(new[] { card }))); Answer(c => c.Cards.SequenceEqual([card])); }
        public void FirePrompt(int seat) => Reach(() => P?.SkillPrompt?.SkillId == Fencheng && P.PlayerSeat == seat);
        public void FireDiscardOne() { Answer(c => c.Parameters.GetValueOrDefault("branch") == "card"); Answer(c => c.Parameters.GetValueOrDefault("branch") == "finish"); }
        public void Search(string criterion, int target)
        { Use("search"); Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "declared-deck-criterion") == true); Answer(c => c.Parameters.GetValueOrDefault("criterion") == criterion); Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "declared-deck-recipient") == true); Answer(c => c.Targets.SequenceEqual([target])); Play(0); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Li Ru fixture command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 240 && !done(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } setup) { Accept(new SelectGeneralCommand(setup.PlayerSeat, setup.PlayerSeat == 0 ? Owner : "fixture:boundary-li-ru-mobile-peer-" + setup.PlayerSeat, G.Revision, setup.PromptId)); continue; }
                if (P?.SkillPrompt?.SkillId is Watch or Hp or Top) { Answer(_ => true); continue; }
                if (P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-owned-cards") == true) { Answer(c => c.Cards.Count == 1); continue; }
                if (P is { Kind: DecisionKind.ProgramTrigger }) throw new InvalidOperationException("Unexpected mobile Li Ru boundary: " + JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The bounded native mobile Li Ru fixture missed its actual expected boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold keeps the original paid top-deck, gift/sequential cost, real movement/health children and Fencheng cursor."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 5).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId } : null, Players = Enumerable.Range(0, 5).Select(i => new { Seat = i, V(i).Hp, V(i).HandCount }) })); }
        public static Scene Start()
        { var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture()); var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 5, HumanSeat = 0, HumanSeats = [0, 1, 2, 3, 4], HumanRole = Role.Lord, ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, r); var s = new Scene(r, g); s.Accept(new StartGameCommand()); return s; }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-li-ru-mobile", new(1, 0, 0));
        internal static string Rules() => $$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
         {"id":"{{Driver}}","revision":1,"activations":[
          {"id":"search","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"declareDeckCriterionAndGiveMatchingCard","target":"owner"}]},
          {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"equipped"}]},
          {"id":"hurt","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":1}]},
          {"id":"clear","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"selectOwnedCards","target":"selectedTarget","numberExpression":"allOwnedZoneCards","zones":["hand","equipment"],"resultBind":"clear"},{"op":"moveBoundCards","target":"owner","sourceBind":"clear","destination":"discardPile","awaitMovementTriggers":true}]}
         ]},
         {"id":"{{Top}}","revision":1,"triggers":[{"id":"paid-top","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:mieji-mobile.MoveBoundCards"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
         {"id":"{{Watch}}","revision":1,"triggers":[
          {"id":"paid-hand","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{GiftReason}}","{{DiscardReason}}","{{FireReason}}"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]},
          {"id":"paid-equipment","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["{{DiscardReason}}","{{FireReason}}"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
         ]},
         {"id":"{{Hp}}","revision":1,"triggers":[{"id":"real-lion-recovery","window":"afterHpRecovered","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"recovered","options":[{"id":"continue"}]}]}]}
        ]}
        """;
        internal static string Presentation() => JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> { [Driver] = new { name = "实际固定实体命令", description = "真实检索给牌、装备、失血与清手" }, [Top] = Pause("原生置顶付款"), [Watch] = Pause("原生给弃付款"), [Hp] = Pause("原生白银狮子回复") } });
        private static object Pause(string name) => new { name, description = "实际孩子返回原始付款帧", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } };
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero); var c = SkillProgramCatalog.Load(Rules(), Presentation()); foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            // The small character fixture intentionally loads no full classic
            // roster package; register its one required canonical armor entity.
            b.AddCard(new("classic:silver-lion", "白银狮子", "装备牌", CardCatalog.Get(CardKind.SilverLion).Description, CardKind.SilverLion));
            b.AddGeneral(new(Owner, "移动界李儒原生场景", "supporter", Mieji, "qun", 3, AdditionalSkillIds: [Juece, Fencheng, Driver, Top, Watch, Hp], Gender: GeneralGender.Male));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:boundary-li-ru-mobile-peer-" + i).ToArray(); foreach (var id in peers) b.AddGeneral(new(id, "实际固定角色", "supporter", "standard:none", "qun", 6, AdditionalSkillIds: [Watch, Hp], Gender: GeneralGender.Male));
            const string deck = "fixture:boundary-li-ru-mobile-deck"; b.AddDeck(new(deck, "真实黑色锦囊和红色装备", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 140).Select(i => new ContentDeckPhysicalCard(i % 2 == 0 ? "standard:dismantlement" : "classic:silver-lion", i % 2 == 0 ? Suit.Spade : Suit.Heart, i % 13 + 1)).ToArray() });
            b.AddMode(new(Mode, "移动界李儒实际身份主流程", 5, 5, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 }, deck, GeneralCandidateCount: 5, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
