using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2460's soldier4861 body and cumulative source3538/951/3277 skills.</summary>
public sealed class StandardShanheSourceLocalMagnatePackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-local-magnate-2460-base";
    public const string HardId = "shanhe:source-local-magnate-2460-hard";
    public const string NightmareId = "shanhe:source-local-magnate-2460-nightmare";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-local-magnate-2460", CurrentVersion,
        [new("standard-classic-generals", StandardClassicGeneralPackage.CurrentVersion)]);

    public static string GetGeneralId(int difficulty) => difficulty switch
    {
        >= 1 and <= 10 => BaseId,
        >= 11 and <= 15 => HardId,
        >= 16 and <= 25 => NightmareId,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
    };

    public void Register(IContentRegistryBuilder builder)
    {
        Add(BaseId, [], "1-10");
        Add(HardId, ["ol:weicheng"], "11-15");
        Add(NightmareId, ["ol:weicheng", "ol:shunshi"], "16-25");

        void Add(string id, IReadOnlyList<string> additionalSkills, string tier) => builder.AddGeneral(
            new(id, "本地豪强", "supporter", "ol:shuzi", "qun", 4, additionalSkills)
            {
                InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
                VariantId = "shanhe-source-2460-" + tier, RulesetId = "shanhe-s5-current-config",
                Gender = GeneralGender.Male
            });
    }
}
