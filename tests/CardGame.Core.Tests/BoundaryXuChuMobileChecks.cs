using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryXuChuMobileChecks
{
    private const string Hero = "boundary:xu-chu-mobile", Skill = "boundary:luoyi-mobile",
        Watch = "fixture:mobile-xu-chu-gain", Enemy = "fixture:mobile-xu-chu-enemy", Deck = "fixture:mobile-xu-chu-deck",
        MoveReason = "skill-program.boundary:luoyi-mobile.MoveBoundCards";

    public static void MandatoryRevealThenChoiceGainChildrenSourceLossAndNativeDamageCold()
    {
        var s = Scene.Start(); s.AtChoice();
        var reveal = s.E<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Skill);
        var root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill); var rootId = root.Id;
        s.Need(reveal.Cards.Count == 3 && reveal.Cards.Select(c => c.Kind).Order().SequenceEqual(new[] { CardKind.Peach, CardKind.Duel, CardKind.Crossbow }.Order()) &&
            Enumerable.Range(0, 4).All(i => s.G.CreateSnapshot(i).PublicRevealedCards.Select(c => c.Id).Order().SequenceEqual(reveal.Cards.Select(c => c.Id).Order())) &&
            root.InstructionIndex == 3 && !s.E<ProgramTurnPhaseSubstitutedEvent>().Any(e => e.SkillId == Skill) &&
            !s.E<CardDamageModifierGrantedEvent>().Any(e => e.Modifier.Source.SkillId == Skill) && s.V(0).HandCount == 1 &&
            s.P!.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "skip-draw") &&
            !s.P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"),
            "Mobile Luoyi mandatorily reveals the three actual public entities before any optional substitution, gain, bonus or activation prompt.");
        s.Cold(); s.Option("skip-draw"); s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        root = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == rootId);
        s.Need(root.InstructionIndex == 5 && root.PendingMovementContinuation is { SubjectSeat: 0 } &&
            reveal.Cards.All(c => s.V(0).Hand.Any(h => h.Id == c.Id)) && s.V(0).HandCount == 4 &&
            s.E<ProgramTurnPhaseSubstitutedEvent>().Count(e => e.FrameId == rootId) == 1 &&
            !s.E<CardDamageModifierGrantedEvent>().Any(e => e.Modifier.Source.SkillId == Skill),
            "The actual basic, weapon and Duel gain pauses in its genuine movement child after committing the original Draw substitution and before issuing the damage benefit.");
        s.Cold(); s.Option("continue");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill) == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish"); s.Play();
        var grant = s.E<CardDamageModifierGrantedEvent>().Single(e => e.Modifier.Source.SkillId == Skill).Modifier;
        s.Need(!s.V(0).Skills!.Any(k => k.ContentId == Skill) && grant.ParentFrameId == rootId && grant.Amount == 1 &&
            grant.Expiration == SkillProgramDamageModifierExpiration.NextOwnerTurnStart && grant.SourceScope == SkillProgramDamageModifierSourceScope.DamageSource &&
            s.E<ProgramCardsRevealedEvent>().Count(e => e.SkillId == Skill) == 1 && s.NormalDraws == 0 && s.V(0).HandCount == 4 &&
            reveal.Cards.All(c => s.G.CardMovements.Count(m => m.CardId == c.Id && m.From == CardLocation.Processing && m.To == CardLocation.Hand(0) && m.Reason.Value == MoveReason) == 1) &&
            s.E<ProgramBindingResolvedEvent>().Count(e => e.FrameId == rootId && e.Completed) == 1,
            "Cold and native source loss return on the same paid Luoyi: gain each original entity once, skip real Draw, and issue the one native damage modifier once.");
        var slash = s.V(0).Hand.Single(c => c.Kind == CardKind.Slash).Id;
        var action = s.G.GetHumanLegalActions().First(a => a.CardId == slash && a.TargetSeats.SequenceEqual([1]));
        s.Accept(new PlayCardCommand(0, slash, action.TargetSeats, s.G.Revision, s.P!.PromptId, action.PlayedCardKind, action.TargetCardId)); s.Play();
        s.Need(s.E<DamageAppliedEvent>().Single(e => e.SourceSeat == 0 && e.TargetSeat == 1) is { Amount: 2, SourceCardUseResolutionId: not null } &&
            s.G.CardMovements.Count(m => m.CardId == slash && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1,
            "The original real Slash pays once and deals base1 plus the committed mobile Luoyi1 after the owner has genuinely lost the source skill.");

        var decline = Scene.Start(); decline.AtChoice(); var declined = decline.E<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Skill);
        decline.Option("normal-draw"); decline.Play();
        decline.Need(decline.NormalDraws == 2 && decline.V(0).HandCount == 3 &&
            !decline.E<ProgramTurnPhaseSubstitutedEvent>().Any(e => e.SkillId == Skill) && !decline.E<CardDamageModifierGrantedEvent>().Any(e => e.Modifier.Source.SkillId == Skill) &&
            declined.Cards.All(c => decline.G.CardMovements.Count(m => m.CardId == c.Id && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason.Value == MoveReason) == 1),
            "After seeing eligible public cards, the owner can decline, discard all three and perform exactly normal Draw2 without a damage grant.");

        var empty = Scene.Start(armorOnly: true); empty.AtChoice();
        empty.Need(empty.E<ProgramCardsRevealedEvent>().Single(e => e.SkillId == Skill).Cards.Count == 3 &&
            empty.P!.Choices.All(c => c.Parameters.GetValueOrDefault("option-id") == "normal-draw"),
            "Three actual nonweapon armors are still revealed but cannot buy a damage bonus without any eligible card.");
        empty.Option("normal-draw"); empty.Play();
        empty.Need(empty.NormalDraws == 2 && !empty.E<CardDamageModifierGrantedEvent>().Any(e => e.Modifier.Source.SkillId == Skill),
            "An empty eligible set returns to real normal Draw2 and does not receive an unearned mobile Luoyi bonus.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => G.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(0).Players[seat];
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public int NormalDraws => G.CardMovements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Draw);
        public void AtChoice() => Reach(() => P?.SkillPrompt?.SkillId == Skill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "normal-draw"));
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Option(string id) => Answer(c => c.Parameters.GetValueOrDefault("option-id") == id);
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native mobile Xu Chu command."); }
        public void Reach(Func<bool> done)
        {
            for (var i = 0; i < 120 && !done(); i++)
            {
                Need(P is null, "Unexpected human mobile Xu Chu boundary.");
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(), "The small mobile Xu Chu scene missed its native boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold preserves the actual public reveal, original Draw parent, exact gained entities and native child."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public void Need(bool condition, string text) { if (!condition) throw new InvalidOperationException(text + " " + JsonSerializer.Serialize(new { Pending = P is { } p ? new { p.Kind, Skill = p.SkillPrompt?.SkillId } : null, Hand = V(0).HandCount, Hp = V(0).Hp })); }
        public static Scene Start(bool armorOnly = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(armorOnly));
            var extras = Enumerable.Range(0, 6).Select(i => "fixture:mobile-xu-chu-dummy-" + i).ToArray();
            var setup = new ShanheBattleSetup(71, 1, "fixture-mobile-xu-chu", Hero, 4, 0, 0, 0, 0,
                [Watch, .. extras], [new(CardKind.Slash)], [],
                Enumerable.Range(1, 3).Select(_ => new ShanheCombatantDefinition(Enemy, MaxHpBonus: 30) { StartingHand = [] }).ToArray(), [], new string('a', 64));
            var scene = new Scene(registry, GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry));
            scene.Accept(new StartGameCommand()); return scene;
        }
    }

    private sealed class Fixture(bool armorOnly) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-xu-chu-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero);
            var skills = new[] { new { id = Watch, revision = 1, triggers = new[] { new { id = "native-gain", window = "cardsGained", subject = "owner",
                destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", movementReasons = new[] { MoveReason }, optional = false,
                effects = new object[] { new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" } } },
                    new { op = "replaceSkillsOnAwakening", target = "owner", skillIds = new[] { "standard:none" } } } } } } };
            var labels = new Dictionary<string, object> { [Watch] = new { name = "真实裸衣获得子窗", description = "已付获得后失去来源技能", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            foreach (var i in Enumerable.Range(0, 6)) b.AddSkill(new("fixture:mobile-xu-chu-dummy-" + i, "真实替换资格", "供已付获得子窗执行原生失技。"));
            b.AddGeneral(new(Enemy, "原生守军", "supporter", "standard:none", "qun", 4));
            b.AddDeck(new(Deck, "固定三个公开实体", 0, 2, []) { PhysicalCards = armorOnly
                ? Enumerable.Range(0, 3).Select(_ => new ContentDeckPhysicalCard("standard:bagua", Suit.Diamond, 6)).ToArray()
                : [new("standard:peach", Suit.Heart, 3), new("standard:duel", Suit.Spade, 4), new("standard:crossbow", Suit.Club, 5)] });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动裸衣原生流程", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
