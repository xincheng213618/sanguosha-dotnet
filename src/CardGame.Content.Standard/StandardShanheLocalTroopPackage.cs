using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Locally authored troop bodies; these are not source generals or new skill implementations.</summary>
public sealed class StandardShanheLocalTroopPackage : IGameContentPackage
{
    public const string FootmanId = "shanhe:local-footman";
    public const string ArcherId = "shanhe:local-archer";
    public const string GuardId = "shanhe:local-guard";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-local-troops", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        Add(FootmanId, "西凉步卒（本地）", 4, null);
        Add(ArcherId, "西凉弓手（本地）", 5, "classic:mashu");
        Add(GuardId, "西凉卫士（本地）", 6, "classic:yizhong");

        void Add(string id, string name, int hp, string? skillId) => builder.AddGeneral(new(id, name,
            "supporter", skillId, "qun", hp)
        {
            CharacterId = "character:" + id, VariantId = "shanhe-local-troop",
            RulesetId = "shanhe-local", Gender = GeneralGender.Male
        });
    }
}
