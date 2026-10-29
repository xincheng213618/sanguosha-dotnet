using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Complete source2488 Zhou Xin recipe on its original general4871 body.</summary>
public sealed class StandardShanheSourceArmorerPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-armorer-2488-base";
    public const string HardId = "shanhe:source-armorer-2488-hard";
    public const string NightmareId = "shanhe:source-armorer-2488-nightmare";
    public const string DuanjiaSkillId = "shanhe:source-duanjia-14095";
    public const string DujinSkillId = "ol:dujin";
    public const string ZizhongSkillId = StandardShanheEliteHandangPackage.ZizhongId;
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-armorer-2488", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion),
         new("shanhe-elite-handang", StandardShanheEliteHandangPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 15 => HardId,
        >= 16 and <= 25 => NightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-armorer-2488", skill =>
            skill with { ExecutionForms = SkillExecutionForm.Trigger });
        Add(BaseId, []);
        Add(HardId, [DujinSkillId]);
        Add(NightmareId, [DujinSkillId, ZizhongSkillId]);

        void Add(string id, IReadOnlyList<string> extra) => builder.AddGeneral(new(id,
            // Keep Zhou Xin's runtime metadata; desktop artwork uses the verified PVE skin1000204.
            "周昕", "supporter", DuanjiaSkillId, "qun", 5, extra)
        {
            InitialHp = 5, CharacterId = "character:shanhe-source-general-4871",
            VariantId = id["shanhe:".Length..] + "-portrait1000204-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
