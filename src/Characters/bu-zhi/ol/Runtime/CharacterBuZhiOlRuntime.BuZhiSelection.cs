#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBuZhiOlRuntime
{
    Role ISelectionRoleQueries.Role(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Role;
}
