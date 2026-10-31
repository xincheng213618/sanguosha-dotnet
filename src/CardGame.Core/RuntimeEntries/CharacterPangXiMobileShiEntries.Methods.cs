#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangXiMobileShiEntries
{
    internal void AnswerShiPx(PromptChoice selected) => _resolve().AnswerShiPx(selected);
    internal void ApplyShiPxKuangxiangSwap(ProgramSkillFrame f, int targetSeat) => _resolve().ApplyShiPxKuangxiangSwap(f, targetSeat);
    internal void ApplyShiPxRiderArm(ProgramSkillFrame f) => _resolve().ApplyShiPxRiderArm(f);
    internal void ApplyShiPxTopPlace(ProgramSkillFrame f, int cardId, int holder) => _resolve().ApplyShiPxTopPlace(f, cardId, holder);
    internal void ApplyShiPxXuyeDraw(ProgramSkillFrame f) => _resolve().ApplyShiPxXuyeDraw(f);
    internal SkillProgramStepOutcome BeginShiPxKuangxiang(ProgramSkillFrame input) => _resolve().BeginShiPxKuangxiang(input);
    internal SkillProgramStepOutcome BeginShiPxRider(ProgramSkillFrame input) => _resolve().BeginShiPxRider(input);
    internal SkillProgramStepOutcome BeginShiPxSweep(ProgramSkillFrame input) => _resolve().BeginShiPxSweep(input);
    internal SkillProgramStepOutcome BeginShiPxXuye(ProgramSkillFrame input) => _resolve().BeginShiPxXuye(input);
    internal PromptChoice ChooseShiPxKuangxiangAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiPxKuangxiangAi(decision, f);
    internal PromptChoice ChooseShiPxRiderAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiPxRiderAi(decision, f);
    internal PromptChoice ChooseShiPxSweepAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiPxSweepAi(decision, f);
    internal PromptChoice ChooseShiPxXuyeAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().ChooseShiPxXuyeAi(decision, f);
    internal IEnumerable<ProgramTriggerCandidate> CollectShiPxRiderCandidates(CardMovementBatchContext batch) => _resolve().CollectShiPxRiderCandidates(batch);
    internal void CollectWindow_CollectShiPxRiderCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectShiPxRiderCandidates(batch, result);
    internal void ContinueShiPx(long id) => _resolve().ContinueShiPx(id);
    internal bool DrainShiPx(ProgramSkillFrame f) => _resolve().DrainShiPx(f);
    internal bool ExcludeOrdinaryMovement_IsShiPxRiderTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsShiPxRiderTrigger(candidate, trigger);
    internal void FinishShiPxOperation(ProgramSkillFrame f) => _resolve().FinishShiPxOperation(f);
    internal PromptChoice MakeShiPxChoice(ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiPxChoice(f, branch, label);
    internal void PublishShiPx(ProgramSkillFrame f) => _resolve().PublishShiPx(f);
    internal bool ResumeShiPx(long id) => _resolve().ResumeShiPx(id);
    internal bool ReturnShiPxMovement(ProgramSkillFrame f) => _resolve().ReturnShiPxMovement(f);
    internal ShiPxKuangxiangSwappedEvent? ShiPxActiveSwap(int seat) => _resolve().ShiPxActiveSwap(seat);
    internal IReadOnlyList<int> ShiPxAreaCardIds(int seat) => _resolve().ShiPxAreaCardIds(seat);
    internal IReadOnlyList<PromptChoice> ShiPxChoices(ProgramSkillFrame f) => _resolve().ShiPxChoices(f);
    internal bool ShiPxIsGlobalMaximumHand(int seat) => _resolve().ShiPxIsGlobalMaximumHand(seat);
    internal bool ShiPxIsGlobalMinimumHand(int seat) => _resolve().ShiPxIsGlobalMinimumHand(seat);
    internal string? ShiPxRiderSide(ShiPxKuangxiangSwappedEvent swap, IReadOnlyList<CardMovementRecord> movements) => _resolve().ShiPxRiderSide(swap, movements);
    internal bool HasShiPxCapability { get => _resolve().HasShiPxCapability; }
}
