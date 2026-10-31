#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterTaiShiCiMobileShiState : Contracts.ICharacterTaiShiCiMobileShiShiTzLieScanSequenceAccess
{
    // Existing match-local derived index; replay rebuilds it in event order.
    // The independent Runtime owns scanning and qualification, not this state.
    private int _shiTzLieScanSequence = default !;
    internal ref int ReferenceToShiTzLieScanSequence => ref _shiTzLieScanSequence;

    ref int Contracts.ICharacterTaiShiCiMobileShiShiTzLieScanSequenceAccess._shiTzLieScanSequence => ref _shiTzLieScanSequence;
}
