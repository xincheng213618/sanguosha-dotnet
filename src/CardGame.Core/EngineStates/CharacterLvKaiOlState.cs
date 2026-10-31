#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterLvKaiOlState : Contracts.ICharacterLvKaiOlBijingMarkedCardIdsAccess
{
    // 闭境 marks survive across turns until the owner's next preparation phase
    // recasts them; replay reconstructs the map from the accepted commands.
    private Dictionary<int, int> _bijingMarkedCardIds = [];
    internal ref readonly Dictionary<int, int> ReferenceToBijingMarkedCardIds => ref _bijingMarkedCardIds;

    ref readonly Dictionary<int, int> Contracts.ICharacterLvKaiOlBijingMarkedCardIdsAccess._bijingMarkedCardIds => ref _bijingMarkedCardIds;
}
