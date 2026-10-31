#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterCaoPiMouState : global::CardGame.Core.EngineStates.Contracts.ICharacterCaoPiMouMouCaoPiControlsAccess
{
    // Issued lasting restrictions, independent of the source skill and pending
    // instruction. Normal command replay rebuilds this per-match state.
    private global::System.Collections.Generic.List<global::CardGame.Core.MouCaoPiControl> _mouCaoPiControls = [];
    internal ref readonly global::System.Collections.Generic.List<global::CardGame.Core.MouCaoPiControl> ReferenceToMouCaoPiControls => ref _mouCaoPiControls;

    ref readonly global::System.Collections.Generic.List<global::CardGame.Core.MouCaoPiControl> global::CardGame.Core.EngineStates.Contracts.ICharacterCaoPiMouMouCaoPiControlsAccess._mouCaoPiControls => ref _mouCaoPiControls;
}
