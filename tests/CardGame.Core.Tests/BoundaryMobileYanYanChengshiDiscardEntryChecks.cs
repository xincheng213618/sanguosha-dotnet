using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryMobileYanYanChengshiDiscardEntryChecks
{
    private const string Hero = "boundary:yan-yan-mobile", Juzhan = "boundary:juzhan-mobile";
    private const string Early = "fixture:chengshi-preserve-earlier-slashes", Watch = "fixture:chengshi-late-rediscard";
    private const string Enemy = "fixture:chengshi-enemy", Deck = "fixture:chengshi-empty-deck";
    private const string Delivery = "skill-program.current-use-physical-claim", Claim = "skill.boundary-yan-yan.chengshi";
    private const string Rediscard = "skill-program." + Watch + ".SelectAndMoveOwnedCard";

    internal static void ChengshiObtainsOnlyTheOriginalThirdSlashDiscardCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var target = new ShanheCombatantDefinition(Enemy, Hp: 6)
        {
            StartingHand = [], StartingEquipment = [new(CardKind.OffensiveHorse, Suit.Club, 4)]
        };
        var setup = new ShanheBattleSetup(83, 1, "fixture:chengshi-discard-entry", Hero, 4, 0, 0, 0, 0, [Early, Watch],
            [new(CardKind.Slash, Suit.Spade, 7)], [new(CardKind.Crossbow, Suit.Club, 1)], [target], [], new string('d', 64)) { ExtraSkillSlots = 2 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        Reach(game, () => IsPlay(game));
        var slash = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var targetEquipment = game.CreateSnapshot(0).Players[1].Equipment.Single().Id;
        Require(game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 0 }, "The real normal draw has exhausted the small deck.");

        for (var ordinal = 1; ordinal <= 2; ordinal++)
        {
            PlaySlash(game, slash);
            Reach(game, () => CanInvoke(game, Juzhan));
            AnswerAction(game, "skip");
            Reach(game, () => CanInvoke(game, Early));
            AnswerAction(game, "activate");
            Reach(game, () => CanInvoke(game, Watch));
            AnswerAction(game, "skip");
            Reach(game, () => IsPlay(game));
            Require(game.CreateSnapshot(0).DiscardPileCount == 0 &&
                game.CardMovements.Count(move => move.CardId == slash && move.Reason.Value == Delivery &&
                    move.From == CardLocation.Processing && move.To == CardLocation.Hand(0)) == ordinal,
                "The first two real Slash uses return their material through the native finalized child without leaving an earlier discard.");
        }

        PlaySlash(game, slash);
        Reach(game, () => CanInvoke(game, Juzhan));
        var third = game.ResolutionStack.OfType<CardUseFrame>().Single();
        AnswerAction(game, "activate");
        Reach(game, () => HasChoice(game, choice => ChoiceAction(choice) == "select-target"));
        Answer(game, choice => ChoiceAction(choice) == "select-target" && choice.Targets.SequenceEqual([1]));
        Reach(game, () => HasChoice(game, choice => ChoiceAction(choice) == "select-and-move-owned-card"));
        Answer(game, choice => ChoiceAction(choice) == "select-and-move-owned-card" && choice.Cards.SequenceEqual([targetEquipment]));
        Reach(game, () => CanInvoke(game, Early));
        AnswerAction(game, "skip");
        Reach(game, () => CanInvoke(game, Watch));

        var grant = Facts<BoundaryYanYanChengshiGrantedEvent>(game).Single();
        var original = game.CardMovements.Single(move => move.CardId == slash && move.Reason == CardMoveReasons.UseFinished);
        var match = game.CreateSnapshot(0);
        Require(grant.UseId == third.Id && grant.ActionId == third.Action!.ActionId && grant.OwnerSeat == 0 &&
            original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.CardId == slash) == 3 &&
            Facts<BoundaryYanYanClaimPaidEvent>(game).Length == 0 && match is { DrawPileCount: 0, DiscardPileCount: 1 },
            "The third true self-use grants Chengshi once and produces its original sole discard before the earlier completion spectator.");

        var control = Cold(game, content);
        AnswerAction(control, "skip");
        Reach(control, () => IsPlay(control));
        AssertGrantAndThirdUse(control, third.Id, grant);
        Require(Facts<BoundaryYanYanClaimPaidEvent>(control).Single() is { Count: 1 } &&
            control.CardMovements.Single(move => move.Reason.Value == Claim) is { CardId: var obtained } gain &&
            obtained == slash && gain.From == original.To && gain.To == CardLocation.Hand(0) && gain.Sequence > original.Sequence,
            "When the earlier completion spectator declines, the original native third Slash is obtained once.");

        AnswerAction(game, "activate");
        Reach(game, () => P(game) is { SkillPrompt.SkillId: Watch } && HasChoice(game, choice => choice.Cards.SequenceEqual([slash])));
        var lift = game.CardMovements.Where(move => move.CardId == slash && move.Sequence > original.Sequence).ToArray();
        Require(lift is [var shuffled, var drawn] && shuffled.From == original.To && shuffled.To == CardLocation.DrawPile &&
            shuffled.Reason == CardMoveReasons.Reshuffle && drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0),
            "The earlier real completion child reshuffles and draws the third Slash from its original finish before the re-discard decision.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([slash]));
        Reach(game, () => IsPlay(game));
        AssertGrantAndThirdUse(game, third.Id, grant);
        var replacement = game.CardMovements.Single(move => move.CardId == slash && move.Reason.Value == Rediscard);
        Require(replacement.Sequence > lift[^1].Sequence && replacement.BatchId != original.BatchId &&
            replacement.From == CardLocation.Hand(0) && replacement.To == CardLocation.DiscardPile,
            "The accepted hand discard creates a different physical discard entry for the same Slash.");
        Require(Facts<BoundaryYanYanClaimPaidEvent>(game).Single() is { Count: 0 } &&
            game.CardMovements.Last(move => move.CardId == slash) == replacement,
            $"Chengshi must leave entry {replacement.Sequence} in the pile after the original third Slash finish {original.Sequence} was moved away.");
        _ = Cold(game, content);
    }

    private static void AssertGrantAndThirdUse(GameEngine game, long useId, BoundaryYanYanChengshiGrantedEvent grant) => Require(
        Facts<BoundaryYanYanChengshiGrantedEvent>(game).SequenceEqual([grant]) &&
        Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == useId) == 1 &&
        game.CreateSnapshot(0).Players[0].SkillRuntimeStates!.Single(skill => skill.SkillId == Juzhan).PublicRuleStates!
            .Single(state => state.StateId == "juzhan-permanent-slash").Value == 1,
        "Both branches preserve one permanent Slash allowance and exactly one completion of the original third use.");

    private static PendingDecision? P(GameEngine game) => game.CreateSnapshot(0).PendingDecision;
    private static bool IsPlay(GameEngine game) => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
    private static string? ChoiceAction(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");
    private static bool CanInvoke(GameEngine game, string skill) => P(game) is { } prompt && prompt.SkillPrompt?.SkillId == skill &&
        prompt.Choices.Any(choice => ChoiceAction(choice) == "activate");
    private static bool HasChoice(GameEngine game, Func<PromptChoice, bool> match) => P(game)?.Choices.Any(match) == true;
    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void PlaySlash(GameEngine game, int slash)
    {
        var action = game.GetHumanLegalActions().Single(item => item.Kind == LegalActionKind.Slash && item.CardId == slash && item.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, slash, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
    }

    private static void AnswerAction(GameEngine game, string action) => Answer(game, choice => ChoiceAction(choice) == action);

    private static void Answer(GameEngine game, Func<PromptChoice, bool> match)
    {
        var prompt = P(game)!;
        var choice = prompt.Choices.Single(match);
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
    }

    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 80; step++)
        {
            if (reached())
                return;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Chengshi did not reach the expected decision or return: " + P(game)?.Prompt);
    }

    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The real Chengshi command was rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold commands reproduce the original third Slash, permanent grant and reshuffled material selection.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-mobile-chengshi-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Early}}}","revision":1,"triggers":[{"id":"keep-earlier-slash","window":"cardUseTargetsFinalized",
                    "ownerRelation":"actor","cardKinds":["slash"],"priority":-1000,"optional":true,
                    "effects":[{"op":"claimCurrentUsePhysicalCards","target":"owner","targetRef":{"kind":"owner"}}]}]},
                  {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"redraw-after-slash","window":"cardUseCompleted",
                    "ownerRelation":"actor","cardKinds":["slash"],"priority":1000,"optional":true,"effects":[
                      {"op":"draw","target":"owner","amount":1},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Early] = new { name = "保留此前杀", description = "取得当前真实使用的杀的实体。" },
                    [Watch] = new { name = "结算后摸回并重弃", description = "摸一张牌，然后弃置一张真实手牌。" }
                }
            }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Enemy, "固定坐骑的对手", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定两张普通摸牌", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Heart, 5), new("standard:dodge", Suit.Diamond, 6)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "乘势原第三次出杀", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
