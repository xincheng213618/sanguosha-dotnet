#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualDiscardRecoveriesEntries
{
    internal IReadOnlyList<PromptChoice> ActualDiscardRecoveryChoices(ProgramSkillFrame f) => _resolve().ActualDiscardRecoveryChoices(f);
    internal SkillProgramStepOutcome BeginActualDiscardRecovery(ProgramSkillFrame f, string stateId) => _resolve().BeginActualDiscardRecovery(f, stateId);
    internal bool CanOfferActualDiscardRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanOfferActualDiscardRecovery(c, context);
    internal bool DrainCapturedEquipmentOrDiscardChildren(ProgramSkillFrame f) => _resolve().DrainCapturedEquipmentOrDiscardChildren(f);
    internal void PublishActualDiscardRecoveryPrompt(ProgramSkillFrame f) => _resolve().PublishActualDiscardRecoveryPrompt(f);
    internal void ResolveActualDiscardRecovery(PromptChoice selected) => _resolve().ResolveActualDiscardRecovery(selected);
    internal bool ResumeActualDiscardRecovery(long id) => _resolve().ResumeActualDiscardRecovery(id);
    internal bool ReturnCapturedEquipmentOrDiscardMovement(ProgramSkillFrame f) => _resolve().ReturnCapturedEquipmentOrDiscardMovement(f);
    internal PromptChoice SelectAiActualDiscardRecovery(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiActualDiscardRecovery(p, f);
}
