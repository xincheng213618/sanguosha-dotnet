#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess : IProgramRuntimeOperations
{
    ref readonly Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> _daQiaoXiaoQiaoQualificationCache { get; }

    ref long _daQiaoXiaoQiaoQualificationRevision { get; }

    ref bool _synchronizingDaQiaoXiaoQiaoLuoyan { get; }
}
