#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundEndingEntries
{
    internal void ContinueRoundEndingBoundary(long frameId) => _resolve().ContinueRoundEndingBoundary(frameId);
    internal bool IsExactRoundEndingCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsExactRoundEndingCandidate(candidate, context);
    internal bool IsRoundEndingMovement(RoundEndingBoundaryFrame parent, CardMovementBatchContext batch) => _resolve().IsRoundEndingMovement(parent, batch);
    internal void ReturnRoundEndingLifecycle(ProgramLifecycleTriggerWindowFrame child) => _resolve().ReturnRoundEndingLifecycle(child);
    internal void ReturnRoundEndingMovement(CardsMovedTriggerWindowFrame child) => _resolve().ReturnRoundEndingMovement(child);
    internal void ReturnRoundEndingProgram(ProgramSkillFrame frame, bool completed) => _resolve().ReturnRoundEndingProgram(frame, completed);
    internal bool TryBeginRoundEndingBoundary(CharacterState previous, bool skipped) => _resolve().TryBeginRoundEndingBoundary(previous, skipped);
    internal bool TracksRoundEnding { get => _resolve().TracksRoundEnding; }
}
