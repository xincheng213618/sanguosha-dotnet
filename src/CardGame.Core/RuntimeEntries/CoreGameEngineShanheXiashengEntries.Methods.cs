#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheXiashengEntries
{
    internal global::CardGame.Core.PromptChoice AiShanheXiasheng(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AiShanheXiasheng(prompt, frame);
    internal void AnswerShanheXiasheng(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShanheXiasheng(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheXiasheng(global::CardGame.Core.ProgramSkillFrame supplied, string stage) => _resolve().BeginShanheXiasheng(supplied, stage);
    internal bool CanOfferShanheXiasheng(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheXiasheng(candidate, trigger, context);
    internal bool IsShanheXiashengDrawMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsShanheXiashengDrawMovement(frame, effect, movement);
    internal bool IsShanheXiashengTargetFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DesignatedExtraTargetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> originalTargets) => _resolve().IsShanheXiashengTargetFact(use, fact, originalTargets);
    internal bool ResumeShanheXiasheng(long frameId) => _resolve().ResumeShanheXiasheng(frameId);
    internal void ReturnShanheXiashengDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShanheXiashengDraw(supplied, outcome);
    internal global::CardGame.Core.CardActionContext? ShanheXiashengAction(int owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ShanheXiashengAction(owner, context);
    internal (int Red, int Black) ShanheXiashengColors(int seat) => _resolve().ShanheXiashengColors(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<int>> ShanheXiashengExtraTargets(global::CardGame.Core.CardUseFrame use) => _resolve().ShanheXiashengExtraTargets(use);
    internal bool ShanheXiashengRequiresTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().ShanheXiashengRequiresTargetWindow(query);
}
