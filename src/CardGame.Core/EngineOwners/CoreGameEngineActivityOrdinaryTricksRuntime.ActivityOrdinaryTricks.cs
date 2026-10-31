#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActivityOrdinaryTricksRuntime
{
    internal bool HasActivityOrdinaryTrickProjection => _modeRulesOperations._modeRules.Cards.ProjectsSkillPrograms;

    internal bool IsActivityOrdinaryTrickNameAvailable(CardKind kind) => _modeRulesOperations._modeRules.Cards.IsDeclaredCardNameAllowed(kind);
    internal SkillProgramViewAs? GetCardConversionDefinition(CardConversionSource source) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(source.SkillId)?.Program is { } original ? ActivityRuntimeProgram(original).ViewAs.FirstOrDefault(rule => rule.Id == source.BindingId) : null;
    internal ContentSkillDefinition ActivityRuntimeSkillDefinition(string id)
    {
        var original = _contentRegistryOperations._contentRegistry.GetSkill(id);
        if (original.Program is not { } source)
            return original;
        var projected = ActivityRuntimeProgram(source);
        return ReferenceEquals(source, projected) ? original : original with
        {
            Program = projected
        };
    }

    internal SkillProgram ActivityRuntimeProgram(SkillProgram original)
    {
        if (!_modeRulesOperations._modeRules.Cards.ShouldProjectProgram(original))
            return original;
        if (_coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess._activityOrdinaryPrograms.TryGetValue(original, out var cached))
            return cached;
        var result = _modeRulesOperations._modeRules.Cards.ProjectProgram(original);
        _coreGameEngineActivityOrdinaryTricksActivityOrdinaryProgramsAccess._activityOrdinaryPrograms.Add(original, result);
        return result;
    }
}
