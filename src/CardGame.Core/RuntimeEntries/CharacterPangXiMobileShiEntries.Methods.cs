#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangXiMobileShiEntries
{
    internal void AnswerShiPx(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShiPx(selected);
    internal void ApplyShiPxKuangxiangSwap(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().ApplyShiPxKuangxiangSwap(f, targetSeat);
    internal void ApplyShiPxRiderArm(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyShiPxRiderArm(f);
    internal void ApplyShiPxTopPlace(global::CardGame.Core.ProgramSkillFrame f, int cardId, int holder) => _resolve().ApplyShiPxTopPlace(f, cardId, holder);
    internal void ApplyShiPxXuyeDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyShiPxXuyeDraw(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiPxKuangxiang(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiPxKuangxiang(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiPxRider(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiPxRider(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiPxSweep(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiPxSweep(input);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiPxXuye(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginShiPxXuye(input);
    internal global::CardGame.Core.PromptChoice ChooseShiPxKuangxiangAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiPxKuangxiangAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiPxRiderAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiPxRiderAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiPxSweepAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiPxSweepAi(decision, f);
    internal global::CardGame.Core.PromptChoice ChooseShiPxXuyeAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiPxXuyeAi(decision, f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectShiPxRiderCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectShiPxRiderCandidates(batch);
    internal void CollectWindow_CollectShiPxRiderCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectShiPxRiderCandidates(batch, result);
    internal void ContinueShiPx(long id) => _resolve().ContinueShiPx(id);
    internal bool DrainShiPx(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiPx(f);
    internal bool ExcludeOrdinaryMovement_IsShiPxRiderTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsShiPxRiderTrigger(candidate, trigger);
    internal void FinishShiPxOperation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiPxOperation(f);
    internal global::CardGame.Core.PromptChoice MakeShiPxChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label) => _resolve().MakeShiPxChoice(f, branch, label);
    internal void PublishShiPx(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiPx(f);
    internal bool ResumeShiPx(long id) => _resolve().ResumeShiPx(id);
    internal bool ReturnShiPxMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiPxMovement(f);
    internal global::CardGame.Core.ShiPxKuangxiangSwappedEvent? ShiPxActiveSwap(int seat) => _resolve().ShiPxActiveSwap(seat);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiPxAreaCardIds(int seat) => _resolve().ShiPxAreaCardIds(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiPxChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiPxChoices(f);
    internal bool ShiPxIsGlobalMaximumHand(int seat) => _resolve().ShiPxIsGlobalMaximumHand(seat);
    internal bool ShiPxIsGlobalMinimumHand(int seat) => _resolve().ShiPxIsGlobalMinimumHand(seat);
    internal string? ShiPxRiderSide(global::CardGame.Core.ShiPxKuangxiangSwappedEvent swap, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().ShiPxRiderSide(swap, movements);
    internal bool HasShiPxCapability { get => _resolve().HasShiPxCapability; }
}
