using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    internal sealed record ShiHcLedgerGrant(ShiHcCujunSlashGrantedEvent Grant, int Index);
    internal sealed record ShiHcLedgerPlayStart(int Seat, int Index);
    private ref global::CardGame.Core.NativeOwners.ShiHcLedgerView? _shiHcLedgerCache => ref _characterHuangChongMobileShi.ReferenceToShiHcLedgerCache;
    private ref int _shiHcLedgerCacheCount => ref _characterHuangChongMobileShi.ReferenceToShiHcLedgerCacheCount;
    private ref global::CardGame.Core.IGameEvent? _shiHcLedgerCacheHead => ref _characterHuangChongMobileShi.ReferenceToShiHcLedgerCacheHead;
    private ref global::CardGame.Core.IGameEvent? _shiHcLedgerCacheTail => ref _characterHuangChongMobileShi.ReferenceToShiHcLedgerCacheTail;
}
