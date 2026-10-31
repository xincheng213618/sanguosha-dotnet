#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaManOlRuntime
{
    internal bool HasRuntimeXili(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ProgramInstances.Any(i => i.Program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.PayXiliActualOtherOwnerDamage)));
}
