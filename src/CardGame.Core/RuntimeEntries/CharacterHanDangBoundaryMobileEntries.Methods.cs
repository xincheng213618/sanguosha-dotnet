#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHanDangBoundaryMobileEntries
{
    internal bool AdvanceProgramAttackRangeAidChild() => _resolve().AdvanceProgramAttackRangeAidChild();
    internal void ContinueProgramAttackRangeAidPayment(long id) => _resolve().ContinueProgramAttackRangeAidPayment(id);
    internal bool KeepPaidProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepPaidProgramAttackRangeAid(frame);
    internal bool ProgramAttackRangeAidChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().ProgramAttackRangeAidChild(frame, child);
    internal bool ProgramAttackRangeAidDying() => _resolve().ProgramAttackRangeAidDying();
    internal global::CardGame.Core.CardMoveReason ProgramAttackRangeAidReason(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ProgramAttackRangeAidReason(frame);
    internal bool ProgramAttackRangeAidRecovery(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().ProgramAttackRangeAidRecovery(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? ProgramAttackRangeAidRoot() => _resolve().ProgramAttackRangeAidRoot();
    internal bool ProgramAttackRangeAidSuffix(int index) => _resolve().ProgramAttackRangeAidSuffix(index);
    internal global::CardGame.Core.Card[] ProgramAttackRangeAidWeapons(int seat) => _resolve().ProgramAttackRangeAidWeapons(seat);
    internal bool ResumeProgramAttackRangeAidPayment(long id) => _resolve().ResumeProgramAttackRangeAidPayment(id);
    internal bool ReturnProgramAttackRangeAidMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnProgramAttackRangeAidMovement(frame);
    internal void ReturnProgramAttackRangeAidNativeDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnProgramAttackRangeAidNativeDraw(frame, outcome);
    internal bool ValidProgramAttackRangeAid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidProgramAttackRangeAid(frame);
}
