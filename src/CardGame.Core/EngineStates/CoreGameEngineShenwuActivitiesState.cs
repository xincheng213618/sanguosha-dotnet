#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineShenwuActivitiesState
{
    private global::CardGame.Core.GameEngine.ShenwuDraftSetup? _shenwuDraft = default !;
    internal ref global::CardGame.Core.GameEngine.ShenwuDraftSetup? ReferenceToShenwuDraft => ref _shenwuDraft;
}
