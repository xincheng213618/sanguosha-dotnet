#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJinMobileEntries
{
    internal PromptChoice AiMobileYuJinChoice(PendingDecision decision, ProgramSkillFrame frame) => _resolve().AiMobileYuJinChoice(decision, frame);
    internal void AnswerMobileYuJinChoice(PromptChoice choice) => _resolve().AnswerMobileYuJinChoice(choice);
    internal SkillProgramStepOutcome BeginMobileYuJinChoice(ProgramSkillFrame frame) => _resolve().BeginMobileYuJinChoice(frame);
    internal SkillProgramStepOutcome BeginMobileYuJinTransfer(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginMobileYuJinTransfer(supplied, effect);
    internal bool ExactMobileYuJinParent(ProgramSkillFrame frame, bool ending) => _resolve().ExactMobileYuJinParent(frame, ending);
    internal bool HasMobileYuJinPile(int seat, string instance) => _resolve().HasMobileYuJinPile(seat, instance);
    internal bool KeepMobileYuJinPaidTransfer(ProgramSkillFrame frame) => _resolve().KeepMobileYuJinPaidTransfer(frame);
    internal IReadOnlyList<PromptChoice> MobileYuJinChoices(ProgramSkillFrame frame) => _resolve().MobileYuJinChoices(frame);
    internal bool MobileYuJinConversionUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().MobileYuJinConversionUnavailable(query);
    internal PublicPersistentPileSource? MobileYuJinPile(int seat, string instance) => _resolve().MobileYuJinPile(seat, instance);
    internal bool MobileYuJinPreparationHasCards(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().MobileYuJinPreparationHasCards(candidate, context);
    internal IReadOnlyList<PromptChoice> MobileYuJinStorePayments(ProgramSkillFrame frame, int target) => _resolve().MobileYuJinStorePayments(frame, target);
    internal bool MobileYuJinSubmittedConversionUnavailable(ProgramViewAsLegalityQuery query) => _resolve().MobileYuJinSubmittedConversionUnavailable(query);
    internal SkillProgramStepOutcome PayMobileYuJinTransfer(ProgramSkillFrame frame, SkillProgramEffectOp operation, PublicPersistentPileSource pile, int[] ids, CardLocation[] from) => _resolve().PayMobileYuJinTransfer(frame, operation, pile, ids, from);
    internal void PublishMobileYuJinChoice(ProgramSkillFrame frame) => _resolve().PublishMobileYuJinChoice(frame);
    internal bool ResumeMobileYuJinTransfer(long id) => _resolve().ResumeMobileYuJinTransfer(id);
    internal bool ReturnMobileYuJinMovement(ProgramSkillFrame frame) => _resolve().ReturnMobileYuJinMovement(frame);
    internal bool ValidMobileYuJinChoice(ProgramSkillFrame frame) => _resolve().ValidMobileYuJinChoice(frame);
    internal bool ValidMobileYuJinTransfer(ProgramSkillFrame frame) => _resolve().ValidMobileYuJinTransfer(frame);
}
