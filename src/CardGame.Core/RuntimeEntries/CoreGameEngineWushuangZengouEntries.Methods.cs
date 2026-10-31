#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangZengouEntries
{
    internal void AnswerWushuangZengou(PromptChoice selected) => _resolve().AnswerWushuangZengou(selected);
    internal SkillProgramStepOutcome BeginWushuangZengou(ProgramSkillFrame f) => _resolve().BeginWushuangZengou(f);
    internal bool CanTriggerWushuangZengou(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanTriggerWushuangZengou(c, trigger, context);
    internal PromptChoice ChooseWushuangZengou(PendingDecision prompt, ProgramSkillFrame frame) => _resolve().ChooseWushuangZengou(prompt, frame);
    internal void CompleteWushuangZengou(ProgramSkillFrame f) => _resolve().CompleteWushuangZengou(f);
    internal bool ContinueWushuangZengouDodge(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().ContinueWushuangZengouDodge(attack, action, continuation);
    internal void PublishWushuangZengou(ProgramSkillFrame f) => _resolve().PublishWushuangZengou(f);
    internal void PublishWushuangZengouPayment(ProgramSkillFrame f) => _resolve().PublishWushuangZengouPayment(f);
    internal bool ResumeWushuangZengou(long id) => _resolve().ResumeWushuangZengou(id);
    internal IReadOnlyList<PromptChoice> WushuangZengouChoices(ProgramSkillFrame f) => _resolve().WushuangZengouChoices(f);
    internal Card[] WushuangZengouCosts(ProgramSkillFrame f) => _resolve().WushuangZengouCosts(f);
    internal Card[] WushuangZengouCosts2(int ownerSeat, string skillId, string skillInstanceId) => _resolve().WushuangZengouCosts(ownerSeat, skillId, skillInstanceId);
    internal WushuangZengouPaidEvent WushuangZengouPayment(ProgramSkillFrame f) => _resolve().WushuangZengouPayment(f);
    internal ProgramCardTriggerWindowFrame? WushuangZengouWindow(ProgramSkillWindowContext? context) => _resolve().WushuangZengouWindow(context);
}
