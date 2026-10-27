using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlSpLiuBei664Checks
{
    private const string Hero = StandardOlSpLiuBei664Package.GeneralId, Xudai = StandardOlSpLiuBei664Package.XudaiSkillId,
        Zhujiu = StandardOlSpLiuBei664Package.ZhujiuSkillId, Jinglei = StandardOlSpLiuBei664Package.JingleiSkillId,
        Ally = "fixture:sp664-ally", Enemy = "fixture:sp664-enemy", Driver = "fixture:sp664-driver";

    public static void RealBrewQuotaLimitedGrantAndThunderAdjustmentResumeCold() =>
        RealBrewQuotaLimitedGrantAndThunderAdjustment(restoreAtBoundaries: true);

    private static void RealBrewQuotaLimitedGrantAndThunderAdjustment(bool restoreAtBoundaries)
    {
        var (game, registry) = Create(true, 4); Play(game);
        Brew(game, [Hand(game).First(card => card.Kind == CardKind.Slash && card.Suit == Suit.Club).Id]);
        Reach(game, () => game.PendingDecision is { PlayerSeat: 0 } prompt && prompt.SkillPrompt?.SkillId == Xudai && prompt.Choices.Any(choice => choice.Targets.SequenceEqual([1])));
        Need(game.Events.Count(item => item.Payload is DamageAppliedEvent { SourceLess: true, Nature: DamageNature.Thunder, TargetSeat: 0, Amount: 1 }) == 1 &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Jinglei && frame.OlSpLiuBei664?.Stage == "damage" && frame.AttackAttempt is not null),
            "The real Jinglei thunder payment is already applied while the original paid frame waits for Xudai's actual damage child.");
        if (restoreAtBoundaries) game = Cold(game, registry);
        Answer(game, choice => choice.Targets.SequenceEqual([1]));
        Reach(game, () => game.PendingDecision is { PlayerSeat: 0 } prompt && prompt.SkillPrompt?.SkillId == Jinglei && prompt.Choices.Any(choice => choice.Targets.SequenceEqual([1])));
        Answer(game, choice => choice.Targets.SequenceEqual([1])); Play(game);
        var discarded = game.CardMovements.Where(move => move.Reason.Value == "skill.ol-sp-liu-bei.jinglei.discard").ToArray();
        var given = game.CardMovements.Where(move => move.Reason.Value == "skill.ol-sp-liu-bei.jinglei.give").ToArray();
        Need(game.CreateSnapshot(0).Players[0].Hp == 3 && discarded.Length == 3 && given.Length == 3 &&
            discarded.Select(move => move.CardId).SequenceEqual(given.Select(move => move.CardId)) &&
            game.Events.Any(item => item.Payload is SkillsAcquiredEvent acquired && acquired.PlayerSeat == 1 && acquired.SourceSkillId == Xudai && acquired.SkillIds.Contains(Zhujiu)),
            "Xudai grants the actual other holder, Jinglei adjusts that holder to MaxHp and transfers only its three real discarded entities without repeating its paid damage.");
        Slash(game); Play(game);
        var club = Hand(game).First(card => card.Suit == Suit.Club).Id;
        var heart = Hand(game).First(card => card.Suit == Suit.Heart).Id;
        Brew(game, [club, heart]); Play(game);
        var wines = game.Events.Select(item => item.Payload).OfType<OlSpLiuBeiZhujiuIssuedEvent>().ToArray();
        var winePayments = game.Events.Select(item => item.Payload).OfType<JianjiOrdinaryAlcoholPaidEvent>()
            .Where(fact => fact.ActorSeat == 0 && fact.CountsQuota).ToArray();
        Need(wines.Select(item => (item.Minimum, item.PaidCount, item.AllClub))
                .SequenceEqual(new[] { (1, 1, true), (2, 2, false) }) &&
            winePayments.Select(fact => fact.NativeUseId).SequenceEqual(wines.Select(fact => fact.UseId)) &&
            winePayments.Select(fact => fact.NativeUseId).Distinct().Count() == 2 &&
            game.Events.Count(item => item.Payload is OlSpLiuBeiJingleiPaidEvent) == 1 &&
            game.Events.Count(item => item.Payload is OlSpLiuBeiZhujiuDisabledEvent) == 1,
            "The next actual wine costs at least X+1 materials; every material must be club to retain the skill, and Jinglei spends one shared actual-turn use.");
        Slash(game); Play(game);
        Need(game.GetHumanLegalActions().All(action => action.ProgramSkillId != Zhujiu), "Non-club material disables brewing for the remainder of this actual turn.");
        Use(game, "ping"); Play(game);
        Need(game.Events.Count(item => item.Payload is SkillUsageConsumedEvent usage && usage.SkillId == Xudai && usage.Scope == SkillUsageScope.Game) == 1,
            "A later real damage occurrence cannot repay the already-spent limited grant.");
        if (restoreAtBoundaries) game = Cold(game, registry);

        var (ordinary, _) = Create(false, 4); Play(ordinary);
        Brew(ordinary, [Hand(ordinary).First(card => card.Suit == Suit.Club).Id]); Play(ordinary, decline: true);
        Slash(ordinary); Play(ordinary, decline: true);
        Need(ordinary.Events.All(item => item.Payload is not OlSpLiuBeiZhujiuDisabledEvent) &&
            ordinary.GetHumanLegalActions().All(action => action.ProgramSkillId != Zhujiu),
            "An all-club conversion preserves Zhujiu but cannot exceed the ordinary wine quota without an actual extra-use policy.");
    }

    private static (GameEngine, ContentRegistry) Create(bool tiger, int hp)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardOlSpLiuBei664Package(), new StandardShanheWineTacticsPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "sp664", Hero, hp, 0, 0, 0, 3, [Driver],
            Enumerable.Range(1, 5).Select(rank => new ShanheStartingCard(CardKind.Slash, Suit.Club, rank))
                .Concat([new(CardKind.Peach, Suit.Heart, 6), new(CardKind.Peach, Suit.Diamond, 7)]).ToArray(), [],
            [new(Enemy) { StartingHand = [] }], [new(Ally) { StartingHand = Enumerable.Range(1, 7).Select(rank => new ShanheStartingCard(CardKind.Dodge, Suit.Spade, rank)).ToArray() }], new string('a', 64))
        { TacticSkillIds = tiger ? [StandardShanheCampaignCatalog.CurrentHugujiuSkillId] : [] };
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, AdvanceAfterHumanCommands = false,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, MaxTurns = 4, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, registry);
        Submit(game, new StartGameCommand()); return (game, registry);
    }
    private static IReadOnlyList<CardSnapshot> Hand(GameEngine game) => game.CreateSnapshot(0).Players[0].Hand;
    private static void Use(GameEngine game, string activation) => Submit(game, new UseProgramSkillCommand(0, Driver, activation, [], [], game.Revision, game.PendingDecision!.PromptId));
    private static void UseBrew(GameEngine game) => Submit(game, new UseProgramSkillCommand(0, Zhujiu, "brew", [], [], game.Revision, game.PendingDecision!.PromptId));
    private static void Brew(GameEngine game, IReadOnlyList<int> cards)
    {
        UseBrew(game);
        foreach (var card in cards) Answer(game, choice => choice.Cards.SequenceEqual([card]));
        Answer(game, choice => choice.Id.Value.EndsWith(".use", StringComparison.Ordinal));
    }
    private static void Slash(GameEngine game) => Submit(game, new PlayCardCommand(0, Hand(game).First(card => card.Kind == CardKind.Slash).Id, [2], game.Revision, game.PendingDecision!.PromptId));
    private static void Answer(GameEngine game, Func<PromptChoice, bool> predicate)
    { var prompt = game.PendingDecision!; Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(predicate).Id, game.Revision)); }
    private static void Play(GameEngine game, bool decline = false) => Reach(game, () => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, decline);
    private static void Reach(GameEngine game, Func<bool> reached, bool decline = false)
    {
        for (var step = 0; step < 180; step++)
        {
            if (reached()) return;
            if (game.PendingDecision is { PlayerSeat: 0 } prompt)
            {
                if (prompt.Kind == DecisionKind.PlayCard) throw new InvalidOperationException("SP664 returned to play before the expected paid child.");
                var choice = prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("program-action") == (decline ? "skip" : "activate")) ??
                    (!decline ? prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") == "program-trigger" && item.Parameters.GetValueOrDefault("skill-id") == Zhujiu) : null) ??
                    prompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is "take-damage" or "pass" or "let-die") ?? prompt.Choices[0];
                Submit(game, new AnswerPromptCommand(0, prompt.PromptId, choice.Id, game.Revision));
            }
            else Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("SP664 did not reach its bounded native boundary.");
    }
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var checkpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), registry);
        Need(SnapshotJson.Serialize(cold.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)) && GameCheckpointJson.Serialize(cold.CreateCheckpoint()) == checkpoint,
            "The accepted native costs and private pending original-frame choices reconstruct exactly from the real command journal.");
        return cold;
    }
    private static void Submit(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "SP664 native command rejected."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-sp664", new Version(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var rules = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new[] { new { id = Driver, revision = 1,
                activations = new[] {
                    new { id = "lose", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "loseHp", target = "owner", amount = 2 } } },
                    new { id = "ping", minCards = 0, maxCards = 0, minTargets = 0, maxTargets = 0, targetKind = "anyLiving", usesPerTurn = (int?)null,
                        effects = new[] { new { op = "damage", target = "owner", amount = 1 } } }
                } } } });
            var programs = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "原生测试", description = "SP664 native fixture" } } }));
            builder.AddSkill(new(Driver, "原生测试", "fixture") { Program = programs.Programs[Driver] });
            builder.AddGeneral(new(Ally, "持牌队友", "", null, "shu", 4));
            builder.AddGeneral(new(Enemy, "敌军", "", null, "wei", 20));
            builder.AddDeck(new("fixture:sp664-deck", "无随机初始牌", 0, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "SP664机制", 3, 3, new Dictionary<string, int>(), "fixture:sp664-deck",
                GeneralPoolIds: [Hero, Ally, Enemy], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
