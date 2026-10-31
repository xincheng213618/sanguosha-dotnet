#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceQingjianEntries
{
    internal void AnswerSourceQingjian(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerSourceQingjian(selected);
    internal bool CanOfferSourceQingjian(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSourceQingjian(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice ChooseSourceQingjian(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseSourceQingjian(decision, frame);
    internal bool ContinueSourceQingjian(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueSourceQingjian(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome DistributeSourceQingjianGains(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().DistributeSourceQingjianGains(supplied);
    internal void FinishSourceQingjian(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishSourceQingjian(frame);
    internal void PublishSourceQingjian(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishSourceQingjian(frame);
    internal bool ReturnSourceQingjianMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnSourceQingjianMovement(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SourceQingjianChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SourceQingjianChoices(frame);
    internal bool SourceQingjianMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().SourceQingjianMovement(frame, effect, pending);
    internal global::System.Collections.Generic.IReadOnlyList<int> SourceQingjianOriginalGains(int owner, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().SourceQingjianOriginalGains(owner, context);
    internal global::CardGame.Core.SkillProgramEffect? SourceQingjianPausedEffect(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SourceQingjianPausedEffect(frame);
}
