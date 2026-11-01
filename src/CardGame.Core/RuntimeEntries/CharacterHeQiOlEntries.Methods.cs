#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHeQiOlEntries
{
    internal SkillProgramStepOutcome BeginHeQi(ProgramSkillFrame supplied, int target) => _resolve().BeginHeQi(supplied, target);
    internal bool CanContinueHeQi(ProgramSkillFrame f) => _resolve().CanContinueHeQi(f);
    internal void ContinueHeQiAfterPublicReveal(ProgramSkillFrame f) => _resolve().ContinueHeQiAfterPublicReveal(f);
    internal void FinishHeQi(ProgramSkillFrame f) => _resolve().FinishHeQi(f);
    internal bool HeQiActivationAllowed(CharacterState owner, SkillProgramActivation a) => _resolve().HeQiActivationAllowed(owner, a);
    internal bool HeQiActivationTargetAllowed(CharacterState owner, CharacterState target, SkillProgramActivation a) => _resolve().HeQiActivationTargetAllowed(owner, target, a);
    internal PromptChoice HeQiAiChoice(ProgramSkillFrame f) => _resolve().HeQiAiChoice(f);
    internal PromptChoice HeQiChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = default) => _resolve().HeQiChoice(f, branch, label, cards);
    internal IReadOnlyList<PromptChoice> HeQiChoices(ProgramSkillFrame f) => _resolve().HeQiChoices(f);
    internal int HeQiEmptySlots(int seat) => _resolve().HeQiEmptySlots(seat);
    internal bool HeQiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().HeQiFirstChild(root, child);
    internal ProgramSkillFrame? HeQiObserverRoot() => _resolve().HeQiObserverRoot();
    internal bool HeQiTargetAllowed(int owner, int target) => _resolve().HeQiTargetAllowed(owner, target);
    internal bool IsHeQiActivation(SkillProgramActivation a) => _resolve().IsHeQiActivation(a);
    internal void IssueHeQiSegment(long id, HeQiReceipt r) => _resolve().IssueHeQiSegment(id, r);
    internal void PublishHeQiChoice(ProgramSkillFrame f) => _resolve().PublishHeQiChoice(f);
    internal void RequireHeQi(ProgramSkillFrame f) => _resolve().RequireHeQi(f);
    internal void ResolveHeQiChoice(PromptChoice choice) => _resolve().ResolveHeQiChoice(choice);
    internal bool ResumeHeQi(long id) => _resolve().ResumeHeQi(id);
    internal bool ReturnHeQiMovement(ProgramSkillFrame f) => _resolve().ReturnHeQiMovement(f);
    internal bool TryAdvanceHeQiSubtree() => _resolve().TryAdvanceHeQiSubtree();
}
