#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianFengMobileShiEntries
{
    internal void AnswerMobileShiTianFeng(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMobileShiTianFeng(selected);
    internal void AssertShiTf(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertShiTf(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileShiTianFeng(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginMobileShiTianFeng(input);
    internal bool CanOfferMobileShiTianFeng(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferMobileShiTianFeng(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseShiTf(global::CardGame.Core.PendingDecision prompt) => _resolve().ChooseShiTf(prompt);
    internal void ContinueMobileShiTianFeng(long id) => _resolve().ContinueMobileShiTianFeng(id);
    internal bool ContinuePaidShiTf(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidShiTf(f);
    internal bool DrainShiTf(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiTf(f);
    internal void FinishShiTfStep(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiTfStep(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiTianFengPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().MobileShiTianFengPublicStates(owner, skillId);
    internal void PublishShiTf(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiTf(f);
    internal bool ResumeShiTf(long id) => _resolve().ResumeShiTf(id);
    internal void ReturnShiTfDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShiTfDraw(f, outcome);
    internal bool ReturnShiTfMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiTfMovement(f);
    internal int ShiTfBackwaters(int owner) => _resolve().ShiTfBackwaters(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiTfChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiTfChoices(f);
    internal bool ShiTfMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShiTfMovement(f, effect, p);
    internal bool ShiTfNobodyDying() => _resolve().ShiTfNobodyDying();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MobileShiTianFengGiftEvent> ShiTfOutstandingGifts(int owner, string instance, string hash) => _resolve().ShiTfOutstandingGifts(owner, instance, hash);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MobileShiTianFengDebtIssuedEvent> ShiTfUseDebts(int owner, string instance, string hash, long action, int actor) => _resolve().ShiTfUseDebts(owner, instance, hash, action, actor);
}
