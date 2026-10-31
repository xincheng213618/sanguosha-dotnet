#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreTurnTargetCardQuotaAllowancesRuntime
{
    internal bool HasTargetCardQuotaAllowance(int actor, int target) => _coreProgramTargetRulesOperations.TargetRules.HasQuotaAllowance(new(actor, target));
    [ProgramTargetRule(ProgramTargetRulePoint.QuotaAllowance, 30)]
    internal bool GrantedTargetQuotaAllowed(ProgramTargetQuotaQuery query) => _turnCardUseEffectsOperations._turnCardUseEffects.HasTargetCardQuotaAllowance(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, query.ActorSeat, query.TargetSeat);
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 110)]
    internal bool GrantedTargetSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => HasTargetCardQuotaAllowance(query.Actor.Seat, query.Target.Seat);
}
