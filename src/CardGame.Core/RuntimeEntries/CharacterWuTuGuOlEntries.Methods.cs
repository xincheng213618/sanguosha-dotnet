#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuTuGuOlEntries
{
    internal bool AdvanceWuTuGuChildren() => _resolve().AdvanceWuTuGuChildren();
    internal void AssertWuTuGuNativeUse(CardUseFrame use) => _resolve().AssertWuTuGuNativeUse(use);
    internal SkillProgramStepOutcome BeginWuTuGu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginWuTuGu(supplied, op);
    internal bool CanOfferWuTuGu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferWuTuGu(c, t, context);
    internal void CaptureWuTuGuHanyongUse(IGameEvent payload) => _resolve().CaptureWuTuGuHanyongUse(payload);
    internal void CaptureWuTuGuRanshangPayment(ProgramSkillHpLostEvent paid) => _resolve().CaptureWuTuGuRanshangPayment(paid);
    internal bool ContinuePaidWuTuGu(ProgramSkillFrame f) => _resolve().ContinuePaidWuTuGu(f);
    internal void ContinueWuTuGu(long id) => _resolve().ContinueWuTuGu(id);
    internal bool ResumeWuTuGu(long id) => _resolve().ResumeWuTuGu(id);
    internal bool ValidWuTuGuHanyongUse(CardUseFrame use) => _resolve().ValidWuTuGuHanyongUse(use);
    internal bool ValidWuTuGuRanshang(ProgramSkillFrame f) => _resolve().ValidWuTuGuRanshang(f);
    internal bool WuTuGuEndingParentMatches(ProgramSkillFrame f) => _resolve().WuTuGuEndingParentMatches(f);
    internal bool WuTuGuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WuTuGuFirstChild(f, child);
    internal int WuTuGuHanyongDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().WuTuGuHanyongDamageBonus(attack);
    internal WuTuGuHanyongQualifiedEvent? WuTuGuHanyongQualification(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().WuTuGuHanyongQualification(c, context);
    internal ProgramSkillFrame? WuTuGuObserverRoot() => _resolve().WuTuGuObserverRoot();
    internal bool HasWuTuGuCapability { get => _resolve().HasWuTuGuCapability; }
}
