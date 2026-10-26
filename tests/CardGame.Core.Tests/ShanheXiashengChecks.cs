using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheXiashengChecks
{
    private const string Owner = "fixture:xiasheng-owner", Other = "fixture:xiasheng-other", Far = "fixture:xiasheng-far", Near = "fixture:xiasheng-near";
    private const string Skill = StandardShanheXiashengPackage.SkillId;

    public static void PaidHandColorsDrawAndLegalExtraTargetsRecoverCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardShanheXiashengPackage(), new Fixture());
        var setup = new ShanheBattleSetup(71, 1, "xiasheng", Owner, 6, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Heart, 7), new(CardKind.Duel, Suit.Spade, 8),
                new(CardKind.Dismantlement, Suit.Club, 9), new(CardKind.Alcohol, Suit.Heart, 3)], [],
            [new(Far) { StartingHand = [] }, new(Near) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 2), new(CardKind.Dodge, Suit.Club, 2)] }],
            [new(Other) { StartingHand = [] }], new string('a', 64));
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, registry);
        Submit(new StartGameCommand()); Play();
        Use(CardKind.Slash);
        Reach(() => game.PendingDecision?.SkillPrompt?.SkillId == Skill);
        var prompt = game.PendingDecision!;
        Need(prompt.Choices.Any(choice => choice.Targets.SequenceEqual([1])) &&
            prompt.Choices.All(choice => !choice.Targets.Contains(2)) &&
            Facts<ShanheXiashengBranchIssuedEvent>() is [{ Branch: "target", RedCount: 1, BlackCount: 2 }],
            "The red Slash is already paid when hand colors are counted; only the nearby extra target is offered, not the distant seat.");
        Cold(); Answer(choice => choice.Targets.SequenceEqual([1])); Play();
        Need(Facts<ProgramCardUseTargetAddedEvent>().Any(item => item.SkillId == Skill && item.TargetSeat == 1) &&
            Facts<DamageAppliedEvent>().Any(item => item.SourceSeat == 0 && item.TargetSeat == 1),
            "The chosen extra target receives the real native Slash continuation after cold restoration.");
        Use(CardKind.Duel); Play();
        Need(Facts<ShanheXiashengBranchIssuedEvent>().Length == 1,
            "A black use leaving equal red and black hand counts grants no draw and no extra target.");
        Use(CardKind.Dismantlement); Play(); Cold();
        Need(Facts<ShanheXiashengBranchIssuedEvent>().Count(item => item.Branch == "draw" && item.RedCount == 1 && item.BlackCount == 0) == 1 &&
            game.CardMovements.Count(move => move.To == CardLocation.Hand(0) && move.Reason.Value == "skill-program.shanhe-xiasheng.draw") == 1,
            "Using a black card while the paid hand has more red cards produces exactly one actual native draw, including replay.");
        return;

        T[] Facts<T>() => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
        void Submit(GameCommand command)
        { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Xiasheng fixture command rejected."); }
        void Use(CardKind kind)
        {
            var card = game.CreateSnapshot(0).Players[0].Hand.Single(item => item.Kind == kind);
            Submit(new PlayCardCommand(0, card.Id, [3], game.Revision, game.PendingDecision!.PromptId));
        }
        void Answer(Func<PromptChoice, bool> select)
        { var pending = game.PendingDecision!; Submit(new AnswerPromptCommand(pending.PlayerSeat, pending.PromptId, pending.Choices.First(select).Id, game.Revision)); }
        void Play() => Reach(() => game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        void Reach(Func<bool> condition)
        {
            for (var step = 0; step < 150; step++)
            {
                if (condition()) return;
                if (game.PendingDecision is { PlayerSeat: 0 } pending)
                {
                    if (pending.Kind == DecisionKind.PlayCard) throw new InvalidOperationException("Xiasheng returned before its expected use boundary.");
                    var choice = pending.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") is "take-damage" or "pass") ?? pending.Choices[0];
                    Submit(new AnswerPromptCommand(0, pending.PromptId, choice.Id, game.Revision));
                }
                else Submit(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("Xiasheng did not complete its native use.");
        }
        void Cold()
        {
            var serialized = GameCheckpointJson.Serialize(game.CreateCheckpoint());
            var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(serialized), registry);
            Need(GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == serialized &&
                SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
                "Cold replay preserves the original color branch, extra-target choice and single draw payment.");
            game = restored;
        }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-xiasheng", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new(Owner, "夏晟行者", "", Skill, "qun", 6));
            foreach (var id in new[] { Other, Far, Near }) builder.AddGeneral(new(id, "原生角色", "", null, "wei", 6));
            builder.AddDeck(new("fixture:xiasheng-deck", "实体摸牌", 0, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "夏晟机制", 4, 4, new Dictionary<string, int>(), "fixture:xiasheng-deck",
                GeneralPoolIds: [Owner, Other, Far, Near], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
