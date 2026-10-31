using System.Text.Json.Serialization;

namespace CardGame.Core;

/// <summary>Immutable selection of real skill grants. Null selectors are unrestricted; empty selectors match nothing.</summary>
public sealed class SkillStateMatch
{
    public SkillStateMatch(IReadOnlyList<string>? skillIds = null, ProgramExpression? filter = null,
        IReadOnlyList<string>? grantIds = null)
    {
        if (filter is not null && !SkillStateMatcher.IsFilterExpression(filter))
            throw new ArgumentException("A skill match filter requires a boolean expression and known skill metadata facts.", nameof(filter));
        SkillIds = FreezeIds(skillIds); Filter = filter; GrantIds = FreezeIds(grantIds);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? SkillIds { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramExpression? Filter { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? GrantIds { get; }
    [JsonIgnore]
    public bool IsTransient => Filter is not null && SkillStateMatcher.IsTransientFilter(Filter);

    public static SkillStateMatch All { get; } = new();
    public static SkillStateMatch NonLocked { get; } = new(filter: SkillStateFilters.NonLocked);
    public static SkillStateMatch CharacterSkills { get; } = new(filter: SkillStateFilters.Character);
    public static SkillStateMatch NonLockedCharacters { get; } = new(filter: SkillStateFilters.And(SkillStateFilters.NonLocked, SkillStateFilters.Character));
    public static SkillStateMatch ForSkills(IReadOnlyList<string> ids) => new(ids);

    public SkillStateMatch Freeze(IEnumerable<SkillGrant> grants) =>
        new(grantIds: grants.Where(grant => !grant.IsModeRule).Select(grant => grant.GrantId).ToArray());

    public bool StructurallyEquals(SkillStateMatch other) => Same(SkillIds, other.SkillIds) && Same(GrantIds, other.GrantIds) &&
        (Filter is null ? other.Filter is null : other.Filter is not null && Filter.StructurallyEquals(other.Filter));

    private static IReadOnlyList<string>? FreezeIds(IReadOnlyList<string>? ids) => ids is null ? null :
        Array.AsReadOnly(ids.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray());
    private static bool Same(IReadOnlyList<string>? left, IReadOnlyList<string>? right) =>
        left is null ? right is null : right is not null && left.SequenceEqual(right, StringComparer.Ordinal);
}

public static class SkillStateFilters
{
    public static ProgramExpression NonLocked { get; } = FactEquals("skill.locked", 0);
    public static ProgramExpression Character { get; } = FactEquals("skill.character", 1);
    public static ProgramExpression Printed { get; } = FactEquals("skill.printed", 1);
    public static ProgramExpression And(params ProgramExpression[] filters) => filters.Length switch
    {
        0 => new("bool", ProgramExpressionType.Bool, [], 1),
        1 => filters[0],
        _ => new("and", ProgramExpressionType.Bool, filters)
    };

    private static ProgramExpression FactEquals(string name, int value) => new("==", ProgramExpressionType.Bool,
        [new("number", ProgramExpressionType.Int, [], name: name), new("int", ProgramExpressionType.Int, [], value)]);
}

internal static class SkillStateMatcher
{
    internal static bool IsPrinted(SkillGrant grant) => grant.SourceId is CharacterState.PrimarySkillSource or CharacterState.SecondarySkillSource;
    internal static bool IsCharacter(SkillGrant grant) => !grant.IsModeRule && !grant.SourceId.StartsWith("equipment:", StringComparison.Ordinal);
    internal static bool IsFilterFact(string? name) => name is "skill.locked" or "skill.limited" or "skill.awakening" or
        "skill.lord" or "skill.printed" or "skill.ordinal" or "skill.character" or "skill.isSource";

    internal static bool Matches(SkillStateMatch match, SkillGrant grant, CharacterState owner,
        ContentSkillDefinition definition, string? sourceSkillId = null, IProgramExpressionWorld? bindings = null) => !grant.IsModeRule &&
        (match.SkillIds is null || match.SkillIds.Contains(grant.SkillId, StringComparer.Ordinal)) &&
        (match.GrantIds is null || match.GrantIds.Contains(grant.GrantId, StringComparer.Ordinal)) &&
        (match.Filter is null || ProgramExpressions.EvaluateBool(match.Filter,
            new MetadataWorld(owner, grant, definition, sourceSkillId, bindings)) == true);

    internal static bool IsFilterExpression(ProgramExpression expression) => expression.Type == ProgramExpressionType.Bool &&
        expression.Nodes().All(node => node.Op != "number" || node.Name?.StartsWith("skill.", StringComparison.Ordinal) != true || IsFilterFact(node.Name));

    internal static bool IsTransientFilter(ProgramExpression expression) => expression.Nodes().Any(node =>
        node.Op == "number" ? !IsFilterFact(node.Name) : node.Op is not
            ("bool" or "int" or "and" or "or" or "not" or "<" or "<=" or ">" or ">=" or "==" or "!=" or
             "+" or "-" or "min" or "max" or "floorDiv" or "when"));

    private sealed class MetadataWorld(CharacterState owner, SkillGrant grant, ContentSkillDefinition skill,
        string? sourceSkillId, IProgramExpressionWorld? bindings) : IProgramExpressionWorld
    {
        public int? Participant(string name) => bindings?.Participant(name);
        public IReadOnlyList<int>? ParticipantSet(string name) => bindings?.ParticipantSet(name);
        public int? Stat(string stat, int seat) => bindings?.Stat(stat, seat);
        public bool? Flag(string flag, int seat) => bindings?.Flag(flag, seat);
        public int? BindingStat(string stat, string name) => bindings?.BindingStat(stat, name);
        public int? Number(string name) => name switch
        {
            "skill.locked" => skill.Tags.HasFlag(SkillTag.Locked) ? 1 : 0,
            "skill.limited" => skill.Tags.HasFlag(SkillTag.Limited) ? 1 : 0,
            "skill.awakening" => skill.Tags.HasFlag(SkillTag.Awakening) ? 1 : 0,
            "skill.lord" => skill.Tags.HasFlag(SkillTag.Lord) ? 1 : 0,
            "skill.printed" => IsPrinted(grant) ? 1 : 0,
            "skill.character" => IsCharacter(grant) ? 1 : 0,
            "skill.isSource" => sourceSkillId is null ? null : grant.SkillId == sourceSkillId ? 1 : 0,
            "skill.ordinal" => PrintedOrdinal(),
            _ => bindings?.Number(name)
        };

        private int? PrintedOrdinal()
        {
            if (!IsPrinted(grant)) return 0;
            var general = grant.SourceId == CharacterState.PrimarySkillSource ? owner.General : owner.SecondaryGeneral;
            var index = general is null ? -1 : Array.FindIndex(general.Skills.ToArray(), definition => definition.ContentId == grant.SkillId);
            return index < 0 ? 0 : index + 1;
        }
    }
}
