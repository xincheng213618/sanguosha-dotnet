#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangYiMobileEntries
{
    internal void BeginDeckTopJudgmentReplacementChoice(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.JudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().BeginDeckTopJudgmentReplacementChoice(pending, candidate, program, trigger);
    internal bool HasDeckTopJudgmentReplacementMaterial() => _resolve().HasDeckTopJudgmentReplacementMaterial();
    internal bool IsDeckTopJudgmentReplacementInput(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsDeckTopJudgmentReplacementInput(judgment, owner, card);
    internal bool IsDeckTopJudgmentReplacementPromptValid(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.PendingDecision? decision) => _resolve().IsDeckTopJudgmentReplacementPromptValid(judgment, decision);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> MobileWangYiPrivateMijiCards(int viewerSeat) => _resolve().MobileWangYiPrivateMijiCards(viewerSeat);
    internal bool TryResolveAiDeckTopJudgmentReplacement() => _resolve().TryResolveAiDeckTopJudgmentReplacement();
    internal bool TryResolveDeckTopJudgmentReplacement(global::CardGame.Core.PromptChoice selected, global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.JudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgram program, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().TryResolveDeckTopJudgmentReplacement(selected, pending, candidate, program, trigger);
}
