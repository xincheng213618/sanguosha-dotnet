using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2495's independent soldier4861 body and cumulative source skills.</summary>
public sealed class StandardShanheSourceExecutionerPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-executioner-2495-base";
    public const string MediumId = "shanhe:source-executioner-2495-medium";
    public const string HardId = "shanhe:source-executioner-2495-hard";
    public const string YishiSkillId = "shanhe:source-yishi-611";
    public const string HuanfuSkillId = "shanhe:source-huanfu-3156";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-executioner-2495", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 5 => BaseId,
        >= 6 and <= 10 => MediumId,
        >= 11 and <= 25 => HardId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-executioner");
        Add(BaseId, "base", ["ol:gongqing"]);
        Add(MediumId, "medium", ["ol:gongqing", HuanfuSkillId]);
        Add(HardId, "hard", ["ol:gongqing", HuanfuSkillId, "classic:junxing"]);
        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "校事典刑官",
            // Source d=1000237 has no verified local portrait: supporter is an explicit fallback.
            "supporter", YishiSkillId, "qun", 4, extra)
        {
            InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2495-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
