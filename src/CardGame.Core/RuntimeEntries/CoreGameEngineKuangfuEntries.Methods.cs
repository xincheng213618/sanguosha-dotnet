#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKuangfuEntries
{
    internal SkillProgramStepOutcome BeginKuangfu(ProgramSkillFrame supplied) => _resolve().BeginKuangfu(supplied);
    internal bool CanIssueKuangfuSlash(int actor, int target, int? excludedEquipmentId = default) => _resolve().CanIssueKuangfuSlash(actor, target, excludedEquipmentId);
    internal bool CanStartKuangfu(CharacterState owner) => _resolve().CanStartKuangfu(owner);
    internal void CompleteKuangfuSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteKuangfuSlash(completion);
    internal bool DrainKuangfu(ProgramSkillFrame f) => _resolve().DrainKuangfu(f);
    internal void FinishKuangfu(ProgramSkillFrame f) => _resolve().FinishKuangfu(f);
    internal void IssueKuangfuSlash(ProgramSkillFrame f, int targetSeat) => _resolve().IssueKuangfuSlash(f, targetSeat);
    internal IReadOnlyList<PromptChoice> KuangfuChoices(ProgramSkillFrame f) => _resolve().KuangfuChoices(f);
    internal IEnumerable<(int Owner, Card Card)> KuangfuEquipment(int actor) => _resolve().KuangfuEquipment(actor);
    internal bool KuangfuGameEnded() => _resolve().KuangfuGameEnded();
    internal bool KuangfuUnpaidSourceCurrent(ProgramSkillFrame f) => _resolve().KuangfuUnpaidSourceCurrent(f);
    internal void PublishKuangfu(ProgramSkillFrame f) => _resolve().PublishKuangfu(f);
    internal void ResolveKuangfuChoice(PromptChoice choice) => _resolve().ResolveKuangfuChoice(choice);
    internal bool ResumeKuangfu(long id) => _resolve().ResumeKuangfu(id);
    internal bool ReturnKuangfuMovement(ProgramSkillFrame f) => _resolve().ReturnKuangfuMovement(f);
    internal bool ReturnRegisteredCompleteKuangfuSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteKuangfuSlash(completion);
    internal PromptChoice SelectAiKuangfu(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiKuangfu(p, f);
}
