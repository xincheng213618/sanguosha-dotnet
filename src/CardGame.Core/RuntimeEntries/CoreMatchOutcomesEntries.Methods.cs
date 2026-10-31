#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchOutcomesEntries
{
    internal void EndAsDraw(string reason) => _resolve().EndAsDraw(reason);
    internal global::CardGame.Core.Winner GetWinnerForFaction(string factionId) => _resolve().GetWinnerForFaction(factionId);
    internal global::CardGame.Core.Winner GetWinnerForTeam(string teamId) => _resolve().GetWinnerForTeam(teamId);
}
