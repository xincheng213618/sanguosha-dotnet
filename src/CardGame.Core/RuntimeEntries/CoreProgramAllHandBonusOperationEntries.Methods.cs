#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramAllHandBonusOperationEntries
{
    internal bool BypassesSlashLimitBySuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind) => _resolve().BypassesSlashLimitBySuit(owner, card, effectiveKind);
    internal bool CanUsePeachToRescue(int responderSeat, int victimSeat) => _resolve().CanUsePeachToRescue(responderSeat, victimSeat);
    internal bool SuitSlashQuotaAfterLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().SuitSlashQuotaAfterLimit(query);
}
