using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheGuanweiPaymentChecks
{
    private const string Skill = "ol:guanwei";
    private const string Owner = "fixture:guanwei-owner", Peer = "fixture:guanwei-peer";
    private const string PaidChild = "fixture:guanwei-paid-child", Deck = "fixture:guanwei-deck";

    public static void EmptyPaymentCancelsAndPaidMovementReturnsBeforeExtraPlay()
    {
        foreach (var canPay in new[] { false, true })
        {
            var s = Scene.Start(canPay);
            s.Use(CardKind.Alcohol, []); s.Use(CardKind.Slash, [1]);
            Need(s.V.Hand.Count == (canPay ? 1 : 0) && s.V.Equipment.Count == 0 &&
                s.G.Events.Select(e => e.Payload).OfType<CurrentTurnCardUseKindsRecordedEvent>()
                    .Count(e => e.ActorSeat == 0) == 2,
                "Two real same-suit uses arm Guanwei with exactly the authored payment stock.");
            var turn = s.G.State.TurnNumber;
            var original = canPay ? s.V.Hand.Single().Id : (int?)null;
            s.EndPlay();
            if (!canPay)
            {
                s.Play();
                Need(s.G.State.TurnNumber > turn && s.Draws().Length == 0 && s.Payments().Length == 0 &&
                    s.Schedules().Length == 0 && !s.G.IsFaulted &&
                    !s.G.Events.Select(e => e.Payload).OfType<ProgramBindingStartedEvent>().Any(e => e.SkillId == Skill),
                    "An empty hand and equipment area cannot activate Guanwei or receive a free draw and Play phase.");
                continue;
            }
            s.Reach(p => p.SkillPrompt?.SkillId == Skill && p.Choices.Any(c =>
                c.Parameters.GetValueOrDefault("program-action") == "activate"));
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
            s.ResolveCurrentTurnTarget();

            s.Reach(p => p.SkillPrompt?.SkillId == Skill && p.Choices.Any(c =>
                c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card"));
            s.Answer(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card" &&
                c.Cards.SequenceEqual([original!.Value]));
            s.Reach(p => p.SkillPrompt?.SkillId == PaidChild && p.Choices.Any(c =>
                c.Parameters.GetValueOrDefault("option-id") == "continue"));
            Need(s.Payments() is [{ CardId: var paid }] && paid == original && s.V.Hand.Count == 0 &&
                s.Draws().Length == 0 && s.Schedules().Length == 0 &&
                s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f =>
                    f.SkillId == Skill && f.PendingMovementContinuation is not null),
                "The actual one-card discard awaits its native movement child on the original Guanwei frame before any reward.");
            s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue"); s.Play();
            Need(s.G.State.TurnNumber == turn && s.Draws().Length == 2 && s.V.Hand.Count == 2 &&
                s.Schedules() is [{ OwnerSeat: 0, Phase: TurnPhase.Play, Started: true }] &&
                s.Payments().Length == 1 && s.G.Events.Select(e => e.Payload).OfType<SkillUsageConsumedEvent>()
                    .Count(e => e.SkillId == Skill) == 1,
                "A completed real cost draws two and enters the same living turn owner's extra Play once.");
            s.EndPlay(); s.Play();
            Need(s.G.State.TurnNumber > turn && s.Schedules() is [{ Started: true }, { Started: false }] &&
                s.Payments().Length == 1 && s.Draws().Length == 2 &&
                s.G.Events.Select(e => e.Payload).OfType<SkillUsageConsumedEvent>().Count(e => e.SkillId == Skill) == 1,
                "Returning from the inserted phase finishes the original grant without another payment, draw or same-turn activation.");
        }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine G { get; } = game;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        private PendingDecision? P => G.PendingDecision;
        public CardMovementRecord[] Payments() => G.CardMovements.Where(m =>
            m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile &&
            m.Reason.Value == $"skill-program.{Skill}.{SkillProgramEffectOp.SelectAndMoveOwnedCard}").ToArray();
        public CardMovementRecord[] Draws() => G.CardMovements.Where(m =>
            m.Reason.Value == $"skill-program.{Skill}.{SkillProgramEffectOp.Draw}").ToArray();
        public ProgramPhaseScheduledEvent[] Schedules() => G.Events.Select(e => e.Payload)
            .OfType<ProgramPhaseScheduledEvent>().Where(e => e.SkillId == Skill).ToArray();
        private void Submit(GameCommand command)
        { var result = G.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The native Guanwei command was rejected."); }
        public void Answer(Func<PromptChoice, bool> select)
        {
            var prompt = P ?? throw new InvalidOperationException("Missing native Guanwei decision.");
            var choice = prompt.Choices.FirstOrDefault(select) ?? throw new InvalidOperationException(JsonSerializer.Serialize(prompt));
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, G.Revision));
        }
        public void Reach(Func<PendingDecision, bool> done)
        {
            for (var step = 0; step < 120; step++)
            {
                if (P is { } prompt && done(prompt)) return;
                Need(P?.PlayerSeat != 0, "Unexpected Guanwei boundary: " + JsonSerializer.Serialize(P));
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            throw new InvalidOperationException("The bounded Guanwei continuation did not arrive.");
        }
        public void Play() => Reach(p => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void ResolveCurrentTurnTarget()
        {
            Reach(p => p.Kind == DecisionKind.PlayCard || p.SkillPrompt?.SkillId == Skill &&
                p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") is
                    "select-target" or "select-and-move-owned-card"));
            if (P!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target"))
                Answer(c => c.Parameters.GetValueOrDefault("program-action") == "select-target" && c.Targets.SequenceEqual([0]));
        }
        public void EndPlay() => Submit(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Use(CardKind kind, int[] targets)
        {
            var card = V.Hand.Single(c => c.Kind == kind);
            Submit(new PlayCardCommand(0, card.Id, targets, G.Revision, P!.PromptId)); Play();
        }
        public static Scene Start(bool canPay)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(canPay));
            var cards = new[] { new ShanheStartingCard(CardKind.Alcohol, Suit.Heart, 5),
                new ShanheStartingCard(CardKind.Slash, Suit.Heart, 7) }
                .Concat(canPay ? [new ShanheStartingCard(CardKind.Dodge, Suit.Club, 4)] : Array.Empty<ShanheStartingCard>()).ToArray();
            var setup = new ShanheBattleSetup(83, 1, "fixture:guanwei-payment", Owner, 20, 0, 0, 0, 0,
                [], cards, [], [new(Peer) { StartingHand = [] }], [], new string('d', 64));
            var s = new Scene(GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
                MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
            }, registry));
            s.Submit(new StartGameCommand()); s.Play(); return s;
        }
    }

    private sealed class Fixture(bool canPay) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-guanwei-payment", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:pan-jun");
            if (canPay)
            {
                var programs = SkillProgramCatalog.Load($$"""
                    {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                      {"id":"{{PaidChild}}","revision":1,"triggers":[{"id":"last-hand-payment-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","optional":false,"effects":[
                        {"op":"chooseOption","target":"owner","condition":{"kind":"not","children":[{"kind":"handCountAtLeast","value":1}]},"resultBind":"paid-return","options":[{"id":"continue"}]}
                      ]}]}
                    ]}
                    """, JsonSerializer.Serialize(new
                    {
                        schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                        skills = new Dictionary<string, object> { [PaidChild] = new
                            { name = "真实弃牌子窗口", description = "原生付款完成后继续原观微", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } }
                    }));
                builder.AddSkill(new(PaidChild, "真实弃牌子窗口", "原生付款完成后继续原观微")
                    { Program = programs.Programs[PaidChild], ProgramPresentation = programs.Presentations[PaidChild] });
            }
            builder.AddGeneral(new(Owner, "观微使用者", "supporter", Skill, "wu", 20, canPay ? [PaidChild] : []));
            builder.AddGeneral(new(Peer, "固定守军", "supporter", "standard:none", "wei", 20));
            builder.AddDeck(new(Deck, "固定无普通摸牌", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Club, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "观微真实付款小局", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
