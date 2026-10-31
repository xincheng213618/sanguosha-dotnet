#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterLvKaiOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterLvKaiOlBijingMarkedCardIdsAccess
{
    // 闭境 marks survive across turns until the owner's next preparation phase
    // recasts them; replay reconstructs the map from the accepted commands.
    private global::System.Collections.Generic.Dictionary<int, int> _bijingMarkedCardIds = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, int> ReferenceToBijingMarkedCardIds => ref _bijingMarkedCardIds;

    ref readonly global::System.Collections.Generic.Dictionary<int, int> global::CardGame.Core.EngineStates.Contracts.ICharacterLvKaiOlBijingMarkedCardIdsAccess._bijingMarkedCardIds => ref _bijingMarkedCardIds;
}
