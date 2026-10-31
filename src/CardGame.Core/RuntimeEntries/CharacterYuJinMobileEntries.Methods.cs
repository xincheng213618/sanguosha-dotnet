#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJinMobileEntries
{
    internal global::CardGame.Core.PromptChoice AiMobileYuJinChoice(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AiMobileYuJinChoice(decision, frame);
    internal void AnswerMobileYuJinChoice(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMobileYuJinChoice(choice);
    internal void AssertMobileYuJinTransfer(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertMobileYuJinTransfer(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileYuJinChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginMobileYuJinChoice(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileYuJinTransfer(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginMobileYuJinTransfer(supplied, effect);
    internal bool ExactMobileYuJinParent(global::CardGame.Core.ProgramSkillFrame frame, bool ending) => _resolve().ExactMobileYuJinParent(frame, ending);
    internal bool HasMobileYuJinPile(int seat, string instance) => _resolve().HasMobileYuJinPile(seat, instance);
    internal bool KeepMobileYuJinPaidTransfer(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepMobileYuJinPaidTransfer(frame);
    internal bool MatchesMobileYuJinChoiceResult(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramChoiceResultBinding binding) => _resolve().MatchesMobileYuJinChoiceResult(frame, binding);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileYuJinChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MobileYuJinChoices(frame);
    internal bool MobileYuJinConversionUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().MobileYuJinConversionUnavailable(query);
    internal bool MobileYuJinPaidMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().MobileYuJinPaidMovement(frame, effect, pending);
    internal global::CardGame.Core.PublicPersistentPileSource? MobileYuJinPile(int seat, string instance) => _resolve().MobileYuJinPile(seat, instance);
    internal bool MobileYuJinPreparationHasCards(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MobileYuJinPreparationHasCards(candidate, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileYuJinStorePayments(global::CardGame.Core.ProgramSkillFrame frame, int target) => _resolve().MobileYuJinStorePayments(frame, target);
    internal bool MobileYuJinSubmittedConversionUnavailable(global::CardGame.Core.ProgramViewAsLegalityQuery query) => _resolve().MobileYuJinSubmittedConversionUnavailable(query);
    internal global::CardGame.Core.SkillProgramStepOutcome PayMobileYuJinTransfer(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffectOp operation, global::CardGame.Core.PublicPersistentPileSource pile, int[] ids, global::CardGame.Core.CardLocation[] from) => _resolve().PayMobileYuJinTransfer(frame, operation, pile, ids, from);
    internal void PublishMobileYuJinChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishMobileYuJinChoice(frame);
    internal bool ResumeMobileYuJinTransfer(long id) => _resolve().ResumeMobileYuJinTransfer(id);
    internal bool ReturnMobileYuJinMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnMobileYuJinMovement(frame);
    internal bool ValidMobileYuJinChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidMobileYuJinChoice(frame);
    internal bool ValidMobileYuJinTransfer(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidMobileYuJinTransfer(frame);
}
