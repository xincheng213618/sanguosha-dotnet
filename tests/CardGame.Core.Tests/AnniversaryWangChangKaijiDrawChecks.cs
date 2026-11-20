using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryWangChangKaijiDrawChecks
{
    private const string Hero = "anniversary:wang-chang", Kaiji = "anniversary:kaiji";
    private const string Peer = "fixture:kaiji-draw-peer", Deck = "fixture:kaiji-draw-deck";
    private const string Watch = "fixture:kaiji-before-draw-movement", DrawReason = "skill-program.anniversary:kaiji.draw";

    internal static void YangNativeDrawReturnsAfterPriorMovementCold()
    {
        const int owner = 0, amount = 3;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(47, 1, "kaiji-yang-native-draw-return", Hero, 3, 0, 0, 0, 0, [Watch],
            [new(CardKind.Dodge, Suit.Club, 13)], [], [new(Peer) { StartingHand = [] }], [], new string('e', 64))
        {
            ExtraSkillSlots = 1
        };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(new StartGameCommand());
        Reach(IsPlay);
        var turn = game.State.TurnNumber;
        var hand = Player().Hand;
        var material = hand.Single(card => card.Rank == 13).Id;
        var beforeIssue = game.CardMovements.Last().Sequence;
        Require(Player().MaxHp == amount && Polarity() == SkillPolarity.Yang &&
            game.GetLegalActions(owner).Any(action => action.ProgramSkillId == Kaiji),
            "The first actual Play offers the printed Yang conversion at maximum health three.");

        Submit(new UseProgramSkillCommand(owner, Kaiji, "convert", [], [], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Watch && Prompt()!.Choices.Any(choice => choice.Cards.SequenceEqual([material])));
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Kaiji);
        var request = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        var started = Facts<AnniversaryWangChangStartedEvent>().Single();
        var committed = Facts<ProgramConversionPolarityCommittedEvent>().Single();
        Require(root is { ActivationId: "convert", OwnerSeat: owner, ConversionPreviousPolarity: SkillPolarity.Yang,
                AnniversaryWangChang: { Polarity: SkillPolarity.Yang, Stage: AnniversaryWangChangStage.Drawing, Maximum: amount } } &&
            request.Context is { RecipientSeat: owner, RequestedCount: amount } && request.Context.ParentFrameId == root.Id &&
            request.Context.Reason.Value == DrawReason && request.Context.SequenceBefore == beforeIssue &&
            request.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation &&
            committed is { PreviousState: SkillPolarity.Yang, CurrentState: SkillPolarity.Yin, BindingId: "convert", SkillId: Kaiji } &&
            committed.FrameId == root.Id && committed.SkillInstanceId == root.SkillInstanceId && committed.TurnNumber == turn &&
            committed.PhaseInstanceId == started.Phase && Polarity() == SkillPolarity.Yin,
            "The real activation commits Yang to Yin once before opening its exact native Draw3 request.");

        Answer(choice => choice.Cards.SequenceEqual([material]));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Watch &&
            Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"));
        var stop = Prompt()!.PromptId;
        var paid = game.CardMovements.Single(move => move.CardId == material && move.Sequence > beforeIssue);
        Require(paid.From == CardLocation.Hand(owner) && paid.To == CardLocation.DiscardPile &&
            game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single().Id == request.Id &&
            game.CardMovements.All(move => move.Reason.Value != DrawReason) && Facts<AnniversaryWangChangDrawReturnedEvent>().Length == 0,
            "The pre-draw observer discards the actual selected card after issue and pauses before the original Draw3 can pay.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restoredAttempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        Require(Prompt()?.PromptId == stop && restoredAttempt.Id == request.Id && restoredAttempt.Context == request.Context &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == material).Location == CardLocation.DiscardPile &&
            Facts<ProgramConversionPolarityCommittedEvent>().Single() == committed,
            "Cold replay keeps the original request, paid physical discard, prompt and one conversion commitment.");
        Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(IsPlay);

        var returned = Facts<AnniversaryWangChangDrawReturnedEvent>().Single();
        var drawn = game.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray();
        var drawnIds = drawn.Select(move => move.CardId).ToArray();
        Require(returned.FrameId == root.Id && returned.Count == amount && returned.Before == paid.Sequence &&
            drawn.Length == amount && returned.After == drawn[^1].Sequence && drawnIds.Distinct().Count() == amount &&
            drawn.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner) &&
                move.Sequence > returned.Before && move.Sequence <= returned.After) &&
            game.CardMovements.Count(move => move.CardId == material && move.Sequence > beforeIssue) == 1 &&
            Player().Hand.Select(card => card.Id).Order().SequenceEqual(hand.Where(card => card.Id != material).Select(card => card.Id).Concat(drawnIds).Order()),
            "The same Yang request obtains its three real cards after the earlier discard and uses the actual movement range.");
        Require(Facts<NativeDrawAttemptDecidedEvent>().Single(fact => fact.AttemptFrameId == request.Id) ==
                new NativeDrawAttemptDecidedEvent(request.Id, root.Id, owner, amount, false) &&
            Facts<AnniversaryWangChangCompletedEvent>().Single().FrameId == root.Id &&
            Facts<ProgramConversionPolarityCommittedEvent>().Single() == committed && Polarity() == SkillPolarity.Yin &&
            Facts<AnniversaryWangChangDiscardPaidEvent>().Length == 0 && game.State.TurnNumber == turn &&
            !game.GetLegalActions(owner).Any(action => action.ProgramSkillId == Kaiji),
            "The original activation settles once, preserves Yin and spends the same Play-phase allowance.");

        PendingDecision? Prompt() => game.CreateSnapshot(owner).PendingDecision;
        PlayerSnapshot Player() => game.CreateSnapshot(owner).Players[owner];
        SkillPolarity? Polarity() => Player().SkillRuntimeStates!.Single(state => state.SkillId == Kaiji).Polarity;
        bool IsPlay() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: owner };
        T[] Facts<T>() => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.Single(predicate);
            Submit(new AnswerPromptCommand(owner, prompt.PromptId, choice.Id, game.Revision));
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                Require(Prompt() is null && game.State.Winner == Winner.None, "The fixture reached a different public command boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Require(ready(), "The fixture did not reach the expected original Kaiji boundary.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Require(result.Accepted, result.Error?.Message ?? "The native Kaiji fixture command was rejected.");
        }
        void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}/{game.State.Phase}, " +
                    $"owner prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-kaiji-draw-return", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var watcher = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,"triggers":[
                  {"id":"discard-before-native-draw","window":"nativeDrawStarting","subject":"owner","condition":{"kind":"phaseIsPlay"},
                   "usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[
                     {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                      "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true},
                     {"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
                ]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                  "name":"开济摸牌前弃置","description":"在真实摸牌请求内弃置一张手牌。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, "开济摸牌前弃置", "在真实摸牌请求内弃置一张手牌。")
                { Program = watcher.Programs[Watch], ProgramPresentation = watcher.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "固定对手", "supporter", "standard:none", "qun", 5));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 2, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 5), 20).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "开济阳项原生摸牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
