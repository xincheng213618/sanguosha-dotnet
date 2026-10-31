#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterDaQiaoXiaoQiaoOlState : Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess
{
    private Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> _daQiaoXiaoQiaoQualificationCache = [];
    internal ref readonly Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> ReferenceToDaQiaoXiaoQiaoQualificationCache => ref _daQiaoXiaoQiaoQualificationCache;

    private long _daQiaoXiaoQiaoQualificationRevision = default !;
    internal ref long ReferenceToDaQiaoXiaoQiaoQualificationRevision => ref _daQiaoXiaoQiaoQualificationRevision;

    private bool _synchronizingDaQiaoXiaoQiaoLuoyan = default !;
    internal ref bool ReferenceToSynchronizingDaQiaoXiaoQiaoLuoyan => ref _synchronizingDaQiaoXiaoQiaoLuoyan;

    ref readonly Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._daQiaoXiaoQiaoQualificationCache => ref _daQiaoXiaoQiaoQualificationCache;

    ref long Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._daQiaoXiaoQiaoQualificationRevision => ref _daQiaoXiaoQiaoQualificationRevision;

    ref bool Contracts.ICharacterDaQiaoXiaoQiaoOlDaQiaoXiaoQiaoQualificationCacheDaQiaoXiaoQiaoQualificationRevisionSynchronizingDaQiaoXiaoQiaoLuoyanAccess._synchronizingDaQiaoXiaoQiaoLuoyan => ref _synchronizingDaQiaoXiaoQiaoLuoyan;
}
