using CardGame.Core;
namespace CardGame.Content.Standard;

/// <summary>S5 group2454 keeps its independent soldier4861 body and cumulative source skills.</summary>
public sealed class StandardShanheSourceWhiteHorsePackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-white-horse-2454-base";
    public const string HardId = "shanhe:source-white-horse-2454-hard";
    public const string HellId = "shanhe:source-white-horse-2454-hell";
    public const string XiyingSkillId = "shanhe:source-xiying-1180";
    public const string QingjiaoSkillId = "ol:qingjiao";
    public const string YicongSkillId = "boundary:yicong-current";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-white-horse-2454", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);
    public static string GetGeneralId(int difficulty) => difficulty switch
    { >= 1 and <= 10 => BaseId, >= 11 and <= 20 => HardId, >= 21 and <= 25 => HellId, _ => throw new ArgumentOutOfRangeException(nameof(difficulty)) };
    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-xiying-1180");
        Add(BaseId, "base", []); Add(HardId, "hard", [QingjiaoSkillId]); Add(HellId, "hell", [QingjiaoSkillId, YicongSkillId]);
        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "白马义从",
            // Source d=1000128 has no verified local portrait: supporter is explicit fallback art.
            "supporter", XiyingSkillId, "qun", 4, extra)
        { InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861", VariantId = "shanhe-source-2454-" + tier + "-portrait-fallback",
          RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male });
    }
}
