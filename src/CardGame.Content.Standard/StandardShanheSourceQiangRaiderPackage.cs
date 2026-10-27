using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2473's complete soldier4861 roster and cumulative source tiers.</summary>
public sealed class StandardShanheSourceQiangRaiderPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-qiang-raider-2473-base";
    public const string HardId = "shanhe:source-qiang-raider-2473-hard";
    public const string NightmareId = "shanhe:source-qiang-raider-2473-nightmare";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-qiang-raider-2473", CurrentVersion,
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
        Add(HardId, ["boundary:paoxiao"], "11-15");
        Add(NightmareId, ["boundary:paoxiao", "ol:sidao"], "16-25");

        void Add(string id, IReadOnlyList<string> additionalSkills, string tier) => builder.AddGeneral(
            new(id, "羌人劫匪", "supporter", "ol:zhidao", "qun", 4, additionalSkills)
            {
                InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
                VariantId = "shanhe-source-2473-" + tier, RulesetId = "shanhe-s5-current-config",
                Gender = GeneralGender.Male
            });
    }
}
