using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ProgramStepReceiptChecks
{
    private const string Anxu = "mobile:anxu", Zhijian = "boundary:zhijian-mobile";
    private const string Ally = "fixture:step-recipient", Donor = "fixture:step-donor", Other = "fixture:step-other";
    private const string Gain = "fixture:step-gain", Watch = "fixture:step-watch", Quiet = "fixture:step-quiet";

    public static void DeliveredStepsRetainPaidReturnsWithoutRepeatingMovement()
    {
        foreach (var scenario in new[] { "anxu", "anxu-before-payment-loss", "anxu-source-loss", "anxu-owner-death", "anxu-moved-away", "zhijian", "zhijian-source-loss", "zhijian-owner-death" })
        {
            var skill = scenario.StartsWith("anxu", StringComparison.Ordinal) ? Anxu : Zhijian;
            var (game, registry) = Start(skill, scenario);
            Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            var activation = skill == Anxu ? "donor-gives-to-lower-hand" : "equip-target";
            Accept(game, new UseProgramSkillCommand(0, skill, activation, [], skill == Anxu ? [] : [1], game.Revision, P(game)!.PromptId));
            if (skill == Anxu)
            {
                Reach(game, () => P(game)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-targets") == true);
                Answer(game, c => c.Targets.SequenceEqual([1, 2]));
            }
            Reach(game, () => P(game)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
            var root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == skill);
            var reason = "skill-program." + skill + ".SelectAndMoveOwnedCard";
            Need(root.SelectedCardPayment is null && !game.CardMovements.Any(m => m.Reason.Value == reason),
                "Publishing the original payment prompt does not commit a card or create a delivered payment.");
            game = Cold(game, registry);
            if (scenario == "anxu-before-payment-loss") DisableSource(game, skill);
            if (P(game)!.PlayerSeat == 0) Answer(game, _ => true);
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
            if (scenario == "anxu-before-payment-loss")
            {
                Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
                Need(!game.CardMovements.Any(m => m.Reason.Value == reason) &&
                    E<ProgramSkillResolvedEvent>(game).Single(e => e.FrameId == root.Id).Completed == false,
                    "A signed cursor and issued choice cannot retain a source lost before physical payment.");
                continue;
            }
            if (scenario == "zhijian-owner-death")
            {
                Reach(game, () => !game.State.Players[0].IsAlive &&
                    (E<ProgramSkillResolvedEvent>(game).Any(e => e.FrameId == root.Id) || game.State.Winner != Winner.None));
                Need(game.CardMovements.Count(m => m.Reason.Value == reason && m.To == CardLocation.Equipment(1)) == 1 &&
                    !game.CardMovements.Any(m => m.Reason.Value == "skill-program." + skill + ".Draw"),
                    "Real delivery grants source-loss retention without granting an owner-death reward.");
                continue;
            }
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == (skill == Anxu ? Gain : Watch) &&
                P(game)!.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
            root = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == root.Id);
            Need(root.SelectedCardPayment is { MovementCommitted: true, Movement: { Delivered: true } } &&
                !game.CardMovements.Any(m => m.Reason.Value == "skill-program." + skill + ".Draw"),
                "The actual destination is committed on the original frame before its movement child returns or rewards run.");
            var card = root.SelectedCardPayment!.CardIds.Single();
            var movement = root.SelectedCardPayment.Movement!;
            var destination = skill == Anxu ? CardLocation.Hand(1) : CardLocation.Equipment(1);
            Need(movement.CardId == card && movement.To == destination && movement.Sequence > 0 &&
                game.CardMovements.Any(m => m.CardId == card && m.Sequence == movement.Sequence && m.From == movement.From && m.To == destination) &&
                game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Any(w => w.Batch.Id == movement.BatchId && w.Batch.Movements.Any(m => m.Sequence == movement.Sequence)),
                "The receipt names the actual terminal delivery entity, destination, batch and last movement sequence.");
            if (skill == Anxu)
                Need(root.SelectedCardPayment.PlacementSourceLocation == CardLocation.Hand(2) &&
                    (movement.From == CardLocation.Hand(2) || movement.From == CardLocation.Processing),
                    "Anxu retains the donor's original Hand source even when terminal delivery returns from Processing.");
            if (scenario.Contains("source-loss", StringComparison.Ordinal) || scenario == "anxu-moved-away")
            {
                DisableSource(game, skill);
                Need(!game.CreateSnapshot(0).Players[0].Skills!.Any(s => s.Id == skill),
                    "The actual source grant is closed while its original delivery movement child is still paused.");
            }
            if (scenario == "zhijian-source-loss")
                Need(game.CardMovements.Last(m => m.CardId == card).To == CardLocation.DiscardPile,
                    "A native child moves the delivered equipment away without undoing its historical delivery.");
            game = Cold(game, registry);
            if (scenario == "anxu-owner-death")
            {
                Reach(game, () => !game.State.Players[0].IsAlive &&
                    (E<ProgramSkillResolvedEvent>(game).Any(e => e.FrameId == root.Id) || game.State.Winner != Winner.None));
                Need(!game.CardMovements.Any(m => m.Reason.Value == "skill-program." + skill + ".Draw"),
                    "Anxu's paid child cannot authorize an owner-death reward.");
                continue;
            }
            if (skill == Anxu && scenario != "anxu-moved-away")
            {
                Reach(game, () => P(game)?.SkillPrompt?.SkillId == Anxu);
                Need(P(game)!.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "draw"),
                    "Anxu evaluates the two live participants' equal hands only after the complete gifted-card child.");
                Answer(game, c => c.Parameters.GetValueOrDefault("option-id") == "draw");
            }
            Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            Need(game.CardMovements.Count(m => m.CardId == card && m.Reason.Value == reason && m.To == destination) == 1 &&
                game.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program." + skill + ".Draw") == (scenario == "anxu-moved-away" ? 0 : 1) &&
                E<ProgramSkillResolvedEvent>(game).Count(e => e.FrameId == root.Id && e.Completed) == 1,
                "Cold movement return completes the same paid frame, moves the original entity once, and awards one owner draw even after paid source loss.");
            if (scenario == "anxu-moved-away")
                Need(game.CardMovements.Last(m => m.CardId == card).To == CardLocation.DiscardPile &&
                    !E<ProgramOptionChosenEvent>(game).Any(e => e.FrameId == root.Id),
                    "Moving the gift away preserves paid completion after source loss, while the later unequal hands correctly skip the benefit choice.");
        }
    }

    private static IEnumerable<T> E<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static void DisableSource(GameEngine game, string skill)
    {
        var owner = ((IReadOnlyList<CharacterState>)FixtureRuntimeAccess.Engine.GetField("_players",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.GetValue(game)!)[0];
        foreach (var grant in owner.SkillGrants.Grants.Where(g => g.SkillId == skill).ToArray())
            owner.SkillGrants.SetEnabled(grant.GrantId, false);
    }
    private static PendingDecision? P(GameEngine game) => Enumerable.Range(0, 4).Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p => p is not null);
    private static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    {
        var prompt = P(game)!;
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision));
    }
    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 180; step++)
        {
            if (reached()) return;
            if (P(game) is { PlayerSeat: 0 } prompt)
            {
                if (prompt.SkillPrompt?.SkillId == Watch) Answer(game, _ => true);
                else if (prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("response") == "let-die"))
                    Answer(game, c => c.Parameters.GetValueOrDefault("response") == "let-die");
                else if (prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip"))
                    Answer(game, c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else throw new InvalidOperationException("Unexpected step receipt boundary: " + JsonSerializer.Serialize(prompt));
            }
            else Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded delivered-step fixture did not reach its expected boundary.");
    }
    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
        Need(result.Accepted, result.Error?.Message ?? "The real delivered-step command was rejected.");
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private static GameEngine Cold(GameEngine game, ContentRegistry registry) => GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
    private static string Hero(string skill) => skill == Anxu ? "mobile:bu-lian-shi" : "boundary:zhang-zhao-zhang-hong-mobile";

    private static (GameEngine Game, ContentRegistry Registry) Start(string skill, string scenario)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(skill, scenario));
        var setup = new ShanheBattleSetup(7, 1, "delivered-step", Hero(skill), 3, 0, 0, 0, 0, skill == Zhijian ? [Watch] : [],
            skill == Zhijian ? [new(CardKind.Crossbow, Suit.Club, 7)] : [], [],
            [new(Donor, Hp: 8) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 7), new(CardKind.Dodge, Suit.Heart, 8)] }, new(Other, Hp: 8) { StartingHand = [] }],
            [new(Ally, Hp: 8) { StartingHand = [] }], new string('e', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false,
            MaxTurns = 8, AiPolicyVersion = AiDecisionPolicy.CurrentVersion }, registry);
        Accept(game, new StartGameCommand());
        return (game, registry);
    }

    private sealed class Fixture(string skill, string scenario) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-delivered-step", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, skill == Anxu ? [Hero(skill), "classic:bu-lian-shi"] : [Hero(skill)]);
            var watchEffects = scenario == "zhijian-owner-death" ? """
                {"op":"loseHp","target":"owner","amount":3}
                """ : scenario == "zhijian-source-loss" ? """
                {"op":"selectTarget","target":"owner","targetKind":"otherPublicTeamAllies"},
                {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},
                  "zones":["equipment"],"count":1,"destination":"discardPile","awaitMovementTriggers":true},
                {"op":"chooseOption","target":"owner","resultBind":"child-return","options":[{"id":"continue"}]}
                """ : """
                {"op":"chooseOption","target":"owner","resultBind":"child-return","options":[{"id":"continue"}]}
                """;
            var gainTail = scenario == "anxu-owner-death" ? """
                ,{"op":"selectTarget","target":"owner","targetKind":"otherPublicTeamAllies"},{"op":"loseHp","target":"selectedTarget","amount":3}
                """ : scenario == "anxu-moved-away" ? """
                ,{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]}
                """ : "";
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Gain}}","revision":1,"triggers":[{"id":"gift-child","window":"cardsGained","subject":"owner","optional":false,
                    "destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile:anxu.SelectAndMoveOwnedCard"],
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"gift-return","options":[{"id":"continue"}]}{{gainTail}}]}]},
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"placement-child","window":"cardsMoved","subject":"owner","optional":false,
                    "sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:zhijian-mobile.SelectAndMoveOwnedCard"],
                    "effects":[{{watchEffects}}]}]},
                  {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,
                    "effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}]}
                """;
            var optionLabels = new Dictionary<string, string> { ["continue"] = "Continue" };
            var labels = new Dictionary<string, object> {
                [Gain] = new { name = Gain, description = "Native delivered-step child", optionLabels },
                [Watch] = scenario == "zhijian-owner-death" ? new { name = Watch, description = "Native owner death" } :
                    new { name = Watch, description = "Native delivered-step child", optionLabels },
                [Quiet] = new { name = Quiet, description = "Skip unrelated Play" }
            };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Ally, "Recipient", "", Quiet, "wu", 8, [Gain]));
            foreach (var id in new[] { Donor, Other }) builder.AddGeneral(new(id, "Other", "", Quiet, "qun", 8));
            const string deck = "fixture:delivered-step-deck";
            builder.AddDeck(new(deck, "Fixed Dodge deck", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "Delivered-step composition", 4, 4, new Dictionary<string, int>(), deck,
                GeneralPoolIds: [Hero(skill), Ally, Donor, Other], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
