#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuChuBoundaryMobileEntries
{
    internal bool BoundaryXuChuCommittedDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryXuChuCommittedDraw(f);
    internal global::CardGame.Core.ProgramLifecycleTriggerWindowFrame BoundaryXuChuDrawParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryXuChuDrawParent(f);
    internal bool ContinueBoundaryXuChuCommittedDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueBoundaryXuChuCommittedDraw(f);
    internal bool ReturnBoundaryXuChuCommittedMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryXuChuCommittedMovement(f);
    internal void SubstituteBoundaryXuChuDraw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().SubstituteBoundaryXuChuDraw(supplied);
    internal bool TrySelectBoundaryXuChuAiOption(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.PromptChoice choice) => _resolve().TrySelectBoundaryXuChuAiOption(decision, f, out choice);
}
