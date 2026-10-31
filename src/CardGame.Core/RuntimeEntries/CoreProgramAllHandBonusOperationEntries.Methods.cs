#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramAllHandBonusOperationEntries
{
    internal bool BypassesSlashLimitBySuit(CharacterState owner, Card card, CardKind effectiveKind) => _resolve().BypassesSlashLimitBySuit(owner, card, effectiveKind);
    internal bool CanUsePeachToRescue(int responderSeat, int victimSeat) => _resolve().CanUsePeachToRescue(responderSeat, victimSeat);
    internal bool SuitSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().SuitSlashQuotaAfterLimit(query);
}
