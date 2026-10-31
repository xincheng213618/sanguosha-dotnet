#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawDiscardCategoryRefundEntries
{
    internal void AssertDrawDiscardCategoryReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertDrawDiscardCategoryReceipt(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDrawDiscardCategoryRefund(global::CardGame.Core.ProgramSkillFrame supplied, int target, int amount, string stateId) => _resolve().BeginDrawDiscardCategoryRefund(supplied, target, amount, stateId);
    internal bool CanContinueDrawDiscardCategoryRefund(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueDrawDiscardCategoryRefund(f);
    internal global::CardGame.Core.ProgramSkillFrame? CategoryActivationObserverRoot(bool awakening) => _resolve().CategoryActivationObserverRoot(awakening);
    internal void CompleteDrawDiscardCategory(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteDrawDiscardCategory(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DrawDiscardCategoryChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrawDiscardCategoryChoices(f);
    internal bool DrawDiscardCategoryFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().DrawDiscardCategoryFirstChild(f, child);
    internal global::CardGame.Core.DrawDiscardCategoryMaterial[] DrawDiscardCategoryLegalMaterials(int seat) => _resolve().DrawDiscardCategoryLegalMaterials(seat);
    internal bool DrawDiscardCategoryTargetAvailable(int owner, string skill, string stateId, int target) => _resolve().DrawDiscardCategoryTargetAvailable(owner, skill, stateId, target);
    internal bool HasCategoryActivationDamageObserver(long id) => _resolve().HasCategoryActivationDamageObserver(id);
    internal bool IsDrawDiscardCategoryMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsDrawDiscardCategoryMovement(f, effect, pending);
    internal bool IsDrawDiscardCategoryPaidDestination(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementRecord movement) => _resolve().IsDrawDiscardCategoryPaidDestination(frame, movement);
    internal bool IsDrawDiscardCategoryProgramDying() => _resolve().IsDrawDiscardCategoryProgramDying();
    internal void IssueDrawDiscardCategoryDraw(global::CardGame.Core.ProgramSkillFrame f, bool bonus) => _resolve().IssueDrawDiscardCategoryDraw(f, bonus);
    internal void PayDrawDiscardCategory(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayDrawDiscardCategory(f);
    internal void PublishDrawDiscardCategoryChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDrawDiscardCategoryChoice(f);
    internal void RefundDrawDiscardCategory(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RefundDrawDiscardCategory(f);
    internal void ResolveDrawDiscardCategoryChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDrawDiscardCategoryChoice(choice);
    internal bool ResumeDrawDiscardCategoryRefund(long frameId) => _resolve().ResumeDrawDiscardCategoryRefund(frameId);
    internal bool ReturnDrawDiscardCategoryMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnDrawDiscardCategoryMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiDrawDiscardCategory(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiDrawDiscardCategory(decision, f);
    internal bool ValidDrawDiscardCategoryReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidDrawDiscardCategoryReceipt(f);
    internal long DrawDiscardCategorySequence { get => _resolve().DrawDiscardCategorySequence; }
}
