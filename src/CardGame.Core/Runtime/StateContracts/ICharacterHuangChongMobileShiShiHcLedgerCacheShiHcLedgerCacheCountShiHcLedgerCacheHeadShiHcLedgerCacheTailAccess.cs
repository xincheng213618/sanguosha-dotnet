#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.NativeOwners.ShiHcLedgerView? _shiHcLedgerCache { get; }

    ref int _shiHcLedgerCacheCount { get; }

    ref global::CardGame.Core.IGameEvent? _shiHcLedgerCacheHead { get; }

    ref global::CardGame.Core.IGameEvent? _shiHcLedgerCacheTail { get; }
}
