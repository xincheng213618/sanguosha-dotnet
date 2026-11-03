namespace CardGame.Core;

internal static class MouXiaoQiaoOlComposition
{
    [ProgramCatalogValidation(1620)]
    internal static void Validate(string path, SkillProgram p)
    {
        static bool Owned(SkillProgramEffectOp op) => op is SkillProgramEffectOp.ResolveMouXiaoQiaoMiluo or
            SkillProgramEffectOp.ResolveMouXiaoQiaoMiluoRoundEnd or SkillProgramEffectOp.ResolveMouXiaoQiaoJueyan;
        var ours = p.Id is MouXiaoQiaoOlRuntime.Miluo or MouXiaoQiaoOlRuntime.Jueyan;
        if (!ours)
        {
            if (p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => Owned(e.Op)))
                throw new InvalidOperationException(path + ": retained-Hand Jueyan and temporary Miluo require their actual native source bindings.");
            return;
        }
        if (p.ViewAs.Count != 0 || p.CardIdentities.Count != 0 || p.CardPolicies.Count != 0 || p.Modifiers.Count != 0 ||
            p.DamageModifiers.Count != 0 || p.BooleanStates.Count != 0 || p.SkillGrants.Count != 0 || p.Contributions.Count != 0 ||
            p.Activations is not [var a] || a.Effects.Count != 1 ||
            a.Effects[0].Target != SkillProgramEffectTarget.Owner ||
            a.Effects[0].Condition.Kind != SkillProgramConditionKind.Always ||
            a.MinTargets != 0 || a.MaxTargets != 0 || a.UsesPerTurn is not null || a.UsesPerGame is not null)
            throw new InvalidOperationException(path + ": Mou Xiao Qiao requires the complete native gift/use and real Round/reveal bindings.");
        if (p.Id == MouXiaoQiaoOlRuntime.Jueyan)
        {
            // Reward qualification is ordinary declared trigger data. Only the
            // still-native retained-use payment protocol belongs to this guard.
            if (a.Id != "retained-hand-use" || a.MinCards != 1 || a.MaxCards != 1 ||
                !a.SourceZones.SequenceEqual([CardZoneKind.Hand]) || a.UsesPerPhase is not null ||
                a.Effects[0].Op != SkillProgramEffectOp.ResolveMouXiaoQiaoJueyan)
                throw new InvalidOperationException(path + ": Jueyan changed its retained-Hand activation protocol.");
            return;
        }
        var valid = a.Id == "reveal-and-give" && a.MinCards == 0 && a.MaxCards == 0 && a.UsesPerPhase == 1 &&
            a.Effects[0].Op == SkillProgramEffectOp.ResolveMouXiaoQiaoMiluo && p.Triggers is [var t] &&
            t.Id == "round-end-consequence" && t.Window == SkillProgramTriggerWindow.RoundEnding &&
            !t.Optional && t.Subject == SkillProgramTriggerSubject.Owner && t.UsageLimit is null && t.MarkerCost is null &&
            t.Effects is [{ Op: SkillProgramEffectOp.ResolveMouXiaoQiaoMiluoRoundEnd,
                Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }];
        if (!valid) throw new InvalidOperationException(path + ": Mou Xiao Qiao changed a reserved payment, target or Round budget binding.");
    }
}
