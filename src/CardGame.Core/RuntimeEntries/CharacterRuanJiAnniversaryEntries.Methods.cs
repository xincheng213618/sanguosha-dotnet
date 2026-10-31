#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuanJiAnniversaryEntries
{
    internal global::CardGame.Core.PromptChoice AiRuanJiDiscard(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AiRuanJiDiscard(prompt, frame);
    internal void AnswerRuanJiDiscard(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerRuanJiDiscard(selected);
    internal bool BanUnavailableRuanJiConversion(global::CardGame.Core.ProgramConfiguredViewAsQuery q) => _resolve().BanUnavailableRuanJiConversion(q);
    internal bool BanUnavailableRuanJiSubmittedSource(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().BanUnavailableRuanJiSubmittedSource(q);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginRuanJi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginRuanJi(supplied, op);
    internal void BeginRuanJiWine(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginRuanJiWine(f);
    internal void CompleteRuanJiReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteRuanJiReveal(f);
    internal bool ContinuePaidRuanJiAfterSkillLoss(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidRuanJiAfterSkillLoss(f);
    internal void ContinueRuanJi(long id) => _resolve().ContinueRuanJi(id);
    internal bool DrainRuanJi(long id) => _resolve().DrainRuanJi(id);
    internal void FinishRuanJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishRuanJi(f);
    internal bool HasRuanJiShownRedUse(int owner, string instance, global::CardGame.Core.CardActionContext action) => _resolve().HasRuanJiShownRedUse(owner, instance, action);
    internal bool IsExactRuanJiVirtualBasicDraft(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramVirtualBasicDraft draft) => _resolve().IsExactRuanJiVirtualBasicDraft(f, draft);
    internal bool KeepsAnniversaryRuanJiAlcohol(int seat) => _resolve().KeepsAnniversaryRuanJiAlcohol(seat);
    internal void PublishRuanJiDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishRuanJiDiscard(f);
    internal void RecordRuanJiConvertedName(global::CardGame.Core.IGameEvent payload) => _resolve().RecordRuanJiConvertedName(payload);
    internal void RequireRuanJi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireRuanJi(f);
    internal bool ResumeRuanJi(long id) => _resolve().ResumeRuanJi(id);
    internal void ReturnRuanJiDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnRuanJiDraw(supplied, draw);
    internal bool ReturnRuanJiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnRuanJiMovement(f);
    internal global::CardGame.Core.RuanJiShownEntityAuthorizedEvent? RuanJiAuthorization(int owner, string instance, int cardId, bool red) => _resolve().RuanJiAuthorization(owner, instance, cardId, red);
    internal bool RuanJiConversionAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind) => _resolve().RuanJiConversionAvailable(owner, card, source, kind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> RuanJiDiscardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RuanJiDiscardChoices(f);
    internal bool RuanJiDiscardPaid(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RuanJiReceipt r) => _resolve().RuanJiDiscardPaid(f, r);
    internal global::CardGame.Core.Card[] RuanJiDiscardable(int owner) => _resolve().RuanJiDiscardable(owner);
    internal bool RuanJiIncoming(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().RuanJiIncoming(owner, context);
    internal bool RuanJiMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().RuanJiMovementProof(f, effect, pending);
    internal bool RuanJiNameAvailable(int owner, global::CardGame.Core.CardKind kind) => _resolve().RuanJiNameAvailable(owner, kind);
    internal bool RuanJiTriggerLegal(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().RuanJiTriggerLegal(candidate, trigger, context);
    internal global::CardGame.Core.CardActionContext? RuanJiWindowAction(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().RuanJiWindowAction(context);
    internal void ValidateRuanJiSubmittedConversion(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().ValidateRuanJiSubmittedConversion(q);
    internal void ValidateRuanJiWine(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateRuanJiWine(use);
}
