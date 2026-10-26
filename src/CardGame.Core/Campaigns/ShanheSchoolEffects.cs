namespace CardGame.Core.Campaigns;

/// <summary>Derived effects of complete schools; these are never purchasable components.</summary>
public static class ShanheSchoolEffects
{
    public const string DrawSkillId = "shanhe:school-draw-2231";
    public const string MultiSlashSkillId = "shanhe:school-multi-slash-2232";
    public const string DamageSkillId = "shanhe:school-damage-2233";
    public const string FullHpSkillId = "shanhe:school-full-hp-2364";
    public const string TeamRankSkillId = "shanhe:school-team-rank-2235";
    public const string DyingSkillId = "shanhe:school-dying-2236";
    public const string LateSlashSkillId = "shanhe:school-late-slash-2255";
    public const string TrickRepeatSkillId = "shanhe:school-trick-repeat-2363";

    public static string? SkillId(ShanheSchoolDefinition school) => (school.Id, school.EffectTacticId) switch
    {
        (2, 2231) => DrawSkillId,
        (3, 2232) => MultiSlashSkillId,
        (4, 2233) => DamageSkillId,
        (5, 2364) => FullHpSkillId,
        (6, 2235) => TeamRankSkillId,
        (7, 2236) => DyingSkillId,
        (8, 2255) => LateSlashSkillId,
        (9, 2363) => TrickRepeatSkillId,
        _ => null
    };

    public static int? SchoolId(string skillId) => skillId switch
    {
        DrawSkillId => 2, MultiSlashSkillId => 3, DamageSkillId => 4,
        FullHpSkillId => 5, TeamRankSkillId => 6, DyingSkillId => 7,
        LateSlashSkillId => 8, TrickRepeatSkillId => 9,
        _ => null
    };
}
