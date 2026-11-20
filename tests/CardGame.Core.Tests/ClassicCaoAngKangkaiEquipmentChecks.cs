using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicCaoAngKangkaiEquipmentChecks
{
    private const string CaoAng = "classic:cao-ang", Kangkai = "classic:kangkai";
    private const string YangXiu = "ol:yang-xiu", Jilei = "ol:jilei", Driver = "fixture:kangkai-driver";
    private const string Mode = "identity:classic-kangkai-equipment", Deck = "fixture:classic-kangkai-equipment-deck";
    private const string Gift = "skill-program.classic:kangkai.SelectAndMoveOwnedCard";
    private const string GiftUse = "skill-program.classic:kangkai.UseBoundCardByTarget";
    private static readonly string[] Generals = [YangXiu, "fixture:kangkai-recipient", CaoAng, "fixture:kangkai-peer"];

    internal static void KangkaiCannotUseEquipmentForbiddenByJileiCold()
    {
        var (game, content) = Start();
        var gift = game.CreateSnapshot(2).Players[2].Hand.First(card => card.Kind == CardKind.Crossbow).Id;
        UseDriver(game, "incoming");
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Jilei });
        Require(Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 1, TargetSeat: 0, Amount: 1, SourceLess: false },
            "Real damage from the eventual gift recipient opens Yang Xiu's Jilei.");

        var control = Cold(game, content);
        Skip(control);
        GiveThroughKangkai(control, gift);
        Reach(control, IsEquipmentChoiceOrSlashResponse);
        Require(P(control) is { PlayerSeat: 1, SkillPrompt.SkillId: Kangkai } && P(control)!.Choices.Any(IsUseEquipment),
            "The original equipment has a usable gift choice when the recipient is unrestricted.");
        Answer(control, IsUseEquipment);
        Require(Facts<CardUseDeclaredEvent>(control).Any(fact => fact is { SourceSeat: 1, CardKind: CardKind.Crossbow } && fact.CardId == gift) &&
            control.CardMovements.Any(move => move.CardId == gift && move.From == CardLocation.Hand(1) && move.To == CardLocation.Processing && move.Reason.Value == GiftUse) &&
            control.CreateSnapshot(1).Players[1].Equipment.Any(card => card.Id == gift),
            "Accepting the unrestricted offer really uses and equips the exact gift before the parent Slash finishes.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Jilei } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "equipment"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "equipment");
        Require(Facts<HandCategoryRestrictionGrantedEvent>(game).Single().Restriction is
            { Category: SkillProgramCardCategory.Equipment, AffectedSeat: 1, TurnSeat: 0, Source.SkillId: Jilei },
            "Yang Xiu issues the actual current-turn equipment restriction against the future recipient.");
        game = Cold(game, content);
        GiveThroughKangkai(game, gift);
        Reach(game, IsEquipmentChoiceOrSlashResponse);
        var offered = P(game) is { PlayerSeat: 1, SkillPrompt.SkillId: Kangkai } pending && pending.Choices.Any(IsUseEquipment);
        if (offered) Answer(game, IsUseEquipment);
        Require(!offered && Facts<CardUseDeclaredEvent>(game).All(fact => fact.CardId != gift) &&
            game.CreateSnapshot(1).Players[1].Hand.Any(card => card.Id == gift),
            "Kangkai must deliver the original equipment without issuing or executing an equipment use prohibited by Jilei.");
        _ = Cold(game, content);
    }

    private static bool IsUseEquipment(PromptChoice choice) => choice.Parameters.GetValueOrDefault("option-id") == "use-equipment";

    private static bool IsEquipmentChoiceOrSlashResponse(PendingDecision prompt) =>
        prompt.PlayerSeat == 1 && (prompt.SkillPrompt?.SkillId == Kangkai || prompt.Kind == DecisionKind.RespondDodge) ||
        prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };

    private static void GiveThroughKangkai(GameEngine game, int gift)
    {
        Settle(game);
        UseDriver(game, "slash");
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Kangkai });
        Activate(game);
        Reach(game, prompt => prompt.PlayerSeat == 2 && (prompt.Choices.Any(choice => choice.Cards.SequenceEqual([gift])) ||
            prompt.Choices.Any(choice => choice.Targets.SequenceEqual([1]))));
        if (!P(game)!.Choices.Any(choice => choice.Cards.SequenceEqual([gift])))
        {
            Answer(game, choice => choice.Targets.SequenceEqual([1]));
            Reach(game, prompt => prompt.PlayerSeat == 2 && prompt.Choices.Any(choice => choice.Cards.SequenceEqual([gift])));
        }
        Answer(game, choice => choice.Cards.SequenceEqual([gift]));
        var movements = game.CardMovements.Where(move => move.CardId == gift && move.Reason.Value == Gift).ToArray();
        Require(movements.Length == 2 && movements[0].From == CardLocation.Hand(2) && movements[0].To == CardLocation.Processing &&
            movements[1].From == CardLocation.Processing && movements[1].To == CardLocation.Hand(1) &&
            Facts<ProgramCardsRevealedEvent>(game).Any(fact => fact.SkillId == Kangkai && fact.Cards.Single().Id == gift) &&
            game.CreateSnapshot(1).Players[1].Hand.Any(card => card.Id == gift),
            "Cao Ang completes a real public gift of the exact printed equipment to the Slash target.");
    }

    private static void UseDriver(GameEngine game, string mode) =>
        Accept(game, new UseProgramSkillCommand(0, Driver, mode, [], [1], game.Revision, P(game)!.PromptId));

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))) &&
            Facts<HandCategoryRestrictionGrantedEvent>(restored).SequenceEqual(Facts<HandCategoryRestrictionGrantedEvent>(game)),
            "Cold replay preserves the accepted commands, issued category restriction and exact gift movement.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "Yang Xiu starts the normal turn.");
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-kangkai-equipment", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new DriverBuilder(builder), CaoAng, YangXiu);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[
                  {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                    "effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]},
                  {"id":"slash","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                    "effects":[{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"normalSlashTarget","useCardActionWindows":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "伤害与用杀", description = "令另一角色对你造成实际伤害，或对其使用杀。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            foreach (var seat in new[] { 1, 3 })
                builder.AddGeneral(new(Generals[seat], "同桌", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定装备", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 40)
                .Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, 1)).ToArray() });
            builder.AddMode(new(Mode, "慷忾赠装合法性", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class DriverBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => inner.AddGeneral(general.Id == YangXiu ?
            general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Driver] } : general);
        public void AddSkill(ContentSkillDefinition skill) => inner.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => inner.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => inner.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => inner.AddMode(mode);
    }
}
