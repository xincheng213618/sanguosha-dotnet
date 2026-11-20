using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class ClassicCaoPiXingshangDiscardEntryChecks
{
    private const string Hero = "classic:cao-pi", Xingshang = "classic:xingshang";
    private const string Victim = "fixture:classic-xingshang-victim", Survivor = "fixture:classic-xingshang-survivor";
    private const string Earlier = "fixture:classic-xingshang-earlier-claim", EarlierBinding = "claim-hand-first";
    private const string Deck = "fixture:classic-xingshang-deck";
    private const string EarlyGain = "skill-program." + Earlier + "." + EarlierBinding + ".claim-death-cleanup";
    private const string Rediscard = "skill-program." + Earlier + ".SelectAndMoveOwnedCard";
    private const string Gain = "skill-program." + Xingshang + ".claim-died-cards.claim-death-cleanup";

    internal static void XingshangClaimsUntouchedDeathEquipmentButCannotClaimTheRedroppedHandCardCold()
    {
        var (game, content) = Start();
        var use = KillVictim(game);
        var death = game.ResolutionStack.OfType<DeathFrame>().Single();
        var window = game.ResolutionStack.OfType<ProgramKillTriggerWindowFrame>().Single();
        var hand = game.CardMovements.Single(move => move.From == CardLocation.Hand(1) && move.Reason == CardMoveReasons.DeathDiscard);
        var equipment = game.CardMovements.Single(move => move.From == CardLocation.Equipment(1) && move.Reason == CardMoveReasons.DeathEquipmentDiscard);
        Require(death is { VictimSeat: 1, KillerSeat: 0 } && death.CleanedUpCardIds.SequenceEqual([hand.CardId, equipment.CardId]) &&
            window.DeathFrameId == death.Id && !window.BeforeDeathCleanup &&
            window.Candidates.Skip(window.CandidateIndex + 1).Any(candidate => candidate.OwnerSeat == 0 && candidate.SkillId == Xingshang) &&
            hand.To == CardLocation.DiscardPile && equipment.To == CardLocation.DiscardPile && hand.Sequence < equipment.Sequence &&
            Facts<DamageAppliedEvent>(game).Single(fact => fact.SourceCardUseResolutionId == use.ResolutionId) is
                { SourceSeat: 0, TargetSeat: 1, Amount: 1 },
            "One lethal native Slash owns the death window with one original Hand discard and one later Equipment discard.");

        var control = Cold(game, content);
        AnswerHuman(control, choice => Action(choice) == "skip");
        AcceptXingshang(control, death.Id);
        Require(XingshangGains(control).Select(move => move.CardId).SequenceEqual([hand.CardId, equipment.CardId]),
            "With both original entries untouched, classic Xingshang claims both once in their original cleanup order.");
        AssertDeathReturned(control, use.ResolutionId);

        AnswerHuman(game, choice => Action(choice) == "activate");
        var discard = Next(game);
        var claimed = game.CardMovements.Single(move => move.Reason.Value == EarlyGain);
        Require(discard.SkillPrompt?.SkillId == Earlier && discard.Choices.Any(choice => choice.Cards.SequenceEqual([hand.CardId])) &&
            claimed.CardId == hand.CardId && claimed.From == CardLocation.DiscardPile && claimed.To == CardLocation.Hand(0) &&
            claimed.Sequence > hand.Sequence && game.CardMovements.Last(move => move.CardId == equipment.CardId) == equipment,
            "The earlier Hand-only claim actually acquires the dead player's hand entity and leaves the original equipment entry untouched.");
        game = Cold(game, content);
        AnswerHuman(game, choice => choice.Cards.SequenceEqual([hand.CardId]));
        var fresh = game.CardMovements.Single(move => move.CardId == hand.CardId && move.Reason.Value == Rediscard);
        Require(fresh.From == CardLocation.Hand(0) && fresh.To == CardLocation.DiscardPile && fresh.Sequence > claimed.Sequence &&
            fresh.BatchId != hand.BatchId, "The later hand payment gives the once-claimed card a different real discard entry.");

        AcceptXingshang(game, death.Id);
        var retained = XingshangGains(game);
        Require(retained.Count(move => move.CardId == equipment.CardId) == 1 &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == equipment.CardId).Location == CardLocation.Hand(0),
            "Classic Xingshang still claims the original equipment from that same death.");
        AssertDeathReturned(game, use.ResolutionId);
        Require(retained.Select(move => move.CardId).SequenceEqual([equipment.CardId]) &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == hand.CardId).Location == CardLocation.DiscardPile,
            $"Classic Xingshang cannot claim discard entry {fresh.Sequence} through death cleanup entry {hand.Sequence}.");
        NoOfferWhenAllOriginalEntriesHaveLeft();
    }

    private static void NoOfferWhenAllOriginalEntriesHaveLeft()
    {
        var (game, content) = Start(withEquipment: false);
        var use = KillVictim(game);
        var death = game.ResolutionStack.OfType<DeathFrame>().Single();
        var hand = game.CardMovements.Single(move => move.From == CardLocation.Hand(1) && move.Reason == CardMoveReasons.DeathDiscard);
        Require(death.CleanedUpCardIds.SequenceEqual([hand.CardId]) &&
            game.ResolutionStack.OfType<ProgramKillTriggerWindowFrame>().Single().Candidates.Any(candidate => candidate.SkillId == Xingshang),
            "The original death window initially contains only one physical entry and a live Xingshang candidate.");
        AnswerHuman(game, choice => Action(choice) == "activate");
        Require(Next(game).SkillPrompt?.SkillId == Earlier && game.CardMovements.Any(move =>
            move.CardId == hand.CardId && move.Reason.Value == EarlyGain && move.To == CardLocation.Hand(0)),
            "The earlier live candidate really acquires that sole cleanup card.");
        game = Cold(game, content);
        AnswerHuman(game, choice => choice.Cards.SequenceEqual([hand.CardId]));
        Require(IsPlay(Next(game)) && XingshangGains(game).Length == 0 && game.CardMovements.Last(move => move.CardId == hand.CardId)
            is { To: var location, Reason.Value: Rediscard, Sequence: var sequence } && location == CardLocation.DiscardPile && sequence > hand.Sequence,
            "A redropped card cannot trigger an empty Xingshang choice when all original cleanup entries have left.");
        AssertDeathReturned(game, use.ResolutionId);
    }

    private static CardUseDeclaredEvent KillVictim(GameEngine game)
    {
        var play = Next(game);
        var slash = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Rank == 7);
        Require(IsPlay(play) && slash.Kind == CardKind.Slash, "Cao Pi starts the first real Play phase with the fixed Slash.");
        var action = game.GetHumanLegalActions().Single(option => option.CardId == slash.Id && option.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, slash.Id, action.TargetSeats, game.Revision, play.PromptId, action.PlayedCardKind));
        Require(Next(game).SkillPrompt?.SkillId == Earlier, "The earlier real death claim opens before classic Xingshang.");
        return Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == slash.Id);
    }

    private static CardMovementRecord[] XingshangGains(GameEngine game) => game.CardMovements.Where(move =>
        move.Reason.Value == Gain && move.From == CardLocation.DiscardPile && move.To == CardLocation.Hand(0)).ToArray();

    private static void AcceptXingshang(GameEngine game, long deathId)
    {
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Xingshang } &&
            game.ResolutionStack.OfType<ProgramKillTriggerWindowFrame>().Single().DeathFrameId == deathId,
            "The real classic Xingshang choice belongs to the same original death.");
        AnswerHuman(game, choice => Action(choice) == "activate");
        Require(IsPlay(Next(game)), "The classic claim returns to Cao Pi's original Play phase.");
    }

    private static void AssertDeathReturned(GameEngine game, long useId)
    {
        Require(Facts<PlayerDiedEvent>(game).Single() is { VictimSeat: 1, KillerSeat: 0 } &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == useId) == 1 &&
            game.ResolutionStack.All(frame => frame is not DeathFrame and not ProgramKillTriggerWindowFrame) &&
            game.CreateSnapshot(0) is { Winner: Winner.None, CurrentSeat: 0, Phase: TurnPhase.Play },
            "The same real death completes once and returns to the original Slash without ending the encounter.");
    }

    private static bool IsPlay(PendingDecision prompt) => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt)
            {
                var pass = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") == "let-die");
                if (pass is null)
                    return prompt;
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, pass.Id, game.Revision));
            }
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Classic Xingshang did not reach the next public decision.");
    }

    private static void AnswerHuman(GameEngine game, Func<PromptChoice, bool> predicate)
    {
        var prompt = game.CreateSnapshot(0).PendingDecision!;
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, game.Revision));
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, game.PlayerCount).All(seat =>
                SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold public-command replay reproduces the original death frame, its exact card entries, and each seat's view.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start(bool withEquipment = true)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var victim = new ShanheCombatantDefinition(Victim, Hp: 1)
        {
            StartingHand = [new(CardKind.Slash, Suit.Heart, 2)],
            StartingEquipment = withEquipment ? [new(CardKind.Crossbow, Suit.Club, 1)] : []
        };
        var setup = new ShanheBattleSetup(89, 1, "classic-xingshang-death-entry", Hero, 3, 0, 0, 0, 0,
            [Earlier], [new(CardKind.Slash, Suit.Spade, 7)], [], [victim, new(Survivor) { StartingHand = [] }], [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-xingshang-death-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Earlier}}}","revision":1,"triggers":[{"id":"{{{EarlierBinding}}}",
                    "window":"characterDied","subject":"owner","optional":true,"priority":100,
                    "effects":[{"op":"claimDeathCleanupCards","target":"owner","zones":["hand"]},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Earlier] = new { name = "先取手牌", description = "其他角色死亡时，可以获得其手牌，然后弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Earlier, catalog.Presentations[Earlier].Name, catalog.Presentations[Earlier].Description)
                { Program = catalog.Programs[Earlier], ProgramPresentation = catalog.Presentations[Earlier] });
            builder.AddGeneral(new(Victim, "死亡弃牌来源", "supporter", "standard:none", "qun", 4));
            builder.AddGeneral(new(Survivor, "仍在场的角色", "supporter", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "固定两张普通摸牌", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Heart, 5), new("standard:dodge", Suit.Diamond, 6)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "经典行殇原死亡入堆", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Victim, Survivor], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
