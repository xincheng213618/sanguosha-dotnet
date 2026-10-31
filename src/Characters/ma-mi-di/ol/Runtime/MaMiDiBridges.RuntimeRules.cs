namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_ma_mi_di_ol_MaMiDiBridges
{
    internal static bool? MaMiDiActionColor(CardActionContext a) => a.PhysicalCards.Count == 0 ? RuntimeRules_Core_PublicPileColors.SuitColor(a.EffectiveSuit) : a.EffectiveIsRed ?? RuntimeRules_Core_PublicPileColors.SuitColor(a.EffectiveSuit);

    internal static bool MaMiDiCandidateMatches(ProgramTriggerCandidate a, ProgramTriggerCandidate b) => a.OwnerSeat == b.OwnerSeat && a.SkillId == b.SkillId &&
        a.BindingId == b.BindingId && a.SkillInstanceId == b.SkillInstanceId && a.GameplayHash == b.GameplayHash && a.OccurrenceIndex == b.OccurrenceIndex;
}
