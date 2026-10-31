#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientContestReceiptsEntries
{
    internal bool MatchesPindianWinnerUse(CardUseFrame use, PindianWinnerSlashReturn returned) => _resolve().MatchesPindianWinnerUse(use, returned);
    internal bool ValidRecipientContest(ProgramSkillFrame f) => _resolve().ValidRecipientContest(f);
    internal bool ValidUniqueHpPeer(ProgramSkillFrame f) => _resolve().ValidUniqueHpPeer(f);
    internal bool ValidUniqueHpPeerInvoice(ProgramSkillFrame f, UniqueHpPeerInvoice r, int seat) => _resolve().ValidUniqueHpPeerInvoice(f, r, seat);
}
