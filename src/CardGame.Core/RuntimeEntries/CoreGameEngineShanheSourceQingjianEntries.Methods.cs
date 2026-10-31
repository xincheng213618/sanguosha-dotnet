#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceQingjianEntries
{
    internal void AnswerSourceQingjian(PromptChoice selected) => _resolve().AnswerSourceQingjian(selected);
    internal bool CanOfferSourceQingjian(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferSourceQingjian(candidate, trigger, context);
    internal PromptChoice ChooseSourceQingjian(PendingDecision decision, ProgramSkillFrame frame) => _resolve().ChooseSourceQingjian(decision, frame);
    internal bool ContinueSourceQingjian(ProgramSkillFrame frame) => _resolve().ContinueSourceQingjian(frame);
    internal SkillProgramStepOutcome DistributeSourceQingjianGains(ProgramSkillFrame supplied) => _resolve().DistributeSourceQingjianGains(supplied);
    internal void FinishSourceQingjian(ProgramSkillFrame frame) => _resolve().FinishSourceQingjian(frame);
    internal void PublishSourceQingjian(ProgramSkillFrame frame) => _resolve().PublishSourceQingjian(frame);
    internal bool ReturnSourceQingjianMovement(ProgramSkillFrame frame) => _resolve().ReturnSourceQingjianMovement(frame);
    internal IReadOnlyList<PromptChoice> SourceQingjianChoices(ProgramSkillFrame frame) => _resolve().SourceQingjianChoices(frame);
    internal bool SourceQingjianMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().SourceQingjianMovement(frame, effect, pending);
    internal IReadOnlyList<int> SourceQingjianOriginalGains(int owner, ProgramSkillWindowContext? context) => _resolve().SourceQingjianOriginalGains(owner, context);
    internal SkillProgramEffect? SourceQingjianPausedEffect(ProgramSkillFrame frame) => _resolve().SourceQingjianPausedEffect(frame);
}
