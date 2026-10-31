#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSteadyHandRuntime
{
    internal bool IsShanheSteadyHandModifierQualified(int ownerSeat, string skillId) => skillId != ShanheSteadyHandPrograms.SkillId || _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null && _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive;
}
