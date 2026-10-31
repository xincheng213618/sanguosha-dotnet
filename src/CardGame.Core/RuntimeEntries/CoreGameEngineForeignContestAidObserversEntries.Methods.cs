#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignContestAidObserversEntries
{
    internal void AssertSameTypeAid(ProgramSkillFrame f) => _resolve().AssertSameTypeAid(f);
    internal ProgramSkillFrame? ForeignContestAidPaidObserverRoot(long? exactDamageWindow = default) => _resolve().ForeignContestAidPaidObserverRoot(exactDamageWindow);
    internal bool IsForeignContestAidProgramDying() => _resolve().IsForeignContestAidProgramDying();
    internal bool IsValidSameTypeAidPayment(ProgramSkillFrame f) => _resolve().IsValidSameTypeAidPayment(f);
    internal bool SameTypeAidParentMatches(ProgramSkillFrame f) => _resolve().SameTypeAidParentMatches(f);
    internal bool TryGetSameTypeAidVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, scoped out IReadOnlyList<int> primary) => _resolve().TryGetSameTypeAidVirtualPrimaryReturn(use, parent, out primary);
}
