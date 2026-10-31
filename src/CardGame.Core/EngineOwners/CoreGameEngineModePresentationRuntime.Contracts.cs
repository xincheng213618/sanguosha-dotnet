#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreModePresentationCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreModePresentationCoreMatchPresentationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string GetRoleName(global::CardGame.Core.Role role);
}

internal interface ICoreModePresentationCoreMatchParticipantSetupOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Role GetTeamRole(string teamId);
}
