namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.List<global::CardGame.Core.HpChangeContext> _pendingHpChanges => ref _hpChangePrograms.ReferenceToPendingHpChanges;
}
