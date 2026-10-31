#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterLiuQiOlState : Contracts.ICharacterLiuQiOlHasIssuedLiuQiAccess
{
    private bool _hasIssuedLiuQi = default !;
    internal ref bool ReferenceToHasIssuedLiuQi => ref _hasIssuedLiuQi;

    ref bool Contracts.ICharacterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi => ref _hasIssuedLiuQi;
}
