#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangYiMobileEntries
{
    internal void BeginDeckTopJudgmentReplacementChoice(JudgmentFrame pending, JudgmentTriggerCandidate candidate, SkillProgram program, SkillProgramTrigger trigger) => _resolve().BeginDeckTopJudgmentReplacementChoice(pending, candidate, program, trigger);
    internal bool HasDeckTopJudgmentReplacementMaterial() => _resolve().HasDeckTopJudgmentReplacementMaterial();
    internal bool IsDeckTopJudgmentReplacementInput(JudgmentFrame judgment, CharacterState owner, Card card) => _resolve().IsDeckTopJudgmentReplacementInput(judgment, owner, card);
    internal bool IsDeckTopJudgmentReplacementPromptValid(JudgmentFrame judgment, PendingDecision? decision) => _resolve().IsDeckTopJudgmentReplacementPromptValid(judgment, decision);
    internal IEnumerable<CardSnapshot> MobileWangYiPrivateMijiCards(int viewerSeat) => _resolve().MobileWangYiPrivateMijiCards(viewerSeat);
    internal bool TryResolveAiDeckTopJudgmentReplacement() => _resolve().TryResolveAiDeckTopJudgmentReplacement();
    internal bool TryResolveDeckTopJudgmentReplacement(PromptChoice selected, JudgmentFrame pending, JudgmentTriggerCandidate candidate, SkillProgram program, SkillProgramTrigger trigger) => _resolve().TryResolveDeckTopJudgmentReplacement(selected, pending, candidate, program, trigger);
}
