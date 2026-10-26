using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileGuoJiaChecks
{
    private const string Tiandu = "mobile:tiandu", Luoshen = "mobile:luoshen", Watch = "fixture:mobile-guo-jia-claimed";
    private const string Owner = "fixture:mobile-guo-jia-owner", Mode = "identity:classic-mobile-guo-jia";
    private const string ClaimReason = "skill-program.mobile:tiandu.claimJudgmentCard", LuoshenReason = "skill-program.mobile:luoshen.repeatJudgment";

    public static void TianduClaimsExactBlackLuoshenEntityAndPaidColdContinues()
    {
        var s = Scene.Start(); s.Reach(() => s.P?.SkillPrompt?.SkillId == Luoshen);
        var originalHand = s.V.Hand.Count;
        s.Answer(c => Action(c)?.EndsWith("activate", StringComparison.Ordinal) == true);
        s.Reach(s.AtTiandu);
        var original = s.Root; var frozen = original.RepeatedJudgment!.FinalOutcome!;
        s.Need(frozen is { FinalSuit: Suit.Spade, Matched: true } &&
            original.RepeatedJudgment.ClaimHandLimitExemption == SkillProgramClaimHandLimitExemption.None &&
            original.RepeatedJudgment.LastClaimReceipt is null && s.Zone(frozen.CardId) == CardLocation.Judgment(0),
            "Current mobile Tiandu is genuinely offered for Luoshen, whose original frame already owns the frozen black final entity.");
        s.Answer(c => Action(c)?.EndsWith("activate", StringComparison.Ordinal) == true);
        s.Reach(() => s.P?.SkillPrompt?.SkillId == Watch);
        var claimant = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Tiandu);
        var claimFrame = claimant.Id;
        s.Need(claimant.PendingMovementContinuation is not null && s.Root.Id == original.Id &&
            s.Root.RepeatedJudgment!.FinalOutcome == frozen && s.Root.RepeatedJudgment.LastClaimReceipt is null &&
            s.Zone(frozen.CardId) == CardLocation.Hand(0) && s.V.Hand.Count == originalHand + 1 &&
            s.Events<ProgramJudgmentCardClaimedEvent>().Count(e => e.SkillId == Tiandu && e.CardId == frozen.CardId &&
                e.JudgmentFrameId == frozen.JudgmentFrameId) == 1 && !s.Events<TurnHandLimitExemptCardGrantedEvent>().Any(),
            "The actual Tiandu child claims the same final entity once, pauses its paid gain on its own frame, and does not forge a Luoshen receipt or hand-limit exemption.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.P?.Kind == DecisionKind.ProgramRepeatJudgment);
        s.Need(s.Root.Id == original.Id && s.Root.RepeatedJudgment is { CompletedCount: 1, LastMatched: true, LastClaimReceipt: null } &&
            s.Root.RepeatedJudgment.FinalOutcome == frozen && s.Zone(frozen.CardId) == CardLocation.Hand(0) &&
            s.Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == claimFrame && e.Activated && e.Completed) == 1 &&
            s.G.CardMovements.Count(m => m.CardId == frozen.CardId && m.From == CardLocation.Judgment(0) && m.To == CardLocation.Hand(0)) == 1,
            "Cold drains the accepted Tiandu claim exactly once and returns to the original frozen black Luoshen continuation without paying or obtaining it again.");
        s.Answer(c => Action(c) == "continue"); s.Reach(s.AtTiandu);
        var second = s.Root.RepeatedJudgment!.FinalOutcome!;
        s.Need(second.Matched && second.CardId != frozen.CardId && second.JudgmentFrameId != frozen.JudgmentFrameId,
            "Continuing the original Luoshen starts a genuinely new judgment with another physical entity.");
        s.Answer(c => Action(c)?.EndsWith("skip", StringComparison.Ordinal) == true);
        s.Reach(() => s.P?.Kind == DecisionKind.ProgramRepeatJudgment);
        s.Need(s.Root.Id == original.Id && s.Root.RepeatedJudgment is { CompletedCount: 2, LastMatched: true,
                LastClaimReceipt: { GrantSequence: null } } && s.Zone(second.CardId) == CardLocation.Hand(0) &&
            s.G.CardMovements.Count(m => m.CardId == frozen.CardId && m.Reason.Value == ClaimReason) == 1 &&
            s.G.CardMovements.All(m => m.CardId != frozen.CardId || m.Reason.Value != LuoshenReason) &&
            s.G.CardMovements.Count(m => m.CardId == second.CardId && m.Reason.Value == LuoshenReason) == 1,
            "Declining Tiandu on the next black judgment lets Luoshen obtain only that new card, with no duplicate movement of the earlier claimed entity.");
        s.Answer(c => Action(c) == "stop"); s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        s.Need(s.V.Hand.Count == originalHand + 2 && s.V.Hand.Any(c => c.Id == frozen.CardId) &&
            s.V.Hand.Any(c => c.Id == second.CardId) && !s.Events<TurnHandLimitExemptCardGrantedEvent>().Any(),
            "Both independent actual claims survive native judgment cleanup, without an extra draw or boundary-version exemption.");
    }

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action") ?? choice.Parameters.GetValueOrDefault("action");

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PlayerSnapshot V => G.CreateSnapshot(0).Players[0];
        public PendingDecision? P => Enumerable.Range(0, 4).Select(i => G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p => p is not null);
        public ProgramSkillFrame Root => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Luoshen);
        public T[] Events<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int card) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == card).Location;
        public bool AtTiandu() => P?.Kind == DecisionKind.ProgramJudgmentTrigger &&
            G.ResolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().LastOrDefault() is { } window &&
            window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex].SkillId == Tiandu;
        public void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Mobile Guo Jia command rejected.");
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(predicate); Need(choice is not null, "The current native prompt lacks the requested answer.");
            Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 80; step++)
            {
                if (done()) return;
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The real Tiandu/Luoshen flow missed its named boundary.");
        }
        private string Dump() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold retains all viewers, the owning finalized judgment, exact black outcome and paid Tiandu/native gain frames.");
        }
        public void Need(bool value, string message)
        {
            if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
            {
                G.State.TurnNumber, G.State.Phase,
                Pending = P is { } p ? new { p.Kind, Skill = p.SkillPrompt?.SkillId, Choices = p.Choices.Select(Action).ToArray() } : null,
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(),
                Claims = Events<ProgramJudgmentCardClaimedEvent>(), Movements = G.CardMovements.TakeLast(6).ToArray()
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
            var scene = new Scene(registry, game); scene.Submit(new StartGameCommand());
            scene.Submit(new SelectGeneralCommand(0, Owner, game.Revision, scene.P!.PromptId)); return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:ordinary-mobile-guo-jia", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mobile:guo-jia", "mobile:zhen-ji", "classic:zhen-ji");
            var rules = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = new[] { new { id = Watch, revision = 1, triggers = new[] { new
                {
                    id = "paid-tiandu-claim", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" },
                    movementOccurrence = "perBatch", movementReasons = new[] { ClaimReason }, optional = false,
                    effects = new[] { new { op = "chooseOption", target = "owner", resultBind = "paid",
                        options = new[] { new { id = "continue" } } } }
                } } } }
            });
            var presentation = JsonSerializer.Serialize(new { schemaVersion = 3, skills = new Dictionary<string, object>
                { [Watch] = new { name = "Actual Tiandu gain", description = "Actual Tiandu gain", optionLabels = new { @continue = "Continue" } } } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            builder.AddSkill(new(Watch, "Actual Tiandu gain", "Actual Tiandu gain") { Program = catalog.Programs[Watch] });
            builder.AddSkill(new("fixture:mobile-guo-jia-none", "none", "none"));
            builder.AddGeneral(new(Owner, "owner", "supporter", Tiandu, "wei", 3, ["mobile:yiji", Luoshen, Watch]));
            var peers = Enumerable.Range(1, 3).Select(i => "fixture:mobile-guo-jia-peer-" + i).ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "other", "supporter", "fixture:mobile-guo-jia-none", "wei", 6, []));
            builder.AddDeck(new("fixture:mobile-guo-jia-deck", "fixed", 4, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 64).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray()
            });
            builder.AddMode(new(Mode, "ordinary mobile Guo Jia", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                "fixture:mobile-guo-jia-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
