using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryHuaTuoMobileChecks
{
    private const string Hero = "boundary:hua-tuo-mobile", Skill = "boundary:qingnang-mobile", Owner = "fixture:mobile-hua-tuo-owner", Peer = "fixture:mobile-hua-tuo-peer";
    private const string Watch = "fixture:mobile-hua-tuo-discard-watch", Deck = "fixture:mobile-hua-tuo-deck";

    public static void RedContinuesBlackStopsAndOriginalRecoveryReturnsCold()
    {
        var s = Scene.Start(); var original = s.V(0).Hand.ToArray();
        var firstRed = original.Single(c => c.Suit == Suit.Heart && c.Rank == 2).Id;
        var secondRed = original.Single(c => c.Suit == Suit.Diamond).Id;
        var black = original.Single(c => c.Suit == Suit.Club).Id;
        var nextPhaseRed = original.Single(c => c.Suit == Suit.Heart && c.Rank == 5).Id;
        s.Use(firstRed, 1);
        s.PaidBoundary(); var first = s.E<BoundaryHuaTuoQingnangPaidEvent>().Single();
        var parent = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == first.FrameId);
        Need(first.Payment is { Red: true, Target: 1 } && first.Payment.CardId == firstRed && first.Payment.Phase.PhaseActorSeat == 0 &&
            first.Payment.MovementSequence > first.Payment.Before && parent.BoundaryHuaTuoQingnang == first.Payment &&
            parent.SelectedCardPayment is { MovementCommitted: true, InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected } &&
            s.G.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(f => f.Batch.ParentFrameId == parent.Id) && s.V(1).Hp == 3,
            "A genuine red hand discard pays on its original selected-card frame and real movement child before the original target recovers.");
        s.Cold(); s.Continue(); s.Play();
        Need(s.V(1).Hp == 4 && s.Action().SelectableTargetSeats.Contains(0) && !s.Action().SelectableTargetSeats.Contains(1),
            "The first actual red payment permits another use, while its still-wounded first target is excluded for the actual turn.");
        s.Heal(secondRed, 0);
        Need(s.V(0).Hp == 3 && s.E<BoundaryHuaTuoQingnangPaidEvent>().Count(e => e.Payment.Red) == 2 && s.Action().SelectableTargetSeats.Contains(2),
            "A second actual red discard heals the owner through native recovery and adds one further use for another target.");
        s.Heal(black, 2);
        Need(s.V(2).Hp == 4 && s.V(3).Hp == 3 && s.V(0).Hand.Any(c => c.Id == nextPhaseRed) &&
            s.G.GetHumanLegalActions().All(a => a.ProgramSkillId != Skill) && s.E<BoundaryHuaTuoQingnangPaidEvent>().Single(e => !e.Payment.Red).Payment.CardId == black,
            "A real black hand discard still heals its original target once but exhausts this actual Play allowance despite a remaining red card and an eligible wounded fourth role.");
        s.Accept(new EndPlayPhaseCommand(0, s.G.Revision, s.P!.PromptId)); s.Play();
        Need(s.Action().SelectableTargetSeats.SequenceEqual([3]) && s.V(1).Hp < s.V(1).MaxHp && s.V(2).Hp < s.V(2).MaxHp,
            "Dangxian's distinct subsequent actual Play resets the phase allowance; the official turn-wide target restriction still excludes both previously healed wounded roles.");
        s.Use(nextPhaseRed, 3); s.PaidBoundary(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "remove-source");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill) == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == Skill);
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish") == true);
        s.Answer(c => c.Parameters.GetValueOrDefault("advanced-value") == "finish");
        s.Reach(() => s.P?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "finish") == true);
        var lostSourcePayment = s.E<BoundaryHuaTuoQingnangPaidEvent>().Last();
        Need(s.V(0).Skills?.Any(k => k.ContentId == Skill) != true && s.V(3).Hp == 3 &&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == lostSourcePayment.FrameId) is
                { InstructionIndex: 1, SelectedCardPayment: { MovementCommitted: true }, SelectedCardPaymentResult: null },
            "The fourth actual discard child removes the real Qingnang source before recovery, while the exact original paid selected-card frame remains suspended.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "finish"); s.Play();
        var payments = s.E<BoundaryHuaTuoQingnangPaidEvent>().ToArray();
        Need(payments.Length == 4 && payments.Take(3).All(e => e.Payment.Phase == first.Payment.Phase) &&
            payments[3].Payment.Phase.TurnNumber == first.Payment.Phase.TurnNumber && payments[3].Payment.Phase != first.Payment.Phase &&
            s.E<RecoveryAppliedEvent>().Count() == 4 && Enumerable.Range(0, 4).All(seat => s.E<RecoveryAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == seat && e.Amount == 1) == 1) &&
            original.All(c => s.G.CardMovements.Count(m => m.CardId == c.Id && m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.boundary:qingnang-mobile.DiscardSelected") == 1) &&
            s.E<ProgramSkillResolvedEvent>().Count(e => e.SkillId == Skill && e.Completed) == 4 && s.V(0).Skills?.Any(k => k.ContentId == Skill) != true,
            "The two real phase tokens, red continuation rights, four original discard entities and original-target recovery completions remain distinct and once-only, including after actual source loss and cold paid return.");
        s.Cold();
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players.Single(p => p.Seat == seat);
        public IEnumerable<T> E<T>() => G.Events.Select(e => e.Payload).OfType<T>();
        public LegalAction Action() => G.GetHumanLegalActions().Single(a => a.ProgramSkillId == Skill);
        public void Use(int card, int target) => Accept(new UseProgramSkillCommand(0, Skill, "discard-and-heal", [card], [target], G.Revision, P!.PromptId));
        public void PaidBoundary() => Reach(() => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Heal(int card, int target) { Use(card, target); PaidBoundary(); Continue(); Play(); }
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Answer(Func<PromptChoice, bool> predicate) { var p = P!; Accept(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(predicate).Id, G.Revision)); }
        public void Accept(GameCommand command) { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected actual mobile Hua Tuo command."); }
        public void Reach(Func<bool> goal)
        {
            for (var step = 0; step < 180; step++)
            {
                if (goal()) return;
                if (P is { PlayerSeat: 0 } p) throw new InvalidOperationException("Unexpected mobile Hua Tuo prompt: " + JsonSerializer.Serialize(new { p.Kind, Skill = p.SkillPrompt?.SkillId }));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("Missing bounded original mobile Qingnang boundary.");
        }
        public void Cold() { var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry); Need(before == Dump(), "Cold retains exact original payment color, actual phase allowance, turn targets and native recovery child return."); }
        private string Dump() => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(), Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements });
        public static Scene Start()
        {
            var r = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var setup = new ShanheBattleSetup(83, 1, "fixture-mobile-hua-tuo-qingnang", Owner, 2, 0, 0, 0, 0, [],
                [new(CardKind.Dodge, Suit.Heart, 2), new(CardKind.Dodge, Suit.Diamond, 3), new(CardKind.Dodge, Suit.Club, 4), new(CardKind.Dodge, Suit.Heart, 5)], [],
                [new(Peer, Hp: 3) { StartingHand = [] }, new(Peer, Hp: 3) { StartingHand = [] }], [new(Peer, Hp: 3) { StartingHand = [] }], new string('a', 64));
            var s = new Scene(r, GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4,
                AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, r)); s.Accept(new StartGameCommand()); s.Play(); return s;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-hua-tuo-mobile", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b, Hero, "boundary:hua-tuo", "classic:liao-hua");
            var removeChosen = new { kind = "choiceIs", sourceBind = "continue", optionId = "remove-source" };
            var skills = new[] {
                new { id = Watch, revision = 1, triggers = new[] {
                    new { id = "paid-hand", window = "cardsMoved", subject = "owner", sourceZones = new[] { "hand" },
                        movementOccurrence = "perBatch", movementReasons = new[] { "skill-program.boundary:qingnang-mobile.DiscardSelected" }, optional = false,
                        effects = new object[] {
                            new { op = "chooseOption", target = "owner", resultBind = "continue", options = new[] { new { id = "continue" }, new { id = "remove-source" } } },
                            new { op = "replaceSkillsOnAwakening", target = "owner", skillIds = new[] { "standard:none" }, condition = removeChosen },
                            new { op = "chooseOption", target = "owner", resultBind = "after-removal", options = new[] { new { id = "finish" } }, condition = removeChosen }
                        } }
                } }
            };
            var labels = new Dictionary<string, object> { [Watch] = new { name = "实际青囊弃牌孩子", description = "仅原弃牌已支付后暂停", optionLabels = new Dictionary<string, string> {
                ["continue"] = "继续原目标回复", ["remove-source"] = "先失去原青囊来源", ["finish"] = "失源后继续原目标回复" } } };
            var c = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            var replacements = Enumerable.Range(0, 6).Select(i => "fixture:mobile-hua-tuo-dummy-" + i).ToArray();
            foreach (var id in replacements) b.AddSkill(new(id, "真实原生替换资格", "令原生技能替换达到技能数资格。"));
            b.AddGeneral(new(Owner, "移动青囊真实拥有者", "", Skill, "qun", 3, [Watch, "classic:dangxian", .. replacements])); b.AddGeneral(new(Peer, "移动青囊真实回复目标", "", "standard:none", "wu", 5));
            b.AddDeck(new(Deck, "固定真实青囊实体", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 64).Select(i => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, i % 13 + 1)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "移动青囊原生回复", 4, 4, new Dictionary<string, int>(), Deck, GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
