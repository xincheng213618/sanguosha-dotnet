#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuBeiSpOl664Entries
{
    internal void AnswerOlSpLiuBei(PromptChoice selected) => _resolve().AnswerOlSpLiuBei(selected);
    internal void BeginOlSpHandAdjustment(ProgramSkillFrame frame, int target) => _resolve().BeginOlSpHandAdjustment(frame, target);
    internal SkillProgramStepOutcome BeginOlSpJingleiAdjustment(ProgramSkillFrame frame) => _resolve().BeginOlSpJingleiAdjustment(frame);
    internal SkillProgramStepOutcome BeginOlSpZhujiu(ProgramSkillFrame frame) => _resolve().BeginOlSpZhujiu(frame);
    internal bool CanContinuePaidOlSpLiuBei664(ProgramSkillFrame frame) => _resolve().CanContinuePaidOlSpLiuBei664(frame);
    internal bool CanOfferOlSpBrew(CharacterState owner) => _resolve().CanOfferOlSpBrew(owner);
    internal bool CanOfferOlSpDyingBrew(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferOlSpDyingBrew(candidate, trigger, context);
    internal bool CanOfferOlSpJinglei(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferOlSpJinglei(candidate, trigger, context);
    internal bool CanOlSpBrew(CharacterState owner) => _resolve().CanOlSpBrew(owner);
    internal bool CanOlSpDyingBrew(int owner, ProgramSkillWindowContext? context) => _resolve().CanOlSpDyingBrew(owner, context);
    internal void CommitOlSpJingleiDiscard(ProgramSkillFrame frame) => _resolve().CommitOlSpJingleiDiscard(frame);
    internal void CommitOlSpZhujiu(ProgramSkillFrame frame) => _resolve().CommitOlSpZhujiu(frame);
    internal void CompleteOlSpAdjustment(ProgramSkillFrame frame) => _resolve().CompleteOlSpAdjustment(frame);
    internal void ContinueOlSpAdjustment(long id) => _resolve().ContinueOlSpAdjustment(id);
    internal SkillProgramStepOutcome GrantOlSpXudai(ProgramSkillFrame frame) => _resolve().GrantOlSpXudai(frame);
    internal bool HasOlSpZhujiu(int seat) => _resolve().HasOlSpZhujiu(seat);
    internal int OlSpAlcoholCount(int owner) => _resolve().OlSpAlcoholCount(owner);
    internal Card[] OlSpBrewMaterials(int owner) => _resolve().OlSpBrewMaterials(owner);
    internal IReadOnlyList<PromptChoice> OlSpLiuBeiChoices(ProgramSkillFrame frame) => _resolve().OlSpLiuBeiChoices(frame);
    internal SkillProgramStepOutcome PayOlSpJinglei(ProgramSkillFrame frame) => _resolve().PayOlSpJinglei(frame);
    internal void PublishOlSpLiuBeiChoice(ProgramSkillFrame frame) => _resolve().PublishOlSpLiuBeiChoice(frame);
    internal bool ResumeOlSpJingleiMovement(long id) => _resolve().ResumeOlSpJingleiMovement(id);
    internal void ReturnOlSpJingleiDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnOlSpJingleiDraw(supplied, outcome);
    internal bool ReturnOlSpZhujiu(CardUseFrame use) => _resolve().ReturnOlSpZhujiu(use);
    internal bool ReturnOlSpZhujiuDyingBinding(ProgramSkillFrame frame, bool completed) => _resolve().ReturnOlSpZhujiuDyingBinding(frame, completed);
    internal PromptChoice SelectAiOlSpLiuBei664(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiOlSpLiuBei664(decision, frame);
}
