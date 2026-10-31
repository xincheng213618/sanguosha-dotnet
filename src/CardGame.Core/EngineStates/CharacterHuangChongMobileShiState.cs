#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHuangChongMobileShiState : Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess
{
    private NativeOwners.ShiHcLedgerView? _shiHcLedgerCache = default !;
    internal ref NativeOwners.ShiHcLedgerView? ReferenceToShiHcLedgerCache => ref _shiHcLedgerCache;

    private int _shiHcLedgerCacheCount = -1;
    internal ref int ReferenceToShiHcLedgerCacheCount => ref _shiHcLedgerCacheCount;

    private IGameEvent? _shiHcLedgerCacheHead = default !;
    internal ref IGameEvent? ReferenceToShiHcLedgerCacheHead => ref _shiHcLedgerCacheHead;

    private IGameEvent? _shiHcLedgerCacheTail = default !;
    internal ref IGameEvent? ReferenceToShiHcLedgerCacheTail => ref _shiHcLedgerCacheTail;

    ref NativeOwners.ShiHcLedgerView? Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCache => ref _shiHcLedgerCache;

    ref int Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheCount => ref _shiHcLedgerCacheCount;

    ref IGameEvent? Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheHead => ref _shiHcLedgerCacheHead;

    ref IGameEvent? Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheTail => ref _shiHcLedgerCacheTail;
}
