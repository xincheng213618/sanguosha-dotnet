using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>S5 group2500's independent general4871 body and complete cumulative source skills.</summary>
public sealed class StandardShanheSourceAgricultureOfficerPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-agriculture-officer-2500-base";
    public const string HardId = "shanhe:source-agriculture-officer-2500-hard";
    public const string HellId = "shanhe:source-agriculture-officer-2500-hell";
    public const string TuntianSkillId = "shanhe:source-tuntian-959";
    public const string TianduSkillId = "classic:tiandu";
    public const string DuanliangSkillId = "boundary:duanliang-mobile";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-agriculture-officer-2500", CurrentVersion,
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
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-tuntian-959");
        Add(BaseId, "base", []);
        Add(HardId, "hard", [TianduSkillId]);
        Add(HellId, "hell", [TianduSkillId, DuanliangSkillId]);
        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "农都尉",
            // Source d=1000213 has no verified local portrait: supporter is an explicit fallback.
            "supporter", TuntianSkillId, "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:shanhe-source-general-4871",
            VariantId = "shanhe-source-2500-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
