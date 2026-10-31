#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterZhugeGuoOlState : Contracts.ICharacterZhugeGuoOlHasIssuedZhugeGuoStateAccess
{
    private bool _hasIssuedZhugeGuoState = default !;
    internal ref bool ReferenceToHasIssuedZhugeGuoState => ref _hasIssuedZhugeGuoState;

    ref bool Contracts.ICharacterZhugeGuoOlHasIssuedZhugeGuoStateAccess._hasIssuedZhugeGuoState => ref _hasIssuedZhugeGuoState;
}
