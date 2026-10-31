#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhaoguMirrorEntries
{
    internal void AssertZhaoguMirror(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? paused) => _resolve().AssertZhaoguMirror(frame, paused);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhaoguMirror(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginZhaoguMirror(input);
    internal bool CanOfferZhaoguMirror(global::CardGame.Core.ProgramTriggerLegalityContext context) => _resolve().CanOfferZhaoguMirror(context);
    internal void CancelZhaogu(global::CardGame.Core.PromptChoice selected) => _resolve().CancelZhaogu(selected);
    internal global::CardGame.Core.PromptChoice ChooseZhaoguMirror(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseZhaoguMirror(prompt, frame);
    internal void CompleteZhaogu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteZhaogu(frame);
    internal bool CompleteZhaoguAttackCleanup() => _resolve().CompleteZhaoguAttackCleanup();
    internal global::CardGame.Core.ProgramSkillFrame CompleteZhaoguVirtualBasicReceipt(global::CardGame.Core.ProgramSkillFrame frame, long childId) => _resolve().CompleteZhaoguVirtualBasicReceipt(frame, childId);
    internal bool ContinueIssuedZhaogu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueIssuedZhaogu(frame);
    internal void ContinueZhaoguAfterReveal(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueZhaoguAfterReveal(frame);
    internal bool IsZhaoguVirtualBasicDraft(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramVirtualBasicDraft draft) => _resolve().IsZhaoguVirtualBasicDraft(frame, draft);
    internal bool IsZhaoguVirtualOrdinaryTrickUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsZhaoguVirtualOrdinaryTrickUse(use);
    internal void PublishZhaoguChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishZhaoguChoice(frame);
    internal void ResolveZhaoguTemplate(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZhaoguTemplate(selected);
    internal void ResolveZhaoguUse(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZhaoguUse(selected);
    internal void ReturnZhaoguVirtualOrdinaryTrickUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnZhaoguVirtualOrdinaryTrickUse(use);
    internal bool ValidZhaoguHeader(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidZhaoguHeader(frame);
    internal global::CardGame.Core.PromptChoice ZhaoguChoice(global::CardGame.Core.ProgramSkillFrame frame, string action, string token, string label, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ZhaoguChoice(frame, action, token, label, cards, targets);
    internal global::CardGame.Core.ProgramSkillFrame ZhaoguChoiceFrame(global::CardGame.Core.PromptChoice selected, global::CardGame.Core.ZhaoguMirrorStage stage) => _resolve().ZhaoguChoiceFrame(selected, stage);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhaoguChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ZhaoguChoices(frame);
    internal global::CardGame.Core.CardConversionSource ZhaoguSource(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ZhaoguSource(frame);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> ZhaoguTemplates(int seat) => _resolve().ZhaoguTemplates(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> ZhaoguTrickOptions(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ZhaoguTrickOptions(frame);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardKind Kind, int Target)> ZhaoguVirtualBasicOptions(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ZhaoguVirtualBasicOptions(frame);
    internal bool IsZhaoguMode { get => _resolve().IsZhaoguMode; }
}
