using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2466's complete soldier4861 body and cumulative source9000/7014 skills.</summary>
public sealed class StandardShanheSourceYouyanWarriorPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-youyan-warrior-2466-base";
    public const string HardId = "shanhe:source-youyan-warrior-2466-hard";
    public const string JuguanSkillId = "ol:juguan";
    public const string ZhaoranSkillId = "shanhe:source-zhaoran-7014";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-youyan-warrior-2466", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 25 => HardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-zhaoran-7014");
        Add(BaseId, "1-10", []);
        Add(HardId, "11-25", [ZhaoranSkillId]);

        void Add(string id, string tier, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            // Desktop binds source skin1000170 by exact runtime ID; supporter remains the runtime fallback.
            "幽燕壮士", "supporter", JuguanSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2466-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
