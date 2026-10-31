#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeQiMobileEntries
{
    internal void AnswerMobileHeQiView(PromptChoice c) => _resolve().AnswerMobileHeQiView(c);
    internal SkillProgramStepOutcome BeginMobileHeQiView(ProgramSkillFrame supplied) => _resolve().BeginMobileHeQiView(supplied);
    internal bool HasMobileHeQiPrompt() => _resolve().HasMobileHeQiPrompt();
    internal PromptChoice MobileHeQiAi(PendingDecision p) => _resolve().MobileHeQiAi(p);
    internal PromptChoice MobileHeQiConfirm(ProgramSkillFrame f) => _resolve().MobileHeQiConfirm(f);
    internal NativeLogicalDiscardMaterialCompletedEvent? MobileHeQiDiscard(ProgramSkillFrame f, scoped out NativeLogicalDiscardInvocationRecordedEvent? invoice) => _resolve().MobileHeQiDiscard(f, out invoice);
    internal bool MobileHeQiOriginal(ProgramSkillFrame f) => _resolve().MobileHeQiOriginal(f);
    internal CardSnapshot[] MobileHeQiPrivateHand(int viewer) => _resolve().MobileHeQiPrivateHand(viewer);
    internal void PublishMobileHeQiView(ProgramSkillFrame f) => _resolve().PublishMobileHeQiView(f);
    internal bool ValidMobileHeQiView(ProgramSkillFrame f) => _resolve().ValidMobileHeQiView(f);
}
