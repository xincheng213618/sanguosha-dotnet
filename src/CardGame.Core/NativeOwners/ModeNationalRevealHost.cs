#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ModeNationalRevealHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNationalGeneralRevealEntries> getCoreNationalGeneralRevealRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::CardGame.Core.NativeOwners.KernelAcceptCall accept, global::CardGame.Core.NativeOwners.KernelRejectCall reject, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreHumanNativeCommandsEntries> getCoreHumanNativeCommandsRuntime) : IModeNationalRevealHost
{
    public DecisionKind? PendingKind => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision?.Kind;

    public CommandError? ValidateHumanPrompt(int actorSeat, DecisionKind expectedKind, PromptId? promptId, CommandErrorCode inactiveCode) => getCoreNativePromptBoundaryRuntime().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
    public IReadOnlyList<GeneralSelectionSlot> LegalRevealSlots(int actorSeat) => Array.AsReadOnly(getCoreNationalGeneralRevealRuntime().BuildNationalRevealActions(matchParticipantsState.ReferenceToPlayers[actorSeat]).Where(action => action.Kind == LegalActionKind.RevealGeneral && action.GeneralSlot is not null).Select(action => action.GeneralSlot!.Value).ToArray());
    public CommandResult Accept(Action action) => accept(action);
    public CommandResult Reject(CommandErrorCode code, string message) => reject(code, message);
    public void RevealHumanGeneral(GeneralSelectionSlot slot, bool advanceToHumanBoundary) => getCoreHumanNativeCommandsRuntime().HumanRevealGeneralCore(slot, advanceToHumanBoundary);
}
