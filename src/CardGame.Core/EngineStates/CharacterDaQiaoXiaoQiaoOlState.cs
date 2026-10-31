#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterDaQiaoXiaoQiaoOlState : global::CardGame.Core.EngineStates.Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess
{
    private global::System.Collections.Generic.Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> _daQiaoXiaoQiaoQualificationCache = [];
    internal ref readonly global::System.Collections.Generic.Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> ReferenceToDaQiaoXiaoQiaoQualificationCache => ref _daQiaoXiaoQiaoQualificationCache;

    private long _daQiaoXiaoQiaoQualificationRevision = default !;
    internal ref long ReferenceToDaQiaoXiaoQiaoQualificationRevision => ref _daQiaoXiaoQiaoQualificationRevision;

    private bool _synchronizingDaQiaoXiaoQiaoLuoyan = default !;
    internal ref bool ReferenceToSynchronizingDaQiaoXiaoQiaoLuoyan => ref _synchronizingDaQiaoXiaoQiaoLuoyan;

    ref readonly global::System.Collections.Generic.Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> global::CardGame.Core.EngineStates.Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._daQiaoXiaoQiaoQualificationCache => ref _daQiaoXiaoQiaoQualificationCache;

    ref long global::CardGame.Core.EngineStates.Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._daQiaoXiaoQiaoQualificationRevision => ref _daQiaoXiaoQiaoQualificationRevision;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._synchronizingDaQiaoXiaoQiaoLuoyan => ref _synchronizingDaQiaoXiaoQiaoLuoyan;
}
