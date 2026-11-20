using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryHanSongShuaiyanDrawChecks
{
    private const string Hero = "anniversary:han-song", Shuaiyan = "anniversary:shuaiyan", Zenrun = "ol:zenrun";
    private const string Peer = "fixture:shuaiyan-replacement-peer", Deck = "fixture:shuaiyan-replacement-deck";
    private const string DrawReason = "anniversary.shuaiyan.draw", TakeReason = "skill-program.sun-hong.zenrun.obtain";
    private const string CompensationReason = "skill-program.sun-hong.zenrun.compensation";

    internal static void ForeignDiscardDrawReturnsOriginalZenrunReplacementCold()
    {
        const int owner = 0, subject = 1;
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var peer = new ShanheCombatantDefinition(Peer) { StartingHand = [] };
        var setup = new ShanheBattleSetup(7, 1, "shuaiyan-discard-native-replacement", Hero, 3, 0, 0, 0, 0, [Zenrun],
            [], [], [peer, peer],
            [peer with
            {
                StartingHand = [new(CardKind.Dodge, Suit.Club, 11)],
                StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 1)]
            }], new string('d', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = owner, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: owner }, skipOwnOffers: true);
        Require(Player(owner).HandCount == 2, "Han Song enters the first actual Play with two genuine draws.");
        var paidCard = Player(subject).Equipment.Single().Id;
        Submit(new EndPlayPhaseCommand(owner, game.Revision, Prompt()!.PromptId));
        Reach(() => game.State is { CurrentSeat: subject, Phase: TurnPhase.Discard } &&
            Prompt()?.SkillPrompt?.SkillId == Shuaiyan && HasChoice("program-action", "activate"), skipOwnOffers: true);

        var turn = game.State.TurnNumber;
        var originalHand = Player(owner).Hand.Select(card => card.Id).ToArray();
        var discardPhase = Facts<ActualDiscardRecoveryPhaseStartedEvent>().Single(fact => fact.Phase is
            { Kind: ActualDiscardRecoveryPhaseKind.Discard, ActorSeat: subject } && fact.Phase.TurnNumber == turn).Phase;
        Require(Player(subject).HandCount == originalHand.Length && originalHand.Length == 2 &&
            game.CardMovements.Count(move => move.TurnNumber == turn && move.From == CardLocation.Hand(subject) &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.HandLimitDiscard) == 1,
            "The foreign seat draws to three, then actually discards one to match Han Song at the Discard-end offer.");
        Choose("program-action", "activate");
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Shuaiyan && HasChoice("verb", "draw"));
        var started = Facts<AnniversaryHanSongStartedEvent>().Single();
        var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == started.FrameId);
        Require(root.AnniversaryHanSong is { Stage: AnniversaryHanSongStage.BenefitChoice, SubjectSeat: subject, DrawCount: 1 } &&
            started.ParentId == root.WindowContext?.ParentFrameId && root.TriggerId == "other-discard-ended" &&
            root.WindowContext is { Window: SkillProgramTriggerWindow.DiscardPhaseEnded, SourceSeat: subject } && HasChoice("verb", "discard"),
            "The original foreign Discard phase offers Shuaiyan a genuine choice between its one-card Draw and discard.");
        var beforeRequest = game.CardMovements.Last().Sequence;
        Choose("verb", "draw");
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Zenrun && HasChoice("program-action", "activate"));
        var attempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        Require(attempt.Context is { RecipientSeat: owner, RequestedCount: 1 } && attempt.Context.Reason.Value == DrawReason &&
            attempt.Context.ActualPhase == discardPhase && attempt.Context.ParentFrameId == root.Id &&
            attempt.Context.SequenceBefore == beforeRequest && attempt.Return.Kind == NativeDrawAttemptReturnKind.ProgramOperation &&
            Facts<AnniversaryHanSongBenefitIssuedEvent>().Single() == new AnniversaryHanSongBenefitIssuedEvent(root.Id, true, 1),
            "The original Shuaiyan frame issues exactly one Draw request to Han Song on the foreign Discard phase key.");

        Choose("program-action", "activate");
        Reach(() => HasChoice("branch", "target"));
        Choose("branch", "target", item => item.Targets.SequenceEqual([subject]));
        Reach(() => HasChoice("branch", "pick"));
        Choose("branch", "pick", item => item.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Equipment) &&
            item.Cards.SequenceEqual([paidCard]));
        Choose("branch", "take");
        Reach(() => Prompt(subject) is { PlayerSeat: subject, SkillPrompt.SkillId: Zenrun });

        var paid = Facts<ZenrunPaidEvent>().Single();
        var compensationPrompt = Prompt(subject)!;
        var replacement = ActiveAttempt().Replacement;
        Require(Prompt() is null && compensationPrompt.IsPrivate &&
            compensationPrompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "draw") &&
            paid.AttemptFrameId == attempt.Id && paid.SourceSeat == subject && paid.Count == 1 &&
            replacement is { SourceSeat: subject } && replacement.GainedIds.SequenceEqual([paidCard]) &&
            replacement.SequenceAfter > attempt.Context.SequenceBefore && Facts<AnniversaryHanSongDrawReturnedEvent>().Length == 0,
            "The original attempt holds the physically obtained equipment while the other seat owns the compensation decision.");
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Require(Prompt(subject)?.PromptId == compensationPrompt.PromptId &&
            ActiveAttempt().Replacement?.GainedIds.SequenceEqual([paidCard]) == true,
            "Cold replay recreates the exact paid replacement and the original recipient's compensation prompt.");
        Submit(new AdvanceOneStepCommand(game.Revision));
        Reach(() => Facts<AnniversaryHanSongCompletedEvent>().Any(fact => fact.FrameId == root.Id));

        var returned = Facts<AnniversaryHanSongDrawReturnedEvent>().Single();
        var compensation = Facts<ZenrunCompensationReturnedEvent>().Single();
        var taken = game.CardMovements.Single(move => move.Reason.Value == TakeReason);
        var compensated = game.CardMovements.Single(move => move.Reason.Value == CompensationReason);
        Require(Facts<NativeDrawAttemptDecidedEvent>().Single(fact => fact.AttemptFrameId == attempt.Id) ==
                new NativeDrawAttemptDecidedEvent(attempt.Id, root.Id, owner, 1, true) &&
            returned.FrameId == root.Id && returned.Count == 0 && returned.Before == returned.After &&
            returned.Before > beforeRequest && returned.Before >= compensation.SequenceAfter &&
            taken.CardId == paidCard && taken.From == CardLocation.Equipment(subject) && taken.To == CardLocation.Hand(owner) &&
            !game.CardMovements.Any(move => move.Reason.Value == DrawReason) &&
            Player(owner).Hand.Select(card => card.Id).Order().SequenceEqual(originalHand.Append(paidCard).Order()),
            "The exact original Shuaiyan draw returns after the earlier real replacement instead of borrowing the issue sequence.");
        Require(compensated.From == CardLocation.DrawPile && compensated.To == CardLocation.Hand(subject) &&
            compensated.CardId != paidCard && compensation.FrameId == paid.FrameId &&
            compensation is { ActualDrawCount: 1, ReplacementGainedCount: 0 } &&
            Facts<AnniversaryHanSongCompletedEvent>().Single().FrameId == root.Id && Facts<AnniversaryHanSongDiscardPaidEvent>().Length == 0,
            "The foreign participant receives its own real compensation and the original Shuaiyan frame completes the chosen Draw once.");

        PendingDecision? Prompt(int seat = owner) => game.CreateSnapshot(seat).PendingDecision;
        PlayerSnapshot Player(int seat) => game.CreateSnapshot(seat).Players[seat];
        T[] Facts<T>() => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();
        NativeDrawAttemptFrame ActiveAttempt() => game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == attempt.Id);
        bool HasChoice(string key, string value) => Prompt()?.Choices.Any(choice => choice.Parameters.GetValueOrDefault(key) == value) == true;
        void Choose(string key, string value, Func<PromptChoice, bool>? matches = null)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault(key) == value && (matches is null || matches(item)));
            Submit(new AnswerPromptCommand(owner, prompt.PromptId, choice.Id, game.Revision));
        }
        void Reach(Func<bool> ready, bool skipOwnOffers = false)
        {
            for (var step = 0; step < 120 && !ready(); step++)
            {
                if (skipOwnOffers && game.State.CurrentSeat == owner && Prompt()?.SkillPrompt?.SkillId is Shuaiyan or Zenrun && HasChoice("program-action", "skip"))
                    Choose("program-action", "skip");
                else
                {
                    Require(Prompt() is null && game.State.Winner == Winner.None, "The fixture reached a different public command boundary.");
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
            }
            Require(ready(), "The fixture did not reach the native Shuaiyan or Zenrun choice.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Require(result.Accepted, result.Error?.Message ?? "The native Shuaiyan fixture command was rejected.");
        }
        void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Turn {game.State.TurnNumber}/{game.State.Phase}, " +
                    $"actor {game.State.CurrentSeat}, owner prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-shuaiyan-replacement", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var sunHong = CharacterFixtureContent.EmbeddedProgramBundle("ol-sun-hong");
            var zenrun = sunHong.Presentations[Zenrun];
            builder.AddSkill(new(Zenrun, zenrun.Name, zenrun.Description)
                { Program = sunHong.Programs[Zenrun], ProgramPresentation = zenrun });
            builder.AddGeneral(new(Peer, "固定同桌", "supporter", "standard:none", "wei", 2));
            builder.AddDeck(new(Deck, "率言替换固定牌堆", 0, 2, [new("standard:dodge", 24)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "率言原弃牌末摸牌", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
