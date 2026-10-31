#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuTuGuOlEntries
{
    internal bool AdvanceWuTuGuChildren() => _resolve().AdvanceWuTuGuChildren();
    internal void AssertWuTuGuNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertWuTuGuNativeUse(use);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWuTuGu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginWuTuGu(supplied, op);
    internal bool CanOfferWuTuGu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferWuTuGu(c, t, context);
    internal void CaptureWuTuGuHanyongUse(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureWuTuGuHanyongUse(payload);
    internal void CaptureWuTuGuRanshangPayment(global::CardGame.Core.ProgramSkillHpLostEvent paid) => _resolve().CaptureWuTuGuRanshangPayment(paid);
    internal bool ContinuePaidWuTuGu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidWuTuGu(f);
    internal void ContinueWuTuGu(long id) => _resolve().ContinueWuTuGu(id);
    internal bool ResumeWuTuGu(long id) => _resolve().ResumeWuTuGu(id);
    internal bool ValidWuTuGuHanyongUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidWuTuGuHanyongUse(use);
    internal bool ValidWuTuGuRanshang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWuTuGuRanshang(f);
    internal bool WuTuGuEndingParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WuTuGuEndingParentMatches(f);
    internal bool WuTuGuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().WuTuGuFirstChild(f, child);
    internal int WuTuGuHanyongDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().WuTuGuHanyongDamageBonus(attack);
    internal global::CardGame.Core.WuTuGuHanyongQualifiedEvent? WuTuGuHanyongQualification(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().WuTuGuHanyongQualification(c, context);
    internal global::CardGame.Core.ProgramSkillFrame? WuTuGuObserverRoot() => _resolve().WuTuGuObserverRoot();
    internal bool HasWuTuGuCapability { get => _resolve().HasWuTuGuCapability; }
}
