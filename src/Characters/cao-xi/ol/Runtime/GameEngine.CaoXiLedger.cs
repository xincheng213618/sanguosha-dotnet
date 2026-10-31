namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref global::CardGame.Core.IGameEvent? _caoXiHistoryTail => ref _characterCaoXiOl.ReferenceToCaoXiHistoryTail;
    private ref int _caoXiHistoryCount => ref _characterCaoXiOl.ReferenceToCaoXiHistoryCount;
    private ref global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CaoXiGangshuIncreasedEvent> _caoXiActiveUnits => ref _characterCaoXiOl.ReferenceToCaoXiActiveUnits;
}
