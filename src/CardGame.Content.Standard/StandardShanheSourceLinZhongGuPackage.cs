using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2476's general4871 body with its exact cumulative Changbiao/Fulin skills.</summary>
public sealed class StandardShanheSourceLinZhongGuPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-linzhonggu-2476-base";
    public const string NightmareId = "shanhe:source-linzhonggu-2476-nightmare";
    public const string ChangbiaoSkillId = "boundary:changbiao-current";
    public const string FulinSkillId = "classic:fulin";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-linzhonggu-2476", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 15 => BaseId,
        >= 16 and <= 25 => NightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        Add(BaseId, "base", []);
        Add(NightmareId, "nightmare", [FulinSkillId]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "林中鹘",
            "supporter", ChangbiaoSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-general-4871",
            VariantId = "shanhe-source-2476-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
