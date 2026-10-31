#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJoinedTrickDamageObserversEntries
{
    internal bool IsJoinedTrickDamageRewardDying() => _resolve().IsJoinedTrickDamageRewardDying();
    internal bool JoinedTrickDamageRewardFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().JoinedTrickDamageRewardFirstChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? JoinedTrickDamageRewardObserverRoot() => _resolve().JoinedTrickDamageRewardObserverRoot();
    internal bool JoinedTrickDamageRewardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().JoinedTrickDamageRewardStructuralEdge(parent, child);
    internal bool TryAdvanceJoinedTrickDamageRewardSubtree() => _resolve().TryAdvanceJoinedTrickDamageRewardSubtree();
}
