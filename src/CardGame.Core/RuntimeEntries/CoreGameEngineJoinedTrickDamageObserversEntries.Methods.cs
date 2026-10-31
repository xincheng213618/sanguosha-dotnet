#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJoinedTrickDamageObserversEntries
{
    internal bool IsJoinedTrickDamageRewardDying() => _resolve().IsJoinedTrickDamageRewardDying();
    internal bool JoinedTrickDamageRewardFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().JoinedTrickDamageRewardFirstChild(frame, child);
    internal ProgramSkillFrame? JoinedTrickDamageRewardObserverRoot() => _resolve().JoinedTrickDamageRewardObserverRoot();
    internal bool JoinedTrickDamageRewardStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().JoinedTrickDamageRewardStructuralEdge(parent, child);
    internal bool TryAdvanceJoinedTrickDamageRewardSubtree() => _resolve().TryAdvanceJoinedTrickDamageRewardSubtree();
}
