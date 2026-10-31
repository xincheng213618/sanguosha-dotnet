#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepPoisonObserversEntries
{
    internal bool AdvancePrepPoisonObserver() => _resolve().AdvancePrepPoisonObserver();
    internal bool PrepPoisonFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().PrepPoisonFirstChild(root, child);
    internal ProgramSkillFrame? PrepPoisonObserverRoot(long? rootId = default) => _resolve().PrepPoisonObserverRoot(rootId);
}
