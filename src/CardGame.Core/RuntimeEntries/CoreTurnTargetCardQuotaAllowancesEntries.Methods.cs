#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreTurnTargetCardQuotaAllowancesEntries
{
    internal bool GrantedTargetQuotaAllowed(ProgramTargetQuotaQuery query) => _resolve().GrantedTargetQuotaAllowed(query);
    internal bool GrantedTargetSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().GrantedTargetSlashQuotaAfterLimit(query);
    internal bool HasTargetCardQuotaAllowance(int actor, int target) => _resolve().HasTargetCardQuotaAllowance(actor, target);
}
