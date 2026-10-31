#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiDianMobileEntries
{
    internal SkillProgramStepOutcome BeginMobileLiDianWangxi(ProgramSkillFrame supplied) => _resolve().BeginMobileLiDianWangxi(supplied);
    internal void ContinueMobileLiDianWangxi(long id) => _resolve().ContinueMobileLiDianWangxi(id);
    internal bool ExactMobileLiDianDamage(ProgramSkillFrame f, scoped out ProgramSkillWindowContext c) => _resolve().ExactMobileLiDianDamage(f, out c);
    internal bool KeepPaidMobileLiDianWangxi(ProgramSkillFrame f) => _resolve().KeepPaidMobileLiDianWangxi(f);
    internal bool ProveMobileLiDianWangxiRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ProveMobileLiDianWangxiRecovery(f, recovery);
    internal bool ResumeMobileLiDianWangxi(long id) => _resolve().ResumeMobileLiDianWangxi(id);
    internal void ReturnMobileLiDianWangxiDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnMobileLiDianWangxiDraw(f, outcome);
    internal bool ReturnMobileLiDianWangxiMovement(ProgramSkillFrame f) => _resolve().ReturnMobileLiDianWangxiMovement(f);
    internal bool ValidMobileLiDianWangxi(ProgramSkillFrame f) => _resolve().ValidMobileLiDianWangxi(f);
}
