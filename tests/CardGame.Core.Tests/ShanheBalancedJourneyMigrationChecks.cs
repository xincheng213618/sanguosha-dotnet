using System.Text.Json;
using CardGame.Core.Campaigns;

internal static class ShanheBalancedJourneyMigrationChecks
{
    public static void FutureRewardsUpgradeAtSafeBoundaryAndReplayOldChoices()
    {
        var definition = Definition();
        var legacy = Create(definition);
        var originalOptions = ShanheCampaignJson.Serialize(legacy.CreateCheckpoint());
        Need(!legacy.HasBalancedJourneyRewards && legacy.PreviewBattleNode("battle").PrimaryTacticRarity == 1 &&
                !originalOptions.Contains("UseBalancedJourneyRewards", StringComparison.Ordinal),
            "Old absent/false options preserve the authored common tactic profile and serialized options.");

        var migrated = Cold(legacy, definition);
        RejectUnchanged(migrated, new EnableShanheBalancedRewardsCommand(migrated.Revision - 1));
        Accept(migrated, new EnableShanheBalancedRewardsCommand(migrated.Revision));
        Need(migrated.HasBalancedJourneyRewards && !migrated.CreateCheckpoint().Options.UseBalancedJourneyRewards &&
                migrated.AcceptedCommands is [EnableShanheBalancedRewardsCommand] &&
                migrated.PreviewBattleNode("battle").PrimaryTacticRarity == 2,
            "Migration appends one replayable command without changing historical options, and previews actual second-tier tactics.");
        RejectUnchanged(migrated, new EnableShanheBalancedRewardsCommand(migrated.Revision));
        migrated = Cold(migrated, definition);
        CompleteBattle(migrated);
        Need(migrated.Snapshot.RewardOffers is [{ Id: "rare", Rarity: 2, Bonuses.MaxHp: 2 }],
            "A real completed ordinary battle awards the actual rare reward body, rather than recoloring the common initial-hand bonus.");
        migrated = Cold(migrated, definition);
        Accept(migrated, new ChooseShanheRewardCommand("rare", null, migrated.Revision));
        Need(migrated.Snapshot.Bonuses.MaxHp == 2 && migrated.Snapshot.Bonuses.InitialHand == 0 &&
                migrated.Snapshot.CompletedNodeIds.SequenceEqual(["battle"]),
            "Claiming the second-tier reward applies its real effect exactly once and advances only the completed battle.");
        Cold(migrated, definition);

        Accept(legacy, new MoveShanheNodeCommand("battle", legacy.Revision));
        var frozenBattle = legacy.PendingBattleSetup!;
        RejectUnchanged(legacy, new EnableShanheBalancedRewardsCommand(legacy.Revision));
        Need(ReferenceEquals(legacy.PendingBattleSetup, frozenBattle),
            "A policy update cannot rewrite an already entered battle setup.");
        FinishEnteredBattle(legacy);
        Need(legacy.Snapshot.RewardOffers is [{ Id: "white", Rarity: 1 }],
            "Legacy victory replay retains its original white reward until a safe future-reward boundary.");
        RejectUnchanged(legacy, new EnableShanheBalancedRewardsCommand(legacy.Revision));
        legacy = Cold(legacy, definition);
        Accept(legacy, new ChooseShanheRewardCommand("white", null, legacy.Revision));
        var historical = legacy.AcceptedCommands.ToArray();
        Accept(legacy, new EnableShanheBalancedRewardsCommand(legacy.Revision));
        Need(legacy.AcceptedCommands.Take(historical.Length).SequenceEqual(historical) &&
                legacy.Snapshot.Bonuses.InitialHand == 1 && legacy.Snapshot.Bonuses.MaxHp == 0,
            "The migration preserves all historical commands and the previously claimed common effect.");
        Cold(legacy, definition);

        var fresh = Create(definition, balanced: true);
        Need(fresh.HasBalancedJourneyRewards && fresh.Revision == 0 && fresh.PreviewBattleNode("battle").PrimaryTacticRarity == 2,
            "New opted-in expeditions use second-tier rewards without an extra migration command.");
        var choiceDefinition = definition with
        {
            StartingNodeIds = ["camp"],
            Nodes = definition.Nodes.Select(node => node.Id == "camp" ? node with { NextNodeIds = ["battle"] } :
                node with { NextNodeIds = [] }).ToArray()
        };
        var atChoice = Create(choiceDefinition);
        Accept(atChoice, new MoveShanheNodeCommand("camp", atChoice.Revision));
        var choices = JsonSerializer.Serialize(atChoice.Snapshot.NodeChoices);
        Accept(atChoice, new EnableShanheBalancedRewardsCommand(atChoice.Revision));
        Need(atChoice.Snapshot.Phase == ShanheCampaignPhase.NodeChoice &&
                JsonSerializer.Serialize(atChoice.Snapshot.NodeChoices) == choices,
            "A pending camp choice is a safe upgrade boundary and retains its already published actions.");
        Cold(atChoice, choiceDefinition);
    }

    private static ShanheCampaignDefinition Definition() => new("fixture:balanced-migration", "战法品质迁移",
        [ShanheCampaignChecks.Player], ["battle"],
        [new("battle", "普通战场", ShanheNodeKind.Battle, ["camp"],
            [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)])], ["white", "rare", "epic"])
            { PrimaryTacticRarity = 1, CategorizedRewards = true, AutomaticRewardCategory = true },
         new("camp", "营地", ShanheNodeKind.Camp, [], Choices: [new("rest", "休整")])],
        [new("white", "初手一张", Bonuses: new(InitialHand: 1)) { Rarity = 1 },
         new("rare", "体力上限二", Bonuses: new(MaxHp: 2)) { Rarity = 2 },
         new("epic", "每回合摸牌一", Bonuses: new(Draw: 1)) { Rarity = 3 }]);

    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, bool balanced = false) =>
        ShanheCampaignSession.Create(definition, new(71, ShanheCampaignChecks.Player)
            { UseBalancedJourneyRewards = balanced }, ShanheCampaignChecks.Registry);

    private static void CompleteBattle(ShanheCampaignSession session)
    {
        Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
        FinishEnteredBattle(session);
    }

    private static void FinishEnteredBattle(ShanheCampaignSession session)
    {
        var battle = ShanheCampaignChecks.CreateBattle(session.PendingBattleSetup!);
        ShanheCampaignChecks.FinishBattle(battle, "defeat");
        var result = session.CompleteBattle(battle, session.Revision);
        Need(result.Accepted, result.Error ?? "The independently replayed native victory must settle.");
    }

    private static ShanheCampaignSession Cold(ShanheCampaignSession session, ShanheCampaignDefinition definition)
    {
        var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(
            ShanheCampaignJson.Serialize(session.CreateCheckpoint())), definition, ShanheCampaignChecks.Registry);
        Need(JsonSerializer.Serialize(restored.Snapshot) == JsonSerializer.Serialize(session.Snapshot) &&
                restored.HasBalancedJourneyRewards == session.HasBalancedJourneyRewards,
            "Cold replay preserves the complete published state and the exact migration boundary.");
        return restored;
    }

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "The migration command must be accepted."); }

    private static void RejectUnchanged(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var before = session.Snapshot;
        Need(!session.Submit(command).Accepted && ReferenceEquals(before, session.Snapshot),
            "A stale or unsafe policy command must preserve the frozen state and journal revision.");
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
