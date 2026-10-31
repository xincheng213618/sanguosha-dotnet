#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenZhiMobileShiEntries
{
    internal void AnswerShiCz(PromptChoice choice) => _resolve().AnswerShiCz(choice);
    internal SkillProgramStepOutcome BeginShiCz(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginShiCz(input, op);
    internal bool CanOfferShiCz(ProgramTriggerLegalityContext gate) => _resolve().CanOfferShiCz(gate);
    internal void CaptureShiCzFirstDiscard(CardMovementBatchContext batch) => _resolve().CaptureShiCzFirstDiscard(batch);
    internal PromptChoice ChooseShiCzAi(PendingDecision pending, ProgramSkillFrame f) => _resolve().ChooseShiCzAi(pending, f);
    internal void CompleteShiCz(ProgramSkillFrame f) => _resolve().CompleteShiCz(f);
    internal void ContinueShiCz(long id) => _resolve().ContinueShiCz(id);
    internal bool DrainShiCz(ProgramSkillFrame f) => _resolve().DrainShiCz(f);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeShiCzExtraStart(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeShiCzExtraStart(owner, window, ordinary);
    internal bool IsIssuedShiCzExtraStart(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedShiCzExtraStart(candidate, context);
    internal void IssueShiCzDraw(ProgramSkillFrame input, int seat, MobileShiChenZhiStage stage) => _resolve().IssueShiCzDraw(input, seat, stage);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiChenZhiPublicStates(CharacterState owner, string skillId) => _resolve().MobileShiChenZhiPublicStates(owner, skillId);
    internal bool PaidShiCzCanReturn(ProgramSkillFrame f) => _resolve().PaidShiCzCanReturn(f);
    internal void PublishShiCz(ProgramSkillFrame f) => _resolve().PublishShiCz(f);
    internal bool ResumeShiCz(long id) => _resolve().ResumeShiCz(id);
    internal bool ReturnShiCz(ProgramSkillFrame f) => _resolve().ReturnShiCz(f);
    internal void SealShiCzMovement(long id) => _resolve().SealShiCzMovement(id);
    internal int[]? SelectShiCzFirstDiscard(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => _resolve().SelectShiCzFirstDiscard(batch, candidate, features);
    internal IReadOnlyList<PromptChoice> ShiCzChoices(ProgramSkillFrame f) => _resolve().ShiCzChoices(f);
    internal IReadOnlyList<int> ShiCzDiscardTargets(ProgramSkillFrame frame) => _resolve().ShiCzDiscardTargets(frame);
    internal IReadOnlyList<PromptChoice> ShiCzPaymentChoices(ProgramSkillFrame f, int holder) => _resolve().ShiCzPaymentChoices(f, holder);
    internal MobileShiChenZhiExtraGrantedEvent? ShiCzPendingExtra(int owner) => _resolve().ShiCzPendingExtra(owner);
    internal bool ShiCzUsedOrPlayedSlash(int seat) => _resolve().ShiCzUsedOrPlayedSlash(seat);
}
