#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterZhugeGuoOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterZhugeGuoOlHasIssuedZhugeGuoStateAccess
{
    private bool _hasIssuedZhugeGuoState = default !;
    internal ref bool ReferenceToHasIssuedZhugeGuoState => ref _hasIssuedZhugeGuoState;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICharacterZhugeGuoOlHasIssuedZhugeGuoStateAccess._hasIssuedZhugeGuoState => ref _hasIssuedZhugeGuoState;
}
