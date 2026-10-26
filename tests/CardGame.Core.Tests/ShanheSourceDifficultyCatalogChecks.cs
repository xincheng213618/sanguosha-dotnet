using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

internal static class ShanheSourceDifficultyCatalogChecks
{
    public static void SourceTiersKeepVerbatimModifiersAndIndependentIdentity()
    {
        var catalog = StandardShanheSourceDifficultyCatalog.Season5;
        Need(catalog.SeasonId == 5 && catalog.MaximumDifficulty == 25 &&
            catalog.Entries.Select(row => row.SourceId).SequenceEqual(Enumerable.Range(1, 25)) &&
            catalog.Entries.GroupBy(row => row.GroupId).Select(group => (group.Key, group.Count()))
                .SequenceEqual(new[] { (1, 5), (2, 5), (3, 5), (4, 5), (5, 5) }),
            "The source has twenty-five distinct tiers, not renamed versions of the local three difficulties.");
        Need(catalog.Get(2).Modifiers is { Hp: 1, CardNum: 1, HpRate: 3, DrawRate: 3 } &&
            catalog.Get(6).Modifiers is { Hp: 0, Draw: 1, HpRate: 30 } &&
            catalog.Get(11).Description == "敌人拥有护甲回复" && catalog.Entries.All(row => row.Modifiers.GetArmor == 0) &&
            catalog.Get(21).Description == "第二章起，敌人造成伤害+1" && catalog.Get(21).Modifiers.OtherAd == 0 &&
            catalog.Get(25).Modifiers is { Hp: 2, Draw: 3, CardNum: 4, Skill: 1, Armor: 6, OtherAd: 1, HpRate: 160 },
            "Source modifier rows retain resets and unresolved description/modifier differences without inventing an armor, damage or rate formula.");
        Need(catalog.OfficialEvidence is { MemberName: "hd_roguelike.sgs.Root.DifficultySelection" } &&
            catalog.Get(5).Availability is { PreviousSourceId: 4, RequiredPlayerLevel: 1 } &&
            catalog.Get(6).Availability is { PreviousSourceId: 1, RequiredPlayerLevel: 45 } &&
            catalog.Get(11).Availability is { PreviousSourceId: 6, RequiredPlayerLevel: 80 } &&
            catalog.Get(16).Availability is { PreviousSourceId: 11, RequiredPlayerLevel: 100, OpenFlag: 0, OpenTimeSpec: "20261015T040000" } &&
            catalog.Get(21).Availability is { PreviousSourceId: 16, RequiredPlayerLevel: 110, OpenTimeSpec: "20261105T040000" } &&
            catalog.Get(1).Modifiers.HpSave is null && catalog.Get(6).Modifiers.HpSave == 1 && catalog.Get(25).RecSlotCtn == 5,
            "Official client fields retain actual per-tier prerequisites, user-level gates, scheduled opening text and absent-versus-present modifiers.");
        var copiedRows = catalog.Entries.ToArray();
        var detached = Copy(copiedRows);
        copiedRows[0] = catalog.Get(25);
        Need(detached.Get(1).Name == "普通1" && detached.BySourceId.Count == 25 &&
            ((ICollection<ShanheSourceDifficultyDefinition>)detached.Entries).IsReadOnly &&
            ((IDictionary<int, ShanheSourceDifficultyDefinition>)detached.BySourceId).IsReadOnly &&
            detached.GroupUnlockRuleText == "通过大难度下的任一小难度可解锁下一大难度。",
            "The source snapshot freezes its row collections and retains the actual group unlock evidence.");
        try { _ = Copy(copiedRows); throw new InvalidOperationException("Duplicated source difficulty IDs were accepted."); }
        catch (ArgumentException) { }
        try { _ = catalog.Get(26); throw new InvalidOperationException("An unavailable source tier was accepted."); }
        catch (ArgumentOutOfRangeException) { }

        ShanheSourceDifficultyCatalog Copy(IEnumerable<ShanheSourceDifficultyDefinition> rows) => new(catalog.SeasonId,
            catalog.MaximumDifficulty, catalog.SourceUrl, catalog.GeneratedAt, catalog.GroupUnlockRuleText, rows);
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
