using System.Text.Json.Serialization;

namespace CardGame.Core.Campaigns;

/// <summary>Source-defined recipe. Completion does not imply that its combat effect is implemented.</summary>
public sealed record ShanheSchoolDefinition
{
    public int Id { get; init; }
    public string Name { get; init; }
    public int EffectTacticId { get; init; }
    public string Description { get; init; }
    private readonly IReadOnlyList<int> _requirements = Array.Empty<int>();
    public IReadOnlyList<int> Requirements
    {
        get => _requirements;
        init => _requirements = ShanheCampaignContent.Freeze(value ?? throw new ArgumentNullException(nameof(value)));
    }
    public ShanheSchoolDefinition(int id, string name, int effectTacticId, string description, IReadOnlyList<int> requirements)
    { Id = id; Name = name; EffectTacticId = effectTacticId; Description = description; Requirements = requirements; }
}

/// <summary>The exact tactic source and its own rarity bucket, independent of shop presentation.</summary>
public sealed record ShanheTacticSchoolMembership(int SourceId, int SchoolId, int Rarity);

public sealed record ShanheSchoolProgress(ShanheSchoolDefinition School, IReadOnlyList<int> OwnedCounts, bool IsComplete)
{
    private readonly IReadOnlyList<int> _ownedCounts = ShanheCampaignContent.Freeze(OwnedCounts);
    public IReadOnlyList<int> OwnedCounts
    { get => _ownedCounts; init => _ownedCounts = ShanheCampaignContent.Freeze(value); }
}

public sealed partial record ShanheRewardDefinition
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheTacticSchoolMembership? SchoolMembership { get; init; }
}

public sealed partial record ShanheCampaignDefinition
{
    private readonly IReadOnlyList<ShanheSchoolDefinition>? _schools;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ShanheSchoolDefinition>? Schools
    {
        get => _schools;
        init => _schools = value is null ? null : ShanheCampaignContent.Freeze(value.Select(school =>
            school is null ? throw new ArgumentException("A school definition cannot be null.", nameof(value)) :
            school with { Requirements = school.Requirements }));
    }
}

public static partial class ShanheCampaignContent
{
    private static void ValidateSchools(ShanheCampaignDefinition definition)
    {
        var schools = definition.Schools ?? [];
        Require(schools.Count <= 32 && schools.Select(s => s.Id).Distinct().Count() == schools.Count &&
            schools.Select(s => s.EffectTacticId).Distinct().Count() == schools.Count,
            "School definitions must have bounded, distinct identities and derived effects.");
        foreach (var school in schools)
        {
            Require(school.Id > 0 && school.EffectTacticId > 0, "Invalid school or derived-effect source identity.");
            RequireText(school.Name, "school name"); RequireText(school.Description, "school effect");
            Require(school.Requirements.Count == 4 && school.Requirements.All(count => count is >= 0 and <= 20) &&
                school.Requirements.Sum() > 0, "A school needs four exact rarity requirements.");
        }
        var ids = schools.Select(s => s.Id).ToHashSet();
        var derived = schools.Select(s => s.EffectTacticId).ToHashSet();
        foreach (var reward in definition.Rewards)
            if (reward.SchoolMembership is { } member)
                Require(member.SourceId > 0 && ids.Contains(member.SchoolId) && member.Rarity is >= 1 and <= 4 &&
                    !derived.Contains(member.SourceId) && reward.SkillId is null &&
                    (reward.TacticSkillId is not null || reward.CampaignEconomy is not null || reward.OpeningSkillRewardIds is not null),
                    "Only a concrete tactic source may count toward a declared school.");
        Require(definition.Rewards.Select(r => r.SchoolMembership).OfType<ShanheTacticSchoolMembership>()
            .GroupBy(member => member.SourceId).All(group => group.Select(member => (member.SchoolId, member.Rarity)).Distinct().Count() == 1),
            "Aliases of one tactic source must retain one school and rarity.");
    }
}
