namespace CardGame.Core;
public sealed partial class GameEngine
{
    // Existing match-local derived index; replay rebuilds it in event order.
    // The independent Runtime owns scanning and qualification, not this state.
    private ref int _shiTzLieScanSequence => ref _characterTaiShiCiMobileShi.ReferenceToShiTzLieScanSequence;
}
