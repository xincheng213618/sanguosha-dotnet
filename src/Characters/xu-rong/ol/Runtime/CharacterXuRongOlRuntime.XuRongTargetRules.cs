#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuRongOlRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 60)]
    internal bool XuRongDirectedTargetProhibited(ProgramDirectedTargetQuery query) => IsXuRongSlashTargetForbidden(query.ActorSeat, query.TargetSeat, query.Kind);
}
