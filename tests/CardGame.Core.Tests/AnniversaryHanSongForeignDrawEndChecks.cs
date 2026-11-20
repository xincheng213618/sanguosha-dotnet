using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryHanSongForeignDrawEndChecks
{
    private const string Hero = "anniversary:han-song", Shuaiyan = "anniversary:shuaiyan", Zenrun = "ol:zenrun";
    private const string Peer = "fixture:shuaiyan-draw-end-peer", Watch = "fixture:shuaiyan-native-draw-watch";
    private const string Deck = "fixture:shuaiyan-draw-end-deck", DrawReason = "anniversary.shuaiyan.draw";

    internal static void EqualHandForeignNativeDrawOffersShuaiyanBeforeReturningCold()
    {
        const int owner = 0, subject = 1;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(7, 1, "shuaiyan-foreign-native-draw-end", Hero, 3, 0, 0, 0, 0, [Zenrun],
            [], [], [peer, peer], [peer with { ExtraSkillIds = [Watch] }], new string('b', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: owner }, skipOwnOffers: true);
        var startingHand = Player(owner).Hand.Select(card => card.Id).ToArray();
        Require(startingHand.Length == 2 && Player(subject).HandCount == 0,
            "Han Song keeps two real normal draw cards and the foreign seat has not drawn yet.");
        Submit(new EndPlayPhaseCommand(owner, game.Revision, Prompt()!.PromptId));
        Reach(() => ForeignOffer() || Facts<ActualDrawPhaseCompletedEvent>().Any(fact => fact.OwnerSeat == subject),
            skipOwnOffers: true);

        var request = Facts<NativeDrawAttemptStartedEvent>().Single(fact => fact.Context.RecipientSeat == subject &&
            fact.Context.Reason == CardMoveReasons.Draw);
        var turn = request.Context.ActualTurnNumber;
        var draws = game.CardMovements.Where(move => move.TurnNumber == turn && move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(subject) && move.Reason == CardMoveReasons.Draw).ToArray();
        Require(request.Return.Kind == NativeDrawAttemptReturnKind.DrawPhase && request.Context.RequestedCount == 2 &&
            request.Context.ActualPhase is { ActorSeat: subject, Kind: ActualDiscardRecoveryPhaseKind.Draw } &&
            Facts<NativeDrawAttemptDecidedEvent>().Single(fact => fact.AttemptFrameId == request.Context.AttemptFrameId) ==
                new NativeDrawAttemptDecidedEvent(request.Context.AttemptFrameId, request.Context.ParentFrameId, subject, 2, false) &&
            draws.Length == 2 && Player(owner).HandCount == 2 && Player(subject).HandCount == 2,
            "The original foreign native Draw really places two pile cards in hand, matching Han Song's two cards.");
        Require(ForeignOffer(),
            "Equal-hand Shuaiyan must be offered before the original foreign native Draw completes.");

        var native = game.ResolutionStack.OfType<DrawPhaseObligationFrame>().Single(frame => frame.Id == request.Context.ParentFrameId);
        var ending = game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(frame =>
            frame.Window == SkillProgramTriggerWindow.DrawPhaseEnded);
        Require(native.OwnerSeat == subject && native.Stage == DrawPhaseObligationStage.ConsumeMarker &&
            native.ActiveWindowFrameId == ending.Id && ending.ResumeDrawPhaseObligationFrameId == native.Id &&
            ending is { OwnerSeat: subject, Continuation: ProgramLifecycleContinuation.CompleteDrawPhaseEnded } &&
            ending.Candidates.Count(candidate => candidate.OwnerSeat == owner && candidate.SkillId == Shuaiyan &&
                candidate.BindingId == "other-draw-ended") == 1 &&
            Facts<ActualDrawPhaseCompletedEvent>().All(fact => fact.FrameId != native.Id),
            "The one foreign Shuaiyan offer lives on the original native Draw-ended window while it still owns its return.");
        var offerId = Prompt()!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(ForeignOffer() && Prompt()?.PromptId == offerId && game.ResolutionStack.OfType<DrawPhaseObligationFrame>()
            .Single(frame => frame.Id == native.Id).ActiveWindowFrameId == ending.Id,
            "Cold replay preserves the foreign offer and its original native phase and observer cursor.");

        Choose("program-action", "activate");
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Shuaiyan && HasChoice("verb", "draw"));
        var started = Facts<AnniversaryHanSongStartedEvent>().Single();
        Require(started is { SubjectSeat: subject, DrawCount: 1 } && started.ParentId == ending.Id &&
            started.Source is { OwnerSeat: owner, BindingId: "other-draw-ended" } && HasChoice("verb", "discard"),
            "Han Song can choose either benefit from the original foreign Draw subject.");
        Choose("verb", "draw");
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Zenrun && HasChoice("program-action", "skip"));
        var benefit = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        Require(benefit.Context is { RecipientSeat: owner, RequestedCount: 1 } && benefit.Context.ParentFrameId == started.FrameId &&
            benefit.Context.ActualPhase == request.Context.ActualPhase && benefit.Context.Reason.Value == DrawReason,
            "The chosen ordinary Draw1 belongs to Han Song while keeping the original foreign actual phase.");
        Choose("program-action", "skip");
        Reach(() => Facts<ActualDrawPhaseCompletedEvent>().Any(fact => fact.FrameId == native.Id));

        var gain = game.CardMovements.Single(move => move.Reason.Value == DrawReason);
        var returned = Facts<AnniversaryHanSongDrawReturnedEvent>().Single();
        var history = game.Events.Select(entry => entry.Payload).ToArray();
        var completed = Facts<AnniversaryHanSongCompletedEvent>().Single();
        var completedDraw = Facts<ActualDrawPhaseCompletedEvent>().Single(fact => fact.FrameId == native.Id);
        Require(returned.FrameId == started.FrameId && returned.Count == 1 && gain.From == CardLocation.DrawPile &&
            gain.To == CardLocation.Hand(owner) && gain.Sequence > returned.Before && gain.Sequence <= returned.After &&
            Player(owner).Hand.Select(card => card.Id).Order().SequenceEqual(startingHand.Append(gain.CardId).Order()) &&
            Facts<ZenrunPaidEvent>().Length == 0 && Facts<AnniversaryHanSongDiscardPaidEvent>().Length == 0,
            "Declining replacement leaves one real pile-to-hand Draw for Han Song and a single benefit return.");
        Require(completed.FrameId == started.FrameId && completedDraw is
            { OwnerSeat: subject, DrawCount: 2, Skipped: false, IsExtra: false } && completedDraw.ActualTurnNumber == turn &&
            Array.IndexOf(history, completed) < Array.IndexOf(history, completedDraw) &&
            Player(subject).Hand.Select(card => card.Id).Order().SequenceEqual(draws.Select(move => move.CardId).Order()),
            "Shuaiyan settles first, then the original foreign native Draw completes once with only its two original cards.");

        PendingDecision? Prompt() => game.CreateSnapshot(owner).PendingDecision;
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        T[] Facts<T>() => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        bool HasChoice(string key, string value) => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault(key) == value) == true;
        bool ForeignOffer() => game.State is { CurrentSeat: subject, Phase: TurnPhase.Draw } &&
            Prompt()?.SkillPrompt?.SkillId == Shuaiyan && HasChoice("program-action", "activate");
        void Choose(string key, string value)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault(key) == value);
            Submit(new AnswerPromptCommand(owner, prompt.PromptId, choice.Id, game.Revision));
        }
        void Reach(Func<bool> ready, bool skipOwnOffers = false)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                if (skipOwnOffers && game.State.CurrentSeat == owner && Prompt()?.SkillPrompt?.SkillId is Shuaiyan or Zenrun && HasChoice("program-action", "skip"))
                    Choose("program-action", "skip");
                else
                {
                    Require(Prompt() is null && game.State.Winner == Winner.None, "The fixture reached a different public command boundary.");
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
            }
            Require(ready(), "The fixture did not reach the original foreign native Draw boundary.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Require(result.Accepted, result.Error?.Message ?? "The native Draw fixture command was rejected.");
        }
        void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}/{game.State.Phase}, actor {game.State.CurrentSeat}, " +
                    $"owner prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-shuaiyan-foreign-draw-end", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var sunHong = CharacterFixtureContent.EmbeddedProgramBundle("ol-sun-hong");
            var zenrun = sunHong.Presentations[Zenrun];
            builder.AddSkill(new(Zenrun, zenrun.Name, zenrun.Description)
                { Program = sunHong.Programs[Zenrun], ProgramPresentation = zenrun });
            var watcher = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,
                  "triggers":[{"id":"observe-native-draw","window":"nativeDrawStarting","subject":"owner","optional":false,
                    "isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"seen",
                      "options":[{"id":"continue"}]}]}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                  "name":"原生摸牌观察","description":"确认原有摸牌请求。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, "原生摸牌观察", "确认原有摸牌请求。")
                { Program = watcher.Programs[Watch], ProgramPresentation = watcher.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "率言原生摸牌结束", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
