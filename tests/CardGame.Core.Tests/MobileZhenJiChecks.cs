using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileZhenJiChecks
{
    private const string Skill = "mobile:luoshen", Watch = "fixture:mobile-zhen-ji-paid";
    private const string Owner = "fixture:mobile-zhen-ji-owner", Mode = "identity:classic-mobile-zhen-ji";
    private const string GainReason = "skill-program.mobile:luoshen.repeatJudgment";

    public static void FrozenBlackClaimPaidColdAndOrdinaryHandLimit()
    {
        var scene = Scene.Start();
        scene.Reach(() => scene.P?.SkillPrompt?.SkillId == Skill);
        var initialHand = scene.G.CreateSnapshot(0).Players[0].Hand.Count;
        scene.Answer(c => Action(c)?.EndsWith("activate", StringComparison.Ordinal) == true);
        scene.Reach(scene.Watching);
        var first = scene.Root;
        var frozen = first.RepeatedJudgment!.FinalOutcome!;
        var receipt = first.RepeatedJudgment.LastClaimReceipt!;
        scene.Need(frozen is { FinalSuit: Suit.Spade, Matched: true } &&
            first.RepeatedJudgment.ClaimHandLimitExemption == SkillProgramClaimHandLimitExemption.None &&
            first.RepeatedJudgment.CompletedCount == 1 && receipt.JudgmentFrameId == frozen.JudgmentFrameId &&
            receipt.CardId == frozen.CardId && receipt.From == CardLocation.Judgment(0) && receipt.To == CardLocation.Hand(0) &&
            receipt.GrantSequence is null && scene.Zone(frozen.CardId) == CardLocation.Hand(0) &&
            scene.G.CreateSnapshot(0).Players[0].Hand.Count == initialHand + 1 &&
            !scene.Events<TurnHandLimitExemptCardGrantedEvent>().Any(),
            "A real black final entity is obtained once on the original exact frame, without a hand-limit grant.");
        var frameId = first.Id;
        scene.Cold();
        scene.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        scene.Reach(() => scene.P?.Kind == DecisionKind.ProgramRepeatJudgment);
        scene.Answer(c => Action(c) == "continue");
        scene.Reach(() => scene.P?.Kind == DecisionKind.ProgramRepeatJudgment);
        var second = scene.Root.RepeatedJudgment!;
        var secondCard = second.FinalOutcome!.CardId;
        scene.Need(scene.Root.Id == frameId && second.CompletedCount == 2 && second.FinalOutcome.Matched &&
            secondCard != frozen.CardId && second.LastClaimReceipt is { GrantSequence: null } &&
            scene.G.CreateSnapshot(0).Players[0].Hand.Count == initialHand + 2 &&
            scene.G.CardMovements.Count(m => m.CardId == frozen.CardId && m.Reason.Value == GainReason) == 1 &&
            scene.G.CardMovements.Count(m => m.CardId == secondCard && m.Reason.Value == GainReason) == 1 &&
            scene.Events<ProgramJudgmentCardClaimedEvent>().Count(e => e.SkillId == Skill) == 2 &&
            !scene.Events<TurnHandLimitExemptCardGrantedEvent>().Any(),
            "Cold resumes the paid gain once, and the same original frame can obtain a second distinct black judgment.");
        scene.Answer(c => Action(c) == "stop");
        scene.Reach(() => scene.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        scene.Submit(new EndPlayPhaseCommand(0, scene.G.Revision, scene.P!.PromptId));
        scene.Reach(() => scene.P is { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 });
        var discard = scene.P!;
        scene.Need(discard.RequiredCardCount == 2 && discard.ValidCardIds.Contains(frozen.CardId) &&
            discard.ValidCardIds.Contains(secondCard),
            "Both obtained cards count in the actual ordinary hand limit, producing two required discards this turn.");
        scene.Submit(new DiscardCardsCommand(0, [frozen.CardId, secondCard], discard.PromptId, scene.G.Revision));
        scene.Need(scene.Zone(frozen.CardId) == CardLocation.DiscardPile && scene.Zone(secondCard) == CardLocation.DiscardPile &&
            scene.G.CardMovements.Count(m => (m.CardId == frozen.CardId || m.CardId == secondCard) &&
                m.From == CardLocation.Hand(0) && m.To == CardLocation.DiscardPile && m.Reason.Value == "rule.hand-limit-discard") == 2,
            "The native hand-limit payment really discards both gained entities; no boundary exemption leaked into mobile Luoshen.");
    }

    private static string? Action(PromptChoice choice) =>
        choice.Parameters.GetValueOrDefault("program-action") ?? choice.Parameters.GetValueOrDefault("action");

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(s => G.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame Root => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Skill);
        public IEnumerable<T> Events<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public bool Watching() => P?.SkillPrompt?.SkillId == Watch && P.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Mobile Zhen Ji command rejected.");
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!;
            var choice = p.Choices.FirstOrDefault(predicate);
            Need(choice is not null, "The actual native prompt lacks the requested choice.");
            Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 80; step++)
            {
                if (done()) return;
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The actual native flow missed its named boundary.");
        }
        private string Dump() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(G.CreateSnapshot(s))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(),
            G.CardMovements, Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(),
            Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        {
            var before = Dump();
            G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves all viewer snapshots, the exact frozen outcome, original paid gain child and movement receipt.");
        }
        public void Need(bool value, string message)
        {
            if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
            {
                G.State.TurnNumber, G.State.Phase, G.State.CurrentSeat,
                Pending = P is { } p ? new { p.Kind, Skill = p.SkillPrompt?.SkillId, p.RequiredCardCount, p.ValidCardIds } : null,
                Repeated = G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.SkillId == Skill)?.RepeatedJudgment,
                Movements = G.CardMovements.TakeLast(6).ToArray()
            }));
        }
        public static Scene Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
            var game = GameEngine.CreateStandard(new GameOptions
            {
                Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
                UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, registry);
            var scene = new Scene(registry, game);
            scene.Submit(new StartGameCommand());
            scene.Submit(new SelectGeneralCommand(0, Owner, game.Revision, scene.P!.PromptId));
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:ordinary-mobile-zhen-ji", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mobile:zhen-ji", "classic:zhen-ji");
            var rules = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Watch, revision = 1, triggers = new[] { new
                {
                    id = "paid-first-gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                    movementOccurrence = "perBatch", movementReasons = new[] { GainReason }, usageScope = "game", usageLimit = 1,
                    optional = false, effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "paid",
                        options = new[] { new { id = "continue" } } } }
                } } } }
            });
            var presentation = JsonSerializer.Serialize(new { schemaVersion = 3, skills = new Dictionary<string, object>
                { [Watch] = new { name = "Paid gain", description = "Paid gain", optionLabels = new { @continue = "Continue" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Watch, "Paid gain", "Paid gain") { Program = catalog.Programs[Watch] });
            builder.AddSkill(new("fixture:mobile-zhen-ji-none", "none", "none"));
            builder.AddGeneral(new(Owner, "owner", "supporter", Skill, "wei", 3, ["classic:qingguo", Watch]));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:mobile-zhen-ji-peer-" + i).ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "other", "supporter", "fixture:mobile-zhen-ji-none", "wei", 6, []));
            builder.AddDeck(new("fixture:mobile-zhen-ji-deck", "fixed", 4, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray()
            });
            builder.AddMode(new(Mode, "ordinary mobile Zhen Ji", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                "fixture:mobile-zhen-ji-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
