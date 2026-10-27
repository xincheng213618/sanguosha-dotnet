using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2453's complete soldier4861 body and cumulative source673/1136/414 skills.</summary>
public sealed class StandardShanheSourceWarElephantPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-war-elephant-2453-base";
    public const string HardId = "shanhe:source-war-elephant-2453-hard";
    public const string HellId = "shanhe:source-war-elephant-2453-hell";
    public const string ShouyiSkillId = "shanhe:source-shouyi-673";
    public const string XiongshouSkillId = "shanhe:source-xiongshou-1136";
    public const string PojunSkillId = "classic:pojun";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-war-elephant-2453", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 20 => HardId,
        >= 21 and <= 25 => HellId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-war-elephant-2453",
            skill => skill with { Tags = SkillTag.Locked });
        Add(BaseId, "1-10", []);
        Add(HardId, "11-20", [XiongshouSkillId]);
        Add(HellId, "21-25", [XiongshouSkillId, PojunSkillId]);

        void Add(string id, string tier, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            // Source portrait1000003 has no verified local image: supporter is explicit fallback art.
            "战象", "supporter", ShouyiSkillId, "qun", 5, extra)
        {
            InitialHp = 5, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2453-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
