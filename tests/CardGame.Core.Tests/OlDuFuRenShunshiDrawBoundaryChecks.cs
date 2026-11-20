using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlDuFuRenShunshiDrawBoundaryChecks
{
    private const string Actor = "fixture:shunshi-yaozhuo-owner", Beneficiary = "fixture:shunshi-shuangshu-owner";
    private const string Peer = "fixture:shunshi-draw-boundary-peer", Deck = "fixture:shunshi-draw-boundary-deck";
    private const string Shunshi = "ol:shunshi", Shuangshu = "happy:shuangshu", Yaozhuo = "mobile:yaozhuo";
    private const string GiftReason = "skill-program.du-fu-ren.shunshi.gift";

    internal static void OffTurnGiftFundsFirstUnskippedDrawCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        VerifyFirstDraw(registry, skipInsertedDraw: false);
        VerifyFirstDraw(registry, skipInsertedDraw: true);
    }

    private static void VerifyFirstDraw(ContentRegistry registry, bool skipInsertedDraw)
    {
        const int actor = 0, owner = 1, recipient = 2;
        var quiet = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(47, 1, "shunshi-first-real-draw", Actor, 4, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Club, 7), new(CardKind.Dodge, Suit.Heart, 13)], [],
            [new(Beneficiary) { StartingHand = [new(CardKind.Slash, Suit.Club, 2), new(CardKind.Slash, Suit.Spade, 3)] }, quiet, quiet],
            [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = actor, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(new StartGameCommand());
        Reach(ActorPlay);
        var openingTurn = game.State.TurnNumber;
        var attack = Player(actor).Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var giftCard = Player(owner).Hand.Single(card => card.Rank == 2).Id;
        var opposed = Player(owner).Hand.Single(card => card.Rank == 3).Id;
        var slash = game.GetLegalActions(actor).Single(action => action.Kind == LegalActionKind.Slash && action.CardId == attack &&
            action.ConversionSource is null && action.TargetSeats.SequenceEqual([owner]));
        Submit(new PlayCardCommand(actor, attack, slash.TargetSeats, game.Revision, Prompt()!.PromptId, slash.PlayedCardKind));
        Reach(() => ShunshiChoice("activate"));
        var damageWindow = game.ResolutionStack.OfType<DamageTriggerWindowFrame>().Single(frame => frame.TriggerWindow == SkillProgramTriggerWindow.AfterDamageApplied);
        Need(game.State.CurrentSeat == actor && damageWindow.TargetSeat == owner &&
            Facts<DamageAppliedEvent>() is [{ SourceSeat: actor, TargetSeat: owner, Amount: 1 }],
            "The original physical Slash causes real foreign-turn damage before any Shunshi grant is issued.");
        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(() => Prompt() is { PlayerSeat: owner, SkillPrompt.SkillId: Shunshi } &&
            Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option") == "gift"));
        Answer(choice => choice.Parameters.GetValueOrDefault("option") == "gift" &&
            choice.Cards.SequenceEqual([giftCard]) && choice.Targets.SequenceEqual([recipient]));
        Reach(ActorPlay);

        var issued = Facts<DuFuRenGiftIssuedEvent>().Single();
        var original = Facts<DuFuRenStartedEvent>().Single(fact => fact.FrameId == issued.FrameId);
        Need(original.Source is { SkillId: Shunshi, OwnerSeat: owner } && original.Source == issued.Source &&
            original.DamageFrameId == damageWindow.ParentFrameId && original.WindowId == damageWindow.Id && original.Turn == openingTurn &&
            original.TurnOwner == actor && issued is { RecipientSeat: recipient, From: var from } && from == CardLocation.Hand(owner) && issued.CardId == giftCard &&
            GiftPaid() && DrawClaims().Length == 0,
            "The off-turn trigger pays exactly one physical Shunshi gift and leaves its Draw grant unused.");

        if (skipInsertedDraw)
        {
            var cost = Player(actor).Hand.Single(card => card.Rank == 13).Id;
            Submit(new UseProgramSkillCommand(actor, Yaozhuo, "contest", [cost], [owner], game.Revision, Prompt()!.PromptId));
            Reach(ActorPlay);
            Need(Facts<PindianResultDeterminedEvent>() is [{ SkillId: Yaozhuo, Result: { SourceSeat: actor, OpponentSeat: owner, SourceWon: true } } result] &&
                result.Result.SourceCardId == cost && result.Result.OpponentCardId == opposed,
                "The real Yaozhuo contest spends both original hand cards and schedules its opponent's next Draw skip.");
        }

        var lastPrompt = Prompt()!.PromptId;
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Prompt()?.PromptId == lastPrompt && Facts<DuFuRenGiftIssuedEvent>() is [var restored] && restored == issued &&
            GiftPaid() && DrawClaims().Length == 0,
            "Cold restore keeps the exact paid off-turn gift unspent before the recipient's next actual Draw.");
        Submit(new EndPlayPhaseCommand(actor, game.Revision, Prompt()!.PromptId));
        Reach(() => game.State.CurrentSeat == owner && game.State.Phase == TurnPhase.Play, declinePreparationGift: true);

        var turn = game.State.TurnNumber;
        var history = game.Events.Select(entry => entry.Payload).ToArray();
        var schedule = Facts<ProgramPhaseScheduledEvent>().Where(fact => fact.SkillId == Shuangshu && fact.OwnerSeat == owner && fact.Phase == TurnPhase.Draw).ToArray();
        var phases = Facts<ActualDiscardRecoveryPhaseStartedEvent>().Where(fact => fact.Phase is { ActorSeat: owner, ActualTurnOwnerSeat: owner, Kind: ActualDiscardRecoveryPhaseKind.Draw } && fact.Phase.TurnNumber == turn).ToArray();
        var skips = Facts<ActualPhaseSkippedEvent>().Where(fact => fact.ActorSeat == owner && fact.TurnNumber == turn && fact.Kind == ActualDiscardRecoveryPhaseKind.Draw).ToArray();
        var draws = game.CardMovements.Where(move => move.TurnNumber == turn && move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(owner) && move.Reason == CardMoveReasons.Draw).ToArray();
        Need(schedule is [{ Started: true }, { Started: false }] && schedule[0].FrameId == schedule[1].FrameId &&
            Facts<TurnStartedEvent>().Any(fact => fact.TurnNumber == turn && fact.ActorSeat == owner),
            "The original Shuangshu insertion starts and returns during the actual beneficiary turn.");
        Need(phases.Length == (skipInsertedDraw ? 1 : 2) &&
            Array.IndexOf(history, schedule[1]) < Array.IndexOf(history, phases[^1]) &&
            DrawClaims().SequenceEqual([new(issued.FrameId, phases[0].Phase)]) &&
            Facts<DuFuRenGiftIssuedEvent>().SequenceEqual([issued]) && GiftPaid(),
            "The original paid grant is consumed once on the first real Draw and no preparation gift changes the count.");

        if (skipInsertedDraw)
        {
            Need(skips is [{ IsExtra: true } skipped] && skipped.NativeFrameId == schedule[0].FrameId &&
                Array.IndexOf(history, schedule[0]) < Array.IndexOf(history, skipped) && Array.IndexOf(history, skipped) < Array.IndexOf(history, schedule[1]) &&
                draws.Length == 3 && draws.All(move => move.Sequence > phases[0].MovementSequenceBefore),
                "The exact skipped Shuangshu phase spends no grant and the first real natural Draw gains three physical cards.");
        }
        else
        {
            Need(skips.Length == 0 && phases[0].Phase.Token != phases[1].Phase.Token &&
                Array.IndexOf(history, schedule[0]) < Array.IndexOf(history, phases[0]) && Array.IndexOf(history, phases[0]) < Array.IndexOf(history, schedule[1]) &&
                draws.Length == 5 && draws.Take(3).All(move => move.Sequence > phases[0].MovementSequenceBefore && move.Sequence <= phases[1].MovementSequenceBefore) &&
                draws.Skip(3).All(move => move.Sequence > phases[1].MovementSequenceBefore),
                "The real inserted phase gains three physical cards, then the independent natural Draw gains its ordinary two.");
        }

        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        PendingDecision? Prompt() => Enumerable.Range(0, game.PlayerCount).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
        bool ActorPlay() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: actor };
        bool ShunshiChoice(string action) => Prompt() is { PlayerSeat: owner, SkillPrompt.SkillId: Shunshi } prompt &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == action);
        T[] Facts<T>() => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        DuFuRenPhaseIssuedEvent[] DrawClaims() => Facts<DuFuRenPhaseIssuedEvent>().Where(fact => fact.GrantId == issued.FrameId && fact.Phase.Kind == ActualDiscardRecoveryPhaseKind.Draw).ToArray();
        bool GiftPaid() => game.CardMovements.Count(move => move.Sequence == issued.MovementSequence && move.CardId == giftCard &&
            move.From == CardLocation.Hand(owner) && move.To == CardLocation.Hand(recipient) && move.Reason.Value == GiftReason) == 1;

        void Answer(Func<PromptChoice, bool> select)
        {
            var prompt = Prompt()!;
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(select).Id, game.Revision));
        }

        void Reach(Func<bool> ready, bool declinePreparationGift = false)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                Need(game.State.Winner == Winner.None && Prompt()?.PlayerSeat != actor, "The fixture stopped at a different public decision.");
                if (declinePreparationGift && ShunshiChoice("skip"))
                    Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip");
                else
                    Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The fixture did not reach the expected public decision or actual phase.");
        }

        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The public Shunshi fixture command was rejected.");
        }

        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Skip inserted Draw: {skipInsertedDraw}. Turn {game.State.TurnNumber}/{game.State.Phase}, prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-shunshi-draw-boundary", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:du-fu-ren", "happy:shen-da-qiao-xiao-qiao", "mobile:shi-chang-shi");
            builder.AddGeneral(new(Actor, "谣诼来源", "supporter", Yaozhuo, "wei", 4));
            builder.AddGeneral(new(Beneficiary, "双姝顺世", "supporter", Shuangshu, "wei", 5, [Shunshi]));
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定闪牌堆", 0, 2, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 4), 32).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "顺世下次实际摸牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Actor, Beneficiary, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
