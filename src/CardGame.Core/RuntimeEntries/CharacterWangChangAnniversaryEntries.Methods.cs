#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangChangAnniversaryEntries
{
    internal void AnswerWangChang(PromptChoice choice) => _resolve().AnswerWangChang(choice);
    internal SkillProgramStepOutcome BeginWangChang(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginWangChang(supplied, operation);
    internal void BindWangChangSlash(IGameEvent fact) => _resolve().BindWangChangSlash(fact);
    internal bool CanActivateWangChang(ProgramActivationLegalityContext c) => _resolve().CanActivateWangChang(c);
    internal bool CanOfferWangChang(ProgramTriggerLegalityContext c) => _resolve().CanOfferWangChang(c);
    internal void CompleteWangChangSlash(long id) => _resolve().CompleteWangChangSlash(id);
    internal void ContinueWangChang(long id) => _resolve().ContinueWangChang(id);
    internal void FinishWangChang(ProgramSkillFrame f) => _resolve().FinishWangChang(f);
    internal bool IsWangChangSlash(CardUseFrame use) => _resolve().IsWangChangSlash(use);
    internal bool KeepWangChangPaid(ProgramSkillFrame f) => _resolve().KeepWangChangPaid(f);
    internal void PayWangChangDiscard(ProgramSkillFrame f, IReadOnlyList<int> ids, int subject) => _resolve().PayWangChangDiscard(f, ids, subject);
    internal void PublishWangChang(ProgramSkillFrame f) => _resolve().PublishWangChang(f);
    internal bool ResumeWangChang(long id) => _resolve().ResumeWangChang(id);
    internal bool ReturnWangChangAttack(GameEngine.AttackCompletionReceipt receipt) => _resolve().ReturnWangChangAttack(receipt);
    internal void ReturnWangChangDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnWangChangDraw(supplied, outcome);
    internal bool ReturnWangChangMovement(ProgramSkillFrame f) => _resolve().ReturnWangChangMovement(f);
    internal void ReturnWangChangSlash(CardUseFrame use) => _resolve().ReturnWangChangSlash(use);
    internal bool ValidWangChang(ProgramSkillFrame f) => _resolve().ValidWangChang(f);
    internal void ValidateWangChangSlash(CardUseFrame use) => _resolve().ValidateWangChangSlash(use);
    internal PromptChoice WangChangAi(PendingDecision p, ProgramSkillFrame f) => _resolve().WangChangAi(p, f);
    internal PromptChoice WangChangChoice(ProgramSkillFrame f, string verb, string suffix, string label, IReadOnlyList<int>? cards = default, IReadOnlyList<int>? targets = default, IReadOnlyDictionary<string, string>? extra = default) => _resolve().WangChangChoice(f, verb, suffix, label, cards, targets, extra);
    internal IReadOnlyList<PromptChoice> WangChangChoices(ProgramSkillFrame f) => _resolve().WangChangChoices(f);
    internal IEnumerable<(Card Card, CardLocation From, int Slot, bool Hidden)> WangChangForeignCards(ProgramSkillFrame f) => _resolve().WangChangForeignCards(f);
    internal bool WangChangOriginal(ProgramSkillFrame f, SkillProgramEffectOp operation) => _resolve().WangChangOriginal(f, operation);
    internal Card[] WangChangOwnCosts(int seat, string instance) => _resolve().WangChangOwnCosts(seat, instance);
}
