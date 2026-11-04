using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ProgramStepReceiptChecks
{
    private const string PangDe = "boundary:jianchu-mobile", PangDeHero = "boundary:pang-de-mobile";
    private const string PangDePaymentReason = "skill-program." + PangDe + ".SelectAndMoveOwnedCard";

    public static void NativeDiscardSignsOnlyPaidPangDeContinuationCold()
    {
        foreach (var scenario in new[] { "unpaid", "claim", "response-rule", "owner-death" })
        {
            var (game, registry) = StartPangDePayment(scenario);
            Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            var slash = game.GetHumanLegalActions().First(action => action.CardId is { } id &&
                game.CreateSnapshot(0).Players[0].Hand.Any(card => card.Id == id && card.Kind == CardKind.Slash) &&
                action.TargetSeats.SequenceEqual([1]));
            Accept(game, new PlayCardCommand(0, slash.CardId!.Value, slash.TargetSeats, game.Revision,
                P(game)!.PromptId, slash.PlayedCardKind, slash.TargetCardId));
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == PangDe &&
                P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
            Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(game, () => P(game)?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == PangDe);
            var rootId = root.Id;
            var useId = root.WindowContext!.CardUse!.ParentCardUseFrameId;
            Need(root.InvocationAuthorization is null && !game.CardMovements.Any(move => move.Reason.Value == PangDePaymentReason),
                "The real Jianchu selection has no source-loss permission before its target card physically moves.");
            game = Cold(game, registry);
            if (scenario == "unpaid") DisableSource(game, PangDe);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("source-zone") ==
                (scenario == "response-rule" ? nameof(CardZoneKind.Equipment) : nameof(CardZoneKind.Hand)));
            if (scenario == "unpaid")
            {
                Reach(game, () => E<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == rootId));
                Need(!game.CardMovements.Any(move => move.Reason.Value == PangDePaymentReason) &&
                    !E<ProgramBindingResolvedEvent>(game).Single(fact => fact.FrameId == rootId).Completed &&
                    !E<ProgramCurrentUsePhysicalCardsClaimedEvent>(game).Any(fact => fact.Claim.ProducerFrameId == rootId),
                    "Losing the source at the unpaid selection cannot pay the target card or issue the claim suffix.");
                continue;
            }

            Reach(game, () => PangDePaymentOption(game, "paid"));
            root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == rootId);
            var parent = game.ResolutionStack.Single(frame => frame.Id == root.WindowContext!.ParentFrameId);
            Need(game.CardMovements.Count(move => move.Reason.Value == PangDePaymentReason) == 1 &&
                root.InvocationAuthorization is { RetainOwnerDeath: false } && ProgramInvocations.Allows(root, parent) &&
                !E<ProgramCurrentUsePhysicalCardsClaimedEvent>(game).Any(fact => fact.Claim.ProducerFrameId == rootId) &&
                !E<ProgramTargetSlashReceiptIssuedEvent>(game).Any(fact => fact.Receipt.ProducerFrameId == rootId),
                "The actual target discard signs the original window and child before its real movement observer returns, without paying a suffix early.");
            game = Cold(game, registry);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            if (scenario != "owner-death")
            {
                Reach(game, () => PangDePaymentOption(game, "lost"));
                root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == rootId);
                Need(!game.CreateSnapshot(0).Players[0].Skills!.Any(skill => skill.ContentId == PangDe) &&
                    ProgramInvocations.Allows(root, game.ResolutionStack.Single(frame => frame.Id == root.WindowContext!.ParentFrameId)),
                    "A real skillState child removes Jianchu while its paid original invocation remains authorized.");
                game = Cold(game, registry);
                Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            }
            Reach(game, () => E<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == rootId));
            var claimed = E<ProgramCurrentUsePhysicalCardsClaimedEvent>(game).Where(fact => fact.Claim.ProducerFrameId == rootId).ToArray();
            var rules = E<ProgramTargetSlashReceiptIssuedEvent>(game).Where(fact => fact.Receipt.ProducerFrameId == rootId).ToArray();
            Need(game.CardMovements.Count(move => move.Reason.Value == PangDePaymentReason) == 1,
                "Cold movement return never discards a second target card.");
            if (scenario == "owner-death")
                Need(!game.CreateSnapshot(0).Players[0].IsAlive && E<PlayerDiedEvent>(game).Any(fact => fact.VictimSeat == 0) &&
                    !E<ProgramBindingResolvedEvent>(game).Single(fact => fact.FrameId == rootId).Completed && claimed.Length == 0 && rules.Length == 0,
                    "The paid source-loss authorization does not retain an unpaid claim or response-rule suffix after actual owner death.");
            else if (scenario == "response-rule")
                Need(E<ProgramBindingResolvedEvent>(game).Single(fact => fact.FrameId == rootId).Completed && claimed.Length == 0 &&
                    rules is [var issued] && issued.CardUseFrameId == useId && issued.Receipt.TargetSeat == 1 && issued.Receipt.PreventCancellation,
                    "An equipment discard retains exactly one original-target Slash response prohibition after real source loss.");
            else
                Need(E<ProgramBindingResolvedEvent>(game).Single(fact => fact.FrameId == rootId).Completed && rules.Length == 0 &&
                    claimed is [var delivered] && delivered.Claim.CardUseFrameId == useId && delivered.Claim.RecipientSeat == 1 &&
                    delivered.Claim.CardIds.SequenceEqual([slash.CardId.Value]) && game.CardMovements.Count(move => move.CardId == slash.CardId &&
                        move.From == CardLocation.Processing && move.To == CardLocation.Hand(1)) == 1,
                    "A non-equipment discard still delivers this exact original Slash to its target once after real source loss.");
        }
    }

    private static bool PangDePaymentOption(GameEngine game, string binding) => P(game)?.SkillPrompt?.SkillId == Watch &&
        P(game)!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("result-bind") == binding);

    private static (GameEngine Game, ContentRegistry Registry) StartPangDePayment(string scenario)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new PangDePaymentFixture(scenario));
        var setup = new ShanheBattleSetup(7, 1, "pangde-paid-discard", PangDeHero, 4, 0, 0, 0, 0, [Watch],
            [new(CardKind.Slash, Suit.Spade, 7)], [],
            [new(Donor, Hp: 8) { StartingHand = [] }, new(Other, Hp: 8) { StartingHand = [] }],
            [new(Ally, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Club, 8)],
                StartingEquipment = scenario == "response-rule" ? [new(CardKind.Crossbow, Suit.Club, 9)] : [] }], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
            MaxTurns = 8 }, registry);
        Accept(game, new StartGameCommand());
        return (game, registry);
    }

    private sealed class PangDePaymentFixture(string scenario) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-pangde-payment", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, PangDeHero, "classic:ma-chao");
            var consequence = scenario == "owner-death" ? """{"op":"loseHp","target":"owner","amount":20}""" : $$$"""
                {"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{{PangDe}}}"]}},
                {"op":"chooseOption","target":"owner","resultBind":"lost","options":[{"id":"continue"}]}
                """;
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Watch}}","revision":1,"triggers":[
                    {"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]},
                    {"id":"paid-discard","window":"discardPileReceived","subject":"owner","discardOwnerScope":"other","sourceZones":["hand","equipment"],"movementOccurrence":"perBatch",
                     "movementReasons":["{{PangDePaymentReason}}"],"optional":false,"effects":[
                       {"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]},{{consequence}}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                {
                    [Watch] = new { name = "Actual discard child", description = "Lose the source or owner after real payment", optionLabels = new Dictionary<string, string> { ["continue"] = "Continue" } },
                    [Quiet] = new { name = "Quiet peer", description = "Skip unrelated Play" }
                } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            foreach (var id in new[] { Donor, Other, Ally }) builder.AddGeneral(new(id, "Fixed peer", "", Quiet, "qun", 8));
            const string deck = "fixture:pangde-payment-deck";
            builder.AddDeck(new(deck, "Fixed Dodge deck", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Actual discard continuation", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [PangDeHero, Donor, Other, Ally], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
