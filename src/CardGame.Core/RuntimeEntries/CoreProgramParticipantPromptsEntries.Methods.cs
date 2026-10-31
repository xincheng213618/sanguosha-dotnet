#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramParticipantPromptsEntries
{
    internal ParticipantGiftWork? CaptureParticipantGiftWork(ParticipantPromptHostState state, int actor) => _resolve().CaptureParticipantGiftWork(state, actor);
    internal ParticipantPromptHostState? CaptureParticipantHostState() => _resolve().CaptureParticipantHostState();
    internal PendingDecision? GetPendingDecisionForActor(int actorSeat) => _resolve().GetPendingDecisionForActor(actorSeat);
    internal bool IsIssuedParticipantPrompt(PendingDecision issued) => _resolve().IsIssuedParticipantPrompt(issued);
    internal bool IsParticipantInputBoundary(ParticipantPromptHostKey key) => _resolve().IsParticipantInputBoundary(key);
    internal bool HasParticipantInputBoundary { get => _resolve().HasParticipantInputBoundary; }
}
