#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchOutcomesEntries
{
    internal void EndAsDraw(string reason) => _resolve().EndAsDraw(reason);
    internal Winner GetWinnerForFaction(string factionId) => _resolve().GetWinnerForFaction(factionId);
    internal Winner GetWinnerForTeam(string teamId) => _resolve().GetWinnerForTeam(teamId);
}
