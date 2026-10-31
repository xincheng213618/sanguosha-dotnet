namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_xin_pi_ol_XinPiProofs
{
    internal static CardConversionSource XinPiSource(ProgramSkillFrame f)=>new(f.SkillId,RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);

    internal static IReadOnlyList<int> XinPiMainTargets(CardUseFrame u)=>u.CardKind==CardKind.BorrowedSword?Array.AsReadOnly(u.Action!.TargetSeats.Where((_,i)=>i%2==0).ToArray()):u.TargetSeats;
}
