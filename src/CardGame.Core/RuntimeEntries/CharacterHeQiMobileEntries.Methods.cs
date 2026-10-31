#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeQiMobileEntries
{
    internal void AnswerMobileHeQiView(global::CardGame.Core.PromptChoice c) => _resolve().AnswerMobileHeQiView(c);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileHeQiView(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMobileHeQiView(supplied);
    internal bool HasMobileHeQiPrompt() => _resolve().HasMobileHeQiPrompt();
    internal global::CardGame.Core.PromptChoice MobileHeQiAi(global::CardGame.Core.PendingDecision p) => _resolve().MobileHeQiAi(p);
    internal global::CardGame.Core.PromptChoice MobileHeQiConfirm(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileHeQiConfirm(f);
    internal global::CardGame.Core.NativeLogicalDiscardMaterialCompletedEvent? MobileHeQiDiscard(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.NativeLogicalDiscardInvocationRecordedEvent? invoice) => _resolve().MobileHeQiDiscard(f, out invoice);
    internal bool MobileHeQiOriginal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileHeQiOriginal(f);
    internal global::CardGame.Core.CardSnapshot[] MobileHeQiPrivateHand(int viewer) => _resolve().MobileHeQiPrivateHand(viewer);
    internal void PublishMobileHeQiView(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileHeQiView(f);
    internal bool ValidMobileHeQiView(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMobileHeQiView(f);
}
