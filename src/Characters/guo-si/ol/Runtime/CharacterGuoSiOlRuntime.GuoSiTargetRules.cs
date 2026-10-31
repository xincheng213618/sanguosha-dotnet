#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoSiOlRuntime
{
    [ProgramTargetRule(ProgramTargetRulePoint.DirectedProhibition, 50)]
    internal bool GuoSiDirectedTargetProhibited(ProgramDirectedTargetQuery query) => HasGuoSiTargetEffect(query.ActorSeat, query.TargetSeat, DirectedTurnCardPolicyEffect.ForbidTarget);
    [ProgramTargetRule(ProgramTargetRulePoint.QuotaAllowance, 20)]
    internal bool GuoSiTargetQuotaAllowed(ProgramTargetQuotaQuery query) => HasGuoSiTargetEffect(query.ActorSeat, query.TargetSeat, DirectedTurnCardPolicyEffect.BypassSlashLimit);
}
