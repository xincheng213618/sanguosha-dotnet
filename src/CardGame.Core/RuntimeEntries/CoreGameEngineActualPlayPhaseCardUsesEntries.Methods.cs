#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualPlayPhaseCardUsesEntries
{
    internal int GetActualPlayPhaseUseCount(int actorSeat) => _resolve().GetActualPlayPhaseUseCount(actorSeat);
    internal void RecordActualPlayPhaseUse(CardActionContext action) => _resolve().RecordActualPlayPhaseUse(action);
    internal bool TracksActualPlayPhaseCardUses { get => _resolve().TracksActualPlayPhaseCardUses; }
}
