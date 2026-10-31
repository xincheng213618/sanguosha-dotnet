#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoLeRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 10)]
    internal bool WeiwanDirectedTargetProhibited(ProgramDirectedTargetQuery query) => IsWeiwanTargetForbidden(query.ActorSeat, query.TargetSeat, query.Kind, query.ExistingUseFrameId);
    [ProgramTargetRule(ProgramTargetRulePoint.QuotaAllowance, 10)]
    internal bool WeiwanTargetQuotaAllowed(ProgramTargetQuotaQuery query) => HasWeiwanQuota(_coreMatchParticipantsPlayersAccess._players[query.ActorSeat], _coreMatchParticipantsPlayersAccess._players[query.TargetSeat]);
}
