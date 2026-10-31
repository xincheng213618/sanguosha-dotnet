#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangChangAnniversaryEntries
{
    internal void AnswerWangChang(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerWangChang(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWangChang(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginWangChang(supplied, operation);
    internal void BindWangChangSlash(global::CardGame.Core.IGameEvent fact) => _resolve().BindWangChangSlash(fact);
    internal bool CanActivateWangChang(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanActivateWangChang(c);
    internal bool CanOfferWangChang(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferWangChang(c);
    internal void CompleteWangChangSlash(long id) => _resolve().CompleteWangChangSlash(id);
    internal void ContinueWangChang(long id) => _resolve().ContinueWangChang(id);
    internal void FinishWangChang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWangChang(f);
    internal bool IsWangChangSlash(global::CardGame.Core.CardUseFrame use) => _resolve().IsWangChangSlash(use);
    internal bool KeepWangChangPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepWangChangPaid(f);
    internal void PayWangChangDiscard(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> ids, int subject) => _resolve().PayWangChangDiscard(f, ids, subject);
    internal void PublishWangChang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWangChang(f);
    internal bool ResumeWangChang(long id) => _resolve().ResumeWangChang(id);
    internal bool ReturnWangChangAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt receipt) => _resolve().ReturnWangChangAttack(receipt);
    internal void ReturnWangChangDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnWangChangDraw(supplied, outcome);
    internal bool ReturnWangChangMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWangChangMovement(f);
    internal void ReturnWangChangSlash(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnWangChangSlash(use);
    internal bool ValidWangChang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWangChang(f);
    internal void ValidateWangChangSlash(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateWangChangSlash(use);
    internal global::CardGame.Core.PromptChoice WangChangAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangChangAi(p, f);
    internal global::CardGame.Core.PromptChoice WangChangChoice(global::CardGame.Core.ProgramSkillFrame f, string verb, string suffix, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyList<int>? targets = default, global::System.Collections.Generic.IReadOnlyDictionary<string, string>? extra = default) => _resolve().WangChangChoice(f, verb, suffix, label, cards, targets, extra);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WangChangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangChangChoices(f);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation From, int Slot, bool Hidden)> WangChangForeignCards(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangChangForeignCards(f);
    internal bool WangChangMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().WangChangMovement(f, effect, p);
    internal bool WangChangOriginal(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().WangChangOriginal(f, operation);
    internal global::CardGame.Core.Card[] WangChangOwnCosts(int seat, string instance) => _resolve().WangChangOwnCosts(seat, instance);
}
