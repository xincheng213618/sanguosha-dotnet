using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlSimaHuiChenghaoGainReplacementChecks
{
    private const string Chenghao = "ol:chenghao", Gujing = "happy:gujing", Driver = "fixture:chenghao-fire";
    private const string Mode = "identity:chenghao-replaced-gain", Deck = "fixture:chenghao-replaced-gain";
    private const string ViewReason = "program.sima-hui.chenghao.view", GiftReason = "program.sima-hui.chenghao.give";
    private static readonly string[] Generals = ["fixture:chenghao-gujing", "fixture:chenghao-abolished", "fixture:chenghao-owner", "fixture:chenghao-peer"];

    internal static void ChenghaoContinuesAfterItsFirstGiftIsRedirectedCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var play = Next(game);
        var bone = game.CreateSnapshot(0).Players[0].Hand.Single().Id;
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.CreateSnapshot(0).DrawPileCount == 4,
            "The four equipment cards remaining on the real draw pile are distinct from the opening hands.");
        Accept(game, new UseProgramSkillCommand(0, Gujing, "bone-path", [], [1], game.Revision, play.PromptId));
        GujingChoice(game, "card:" + bone);
        GujingChoice(game, "slot:Weapon");
        GujingChoice(game, "abolish:" + bone);
        GujingChoice(game, "finish");
        var issue = Facts<HappyWeiYanGujingIssuedEvent>(game).Single();
        Require(Facts<HappyWeiYanGujingAbolishedEvent>(game).Single() is { Holder: 1, Slot: EquipmentSlot.Weapon } abolished && abolished.FrameId == issue.FrameId &&
            game.CardMovements.Single(move => move.CardId == bone && move.Sequence == abolished.After) is { From: var from, To: var to } && from == CardLocation.Equipment(1) && to == CardLocation.DiscardPile,
            "A genuinely paid bone abolishes the selected opponent's weapon slot and issues Gujing's ongoing gain replacement.");

        Drive(game, "chain", 3);
        Drive(game, "fire", 3);
        var offer = Next(game);
        Require(offer is { PlayerSeat: 2, SkillPrompt.SkillId: Chenghao } &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 3, Nature: DamageNature.Fire } &&
            Facts<IronChainStateChangedEvent>(game).Any(fact => fact.TargetSeat == 3 && !fact.IsChained),
            "Chenghao is offered by the real elemental damage to its native chain origin.");
        Activate(game);
        var chooseFirst = Next(game);
        var watched = Facts<SimaHuiTopViewedEvent>(game).Single();
        Require(watched.Origin is { SourceSeat: 0, TargetSeat: 3, Nature: DamageNature.Fire, LivingChainedCount: 1 } && watched.CardIds.Count == 2 &&
            game.CardMovements.Where(move => move.Reason.Value == ViewReason).Select(move => move.CardId).SequenceEqual(watched.CardIds) &&
            chooseFirst is { PlayerSeat: 2, SkillPrompt.SkillId: Chenghao, IsPrivate: true } &&
            Enumerable.Range(0, 4).Where(seat => seat != 2).All(seat => game.CreateSnapshot(seat).PendingDecision is null),
            "The real chain origin exposes exactly two original equipment cards to the choosing viewer.");
        var first = watched.CardIds[0];
        var second = watched.CardIds[1];

        var control = Cold(game, content);
        Distribute(control, first, 3);
        var originalGift = Gift(control, first);
        Require(originalGift.From == CardLocation.Processing && originalGift.To == CardLocation.Hand(3) && originalGift.HappyWeiYanGainDestination is null,
            "With an intact weapon slot, the first real equipment reaches the chosen hand directly.");
        CheckSecondAndFinish(control, watched, first, second, firstRecipient: 3);

        game = Cold(game, content);
        try
        {
            Distribute(game, first, 1);
        }
        catch (InvalidOperationException error) when (error.Message == "Sima Hui lost its exact native source, frozen cards or completed payment invoice." &&
            RedirectedGift(game, first, issue.FrameId))
        {
            throw new InvalidOperationException("Chenghao failed to continue after its first real gift was redirected by Gujing.", error);
        }

        Require(RedirectedGift(game, first, issue.FrameId) && Zone(game, first) == CardLocation.Hand(0),
            "Gujing redirects the actual first equipment gift from the abolished slot to its living issuer once.");
        CheckSecondAndFinish(game, watched, first, second, firstRecipient: 0);
    }

    private static void CheckSecondAndFinish(GameEngine game, SimaHuiTopViewedEvent watched, int first, int second, int firstRecipient)
    {
        var next = Next(game);
        var binding = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == watched.FrameId);
        Require(next is { PlayerSeat: 2, SkillPrompt.SkillId: Chenghao, IsPrivate: true } &&
            next.Choices.All(choice => choice.Cards.SequenceEqual([second])) &&
            binding.SimaHui is { Cursor: 1, Stage: "distribute" } receipt && receipt.OriginalCards.SequenceEqual([first, second]) &&
            receipt.ChainOrigin == watched.Origin && Zone(game, second) == CardLocation.Processing,
            "The same paid Chenghao resumes at the second private card and retains the original native chain owner.");
        Distribute(game, second, 2);
        Require(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Gift(game, second).To == CardLocation.Hand(2) &&
            Zone(game, first) == CardLocation.Hand(firstRecipient) && Zone(game, second) == CardLocation.Hand(2) &&
            Facts<DamageAppliedEvent>(game).Count() == 1 && Facts<SimaHuiTopViewedEvent>(game).Count() == 1 &&
            !game.ResolutionStack.Any(frame => frame.Id == watched.FrameId),
            "Both real dispositions complete once and the original damage and Play phase finish normally.");
    }

    private static bool RedirectedGift(GameEngine game, int card, long issue) =>
        game.CardMovements.SingleOrDefault(move => move.CardId == card && move.Reason.Value == GiftReason) is { From: var from, To: var to, HappyWeiYanGainDestination: { } plan } &&
        from == CardLocation.Processing && to == CardLocation.Hand(0) && plan.RequestedTo == CardLocation.Hand(1) && plan.ActualTo == to && plan.IssueFrameId == issue;

    private static CardMovementRecord Gift(GameEngine game, int card) => game.CardMovements.Single(move => move.CardId == card && move.Reason.Value == GiftReason);

    private static void Distribute(GameEngine game, int card, int seat)
    {
        var prompt = Next(game);
        Require(prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Chenghao }, "The actual Chenghao choice is offered to Sima Hui.");
        Answer(game, choice => choice.Cards.SequenceEqual([card]) && choice.Targets.SequenceEqual([seat]));
    }

    private static void GujingChoice(GameEngine game, string option)
    {
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Gujing }, "The original Gujing owner chooses the real bone action.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == option);
    }

    private static void Drive(GameEngine game, string action, int target)
    {
        var play = Next(game);
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The real field change or damage starts in the original Play phase.");
        Accept(game, new UseProgramSkillCommand(0, Driver, action, [], [target], game.Revision, play.PromptId));
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        return game;
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold restore preserves the exact native chain frame, the replacement issue and only Sima Hui's original private choice.");
        return restored;
    }

    private static CardLocation Zone(GameEngine game, int card) => game.CreateCardZoneDiagnostics().Single(fact => fact.CardId == card).Location;
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-sima-hui-chenghao-replaced-gain", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "happy:shen-wei-yan", "ol:sima-hui");
            var rules = $$"""
                {"skills":[{"id":"{{Driver}}","revision":1,"activations":[
                  {"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving",
                   "usesPerTurn":null,"effects":[{"op":"setChainedState","target":"selectedTarget","chained":true}]},
                  {"id":"fire","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving",
                   "usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1,"nature":"fire"}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "连环与火伤", description = "横置一名角色或对一名角色造成火焰伤害。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Generals[0], "骨径使用者", "supporter", Gujing, "shu", 6, [Driver]));
            builder.AddGeneral(new(Generals[2], "司马徽", "supporter", Chenghao, "qun", 3));
            foreach (var seat in new[] { 1, 3 })
                builder.AddGeneral(new(Generals[seat], "同桌", "supporter", "standard:none", "wei", 5));
            builder.AddDeck(new(Deck, "真实诸葛连弩", InitialHandSize: 1, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(2, 8).Select(rank => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, rank)).ToArray()
            });
            builder.AddMode(new(Mode, "OL 司马徽称好转向实付", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
