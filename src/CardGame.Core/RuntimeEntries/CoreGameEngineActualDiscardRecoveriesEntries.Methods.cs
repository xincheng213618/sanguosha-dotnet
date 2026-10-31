#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualDiscardRecoveriesEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ActualDiscardRecoveryChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ActualDiscardRecoveryChoices(f);
    internal void AssertActualDiscardRecovery(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertActualDiscardRecovery(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginActualDiscardRecovery(global::CardGame.Core.ProgramSkillFrame f, string stateId) => _resolve().BeginActualDiscardRecovery(f, stateId);
    internal bool CanOfferActualDiscardRecovery(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferActualDiscardRecovery(c, context);
    internal bool DrainCapturedEquipmentOrDiscardChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainCapturedEquipmentOrDiscardChildren(f);
    internal void PublishActualDiscardRecoveryPrompt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishActualDiscardRecoveryPrompt(f);
    internal void ResolveActualDiscardRecovery(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveActualDiscardRecovery(selected);
    internal bool ResumeActualDiscardRecovery(long id) => _resolve().ResumeActualDiscardRecovery(id);
    internal bool ReturnCapturedEquipmentOrDiscardMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnCapturedEquipmentOrDiscardMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiActualDiscardRecovery(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiActualDiscardRecovery(p, f);
}
