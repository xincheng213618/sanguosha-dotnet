using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileClassicBatch70Checks
{
    private const string Owner = "fixture:mobile-classic70-owner", Driver = "fixture:mobile-classic70-driver",
        Quiet = "fixture:mobile-classic70-quiet", Extra = "fixture:mobile-classic70-extra", Mode = "identity:classic-mobile-classic70";

    public static void GuoHuaiCountsOnlyItsCurrentActualPlay()
    {
        var s = Scene.Start("mobile:guo-huai"); s.Play(0);
        var hp = s.V(0).Hp;
        var armors = s.V(0).Hand.Where(c => c.Kind == CardKind.BaguaFormation).Select(c => c.Id).ToArray();
        s.Need(hp > 0 && armors.Length >= hp, "The fixed opening has enough real equipment uses for the Jingce HP threshold.");
        foreach (var armor in armors.Take(hp)) { s.Equip(0, armor); s.Play(0); }
        s.Drive(0, "stock", s.V(0).Hand.Take(2).Select(c => c.Id).ToArray(), []); s.Play(0);
        s.End(0); s.Offer("mobile:jingce"); s.Cold(); var before = s.V(0).HandCount;
        s.Activate(); s.Play(0);
        s.Need(s.V(0).HandCount == before + 2 && s.G.CardMovements.Count(m => m.Reason.Value == "skill-program.mobile:jingce.Draw") == 2,
            "Jingce pays exactly two real draws once for its current inserted Play's genuine uses.");
        s.End(0); s.Play(1);
        s.Need(s.G.Events.Count(e => e.Payload is ProgramBindingStartedEvent { SkillId: "mobile:jingce" }) == 1,
            "The following natural Play has no uses: the same actual turn's earlier uses cannot qualify Jingce again.");
    }

    public static void GuanXingZhangBaoRevealsMixedColorsAndExpiresParentSkills()
    {
        var s = Scene.Start("mobile:guan-xing-zhang-bao"); s.Offer("mobile:fuhun"); s.Skip(); s.Play(0);
        var red = s.V(0).Hand.First(c => c.Suit is Suit.Heart or Suit.Diamond).Id;
        var black = s.V(0).Hand.First(c => c.Suit is Suit.Spade or Suit.Club).Id;
        s.Drive(0, "stock", [red, black], []); s.Play(0); s.End(0);
        s.Offer("mobile:fuhun"); s.Cold(); var before = s.V(0).HandCount;
        s.Activate(); s.Play(0);
        s.Need(s.V(0).HandCount == before + 2 && s.V(0).Hand.Any(c => c.Id == red) && s.V(0).Hand.Any(c => c.Id == black) &&
            s.G.Events.Any(e => e.Payload is ProgramTurnSkillsGrantedEvent gained && gained.OwnerSeat == 0 && gained.GrantedSkillIds.Contains("classic:wusheng")) &&
            s.V(0).Skills!.Any(skill => skill.Id == "classic:wusheng") && s.V(0).Skills!.Any(skill => skill.Id == "classic:paoxiao"),
            "Normal draw is replaced by the exact two revealed original entities, and their different colors grant the existing parent skills.");
        s.End(0); s.Play(1);
        s.Need(!s.V(0).Skills!.Any(skill => skill.Id == "classic:wusheng") &&
            !s.V(0).Skills!.Any(skill => skill.Id == "classic:paoxiao"),
            "Both parent grants expire with the original actual turn.");
    }

    public static void LiRuMiejiAcceptsEquipmentOnlyTargetAndPaysOnce()
    {
        var s = Scene.Start("mobile:li-ru"); s.Play(0);
        var armor = s.V(0).Hand.First(c => c.Kind == CardKind.BaguaFormation).Id;
        s.Drive(0, "gift", [armor], [1]); s.Play(0); s.End(0); s.Play(1);
        var equip = s.G.GetLegalActions(1).Single(a => a.CardId == armor);
        s.Accept(new PlayCardCommand(1, armor, equip.TargetSeats, s.G.Revision, s.P!.PromptId, equip.PlayedCardKind, equip.TargetCardId));
        s.Play(1); s.End(1); s.Play(0);
        var cost = s.V(0).Hand.First(c => c.Kind == CardKind.Dismantlement && c.Suit is Suit.Spade or Suit.Club).Id;
        var action = s.G.GetLegalActions(0).Single(a => a.ProgramSkillId == "mobile:mieji");
        s.Need(s.V(1).HandCount == 0 && s.V(1).Equipment.Count == 1 && action.SelectableTargetSeats.Contains(1),
            "The actual equipment-only target is admitted without the classic hand-only restriction.");
        s.Accept(new UseProgramSkillCommand(0, "mobile:mieji", "topdeck-black-trick", [cost], [1], s.G.Revision, s.P!.PromptId));
        s.Reach(() => s.P?.PlayerSeat == 1 && s.P.SkillPrompt?.SkillId == "mobile:mieji"); s.Cold();
        s.Answer(c => c.Cards.SequenceEqual([armor])); s.Play(0);
        s.Need(s.G.CardMovements.Count(m => m.CardId == cost && m.To == CardLocation.DrawPile) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == armor && m.From == CardLocation.Equipment(1) && m.To == CardLocation.DiscardPile) == 1 &&
            !s.G.GetLegalActions(0).Any(a => a.ProgramSkillId == "mobile:mieji"),
            "Mieji topdecks its original black Trick once, discards the only available non-Trick and consumes its phase usage once across cold recovery.");
    }

    private sealed class Scene(GameEngine game, ContentRegistry registry, string hero)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public void Need(bool ok, string text) { if (!ok) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(P)); }
        public void Accept(GameCommand c) { var r = G.Submit(c); Need(r.Accepted, r.Error?.Message ?? "Rejected batch70 native command."); }
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Skip() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
        public void Offer(string skill) => Reach(() => P?.SkillPrompt?.SkillId == skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        public void Play(int seat) => Reach(() => P is { Kind: DecisionKind.PlayCard } p && p.PlayerSeat == seat);
        public void End(int seat) => Accept(new EndPlayPhaseCommand(seat, G.Revision, P!.PromptId));
        public void Equip(int seat, int card)
        {
            var action = G.GetLegalActions(seat).Single(a => a.CardId == card && a.Kind == LegalActionKind.Equip);
            Accept(new PlayCardCommand(seat, card, action.TargetSeats, G.Revision, P!.PromptId, action.PlayedCardKind, action.TargetCardId));
        }
        public void Drive(int seat, string activation, IReadOnlyList<int> cards, IReadOnlyList<int> targets) =>
            Accept(new UseProgramSkillCommand(seat, Driver, activation, cards, targets, G.Revision, P!.PromptId));
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 400 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } setup) { Accept(new SelectGeneralCommand(setup.PlayerSeat, setup.PlayerSeat == 0 ? Owner : Peer(setup.PlayerSeat), G.Revision, setup.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard } play && play.PlayerSeat != 0) { End(play.PlayerSeat); continue; }
                if (P is { Kind: DecisionKind.RespondDodge }) { Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage"); continue; }
                if (P?.SkillPrompt?.SkillId == "classic:juece") { Skip(); continue; }
                Need(P is null, "Unexpected batch70 prompt."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded batch70 phase for " + hero);
        }
        public void Cold()
        {
            var json = GameCheckpointJson.Serialize(G.CreateCheckpoint());
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(json), registry);
            Need(GameCheckpointJson.Serialize(G.CreateCheckpoint()) == json, "The original native pending command prefix survives checkpoint JSON replay.");
        }
        public static Scene Start(string hero)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(hero));
            var g = GameEngine.CreateStandard(new() { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, registry);
            var s = new Scene(g, registry, hero); s.Accept(new StartGameCommand()); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-classic70-peer-" + seat;
    private sealed class Fixture(string hero) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-classic70", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, hero == "mobile:guan-xing-zhang-bao" ? [hero, "classic:guan-yu", "classic:zhang-fei"] : hero == "mobile:li-ru" ? [hero, "classic:li-ru"] : [hero]);
            object[] skills = [
                new { id = Driver, revision = 1,
                    activations = new object[] {
                        new { id = "slash", minCards = 0, maxCards = 0, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                            effects = new[] { new { op = "useVirtualCard", target = "selectedTarget", outputKind = "slash", targetRestriction = "distanceUnlimitedAgainstTarget" } } },
                        new { id = "stock", minCards = 2, maxCards = 2, sourceZones = new[] { "hand" }, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                            effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "stock" }, new { op = "moveBoundCards", target = "owner", sourceBind = "stock", destination = "drawPileTop", awaitMovementTriggers = true } } },
                        new { id = "gift", minCards = 1, maxCards = 1, sourceZones = new[] { "hand" }, minTargets = 1, maxTargets = 1, targetKind = "otherLiving", usesPerTurn = (int?)null,
                            effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "gift" }, new { op = "moveBoundCards", target = "owner", sourceBind = "gift", destination = "selectedTargetHand" } } } } },
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "zero-normal-draw", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false, priority = 100,
                    effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } },
                new { id = Extra, revision = 1, modifiers = new[] { new { id = "eight-owned-entities", query = "initialHandSize", operation = "add", value = 8, priority = 0 }, new { id = "retain-test-materials", query = "handLimit", operation = "add", value = 20, priority = 0 } },
                    triggers = hero == "mobile:guo-huai" ? new[] { new { id = "first-extra-play", window = "turnStartBeforeNormalFlow", subject = "owner", optional = false, usageScope = "game", usageLimit = 1,
                        effects = new[] { new { op = "insertPhase", target = "owner", phase = "play", phaseContinuation = "beforeNormalPreparation" } } } } : [] }
            ];
            var labels = skills.Select(s => s.GetType().GetProperty("id")!.GetValue(s)!.ToString()!).ToDictionary(id => id, _ => (object)new { name = "固定原生夹具", description = "真实阶段、实体和命令驱动。" });
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            var main = hero == "mobile:guo-huai" ? "mobile:jingce" : hero == "mobile:li-ru" ? "mobile:mieji" : "mobile:fuhun";
            b.AddGeneral(new(Owner, "移动主流程", "supporter", main, "wei", 1,
                AdditionalSkillIds: [Driver, Quiet, Extra], Gender: GeneralGender.Male));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "固定同桌", "supporter", Quiet, "wu", 20));
            const string deck = "fixture:mobile-classic70-deck";
            b.AddDeck(new(deck, "固定八张实体", 0, 2, []) { PhysicalCards = Enumerable.Range(0, 2).SelectMany(i => new[] {
                new ContentDeckPhysicalCard("standard:dismantlement", Suit.Spade, i + 1), new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, i + 3),
                new ContentDeckPhysicalCard("standard:bagua", Suit.Club, i + 5), new ContentDeckPhysicalCard("standard:slash", Suit.Diamond, i + 7) }).ToArray() });
            b.AddMode(new(Mode, "移动本批真实身份", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
