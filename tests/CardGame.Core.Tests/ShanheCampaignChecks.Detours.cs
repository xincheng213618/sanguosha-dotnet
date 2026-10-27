using System.Text.Json;
using CardGame.Core.Campaigns;

internal static partial class ShanheCampaignChecks
{
    public static void BothOpeningBranchesGrantRewardsAndRejoinProgressCold()
    {
        var definition = new ShanheCampaignDefinition("fixture:chapter-detours", "回头补战", [Player], ["left", "right"],
        [
            Opening("left", "tactic-left"), Opening("right", "tactic-right"),
            new("market", "集市", ShanheNodeKind.Shop, ["road"], Choices:
                [new("buy", "战法", 100, RewardId: "tactic-left", LeavesNode: false)]) { ChapterNumber = 1 },
            new("road", "汇合路口", ShanheNodeKind.Camp, ["gate", "cache"], Choices: [new("continue", "继续")]) { ChapterNumber = 1 },
            new("cache", "可选支路", ShanheNodeKind.Camp, ["gate"], Choices: [new("continue", "继续")]) { ChapterNumber = 1 },
            new("gate", "出关", ShanheNodeKind.Camp, ["next"], Choices: [new("continue", "继续")]) { ChapterNumber = 1 },
            new("next", "第二章", ShanheNodeKind.Camp, [], Choices: [new("continue", "继续")]) { ChapterNumber = 2 }
        ], [new("tactic-left", "左路战法") { TacticSkillId = Guard }, new("tactic-right", "右路战法") { TacticSkillId = Raid }])
        { AllowChapterDetours = true };
        foreach (var first in new[] { "left", "right" })
        foreach (var persistent in new[] { false, true })
        {
            var session = ShanheCampaignSession.Create(definition, new(71, Player)
                { PersistentMarket = persistent, UseCodexRewardPool = persistent }, Registry);
            var second = first == "left" ? "right" : "left";
            var preview = JsonSerializer.Serialize(session.PreviewEncounter(second));
            WinAndClaim(first);
            Require(session.Snapshot.AvailableNodes.Any(node => node.Id == second) &&
                JsonSerializer.Serialize(session.PreviewEncounter(second)) == preview,
                "The other opening battle remains available and retains the encounter previewed before the first victory.");
            if (!persistent) Leave("market");
            Leave("road");
            session = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())), definition, Registry);
            Require(session.Snapshot.AvailableNodes.Select(node => node.Id).ToHashSet().SetEquals([second, "gate", "cache"]),
                "Passing the shared road and restoring progress keeps the unplayed opening branch and the unlocked forward route together.");
            WinAndClaim(second);
            Require(session.Snapshot.Gold == 400 && session.Snapshot.TacticIds.Count == 2 &&
                session.Snapshot.CompletedNodeIds.Count(id => id == first || id == second) == 2 &&
                session.Snapshot.AvailableNodes.Select(node => node.Id).ToHashSet().SetEquals(["gate", "cache"]),
                "Each opening battle grants its own copper and tactic once; returning through a completed junction preserves forward progress.");
            var before = Snapshot(session);
            Require(!session.Submit(new MoveShanheNodeCommand(first, session.Revision)).Accepted &&
                !session.Submit(new MoveShanheNodeCommand(second, session.Revision)).Accepted && Snapshot(session) == before,
                "Completed locations cannot be replayed to farm rewards.");
            Cold(session, definition);
            Leave("gate");
            Require(session.Snapshot.AvailableNodes.Select(node => node.Id).SequenceEqual(["next"]) &&
                !session.Submit(new MoveShanheNodeCommand("cache", session.Revision)).Accepted,
                "Entering the next chapter closes unfinished old-chapter detours.");
            Cold(session, definition);

            void Leave(string id)
            {
                Accept(session, new MoveShanheNodeCommand(id, session.Revision));
                Accept(session, new ChooseShanheNodeOptionCommand(id == "market" ? "$leave" : "continue", null, session.Revision));
            }
            void WinAndClaim(string id)
            {
                Accept(session, new MoveShanheNodeCommand(id, session.Revision));
                var battle = CreateBattle(session.PendingBattleSetup!);
                FinishBattle(battle, "defeat");
                Require(session.CompleteBattle(battle, session.Revision).Accepted, "A detour reward requires a validated native victory.");
                Accept(session, new ChooseShanheRewardCommand("tactic-" + id, null, session.Revision));
            }
        }

        static ShanheNodeDefinition Opening(string id, string reward) => new(id, id, ShanheNodeKind.Battle, ["market"],
            Enumerable.Range(1, 4).Select(index => new ShanheEncounterDefinition(id + index, "驻军" + index, [new(Enemy)], VictoryGold: 200)).ToArray(), [reward])
            { ChapterNumber = 1 };
    }
}
