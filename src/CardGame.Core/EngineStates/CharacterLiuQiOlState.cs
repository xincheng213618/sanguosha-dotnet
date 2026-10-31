#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterLiuQiOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterLiuQiOlHasIssuedLiuQiAccess
{
    private bool _hasIssuedLiuQi = default !;
    internal ref bool ReferenceToHasIssuedLiuQi => ref _hasIssuedLiuQi;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICharacterLiuQiOlHasIssuedLiuQiAccess._hasIssuedLiuQi => ref _hasIssuedLiuQi;
}
