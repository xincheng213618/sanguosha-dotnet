#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepPoisonObserversEntries
{
    internal bool AdvancePrepPoisonObserver() => _resolve().AdvancePrepPoisonObserver();
    internal bool PrepPoisonFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().PrepPoisonFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? PrepPoisonObserverRoot(long? rootId = default) => _resolve().PrepPoisonObserverRoot(rootId);
}
