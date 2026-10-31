#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineZhaoguMirrorEntries
{
    internal SkillProgramStepOutcome BeginZhaoguMirror(ProgramSkillFrame input) => _resolve().BeginZhaoguMirror(input);
    internal bool CanOfferZhaoguMirror(ProgramTriggerLegalityContext context) => _resolve().CanOfferZhaoguMirror(context);
    internal void CancelZhaogu(PromptChoice selected) => _resolve().CancelZhaogu(selected);
    internal PromptChoice ChooseZhaoguMirror(PendingDecision prompt, ProgramSkillFrame frame) => _resolve().ChooseZhaoguMirror(prompt, frame);
    internal void CompleteZhaogu(ProgramSkillFrame frame) => _resolve().CompleteZhaogu(frame);
    internal bool CompleteZhaoguAttackCleanup() => _resolve().CompleteZhaoguAttackCleanup();
    internal ProgramSkillFrame CompleteZhaoguVirtualBasicReceipt(ProgramSkillFrame frame, long childId) => _resolve().CompleteZhaoguVirtualBasicReceipt(frame, childId);
    internal bool ContinueIssuedZhaogu(ProgramSkillFrame frame) => _resolve().ContinueIssuedZhaogu(frame);
    internal void ContinueZhaoguAfterReveal(ProgramSkillFrame frame) => _resolve().ContinueZhaoguAfterReveal(frame);
    internal bool IsZhaoguVirtualBasicDraft(ProgramSkillFrame frame, ProgramVirtualBasicDraft draft) => _resolve().IsZhaoguVirtualBasicDraft(frame, draft);
    internal bool IsZhaoguVirtualOrdinaryTrickUse(CardUseFrame use) => _resolve().IsZhaoguVirtualOrdinaryTrickUse(use);
    internal void PublishZhaoguChoice(ProgramSkillFrame frame) => _resolve().PublishZhaoguChoice(frame);
    internal void ResolveZhaoguTemplate(PromptChoice selected) => _resolve().ResolveZhaoguTemplate(selected);
    internal void ResolveZhaoguUse(PromptChoice selected) => _resolve().ResolveZhaoguUse(selected);
    internal void ReturnZhaoguVirtualOrdinaryTrickUse(CardUseFrame use) => _resolve().ReturnZhaoguVirtualOrdinaryTrickUse(use);
    internal bool ValidZhaoguHeader(ProgramSkillFrame frame) => _resolve().ValidZhaoguHeader(frame);
    internal PromptChoice ZhaoguChoice(ProgramSkillFrame frame, string action, string token, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().ZhaoguChoice(frame, action, token, label, cards, targets);
    internal ProgramSkillFrame ZhaoguChoiceFrame(PromptChoice selected, ZhaoguMirrorStage stage) => _resolve().ZhaoguChoiceFrame(selected, stage);
    internal IReadOnlyList<PromptChoice> ZhaoguChoices(ProgramSkillFrame frame) => _resolve().ZhaoguChoices(frame);
    internal CardConversionSource ZhaoguSource(ProgramSkillFrame frame) => _resolve().ZhaoguSource(frame);
    internal IEnumerable<Card> ZhaoguTemplates(int seat) => _resolve().ZhaoguTemplates(seat);
    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> ZhaoguTrickOptions(ProgramSkillFrame frame) => _resolve().ZhaoguTrickOptions(frame);
    internal IEnumerable<(CardKind Kind, int Target)> ZhaoguVirtualBasicOptions(ProgramSkillFrame frame) => _resolve().ZhaoguVirtualBasicOptions(frame);
    internal bool IsZhaoguMode { get => _resolve().IsZhaoguMode; }
}
