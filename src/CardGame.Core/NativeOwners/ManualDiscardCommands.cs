#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ManualDiscardCommands(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries> getCoreNativePromptBoundaryRuntime, global::CardGame.Core.NativeOwners.KernelAcceptCall accept, global::CardGame.Core.NativeOwners.KernelRejectCall reject) : IManualDiscardCommands
{
    public CommandError? ValidatePrompt(int actorSeat, PromptId? promptId) => getCoreNativePromptBoundaryRuntime().ValidateHumanPrompt(actorSeat, DecisionKind.DiscardCards, promptId, CommandErrorCode.IllegalAction);
    public CommandResult Accept(Action operation) => accept(operation);
    public CommandResult Reject(CommandErrorCode code, string message) => reject(code, message);
}
