using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2455's complete soldier4861 roster and cumulative source tiers.</summary>
public sealed class StandardShanheSourceCoalitionScoutPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-coalition-scout-2455-base";
    public const string HardId = "shanhe:source-coalition-scout-2455-hard";
    public const string HellId = "shanhe:source-coalition-scout-2455-hell";
    public static readonly Version CurrentVersion = new(1, 0, 0);

    public PackageManifest Manifest { get; } = new("shanhe-source-coalition-scout-2455", CurrentVersion,
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
        Add(BaseId, [], "1-10");
        Add(HardId, ["classic:kanpo"], "11-20");
        Add(HellId, ["classic:kanpo", "ol:dianhu"], "21-25");

        void Add(string id, IReadOnlyList<string> additionalSkills, string tier) => builder.AddGeneral(
            new(id, "联军斥候", "supporter", "classic:dangxian", "qun", 4, additionalSkills)
            {
                InitialHp = 4, CharacterId = "character:shanhe-source-soldier-4861",
                VariantId = "shanhe-source-2455-" + tier, RulesetId = "shanhe-s5-current-config",
                Gender = GeneralGender.Male
            });
    }
}
