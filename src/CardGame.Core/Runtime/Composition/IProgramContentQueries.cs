namespace CardGame.Core;

internal interface IProgramContentQueries : IProgramSelectionQuery
{
    ContentSkillDefinition Skill(string skillId);
    ContentSkillDefinition? FindSkill(string skillId);
    bool HasSkill(string skillId);
    SkillTag SkillTags(string skillId);
    bool HasTriggerWindow(SkillProgramTriggerWindow window);
    bool HasTriggerOperation(SkillProgramEffectOp operation);
    bool HasActivationOperation(SkillProgramEffectOp operation);
    bool UsesTriggerCondition(SkillProgramTriggerConditionKind condition);
    bool UsesTriggerValue(SkillProgramTriggerValueKind value);
    bool UsesActualTurnDeathBenefits { get; }
    bool UsesAnyLivingTurnStart { get; }
    bool RequiresFact(ProgramTrackedFact fact);
    bool UsesDrawPhaseDrawCountHandLimit { get; }
    bool UsesSkillStatePhaseEnds { get; }
    bool UsesEqualNameLengthUse { get; }
    bool HasCardPolicy(SkillProgramCardPolicyKind policy);
    string SkillName(string skillId);
    string? AuthorityName(string skillId);
}

public sealed partial class ContentRegistry : IProgramContentQueries
{
    ContentSkillDefinition IProgramContentQueries.Skill(string skillId) => GetSkill(skillId);
    ContentSkillDefinition? IProgramContentQueries.FindSkill(string skillId) => Skills.GetValueOrDefault(skillId);
    bool IProgramContentQueries.HasSkill(string skillId) => Skills.ContainsKey(skillId);
    SkillTag IProgramContentQueries.SkillTags(string skillId) => GetSkill(skillId).Tags;
    bool IProgramContentQueries.HasTriggerWindow(SkillProgramTriggerWindow window) =>
        ProgramDependencies.HasTriggerWindow(window);
    bool IProgramContentQueries.HasTriggerOperation(SkillProgramEffectOp operation) =>
        ProgramDependencies.HasTriggerOperation(operation);
    bool IProgramContentQueries.HasActivationOperation(SkillProgramEffectOp operation) =>
        ProgramDependencies.HasActivationOperation(operation);
    bool IProgramContentQueries.UsesTriggerCondition(SkillProgramTriggerConditionKind condition) =>
        ProgramDependencies.UsesTriggerCondition(condition);
    bool IProgramContentQueries.UsesTriggerValue(SkillProgramTriggerValueKind value) =>
        ProgramDependencies.UsesTriggerValue(value);
    bool IProgramContentQueries.UsesActualTurnDeathBenefits => ProgramDependencies.UsesActualTurnDeathBenefits;
    bool IProgramContentQueries.UsesAnyLivingTurnStart => ProgramDependencies.UsesAnyLivingTurnStart;
    bool IProgramContentQueries.RequiresFact(ProgramTrackedFact fact) => ProgramDependencies.RequiresFact(fact);
    bool IProgramContentQueries.UsesDrawPhaseDrawCountHandLimit => ProgramDependencies.UsesDrawPhaseDrawCountHandLimit;
    bool IProgramContentQueries.UsesSkillStatePhaseEnds => ProgramDependencies.UsesSkillStatePhaseEnds;
    bool IProgramContentQueries.UsesEqualNameLengthUse => ProgramDependencies.UsesEqualNameLengthUse;
    bool IProgramContentQueries.HasCardPolicy(SkillProgramCardPolicyKind policy) => ProgramDependencies.HasCardPolicy(policy);
    string IProgramContentQueries.SkillName(string skillId) => GetSkill(skillId).Name;
    string? IProgramContentQueries.AuthorityName(string skillId) =>
        Skills.GetValueOrDefault(skillId)?.ProgramPresentation?.AuthorityName;
}
