
namespace CardGame.Core;

[ProgramRuntimeModule]
internal static class ProgramPindianSelectionRules
{
    // A selected contest card supplies the source in these activation shapes.
    // Other activations retain their declared physical-card requirements.
    internal static bool UsesSelectedHandSource(SkillProgramActivation activation) =>
        activation.MinCards == 1 && activation.MaxCards == 1 && activation.MinTargets == 1 &&
        activation.TargetKind is (SkillProgramTargetKind.OtherLivingWithHand or SkillProgramTargetKind.OtherLivingWuFactionWithHand or
            SkillProgramTargetKind.OtherLivingWithHandHpGreaterThanOwner) && activation.CardCountExpression is null &&
        activation.SourceZones.SequenceEqual([CardZoneKind.Hand]) && activation.MarkerCost is null &&
        !activation.SelectedCardsSameSuit && !activation.SelectedCardsDistinctSuits && activation.CardKinds.Count == 0 &&
        activation.CardSuits.Count == 0 && activation.CardCategories.Count == 0 && activation.EquipmentSlots.Count == 0 &&
        (activation.MaxTargets == 3 && activation.Effects is [{ Op: SkillProgramEffectOp.StartSimultaneousPindian,
            Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] ||
         activation.MaxTargets == 1 && activation.Effects.Count > 0 && activation.Effects[0] is { Op: SkillProgramEffectOp.Pindian,
            Target: SkillProgramEffectTarget.SelectedTarget, Condition.Kind: SkillProgramConditionKind.Always } &&
         activation.Effects.Skip(1).All(e => e.NumberExpression is null &&
            e.Condition.Kind is (SkillProgramConditionKind.PindianWon or SkillProgramConditionKind.PindianNotWon) &&
            e.Op is SkillProgramEffectOp.GrantTurnCardTargetRestriction or
            SkillProgramEffectOp.GrantTurnCardActionProhibition or SkillProgramEffectOp.GrantTurnRuleModifier or
            SkillProgramEffectOp.SelectTarget or SkillProgramEffectOp.Damage or SkillProgramEffectOp.GrantNextActualUseTargetAdjustment or
            SkillProgramEffectOp.SetBooleanState or SkillProgramEffectOp.GrantDirectedTurnCardPolicy or
            SkillProgramEffectOp.GrantTurnOriginalTargetAddition or SkillProgramEffectOp.GrantTurnHandLimitCardKindExemption));

    internal static bool SelectsPindianOpponent(IReadOnlyList<SkillProgramEffect> instructions, int index) =>
        index >= 0 && index + 1 < instructions.Count && instructions[index] is
        { Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.OtherLivingWithHand, Zones.Count: 0 } &&
        instructions[index + 1] is { Op: SkillProgramEffectOp.StartPindian, Target: SkillProgramEffectTarget.Owner,
            Condition.Kind: SkillProgramConditionKind.Always, OpponentReference.Kind: ProgramParticipantRef.SelectedTarget };
}
