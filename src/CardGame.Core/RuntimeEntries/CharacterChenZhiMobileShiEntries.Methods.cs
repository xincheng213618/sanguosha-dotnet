#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenZhiMobileShiEntries
{
    internal void AnswerShiCz(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShiCz(choice);
    internal void AssertShiCzFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertShiCzFrame(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiCz(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShiCz(input, op);
    internal bool CanOfferShiCz(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferShiCz(gate);
    internal void CaptureShiCzFirstDiscard(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureShiCzFirstDiscard(batch);
    internal global::CardGame.Core.PromptChoice ChooseShiCzAi(global::CardGame.Core.PendingDecision pending, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiCzAi(pending, f);
    internal void CompleteShiCz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteShiCz(f);
    internal void ContinueShiCz(long id) => _resolve().ContinueShiCz(id);
    internal bool DrainShiCz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiCz(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeShiCzExtraStart(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => _resolve().IncludeShiCzExtraStart(owner, window, ordinary);
    internal bool IsIssuedShiCzExtraStart(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShiCzExtraStart(candidate, context);
    internal void IssueShiCzDraw(global::CardGame.Core.ProgramSkillFrame input, int seat, global::CardGame.Core.MobileShiChenZhiStage stage) => _resolve().IssueShiCzDraw(input, seat, stage);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiChenZhiPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().MobileShiChenZhiPublicStates(owner, skillId);
    internal bool PaidShiCzCanReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PaidShiCzCanReturn(f);
    internal void PublishShiCz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiCz(f);
    internal bool ResumeShiCz(long id) => _resolve().ResumeShiCz(id);
    internal bool ReturnShiCz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiCz(f);
    internal void SealShiCzMovement(long id) => _resolve().SealShiCzMovement(id);
    internal int[]? SelectShiCzFirstDiscard(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().SelectShiCzFirstDiscard(batch, candidate, features);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiCzChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiCzChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiCzDiscardTargets(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShiCzDiscardTargets(frame);
    internal bool ShiCzMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().ShiCzMovementProof(f, effect, movement);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiCzPaymentChoices(global::CardGame.Core.ProgramSkillFrame f, int holder) => _resolve().ShiCzPaymentChoices(f, holder);
    internal global::CardGame.Core.MobileShiChenZhiExtraGrantedEvent? ShiCzPendingExtra(int owner) => _resolve().ShiCzPendingExtra(owner);
    internal bool ShiCzUsedOrPlayedSlash(int seat) => _resolve().ShiCzUsedOrPlayedSlash(seat);
}
