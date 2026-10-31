#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuRuntimeRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 70)]
    internal bool GuanFocusDirectedTargetProhibited(ProgramDirectedTargetQuery query) => GuanFocusProhibitsSlash(query.ActorSeat, query.TargetSeat, query.Kind);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 40)]
    internal bool GuanFocusSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => GuanFocusAllowsSlash(query.Actor.Seat, query.Target.Seat);
}
