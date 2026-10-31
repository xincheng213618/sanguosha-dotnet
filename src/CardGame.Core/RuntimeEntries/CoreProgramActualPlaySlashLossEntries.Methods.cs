#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramActualPlaySlashLossEntries
{
    internal ActualPlayPhysicalSlashLossStamp? CaptureActualPlaySlashLoss(Card card, CardLocation from, CardLocation to) => _resolve().CaptureActualPlaySlashLoss(card, from, to);
    internal int CurrentActualPlaySlashLossCount(int owner) => _resolve().CurrentActualPlaySlashLossCount(owner);
    internal bool TracksActualPlaySlashLoss { get => _resolve().TracksActualPlaySlashLoss; }
}
