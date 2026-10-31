#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianFengMobileShiEntries
{
    internal void AnswerMobileShiTianFeng(PromptChoice selected) => _resolve().AnswerMobileShiTianFeng(selected);
    internal SkillProgramStepOutcome BeginMobileShiTianFeng(ProgramSkillFrame input) => _resolve().BeginMobileShiTianFeng(input);
    internal bool CanOfferMobileShiTianFeng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferMobileShiTianFeng(candidate, trigger, context);
    internal PromptChoice ChooseShiTf(PendingDecision prompt) => _resolve().ChooseShiTf(prompt);
    internal void ContinueMobileShiTianFeng(long id) => _resolve().ContinueMobileShiTianFeng(id);
    internal bool ContinuePaidShiTf(ProgramSkillFrame f) => _resolve().ContinuePaidShiTf(f);
    internal bool DrainShiTf(ProgramSkillFrame f) => _resolve().DrainShiTf(f);
    internal void FinishShiTfStep(ProgramSkillFrame f) => _resolve().FinishShiTfStep(f);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiTianFengPublicStates(CharacterState owner, string skillId) => _resolve().MobileShiTianFengPublicStates(owner, skillId);
    internal void PublishShiTf(ProgramSkillFrame f) => _resolve().PublishShiTf(f);
    internal bool ResumeShiTf(long id) => _resolve().ResumeShiTf(id);
    internal void ReturnShiTfDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnShiTfDraw(f, outcome);
    internal bool ReturnShiTfMovement(ProgramSkillFrame f) => _resolve().ReturnShiTfMovement(f);
    internal int ShiTfBackwaters(int owner) => _resolve().ShiTfBackwaters(owner);
    internal IReadOnlyList<PromptChoice> ShiTfChoices(ProgramSkillFrame f) => _resolve().ShiTfChoices(f);
    internal bool ShiTfNobodyDying() => _resolve().ShiTfNobodyDying();
    internal IEnumerable<MobileShiTianFengGiftEvent> ShiTfOutstandingGifts(int owner, string instance, string hash) => _resolve().ShiTfOutstandingGifts(owner, instance, hash);
    internal IEnumerable<MobileShiTianFengDebtIssuedEvent> ShiTfUseDebts(int owner, string instance, string hash, long action, int actor) => _resolve().ShiTfUseDebts(owner, instance, hash, action, actor);
}
