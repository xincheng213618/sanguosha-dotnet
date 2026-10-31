#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterHuangChongMobileShiShiHcLedgerCacheShiHcLedgerCacheCountShiHcLedgerCacheHeadShiHcLedgerCacheTailAccess : IProgramRuntimeOperations
{
    ref NativeOwners.ShiHcLedgerView? _shiHcLedgerCache { get; }

    ref int _shiHcLedgerCacheCount { get; }

    ref IGameEvent? _shiHcLedgerCacheHead { get; }

    ref IGameEvent? _shiHcLedgerCacheTail { get; }
}
