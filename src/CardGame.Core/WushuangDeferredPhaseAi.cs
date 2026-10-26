namespace CardGame.Core;

/// <summary>Public consequences of an executable native phase grant; no future private card is inspected.</summary>
internal static class WushuangDeferredPhaseAi
{
    internal static SkillProgramAiHint Describe(SkillProgramActivation activation, SkillProgramAiHint hint)
    {
        if (activation.Effects.Count != 2 || activation.Effects[0] is not
                { Op: SkillProgramEffectOp.ArmWushuangYinglue, Condition.Kind: SkillProgramConditionKind.Always }) return hint;
        bool Selected(SkillProgramEffect effect) => effect.Condition.Kind == SkillProgramConditionKind.Always &&
            (effect.TargetReference?.Kind == ProgramParticipantRef.SelectedTarget || effect.TargetReference is null &&
                effect.Target == SkillProgramEffectTarget.SelectedTarget);
        if (Selected(activation.Effects[1]) && activation.Effects[1] is { Op: SkillProgramEffectOp.LoseHp, Amount: 1 })
            return hint with { TargetNextDrawBonus = 2 };
        if (Selected(activation.Effects[1]) && activation.Effects[1] is { Op: SkillProgramEffectOp.Draw, Amount: 2 })
            return hint with { TargetNextDiscardHandLimitPenalty = 2 };
        return hint;
    }
}
