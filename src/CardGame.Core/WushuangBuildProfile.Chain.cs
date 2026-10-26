using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed partial record WushuangBuildProfile
{
    /// <summary>A conservative hint from currently owned executable programs, including acquired skills.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool HasChainSynergy { get; init; }

    private static bool ProgramMayBenefitFromChain(SkillProgram program) =>
        program.Modifiers.Any(modifier => ChainCondition(modifier.Condition) || modifier.CardKinds.Contains(CardKind.IronChain)) ||
        program.ViewAs.Any(rule => rule.OutputKind == CardKind.IronChain || rule.AllowChainedInput ||
            rule.ChainedStateCost is not null || ChainCondition(rule.Condition)) ||
        program.CardIdentities.Any(identity => identity.OutputKind == CardKind.IronChain) ||
        program.DamageModifiers.Any(modifier => modifier.Condition == SkillProgramDamageModifierCondition.ChainedFirePropagationOrigin) ||
        program.CardPolicies.Any(policy => policy.Kind is SkillProgramCardPolicyKind.ForceChained or SkillProgramCardPolicyKind.ChainedHandLimitAura) ||
        program.Activations.Any(activation => ChainTarget(activation.TargetKind) || ChainCondition(activation.Condition) ||
            activation.Effects.Any(ChainEffect)) ||
        program.Triggers.Any(trigger => trigger.CardKinds.Contains(CardKind.IronChain) || trigger.Effects.Any(ChainEffect) ||
            // Generic trick/card-use rewards (for example drawing after a trick)
            // can make a one-target Iron Chain useful without propagation.
            trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor &&
            trigger.Window is SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardUseCompleted or
                SkillProgramTriggerWindow.CardUseTargetsFinalized &&
            (trigger.CardKinds.Count == 0 || trigger.CardKinds.Contains(CardKind.IronChain)) &&
            (trigger.CardCategories.Count == 0 || trigger.CardCategories.Any(category =>
                category is SkillProgramCardCategory.Trick or SkillProgramCardCategory.InstantTrick)));

    private static bool ChainCondition(SkillProgramCondition condition) => condition.Kind == SkillProgramConditionKind.Chained ||
        condition.Children.Any(ChainCondition);

    private static bool ChainTarget(SkillProgramTargetKind kind) => kind is SkillProgramTargetKind.OtherLivingChained or
        SkillProgramTargetKind.AnyLivingChained or SkillProgramTargetKind.PublicTeamUnchainedEnemies;

    private static bool ChainEffect(SkillProgramEffect effect) => ChainCondition(effect.Condition) ||
        effect.Op is SkillProgramEffectOp.SetChainedState or SkillProgramEffectOp.SelectChainedByMarker or
            SkillProgramEffectOp.SetHellOthersChained or SkillProgramEffectOp.SetHellAllLivingChained or
            SkillProgramEffectOp.ResolveShiZhouYuFentaoRechain ||
        effect.TargetKind is { } target && ChainTarget(target) || effect.OutputKind == CardKind.IronChain ||
        effect.CardKinds.Contains(CardKind.IronChain) || effect.Options.Any(option => ChainCondition(option.Condition));
}
