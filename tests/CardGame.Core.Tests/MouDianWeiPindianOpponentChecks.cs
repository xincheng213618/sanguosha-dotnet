using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MouDianWeiPindianOpponentChecks
{
    private const string Hero = "mou:dian-wei", Kuangzhan = "mou:kuangzhan", Deck = "fixture:kuangzhan-opponents";
    private const string Enemy = "fixture:kuangzhan-enemy", Ally = "fixture:kuangzhan-ally", Empty = "fixture:kuangzhan-empty";
    private const string Zenrun = "ol:zenrun", Watch = "fixture:kuangzhan-draw-child";
    private const string DrawReason = "skill-program.mou:kuangzhan.draw", TakeReason = "skill-program.sun-hong.zenrun.obtain";
    private const string CompensationReason = "skill-program.sun-hong.zenrun.compensation";

    internal static void ActualKuangzhanDrawAndNativePindianUseSameOpponentsCold()
    {
        VerifyDrawAndContest(replaceDraw: false);
        VerifyDrawAndContest(replaceDraw: true);
    }

    private static void VerifyDrawAndContest(bool replaceDraw)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(7, 1, "kuangzhan-opponents", Hero, 1, 0, 0, 0, 0, [Zenrun, Watch],
            [new(CardKind.Dodge, Suit.Spade, 13)], [],
            [
                new(Ally, Hp: 10) { StartingHand = [new(CardKind.Slash, Suit.Club, 3)] },
                new(Empty, Hp: 10) { StartingHand = [] }
            ],
            [new(Enemy, Hp: 10)
            {
                StartingHand = [new(CardKind.Slash, Suit.Club, 1), new(CardKind.Slash, Suit.Club, 2)],
                StartingEquipment = [new(CardKind.Crossbow, Suit.Club, 3)]
            }], new string('d', 64)) { ExtraSkillSlots = 2 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        T[] Facts<T>() where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        void Need(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException($"{message} Replacement {replaceDraw}. Turn {game.State.TurnNumber}, phase {game.State.Phase}, " +
                    $"prompt {Prompt()?.Kind}/{Prompt()?.SkillPrompt?.SkillId}.");
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The actual Kuangzhan command was rejected.");
        }
        bool ZenrunOffer() => Prompt()?.SkillPrompt?.SkillId == Zenrun &&
            Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
        bool ZenrunBranch(string value) => Prompt()?.SkillPrompt?.SkillId == Zenrun &&
            Prompt()!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == value);
        void Answer(string key, string value, int? target = null, string? zone = null)
        {
            var prompt = Prompt()!;
            var choice = prompt.Choices.First(item => item.Parameters.GetValueOrDefault(key) == value &&
                (target is null || item.Targets.SequenceEqual([target.Value])) &&
                (zone is null || item.Parameters.GetValueOrDefault("source-zone") == zone));
            Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
        }
        ProgramSkillFrame Root(long id) => game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == id);
        GameEngine Restore() => GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        void Reach(Func<bool> ready, bool beforePlay = false)
        {
            for (var step = 0; step < 80 && !ready(); step++)
            {
                if (beforePlay && game.State.Phase == TurnPhase.Draw && ZenrunOffer())
                {
                    Answer("program-action", "skip");
                    continue;
                }
                if (Prompt()?.SkillPrompt?.SkillId == Watch && game.ResolutionStack.OfType<ProgramSkillFrame>()
                    .Any(frame => frame.MouDianWei?.Stage == MouDianWeiStage.DrawChildren))
                {
                    Answer("option-id", "continue");
                    continue;
                }
                Need(Prompt() is null && game.State.Winner == Winner.None, "The fixed scene reached an unexpected boundary.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            Need(ready(), "The fixed scene did not reach its actual choice.");
        }

        Submit(new StartGameCommand());
        Reach(() => Prompt() is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, beforePlay: true);
        var handBefore = game.CreateSnapshot(0).Players[0].HandCount;
        var victim = game.CreateSnapshot(1).Players[1];
        var originalHand = victim.Hand.Select(card => card.Id).ToArray();
        var originalEquipment = victim.Equipment.Single().Id;
        Submit(new UseProgramSkillCommand(0, Kuangzhan, "draw-and-contest", [], [], game.Revision, Prompt()!.PromptId));
        Reach(ZenrunOffer);
        var attempt = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single();
        var rootId = attempt.Context.ParentFrameId;
        Need(attempt.Context is { RecipientSeat: 0, RequestedCount: 3 } && attempt.Context.Reason.Value == DrawReason &&
            Root(rootId).NativeDraw?.ActiveAttemptId == attempt.Id && Facts<MouDianWeiDrawReturnedEvent>().Length == 0,
            "The same original Kuangzhan activation owns the native Draw3 attempt before either branch pays.");

        if (replaceDraw)
        {
            Answer("program-action", "activate");
            Reach(() => ZenrunBranch("target"));
            Answer("branch", "target", target: 1);
            Reach(() => ZenrunBranch("pick"));
            Need(Prompt()!.Choices.Where(choice => choice.Parameters.GetValueOrDefault("source-zone") == nameof(CardZoneKind.Hand))
                .All(choice => choice.Cards.Count == 0), "Zenrun must offer the foreign hand as private slots.");
            Answer("branch", "pick", zone: nameof(CardZoneKind.Hand));
            Answer("branch", "pick", zone: nameof(CardZoneKind.Hand));
            Answer("branch", "pick", zone: nameof(CardZoneKind.Equipment));
            Answer("branch", "take");
            Reach(() => game.CreateSnapshot(1).PendingDecision?.SkillPrompt?.SkillId == Zenrun &&
                game.CreateSnapshot(1).PendingDecision!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "draw"));
            var paid = game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == attempt.Id).Replacement;
            var compensationPrompt = game.CreateSnapshot(1).PendingDecision!;
            Need(paid is not null && paid.GainedIds.Order().SequenceEqual(originalHand.Append(originalEquipment).Order()) &&
                Prompt() is null && Root(rootId).NativeDraw is { AttemptReturned: false } && Facts<MouDianWeiDrawReturnedEvent>().Length == 0,
                "The actual foreign Hand and Equipment gains are paid, and Kuangzhan waits for the victim's private compensation choice.");
            game = Restore();
            Need(game.CreateSnapshot(1).PendingDecision?.PromptId == compensationPrompt.PromptId &&
                game.ResolutionStack.OfType<NativeDrawAttemptFrame>().Single(frame => frame.Id == attempt.Id).Replacement?.GainedIds.SequenceEqual(paid!.GainedIds) == true,
                "Cold replay preserves the paid replacement and the original victim prompt.");
            Submit(new AdvanceOneStepCommand(game.Revision));
        }
        else
        {
            Answer("program-action", "skip");
            Reach(() => Prompt()?.SkillPrompt?.SkillId == Watch);
            var gainPrompt = Prompt()!;
            var original = Root(rootId).NativeOperationDrawReturns.Single().Outcome;
            Need(original.ActualDrawIds.Count == 3 && original.Replacement is null && original.AttemptFrameId == attempt.Id &&
                Root(rootId).MouDianWei is { Stage: MouDianWeiStage.DrawChildren, X: 3 },
                "The ordinary Draw settles its three physical gains on the original producer before the gain child returns.");
            game = Restore();
            Need(Prompt()?.PromptId == gainPrompt.PromptId &&
                Root(rootId).NativeOperationDrawReturns.Single().Outcome.ActualDrawIds.SequenceEqual(original.ActualDrawIds),
                "Cold replay restores the actual gain child with the same three original Draw entities.");
            Answer("option-id", "continue");
        }

        Reach(() => Prompt() is { Kind: DecisionKind.ProgramTrigger, PlayerSeat: 0 } && game.ResolutionStack.OfType<ProgramSkillFrame>()
            .Any(frame => frame.MouDianWei?.Stage == MouDianWeiStage.Opponent));
        var draw = Facts<MouDianWeiDrawReturnedEvent>().Single();
        var root = Root(rootId);
        var returned = root.NativeOperationDrawReturns.Single();
        var outcome = returned.Outcome;
        var draws = game.CardMovements.Where(move => move.Reason.Value == DrawReason).ToArray();
        var compensated = game.CardMovements.Where(move => move.Reason.Value == CompensationReason).ToArray();
        Need(draw is { Requested: 3, X: 3 } && draw.FrameId == root.Id && draw.AttemptId == attempt.Id &&
            returned is { InstructionIndex: 1, Operation: SkillProgramEffectOp.ResolveMouDianWeiKuangzhan } && returned.Reason == attempt.Context.Reason &&
            outcome is { RecipientSeat: 0, RequestedCount: 3 } && outcome.AttemptFrameId == attempt.Id &&
            Facts<NativeDrawAttemptDecidedEvent>().Count(fact => fact.AttemptFrameId == attempt.Id && fact.ParentFrameId == root.Id && fact.Replaced == replaceDraw) == 1,
            "The same owned Draw returns once and freezes three comparisons for the original Kuangzhan activation.");
        if (replaceDraw)
        {
            var taken = game.CardMovements.Where(move => move.Reason.Value == TakeReason).ToArray();
            Need(draw is { ActualDrawn: 0, ReplacementGained: 3 } && draws.Length == 0 && outcome.ActualDrawIds.Count == 0 &&
                outcome.Replacement is { SourceSeat: 1 } && outcome.ReplacementGainedIds.SequenceEqual(taken.Select(move => move.CardId)) &&
                taken.Count(move => move.From == CardLocation.Hand(1)) == 2 && taken.Any(move => move.From == CardLocation.Equipment(1)) &&
                taken.All(move => move.To == CardLocation.Hand(0)) && outcome.SequenceBefore == outcome.SequenceAfter &&
                compensated.Length == 3 && compensated.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) && move.Sequence <= outcome.SequenceBefore) &&
                !compensated.Select(move => move.CardId).Intersect(outcome.ReplacementGainedIds).Any() &&
                Facts<ZenrunCompensationReturnedEvent>().Single() is { ActualDrawCount: 3, ReplacementGainedCount: 0 },
                "Only the original Hand and Equipment entities gained by Kuangzhan count toward X. The foreign victim's separate real Draw3 has already settled.");
        }
        else
            Need(draw is { ActualDrawn: 3, ReplacementGained: 0 } && outcome.Replacement is null && compensated.Length == 0 &&
                outcome.ActualDrawIds.SequenceEqual(draws.Select(move => move.CardId)) &&
                draws.All(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Sequence > outcome.SequenceBefore && move.Sequence <= outcome.SequenceAfter),
                "Ordinary Kuangzhan counts only its exact three physical DrawPile-to-Hand receipts.");

        var choices = Prompt()!;
        var players = game.CreateSnapshot(0).Players;
        Need(players[0].HandCount == handBefore + 3 &&
            players[1].HandCount > 0 && players[2].HandCount > 0 && players[3].HandCount == 0 &&
            root.MouDianWei is { Stage: MouDianWeiStage.Opponent, Index: 0 } &&
            choices.Choices.SelectMany(choice => choice.Targets).SequenceEqual([1, 2]) && Facts<MouDianWeiContestIssuedEvent>().Length == 0,
            "After three real gains the original skill offers exactly the two opponents with input and excludes the empty seat.");

        var opponent = choices.Choices.Single(choice => choice.Targets.SequenceEqual([1]));
        Submit(new AnswerPromptCommand(0, choices.PromptId, opponent.Id, game.Revision));
        var issued = Facts<MouDianWeiContestIssuedEvent>().Single();
        var pindian = game.ResolutionStack.OfType<PindianFrame>().Single(frame => frame.Id == issued.ContestId);
        Need(issued is { Opponent: 1, Index: 0 } && issued.FrameId == root.Id && pindian.ParentFrameId == root.Id &&
            pindian is { SourceSeat: 0, OpponentSeat: 1, PindianStep: PindianStep.ChooseSourceCard } &&
            Prompt() is { PlayerSeat: 0, IsPrivate: true },
            "Submitting the displayed opponent creates a private native Pindian under the same Kuangzhan frame.");

        var restored = Restore();
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
            game.CardMovements.SequenceEqual(restored.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack.OfType<PindianFrame>().Single()) == JsonSerializer.Serialize(pindian),
            "Cold replay keeps all seat views, the actual draw movements and the same pending contest.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-kuangzhan-opponents", new(1, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var sunHong = CharacterFixtureContent.EmbeddedProgramBundle("ol-sun-hong");
            var zenrun = sunHong.Presentations[Zenrun];
            builder.AddSkill(new(Zenrun, zenrun.Name, zenrun.Description)
                { Program = sunHong.Programs[Zenrun], ProgramPresentation = zenrun });
            var watcher = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,
                  "triggers":[{"id":"actual-draw-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],
                    "movementReasons":["{{DrawReason}}"],"movementOccurrence":"perBatch","optional":false,"isSkillInvocation":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                  "name":"狂战原摸牌收益","description":"观察狂战实际摸到的牌。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, "狂战原摸牌收益", "观察狂战实际摸到的牌。")
                { Program = watcher.Programs[Watch], ProgramPresentation = watcher.Presentations[Watch] });
            foreach (var id in new[] { Enemy, Ally, Empty })
                builder.AddGeneral(new(id, "固定同桌", "supporter", "standard:none", "wei", 10));
            builder.AddDeck(new(Deck, "狂战固定牌堆", 0, 1, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "狂战原生拼点", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy, Ally, Empty], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
