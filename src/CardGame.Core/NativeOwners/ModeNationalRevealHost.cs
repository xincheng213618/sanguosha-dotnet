#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ModeNationalRevealHost(Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, Func<RuntimeEntries.CoreNationalGeneralRevealEntries> getCoreNationalGeneralRevealRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, KernelAcceptCall accept, KernelRejectCall reject, Func<RuntimeEntries.CoreHumanNativeCommandsEntries> getCoreHumanNativeCommandsRuntime) : IModeNationalRevealHost
{
    public DecisionKind? PendingKind => getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision?.Kind;

    public CommandError? ValidateHumanPrompt(int actorSeat, DecisionKind expectedKind, PromptId? promptId, CommandErrorCode inactiveCode) => getCoreNativePromptBoundaryRuntime().ValidateHumanPrompt(actorSeat, expectedKind, promptId, inactiveCode);
    public IReadOnlyList<GeneralSelectionSlot> LegalRevealSlots(int actorSeat) => Array.AsReadOnly(getCoreNationalGeneralRevealRuntime().BuildNationalRevealActions(matchParticipantsState.ReferenceToPlayers[actorSeat]).Where(action => action.Kind == LegalActionKind.RevealGeneral && action.GeneralSlot is not null).Select(action => action.GeneralSlot!.Value).ToArray());
    public CommandResult Accept(Action action) => accept(action);
    public CommandResult Reject(CommandErrorCode code, string message) => reject(code, message);
    public void RevealHumanGeneral(GeneralSelectionSlot slot, bool advanceToHumanBoundary) => getCoreHumanNativeCommandsRuntime().HumanRevealGeneralCore(slot, advanceToHumanBoundary);
}
