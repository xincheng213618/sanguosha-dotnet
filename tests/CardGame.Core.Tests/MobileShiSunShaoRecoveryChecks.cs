using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileShiSunShaoRecoveryChecks
{
    private const string SunShao = "mobile:shi-sun-shao", Zhuji = "mobile:shi-zhuji";
    private const string Yaoli = StandardShanheCampaignCatalog.CurrentYaoli5SkillId;
    private const string Enemy = "fixture:zhuji-recovery-enemy", Deck = "fixture:zhuji-recovery-deck";
    private const string DiscardReason = "skill-program.mobile:shi-zhuji.discard", GainReason = "skill-program.mobile:shi-zhuji.gain";

    internal static void ZhujiRecoveryAllowsOneActualShanheYaoliBonusCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        VerifyRecovery(content, withYaoli: false);
        VerifyRecovery(content, withYaoli: true);
    }

    private static void VerifyRecovery(ContentRegistry content, bool withYaoli)
    {
        var setup = new ShanheBattleSetup(83, 1, "fixture:zhuji-recovery", SunShao, 1, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Dodge, Suit.Heart, 8)], [],
            [new(Enemy) { StartingHand = [] }], [], new string('b', 64)) { TacticSkillIds = withYaoli ? [Yaoli] : [] };
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        var player = game.CreateSnapshot(0).Players[0];
        var hand = player.Hand.Where(card => card.Suit == Suit.Heart).Select(card => card.Id).ToArray();
        Need(player.Hp == 1 && player.MaxHp == 4 && hand.Length == 2 && player.Skills.Any(skill => skill.ContentId == Yaoli) == withYaoli,
            "The real Shanhe setup starts Sun Shao wounded with the same two physical payment cards and the selected tactic.");
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
        Reach(game, () => HasBranch(game, "arm"));
        Answer(game, "arm");
        Reach(game, () => HasBranch(game, "hand"));
        Answer(game, "hand");
        Reach(game, () => HasBranch(game, "recover"));

        var discarded = Facts<ShiSsZhujiDiscardedEvent>(game).Single();
        var equipped = Facts<ShiSsZhujiEquippedEvent>(game).Single();
        Need(discarded is { OwnerSeat: 0, Suit: Suit.Heart, DiscardedCount: 2 } && hand.Contains(discarded.ChosenCardId) &&
            equipped.FrameId == discarded.FrameId && equipped.ReplacedCardId is null &&
            game.CardMovements.Where(move => move.From == CardLocation.Hand(0) && move.To == CardLocation.DiscardPile && move.Reason.Value == DiscardReason)
                .Select(move => move.CardId).Order().SequenceEqual(hand.Order()) &&
            game.CardMovements.Count(move => move.CardId == equipped.GainedCardId && move.From == CardLocation.DrawPile &&
                move.To == CardLocation.Hand(0) && move.Reason.Value == GainReason) == 1 &&
            game.CreateSnapshot(0).Players[0].Equipment.Any(card => card.Id == equipped.GainedCardId && card.Suit == Suit.Heart) &&
            Facts<RecoveryAppliedEvent>(game).Length == 0,
            "The original Zhuji pays both same-suit cards and installs the actual deck equipment before offering recovery.");
        game = Cold(game, content);
        Answer(game, "recover");
        Reach(game, () => Facts<ProgramBindingResolvedEvent>(game).Any(fact => fact.FrameId == discarded.FrameId));

        var expected = withYaoli ? 2 : 1;
        var recovered = Facts<RecoveryAppliedEvent>(game).Single();
        Need(recovered == new RecoveryAppliedEvent(0, 0, expected, 1 + expected),
            $"The original Zhuji recovery must heal {expected} with Yaoli {withYaoli}; observed {recovered.Amount}.");
        var bonuses = Facts<RecoveryAmountBonusAppliedEvent>(game);
        Need((withYaoli ? bonuses is [{ Receipt: { SourceSeat: 0, TargetSeat: 0, BaseRequestedAmount: 1, ModifiedRequestedAmount: 2,
                    HpBefore: 1, HpAfter: 3 } receipt }] && receipt.Sources.Select(source => source.Source.SkillId).SequenceEqual([Yaoli]) : bonuses.Length == 0) &&
            Facts<ShiSsZhujiDiscardedEvent>(game).Count(fact => fact.FrameId == discarded.FrameId) == 1 &&
            Facts<ShiSsZhujiEquippedEvent>(game).Count(fact => fact.FrameId == discarded.FrameId) == 1 &&
            Facts<ShiSsZhujiBonusEvent>(game).Single(fact => fact.FrameId == discarded.FrameId && fact.Choice == "recover").Amount == expected &&
            game.CreateSnapshot(0).Players[0].Hp == 1 + expected,
            "Only the real tactic modifies the single paid Zhuji recovery and the original equipment and choice settle once.");
        _ = Cold(game, content);
    }

    private static PendingDecision? P(GameEngine game) => game.CreateSnapshot(0).PendingDecision;

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static bool HasBranch(GameEngine game, string branch) => P(game) is { SkillPrompt.SkillId: Zhuji } prompt &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == branch);

    private static void Answer(GameEngine game, string branch)
    {
        var prompt = P(game)!;
        var choice = prompt.Choices.First(item => item.Parameters.GetValueOrDefault("branch") == branch);
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
    }

    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 60; step++)
        {
            if (reached())
                return;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Zhuji did not reach its expected decision or return: " + P(game)?.Prompt);
    }

    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The real Zhuji command was rejected.");
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
            "Cold commands preserve the two paid cards, obtained equipment, original bonus choice and the native recovery.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-mobile-shi-zhuji-recovery", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, SunShao);
            var productionSetup = new ShanheBattleSetup(83, 1, "fixture:zhuji-yaoli-content", "classic:guan-yu", 4, 0, 0, 0, 0,
                [], [], [], [new("classic:cao-cao")], [], new string('c', 64)) { TacticSkillIds = [Yaoli] };
            builder.AddSkill(StandardShanheCampaignCatalog.CreateBattleRegistry(productionSetup).GetSkill(Yaoli));
            builder.AddGeneral(new(Enemy, "无固定技能的对手", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定红桃装备", 0, 0, []) { PhysicalCards = Enumerable.Range(0, 24)
                .Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Heart, 1)).ToArray() });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "筑墼实际回复", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [SunShao, Enemy], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
