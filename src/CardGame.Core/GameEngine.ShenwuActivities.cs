namespace CardGame.Core;
public sealed partial class GameEngine
{
    internal sealed record ShenwuDraftSetup(IReadOnlyList<string> SharedGods, string LeaderId);
    private ref global::CardGame.Core.GameEngine.ShenwuDraftSetup? _shenwuDraft => ref _shenwuActivities.ReferenceToShenwuDraft;
}
