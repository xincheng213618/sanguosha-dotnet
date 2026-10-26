using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Recruitment panels from the 2026 guide reuse these existing native skill editions.</summary>
public sealed class StandardShanheAdditionalRecruitmentAPackage : IGameContentPackage
{
    public const string QinmiEliteId = "shanhe:companion-qin-mi-elite";
    public const string SunluyuEliteId = "shanhe:companion-sun-lu-yu-elite";
    public const string LusuEliteId = "shanhe:companion-lu-su-elite";
    public const string SunruEliteId = "shanhe:companion-sun-ru-elite";
    public const string XizhicaiEliteId = "shanhe:companion-xi-zhi-cai-elite";

    public PackageManifest Manifest { get; } = new("shanhe-additional-recruitment-a", new Version(1, 0, 0),
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static bool Contains(string generalId) => generalId is QinmiEliteId or SunluyuEliteId or LusuEliteId or SunruEliteId or XizhicaiEliteId;

    public void Register(IContentRegistryBuilder builder)
    {
        Add(QinmiEliteId, "秦宓", "qin_mi", "qin-mi", "shu", GeneralGender.Male,
            "classic:jianzheng", ["classic:zhuandui", "classic:tianbian", "classic:zongshi-pindian"]);
        Add(SunluyuEliteId, "孙鲁育", "ol-sun-lu-yu", "sun-lu-yu", "wu", GeneralGender.Female,
            "ol:mumu", ["ol:meibu", "classic:bazhen"]);
        Add(LusuEliteId, "界鲁肃", "boundary_lu_su", "lu-su", "wu", GeneralGender.Male,
            "boundary:haoshi-current", ["boundary:dimeng-current", "ol:hongde"]);
        Add(SunruEliteId, "孙茹", "ol-sun-ru", "sun-ru", "wu", GeneralGender.Female,
            "ol:chishi", ["ol:weimian", "ol:fenrui"]);
        Add(XizhicaiEliteId, "戏志才", "ol-xi-zhi-cai", "xi-zhi-cai", "wei", GeneralGender.Male,
            "classic:tiandu", ["ol:xianfu", "ol:chouce", "ol:zhongyun"]);
        return;

        void Add(string id, string name, string portrait, string character, string faction, GeneralGender gender,
            string skill, IReadOnlyList<string> additionalSkills) =>
            builder.AddGeneral(new(id, name, portrait, skill, faction, 3, additionalSkills)
            {
                CharacterId = "character:" + character, VariantId = "shanhe-companion-elite",
                RulesetId = "shanhe-2026", Gender = gender
            });
    }
}
