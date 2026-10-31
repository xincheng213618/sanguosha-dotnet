#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterCaoPiMouState : Contracts.ICharacterCaoPiMouMouCaoPiControlsAccess
{
    // Issued lasting restrictions, independent of the source skill and pending
    // instruction. Normal command replay rebuilds this per-match state.
    private List<MouCaoPiControl> _mouCaoPiControls = [];
    internal ref readonly List<MouCaoPiControl> ReferenceToMouCaoPiControls => ref _mouCaoPiControls;

    ref readonly List<MouCaoPiControl> Contracts.ICharacterCaoPiMouMouCaoPiControlsAccess._mouCaoPiControls => ref _mouCaoPiControls;
}
