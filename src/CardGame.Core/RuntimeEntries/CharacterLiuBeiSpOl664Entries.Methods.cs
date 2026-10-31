#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuBeiSpOl664Entries
{
    internal void AnswerOlSpLiuBei(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerOlSpLiuBei(selected);
    internal void BeginOlSpHandAdjustment(global::CardGame.Core.ProgramSkillFrame frame, int target) => _resolve().BeginOlSpHandAdjustment(frame, target);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOlSpJingleiAdjustment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginOlSpJingleiAdjustment(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOlSpZhujiu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginOlSpZhujiu(frame);
    internal bool CanContinuePaidOlSpLiuBei664(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinuePaidOlSpLiuBei664(frame);
    internal bool CanOfferOlSpBrew(global::CardGame.Core.CharacterState owner) => _resolve().CanOfferOlSpBrew(owner);
    internal bool CanOfferOlSpDyingBrew(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferOlSpDyingBrew(candidate, trigger, context);
    internal bool CanOfferOlSpJinglei(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferOlSpJinglei(candidate, trigger, context);
    internal bool CanOlSpBrew(global::CardGame.Core.CharacterState owner) => _resolve().CanOlSpBrew(owner);
    internal bool CanOlSpDyingBrew(int owner, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().CanOlSpDyingBrew(owner, context);
    internal void CommitOlSpJingleiDiscard(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CommitOlSpJingleiDiscard(frame);
    internal void CommitOlSpZhujiu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CommitOlSpZhujiu(frame);
    internal void CompleteOlSpAdjustment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteOlSpAdjustment(frame);
    internal void ContinueOlSpAdjustment(long id) => _resolve().ContinueOlSpAdjustment(id);
    internal global::CardGame.Core.SkillProgramStepOutcome GrantOlSpXudai(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().GrantOlSpXudai(frame);
    internal bool HasOlSpZhujiu(int seat) => _resolve().HasOlSpZhujiu(seat);
    internal int OlSpAlcoholCount(int owner) => _resolve().OlSpAlcoholCount(owner);
    internal global::CardGame.Core.Card[] OlSpBrewMaterials(int owner) => _resolve().OlSpBrewMaterials(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OlSpLiuBeiChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OlSpLiuBeiChoices(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome PayOlSpJinglei(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PayOlSpJinglei(frame);
    internal void PublishOlSpLiuBeiChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishOlSpLiuBeiChoice(frame);
    internal bool ResumeOlSpJingleiMovement(long id) => _resolve().ResumeOlSpJingleiMovement(id);
    internal void ReturnOlSpJingleiDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnOlSpJingleiDraw(supplied, outcome);
    internal bool ReturnOlSpZhujiu(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnOlSpZhujiu(use);
    internal bool ReturnOlSpZhujiuDyingBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().ReturnOlSpZhujiuDyingBinding(frame, completed);
    internal global::CardGame.Core.PromptChoice SelectAiOlSpLiuBei664(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiOlSpLiuBei664(decision, frame);
}
