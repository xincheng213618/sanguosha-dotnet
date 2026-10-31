#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramChildResolutionEntries
{
    internal bool HasOwnedProgramChildSubtree(long rootFrameId) => _resolve().HasOwnedProgramChildSubtree(rootFrameId);
    internal bool ProgramChildResolutionEdge(int index) => _resolve().ProgramChildResolutionEdge(index);
}
