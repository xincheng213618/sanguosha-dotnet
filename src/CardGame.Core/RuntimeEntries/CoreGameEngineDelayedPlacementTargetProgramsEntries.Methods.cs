#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDelayedPlacementTargetProgramsEntries
{
    internal void ContinueFinalizedDelayedPlacement(global::CardGame.Core.ProgramCardTriggerWindowFrame completedWindow) => _resolve().ContinueFinalizedDelayedPlacement(completedWindow);
    internal bool IsExactFinalizedDelayedPlacementWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().IsExactFinalizedDelayedPlacementWindow(window);
    internal bool TryBeginFinalizedDelayedPlacementPrograms(long frameId, global::CardGame.Core.Card card, global::CardGame.Core.LegalActionKind actionKind) => _resolve().TryBeginFinalizedDelayedPlacementPrograms(frameId, card, actionKind);
}
