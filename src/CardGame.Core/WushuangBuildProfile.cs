using System.Text.Json.Serialization;

namespace CardGame.Core;

/// <summary>Visible executable build traits, not a promise that a conditional bonus currently applies.</summary>
public sealed partial record WushuangBuildProfile(double InjuryDrawPerDamagePoint, double InjuryRecoverPerDamagePoint,
    double SlashBurstBonus, bool SlashSealsTargetCards, double ProactiveHpCost)
{
    internal static WushuangBuildProfile Read(IEnumerable<SkillProgram> programs)
    {
        double draw = 0, recover = 0, burst = 0, hpCost = 0;
        var seals = false;
        var chainSynergy = false;
        foreach (var program in programs.DistinctBy(p => p.Id))
        {
            chainSynergy |= ProgramMayBenefitFromChain(program);
            foreach (var trigger in program.Triggers)
            {
                // Per-event damage and dying rescue do not establish a safe
                // per-point injury return. Unknown conditions are conservative.
                if (trigger.Window == SkillProgramTriggerWindow.AfterDamageApplied &&
                    trigger.Subject == SkillProgramTriggerSubject.Owner &&
                    trigger.DamageOccurrence == SkillProgramDamageOccurrence.PerDamagePoint &&
                    trigger.UsageScope is null && trigger.UsageLimit is null && trigger.DynamicUsageLimit is null && trigger.NamedUsageGroup is null &&
                    trigger.Condition.Kind == SkillProgramTriggerConditionKind.Always)
                {
                    foreach (var effect in trigger.Effects.Where(DirectOwnerAmount))
                    {
                        if (effect.Op == SkillProgramEffectOp.Draw) draw += effect.Amount;
                        if (effect.Op == SkillProgramEffectOp.Recover) recover += effect.Amount;
                    }
                    // GiveBoundCard is an optional distribution in the native
                    // program; retaining Yiji's drawn cards remains legal in 1v1.
                }
                if (trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor &&
                    trigger.Window is SkillProgramTriggerWindow.SlashBeforeResponse or
                        SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardUseCommitted &&
                    (trigger.CardKinds.Count == 0 || trigger.CardKinds.Any(IsSlash)) &&
                    trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.HoldTargetCards &&
                        effect.CardOwnerRef?.Kind == ProgramParticipantRef.EventTarget)) seals = true;
            }
            burst += program.DamageModifiers.Where(modifier => !modifier.CapsToAmount && modifier.Amount > 0 &&
                modifier.SourceScope is SkillProgramDamageModifierSourceScope.OwnerUsed or SkillProgramDamageModifierSourceScope.DamageSource &&
                (modifier.CardKinds.Count == 0 || modifier.CardKinds.Any(IsSlash))).Sum(modifier => (double)modifier.Amount);
            foreach (var activation in program.Activations)
                hpCost = Math.Max(hpCost, activation.Effects.Where(effect => DirectOwnerAmount(effect) &&
                    effect.Op is SkillProgramEffectOp.LoseHp or SkillProgramEffectOp.LoseHpUnclamped).Sum(effect => (double)effect.Amount));
        }
        return new(draw, recover, burst, seals, hpCost) { HasChainSynergy = chainSynergy };
    }

    private static bool DirectOwnerAmount(SkillProgramEffect effect) =>
        effect.Target == SkillProgramEffectTarget.Owner &&
        (effect.TargetReference is null || effect.TargetReference.Kind == ProgramParticipantRef.Owner) &&
        effect.Condition.Kind == SkillProgramConditionKind.Always && effect.NumberExpression is null && effect.Amount > 0;
    private static bool IsSlash(CardKind kind) => kind is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash;
}

public sealed partial record PlayerSnapshot
{
    /// <summary>Derived exclusively from this viewer's current visible skill definitions in Wushuang.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WushuangBuildProfile? WushuangBuild { get; init; }
}
