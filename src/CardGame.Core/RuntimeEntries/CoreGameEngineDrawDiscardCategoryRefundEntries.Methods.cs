#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawDiscardCategoryRefundEntries
{
    internal SkillProgramStepOutcome BeginDrawDiscardCategoryRefund(ProgramSkillFrame supplied, int target, int amount, string stateId) => _resolve().BeginDrawDiscardCategoryRefund(supplied, target, amount, stateId);
    internal bool CanContinueDrawDiscardCategoryRefund(ProgramSkillFrame f) => _resolve().CanContinueDrawDiscardCategoryRefund(f);
    internal ProgramSkillFrame? CategoryActivationObserverRoot(bool awakening) => _resolve().CategoryActivationObserverRoot(awakening);
    internal void CompleteDrawDiscardCategory(ProgramSkillFrame f) => _resolve().CompleteDrawDiscardCategory(f);
    internal IReadOnlyList<PromptChoice> DrawDiscardCategoryChoices(ProgramSkillFrame f) => _resolve().DrawDiscardCategoryChoices(f);
    internal bool DrawDiscardCategoryFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().DrawDiscardCategoryFirstChild(f, child);
    internal DrawDiscardCategoryMaterial[] DrawDiscardCategoryLegalMaterials(int seat) => _resolve().DrawDiscardCategoryLegalMaterials(seat);
    internal bool DrawDiscardCategoryTargetAvailable(int owner, string skill, string stateId, int target) => _resolve().DrawDiscardCategoryTargetAvailable(owner, skill, stateId, target);
    internal bool IsDrawDiscardCategoryPaidDestination(ProgramSkillFrame frame, CardMovementRecord movement) => _resolve().IsDrawDiscardCategoryPaidDestination(frame, movement);
    internal bool IsDrawDiscardCategoryProgramDying() => _resolve().IsDrawDiscardCategoryProgramDying();
    internal void IssueDrawDiscardCategoryDraw(ProgramSkillFrame f, bool bonus) => _resolve().IssueDrawDiscardCategoryDraw(f, bonus);
    internal void PayDrawDiscardCategory(ProgramSkillFrame f) => _resolve().PayDrawDiscardCategory(f);
    internal void PublishDrawDiscardCategoryChoice(ProgramSkillFrame f) => _resolve().PublishDrawDiscardCategoryChoice(f);
    internal void RefundDrawDiscardCategory(ProgramSkillFrame f) => _resolve().RefundDrawDiscardCategory(f);
    internal void ResolveDrawDiscardCategoryChoice(PromptChoice choice) => _resolve().ResolveDrawDiscardCategoryChoice(choice);
    internal bool ResumeDrawDiscardCategoryRefund(long frameId) => _resolve().ResumeDrawDiscardCategoryRefund(frameId);
    internal bool ReturnDrawDiscardCategoryMovement(ProgramSkillFrame f) => _resolve().ReturnDrawDiscardCategoryMovement(f);
    internal PromptChoice SelectAiDrawDiscardCategory(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiDrawDiscardCategory(decision, f);
    internal bool ValidDrawDiscardCategoryReceipt(ProgramSkillFrame f) => _resolve().ValidDrawDiscardCategoryReceipt(f);
    internal long DrawDiscardCategorySequence { get => _resolve().DrawDiscardCategorySequence; }
}
