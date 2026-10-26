using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheRemainingSchoolStockChecks
{
    public static void ProductionFiveMemberRecipesDispatchThroughTheNativeBattleFactory()
    {
        var source = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        foreach (var (schoolId, sources) in new[]
                 {
                     (6, new[] { 2201, 2202, 2200, 2204, 2199 }),
                     (8, new[] { 2374, 2380, 2072, 2373, 2073 })
                 })
        {
            var school = source.Schools!.Single(s => s.Id == schoolId);
            Need(school.Requirements.SequenceEqual([2, 2, 1, 0]), "The two remaining production schools retain the published five-member recipe.");
            var components = sources.Select(id => source.Rewards.SingleOrDefault(r => r.SchoolMembership?.SourceId == id)
                ?? throw new InvalidOperationException($"The real production school {schoolId} is missing component source {id}.")).ToArray();
            var counts = Enumerable.Range(1, 4).Select(tier => components.Count(r => r.SchoolMembership!.Rarity == tier)).ToArray();
            Need(components.All(r => r.SchoolMembership!.SchoolId == schoolId && r.TacticSkillId is not null) &&
                 counts.SequenceEqual(school.Requirements) && components.Select(r => r.SchoolMembership!.SourceId).Distinct().Count() == 5,
                "Every selected member is a concrete native tactic of the correct source school and exact rarity bucket: " + JsonSerializer.Serialize(components));
            var definition = source with
            {
                Id = "fixture:remaining-school-production-" + schoolId, StartingNodeIds = ["battle"],
                Nodes = [new("battle", "五件组合原生试阵", ShanheNodeKind.Battle, [],
                    [new("guard", "同行与守军", [new(StandardShanheLocalTroopPackage.FootmanId) { StartingHand = [] }],
                        [new("classic:guan-yu") { StartingHand = [] }])])]
            };
            var session = ShanheCampaignSession.Create(definition, new(71, "classic:cao-cao")
            {
                UseCodexRewardPool = true, EquippedTacticIds = components.Select(r => r.Id).ToArray(),
                UnlockedRewardIds = source.Rewards.Select(r => r.Id).ToArray()
            }, StandardShanheCampaignCatalog.CreateBattleRegistry);
            Need(session.GetSchoolProgress().Single(p => p.School.Id == schoolId) is { IsComplete: true } progress &&
                 progress.OwnedCounts.SequenceEqual(school.Requirements),
                "Actual production acquisition identities must complete the five-member school, rather than duplicate an aliased component.");
            var entered = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
            Need(entered.Accepted, entered.Error ?? "The complete production five-member school did not enter battle.");
            var setup = session.PendingBattleSetup!;
            var effect = ShanheSchoolEffects.SkillId(school);
            Need(effect is not null && setup.ActiveSchoolIds.Contains(schoolId) && setup.TacticSkillIds.Count(id => id == effect) == 1,
                "The real campaign entry emits one derived effect without a separately purchased set reward.");
            var game = StandardShanheCampaignCatalog.CreateBattle(setup);
            Accept(game, new StartGameCommand());
            for (var step = 0; step < 180 && game.PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
            {
                if (game.PendingDecision is { PlayerSeat: 0 } prompt)
                    throw new InvalidOperationException("Unexpected human opening choice in the fixed production recipe: " + JsonSerializer.Serialize(prompt));
                Accept(game, new AdvanceOneStepCommand(game.Revision));
            }
            Need(game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                 game.CreateSnapshot(0).Players[0].Skills!.Count(skill => skill.ContentId == effect) == 1 &&
                 session.Snapshot.ExtraSkillIds.Count == 0,
                "The production content factory registers and starts the actually earned native school, without consuming a general-skill slot.");
        }
    }

    private static void Accept(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "The production school command was rejected."); }
    private static void Need(bool valid, string message)
    { if (!valid) throw new InvalidOperationException(message); }
}
