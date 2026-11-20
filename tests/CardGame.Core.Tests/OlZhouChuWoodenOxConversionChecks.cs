using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlZhouChuWoodenOxConversionChecks
{
    private const string Hero = "ol:zhou-chu", Yilie = "ol:yilie", Shanduan = "ol:shanduan";
    private const string Watch = "fixture:yilie-wooden-ox-watch", Peer = "fixture:yilie-wooden-ox-peer";
    private const string Deck = "fixture:yilie-wooden-ox-deck", WoodenOx = "classic:wooden-ox";

    internal static void YilieUsesOneGrainAndOneHandCardOncePerRoundCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "yilie-wooden-ox", Hero, 4, 0, 0, 0, 0, [Watch],
            [new(CardKind.WoodenOx, Suit.Diamond, 5), new(CardKind.Dodge, Suit.Heart, 2),
                new(CardKind.Dodge, Suit.Diamond, 3), new(CardKind.Dodge, Suit.Heart, 4),
                new(CardKind.Dodge, Suit.Diamond, 5), new(CardKind.Slash, Suit.Spade, 7)], [],
            [new(Peer) { StartingHand = [] }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 4
        }, registry);

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        IEnumerable<T> Facts<T>() where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>();
        void Need(bool success, string message)
        {
            if (!success)
                throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
                {
                    game.State.CurrentSeat, game.State.TurnNumber, game.State.Phase, Prompt = Prompt(),
                    Recent = game.Events.TakeLast(8).Select(item => item.Payload.GetType().Name)
                }));
        }
        void Submit(GameCommand command)
        {
            var result = game.Submit(command);
            Need(result.Accepted, result.Error?.Message ?? "The Yilie command was rejected.");
        }
        void Answer(Func<PromptChoice, bool> match)
        {
            var prompt = Prompt()!;
            Submit(new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(match).Id, game.Revision));
        }
        void Reach(Func<bool> done)
        {
            for (var step = 0; step < 80; step++)
            {
                if (done()) return;
                if (Prompt()?.SkillPrompt?.SkillId == Shanduan)
                {
                    var stat = Prompt()!.Choices[0].Parameters["stat-kind"];
                    var value = stat switch { "DrawCount" => "1", "AttackRange" => "2", "SlashLimit" => "3", _ => "4" };
                    Answer(choice => choice.Parameters["value"] == value);
                }
                else
                {
                    Need(Prompt() is null, "An unexpected human prompt interrupted the Yilie sequence.");
                    Submit(new AdvanceOneStepCommand(game.Revision));
                }
            }
            Need(false, "The fixed public commands did not reach the expected Yilie boundary.");
        }
        bool Play() => Prompt() is { Kind: DecisionKind.PlayCard } && game.State.CurrentSeat == 0;
        bool IsYilieSlash(LegalAction action) => action.ProgramSkillId == Yilie &&
            action.PlayedCardKind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash;

        Submit(new StartGameCommand());
        Reach(Play);
        var hand = game.CreateSnapshot(0).Players[0].Hand;
        int CardId(CardKind kind, Suit suit, int rank) => hand.Single(card => card.Kind == kind && card.Suit == suit && card.Rank == rank).Id;
        var treasureId = CardId(CardKind.WoodenOx, Suit.Diamond, 5);
        var grainId = CardId(CardKind.Dodge, Suit.Heart, 2);
        var handId = CardId(CardKind.Dodge, Suit.Diamond, 3);
        var normalSlashId = CardId(CardKind.Slash, Suit.Spade, 7);
        var selected = new[] { grainId, handId }.Order().ToArray();
        var alternate = new[] { CardId(CardKind.Dodge, Suit.Heart, 4), CardId(CardKind.Dodge, Suit.Diamond, 5) }.Order().ToArray();

        Submit(new PlayCardCommand(0, treasureId, [], game.Revision, Prompt()!.PromptId));
        Reach(Play);
        Submit(new UseEquipmentEffectCommand(0, CardKind.WoodenOx, [grainId], [], game.Revision, Prompt()!.PromptId));
        Reach(Play);
        Need(game.CreateSnapshot(0).Players[0].Equipment.Single().Id == treasureId &&
            game.CardMovements.Count(move => move.CardId == grainId && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.WoodenOxGrain(0) && move.Reason == CardMoveReasons.WoodenOxStore) == 1 &&
            game.CreateSnapshot(1).Players[0].WoodenOxGrain is { Count: 0 },
            "The actual equipped Wooden Ox stores one original hand card privately.");

        var actions = game.GetHumanLegalActions().Where(IsYilieSlash).ToArray();
        var pair = actions.SingleOrDefault(action => action.PlayedCardKind == CardKind.Slash &&
            action.SelectableCardIds.SequenceEqual(selected) && action.SelectableTargetSeats.SequenceEqual([1]));
        var alternative = actions.SingleOrDefault(action => action.PlayedCardKind == CardKind.FireSlash &&
            action.SelectableCardIds.SequenceEqual(alternate) && action.SelectableTargetSeats.SequenceEqual([1]));
        Need(pair is not null && alternative is not null &&
            actions.All(action => !action.SelectableCardIds.Order().SequenceEqual(new[] { grainId, normalSlashId }.Order())),
            "Official Yilie offers same-color grain plus hand and an independent second pair, while refusing the mixed-color pair.");
        Submit(new UseProgramSkillCommand(0, Yilie, pair!.ProgramActivationId!, selected, [1], game.Revision, Prompt()!.PromptId));
        Reach(() => Prompt()?.SkillPrompt?.SkillId == Watch);
        var receipt = Facts<RoundDistinctBasicUseAcceptedEvent>().Single().Receipt;
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == receipt.OwnerFrameId);
        var action = use.Action!;
        bool Paid(int cardId, CardLocation from) => game.CardMovements.Count(move => move.CardId == cardId &&
            move.From == from && move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1;
        Need(receipt is { ActorSeat: 0, ActualTurnOwnerSeat: 0, EffectiveKind: CardKind.Slash, CanonicalName: CardKind.Slash,
                FrozenIsRed: true, IsResponseUse: false } && receipt.Source == pair.ConversionSource &&
            receipt.Source.SkillId == Yilie && !string.IsNullOrWhiteSpace(receipt.Source.SkillInstanceId) &&
            receipt.ActualTurnNumber == game.State.TurnNumber && receipt.RoundNumber == Facts<RoundStartedEvent>().Last().RoundNumber &&
            action.Type == CardActionType.Use && action.ActionId == receipt.CardActionId &&
            action.PhysicalCards.Count == 2 && action.PhysicalCards.Any(card => card.CardId == grainId && card.From == CardLocation.WoodenOxGrain(0)) &&
            action.PhysicalCards.Any(card => card.CardId == handId && card.From == CardLocation.Hand(0)) &&
            Paid(grainId, CardLocation.WoodenOxGrain(0)) && Paid(handId, CardLocation.Hand(0)) &&
            game.CreateCardZoneDiagnostics().Count(card => selected.Contains(card.CardId) && card.Location == CardLocation.Processing) == 2,
            "The original accepted native Slash owns both actual same-color payments from their separate zones and the exact round receipt.");

        var snapshot = SnapshotJson.Serialize(game.CreateSnapshot(0));
        var journal = game.CardMovements.ToArray();
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        var restored = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == use.Id);
        Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == snapshot && game.CardMovements.SequenceEqual(journal) &&
            JsonSerializer.Serialize(restored.Action) == JsonSerializer.Serialize(action) &&
            restored.RoundDistinctBasicUses?.Single() == receipt && Facts<RoundDistinctBasicUseAcceptedEvent>().Single().Receipt == receipt,
            "Cold restore keeps the exact paid action and original round receipt without a second payment.");

        Answer(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(Play);
        var after = game.GetHumanLegalActions();
        Need(Facts<CardUseFinishedEvent>().Count(finished => finished.ResolutionId == use.Id) == 1 &&
            selected.All(id => game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.UseFinished) == 1) &&
            alternate.All(id => game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == id)) &&
            after.Any(candidate => candidate.Kind == LegalActionKind.Slash && candidate.CardId == normalSlashId && candidate.TargetSeat == 1) &&
            after.All(candidate => !IsYilieSlash(candidate)),
            "The original Slash completes once. Available ordinary Slash quota and a retained same-color pair cannot repeat Yilie's canonical Slash name in this round.");
        var movesBeforeRejection = game.CardMovements.Count;
        var denied = game.Submit(new UseProgramSkillCommand(0, Yilie, alternative!.ProgramActivationId!, alternate, [1], game.Revision, Prompt()!.PromptId));
        Need(!denied.Accepted && game.CardMovements.Count == movesBeforeRejection && Facts<RoundDistinctBasicUseAcceptedEvent>().Count() == 1,
            "Submitting the previously issued alternative Fire Slash cannot pay another pair in the same actual round.");
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-zhou-chu-wooden-ox", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var treasure = CardCatalog.Get(CardKind.WoodenOx);
            builder.AddCard(new(WoodenOx, treasure.DisplayName, treasure.CategoryName, treasure.Description, CardKind.WoodenOx));
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Watch}}","revision":1,
                    "triggers":[{"id":"paid-slash","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["slash"],
                        "optional":false,"isSkillInvocation":false,"effects":[
                            {"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{"{{Watch}}":{
                    "name":"观察义烈","description":"在实际杀付款后暂停。","optionLabels":{"continue":"继续"} } } }
                """);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "Slash target", "", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "Fixed Wooden Ox conversion", 0, 0, [])
            {
                PhysicalCards = Enumerable.Repeat(new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 9), 32).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Wooden Ox Yilie", 2, 2,
                new Dictionary<string, int>(), Deck, GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
