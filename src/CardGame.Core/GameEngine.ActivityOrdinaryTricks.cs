namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Projections and their cache belong to this match. Registered source programs keep their content identity.
    private readonly Dictionary<SkillProgram, SkillProgram> _activityOrdinaryPrograms = [];
    private bool HasActivityOrdinaryTrickProjection => _modeRules.Cards.ProjectsSkillPrograms;
    private bool IsActivityOrdinaryTrickNameAvailable(CardKind kind) => _modeRules.Cards.IsDeclaredCardNameAllowed(kind);

    /// <summary>Returns immutable rule metadata for a published conversion source in this match's mode.</summary>
    public SkillProgramViewAs? GetCardConversionDefinition(CardConversionSource source) =>
        _contentRegistry.Skills.GetValueOrDefault(source.SkillId)?.Program is { } original
            ? ActivityRuntimeProgram(original).ViewAs.FirstOrDefault(rule => rule.Id == source.BindingId)
            : null;

    private ContentSkillDefinition ActivityRuntimeSkillDefinition(string id)
    {
        var original = _contentRegistry.GetSkill(id);
        if (original.Program is not { } source) return original;
        var projected = ActivityRuntimeProgram(source);
        return ReferenceEquals(source, projected) ? original : original with { Program = projected };
    }

    private SkillProgram ActivityRuntimeProgram(SkillProgram original)
    {
        if (!_modeRules.Cards.ShouldProjectProgram(original)) return original;
        if (_activityOrdinaryPrograms.TryGetValue(original, out var cached)) return cached;
        var result = _modeRules.Cards.ProjectProgram(original);
        _activityOrdinaryPrograms.Add(original, result);
        return result;
    }
}
