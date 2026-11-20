using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlTangZiXingzhaoWoundedOwnerChecks
{
    private const string Hero = "ol:tang-zi", Xingzhao = "ol:xingzhao", Xunxun = "boundary:xunxun";
    private const string Peer = "fixture:xingzhao-wounded-peer", Deck = "fixture:xingzhao-wounded-deck";

    internal static void RealWoundLendsXunxunAndOrdersNextActualDrawCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(),
            new StandardRescueSkillExpansionPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "xingzhao-wounded-draw", Hero, 4, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Spade, 2), new(CardKind.Dodge, Suit.Diamond, 3)], [],
            [new(Peer, Hp: 8) { StartingHand = [] }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Pending() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot Owner() => game.CreateSnapshot(0).Players[0];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        bool AtPlay() => Pending() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0;
        int[] Draws(int turn) => game.CardMovements.Where(move => move.TurnNumber == turn && move.From == CardLocation.DrawPile &&
            move.To == CardLocation.Hand(0) && move.Reason == CardMoveReasons.Draw).Select(move => move.CardId).ToArray();

        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}, phase {game.State.Phase}, " +
                    $"prompt {Pending()?.Kind}/{Pending()?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Xingzhao fixture command was rejected.");
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                Need(Pending() is null && game.State.Winner == Winner.None, "The Xingzhao fixture reached an unexpected boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The required native Xingzhao boundary was not reached.");
        }
        void Answer(Func<PromptChoice, bool> pick)
        {
            var prompt = Pending()!;
            Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));
        }

        Submit(new StartGameCommand());
        Reach(AtPlay);
        var firstTurn = game.State.TurnNumber;
        Need(game.CreateSnapshot(0).Players.All(player => player.Hp == player.MaxHp) && Draws(firstTurn).Length == 2 &&
            Owner().Skills?.All(skill => skill.ContentId != Xunxun) == true &&
            !Facts<ConditionalSkillGrantChangedEvent>().Any(fact => fact.SkillId == Xunxun),
            "With no living wounded player, the first normal Draw gives two cards and Xingzhao has not lent Xunxun.");

        var slash = Owner().Hand.Single(card => card.Kind == CardKind.Slash);
        var action = game.GetLegalActions(0).Single(candidate => candidate.CardId == slash.Id && candidate.ConversionSource is null &&
            candidate.TargetSeats.SequenceEqual([1]));
        Submit(new PlayCardCommand(0, slash.Id, action.TargetSeats, game.Revision, Pending()!.PromptId, action.PlayedCardKind));
        Reach(AtPlay);
        var declared = Facts<CardUseDeclaredEvent>().Single(fact => fact.CardId == slash.Id);
        var grant = Facts<ConditionalSkillGrantChangedEvent>().Single(fact => fact.OwnerSeat == 0 && fact.SkillId == Xunxun);
        Need(game.CardMovements.Count(move => move.CardId == slash.Id && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
            Facts<DamageAppliedEvent>().Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1 } damage &&
            damage.SourceCardUseResolutionId == declared.ResolutionId && game.CreateSnapshot(0).Players[1] is { Hp: 7, MaxHp: 8, IsAlive: true } &&
            grant is { Added: true, Source: { CapabilitySkillId: Xingzhao, RuleId: "wounded-xunxun" } } &&
            Owner().Skills?.Any(skill => skill.ContentId == Xunxun) == true,
            "One original physical Slash wounds the living opponent and produces the formal derived Xunxun grant.");

        Submit(new EndPlayPhaseCommand(0, game.Revision, Pending()!.PromptId));
        Reach(() => Pending()?.SkillPrompt?.SkillId == Xunxun);
        var nextTurn = game.State.TurnNumber;
        Need(game.State is { Phase: TurnPhase.Draw, CurrentSeat: 0 } && nextTurn > firstTurn && Draws(nextTurn).Length == 0,
            "The derived skill is offered at the next actual owner Draw, before ordinary cards move.");
        Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(() => Pending() is { Kind: DecisionKind.ProgramTopReorder, PlayerSeat: 0 });
        var frame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(active => active.SkillId == Xunxun);
        var order = frame.TopReorder!;
        var viewed = order.ViewedCardIds.ToArray();
        var hand = Owner().Hand.Select(card => card.Id).ToHashSet();
        Need(frame.SkillInstanceId == grant.SkillInstanceId && frame.ConditionalSourceReceipt is { } receipt &&
            receipt.Source == grant.Source && receipt.OnSourceLoss == ConditionalSkillGrantContinuation.FinishInvocation &&
            frame.WindowContext?.Window == SkillProgramTriggerWindow.DrawPhaseStarting && order.RequiredTopCount == 2 && viewed.Length == 4 &&
            game.CreateCardZoneDiagnostics().Where(card => card.Location == CardLocation.DrawPile).OrderByDescending(card => card.ZoneIndex)
                .Take(4).Select(card => card.CardId).SequenceEqual(viewed) && !viewed.Any(hand.Contains) &&
            Pending() is { IsPrivate: true } privateOrder && privateOrder.ValidCardIds.SequenceEqual(viewed) &&
            game.CreateSnapshot(1).PendingDecision is null,
            "The real borrowed skill privately owns the original top four entities and requires two top cards on its accepted frame.");

        var top = new[] { viewed[1], viewed[0] };
        var bottom = new[] { viewed[3], viewed[2] };
        Answer(choice => choice.Cards.SequenceEqual([top[0]]));
        var before = game;
        var held = before.ResolutionStack.OfType<ProgramSkillFrame>().Single(active => active.Id == frame.Id);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
        var restored = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(active => active.Id == frame.Id);
        Need(restored.TopReorder?.TopCardIds.SequenceEqual([top[0]]) == true &&
            JsonSerializer.Serialize(restored) == JsonSerializer.Serialize(held) && game.CardMovements.SequenceEqual(before.CardMovements) &&
            Enumerable.Range(0, 2).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(before.CreateSnapshot(seat))),
            "Cold replay retains the same paid borrowing receipt, the first top position and both participant views.");

        foreach (var id in top.Skip(1).Concat(bottom))
            Answer(choice => choice.Cards.SequenceEqual([id]));
        Reach(AtPlay);
        Need(Facts<TurnStartedEvent>().Where(fact => fact.ActorSeat == 0).Select(fact => fact.TurnNumber)
                .SequenceEqual([firstTurn, nextTurn]) && Draws(nextTurn).SequenceEqual(top) &&
            Owner().Hand.Select(card => card.Id).ToHashSet().SetEquals(hand.Concat(top)) &&
            game.CreateCardZoneDiagnostics().Where(card => card.Location == CardLocation.DrawPile).OrderBy(card => card.ZoneIndex)
                .Take(2).Select(card => card.CardId).SequenceEqual(bottom),
            "The next actual normal Draw moves exactly the chosen top two in their chosen order and leaves the other two at the physical bottom.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new StandardClassicGeneralPackage().Manifest;

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero, "boundary:li-dian");
            builder.AddGeneral(new(Peer, "固定相邻对手", "supporter", "standard:none", "shu", 8));
            builder.AddDeck(new(Deck, "兴棹固定摸牌", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "兴棹实际摸牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
