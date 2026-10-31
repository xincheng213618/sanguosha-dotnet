#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangZengouEntries
{
    internal void AnswerWushuangZengou(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerWushuangZengou(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWushuangZengou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginWushuangZengou(f);
    internal bool CanTriggerWushuangZengou(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanTriggerWushuangZengou(c, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseWushuangZengou(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseWushuangZengou(prompt, frame);
    internal void CompleteWushuangZengou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteWushuangZengou(f);
    internal bool ContinueWushuangZengouDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().ContinueWushuangZengouDodge(attack, action, continuation);
    internal void PublishWushuangZengou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWushuangZengou(f);
    internal void PublishWushuangZengouPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWushuangZengouPayment(f);
    internal bool ResumeWushuangZengou(long id) => _resolve().ResumeWushuangZengou(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WushuangZengouChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WushuangZengouChoices(f);
    internal global::CardGame.Core.Card[] WushuangZengouCosts(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WushuangZengouCosts(f);
    internal global::CardGame.Core.Card[] WushuangZengouCosts2(int ownerSeat, string skillId, string skillInstanceId) => _resolve().WushuangZengouCosts(ownerSeat, skillId, skillInstanceId);
    internal global::CardGame.Core.WushuangZengouPaidEvent WushuangZengouPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WushuangZengouPayment(f);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? WushuangZengouWindow(global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().WushuangZengouWindow(context);
}
