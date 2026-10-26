namespace CardGame.Core;

public sealed partial class SimpleAiBrain
{
    private static double ShopBuildRecoveryBonus(SkillProgram[] owned)
    {
        var build = WushuangBuildProfile.Read(owned);
        // A recoverable HP can pay for another survivable injury. This is a
        // resource reserve value, never permission to take lethal damage.
        return Math.Min(10, build.InjuryDrawPerDamagePoint * 3 + build.InjuryRecoverPerDamagePoint * 2);
    }

    private static double ShopSlashBuildBonus(PlayerSnapshot self, PlayerSnapshot? opponent, SkillProgram[] owned)
    {
        var build = WushuangBuildProfile.Read(owned);
        var damage = self.WushuangTacticIds?.Contains(2210) == true || opponent?.IncomingDamageCap == 1
            ? 0 : build.SlashBurstBonus;
        var seals = build.SlashSealsTargetCards && opponent is not null && opponent.HandCount + opponent.Equipment.Count > 0 ? 1 : 0;
        return Math.Min(8, damage * 3 + seals * 4);
    }

    private static double ShopBuildInteractionValue(PlayerSnapshot self, PlayerSnapshot? opponent, SkillProgram[] owned)
    {
        var build = WushuangBuildProfile.Read(owned);
        var healing = Math.Min(3, self.Hand.Count(card => card.Kind == CardKind.Peach) +
            owned.Sum(ShopReusableRecovery));
        var sustain = Math.Min(3, build.InjuryDrawPerDamagePoint) * (healing + Math.Min(1, build.InjuryRecoverPerDamagePoint)) * 2;
        var burst = ShopSlashBuildBonus(self, opponent, owned) * Math.Min(2, self.Hand.Count(card => IsSlashCard(card.Kind))) * .75;
        var paidHp = self.Hp > build.ProactiveHpCost && build.ProactiveHpCost > 0 ? Math.Min(2, build.ProactiveHpCost) * healing * 1.5 : 0;
        return Math.Min(30, sustain + burst + paidHp);
    }

    private static double ShopSkillBuildFit(SkillProgram program, PlayerSnapshot self, PlayerSnapshot? opponent,
        SkillProgram[] owned, double future)
    {
        // `owned` excludes a proposed replacement. Price both adding and
        // retaining the skill against exactly that same remaining build.
        var combined = owned.Append(program).ToArray();
        var interaction = ShopBuildInteractionValue(self, opponent, combined) - ShopBuildInteractionValue(self, opponent, owned);
        var recovery = ShopReusableRecovery(program);
        return Math.Min(1, future) * (interaction + recovery * ShopBuildRecoveryBonus(owned) * .5);
    }

    private static bool ShopRecoveryEffect(SkillProgramEffect effect) => effect.Op is
        SkillProgramEffectOp.Recover or SkillProgramEffectOp.RecoverTo or SkillProgramEffectOp.RecoverToMaximum;

    private static double ShopReusableRecovery(SkillProgram program)
    {
        // Acquisition rewards and already limited one-shot heals cannot fund
        // another injury merely by retaining the spent skill in a slot.
        var reusable = program.Activations.Any(activation => activation.UsesPerGame is null && activation.Effects.Any(ShopRecoveryEffect)) ||
            program.Triggers.Any(trigger => trigger.Window != SkillProgramTriggerWindow.SkillsChanged &&
                trigger.UsageScope != SkillUsageScope.Game && trigger.Effects.Any(ShopRecoveryEffect));
        return reusable ? ReadDraftSkillBenefits(program, null).Recovery : 0;
    }

    private static double ShopTriggerFrequency(SkillProgramTrigger trigger, PlayerSnapshot self, SkillProgram[] owned, double future)
    {
        if (trigger.Window == SkillProgramTriggerWindow.SkillsChanged) return 1;
        var save = self.Hand.Any(card => card.Kind is CardKind.Peach or CardKind.Alcohol) ||
            owned.Any(program => program.Triggers.Any(rescue => rescue.Window == SkillProgramTriggerWindow.SelfDyingResponse &&
                rescue.Effects.Any(ShopRecoveryEffect)));
        if (trigger.Window is SkillProgramTriggerWindow.DyingEntering or SkillProgramTriggerWindow.DyingEntered or
            SkillProgramTriggerWindow.DyingExited or SkillProgramTriggerWindow.SelfDyingResponse or SkillProgramTriggerWindow.DyingResponse)
        {
            // Entering dying is exceptional, and exiting it additionally needs
            // a rescue. Neither event is a routine draw once per future turn.
            var opportunity = self.Hp <= 1 ? .35 : self.Hp == 2 ? .12 : .04;
            if (trigger.Window == SkillProgramTriggerWindow.DyingExited) opportunity *= save ? .8 : .15;
            return Math.Min(1, future) * opportunity;
        }
        if (trigger.Window is SkillProgramTriggerWindow.AfterDamageApplied or SkillProgramTriggerWindow.DamageAppliedBeforeDying)
            return future * .6 * (self.Hp <= 1 && !save ? .2 : 1);
        return future;
    }

    private static double ShopNativeBuildFit(int id, double value, PlayerSnapshot self, PlayerSnapshot? opponent, SkillProgram[] owned)
    {
        if (value <= 0 || !self.Hand.Any(card => IsSlashCard(card.Kind))) return 0;
        if (id is not (2042 or 2043 or 2045 or 2115 or 2150 or 2151 or 2152 or 2162 or 2163 or 2188 or 2189)) return 0;
        var build = WushuangBuildProfile.Read(owned);
        return build.SlashSealsTargetCards && opponent is { HandCount: > 0 } ? Math.Min(6, value * .35) : 0;
    }
}
