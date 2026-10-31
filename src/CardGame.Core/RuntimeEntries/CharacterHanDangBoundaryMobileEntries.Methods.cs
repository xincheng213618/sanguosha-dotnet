#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHanDangBoundaryMobileEntries
{
    internal bool AdvanceProgramAttackRangeAidChild() => _resolve().AdvanceProgramAttackRangeAidChild();
    internal void ContinueProgramAttackRangeAidPayment(long id) => _resolve().ContinueProgramAttackRangeAidPayment(id);
    internal bool KeepPaidProgramAttackRangeAid(ProgramSkillFrame frame) => _resolve().KeepPaidProgramAttackRangeAid(frame);
    internal bool ProgramAttackRangeAidChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().ProgramAttackRangeAidChild(frame, child);
    internal bool ProgramAttackRangeAidDying() => _resolve().ProgramAttackRangeAidDying();
    internal CardMoveReason ProgramAttackRangeAidReason(ProgramSkillFrame frame) => _resolve().ProgramAttackRangeAidReason(frame);
    internal bool ProgramAttackRangeAidRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame child) => _resolve().ProgramAttackRangeAidRecovery(frame, child);
    internal ProgramSkillFrame? ProgramAttackRangeAidRoot() => _resolve().ProgramAttackRangeAidRoot();
    internal bool ProgramAttackRangeAidSuffix(int index) => _resolve().ProgramAttackRangeAidSuffix(index);
    internal Card[] ProgramAttackRangeAidWeapons(int seat) => _resolve().ProgramAttackRangeAidWeapons(seat);
    internal bool ResumeProgramAttackRangeAidPayment(long id) => _resolve().ResumeProgramAttackRangeAidPayment(id);
    internal bool ReturnProgramAttackRangeAidMovement(ProgramSkillFrame frame) => _resolve().ReturnProgramAttackRangeAidMovement(frame);
    internal void ReturnProgramAttackRangeAidNativeDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome) => _resolve().ReturnProgramAttackRangeAidNativeDraw(frame, outcome);
    internal bool ValidProgramAttackRangeAid(ProgramSkillFrame frame) => _resolve().ValidProgramAttackRangeAid(frame);
}
