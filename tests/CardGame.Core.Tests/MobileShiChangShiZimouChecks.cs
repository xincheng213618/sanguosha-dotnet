using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class MobileShiChangShiZimouChecks
{
    private const string Zimou = "mobile:zimou";
    private const string Watch = "fixture:zimou-deck-gain-watch";
    private const string Owner = "fixture:zimou-owner";
    private const string Enemy = "fixture:zimou-enemy";
    private const string Reason = "skill-program.obtain-deck-cards";

    internal static void FourthActualUseObtainsPhysicalSlashFamilyCold()
    {
        VerifyFourthUse(CardKind.Slash);
        VerifyFourthUse(CardKind.ColdSlash);
    }

    private static void VerifyFourthUse(CardKind kind)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(kind));
        var setup = new ShanheBattleSetup(83, 1, "zimou-fourth-use", Owner, 8, 0, 0, 0, 0, [],
            Enumerable.Range(1, 4).Select(rank => new ShanheStartingCard(CardKind.Crossbow, Suit.Club, rank)).ToArray(),
            [], [new(Enemy) { StartingHand = [] }], [], new string('c', 64));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        var prompt = ReachPrompt(game);

        for (var ordinal = 1; ordinal <= 3; ordinal++)
        {
            Require(IsPlay(prompt), "The first three actions must return to the original Play window.");
            Equip(game, prompt);
            prompt = ReachPrompt(game);
            Require(IsPlay(prompt) && game.CreateSnapshot(0).Players[0].ActualPlayPhaseCardUseState?.UseCount == ordinal &&
                !game.CardMovements.Any(move => move.Reason.Value == Reason),
                "Each real equipment use advances the current Play count without obtaining an unrelated card.");
        }

        var pile = game.CreateCardZoneDiagnostics().Where(card => card.Location == CardLocation.DrawPile)
            .OrderByDescending(card => card.ZoneIndex).ToArray();
        Require(pile.Length > 1 && pile.All(card => card.CardKind == kind),
            $"The fourth use searches a physical draw pile containing only {kind}.");
        var expectedCard = pile[0].CardId;
        var usedCard = Equip(game, prompt);
        prompt = ReachPrompt(game);
        Require(prompt.SkillPrompt?.SkillId == Watch,
            $"Zimou's fourth actual use must obtain one physical {kind} and enter its native gain window.");

        var parent = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Zimou);
        var receipt = parent.DeckCardGain!;
        Require(receipt is { Source: { BindingId: "fourth-use", OwnerSeat: 0 }, Requested: 1 } &&
            receipt.CardIds.SequenceEqual([expectedCard]) && receipt.SelectedKinds.SequenceEqual([kind]) &&
            receipt.FilterKinds.SequenceEqual([CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash]) &&
            game.CreateSnapshot(0).Players[0].ActualPlayPhaseCardUseState?.UseCount == 4 &&
            !Events<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == parent.Id),
            "The accepted fourth use freezes the top matching physical card on the original, still active Zimou frame.");

        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        var restoredPrompt = game.CreateSnapshot(0).PendingDecision!;
        var restoredReceipt = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == parent.Id).DeckCardGain;
        Require(restoredPrompt.PromptId == prompt.PromptId && restoredReceipt is { } resumed &&
            resumed.BatchId == receipt.BatchId && resumed.CardIds.SequenceEqual([expectedCard]),
            "Cold replay preserves the original gain child, receipt and selected physical entity.");
        Accept(game, new AnswerPromptCommand(0, restoredPrompt.PromptId,
            restoredPrompt.Choices.Single(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision));
        Require(IsPlay(ReachPrompt(game)), "The original card use returns to its Play window after the gain child.");

        var move = game.CardMovements.Single(item => item.Reason.Value == Reason);
        var gain = Events<DeckCardsObtainedEvent>(game).Single();
        Require(move.CardId == expectedCard && move.CardKind == kind && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) &&
            gain is { OwnerSeat: 0, Count: 1 } && gain.ProgramFrameId == parent.Id && gain.BatchId == receipt.BatchId &&
            Events<ProgramBindingResolvedEvent>(game).Count(fact => fact.FrameId == parent.Id && fact.Completed) == 1 &&
            Events<CardUseFinishedEvent>(game).Count(fact => fact.CardId == usedCard) == 1,
            "The original physical Slash variant enters hand once, the same Zimou frame completes, and the fourth equipment use finishes.");
    }

    private static int Equip(GameEngine game, PendingDecision prompt)
    {
        var action = game.GetLegalActions(0).First(item => item.Kind == LegalActionKind.Equip && item.ConversionSource is null);
        var cardId = action.CardId!.Value;
        Accept(game, new PlayCardCommand(0, cardId, action.TargetSeats, game.Revision, prompt.PromptId, action.PlayedCardKind));
        return cardId;
    }

    private static bool IsPlay(PendingDecision prompt) => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };

    private static PendingDecision ReachPrompt(GameEngine game)
    {
        for (var step = 0; step < 120; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { PlayerSeat: 0 } prompt) return prompt;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Zimou fixture stalled before the next owner prompt.");
    }

    private static T[] Events<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private sealed class Fixture(CardKind kind) : IGameContentPackage
    {
        private const string Deck = "fixture:zimou-deck";
        private const string Ice = "fixture:zimou-cold-slash";
        public PackageManifest Manifest { get; } = new("fixture-zimou", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mobile:shi-chang-shi");
            var cold = CardCatalog.Get(CardKind.ColdSlash);
            builder.AddCard(new(Ice, cold.DisplayName, cold.CategoryName, cold.Description, CardKind.ColdSlash));
            var watcher = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"gain-child","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementReasons":["{{Reason}}"],"movementOccurrence":"perBatch",
                    "optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"continue",
                      "options":[{"id":"continue"},{"id":"other"}]}]}]}]}
                """, JsonSerializer.Serialize(new
                {
                    schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object>
                    {
                        [Watch] = new { name = "获得自谋实体", description = "确认原实体的获得。",
                            optionLabels = new Dictionary<string, string> { ["continue"] = "继续", ["other"] = "确认" } }
                    }
                }));
            builder.AddSkill(new(Watch, "获得自谋实体", "确认原实体的获得。")
                { Program = watcher.Programs[Watch], ProgramPresentation = watcher.Presentations[Watch] });
            builder.AddGeneral(new(Owner, "孙璋", "supporter", Zimou, "qun", 8, [Watch]));
            builder.AddGeneral(new(Enemy, "对手", "supporter", "standard:none", "wei", 8));
            builder.AddDeck(new(Deck, "固定单一杀牌堆", 0, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 15).Select(index =>
                    new ContentDeckPhysicalCard(kind == CardKind.Slash ? "standard:slash" : Ice, Suit.Spade, index % 13 + 1)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "自谋第四次真实用牌", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Owner, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
