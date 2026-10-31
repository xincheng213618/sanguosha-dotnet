#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramActualPlaySlashLossEntries
{
    internal global::CardGame.Core.ActualPlayPhysicalSlashLossStamp? CaptureActualPlaySlashLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => _resolve().CaptureActualPlaySlashLoss(card, from, to);
    internal int CurrentActualPlaySlashLossCount(int owner) => _resolve().CurrentActualPlaySlashLossCount(owner);
    internal bool TracksActualPlaySlashLoss { get => _resolve().TracksActualPlaySlashLoss; }
}
