using CardGame.Core;

namespace CardGame.Content.Standard;

/// <summary>Source2459's independent soldier4861 body and cumulative source skills.</summary>
public sealed class StandardShanheSourceCoalitionSpyPackage : IGameContentPackage
{
    public const string BaseId = "shanhe:source-coalition-spy-2459-base";
    public const string HardId = "shanhe:source-coalition-spy-2459-hard";
    public const string NightmareId = "shanhe:source-coalition-spy-2459-nightmare";
    public const string QiceSkillId = "classic:qice";
    public const string TishenSkillId = "boundary:tishen";
    public const string DuanfaSkillId = "mobile:duanfa";
    public static readonly Version CurrentVersion = new(1, 0, 0);
    public PackageManifest Manifest { get; } = new("shanhe-source-coalition-spy-2459", CurrentVersion,
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
        Add(BaseId, "base", []);
        Add(HardId, "hard", [TishenSkillId]);
        Add(NightmareId, "nightmare", [TishenSkillId, DuanfaSkillId]);

        void Add(string id, string tier, string[] extra) => builder.AddGeneral(new(id, "联军细作",
            // The source d=1000304 has no verified local portrait; this is an explicit fallback.
            "supporter", QiceSkillId, "qun", 3, extra)
        {
            InitialHp = 3, CharacterId = "character:shanhe-source-soldier-4861",
            VariantId = "shanhe-source-2459-" + tier + "-portrait-fallback",
            RulesetId = "shanhe-s5-current-config", Gender = GeneralGender.Male
        });
    }
}
