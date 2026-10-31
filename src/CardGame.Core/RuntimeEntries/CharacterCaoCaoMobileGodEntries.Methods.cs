#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoCaoMobileGodEntries
{
    internal global::CardGame.Core.PromptChoice AiMobileGuixin(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMobileGuixin(p, f);
    internal void AnswerMobileGuixin(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerMobileGuixin(chosen);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileGuixin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMobileGuixin(supplied);
    internal bool DrainMobileGuixinChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMobileGuixinChildren(f);
    internal void FinishMobileGuixin(global::CardGame.Core.ProgramSkillFrame f, bool cancelled) => _resolve().FinishMobileGuixin(f, cancelled);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileGuixinChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileGuixinChoices(f);
    internal bool MobileGuixinMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().MobileGuixinMovementProof(f, effect, pending);
    internal bool MobileGuixinPaymentTail(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.MobileGuixinPayment payment, global::CardGame.Core.CardMovementRecord gain) => _resolve().MobileGuixinPaymentTail(frame, payment, gain);
    internal bool ResumeMobileGuixin(long id) => _resolve().ResumeMobileGuixin(id);
    internal bool RetainMobileGuixinPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainMobileGuixinPaid(f);
    internal bool ReturnMobileGuixinMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileGuixinMovement(f);
    internal bool ValidMobileGuixin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMobileGuixin(f);
}
