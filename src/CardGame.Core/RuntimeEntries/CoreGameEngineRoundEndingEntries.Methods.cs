#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundEndingEntries
{
    internal void ContinueRoundEndingBoundary(long frameId) => _resolve().ContinueRoundEndingBoundary(frameId);
    internal bool HasRoundEndingBoundaryFrame() => _resolve().HasRoundEndingBoundaryFrame();
    internal bool IsExactRoundEndingCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsExactRoundEndingCandidate(candidate, context);
    internal bool IsRoundEndingMovement(global::CardGame.Core.RoundEndingBoundaryFrame parent, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsRoundEndingMovement(parent, batch);
    internal void ReturnRoundEndingLifecycle(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame child) => _resolve().ReturnRoundEndingLifecycle(child);
    internal void ReturnRoundEndingMovement(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().ReturnRoundEndingMovement(child);
    internal void ReturnRoundEndingProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().ReturnRoundEndingProgram(frame, completed);
    internal bool TryBeginRoundEndingBoundary(global::CardGame.Core.CharacterState previous, bool skipped) => _resolve().TryBeginRoundEndingBoundary(previous, skipped);
    internal bool TracksRoundEnding { get => _resolve().TracksRoundEnding; }
}
