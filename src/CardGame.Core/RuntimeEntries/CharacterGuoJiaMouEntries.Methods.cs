#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoJiaMouEntries
{
    internal void BeginMouGjJudgment(ProgramSkillFrame f) => _resolve().BeginMouGjJudgment(f);
    internal bool IsMouGjJudgment(JudgmentFrame j, ProgramSkillFrame f) => _resolve().IsMouGjJudgment(j, f);
    internal bool IsMouGjLightningJudgment(JudgmentFrame j) => _resolve().IsMouGjLightningJudgment(j);
    internal bool MouGjAdvanceChildren() => _resolve().MouGjAdvanceChildren();
    internal bool MouGjDying() => _resolve().MouGjDying();
    internal bool MouGjEdge(int i) => _resolve().MouGjEdge(i);
    internal bool MouGjFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouGjFirstChild(f, child);
    internal bool MouGjJudgmentEdge(int i) => _resolve().MouGjJudgmentEdge(i);
    internal bool MouGjJudgmentRequest(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _resolve().MouGjJudgmentRequest(f, target, reason, bind, visibility);
    internal bool MouGjLunshiBan(ProgramViewAsLegalityQuery q) => _resolve().MouGjLunshiBan(q);
    internal bool MouGjLunshiEligible(CharacterState actor, IReadOnlyList<int> cards) => _resolve().MouGjLunshiEligible(actor, cards);
    internal CardConversionSource? MouGjLunshiSource(CharacterState owner, IReadOnlyList<int> cards, CardConversionSource? selected) => _resolve().MouGjLunshiSource(owner, cards, selected);
    internal bool MouGjMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().MouGjMovement(f, e, p);
    internal bool MouGjParent(ProgramSkillFrame f) => _resolve().MouGjParent(f);
    internal CardSnapshot[] MouGjPrivateCards(int viewer) => _resolve().MouGjPrivateCards(viewer);
    internal bool MouGjRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouGjRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouGjRoot() => _resolve().MouGjRoot();
    internal bool MouGjSuffix(int n) => _resolve().MouGjSuffix(n);
    internal bool ReturnMouGjJudgment(JudgmentFrame j) => _resolve().ReturnMouGjJudgment(j);
    internal MouGuoJiaRuntime MouGuoJia { get => _resolve().MouGuoJia; }
}
