#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignContestAidObserversEntries
{
    internal void AssertSameTypeAid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertSameTypeAid(f);
    internal global::CardGame.Core.ProgramSkillFrame? ForeignContestAidPaidObserverRoot(long? exactDamageWindow = default) => _resolve().ForeignContestAidPaidObserverRoot(exactDamageWindow);
    internal bool IsForeignContestAidProgramDying() => _resolve().IsForeignContestAidProgramDying();
    internal bool IsValidSameTypeAidPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsValidSameTypeAidPayment(f);
    internal bool SameTypeAidParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SameTypeAidParentMatches(f);
    internal bool TryGetSameTypeAidVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> primary) => _resolve().TryGetSameTypeAidVirtualPrimaryReturn(use, parent, out primary);
}
