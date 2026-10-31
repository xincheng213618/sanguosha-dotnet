namespace CardGame.Core;

// A query reads the supplied participant at the original evaluation stage.
// Empty bindings never capture a rule context or its optional dynamic facts.
internal interface IProgramNumericContributionQuery : IProgramSelectionQuery
{
    IReadOnlyList<IndexedSkillProgramModifier> Modifiers { get; }
    int RoundNumber { get; }
    int LivingFactionCount { get; }
    bool SupportsMultiplePublicPiles { get; }
    int PublicLivingFactionCount { get; }
    int PublicTeamEnemyCount { get; }
    int OtherPublicTeamAllyCount { get; }
    int OtherLivingPlayerCount { get; }
    int LivingFemaleCount { get; }
    int CurrentTurnUsedHandSuitCount { get; }
    int CurrentTurnDrawPhaseDrawCount { get; }
    int CurrentRoundActualUseCount { get; }
    int EmptyCampaignSkillSlotCount { get; }
    IEnumerable<RuleQueryContribution> StrategicContributions { get; }
    IEnumerable<RuleQueryContribution> WuqianContributions { get; }
    IEnumerable<RuleQueryContribution> WushuangContributions { get; }
    IEnumerable<RuleQueryContribution> OtherPublicTeamContributions { get; }
    IEnumerable<RuleQueryContribution> MouSimaSlashContributions { get; }
    IEnumerable<RuleQueryContribution> HappyZhaoLanContributions { get; }
    IEnumerable<RuleQueryContribution> ZhangXunSlashContributions { get; }
    IEnumerable<RuleQueryContribution> HaoMengSlashContributions { get; }
    IEnumerable<RuleQueryContribution> HanSongContributions { get; }
    IEnumerable<RuleQueryContribution> CaoZhangPhaseContributions { get; }
    IEnumerable<RuleQueryContribution> CaoZhenSidiContributions { get; }
    IEnumerable<RuleQueryContribution> ModeContributions { get; }
    bool SchoolQualified(string skillId);
    bool SteadyHandQualified(string skillId);
    bool IsLordSkill(string skillId);
    bool HasLordRoleQualification(string skillId, string instanceId);
    PlayerSkillContext CreateOwnerContext();
    int OwnedZoneCount(CardZoneKind zone);
    int PublicPileCount(string skillId, string instanceId);
    int OwnerMarkerCount(PlayerMarkerKind marker);
}

[ProgramRuntimeModule]
internal sealed class ProgramNumericContributionRuntime
{
    internal IReadOnlyList<RuleQueryContribution> Collect<TFacts>(TFacts facts, SkillRuleQuery query,
        CardKind? effectiveCardKind = null) where TFacts : IProgramNumericContributionQuery
    {
        var bindings = facts.Modifiers
            .Where(binding => facts.SchoolQualified(binding.Source.SkillId))
            .Where(binding => facts.SteadyHandQualified(binding.Source.SkillId))
            .Where(binding => binding.Modifier.RecipientScope == SkillRuleRecipientScope.Owner)
            .Where(binding => binding.Modifier.MinimumRound is not { } round || facts.RoundNumber >= round)
            .Where(binding => binding.Modifier.ValueExpression != SkillRuleValueExpression.OwnerMarkerCount ||
                !facts.IsLordSkill(binding.Source.SkillId) ||
                facts.HasLordRoleQualification(binding.Source.SkillId, binding.Source.SkillInstanceId)).ToArray();
        if (bindings.Length == 0) return facts.StrategicContributions
            .Concat(facts.WuqianContributions)
            .Concat(facts.WushuangContributions)
            .Concat(facts.ModeContributions)
            .Concat(facts.OtherPublicTeamContributions).Concat(facts.MouSimaSlashContributions)
            .Concat(facts.HappyZhaoLanContributions)
            .Concat(facts.ZhangXunSlashContributions)
            .Concat(facts.HaoMengSlashContributions)
            .Concat(facts.HanSongContributions)
            .Concat(facts.CaoZhangPhaseContributions)
            .Concat(facts.CaoZhenSidiContributions).ToArray();
        var context = new SkillProgramRuleContext(
            facts.CreateOwnerContext(),
            facts.LivingFactionCount,
            zone => facts.OwnedZoneCount(zone),
            effectiveCardKind,
            (skill, instance, zone) => zone == CardZoneKind.PublicPersistentPile && facts.SupportsMultiplePublicPiles
                ? facts.PublicPileCount(skill, instance)
                : facts.OwnedZoneCount(zone))
        {
            OwnerMarkerCount = bindings.Any(binding => binding.Modifier.QueryDependencies.Contains(SkillRuleQueryDependency.MarkerState))
                ? marker => facts.OwnerMarkerCount(marker) : null,
            PublicLivingFactionCount = bindings.Any(binding => binding.Modifier.ValueExpression == SkillRuleValueExpression.PublicLivingFactionCount)
                ? facts.PublicLivingFactionCount : null,
            PublicTeamEnemyCount = bindings.Any(binding => binding.Modifier.ValueExpression == SkillRuleValueExpression.PublicTeamEnemyCount)
                ? facts.PublicTeamEnemyCount : null,
            OtherPublicTeamAllyCount = bindings.Any(binding => binding.Modifier.ValueExpression == SkillRuleValueExpression.OtherPublicTeamAllyCount)
                ? facts.OtherPublicTeamAllyCount : null,
            OtherLivingPlayerCount = bindings.Any(binding => binding.Modifier.ValueExpression == SkillRuleValueExpression.OtherLivingPlayerCount)
                ? facts.OtherLivingPlayerCount : null,
            LivingFemaleCount = bindings.Any(binding => binding.Modifier.ValueExpression == SkillRuleValueExpression.NegatedLivingFemaleCount)
                ? facts.LivingFemaleCount : null,
            CurrentTurnUsedHandSuitCount = bindings.Any(binding => binding.Modifier.ValueExpression == SkillRuleValueExpression.CurrentTurnUsedHandSuitCount)
                ? facts.CurrentTurnUsedHandSuitCount : null,
            CurrentTurnDrawPhaseDrawCount = bindings.Any(binding => binding.Modifier.ValueExpression == SkillRuleValueExpression.CurrentTurnDrawPhaseDrawCountHalfFloor)
                ? facts.CurrentTurnDrawPhaseDrawCount : null,
            CurrentRoundActualUseCount = bindings.Any(binding => binding.Modifier.ValueExpression == SkillRuleValueExpression.NegatedCurrentRoundActualUseCount)
                ? facts.CurrentRoundActualUseCount : null,
            EmptyCampaignSkillSlotCount = bindings.Any(binding => binding.Modifier.ValueExpression == SkillRuleValueExpression.EmptyCampaignSkillSlotCount)
                ? facts.EmptyCampaignSkillSlotCount : null
        };
        var programContributions = SkillProgramRules.CollectIndexedContributions(
            query,
            context,
            bindings);
        return programContributions.Concat(facts.StrategicContributions)
            .Concat(facts.WuqianContributions)
            .Concat(facts.WushuangContributions)
            .Concat(facts.ModeContributions)
            .Concat(facts.OtherPublicTeamContributions).Concat(facts.MouSimaSlashContributions)
            .Concat(facts.HappyZhaoLanContributions)
            .Concat(facts.ZhangXunSlashContributions)
            .Concat(facts.HaoMengSlashContributions)
            .Concat(facts.HanSongContributions)
            .Concat(facts.CaoZhangPhaseContributions)
            .Concat(facts.CaoZhenSidiContributions).ToArray();
    }
}
