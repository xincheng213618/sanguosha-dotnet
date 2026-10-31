#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterRuanJiAnniversaryEntries
{
    internal PromptChoice AiRuanJiDiscard(PendingDecision prompt, ProgramSkillFrame frame) => _resolve().AiRuanJiDiscard(prompt, frame);
    internal void AnswerRuanJiDiscard(PromptChoice selected) => _resolve().AnswerRuanJiDiscard(selected);
    internal bool BanUnavailableRuanJiConversion(ProgramConfiguredViewAsQuery q) => _resolve().BanUnavailableRuanJiConversion(q);
    internal bool BanUnavailableRuanJiSubmittedSource(ProgramViewAsLegalityQuery q) => _resolve().BanUnavailableRuanJiSubmittedSource(q);
    internal SkillProgramStepOutcome BeginRuanJi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginRuanJi(supplied, op);
    internal void BeginRuanJiWine(ProgramSkillFrame f) => _resolve().BeginRuanJiWine(f);
    internal void CompleteRuanJiReveal(ProgramSkillFrame f) => _resolve().CompleteRuanJiReveal(f);
    internal bool ContinuePaidRuanJiAfterSkillLoss(ProgramSkillFrame f) => _resolve().ContinuePaidRuanJiAfterSkillLoss(f);
    internal void ContinueRuanJi(long id) => _resolve().ContinueRuanJi(id);
    internal bool DrainRuanJi(long id) => _resolve().DrainRuanJi(id);
    internal void FinishRuanJi(ProgramSkillFrame f) => _resolve().FinishRuanJi(f);
    internal bool HasRuanJiShownRedUse(int owner, string instance, CardActionContext action) => _resolve().HasRuanJiShownRedUse(owner, instance, action);
    internal bool KeepsAnniversaryRuanJiAlcohol(int seat) => _resolve().KeepsAnniversaryRuanJiAlcohol(seat);
    internal void PublishRuanJiDiscard(ProgramSkillFrame f) => _resolve().PublishRuanJiDiscard(f);
    internal void RecordRuanJiConvertedName(IGameEvent payload) => _resolve().RecordRuanJiConvertedName(payload);
    internal bool ResumeRuanJi(long id) => _resolve().ResumeRuanJi(id);
    internal void ReturnRuanJiDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnRuanJiDraw(supplied, draw);
    internal bool ReturnRuanJiMovement(ProgramSkillFrame f) => _resolve().ReturnRuanJiMovement(f);
    internal RuanJiShownEntityAuthorizedEvent? RuanJiAuthorization(int owner, string instance, int cardId, bool red) => _resolve().RuanJiAuthorization(owner, instance, cardId, red);
    internal bool RuanJiConversionAvailable(CharacterState owner, Card card, CardConversionSource source, CardKind kind) => _resolve().RuanJiConversionAvailable(owner, card, source, kind);
    internal IReadOnlyList<PromptChoice> RuanJiDiscardChoices(ProgramSkillFrame f) => _resolve().RuanJiDiscardChoices(f);
    internal Card[] RuanJiDiscardable(int owner) => _resolve().RuanJiDiscardable(owner);
    internal bool RuanJiIncoming(int owner, ProgramSkillWindowContext context) => _resolve().RuanJiIncoming(owner, context);
    internal bool RuanJiNameAvailable(int owner, CardKind kind) => _resolve().RuanJiNameAvailable(owner, kind);
    internal bool RuanJiTriggerLegal(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().RuanJiTriggerLegal(candidate, trigger, context);
    internal CardActionContext? RuanJiWindowAction(ProgramSkillWindowContext context) => _resolve().RuanJiWindowAction(context);
    internal void ValidateRuanJiSubmittedConversion(ProgramSubmittedUseQuery q) => _resolve().ValidateRuanJiSubmittedConversion(q);
    internal void ValidateRuanJiWine(CardUseFrame use) => _resolve().ValidateRuanJiWine(use);
}
