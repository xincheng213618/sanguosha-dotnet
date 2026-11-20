using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class DeathInteractionChecks
{
    private const string Driver = "fixture:death-interaction-driver";
    private const string Observer = "fixture:death-interaction-observer";
    private const string OrdinarySource = "fixture:death-interaction-source";
    private const string RestingSource = "mobile:shi-chang-shi";
    private const string Target = "fixture:death-interaction-cai-wen-ji";
    private const string Survivor = "fixture:death-interaction-survivor";
    private const string Mode = "identity:classic-death-interaction";
    private const string Deck = "fixture:death-interaction-deck";
    private const string Fire = "fixture:death-interaction-fire-attack";
    private const string Xianfu = "ol:xianfu";
    private const string Duanchang = "classic:duanchang";

    internal static void FireAttackChainLosesDeadSourceAndPreservesOnlyQualifiedXianfuCold()
    {
        CheckSelfFire(rests: false, attackerHp: 2);
        CheckSelfFire(rests: false, attackerHp: 1);
        CheckSelfFire(rests: true, attackerHp: 1);
    }

    private static void CheckSelfFire(bool rests, int attackerHp)
    {
        var sourceGeneral = rests ? RestingSource : OrdinarySource;
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(sourceGeneral, attackerHp));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 4, ModeId = Mode, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        var generals = new[] { Observer, sourceGeneral, Target, Survivor };
        Accept(game, new StartGameCommand());
        Drive(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, generals);
        Require(Facts<XianfuRelationArmedEvent>(game).Single().Grant.TargetSeat == 1,
            "The real game-start Xianfu relation binds the future Fire Attack source.");
        Accept(game, new UseProgramSkillCommand(0, Driver, "chain", [], [1, 2], game.Revision, P(game)!.PromptId));
        Drive(game, () => PlayAt(game, 1));
        Require(game.CreateSnapshot(0).Players[1].IsChained && game.CreateSnapshot(0).Players[2].IsChained,
            "The source and Cai Wen Ji are both chained before the actual Fire Attack.");

        BeginSelfFire(game);
        game = Cold(game, content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "fire-attack-discard");
        Drive(game, () => Facts<PlayerDiedEvent>(game).Any(e => e.VictimSeat == 2) &&
            Facts<CardUseFinishedEvent>(game).Any(e => e.CardKind == CardKind.FireAttack) && game.ResolutionStack.Count == 0);

        var sourceSurvives = attackerHp > 1;
        int? expectedKiller = sourceSurvives ? 1 : null;
        var propagation = Facts<ChainedDamagePropagatedEvent>(game).Single();
        var fireDeath = Facts<PlayerDiedEvent>(game).Single(e => e.VictimSeat == 2);
        Require(propagation is { FromSeat: 1, TargetSeat: 2, Nature: DamageNature.Fire, Amount: 1 } &&
            propagation.SourceLess == !sourceSurvives && fireDeath.KillerSeat == expectedKiller,
            "Propagated fire keeps its original damage and credits a killer only while the original damage source lives.");
        Require(Facts<DamageAppliedEvent>(game).Single(e => e.TargetSeat == 1) is { SourceSeat: 1, SourceLess: false } &&
            Facts<DamageAppliedEvent>(game).Single(e => e.TargetSeat == 2).SourceLess == !sourceSurvives &&
            Facts<CharacterSkillsLostEvent>(game).Any(e => e.Seat == 1 && e.SourceSeat == 2) == sourceSurvives,
            "The actual Duanchang skill removes skills from a surviving killer and has no valid target after the source dies.");
        var echo = Facts<XianfuEchoIssuedEvent>(game).Single();
        Require(echo.Receipt is { OriginalTargetSeat: 1, ActualAmount: 1 } &&
            Facts<XianfuEchoCompletedEvent>(game).Count(e => e.NativeChangeId == echo.Receipt.NativeChangeId) == 1,
            "The real applied damage qualifies one Xianfu echo even when that occurrence is lethal.");
        game = Cold(game, content);

        if (!rests)
            return;

        Require(Facts<PlayerDiedEvent>(game).Single(e => e.VictimSeat == 1).IsResting &&
            game.CreateSnapshot(0).Players[1] is { IsAlive: false, IsResting: true },
            "The first Fire Attack death of the real Shi Chang Shi enters rest.");
        Drive(game, () => PlayAt(game, 1));
        Require(Facts<PlayerReturnedFromRestEvent>(game).Single().PlayerSeat == 1 &&
            game.CreateSnapshot(0).Players[1] is { IsAlive: true, IsResting: false },
            "Shi Chang Shi resumes at its next normal turn and can be damaged again.");
        BeginSelfFire(game);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "fire-attack-discard");
        Drive(game, () => Facts<CardUseFinishedEvent>(game).Count(e => e.CardKind == CardKind.FireAttack) == 2 && game.ResolutionStack.Count == 0);
        Require(Facts<DamageAppliedEvent>(game).Count(e => e.TargetSeat == 1) == 2 &&
            Facts<XianfuEchoQualifiedEvent>(game).Length == 1 && Facts<XianfuEchoIssuedEvent>(game).Length == 1,
            "Real damage after the return cannot recreate the original game-start Xianfu relation.");
        _ = Cold(game, content);
    }

    private static void BeginSelfFire(GameEngine game)
    {
        var action = game.GetLegalActions(1).First(item => item.Kind == LegalActionKind.FireAttack && item.TargetSeat == 1);
        Accept(game, new PlayCardCommand(1, action.CardId!.Value, action.TargetSeats, game.Revision, P(game)!.PromptId, action.PlayedCardKind));
        Drive(game, () => P(game) is { Kind: DecisionKind.FireAttackReveal, PlayerSeat: 1 });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "fire-attack-reveal");
        Drive(game, () => P(game) is { Kind: DecisionKind.FireAttackDiscard, PlayerSeat: 1 });
    }

    private static bool PlayAt(GameEngine game, int seat) => P(game) is { Kind: DecisionKind.PlayCard } prompt && prompt.PlayerSeat == seat;

    private static void Drive(GameEngine game, Func<bool> ready, IReadOnlyList<string>? generals = null)
    {
        for (var step = 0; step < 200; step++)
        {
            if (ready())
                return;
            var prompt = P(game);
            if (prompt is null)
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            else if (prompt.Kind == DecisionKind.SelectGeneral && generals is not null)
                Accept(game, new SelectGeneralCommand(prompt.PlayerSeat, generals[prompt.PlayerSeat], game.Revision, prompt.PromptId));
            else if (prompt.Kind == DecisionKind.PlayCard)
                Accept(game, new EndPlayPhaseCommand(prompt.PlayerSeat, game.Revision, prompt.PromptId));
            else if (prompt.SkillPrompt?.SkillId == Xianfu)
                Answer(game, choice => choice.Targets.SequenceEqual([1]));
            else if (prompt.SkillPrompt?.SkillId == "mobile:danggu")
                Answer(game, _ => true);
            else if (prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"))
                Skip(game);
            else if (prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "let-die"))
                Answer(game, choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
            else
                throw new InvalidOperationException($"Unexpected death interaction decision {prompt.Kind}: {prompt.Prompt}");
        }
        var relevant = game.Events.Select(entry => entry.Payload).Where(fact =>
            fact is FireAttackResolvedEvent or ChainedDamagePropagatedEvent or DamageAppliedEvent or PlayerDiedEvent or CardUseFinishedEvent);
        throw new InvalidOperationException($"Death interaction did not settle at the next boundary: {P(game)?.Prompt}. " +
            $"Frames: {string.Join(", ", game.ResolutionStack.Select(frame => frame.GetType().Name))}. " +
            $"Facts: {string.Join(", ", relevant)}.");
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))) &&
            Facts<DamageAppliedEvent>(restored).SequenceEqual(Facts<DamageAppliedEvent>(game)) &&
            Facts<PlayerDiedEvent>(restored).SequenceEqual(Facts<PlayerDiedEvent>(game)) &&
            Facts<XianfuEchoIssuedEvent>(restored).SequenceEqual(Facts<XianfuEchoIssuedEvent>(game)),
            "Cold public-command replay preserves propagation, death attribution, Xianfu, movement and each seat's view.");
        return restored;
    }

    private sealed class Fixture(string sourceGeneral, int attackerHp) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-death-interaction", new(1, 0, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var requiredGenerals = new List<string> { "ol:xi-zhi-cai", "classic:guo-jia", "classic:cai-wen-ji" };
            if (sourceGeneral == RestingSource)
                requiredGenerals.Add(RestingSource);
            CharacterFixtureContent.Register(builder, requiredGenerals.ToArray());
            var catalog = SkillProgramCatalog.LoadCurrent($$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{
                  "id":"chain","minCards":0,"maxCards":0,"minTargets":2,"maxTargets":2,"targetKind":"otherLiving","usesPerTurn":1,
                  "effects":[{"op":"setChainedState","target":"selectedTargets","chained":true}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "连环测试", description = "连环两名角色。" } } }));
            builder.AddSkill(new(Driver, "连环测试", "连环两名角色。")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Observer, "先辅观察者", "supporter", Xianfu, "wei", 3, [Driver, "ol:chouce"]));
            builder.AddGeneral(new(OrdinarySource, "火攻来源", "supporter", Driver, "qun", attackerHp));
            builder.AddGeneral(new(Target, "断肠目标", "supporter", Duanchang, "qun", 1));
            builder.AddGeneral(new(Survivor, "旁观角色", "supporter", "standard:none", "wei", 4));
            var fire = CardCatalog.Get(CardKind.FireAttack);
            builder.AddCard(new(Fire, fire.DisplayName, fire.CategoryName, fire.Description, CardKind.FireAttack));
            builder.AddDeck(new(Deck, "固定红桃火攻", 2, 2, []) { PhysicalCards = Enumerable.Range(0, 60)
                .Select(_ => new ContentDeckPhysicalCard(Fire, Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "连环断肠", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Observer, sourceGeneral, Target, Survivor]));
        }
    }
}
