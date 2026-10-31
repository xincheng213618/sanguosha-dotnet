#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramParticipantPromptsEntries
{
    internal global::CardGame.Core.ParticipantGiftWork? CaptureParticipantGiftWork(global::CardGame.Core.ParticipantPromptHostState state, int actor) => _resolve().CaptureParticipantGiftWork(state, actor);
    internal global::CardGame.Core.ParticipantPromptHostState? CaptureParticipantHostState() => _resolve().CaptureParticipantHostState();
    internal global::CardGame.Core.PendingDecision? GetPendingDecisionForActor(int actorSeat) => _resolve().GetPendingDecisionForActor(actorSeat);
    internal bool IsIssuedParticipantPrompt(global::CardGame.Core.PendingDecision issued) => _resolve().IsIssuedParticipantPrompt(issued);
    internal bool IsParticipantInputBoundary(global::CardGame.Core.ParticipantPromptHostKey key) => _resolve().IsParticipantInputBoundary(key);
    internal bool HasParticipantInputBoundary { get => _resolve().HasParticipantInputBoundary; }
}
