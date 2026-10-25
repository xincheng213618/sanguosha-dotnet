using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileLingCaoChecks
{
    private const string Hero = "mobile:ling-cao", Skill = "mobile:dujin", Owner = "fixture:mobile-ling-cao-owner", Driver = "fixture:mobile-ling-cao-stock", Quiet = "fixture:mobile-ling-cao-normal", Mode = "identity:mobile-ling-cao";
    public static void OddAndEvenEquipmentUseActualExtraDraws()
    {
        var s = Scene.Start(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill);
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip"); s.Play();
        var stock = s.V.Hand.Where(c => c.Kind == CardKind.Dodge).Select(c => c.Id).ToArray();
        s.Need(stock.Length == 6 && s.V.HandCount == 8, "Native opening deals the eight fixed materials before any optional extra draw.");
        s.Accept(new UseProgramSkillCommand(0, Driver, "stock", stock, [], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Equip(CardKind.Crossbow); s.Need(s.V.Equipment.Count == 1, "The real weapon use establishes one actual equipped entity.");
        s.End(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill); var odd = s.V.HandCount;
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.Play();
        s.Need(s.V.Equipment.Count == 1 && s.V.HandCount == odd + 1 && s.ExtraDraws == 1,
            "One actual equipment grants one real extra card, preserving the mobile floor rule rather than OL's two-card ceiling result.");
        s.Equip(CardKind.BaguaFormation); s.Need(s.V.Equipment.Count == 2, "A second real equipment use adds the armor without replacing the weapon.");
        s.End(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Skill); var even = s.V.HandCount;
        s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate"); s.Play();
        s.Need(s.V.Equipment.Count == 2 && s.V.HandCount == even + 2 && s.ExtraDraws == 3,
            "The following actual draw phase observes both current equipment entities and grants two real extra cards exactly once.");
    }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public int ExtraDraws => G.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program.mobile:dujin.Draw");
        public void Answer(Func<PromptChoice, bool> choose) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(choose).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native Ling Cao command."); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Equip(CardKind kind)
        {
            var id = V.Hand.Single(c => c.Kind == kind).Id; var a = G.GetLegalActions(0).Single(a => a.CardId == id);
            Accept(new PlayCardCommand(0, id, a.TargetSeats, G.Revision, P!.PromptId, a.PlayedCardKind, a.TargetCardId)); Play();
        }
        public void Reach(Func<bool> ready)
        {
            for (var i = 0; i < 350 && !ready(); i++)
            {
                if (P is { Kind: DecisionKind.SelectGeneral } setup) { Accept(new SelectGeneralCommand(setup.PlayerSeat, setup.PlayerSeat == 0 ? Owner : Peer(setup.PlayerSeat), G.Revision, setup.PromptId)); continue; }
                if (P is { Kind: DecisionKind.PlayCard } other && other.PlayerSeat != 0) { Accept(new EndPlayPhaseCommand(other.PlayerSeat, G.Revision, other.PromptId)); continue; }
                Need(P is null, "Unexpected native Ling Cao prompt."); Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(ready(), "Missing bounded native Ling Cao phase.");
        }
        public void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Pending = P, V.Hp, V.HandCount, Equipment = V.Equipment.Select(c => c.Kind), G.State.TurnNumber })); }
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var s = new Scene(GameEngine.CreateStandard(new() { Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 12 }, r));
            s.Accept(new StartGameCommand()); return s;
        }
    }
    private static string Peer(int seat) => "fixture:mobile-ling-cao-peer-" + seat;
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-ling-cao", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var skills = new object[] {
                new { id = Driver, revision = 1, modifiers = new[] { new { id = "fixed-eight-opening", query = "initialHandSize", operation = "add", value = 8, priority = 0 } },
                    activations = new[] { new { id = "stock", minCards = 6, maxCards = 6, sourceZones = new[] { "hand" }, cardKinds = new[] { "dodge" }, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new object[] { new { op = "captureSelectedCards", target = "owner", resultBind = "stock" }, new { op = "moveBoundCards", target = "owner", sourceBind = "stock", destination = "drawPileTop", awaitMovementTriggers = true } } } } },
                new { id = Quiet, revision = 1, triggers = new[] { new { id = "normal-zero", window = "drawPhaseStarting", subject = "owner", optional = false, isSkillInvocation = false, priority = 100,
                    effects = new[] { new { op = "adjustNormalDraw", target = "owner", amount = -2 } } } } } };
            var labels = new Dictionary<string, object> { [Driver] = new { name = "固定真实八牌", description = "原生开局和真实放回牌堆" }, [Quiet] = new { name = "固定正常摸牌", description = "正常阶段保留，单独观察技能额外摸牌" } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = program, ProgramPresentation = c.Presentations[id] });
            b.AddGeneral(new(Owner, "移动凌操真实装备", "supporter", Skill, "wu", 4, AdditionalSkillIds: [Driver, Quiet], Gender: GeneralGender.Male));
            for (var seat = 1; seat < 4; seat++) b.AddGeneral(new(Peer(seat), "固定真实同桌", "supporter", Quiet, "wei", 4));
            const string deck = "fixture:mobile-ling-cao-deck";
            b.AddDeck(new(deck, "固定八张原生实体", 0, 2, []) { PhysicalCards = new[] { new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 1), new ContentDeckPhysicalCard("standard:bagua", Suit.Spade, 2) }
                .Concat(Enumerable.Range(0, 6).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, i + 3))).ToArray() });
            b.AddMode(new(Mode, "移动凌操原生向下取整", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 1, [nameof(Role.Renegade)] = 2 }, deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, Peer(1), Peer(2), Peer(3)]));
        }
    }
}
