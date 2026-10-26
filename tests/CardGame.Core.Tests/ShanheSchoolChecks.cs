using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSchoolChecks
{
    private const string Owner = "fixture:school-owner", Peer = "fixture:school-peer";
    private const string AliasA = "fixture:school-alias-a", AliasB = "fixture:school-alias-b";
    private static readonly Lazy<ContentRegistry> Registry = new(() => ContentRegistry.Build(new StandardContentPackage(), new Fixture()));

    public static void CompletionRequiresOwnedExactRaritiesAndDistinctAcquisitionSources()
    {
        foreach (var sameAcquisitionIdentity in new[] { false, true })
        {
            var requirements = new[] { 2, 2, 2, 1 };
            var definition = Definition(sameAcquisitionIdentity, requirements);
            requirements[0] = 0;
            var unlockedOnly = Create(definition, []);
            Need(unlockedOnly.GetSchoolProgress() is [{ IsComplete: false } unowned] && unowned.OwnedCounts.SequenceEqual([0, 0, 0, 0]),
                "Research access to every component is not ownership and does not complete a school.");
            var owned = new[] { "common", "rare-one", "rare-two", "epic-one", "epic-two", "legendary" };
            var s = Create(definition, owned);
            Need(s.GetSchoolProgress() is [{ IsComplete: false } six] && six.OwnedCounts.SequenceEqual([1, 2, 2, 1]) &&
                six.School.Requirements.SequenceEqual([2, 2, 2, 1]),
                "Six real initial components retain the frozen recipe; a legendary cannot fill a missing common bucket.");
            Accept(s, new MoveShanheNodeCommand("grant", s.Revision));
            Accept(s, new ChooseShanheNodeOptionCommand("take", null, s.Revision));
            Need(s.Snapshot.TacticIds.Count == 7 && s.GetSchoolProgress() is [{ IsComplete: false } aliases] &&
                aliases.OwnedCounts.SequenceEqual([1, 2, 2, 1]),
                "A seventh owned alias cannot duplicate its canonical acquisition identity or its concrete source.");
            Accept(s, new MoveShanheNodeCommand("shop", s.Revision));
            Need(!s.Snapshot.ShopPaidRefreshUnlimited, "Six distinct source identities still have a finite merchant allowance.");
            var item = s.Snapshot.ShopItems.Single();
            Accept(s, new ChooseShanheNodeOptionCommand(item.ChoiceId, null, s.Revision));
            var progress = s.GetSchoolProgress().Single();
            Need(progress.IsComplete && progress.OwnedCounts.SequenceEqual([2, 2, 2, 1]) && s.Snapshot.ShopPaidRefreshUnlimited,
                "Acquiring a distinct common component completes the exact recipe and activates resource refreshes immediately.");
            var frozen = false;
            try { ((IList<int>)progress.OwnedCounts)[0] = 99; } catch (NotSupportedException) { frozen = true; }
            Need(frozen && s.GetSchoolProgress().Single().OwnedCounts[0] == 2,
                "Published school counts cannot rewrite actual campaign ownership.");
        }
        var normal = Definition(false, [2, 2, 2, 1]);
        normal = normal with
        {
            Schools = null,
            Rewards = normal.Rewards.Select(r => r with { SchoolMembership = null }).Append(
                new ShanheRewardDefinition("standalone-flag", "旧独立标记") { CampaignEconomy = new(UnlimitedPaidShopRefreshes: true) }).ToArray(),
            StartingNodeIds = ["shop"],
            Nodes = normal.Nodes.Where(node => node.Id != "grant").ToArray()
        };
        var noSchools = Create(normal, ["standalone-flag"]);
        Accept(noSchools, new MoveShanheNodeCommand("shop", noSchools.Revision));
        Need(noSchools.GetSchoolProgress().Count == 0 && !noSchools.Snapshot.ShopPaidRefreshUnlimited && noSchools.Snapshot.ShopPaidRefreshRemaining == 1,
            "A recipe without school data retains ordinary refresh rules; a standalone economy flag is not a completed school.");
    }

    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, IReadOnlyList<string> carried) =>
        ShanheCampaignSession.Create(definition, new(71, Owner) { UseCodexRewardPool = true,
            UnlockedRewardIds = definition.Rewards.Select(r => r.Id).ToArray(), EquippedTacticIds = carried }, _ => Registry.Value);
    private static ShanheCampaignDefinition Definition(bool sameIdentity, IReadOnlyList<int> requirements)
    {
        ShanheRewardDefinition Economic(string id, int source, int rarity) => new(id, id)
            { Rarity = rarity, CampaignEconomy = new(ReplacedEquipmentGold: 1), SchoolMembership = new(source, 1, rarity) };
        var common = new ShanheRewardDefinition("common", "普通实际来源", Bonuses: null)
            { Rarity = 1, TacticSkillId = AliasA, SchoolMembership = new(2018, 1, 1) };
        var alias = sameIdentity ? new ShanheRewardDefinition("alias", "同取得身份别名")
            { Rarity = 1, TacticSkillId = AliasB, SchoolMembership = new(2092, 1, 1) }
            : Economic("alias", 2018, 1);
        var rewards = new[] { common, alias, Economic("rare-one", 2013, 2), Economic("rare-two", 2014, 2),
            Economic("epic-one", 2107, 3), Economic("epic-two", 2137, 3), Economic("legendary", 2146, 4),
            Economic("missing-common", 2180, 1) };
        return new("fixture:school-identities", "资源组合", [Owner], ["grant"],
            [new("grant", "取得一份别名", ShanheNodeKind.Event, ["shop"], Choices: [new("take", "领取", RewardId: "alias")]),
             new("shop", "商店", ShanheNodeKind.Shop, ["battle"], Choices: [new("buy", "缺少的普通件", 100, RewardId: "missing-common", LeavesNode: false)]),
             new("battle", "终点", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(Peer)])])], rewards, StartingGold: 500)
            { Schools = [new(1, "资源流", 2230, "商店可无限刷新", requirements)] };
    }
    private static void Accept(ShanheCampaignSession s, ShanheCampaignCommand c)
    { var result = s.Submit(c); Need(result.Accepted, result.Error ?? "School ownership command failed."); }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:school-identities", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var ids = new[] { AliasA, AliasB };
            var catalog = SkillProgramCatalog.Load(JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
                skills = ids.Select(id => new { id, revision = 1, triggers = new[] { new { id = "opening", window = "gameStarting", subject = "owner", optional = false,
                    effects = new[] { new { op = "draw", target = "owner", amount = 1 } } } } }).ToArray()
            }), JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = ids.ToDictionary(id => id, _ => new { name = "取得身份别名", description = "共享同一可重复取得身份" }) }));
            foreach (var id in ids) builder.AddSkill(new(id, id, id) { Program = catalog.Programs[id],
                ProgramPresentation = catalog.Presentations[id], CampaignTacticAcquisition = new(AliasA, Repeatable: true) });
            builder.AddGeneral(new(Owner, "组合使用者", "supporter", "standard:none", "shu", 4));
            builder.AddGeneral(new(Peer, "守军", "supporter", "standard:none", "wei", 4));
        }
    }
}
