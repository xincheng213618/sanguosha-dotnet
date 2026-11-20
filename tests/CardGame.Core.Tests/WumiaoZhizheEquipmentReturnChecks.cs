using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class WumiaoZhizheEquipmentReturnChecks
{
    private const string Hero = "wumiao:zhuge-liang", Zhizhe = "wumiao:zhizhe", Qingshi = "wumiao:qingshi";
    private const string Watch = "fixture:zhizhe-before-placement", Enemy = "fixture:zhizhe-enemy", Deck = "fixture:zhizhe-deck";
    private const string Abolished = "area.abolished.equipment";

    internal static void ZhizheReturnsPaidEquipmentWhenSlotIsAbolishedBeforePlacementCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(83, 1, "fixture:zhizhe-equipment", Hero, 4, 0, 0, 0, 0, [Watch],
            [new(CardKind.Crossbow, Suit.Club, 1)], [], [new(Enemy) { StartingHand = [] }], [], new string('b', 64)) { ExtraSkillSlots = 1 };
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard });
        var original = game.CreateSnapshot(0).Players[0].Hand.Single(card => card.Kind == CardKind.Crossbow);
        Accept(game, new UseProgramSkillCommand(0, Zhizhe, "copy-hand", [original.Id], [], game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard });
        var copy = Facts<WuZhugeLiangCopyCreatedEvent>(game).Single();
        Need(copy.OriginalCardId == original.Id && copy.CardId != original.Id && copy.Kind == original.Kind &&
            copy.Suit == original.Suit && copy.Rank == original.Rank,
            "Zhizhe must really copy the selected physical Crossbow.");
        var equip = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Equip && action.CardId == copy.CardId);
        Accept(game, new PlayCardCommand(0, copy.CardId, equip.TargetSeats, game.Revision, P(game)!.PromptId));
        Reach(game, () => P(game) is { SkillPrompt.SkillId: Watch });

        var use = game.ResolutionStack.OfType<CardUseFrame>().Single();
        var payment = game.CardMovements.Single(move => move.CardId == copy.CardId && move.From == CardLocation.Hand(0) &&
            move.To == CardLocation.Processing && move.Reason == CardMoveReasons.EquipmentUse);
        Need(use.Action is { Type: CardActionType.Use, ActorSeat: 0, EffectiveKind: CardKind.Crossbow, PhysicalCards: [var material] } &&
            material.CardId == copy.CardId && material.From == payment.From && !Finished(game, use.Id, copy.CardId) &&
            !game.CreateSnapshot(0).Players[0].IsEquipmentAreaAbolished,
            "The actual Crossbow use must already own the copied material before the optional slot change.");

        var control = Cold(game, content);
        Answer(control, "program-action", "skip");
        Reach(control, () => P(control) is { Kind: DecisionKind.PlayCard });
        Need(Finished(control, use.Id, copy.CardId) &&
            control.CardMovements.Count(move => move.CardId == copy.CardId && move.From == CardLocation.Processing &&
                move.To == CardLocation.Equipment(0) && move.Reason == CardMoveReasons.EquipmentEnter) == 1 &&
            control.CreateSnapshot(0).Players[0].Equipment.Any(card => card.Id == copy.CardId) &&
            Facts<WuZhugeLiangCopyReturnedEvent>(control).Length == 0,
            "Declining the slot abolition must install the same copied Crossbow through the original native use once.");
        _ = Cold(control, content);

        Answer(game, "program-action", "activate");
        Reach(game, () => P(game) is { SkillPrompt.SkillId: Watch } prompt &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "continue"));
        Need(game.CreateSnapshot(0).Players[0].IsEquipmentAreaAbolished &&
            Facts<PlayerAreasAbolishedEvent>(game).Count(fact => fact.Seat == 0 && fact.Equipment && !fact.Judgment) == 1 &&
            game.ResolutionStack.OfType<CardUseFrame>().Single().Id == use.Id &&
            game.CardMovements.Last(move => move.CardId == copy.CardId) == payment,
            "The real target-finalized child must abolish the slot while the original paid equipment still resides in Processing.");
        game = Cold(game, content);
        Answer(game, "option-id", "continue");
        Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard });
        var exit = game.CardMovements.Single(move => move.CardId == copy.CardId && move.From == CardLocation.Processing &&
            move.Reason.Value == Abolished);
        Need(Finished(game, use.Id, copy.CardId) && exit.To == CardLocation.Hand(0),
            $"The original paid Crossbow entry {payment.Sequence} must return directly to hand after its slot is abolished; exit {exit.Sequence} went to {exit.To}.");
        var returned = Facts<WuZhugeLiangCopyReturnedEvent>(game).Single();
        var player = game.CreateSnapshot(0).Players[0];
        Need(returned.OwnerSeat == 0 && returned.CardId == copy.CardId && returned.EntrySequence == payment.Sequence &&
            returned.ReturnSequence == exit.Sequence && returned.Reason == exit.Reason &&
            player.Hand.Any(card => card.Id == original.Id) &&
            player.Hand.Single(card => card.Id == copy.CardId).SkillMarkLabel == "智哲·本回合禁用",
            "The same physical copy receives its actual original-entry return receipt and the rest-of-turn restriction.");
        _ = Cold(game, content);
    }

    private static bool Finished(GameEngine game, long useId, int cardId) =>
        Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == useId && fact.CardId == cardId) == 1;

    private static PendingDecision? P(GameEngine game) => game.CreateSnapshot(0).PendingDecision;

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void Answer(GameEngine game, string key, string value)
    {
        var prompt = P(game)!;
        var choice = prompt.Choices.Single(item => item.Parameters.GetValueOrDefault(key) == value);
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
    }

    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 60; step++)
        {
            if (reached())
                return;
            if (P(game) is { Kind: DecisionKind.ProgramTopReorder } prompt)
                Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.First().Id, game.Revision));
            else if (P(game) is { SkillPrompt.SkillId: Qingshi })
                Answer(game, "program-action", "skip");
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Zhizhe did not reach the expected decision or return: " + P(game)?.Prompt);
    }

    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The real Zhizhe command was rejected.");
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
            "Cold commands must preserve the copied equipment, its paid use, slot change and actual settled destination.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-wumiao-zhizhe-equipment-return", new(1, 0));

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
                    [Watch] = new { name = "废除装备区", description = "实际使用的装备生效前，可以废除装备区。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续原装备结算" } }
                }
            }));
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Enemy, "无固定技能的对手", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定闪", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 24)
                .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 2)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "智哲装备结算", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
