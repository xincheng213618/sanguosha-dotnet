using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class ShenwuNpcChecks
{

    public static void ExpandedSipuAndRealTierInitialization()
    {
        {
            var (g, content) = Start(["shenwu:sipu"], [], "standard:nullification", expanded: true);
            void Dismantle()
            {
                var action = g.GetHumanLegalActions().First(a => a.ConversionSource is { SkillId: Driver, BindingId: "fixed-dismantlement" } && a.TargetSeats.SequenceEqual([1]));
                Accept(g, new PlayCardCommand(0, action.CardId!.Value, [1], g.Revision, P(g)!.PromptId, CardKind.Dismantlement) { ConversionSource = action.ConversionSource });
            }
            for (var use = 0; use < 2; use++)
            {
                Dismantle();
                Until(g, () => P(g) is { Kind: DecisionKind.SelectTargetCard });
                Answer(g, _ => true); Play(g);
            }
            Require(!E<NullificationRespondedEvent>(g).Any(), "Other players cannot play Nullification against Sipu's first two actual Play uses.");
            Dismantle();
            Until(g, () => P(g) is { Kind: DecisionKind.Nullification, PlayerSeat: not 0 });
            Require(P(g)!.Choices.Any(choice => choice.Cards.Count > 0), "The third actual use reopens real card responses.");
            Cold(g, content);
        }
        var registry = CardGame.Content.Standard.ComposedSkillContentRegistry.CreateShowcase(includeShenwuActivities: true);
        foreach (var tier in new[] { 1, 5 })
        {
            var g = GameEngine.CreateStandard(new GameOptions { Seed = 17, PlayerCount = 6, HumanSeat = 0,
                HumanRole = null, HumanTeamId = "team:allies", ModeId = $"activity:shenwu-2020-tier-{tier}",
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, registry);
            Accept(g, new StartGameCommand());
            for (var step = 0; step < 80 && !g.CreateSnapshot(0).Players.All(player => player.HandCount > 0); step++)
                Accept(g, new AdvanceOneStepCommand(g.Revision));
            var view = g.CreateSnapshot(0);
            var leader = view.Players[4];
            Require(view.Players.Count == 6 && leader.MaxHp == registry.Generals[leader.GeneralId].BaseHp + (tier == 5 ? 2 : 0) &&
                view.Players[3].MaxHp == 4 + (tier == 5 ? 2 : 0) &&
                view.Players.Where(player => player.TeamId == "team:beasts").All(player =>
                    player.Skills!.Any(skill => skill.ContentId == "shenwu:kunshou") == (tier == 5)),
                "Real selected tiers initialize leader and independent minion health and native Kunshou grants from the 2020 table.");
            var cold = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
            Require(SnapshotJson.Serialize(g.CreateSnapshot(0)) == SnapshotJson.Serialize(cold.CreateSnapshot(0)), "The real version and enemy tier survive cold replay.");
        }
    }
}
