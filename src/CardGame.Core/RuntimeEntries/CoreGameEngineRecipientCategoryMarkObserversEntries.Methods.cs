#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategoryMarkObserversEntries
{
    internal bool ExactRecipientCategoryParent(ProgramSkillFrame f) => _resolve().ExactRecipientCategoryParent(f);
    internal bool IsRecipientCategoryGiftDrawContext(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().IsRecipientCategoryGiftDrawContext(candidate, c);
    internal bool IsRecipientCategoryMarkChoice(ProgramSkillFrame f, PendingDecision p) => _resolve().IsRecipientCategoryMarkChoice(f, p);
    internal bool RecipientCategoryMarkFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().RecipientCategoryMarkFirstChild(f, child);
    internal bool RecipientCategoryMarkStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().RecipientCategoryMarkStructuralEdge(parent, child);
    internal bool RecipientCategoryPaidLionOrOx(ProgramSkillFrame f, CardKind kind) => _resolve().RecipientCategoryPaidLionOrOx(f, kind);
    internal bool ValidRecipientCategoryMarkReceipt(ProgramSkillFrame f) => _resolve().ValidRecipientCategoryMarkReceipt(f);
}
