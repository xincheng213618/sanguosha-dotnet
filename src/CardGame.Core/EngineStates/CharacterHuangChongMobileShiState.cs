#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterHuangChongMobileShiState : global::CardGame.Core.EngineStates.Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess
{
    private global::CardGame.Core.NativeOwners.ShiHcLedgerView? _shiHcLedgerCache = default !;
    internal ref global::CardGame.Core.NativeOwners.ShiHcLedgerView? ReferenceToShiHcLedgerCache => ref _shiHcLedgerCache;

    private int _shiHcLedgerCacheCount = -1;
    internal ref int ReferenceToShiHcLedgerCacheCount => ref _shiHcLedgerCacheCount;

    private global::CardGame.Core.IGameEvent? _shiHcLedgerCacheHead = default !;
    internal ref global::CardGame.Core.IGameEvent? ReferenceToShiHcLedgerCacheHead => ref _shiHcLedgerCacheHead;

    private global::CardGame.Core.IGameEvent? _shiHcLedgerCacheTail = default !;
    internal ref global::CardGame.Core.IGameEvent? ReferenceToShiHcLedgerCacheTail => ref _shiHcLedgerCacheTail;

    ref global::CardGame.Core.NativeOwners.ShiHcLedgerView? global::CardGame.Core.EngineStates.Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCache => ref _shiHcLedgerCache;

    ref int global::CardGame.Core.EngineStates.Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheCount => ref _shiHcLedgerCacheCount;

    ref global::CardGame.Core.IGameEvent? global::CardGame.Core.EngineStates.Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheHead => ref _shiHcLedgerCacheHead;

    ref global::CardGame.Core.IGameEvent? global::CardGame.Core.EngineStates.Contracts.ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess._shiHcLedgerCacheTail => ref _shiHcLedgerCacheTail;
}
