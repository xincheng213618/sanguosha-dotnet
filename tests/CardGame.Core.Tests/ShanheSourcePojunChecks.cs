using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourcePojunChecks
{
    private const string Skill = StandardShanheSourceXuShengPackage.SkillId;
    private const string Owner = "fixture:source-pojun-owner", Target = "fixture:source-pojun-target";
    private const string Watch = "fixture:source-pojun-watch", Deck = "fixture:source-pojun-deck";

    public static void NativeSlashHoldsEveryHandCardDrainsMovementAndReturnsAtRealTurnEnd()
    {
        foreach (var extraTargetEquipment in new[] { false, true })
        {
            var scene = Scene.Start(extraTargetEquipment);
            var target = scene.View(1);
            var held = target.Hand.Select(card => card.Id).ToArray();
            var equipment = target.Equipment.Select(card => card.Id).ToArray();
            Need(held.Length == 5 && held.Length > target.Hp && target.Hp == 3,
                "The native whole-hand fixture has five real Dodge cards, exceeding the target's three HP.");
            var slash = scene.Game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([1]));
            Need(scene.Game.PredictPublicActionDamage(0, slash, 1).DamageAmount == 1,
                "Before the optional hold, the target's larger hand does not qualify for the native damage modifier.");
            scene.Submit(new PlayCardCommand(0, slash.CardId!.Value, [1], scene.Game.Revision, scene.P!.PromptId, slash.PlayedCardKind));
            scene.Until(() => scene.P?.Kind == DecisionKind.ProgramTrigger && scene.P.SkillPrompt?.SkillId == Skill);
            var owningUse = scene.Game.ResolutionStack.OfType<CardUseFrame>().Single(use => use.CardKind == CardKind.Slash);
            scene.Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            scene.Until(() => scene.P?.SkillPrompt?.SkillId == Watch);
            var placed = scene.Events<ShanheSourcePojunHeldEvent>().Single();
            Need(scene.P!.PlayerSeat == 1 && placed.OwnerSeat == 0 && placed.TargetSeat == 1 && placed.Count == held.Length &&
                placed.CardUseFrameId == owningUse.Id && placed.ActionId == owningUse.Action!.ActionId &&
                scene.View(1).HandCount == 0 && scene.View(1).PojunHoldCount == held.Length &&
                scene.View(1).Equipment.Select(card => card.Id).SequenceEqual(equipment) &&
                scene.Events<DamageAppliedEvent>().Length == 0 &&
                scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.Id == placed.ProgramFrameId &&
                    frame.SkillId == Skill && frame.PendingMovementContinuation is not null) &&
                held.All(id => scene.Game.CreateCardZoneDiagnostics().Single(card => card.CardId == id).Location == CardLocation.PojunHold(1) &&
                    scene.Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.Hand(1) &&
                        move.To == CardLocation.PojunHold(1) && move.Reason == CardMoveReasons.PojunHold) == 1),
                "Activating the original Slash moves all five hand entities once, leaves equipment untouched and waits on its actual holder's loss child before response or damage.");
            var expectedDamage = extraTargetEquipment ? 1 : 2;
            Need(scene.Game.PredictPublicActionDamage(0, slash, 1).DamageAmount == expectedDamage,
                "Public prediction observes the emptied hand and separately compares both actual equipment counts.");
            // Seat1 remains a real AI participant in the single-player mode.
            // Its published choice is observed read-only; the native AI answers
            // through one ordinary step, with no external seat1 command.
            Need(!scene.View(1).IsHuman && scene.P!.Choices.Single().Parameters.GetValueOrDefault("option-id") == "continue",
                "The actual holder's movement child publishes its one native AI continuation.");
            var childFrameId = scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Watch).Id;
            scene.Submit(new AdvanceOneStepCommand(scene.Game.Revision));
            scene.Play();
            Need(scene.View(1).Hp == 3 - expectedDamage && scene.Events<DamageAppliedEvent>() is
                [{ SourceSeat: 0, TargetSeat: 1, Amount: var applied }] && applied == expectedDamage &&
                scene.Events<ShanheSourcePojunHeldEvent>().Length == 1 &&
                scene.Events<ProgramOptionChosenEvent>().Count(fact => fact.FrameId == childFrameId && fact.SkillId == Watch &&
                    fact.OwnerSeat == 1 && fact.ChooserSeat == 1 && fact.OptionId == "continue") == 1 &&
                scene.Events<ProgramBindingResolvedEvent>().Count(fact => fact.SkillId == Watch && fact.Completed) == 1 &&
                scene.Game.CardMovements.Count(move => move.CardId == slash.CardId && move.From == CardLocation.Hand(0) && move.To == CardLocation.Processing) == 1 &&
                scene.Game.CardMovements.Count(move => move.CardId == slash.CardId && move.From == CardLocation.Processing &&
                    move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.UseFinished) == 1,
                "The original physical Slash pays once, the completed movement child returns once, and the damage modifier requires both hand and equipment comparisons.");
            scene.Submit(new EndPlayPhaseCommand(0, scene.Game.Revision, scene.P!.PromptId));
            scene.Until(() => scene.Events<PojunHoldReturnedEvent>().Length > 0);
            Need(scene.Events<PojunHoldReturnedEvent>() is [{ HolderSeat: 1, CardIds: var returned }] && returned.SequenceEqual(held) &&
                scene.View(1).PojunHoldCount == 0 && held.All(id =>
                    scene.Game.CardMovements.Count(move => move.CardId == id && move.From == CardLocation.PojunHold(1) &&
                        move.To == CardLocation.Hand(1) && move.Reason == CardMoveReasons.PojunHoldReturn) == 1) &&
                scene.View(1).Equipment.Select(card => card.Id).SequenceEqual(equipment),
                "The real current turn end returns each original held entity through the native hold lease once, without moving either equipment slot.");
        }
    }

    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? P => Enumerable.Range(0, 2).Select(seat => Game.CreateSnapshot(seat).PendingDecision)
            .FirstOrDefault(prompt => prompt is not null);
        public PlayerSnapshot View(int seat) => Game.CreateSnapshot(seat).Players[seat];
        public T[] Events<T>() => Game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected native source Pojun command."); }
        public void Answer(Func<PromptChoice, bool> pick)
        {
            var prompt = P!;
            Need(prompt.PlayerSeat == 0, "Only the real human seat submits an external Pojun answer.");
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(pick).Id, Game.Revision));
        }
        public void Play() => Until(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Until(Func<bool> done)
        {
            for (var step = 0; step < 140; step++)
            {
                if (done()) return;
                Need(P is null || !View(P.PlayerSeat).IsHuman, "Unexpected source Pojun boundary: " + JsonSerializer.Serialize(P));
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Source Pojun failed to reach its exact forward boundary.");
        }
        public static Scene Start(bool extraTargetEquipment)
        {
            var target = new ShanheCombatantDefinition(Target)
            {
                StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Dodge, Suit.Heart, 2), 5).ToArray(),
                StartingEquipment = extraTargetEquipment
                    ? [new(CardKind.Crossbow, Suit.Club, 1), new(CardKind.OffensiveHorse, Suit.Heart, 5)]
                    : [new(CardKind.Crossbow, Suit.Club, 1)]
            };
            var setup = new ShanheBattleSetup(71, 1, "source-pojun-native", Owner, 8, 0, 0, 0, 0, [],
                [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Club, 8), new(CardKind.Dodge, Suit.Heart, 3)],
                [new(CardKind.Crossbow, Suit.Club, 1)], [target], [], new string('d', 64)) { TacticSkillIds = [Skill] };
            var production = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            {
                PlayerGeneralId = StandardShanheSourceXuShengPackage.MediumGeneralId, Hp = 4,
                EnemyRoster = [new("classic:guan-yu")], TacticSkillIds = []
            });
            var registry = production.WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId
                ? mode with { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Concat([Owner, Target]).Distinct(StringComparer.Ordinal).ToArray() } : mode);
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
            }, registry);
            var scene = new Scene(game); scene.Submit(new StartGameCommand()); scene.Play(); return scene;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-pojun", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Watch}}}","revision":1,"triggers":[
                    {"id":"held-hand-child","window":"cardsMoved","subject":"owner","sourceZones":["hand"],
                     "movementOccurrence":"perOwnerBatch","movementReasons":["skill.pojun.hold"],"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"seen","options":[{"id":"continue"}]}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Watch] = new { name = "真实手牌离区孩子", description = "封存后继续原杀",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Owner, "专版破军持有者", "supporter", null, "wu", 8));
            builder.AddGeneral(new(Target, "真实封存目标", "supporter", Watch, "wei", 3));
            builder.AddDeck(new(Deck, "零随机起手与摸牌", 0, 0, [new("standard:slash", 20), new("standard:dodge", 20)]));
        }
    }
}
