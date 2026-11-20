using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class OlBuZhiDingpanRoleQuotaChecks
{
    private const string Hero = "ol:bu-zhi", Dingpan = "ol:dingpan", Activation = "equipment-choice";
    private const string Mode = "identity:classic-dingpan-role-quota", Deck = "fixture:dingpan-role-quota-deck";

    internal static void TwoLivingRebelsFundTwoNativeEquipmentChoicesCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(),
            new StandardRescueSkillExpansionPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, ConfirmOpeningHand = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Pending() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot Owner() => game.CreateSnapshot(0).Players[0];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        ProgramPublicRuleStateSnapshot Quota() => Owner().SkillRuntimeStates!.Single(skill => skill.SkillId == Dingpan)
            .PublicRuleStates!.Single(state => state is { Kind: ProgramPublicRuleStateKind.ActivationLimit, StateId: Activation, Scope: SkillUsageScope.Phase });
        LegalAction? Opportunity() => game.GetHumanLegalActions().SingleOrDefault(action =>
            action.ProgramSkillId == Dingpan && action.ProgramActivationId == Activation);
        bool AtPlay() => Pending() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0;
        static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");

        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}, phase {game.State.Phase}, " +
                    $"prompt {Pending()?.Kind}/{Pending()?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Dingpan fixture command was rejected.");
        }
        void Answer(Func<PromptChoice, bool> pick)
        {
            var prompt = Pending()!;
            Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(pick).Id, game.Revision));
        }
        void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 100 && !ready(); step++)
            {
                if (Pending() is { SkillPrompt.SkillId: "ol:hongde" })
                    Answer(choice => Branch(choice) == "decline");
                else
                {
                    Need(Pending() is null && game.State.Winner == Winner.None, "The Dingpan fixture reached an unexpected boundary.");
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
            }
            Need(ready(), "The required native Dingpan boundary was not reached.");
        }

        int Equip()
        {
            var action = game.GetHumanLegalActions().First(candidate => candidate.Kind == LegalActionKind.Equip && candidate.CardId is not null);
            var card = action.CardId!.Value;
            Submit(new PlayCardCommand(0, card, action.TargetSeats, game.Revision, Pending()!.PromptId, action.PlayedCardKind));
            Reach(AtPlay);
            Need(Owner().Equipment.Single().Id == card && game.CardMovements.Count(move => move.CardId == card &&
                move.To == CardLocation.Equipment(0) && move.Reason == CardMoveReasons.EquipmentEnter) == 1,
                "A real physical hand card entered the owner equipment zone through ordinary use.");
            return card;
        }

        UseProgramSkillCommand Activate() => new(0, Dingpan, Activation, [], [0], game.Revision, Pending()!.PromptId);

        Submit(new StartGameCommand());
        Reach(() => Pending() is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 });
        Submit(new SelectGeneralCommand(0, Hero, game.Revision, Pending()!.PromptId));
        Reach(AtPlay);
        var turn = game.State.TurnNumber;
        var seats = game.CreateSnapshot(0, revealAll: true).Players;
        Need(seats.Count(player => player is { Role: Role.Rebel, IsAlive: true }) == 2 &&
            seats.Count(player => player is { Role: Role.Renegade, IsAlive: true }) == 1 && Owner().Role == Role.Lord &&
            Quota() is { Value: 2, Used: 0 } && Opportunity() is null,
            "Two living Rebels establish the public phase cap, while a living Renegade and an unequipped table do not qualify as quota or targets.");

        var paidEquipment = new List<int>();
        for (var used = 0; used < 2; used++)
        {
            var equipment = Equip();
            var opportunity = Opportunity();
            Need(Quota().Used == used && opportunity is { MinTargetCount: 1, MaxTargetCount: 1 } &&
                opportunity.SelectableTargetSeats.SequenceEqual([0]),
                "The current phase allowance offers the exact living equipped participant.");
            var beforeMoves = game.CardMovements.Count;
            Submit(Activate());
            Reach(() => Pending() is { PlayerSeat: 0, SkillPrompt.SkillId: Dingpan } prompt && prompt.Choices.Any(choice => Branch(choice) == "discard"));
            var entry = Facts<BuZhiStartedEvent>().Last(fact => fact.Source.SkillId == Dingpan);
            var held = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == entry.FrameId);
            var paidDraw = game.CardMovements.Skip(beforeMoves).Single(move => move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Hand(0) && move.Reason.Value == "program.bu-zhi.draw");
            Need(entry is { Target: 0 } && entry.Turn == turn && held.BuZhi is { Stage: "branch" } &&
                Facts<BuZhiMovementPaidEvent>().Count(fact => fact.FrameId == entry.FrameId && fact.ReturnStage == "branch") == 1,
                "Each activation truly draws one original entity and waits on the exact equipped recipient's branch.");
            if (used == 0)
            {
                var before = game;
                game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(before.CreateCheckpoint())), registry);
                Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(before.CreateSnapshot(seat))) &&
                    JsonSerializer.Serialize(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == entry.FrameId)) == JsonSerializer.Serialize(held) &&
                    game.CardMovements.SequenceEqual(before.CardMovements),
                    "Cold restore retains the original paid draw, equipped entity and pending Dingpan choice.");
            }

            Answer(choice => Branch(choice) == "discard");
            Reach(() => Pending()?.Choices.Any(choice => Branch(choice) == "pay-discard") == true);
            Answer(choice => Branch(choice) == "pay-discard");
            Reach(AtPlay);
            paidEquipment.Add(equipment);
            Need(Quota() is { Value: 2 } limit && limit.Used == used + 1 && Owner().Equipment.Count == 0 &&
                Owner().Hand.Any(card => card.Id == paidDraw.CardId) &&
                game.CardMovements.Count(move => move.CardId == equipment && move.From == CardLocation.Equipment(0) &&
                    move.To == CardLocation.DiscardPile && move.Reason.Value == "program.bu-zhi.equipment") == 1 &&
                Facts<BuZhiMovementPaidEvent>().Count(fact => fact.FrameId == entry.FrameId && fact.ReturnStage == "complete" &&
                    fact.CardIds.SequenceEqual([equipment])) == 1,
                "The public choice pays only the selected physical equipment and records exactly one phase use.");
        }

        var thirdEquipment = Equip();
        var revision = game.Revision;
        var eventCount = game.Events.Count;
        var movementCount = game.CardMovements.Count;
        Need(Quota() is { Value: 2, Used: 2 } && Opportunity() is null && Owner().IsAlive &&
            !paidEquipment.Contains(thirdEquipment) && Owner().Equipment.Single().Id == thirdEquipment,
            "A fresh equipped target remains available as material when the native Dingpan allowance is exhausted.");
        var rejected = game.Submit(Activate());
        Need(!rejected.Accepted && rejected.Error?.Code == CommandErrorCode.IllegalAction &&
            game.Revision == revision && game.Events.Count == eventCount && game.CardMovements.Count == movementCount &&
            Quota() is { Value: 2, Used: 2 } && Facts<BuZhiStartedEvent>().Count(fact => fact.Source.SkillId == Dingpan) == 2 &&
            Owner().Equipment.Single().Id == thirdEquipment,
            "Submitting the third activation is rejected without paying, starting a child or touching the legal equipped entity.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new StandardClassicGeneralPackage().Manifest;

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var peers = Enumerable.Range(1, 3).Select(index => $"fixture:dingpan-role-quota-peer-{index}").ToArray();
            foreach (var peer in peers)
                builder.AddGeneral(new(peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "定叛固定实体连弩", 4, 0, [new("standard:crossbow", 32)]));
            builder.AddMode(new(Mode, "两名反贼的定叛额度", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 },
                Deck, GeneralCandidateCount: 4, GeneralPoolIds: [Hero, .. peers]));
        }
    }
}
