using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryCuiLieZijueDrawReplacementChecks
{
    private const string CuiLie = "anniversary:cui-lie", Zijue = "anniversary:zijue", Zenrun = "ol:zenrun";
    private const string Peer = "fixture:zijue-replacement-peer", Deck = "fixture:zijue-replacement-deck";
    private const string DrawReason = "skill-program.anniversary:zijue.draw";
    private const string TakeReason = "skill-program.sun-hong.zenrun.obtain";
    private const string CompensationReason = "skill-program.sun-hong.zenrun.compensation";

    internal static void DeclinedGiftReturnsOriginalZenrunReplacementCold()
    {
        const int owner = 0, source = 1, count = 2;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "zijue-native-replacement", CuiLie, 4, 0, 0, 0, 0, [Zenrun],
            [new(CardKind.Dodge, Suit.Spade, 12), new(CardKind.Dodge, Suit.Spade, 13)], [],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [new(Peer)
            {
                StartingHand = [new(CardKind.Slash, Suit.Club, 1)],
                StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 3)]
            }], new string('d', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt(int seat = owner) => game.CreateSnapshot(seat).PendingDecision;
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}/{game.State.Phase}, " +
                    $"owner prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}, source prompt {Prompt(source)?.Kind}/{Prompt(source)?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The actual Zijue replacement command was rejected.");
        }
        bool ChoiceIs(string key, string value) => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault(key) == value) == true;
        void Choose(string key, string value, Func<PromptChoice, bool>? matches = null)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault(key) == value && (matches is null || matches(item)));
            Submit(new AnswerPromptCommand(owner, prompt.PromptId, choice.Id, game.Revision));
        }
        void Reach(Func<bool> ready, bool beforePlay = false)
        {
            for (var step = 0; step < 60 && !ready(); step++)
            {
                if (beforePlay && game.State.Phase == TurnPhase.Draw && Prompt()?.SkillPrompt?.SkillId == Zenrun && ChoiceIs("program-action", "skip"))
                    Choose("program-action", "skip");
                else
                {
                    Need(Prompt() is null && game.State.Winner == Winner.None, "The game reached a different public boundary.");
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
            }
            Need(ready(), "The game did not reach the expected Zijue or Zenrun choice.");
        }
        NativeDrawAttemptFrame Attempt(long id) => game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == id);

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: owner }, beforePlay: true);
        var turn = game.State.TurnNumber;
        var handBefore = Player(owner).Hand.Select(card => card.Id).ToArray();
        var originalHand = Player(source).Hand.Single().Id;
        var originalEquipment = Player(source).Equipment.Single().Id;
        var materials = new[] { originalHand, originalEquipment };
        Submit(new UseProgramSkillCommand(owner, Zijue, "declare-number", [], [source], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt(source) is { PlayerSeat: source, SkillPrompt.SkillId: Zijue });
        Submit(new AdvanceOneStepCommand(game.Revision));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Zijue && ChoiceIs("verb", "draw"));
        var number = Facts<AnniversaryCuiLieNumberEvent>().Single();
        Need(number is { Recipient: source, Number: count } && ChoiceIs("verb", "card"),
            "The real foreign declarer chooses two and Cui Lie can actually give the declared number.");
        Choose("verb", "draw");
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Zenrun && ChoiceIs("program-action", "activate"));
        var request = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == number.FrameId);
        Need(request.Context is { RecipientSeat: owner, RequestedCount: count } && request.Context.Reason.Value == DrawReason &&
            request.Context.ParentFrameId == number.FrameId && request.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation &&
            root.AnniversaryCuiLie is { Stage: AnniversaryCuiLieStage.Drawing } && root.AnniversaryCuiLie.Before == request.Context.SequenceBefore,
            "Only the original Zijue frame has issued this Draw2 request.");
        Choose("program-action", "activate");
        Reach(() => ChoiceIs("branch", "target"));
        Choose("branch", "target", choice => choice.Targets.SequenceEqual([source]));
        Reach(() => ChoiceIs("branch", "pick"));
        Choose("branch", "pick", choice => choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand) && choice.Cards.Count == 0);
        Choose("branch", "pick", choice => choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Equipment) && choice.Cards.SequenceEqual([originalEquipment]));
        Choose("branch", "take");
        Reach(() => Prompt(source) is { PlayerSeat: source, SkillPrompt.SkillId: Zenrun });

        var replacement = Attempt(request.Id).Replacement;
        var compensationPromptId = Prompt(source)!.PromptId;
        var paid = Facts<ZenrunPaidEvent>().Single();
        Need(Prompt() is null && paid.AttemptFrameId == request.Id && paid.SourceSeat == source && paid.Count == count &&
            replacement is { SourceSeat: source } && replacement.GainedIds.Order().SequenceEqual(materials.Order()) &&
            replacement.SequenceAfter > request.Context.SequenceBefore && Facts<AnniversaryCuiLieDrawReturnedEvent>().Length == 0,
            "The original native request owns both paid replacement entities while the foreign seat owns the separate compensation prompt.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Prompt(source)?.PromptId == compensationPromptId && Attempt(request.Id).Replacement?.GainedIds.Order().SequenceEqual(materials.Order()) == true,
            "Cold replay retains the exact paid replacement entities and foreign compensation choice.");
        Submit(new AdvanceOneStepCommand(game.Revision));
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: owner });

        var returned = Facts<AnniversaryCuiLieDrawReturnedEvent>().Single();
        var compensation = Facts<ZenrunCompensationReturnedEvent>().Single();
        var taken = game.CardMovements.Where(move => move.Reason.Value == TakeReason).ToArray();
        var compensationCards = game.CardMovements.Where(move => move.Reason.Value == CompensationReason).ToArray();
        Need(Facts<NativeDrawAttemptDecidedEvent>().Single(fact => fact.AttemptFrameId == request.Id) ==
                new NativeDrawAttemptDecidedEvent(request.Id, number.FrameId, owner, count, true) &&
            returned is { Recipient: owner, Requested: count, Actual: 0 } && returned.FrameId == number.FrameId &&
            returned.Before == returned.After && returned.Before >= compensation.SequenceAfter &&
            taken.Select(move => move.CardId).Order().SequenceEqual(materials.Order()) &&
            taken.Any(move => move.CardId == originalHand && move.From == CardLocation.Hand(source)) &&
            taken.Any(move => move.CardId == originalEquipment && move.From == CardLocation.Equipment(source)) && taken.All(move => move.To == CardLocation.Hand(owner)) &&
            Player(owner).Hand.Select(card => card.Id).Order().SequenceEqual(handBefore.Concat(materials).Order()) &&
            !game.CardMovements.Any(move => move.Reason.Value == DrawReason ||
                move.Sequence > request.Context.SequenceBefore && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(owner)),
            "The original Zijue returns its paid replacement once and draws no physical card from the pile.");
        Need(Facts<ZenrunCompensationRequestedEvent>().Single() is { RecipientSeat: source, RequestedCount: count } compensationRequest &&
            compensationRequest.FrameId == paid.FrameId && compensation is { ActualDrawCount: count, ReplacementGainedCount: 0 } && compensation.FrameId == paid.FrameId &&
            compensationCards.Length == count && compensationCards.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(source)) &&
            Player(source).Hand.Select(card => card.Id).Order().SequenceEqual(compensationCards.Select(move => move.CardId).Order()) &&
            Facts<AnniversaryCuiLieGiftEvent>().Length == 0 && Facts<AnniversaryCuiLieLeaseEvent>().Length == 0 &&
            Facts<AnniversaryCuiLieCompletedEvent>().Single().FrameId == number.FrameId && game.State.TurnNumber == turn,
            "The foreign actor receives one distinct compensation draw and Cui Lie returns to his original Play without issuing a lease.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-zijue-draw-replacement", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, CuiLie);
            var sunHong = CharacterFixtureContent.EmbeddedProgramBundle("ol-sun-hong");
            var zenrun = sunHong.Presentations[Zenrun];
            builder.AddSkill(new(Zenrun, zenrun.Name, zenrun.Description)
                { Program = sunHong.Programs[Zenrun], ProgramPresentation = zenrun });
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "赀爵替换固定牌堆", 0, 1, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "赀爵替换摸牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [CuiLie, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
