#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreTurnTargetCardQuotaAllowancesEntries
{
    internal bool GrantedTargetQuotaAllowed(global::CardGame.Core.ProgramTargetQuotaQuery query) => _resolve().GrantedTargetQuotaAllowed(query);
    internal bool GrantedTargetSlashQuotaAfterLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().GrantedTargetSlashQuotaAfterLimit(query);
    internal bool HasTargetCardQuotaAllowance(int actor, int target) => _resolve().HasTargetCardQuotaAllowance(actor, target);
}
