namespace CardGame.Core;

internal static class ShiSunJunComposition
{
    [ProgramCatalogValidation(49200)] internal static void Validate(string path, SkillProgram p)
    {
        var activation = SkillProgramEffectOp.ResolveMobileShiXiongtu;
        var benefit = SkillProgramEffectOp.ResolveMobileShiXiongtuDamageBenefit;
        if (!p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op == activation || e.Op == benefit)) return;
        if (p.Id != "mobile:shi-xiongtu" || p.Activations is not [{ Id: "show-hand", MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1,
                TargetKind: SkillProgramTargetKind.OtherLivingWithHand, UsesPerPhase: 2, UsesPerTurn: null, UsesPerGame: null } a] ||
            a.Effects is not [{ Op: SkillProgramEffectOp.ResolveMobileShiXiongtu, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] ||
            p.Triggers is not [{ Id: "other-damage", Window: SkillProgramTriggerWindow.AfterDamageApplied, Subject: SkillProgramTriggerSubject.DamageSource,
                DamageOccurrence: SkillProgramDamageOccurrence.PerDamage, Optional: false, IsSkillInvocation: false } t] ||
            t.Effects is not [{ Op: SkillProgramEffectOp.ResolveMobileShiXiongtuDamageBenefit, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] ||
            p.BooleanStates is not [{ Id: "armed", InitialValue: false, Visibility: SkillProgramStateVisibility.Public, ResetScope: SkillProgramStateResetScope.Turn }] ||
            p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.CardPolicies.Count != 0)
            throw new InvalidOperationException(path + ": Xiongtu requires its actual-Play two-cap native gate, one turn listener and shared turn-reset state.");
    }
}
