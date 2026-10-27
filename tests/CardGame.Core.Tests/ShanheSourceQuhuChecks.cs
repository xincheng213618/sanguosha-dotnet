using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceQuhuChecks
{
    private const string Deck = "fixture:shanhe-source-quhu-deck";
    private const string Opponent = "fixture:shanhe-source-quhu-opponent";
    private const string Victim = "fixture:shanhe-source-quhu-victim";
    private const string EqualHp = "fixture:shanhe-source-quhu-equal-hp";
    private const string Skill = StandardShanheSourceXunYuPackage.QuhuSkillId;

    public static void WinningOpponentDealsItsCappedMaximumHpAndNonWinsRetainOriginalDamageSource()
    {
        foreach (var (rank, maximum) in new[] { (13, 8), (13, 4), (1, 8), (7, 8) })
        {
            var scene = Scene.Start(rank, maximum);
            var ownerMaterial = scene.Player(0).Hand[0].Id;
            var opponentMaterials = scene.Player(1).Hand.Select(card => card.Id).ToArray();
            var legal = scene.Game.GetHumanLegalActions().Single(action => action.ProgramSkillId == Skill);
            Need(legal.SelectableTargetSeats!.Contains(1) && !legal.SelectableTargetSeats.Contains(3) &&
                scene.Player(0) is { Hp: 3, MaxHp: 3 } && scene.Player(1).MaxHp == maximum && scene.Player(1).Hp == 4 &&
                scene.Player(0).Skills?.Any(skill => skill.Id == StandardShanheSourceXunYuPackage.JiemingSkillId) == true,
                "The production 3/3 Xun Yu has the reused current Jieming and can contest only a higher-current-HP hand owner.");
            scene.Submit(new UseProgramSkillCommand(0, Skill, "contest", [ownerMaterial], [1],
                scene.Game.Revision, scene.Prompt!.PromptId));
            if (rank > 7)
            {
                scene.Reach(() => scene.Prompt?.SkillPrompt?.SkillId == Skill &&
                    scene.Game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Skill &&
                        frame.PindianResultBindings.Any(result => result.SourceWon)));
                var choice = scene.Prompt!;
                Need(choice.PlayerSeat == 0 && choice.Choices.Any(option => option.Targets.SequenceEqual([2])) &&
                    choice.Choices.All(option => !option.Targets.Contains(1) && !option.Targets.Contains(3)) &&
                    scene.Events<DamageAppliedEvent>().Length == 0,
                    "After the real win, the Quhu owner chooses within the original opponent's range; the opponent and distance-two seat are absent, and damage has not happened yet.");
                scene.Answer(option => option.Targets.SequenceEqual([2]));
            }
            scene.Reach(() => scene.Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                scene.Game.ResolutionStack.Count == 0);
            var contest = scene.Events<PindianResultDeterminedEvent>().Single(fact => fact.SkillId == Skill);
            var damage = scene.Events<DamageAppliedEvent>().Single();
            var won = rank > 7;
            Need(contest.Result is { SourceSeat: 0, OpponentSeat: 1, OpponentRank: 7 } &&
                contest.Result.SourceRank == rank && contest.Result.SourceWon == won &&
                damage.SourceSeat == 1 && damage.TargetSeat == (won ? 2 : 0) &&
                damage.Amount == (won ? Math.Min(5, maximum) : 1) &&
                scene.Player(0).Hp == (won ? 3 : 2) && scene.Player(2).Hp == 12 - (won ? Math.Min(5, maximum) : 0),
                "Native win damage uses the original opponent's own maximum HP capped at five; loss and tie each make that opponent damage the owner once.");
            Need(contest.Result.SourceCardId == ownerMaterial && opponentMaterials.Contains(contest.Result.OpponentCardId) &&
                scene.Game.CardMovements.Count(move => move.CardId == ownerMaterial && move.From == CardLocation.Hand(0) &&
                    move.To == CardLocation.Processing) == 1 &&
                scene.Game.CardMovements.Count(move => move.CardId == contest.Result.OpponentCardId && move.From == CardLocation.Hand(1) &&
                    move.To == CardLocation.Processing) == 1 &&
                scene.Events<ProgramSkillResolvedEvent>().Count(fact => fact.SkillId == Skill && fact.Completed) == 1 &&
                scene.Player(0).HandCount == 1 && scene.Player(1).HandCount == 1 &&
                !scene.Game.GetHumanLegalActions().Any(action => action.ProgramSkillId == Skill) &&
                scene.Game.State.Winner == Winner.None,
                "Both real Pindian materials are paid once and the activation completes once; remaining real cards cannot pay a second contest in this same Play.");
        }

        var emptyRange = Scene.Start(13, 8, noRangeTargets: true);
        var material = emptyRange.Player(0).Hand[0].Id;
        Need(emptyRange.Player(0).Equipment.Any(card => card.Kind == CardKind.DefensiveHorse) &&
            emptyRange.Player(2).Equipment.Any(card => card.Kind == CardKind.DefensiveHorse),
            "Both neighbours have real installed defensive horses; the remaining character is already distance two from the opponent.");
        emptyRange.Submit(new UseProgramSkillCommand(0, Skill, "contest", [material], [1],
            emptyRange.Game.Revision, emptyRange.Prompt!.PromptId));
        emptyRange.Reach(() => emptyRange.Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            emptyRange.Game.ResolutionStack.Count == 0);
        Need(emptyRange.Events<PindianResultDeterminedEvent>().Single().Result.SourceWon &&
            emptyRange.Events<DamageAppliedEvent>().Length == 0 && emptyRange.Player(0).Hp == 3 &&
            emptyRange.Player(2).Hp == 12 && emptyRange.Player(0).HandCount == 1 && emptyRange.Player(1).HandCount == 1 &&
            emptyRange.Events<ProgramSkillResolvedEvent>().Count(fact => fact.SkillId == Skill && fact.Completed) == 1 &&
            emptyRange.Game.CardMovements.Count(move => move.CardId == material && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing) == 1,
            "A real winning contest with no opponent-range target returns to the original Play without damage, another material payment or an abandoned program frame.");
    }

    private sealed class Scene(GameEngine game)
    {
        public GameEngine Game { get; } = game;
        public PendingDecision? Prompt => Game.CreateSnapshot(0).PendingDecision;
        public PlayerSnapshot Player(int seat) => Game.CreateSnapshot(seat).Players.Single(player => player.Seat == seat);
        public T[] Events<T>() => Game.Events.Select(envelope => envelope.Payload).OfType<T>().ToArray();
        public void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Source Quhu native command rejected."); }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var prompt = Prompt!;
            var choice = prompt.Choices.Single(predicate);
            Submit(new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, choice.Id, Game.Revision));
        }
        public void Reach(Func<bool> ready)
        {
            for (var step = 0; step < 240; step++)
            {
                if (ready()) return;
                Need(Game.State.Winner == Winner.None, "Source Quhu must remain a bounded real Play flow.");
                if (Prompt is { } prompt)
                {
                    if (prompt.Kind == DecisionKind.PlayCard && Game.ResolutionStack.Count != 0)
                    { Submit(new AdvanceOneStepCommand(Game.Revision)); continue; }
                    if (prompt.SkillPrompt?.SkillId == StandardShanheSourceXunYuPackage.JiemingSkillId)
                    { Answer(choice => choice.Parameters.GetValueOrDefault("program-action") == "skip"); continue; }
                    throw new InvalidOperationException("Unexpected Quhu boundary: " + JsonSerializer.Serialize(new
                    { prompt.Kind, prompt.PlayerSeat, Skill = prompt.SkillPrompt?.SkillId, Frames = Game.ResolutionStack.Select(frame => frame.Kind) }));
                }
                Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("Source Quhu did not reach its bounded native decision.");
        }
        public static Scene Start(int rank, int maximum, bool noRangeTargets = false)
        {
            var setup = new ShanheBattleSetup(31, 1, "fixture-source-quhu", StandardShanheSourceXunYuPackage.GeneralId,
                3, 0, 0, 0, 0, [], [new(CardKind.Dodge, Suit.Club, rank), new(CardKind.Dodge, Suit.Club, rank)],
                noRangeTargets ? [new(CardKind.DefensiveHorse, Suit.Spade, 5)] : [],
                [new(Opponent, maximum - 4, 4) { StartingHand = [new(CardKind.Dodge, Suit.Club, 7), new(CardKind.Dodge, Suit.Club, 7)] },
                 new(Victim) { StartingHand = [], StartingEquipment = noRangeTargets ? [new(CardKind.DefensiveHorse, Suit.Spade, 5)] : [] },
                 new(EqualHp) { StartingHand = [new(CardKind.Dodge, Suit.Club, 7)] }], [], "fixture:source-quhu-native");
            // Ask the production factory for the actual source body and skill dependencies.
            // Only the three neutral peers are supplied by the independent fixture below.
            var production = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
            { EnemyRoster = Enumerable.Range(0, setup.EnemyRoster.Count).Select(_ => new ShanheCombatantDefinition("classic:guan-yu")).ToArray() });
            var registry = production.WithExtension(new Fixture(), mode => mode.Id == ShanheBattleSetup.BattleModeId
                ? mode with
                {
                    DeckId = Deck,
                    GeneralPoolIds = (mode.GeneralPoolIds ?? []).Concat(new[] { Opponent, Victim, EqualHp }).Distinct().ToArray()
                } : mode);
            var scene = new Scene(GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 2
            }, registry));
            scene.Submit(new StartGameCommand());
            scene.Reach(() => scene.Prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && scene.Game.ResolutionStack.Count == 0);
            return scene;
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-quhu-native", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new(Opponent, "拼点对手", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Victim, "驱虎目标", "supporter", "standard:none", "wei", 12));
            builder.AddGeneral(new(EqualHp, "同体力旁观者", "supporter", "standard:none", "wei", 3));
            builder.AddDeck(new(Deck, "固定拼点材料", 0, 0, [new("standard:dodge", 48)]));
        }
    }

    private static void Need(bool yes, string reason)
    { if (!yes) throw new InvalidOperationException(reason); }
}
