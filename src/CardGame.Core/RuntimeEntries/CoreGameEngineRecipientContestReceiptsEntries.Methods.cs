#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientContestReceiptsEntries
{
    internal bool MatchesPindianWinnerUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.PindianWinnerSlashReturn returned) => _resolve().MatchesPindianWinnerUse(use, returned);
    internal bool ValidRecipientContest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidRecipientContest(f);
    internal bool ValidUniqueHpPeer(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidUniqueHpPeer(f);
    internal bool ValidUniqueHpPeerInvoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.UniqueHpPeerInvoice r, int seat) => _resolve().ValidUniqueHpPeerInvoice(f, r, seat);
}
