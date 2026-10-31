#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoMobileGodEntries
{
    internal PromptChoice AiMobileGuixin(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMobileGuixin(p, f);
    internal void AnswerMobileGuixin(PromptChoice chosen) => _resolve().AnswerMobileGuixin(chosen);
    internal SkillProgramStepOutcome BeginMobileGuixin(ProgramSkillFrame supplied) => _resolve().BeginMobileGuixin(supplied);
    internal bool DrainMobileGuixinChildren(ProgramSkillFrame f) => _resolve().DrainMobileGuixinChildren(f);
    internal void FinishMobileGuixin(ProgramSkillFrame f, bool cancelled) => _resolve().FinishMobileGuixin(f, cancelled);
    internal IReadOnlyList<PromptChoice> MobileGuixinChoices(ProgramSkillFrame f) => _resolve().MobileGuixinChoices(f);
    internal bool MobileGuixinPaymentTail(ProgramSkillFrame frame, MobileGuixinPayment payment, CardMovementRecord gain) => _resolve().MobileGuixinPaymentTail(frame, payment, gain);
    internal bool ResumeMobileGuixin(long id) => _resolve().ResumeMobileGuixin(id);
    internal bool RetainMobileGuixinPaid(ProgramSkillFrame f) => _resolve().RetainMobileGuixinPaid(f);
    internal bool ReturnMobileGuixinMovement(ProgramSkillFrame f) => _resolve().ReturnMobileGuixinMovement(f);
    internal bool ValidMobileGuixin(ProgramSkillFrame f) => _resolve().ValidMobileGuixin(f);
}
