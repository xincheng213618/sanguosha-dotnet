#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoJiaMouEntries
{
    internal void BeginMouGjJudgment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMouGjJudgment(f);
    internal bool IsMouGjJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsMouGjJudgment(j, f);
    internal bool IsMouGjLightningJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().IsMouGjLightningJudgment(j);
    internal bool MouGjAdvanceChildren() => _resolve().MouGjAdvanceChildren();
    internal bool MouGjDying() => _resolve().MouGjDying();
    internal bool MouGjEdge(int i) => _resolve().MouGjEdge(i);
    internal bool MouGjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouGjFirstChild(f, child);
    internal bool MouGjJudgmentEdge(int i) => _resolve().MouGjJudgmentEdge(i);
    internal bool MouGjJudgmentRequest(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => _resolve().MouGjJudgmentRequest(f, target, reason, bind, visibility);
    internal bool MouGjLunshiBan(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().MouGjLunshiBan(q);
    internal bool MouGjLunshiEligible(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> cards) => _resolve().MouGjLunshiEligible(actor, cards);
    internal global::CardGame.Core.CardConversionSource? MouGjLunshiSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cards, global::CardGame.Core.CardConversionSource? selected) => _resolve().MouGjLunshiSource(owner, cards, selected);
    internal bool MouGjMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouGjMovement(f, e, p);
    internal bool MouGjParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouGjParent(f);
    internal global::CardGame.Core.CardSnapshot[] MouGjPrivateCards(int viewer) => _resolve().MouGjPrivateCards(viewer);
    internal bool MouGjRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouGjRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouGjRoot() => _resolve().MouGjRoot();
    internal bool MouGjSuffix(int n) => _resolve().MouGjSuffix(n);
    internal bool ReturnMouGjJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().ReturnMouGjJudgment(j);
    internal global::CardGame.Core.MouGuoJiaRuntime MouGuoJia { get => _resolve().MouGuoJia; }
}
