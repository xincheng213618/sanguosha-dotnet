#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineShenwuActivitiesState
{
    private GameEngine.ShenwuDraftSetup? _shenwuDraft = default !;
    internal ref GameEngine.ShenwuDraftSetup? ReferenceToShenwuDraft => ref _shenwuDraft;
}
