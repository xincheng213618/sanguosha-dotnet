namespace CardGame.Core;

/// <summary>Derives this match's conversion names without changing source content identity or instructions.</summary>
internal static class ModeSkillProgramProjection
{
    private static readonly IReadOnlySet<CardKind> TraditionalProactiveOrdinaryTricks = new HashSet<CardKind>
    {
        CardKind.Duel, CardKind.DrawTwo, CardKind.BarbarianAssault, CardKind.ArrowBarrage,
        CardKind.PeachGarden, CardKind.FiveGrains, CardKind.Dismantlement, CardKind.Snatch,
        CardKind.FireAttack, CardKind.IronChain, CardKind.BorrowedSword
    };
    private static readonly IReadOnlySet<CardKind> TraditionalBasicKinds = new HashSet<CardKind>
        { CardKind.Slash, CardKind.Dodge, CardKind.Peach, CardKind.Alcohol };

    internal static SkillProgram Project(SkillProgram original, ModeCardRules cards)
    {
        var families = original.ViewAs.Where(rule => TraditionalProactiveOrdinaryTricks.Contains(rule.OutputKind))
            .GroupBy(ActivityViewAsFamily, StringComparer.Ordinal)
            .Where(group => TraditionalProactiveOrdinaryTricks.IsSubsetOf(group.Select(rule => rule.OutputKind)))
            .SelectMany(group => group).ToHashSet();
        var basicFamilies = !cards.ProjectsBasicConversionFamilies ? new HashSet<SkillProgramViewAs>() :
            original.ViewAs.Where(rule => TraditionalBasicKinds.Contains(rule.OutputKind))
                .GroupBy(BasicViewAsFamily, StringComparer.Ordinal)
                .Where(group => TraditionalBasicKinds.IsSubsetOf(group.Select(rule => rule.OutputKind)))
                .SelectMany(group => group).ToHashSet();
        var changed = false;
        var views = new List<SkillProgramViewAs>();
        foreach (var rule in original.ViewAs)
        {
            if (basicFamilies.Contains(rule) && cards.TryReplaceBasicConversion(rule.OutputKind, out var basicOutput))
            {
                changed = true;
                views.Add(CloneViewAs(rule, basicOutput, forResponse: false));
                continue;
            }
            if (!families.Contains(rule)) { views.Add(rule); continue; }
            var output = cards.ReplaceOrdinaryTrick(rule.OutputKind);
            if (output == rule.OutputKind) { views.Add(rule); continue; }
            changed = true;
            if (output is { } kind) views.Add(CloneViewAs(rule, kind));
        }
        // Keep the original batch -> conversion family -> added name order and exact binding suffixes.
        foreach (var batch in cards.AdditionalOrdinaryTricks.GroupBy(addition => addition.Batch))
        foreach (var family in families.GroupBy(ActivityViewAsFamily, StringComparer.Ordinal))
        foreach (var addition in batch)
        {
            var template = family.First();
            views.Add(CloneViewAs(template, addition.Kind, template.Id + addition.BindingSuffix));
            changed = true;
        }
        var triggers = original.Triggers.Select(trigger =>
        {
            if (!TraditionalProactiveOrdinaryTricks.IsSubsetOf(trigger.CardKinds)) return trigger;
            var kinds = trigger.CardKinds.Select(cards.ReplaceOrdinaryTrick).Where(kind => kind is not null)
                .Concat(cards.AdditionalOrdinaryTricks.Select(addition => (CardKind?)addition.Kind))
                .Select(kind => kind!.Value).Distinct().ToArray();
            if (kinds.SequenceEqual(trigger.CardKinds)) return trigger;
            changed = true;
            return global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ActivityOrdinaryTricks.CloneActivityTrigger(
                trigger, Array.AsReadOnly(kinds));
        }).ToArray();
        return !changed ? original : new SkillProgram(original.Id, original.Revision, original.GameplayHash,
            original.RuntimeVersion, original.MinimumRulesVersion, original.Modifiers, Array.AsReadOnly(views.ToArray()),
            original.Activations, Array.AsReadOnly(triggers), original.Contributions, original.CardIdentities,
            original.BooleanStates, original.DamageModifiers, original.CardPolicies, original.LordSkillProjection,
            original.CannotChallengeDeclarations, original.HpSkillGrants, original.EquipmentSuitSkillGrants);
    }

    private static string ActivityViewAsFamily(SkillProgramViewAs rule) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ActivityOrdinaryTricks.ActivityViewAsFamily(rule);
    private static string BasicViewAsFamily(SkillProgramViewAs rule) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DragonBoatBasicConversions.DragonBoatBasicViewAsFamily(rule);
    private static SkillProgramViewAs CloneViewAs(SkillProgramViewAs rule, CardKind kind,
        string? bindingId = null, bool? forResponse = null) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ActivityOrdinaryTricks.CloneActivityViewAs(rule, kind, bindingId, forResponse);
}
