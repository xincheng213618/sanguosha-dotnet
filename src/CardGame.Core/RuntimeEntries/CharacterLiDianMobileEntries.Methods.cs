#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiDianMobileEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileLiDianWangxi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMobileLiDianWangxi(supplied);
    internal void ContinueMobileLiDianWangxi(long id) => _resolve().ContinueMobileLiDianWangxi(id);
    internal bool ExactMobileLiDianDamage(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().ExactMobileLiDianDamage(f, out c);
    internal bool KeepPaidMobileLiDianWangxi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMobileLiDianWangxi(f);
    internal bool ProveMobileLiDianWangxiRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ProveMobileLiDianWangxiRecovery(f, recovery);
    internal bool ResumeMobileLiDianWangxi(long id) => _resolve().ResumeMobileLiDianWangxi(id);
    internal void ReturnMobileLiDianWangxiDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMobileLiDianWangxiDraw(f, outcome);
    internal bool ReturnMobileLiDianWangxiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileLiDianWangxiMovement(f);
    internal bool ValidMobileLiDianWangxi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMobileLiDianWangxi(f);
}
