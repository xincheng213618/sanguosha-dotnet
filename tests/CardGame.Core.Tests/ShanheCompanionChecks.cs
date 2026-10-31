using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheCompanionChecks
{
    private const string Player = "classic:guan-yu";
    private const string Companion = StandardShanheCompanionPackage.ManzhuId;

    public static void EquippedOpeningCompanionNativeRosterPreviewAndColdReplay()
    {
        var catalogue = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        Need(StandardShanheCampaignCatalog.CodexRewardIdsByNodeId[1] == StandardShanheCampaignCatalog.OpeningCompanionRewardId &&
            catalogue.Rewards.Single(reward => reward.Id == StandardShanheCampaignCatalog.OpeningCompanionRewardId)
                .OpeningCompanionPool!.Select(member => member.GeneralId).ToHashSet(StringComparer.Ordinal)
                .SetEquals([Companion, StandardShanheChilianCompanionPackage.ChilianId, StandardShanheBossPackage.FenghuangId,
                    StandardShanheExpandedCompanionPackage.AlingId]),
            "The ordinary Heguangmeng source node binds all four evidenced ordinary companions.");
        var definition = Definition();
        var bare = Create(definition, equipped: false);
        Need(bare.Snapshot.StartingCompanions.Count == 0 && bare.PreviewEncounter("battle").Allies!.Count == 0 &&
            bare.Snapshot.TacticIds.Count == 0,
            "An expedition that did not equip the opening-companion tactic receives no automatic companion.");
        var session = Create(definition);
        Need(session.Snapshot.StartingCompanions is [{ GeneralId: Companion }] && session.Revision == 0 &&
            session.Snapshot.ExtraSkillIds.Count == 0 && session.Snapshot.TacticIds.SequenceEqual([StandardShanheCampaignCatalog.OpeningCompanionRewardId]),
            "Only the equipped tactic grants one fixed companion without a command, skill slot or forced skill draft.");
        var before = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
        var preview = session.PreviewEncounter("battle");
        Need(preview.Allies is [{ GeneralId: Companion }] && JsonSerializer.Serialize(preview) == JsonSerializer.Serialize(session.PreviewEncounter("battle")) &&
            ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before && session.PendingBattleSetup is null,
            "Repeated previews include the chosen companion without rerolling, starting a battle or advancing the journal.");
        Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
        Need(JsonSerializer.Serialize(session.PendingBattleSetup!.AllyRoster) == JsonSerializer.Serialize(preview.Allies),
            "The actual challenge uses precisely the companion roster shown by its preview.");
        var battle = StandardShanheCampaignCatalog.CreateBattle(session.PendingBattleSetup!);
        Need(battle.Submit(new StartGameCommand()).Accepted, "The companion starts a real native battle.");
        var member = battle.CreateSnapshot(0).Players.Single(player => player.GeneralId == Companion);
        Need(member.MaxHp == 3 && member.Hp == 3 && member.Skills!.Select(skill => skill.Id).ToHashSet(StringComparer.Ordinal)
            .SetEquals(["boundary:zhaxiang", "ol:zengou", "classic:longyin"]),
            "Manzhu is the screenshot's three-HP Qun body with the native Zhaxiang, Zengou and Longyin versions.");
        var cold = Cold(session, definition);
        Need(cold.Snapshot.StartingCompanions.Single().GeneralId == Companion &&
            ShanheBattleSetup.ValueEquals(cold.PendingBattleSetup, session.PendingBattleSetup),
            "Cold campaign replay restores the same initially chosen companion and exact native battle recipe.");

        var duplicateDefinition = Definition(allies: [new(Companion)]);
        var duplicate = Create(duplicateDefinition);
        Need(duplicate.PreviewEncounter("battle").Allies is [{ GeneralId: Companion }],
            "An identical authored companion is not duplicated when merging the opening roster.");
        Accept(duplicate, new MoveShanheNodeCommand("battle", duplicate.Revision));
        Need(duplicate.PendingBattleSetup!.AllyRoster.Count == 1, "Actual play preserves the same deduplicated companion roster.");

        var fullDefinition = Definition(enemies: new[]
        {
            "classic:zhang-liao", "classic:cao-cao", "classic:sun-quan", "classic:zhou-yu",
            "classic:ma-chao", "classic:zhao-yun", "classic:huang-yueying"
        }.Select(id => new ShanheCombatantDefinition(id)).ToArray());
        var full = Create(fullDefinition);
        var fullPreview = full.PreviewEncounter("battle");
        Need(fullPreview.Enemies.Count == 7 && fullPreview.Allies!.Count == 0 &&
            fullPreview.StoryText!.Contains("本场席位已满，开局队友暂不参战") && full.Snapshot.StartingCompanions.Count == 1,
            "A full eight-seat authored encounter retains all enemies and explicitly explains that the expedition companion sits out.");
        Accept(full, new MoveShanheNodeCommand("battle", full.Revision));
        var fullCold = Cold(full, fullDefinition);
        Need(fullCold.PendingBattleSetup is { EnemyRoster.Count: 7, AllyRoster.Count: 0 } &&
            fullCold.Snapshot.StartingCompanions.Count == 1 && ShanheBattleSetup.ValueEquals(fullCold.PendingBattleSetup, full.PendingBattleSetup),
            "The full encounter's preview, actual setup and cold replay agree without losing the expedition companion.");

        // Two evidenced bodies are a mechanism fixture, not a claim about the source game's complete random pool.
        var randomDefinition = Definition(pool: [new(Companion), new(StandardShanheBossPackage.FenghuangId)]);
        var first = Create(randomDefinition, seed: 3);
        var second = Create(randomDefinition, seed: 1);
        Need(first.Snapshot.StartingCompanions.Single().GeneralId == Companion &&
            second.Snapshot.StartingCompanions.Single().GeneralId == StandardShanheBossPackage.FenghuangId &&
            Cold(first, randomDefinition).Snapshot.StartingCompanions.Single().GeneralId == Companion &&
            Cold(second, randomDefinition).Snapshot.StartingCompanions.Single().GeneralId == StandardShanheBossPackage.FenghuangId,
            "Verified seeds select distinct members of a multi-entry fixture once and reproduce the same picks after cold restoration.");
    }

    private static ShanheCampaignDefinition Definition(IReadOnlyList<ShanheCombatantDefinition>? enemies = null,
        IReadOnlyList<ShanheCombatantDefinition>? allies = null, IReadOnlyList<ShanheCombatantDefinition>? pool = null) =>
        new("fixture:companion", "开局同伴机制", [Player], ["battle"],
            [new("battle", "同伴测试", ShanheNodeKind.Battle, [],
                [new("encounter", "同伴遭遇", enemies ?? [new("classic:zhang-liao")], allies ?? [])])],
            [new(StandardShanheCampaignCatalog.OpeningCompanionRewardId, "战法·和光盟")
                { OpeningCompanionPool = pool ?? [new(Companion)] }]);

    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, bool equipped = true, int seed = 71) =>
        ShanheCampaignSession.Create(definition, new(seed, Player)
        { EquippedTacticIds = equipped ? [StandardShanheCampaignCatalog.OpeningCompanionRewardId] : [] },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
    private static ShanheCampaignSession Cold(ShanheCampaignSession session, ShanheCampaignDefinition definition) =>
        ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())),
            definition, StandardShanheCampaignCatalog.CreateBattleRegistry);
    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var result = session.Submit(command);
        Need(result.Accepted, result.Error ?? "The companion fixture command was rejected.");
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
