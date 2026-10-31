#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBuZhiOlRuntime
{
    Role global::CardGame.Core.ISelectionRoleQueries.Role(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Role;
}
