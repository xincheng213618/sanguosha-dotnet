using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryMobileJingceEquipmentDiscardChecks
{
    private const string Hero = "boundary:guo-huai-mobile", Jingce = "boundary:jingce-mobile";
    private const string Watch = "fixture:jingce-before-placement", Enemy = "fixture:jingce-enemy", Deck = "fixture:jingce-deck";
    private const string Abolished = "area.abolished.equipment";

    internal static void JingceCountsOnlyThePaidEquipmentUseThatFinishesIntoDiscardCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(83, 1, "fixture:jingce-equipment-discard", Hero, 1, 0, 0, 0, 0, [Watch],
            [new(CardKind.Crossbow, Suit.Club, 1)], [], [new(Enemy) { StartingHand = [] }], [], new string('b', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        Reach(game, () => IsPlay(game));
        var player = game.CreateSnapshot(0).Players[0];
        var weapon = player.Hand.Single(card => card.Kind == CardKind.Crossbow);
        Need(player.Hp == 1 && player.Equipment.Count == 0, "Guo Huai starts at the one-card Jingce threshold without a weapon.");
        var equip = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Equip && action.CardId == weapon.Id);
        Accept(game, new PlayCardCommand(0, weapon.Id, equip.TargetSeats, game.Revision, P(game)!.PromptId));
        Reach(game, () => IsChoice(game, Watch, "program-action", "activate"));
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single();
        var action = use.Action!;
        var payment = game.CardMovements.Single(move => move.CardId == weapon.Id && move.Reason == CardMoveReasons.EquipmentUse);
        Need(action is { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Crossbow, PhysicalCards: [var material] } &&
            material.CardId == weapon.Id && material.From == payment.From && payment.From == CardLocation.Hand(0) && payment.To == CardLocation.Processing &&
            Facts<BoundaryGuoHuaiDiscardedEntityEvent>(game).Length == 0,
            "The original accepted weapon pays its precise hand entity to Processing and has not yet contributed a discard.");

        var control = Cold(game, content);
        Answer(control, "program-action", "skip");
        Reach(control, () => IsPlay(control));
        Need(control.CardMovements.Count(move => move.CardId == weapon.Id && move.From == CardLocation.Processing &&
                move.To == CardLocation.Equipment(0) && move.Reason == CardMoveReasons.EquipmentEnter) == 1 &&
            Facts<CardUseFinishedEvent>(control).Count(fact => fact.ResolutionId == use.Id) == 1,
            "Declining the actual slot change installs the same physical weapon through the original native use.");
        EndPlay(control);
        Reach(control, () => IsChoice(control, Jingce, "program-action", "activate") || TurnEnded(control, payment.TurnNumber));
        Need(TurnEnded(control, payment.TurnNumber) && Facts<BoundaryGuoHuaiDiscardedEntityEvent>(control).Length == 0,
            "Installing the paid weapon contributes no Jingce entry and gives no own Ending reward.");

        Answer(game, "program-action", "activate");
        Reach(game, () => IsChoice(game, Watch, "option-id", "continue"));
        Need(game.CreateSnapshot(0).Players[0].IsEquipmentAreaAbolished &&
            Facts<PlayerAreasAbolishedEvent>(game).Count(fact => fact.Seat == 0 && fact.Equipment && !fact.Judgment) == 1 &&
            game.CardMovements.Last(move => move.CardId == weapon.Id) == payment,
            "The true slot-abolition child pauses with the original weapon payment still in Processing.");
        game = Cold(game, content);
        Answer(game, "option-id", "continue");
        Reach(game, () => IsPlay(game));
        var discard = game.CardMovements.Single(move => move.CardId == weapon.Id && move.Reason.Value == Abolished);
        Need(discard.From == CardLocation.Processing && discard.To == CardLocation.DiscardPile && discard.Sequence > payment.Sequence &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == use.Id) == 1 &&
            Facts<EquipmentChangedEvent>(game).All(fact => fact.ResolutionId != use.Id),
            "The original native weapon really finishes by entering the discard pile before it can be installed.");
        var credited = Facts<BoundaryGuoHuaiDiscardedEntityEvent>(game);
        Need(credited is [var item] && item.CardId == weapon.Id && item.NativeParentFrameId == use.Id &&
            item.ActionId == action.ActionId && item.MovementSequence == discard.Sequence,
            $"Jingce must count entry {discard.Sequence} from the paid weapon's original use {use.Id} once, observed {credited.Length} qualifying entries.");

        EndPlay(game);
        Reach(game, () => IsChoice(game, Jingce, "program-action", "activate") || TurnEnded(game, payment.TurnNumber));
        Need(IsChoice(game, Jingce, "program-action", "activate"), "The one genuine native discard reaches Guo Huai's current HP threshold.");
        game = Cold(game, content);
        Answer(game, "program-action", "activate");
        Reach(game, () => TurnEnded(game, payment.TurnNumber));
        var qualified = Facts<BoundaryGuoHuaiEndingQualifiedEvent>(game).Single();
        Need(qualified is { Count: 1, Hp: 1, ActualTurnOwnerSeat: 0 } &&
            Facts<ProgramDrawCompletedEvent>(game).Single(fact => fact.FrameId == qualified.FrameId) is
                { RecipientSeat: 0, RequestedCount: 2, ActualDrawCount: 2 } &&
            Facts<BoundaryGuoHuaiDiscardedEntityEvent>(game).SequenceEqual(credited),
            "The original ending grants a single real two-card draw from the exact recorded weapon finish.");
        _ = Cold(game, content);
    }

    private static bool TurnEnded(GameEngine game, int turn) => Facts<TurnEndedEvent>(game).Any(fact => fact.ActorSeat == 0 && fact.TurnNumber == turn);

    private static void EndPlay(GameEngine game) => Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));

    private static PendingDecision? P(GameEngine game) => game.CreateSnapshot(0).PendingDecision;
    private static bool IsPlay(GameEngine game) => P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };
    private static bool IsChoice(GameEngine game, string skill, string key, string value) => P(game) is { } prompt &&
        prompt.SkillPrompt?.SkillId == skill && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault(key) == value);
    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void Answer(GameEngine game, string key, string value)
    {
        var prompt = P(game)!;
        var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault(key) == value);
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
        throw new InvalidOperationException("Jingce did not reach its expected decision or return: " + P(game)?.Prompt);
    }

    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The real Jingce command was rejected.");
    }

    private static void Need(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Need(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold commands must retain the original weapon use, pre-placement child and exact own Ending threshold.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-boundary-mobile-jingce-equipment-discard", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"abolish-paid-slot","window":"cardUseTargetsFinalized",
                    "ownerRelation":"actor","cardCategories":["equipment"],"optional":true,"effects":[
                        {"op":"abolishOwnerAreas","target":"owner","zones":["equipment"]},
                        {"op":"chooseOption","target":"owner","resultBind":"after-abolish","options":[{"id":"continue"}]}]}]}]}
                """;
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Watch] = new { name = "废除装备区", description = "实际装备生效前可以废除装备区。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续原装备结算" } }
                }
            }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Enemy, "无固定技能的对手", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定闪", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 32)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "精策真实用装入弃", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
