#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKuangfuEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginKuangfu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginKuangfu(supplied);
    internal bool CanIssueKuangfuSlash(int actor, int target, int? excludedEquipmentId = default) => _resolve().CanIssueKuangfuSlash(actor, target, excludedEquipmentId);
    internal bool CanStartKuangfu(global::CardGame.Core.CharacterState owner) => _resolve().CanStartKuangfu(owner);
    internal void CompleteKuangfuSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().CompleteKuangfuSlash(completion);
    internal bool DrainKuangfu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainKuangfu(f);
    internal void FinishKuangfu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishKuangfu(f);
    internal void IssueKuangfuSlash(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().IssueKuangfuSlash(f, targetSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> KuangfuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KuangfuChoices(f);
    internal global::System.Collections.Generic.IEnumerable<(int Owner, global::CardGame.Core.Card Card)> KuangfuEquipment(int actor) => _resolve().KuangfuEquipment(actor);
    internal bool KuangfuGameEnded() => _resolve().KuangfuGameEnded();
    internal bool KuangfuUnpaidSourceCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KuangfuUnpaidSourceCurrent(f);
    internal void PublishKuangfu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishKuangfu(f);
    internal void ResolveKuangfuChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveKuangfuChoice(choice);
    internal bool ResumeKuangfu(long id) => _resolve().ResumeKuangfu(id);
    internal bool ReturnKuangfuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnKuangfuMovement(f);
    internal bool ReturnRegisteredCompleteKuangfuSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnRegisteredCompleteKuangfuSlash(completion);
    internal global::CardGame.Core.PromptChoice SelectAiKuangfu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiKuangfu(p, f);
}
