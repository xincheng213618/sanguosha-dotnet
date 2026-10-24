namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Constructor draft data is deterministic from the seed. Queue its public
    // facts only after Start, when the command session can retain them.
    private sealed record ShenwuDraftSetup(IReadOnlyList<string> SharedGods, string LeaderId);
    private ShenwuDraftSetup? _shenwuDraft;
    private IShenwuBattleFeature? ShenwuBattleRules => _modeRuntime.Find<IShenwuBattleFeature>();

}
