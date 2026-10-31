#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnequalHandHpObserversEntries
{
    internal bool HasUnequalHandHpDamageObserver(long id) => _resolve().HasUnequalHandHpDamageObserver(id);
    internal bool IsUnequalHandHpDying() => _resolve().IsUnequalHandHpDying();
    internal bool TryAdvanceUnequalHandHpSubtree() => _resolve().TryAdvanceUnequalHandHpSubtree();
    internal bool UnequalHandHpFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().UnequalHandHpFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? UnequalHandHpObserverRoot() => _resolve().UnequalHandHpObserverRoot();
}
