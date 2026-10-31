#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategoryMarkObserversEntries
{
    internal void AssertRecipientCategoryMark(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertRecipientCategoryMark(f);
    internal void AssertRecipientCategoryMarkState() => _resolve().AssertRecipientCategoryMarkState();
    internal bool ExactRecipientCategoryParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactRecipientCategoryParent(f);
    internal bool IsRecipientCategoryGiftDrawContext(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsRecipientCategoryGiftDrawContext(candidate, c);
    internal bool IsRecipientCategoryMarkChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PendingDecision p) => _resolve().IsRecipientCategoryMarkChoice(f, p);
    internal bool RecipientCategoryMarkFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().RecipientCategoryMarkFirstChild(f, child);
    internal bool RecipientCategoryMarkStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().RecipientCategoryMarkStructuralEdge(parent, child);
    internal bool RecipientCategoryPaidLionOrOx(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardKind kind) => _resolve().RecipientCategoryPaidLionOrOx(f, kind);
    internal bool ValidRecipientCategoryMarkReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidRecipientCategoryMarkReceipt(f);
}
