using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlBoundaryBuquRecoveryChecks
{
    private const string ZhouTai = "boundary:zhou-tai", SunShangxiang = "ol:sp-sun-shangxiang";
    private const string Buqu = "boundary:buqu-current", Fenji = "boundary:fenji-current", Liangzhu = "ol:liangzhu";
    private const string Source = "fixture:buqu-recovery-source", Peer = "fixture:buqu-recovery-peer", Driver = "fixture:buqu-recovery-driver";
    private const string Mode = "identity:classic-buqu-recovery", Deck = "fixture:buqu-recovery-deck";
    private const int Other = 0, ZhouTaiSeat = 1, SunShangxiangSeat = 2;
    private static readonly string[] Generals = [Source, ZhouTai, SunShangxiang, Peer];

    internal static void BuquInOwnPlayQualifiesRealLiangzhuAndDrawCold()
    {
        VerifyRecovery(atOwnPlay: false);
        VerifyRecovery(atOwnPlay: true);
    }

    private static void VerifyRecovery(bool atOwnPlay)
    {
        var (game, content) = Start();
        Accept(game, new UseProgramSkillCommand(Other, Driver, "wound", [], [ZhouTaiSeat], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: Other });
        Require(game.CreateSnapshot(ZhouTaiSeat).Players[ZhouTaiSeat].Hp == 1,
            "The real OL boundary Zhou Tai starts the native Fire Attack at one HP.");
        var source = atOwnPlay ? ZhouTaiSeat : Other;
        if (atOwnPlay)
        {
            Accept(game, new EndPlayPhaseCommand(Other, game.Revision, P(game)!.PromptId));
            Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: ZhouTaiSeat });
        }

        var fireAttack = game.GetLegalActions(source).First(action => action.Kind == LegalActionKind.FireAttack && action.TargetSeat == ZhouTaiSeat);
        Accept(game, new PlayCardCommand(source, fireAttack.CardId!.Value, [ZhouTaiSeat], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { Kind: DecisionKind.FireAttackReveal, PlayerSeat: ZhouTaiSeat });
        Answer(game, choice => choice.Cards.Count == 1);
        Reach(game, prompt => prompt.Kind == DecisionKind.FireAttackDiscard && prompt.PlayerSeat == source);
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.Count == 1);
        Reach(game, prompt => prompt.SkillPrompt?.SkillId == Liangzhu || prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == source);

        var wound = Facts<ProgramUniqueRankDyingResolvedEvent>(game).Single();
        Require(wound is { SkillId: Buqu, OwnerSeat: ZhouTaiSeat, Zone: CardZoneKind.BuquWound, RankWasUnique: true } &&
            wound.WoundCardIds.SequenceEqual([wound.CardId]) && game.CardMovements.Count(move => move.CardId == wound.CardId &&
                move.From == CardLocation.DrawPile && move.To == new CardLocation(CardZoneKind.BuquWound, ZhouTaiSeat)) == 1 &&
            Facts<PlayerDyingEvent>(game).Single() is { VictimSeat: ZhouTaiSeat } dyingEntry && dyingEntry.ResolutionId == wound.DyingFrameId &&
            Facts<DamageAppliedEvent>(game).Single() is { TargetSeat: ZhouTaiSeat, RemainingHp: 0, Nature: DamageNature.Fire } damage && damage.SourceSeat == source &&
            game.CreateSnapshot(ZhouTaiSeat).Players[ZhouTaiSeat].Hp == 1,
            "The native Fire Attack opens the original Dying and Buqu keeps exactly the real unique-rank draw-pile card as a wound.");

        if (!atOwnPlay)
        {
            Require(P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: Other } &&
                Facts<SpSunShangxiangRecoveryQualifiedEvent>(game).Length == 0 &&
                Facts<DyingResolvedEvent>(game).Single(fact => fact.ResolutionId == wound.DyingFrameId).Survived,
                "The same successful Buqu rescue on another player's turn does not qualify Liangzhu.");
            return;
        }

        Require(P(game) is { PlayerSeat: SunShangxiangSeat, SkillPrompt.SkillId: Liangzhu },
            "Buqu's actual recovery during Zhou Tai's own Play phase must open SP Sun Shangxiang's Liangzhu choice.");
        var rewardFrameId = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Liangzhu).Id;
        Require(game.ResolutionStack.OfType<DyingFrame>().Any(frame => frame.Id == wound.DyingFrameId) &&
            Facts<RecoveryAppliedEvent>(game).Count(fact => fact == new RecoveryAppliedEvent(ZhouTaiSeat, ZhouTaiSeat, 1, 1)) == 1 &&
            Facts<SpSunShangxiangRecoveryQualifiedEvent>(game).Single() is { RecoveredSeat: ZhouTaiSeat, HpBefore: 0, HpAfter: 1 } &&
            Facts<SpSunShangxiangHelpedEvent>(game).Length == 0,
            "One real self-recovery qualifies Liangzhu within the original Dying before the reward is paid.");
        game = Cold(game, content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "help");
        Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: ZhouTaiSeat });
        Require(Facts<ProgramDrawCompletedEvent>(game).Single(fact => fact.FrameId == rewardFrameId) is
                { RecipientSeat: ZhouTaiSeat, RequestedCount: 2, ActualDrawCount: 2 } &&
            Facts<RecoveryAppliedEvent>(game).Count(fact => fact.TargetSeat == ZhouTaiSeat) == 1 &&
            Facts<DyingResolvedEvent>(game).Single(fact => fact.ResolutionId == wound.DyingFrameId).Survived,
            "The recovered original actor receives Liangzhu's two physical draws and Dying returns without another recovery.");
        _ = Cold(game, content);
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static void Reach(GameEngine game, Func<PendingDecision, bool> reached)
    {
        for (var step = 0; step < 100; step++)
        {
            if (P(game) is { } prompt && reached(prompt))
                return;
            if (P(game)?.SkillPrompt?.SkillId == Fenji)
                Skip(game);
            else
                Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The Buqu fixture did not reach its expected prompt: " + P(game)?.Prompt);
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold accepted-command replay preserves the original Dying, wound card, and positive recovery frames and facts.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, ModeId = Mode, HumanSeat = Other,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-boundary-buqu-recovery", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, ZhouTai, SunShangxiang);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"wound","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"otherLiving","usesPerTurn":1,"effects":[{"op":"loseHp","target":"selectedTarget","amount":3}]}]}]}
                """;
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "准备体力", description = "令一名其他角色失去三点体力。" } } }));
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Source, "原出牌者", "supporter", Driver, "wei", 6));
            builder.AddGeneral(new(Peer, "其他角色", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "同花色火攻", 3, 0, []) { PhysicalCards = Enumerable.Range(0, 40)
                .Select(index => new ContentDeckPhysicalCard("standard:fire_attack", Suit.Heart, index % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "不屈原生回复", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
