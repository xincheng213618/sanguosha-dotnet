#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDelayedPlacementTargetProgramsEntries
{
    internal void ContinueFinalizedDelayedPlacement(ProgramCardTriggerWindowFrame completedWindow) => _resolve().ContinueFinalizedDelayedPlacement(completedWindow);
    internal bool IsExactFinalizedDelayedPlacementWindow(ProgramCardTriggerWindowFrame window) => _resolve().IsExactFinalizedDelayedPlacementWindow(window);
    internal bool TryBeginFinalizedDelayedPlacementPrograms(long frameId, Card card, LegalActionKind actionKind) => _resolve().TryBeginFinalizedDelayedPlacementPrograms(frameId, card, actionKind);
}
