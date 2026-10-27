using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2489 keeps the independent general4871 body and source3252 skill.</summary>
public sealed class StandardShanheSourceXinPingPackage : IGameContentPackage
{
    public const string GeneralId = "shanhe:source-xinping-2489";
    public const string SkillId = "ol:chuanwu";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-xinping-2489", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty is >= 1 and <= 25
        ? GeneralId : throw new ArgumentOutOfRangeException(nameof(difficulty));

    public void Register(IContentRegistryBuilder builder) => builder.AddGeneral(new(GeneralId,
        // Source d=1000261 is not a verified local Xin Ping portrait.
        "辛评", "supporter", SkillId, "qun", 4)
    {
        InitialHp = 4, CharacterId = "character:shanhe-source-general-4871",
        VariantId = "shanhe-source-2489-portrait-fallback", RulesetId = "shanhe-s5-current-config",
        Gender = GeneralGender.Male
    });
}
