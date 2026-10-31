#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ManualDiscardCommands(Func<RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, KernelAcceptCall accept, KernelRejectCall reject) : IManualDiscardCommands
{
    public CommandError? ValidatePrompt(int actorSeat, PromptId? promptId) => getCoreNativePromptBoundaryRuntime().ValidateHumanPrompt(actorSeat, DecisionKind.DiscardCards, promptId, CommandErrorCode.IllegalAction);
    public CommandResult Accept(Action operation) => accept(operation);
    public CommandResult Reject(CommandErrorCode code, string message) => reject(code, message);
}
