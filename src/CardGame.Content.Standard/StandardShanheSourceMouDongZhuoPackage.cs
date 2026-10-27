using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2684's independent three-skill variant and the shared original Guanbian.</summary>
public sealed class StandardShanheSourceMouDongZhuoPackage : IGameContentPackage
{
    public const string GeneralId = "shanhe:source-mou-dongzhuo-2684";
    public const string XiongniSkillId = "shanhe:source-xiongni-14206";
    public const string FengshangSkillId = "shanhe:source-fengshang-14207";
    public const string ZhibingSkillId = "shanhe:source-zhibing-14208";
    public const string ZhixiSkillId = "shanhe:source-zhixi-14209";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-mou-dongzhuo-2684", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public void Register(IContentRegistryBuilder builder)
    {
        EmbeddedSkillProgramCatalog.RegisterBundle(builder, "shanhe-source-mou-dongzhuo-2684",
            skill => skill.Id == ZhibingSkillId ? skill with { Tags = SkillTag.Locked } : skill);
        builder.AddSkill(new(ZhixiSkillId, "止息", "锁定技，你不能使用牌。")
            { Tags = SkillTag.Locked, ExecutionForms = SkillExecutionForm.Trigger });
        builder.AddGeneral(new(GeneralId, "逆京枭雄", "mou-dong-zhuo-ol", "mou-ol:guanbian", "qun", 20,
            [XiongniSkillId, FengshangSkillId, ZhibingSkillId])
        {
            InitialHp = 20, CharacterId = "character:dong-zhuo", VariantId = "shanhe-source-2684-60500",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
