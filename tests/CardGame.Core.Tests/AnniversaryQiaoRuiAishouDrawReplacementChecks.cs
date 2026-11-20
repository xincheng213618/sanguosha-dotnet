using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryQiaoRuiAishouDrawReplacementChecks
{
    private const string Hero = "anniversary:qiao-rui", Aishou = "anniversary:aishou", Zenrun = "ol:zenrun";
    private const string Peer = "fixture:aishou-draw-peer", Deck = "fixture:aishou-draw-deck";
    private const string DrawReason = "skill-program.anniversary:aishou.draw";
    private const string TakeReason = "skill-program.sun-hong.zenrun.obtain";
    private const string CompensationReason = "skill-program.sun-hong.zenrun.compensation";

    internal static void EndingZenrunReplacementReturnsWithoutAiMarksCold()
    {
        const int owner = 0, source = 1, count = 4;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "aishou-ending-replacement", Hero, count, 0, 0, 0, 0, [Zenrun],
            [], [], [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer) { StartingHand = Enumerable.Range(1, count).Select(rank => new ShanheStartingCard(CardKind.Dodge, Suit.Club, rank)).ToArray() }],
            new string('c', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt(int seat = owner) => game.CreateSnapshot(seat).PendingDecision;
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}/{game.State.Phase}, " +
                    $"owner prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}, source prompt {Prompt(source)?.Kind}/{Prompt(source)?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The actual Aishou command was rejected.");
        }
        bool Has(string key, string value) => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault(key) == value) == true;
        void Choose(string key, string value, Func<PromptChoice, bool>? matches = null)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.First(item => item.Parameters.GetValueOrDefault(key) == value && (matches is null || matches(item)));
            Submit(new AnswerPromptCommand(owner, prompt.PromptId, choice.Id, game.Revision));
        }
        void Reach(Func<bool> ready, bool beforePlay = false)
        {
            for (var step = 0; step < 60 && !ready(); step++)
            {
                if (beforePlay && game.State.Phase == TurnPhase.Draw && Prompt()?.SkillPrompt?.SkillId == Zenrun && Has("program-action", "skip"))
                    Choose("program-action", "skip");
                else
                {
                    Need(Prompt() is null && game.State.Winner == Winner.None, "The game reached an unexpected public boundary.");
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
            }
            Need(ready(), "The game did not reach the expected Aishou or Zenrun boundary.");
        }

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: owner }, beforePlay: true);
        var turn = game.State.TurnNumber;
        var original = Player(source).Hand.Select(card => card.Id).ToArray();
        Need(Player(owner).MaxHp == count && original.Length == count, "Aishou and Zenrun have four actual original materials.");
        Submit(new EndPlayPhaseCommand(owner, game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Aishou && Has("program-action", "activate"));
        Choose("program-action", "activate");
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Zenrun && Has("program-action", "activate"));

        var started = Facts<AnniversaryQiaoRuiStartedEvent>().Single();
        var request = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        Need(started.Operation == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouDraw && started.Turn == turn &&
            game.ResolutionStack.OfType<TurnEndingBoundaryFrame>().Any(frame => frame.Id == started.ParentId && frame.OwnerSeat == owner) &&
            request.Context is { RecipientSeat: owner, RequestedCount: count, ActualPhase.Kind: ActualDiscardRecoveryPhaseKind.Ending } &&
            request.Context.ParentFrameId == root.Id && request.Context.Reason.Value == DrawReason && request.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation &&
            root.AnniversaryQiaoRui is { Stage: AnniversaryQiaoRuiStage.Drawing } issued && issued.Before == request.Context.SequenceBefore,
            "The original Aishou frame issues its Max-HP draw in the actual Ending phase before any replacement moves.");
        Choose("program-action", "activate");
        Reach(() => Has("branch", "target"));
        Choose("branch", "target", choice => choice.Targets.SequenceEqual([source]));
        for (var index = 0; index < count; index++)
            Choose("branch", "pick", choice => choice.Cards.Count == 0 && choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand));
        Choose("branch", "take");
        Reach(() => Prompt(source) is { PlayerSeat: source, SkillPrompt.SkillId: Zenrun } decision &&
            decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "draw"));

        NativeDrawReplacementResult? Payment() => game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == request.Id).Replacement;
        var paid = Payment();
        var compensationPrompt = Prompt(source)!.PromptId;
        Need(Prompt() is null && paid is { SourceSeat: source } && paid.GainedIds.Order().SequenceEqual(original.Order()) &&
            paid.SequenceAfter > request.Context.SequenceBefore && Facts<AnniversaryQiaoRuiDrawReturnedEvent>().Length == 0 &&
            Facts<AnniversaryQiaoRuiMarkedEvent>().Length == 0,
            "The four original entities have already moved under the native replacement while Aishou waits for foreign compensation.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Prompt(source)?.PromptId == compensationPrompt && Payment()?.GainedIds.Order().SequenceEqual(original.Order()) == true,
            "Cold restore preserves the original paid replacement and the source's private compensation choice.");
        Submit(new AdvanceOneStepCommand(game.Revision));
        Reach(() => Facts<TurnEndedEvent>().Any(e => e.ActorSeat == owner && e.TurnNumber == turn));

        var returned = Facts<AnniversaryQiaoRuiDrawReturnedEvent>().Single();
        var compensation = Facts<ZenrunCompensationReturnedEvent>().Single();
        var taken = game.CardMovements.Where(move => move.Reason.Value == TakeReason).ToArray();
        var compensationCards = game.CardMovements.Where(move => move.Reason.Value == CompensationReason).ToArray();
        Need(Facts<NativeDrawAttemptDecidedEvent>().Single(e => e.AttemptFrameId == request.Id) ==
                new NativeDrawAttemptDecidedEvent(request.Id, root.Id, owner, count, true) &&
            returned is { Requested: count, Actual: 0 } && returned.FrameId == root.Id && returned.Before == returned.After &&
            returned.Before >= compensation.SequenceAfter && taken.Select(move => move.CardId).Order().SequenceEqual(original.Order()) &&
            taken.All(move => move.From == CardLocation.Hand(source) && move.To == CardLocation.Hand(owner)) &&
            Player(owner).Hand.Count(card => original.Contains(card.Id) && card.SkillMarkLabel is null) == count &&
            !game.CardMovements.Any(move => move.Reason.Value == DrawReason) && Facts<AnniversaryQiaoRuiMarkedEvent>().Length == 0,
            "The original Aishou returns with zero DrawPile entities and gives none of the paid replacements an Ai mark.");
        Need(Facts<ZenrunCompensationRequestedEvent>().Single() is { RecipientSeat: source, RequestedCount: count } compensationRequest &&
            compensation is { ActualDrawCount: count, ReplacementGainedCount: 0 } && compensation.FrameId == compensationRequest.FrameId &&
            compensationCards.Length == count && compensationCards.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(source)) &&
            Facts<AnniversaryQiaoRuiCompletedEvent>().Single().FrameId == root.Id &&
            Facts<TurnEndedEvent>().Count(e => e.ActorSeat == owner && e.TurnNumber == turn) == 1,
            "The foreign seat draws its separate compensation, and the original Ending finishes exactly once.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-aishou-draw-replacement", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var sunHong = CharacterFixtureContent.EmbeddedProgramBundle("ol-sun-hong");
            var zenrun = sunHong.Presentations[Zenrun];
            builder.AddSkill(new(Zenrun, zenrun.Name, zenrun.Description)
                { Program = sunHong.Programs[Zenrun], ProgramPresentation = zenrun });
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "隘守替换固定牌堆", 0, 1, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "隘守结束阶段替换", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
