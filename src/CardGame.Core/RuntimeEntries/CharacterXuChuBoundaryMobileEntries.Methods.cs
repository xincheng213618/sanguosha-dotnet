#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuChuBoundaryMobileEntries
{
    internal bool BoundaryXuChuCommittedDraw(ProgramSkillFrame f) => _resolve().BoundaryXuChuCommittedDraw(f);
    internal ProgramLifecycleTriggerWindowFrame BoundaryXuChuDrawParent(ProgramSkillFrame f) => _resolve().BoundaryXuChuDrawParent(f);
    internal bool ContinueBoundaryXuChuCommittedDraw(ProgramSkillFrame f) => _resolve().ContinueBoundaryXuChuCommittedDraw(f);
    internal bool ReturnBoundaryXuChuCommittedMovement(ProgramSkillFrame f) => _resolve().ReturnBoundaryXuChuCommittedMovement(f);
    internal void SubstituteBoundaryXuChuDraw(ProgramSkillFrame supplied) => _resolve().SubstituteBoundaryXuChuDraw(supplied);
    internal bool TrySelectBoundaryXuChuAiOption(PendingDecision decision, ProgramSkillFrame f, scoped out PromptChoice choice) => _resolve().TrySelectBoundaryXuChuAiOption(decision, f, out choice);
}
