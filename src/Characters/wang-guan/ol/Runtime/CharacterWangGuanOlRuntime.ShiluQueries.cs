#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangGuanOlRuntime
{
    internal IReadOnlyList<int> ShiluTargets(int owner) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0 && _coreProgramDodgeResponsesOperations.IsWithinAttackRange(owner, p.Seat)).Select(p => p.Seat).ToArray());
}
