#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::System.Collections.Generic.Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> _daQiaoXiaoQiaoQualificationCache { get; }

    ref long _daQiaoXiaoQiaoQualificationRevision { get; }

    ref bool _synchronizingDaQiaoXiaoQiaoLuoyan { get; }
}
