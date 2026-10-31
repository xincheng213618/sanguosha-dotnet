namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<int, ((long Base, long Grant, long Movement, int Hp, bool Alive, bool Concealed) Key, long Stamp)> _daQiaoXiaoQiaoQualificationCache => ref _characterDaQiaoXiaoQiaoOl.ReferenceToDaQiaoXiaoQiaoQualificationCache;
    private ref long _daQiaoXiaoQiaoQualificationRevision => ref _characterDaQiaoXiaoQiaoOl.ReferenceToDaQiaoXiaoQiaoQualificationRevision;
    private ref bool _synchronizingDaQiaoXiaoQiaoLuoyan => ref _characterDaQiaoXiaoQiaoOl.ReferenceToSynchronizingDaQiaoXiaoQiaoLuoyan;
}
