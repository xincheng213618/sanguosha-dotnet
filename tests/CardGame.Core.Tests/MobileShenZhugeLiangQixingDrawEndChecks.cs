using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileShenZhugeLiangQixingDrawEndChecks
{
    private const string XuHuang = "boundary:xu-huang", Jiezi = "boundary:jiezi-current";
    private const string ZhugeLiang = "mobile:shen-zhuge-liang", Qixing = "mobile-god:qixing";
    private const string Skip = "fixture:qixing-skip", Peer = "fixture:qixing-draw-peer", Deck = "fixture:qixing-draw-deck";
    private const string ExchangeReason = "program.private-reserve.exchange";

    internal static void InitialAndBothActualDrawEndsExchangeOriginalStarsCold()
    {
        const int xuHuang = 0, owner = 1;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var startingCard = new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 2);
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [startingCard] };
        var setup = new ShanheBattleSetup(7, 1, "qixing-each-draw-end", XuHuang, 4, 0, 0, 0, 0,
            [Skip], [startingCard], [], [peer, peer], [new(ZhugeLiang) { StartingHand = [startingCard] }], new string('b', 64))
            { DefaultGodFactionId = "wei", ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = xuHuang, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            ConfirmOpeningHand = true, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(new StartGameCommand());
        Need(Prompt() is { PlayerSeat: xuHuang, Kind: DecisionKind.OpeningHand },
            "The opening confirmation lets the fixture observe the other seat's real game-start choice.");
        Answer(choice => choice.Parameters.GetValueOrDefault("action") == "confirm");
        Reach(() => IsOffer(Qixing));
        var initial = QixingWindow();
        Need(initial.Window == SkillProgramTriggerWindow.GameStarting && initial.Candidates[initial.CandidateIndex].BindingId == "initial-exchange" &&
            Player(owner).PrivateReserveCards?.Count == 7 && !Facts<TurnStartedEvent>().Any(),
            "The actual seven starting stars precede the separate game-start exchange.");
        var initialStar = ExchangeOne(initial, Player(owner).Hand.Single().Id, []);

        Reach(() => IsOffer(Jiezi));
        var skipped = game.ResolutionStack.OfType<DrawPhaseObligationFrame>().Single();
        Need(skipped is { OwnerSeat: xuHuang, Skipped: true, IsExtra: false } &&
            Facts<DrawPhaseSkippedEvent>().Count(fact => fact.FrameId == skipped.Id) == 1 &&
            game.CreateSnapshot(xuHuang).Players.All(player => player.HandCount >= Player(owner).HandCount),
            "The real Xu Huang sees the original skipped Draw and the star owner is tied for fewest hand cards.");
        Answer(choice => Action(choice) == "activate");
        Reach(() => Prompt()?.Choices.Any(choice => Action(choice) == "select-targets" && choice.Targets.SequenceEqual([owner])) == true);
        Answer(choice => Action(choice) == "select-targets" && choice.Targets.SequenceEqual([owner]));
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: xuHuang });
        var marker = Facts<ProgramLeastHandMarkerOrDrawEvent>().Single();
        Need(marker is { OwnerSeat: xuHuang, TargetSeat: owner, SkillId: Jiezi, Marker: PlayerMarkerKind.Zi, GrantedMarker: true } &&
            Player(owner).Markers?.Any(item => item.Kind == PlayerMarkerKind.Zi && item.Count == 1) == true &&
            Facts<ExtraDrawPhaseStartedEvent>().Length == 0,
            "The accepted real Jiezi choice gives one attributed Zi before Zhuge Liang's turn.");

        Submit(new EndPlayPhaseCommand(xuHuang, game.Revision, Prompt()!.PromptId));
        Reach(() => IsOffer(Qixing));
        var turn = game.State.TurnNumber;
        var normal = ReadDrawEnd(extra: false);
        var normalCards = Draws(CardMoveReasons.Draw);
        Need(normalCards.Length == 2 && Draws(new("program.extra-draw-phase.draw")).Length == 0 &&
            Facts<ExtraDrawPhaseStartedEvent>().Length == 0 &&
            !Facts<ActualDrawPhaseCompletedEvent>().Any(fact => fact.FrameId == normal.Id),
            "The first Draw-end exchange offers after two real normal cards, before Zi starts the extra phase.");
        var firstStar = ExchangeOne(QixingWindow(), normalCards[0].CardId, [initialStar], cold: true);

        Reach(() => IsOffer(Qixing));
        var extra = ReadDrawEnd(extra: true);
        var extraCards = Draws(new("program.extra-draw-phase.draw"));
        var extraStarted = Facts<ExtraDrawPhaseStartedEvent>().Single();
        Need(extra.ParentFrameId == normal.Id && extraStarted.FrameId == extra.Id && extraStarted.ParentFrameId == normal.Id &&
            extraStarted.OwnerSeat == owner && extraStarted.ActualTurnNumber == turn && extraStarted.SourceSeat == xuHuang &&
            extra.DiscardRecoveryPhase!.Token != normal.DiscardRecoveryPhase!.Token && extraCards.Length == 2 &&
            Draws(CardMoveReasons.Draw).SequenceEqual(normalCards) &&
            !Facts<ActualDrawPhaseCompletedEvent>().Any(fact => fact.FrameId == extra.Id),
            "The second exchange waits for its own two physical cards in a different actual phase under the same turn.");
        ExchangeOne(QixingWindow(), extraCards[0].CardId, [initialStar, firstStar]);
        Reach(() => game.State.CurrentSeat == owner && game.State.Phase == TurnPhase.Play);

        var bindings = Facts<ProgramBindingStartedEvent>().Where(fact => fact.SkillId == Qixing &&
            fact.BindingId is "initial-exchange" or "draw-exchange").ToArray();
        var phases = Facts<ActualDrawPhaseCompletedEvent>().Where(fact => fact.OwnerSeat == owner && fact.ActualTurnNumber == turn).ToArray();
        Need(bindings.Select(fact => (fact.BindingId, fact.Window)).SequenceEqual([
                ("initial-exchange", SkillProgramTriggerWindow.GameStarting), ("draw-exchange", SkillProgramTriggerWindow.DrawPhaseEnded),
                ("draw-exchange", SkillProgramTriggerWindow.DrawPhaseEnded)]) && bindings.Select(fact => fact.FrameId).Distinct().Count() == 3 &&
            phases.Length == 2 && phases.Count(fact => fact.FrameId == normal.Id && !fact.IsExtra && !fact.Skipped) == 1 &&
            phases.Count(fact => fact.FrameId == extra.Id && fact.IsExtra && !fact.Skipped) == 1 &&
            Facts<ProgramMarkerExtraDrawConsumedEvent>().Count(fact => fact.FrameId == normal.Id && fact.TargetSeat == owner &&
                fact.SourceSeat == xuHuang && fact.SkillId == Jiezi && fact.SkillInstanceId == marker.SkillInstanceId) == 1 &&
            game.CardMovements.Count(move => move.Reason.Value == ExchangeReason) == 9,
            "One initial and two actual Draw-end exchanges return once each, with no shared fourth exchange or second Zi consumption.");

        DrawPhaseObligationFrame ReadDrawEnd(bool extra)
        {
            var window = QixingWindow();
            var candidate = window.Candidates[window.CandidateIndex];
            var actualTurn = game.State.TurnNumber;
            Need(candidate is { OwnerSeat: owner, SkillId: Qixing, BindingId: "draw-exchange" } &&
                window is { OwnerSeat: owner, Window: SkillProgramTriggerWindow.DrawPhaseEnded, Continuation: ProgramLifecycleContinuation.CompleteDrawPhaseEnded },
                $"Qixing must open on the original {(extra ? "extra" : "normal")} Draw end. Actual window {window.Window}, " +
                $"normal cards {Draws(CardMoveReasons.Draw).Length}, extra cards {Draws(new("program.extra-draw-phase.draw")).Length}, " +
                $"completed Draws {Facts<ActualDrawPhaseCompletedEvent>().Count(fact => fact.OwnerSeat == owner)}.");
            var phase = game.ResolutionStack.OfType<DrawPhaseObligationFrame>().Single(frame => frame.Id == window.ResumeDrawPhaseObligationFrameId);
            Need(phase.ActiveWindowFrameId == window.Id && phase.OwnerSeat == owner && phase.IsExtra == extra && !phase.Skipped &&
                phase.ActualTurnNumber == actualTurn && phase.DiscardRecoveryPhase is { ActorSeat: owner, ActualTurnOwnerSeat: owner, Kind: ActualDiscardRecoveryPhaseKind.Draw } token &&
                token.TurnNumber == actualTurn && Facts<ActualDiscardRecoveryPhaseStartedEvent>().Count(fact => fact.Phase == token) == 1,
                "The published candidate belongs to one exact actual Draw token and that phase's active lifecycle child.");
            return phase;
        }

        int ExchangeOne(ProgramLifecycleTriggerWindowFrame window, int handCard, int[] earlierStars, bool cold = false)
        {
            CheckPrivate();
            Answer(choice => Action(choice) == "activate");
            Reach(() => ReserveMode("exchange-hand"));
            var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Qixing);
            Need(frame.WindowContext?.ParentFrameId == window.Id && frame.TriggerId == window.Candidates[window.CandidateIndex].BindingId,
                "The owner's physical draft still belongs to the exact accepted binding and original parent.");
            Answer(choice => choice.Cards.SequenceEqual([handCard]));
            if (ReserveMode("exchange-hand"))
                Answer(choice => choice.Parameters.GetValueOrDefault("finish") == "true");
            Reach(() => ReserveMode("exchange-stars"));
            CheckPrivate();
            var star = Prompt()!.Choices.Select(choice => choice.Cards.Single()).First(card => !earlierStars.Contains(card));
            var promptId = Prompt()!.PromptId;
            if (cold)
            {
                var before = game.CreateSnapshot(owner);
                game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
                Need(Prompt()?.PromptId == promptId && SnapshotJson.Serialize(game.CreateSnapshot(owner)) == SnapshotJson.Serialize(before) &&
                    QixingWindow().Id == window.Id && ReadDrawEnd(extra: false).ActiveWindowFrameId == window.Id,
                    "Cold replay retains the exact private star choice and original actual Draw continuation.");
                CheckPrivate();
            }

            var beforeMoves = game.CardMovements.Count;
            Answer(choice => choice.Cards.SequenceEqual([star]));
            var swapped = game.CardMovements.Skip(beforeMoves).Where(move => move.Reason.Value == ExchangeReason).ToArray();
            var reserve = new CardLocation(CardZoneKind.PrivateReserve, owner);
            Need(swapped.Length == 3 && swapped[0].CardId == handCard && swapped[0].From == CardLocation.Hand(owner) && swapped[0].To == CardLocation.Processing &&
                swapped[1].CardId == star && swapped[1].From == reserve && swapped[1].To == CardLocation.Hand(owner) &&
                swapped[2].CardId == handCard && swapped[2].From == CardLocation.Processing && swapped[2].To == reserve &&
                Player(owner).PrivateReserveCards?.Any(card => card.Id == handCard) == true && Player(owner).Hand.Any(card => card.Id == star),
                "The two privately selected entities really exchange their original hand and reserve locations.");
            return star;
        }

        ProgramLifecycleTriggerWindowFrame QixingWindow() => game.ResolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>()
            .Single(window => window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex].SkillId == Qixing);
        static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
        bool IsOffer(string skill) => Prompt()?.SkillPrompt?.SkillId == skill && Prompt()!.Choices.Any(choice => Action(choice) == "activate");
        bool ReserveMode(string mode) => Prompt()?.SkillPrompt?.SkillId == Qixing && Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("reserve-mode") == mode);
        PendingDecision? Prompt() => Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        CardMovementRecord[] Draws(CardMoveReason reason) => game.CardMovements.Where(move => move.TurnNumber == game.State.TurnNumber &&
            move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner) && move.Reason == reason).ToArray();

        void CheckPrivate() => Need(Prompt() is { IsPrivate: true, PlayerSeat: owner } &&
            Enumerable.Range(0, game.PlayerCount).Where(seat => seat != owner).All(seat =>
                game.CreateSnapshot(seat) is { PendingDecision: null } view && view.Players[owner].PrivateReserveCards is null && view.Players[owner].PrivateReserveCount == 7),
            "Only the original owner sees the exchange choice and the seven reserve entities.");

        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                Need(Prompt() is null && game.State.Winner == Winner.None, "The fixture hit an unexpected decision.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The game did not reach the expected real Qixing or Jiezi window.");
        }

        void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = Prompt()!;
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(predicate).Id, game.Revision));
        }

        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The public Qixing command was rejected.");
        }

        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}/{game.State.Phase}, prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-qixing-actual-draw-end", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            // The registered Duliang descriptor enables the original actual-phase history for both Draws.
            CharacterFixtureContent.Register(builder, XuHuang, ZhugeLiang, "classic:li-yan", "mobile:li-yan");
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Skip}}","revision":1,"triggers":[
                  {"id":"skip-initial-draw","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,
                   "usageScope":"game","usageLimit":1,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["draw"]}]}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Skip}}":{"name":"跳过摸牌","description":"跳过自己的首次摸牌阶段。"} } }
                """);
            var shown = catalog.Presentations[Skip];
            builder.AddSkill(new(Skip, shown.Name, shown.Description) { Program = catalog.Programs[Skip], ProgramPresentation = shown });
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定星与摸牌牌堆", 0, 2, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "七星真实摸牌阶段结束", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [XuHuang, ZhugeLiang, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
