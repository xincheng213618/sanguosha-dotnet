#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategoryMarkContinuationsEntries
{
    internal bool ExactRecipientCategoryDyingCursor(ProgramSkillFrame root, DyingFrame dying, ProgramLifecycleTriggerWindowFrame entry) => _resolve().ExactRecipientCategoryDyingCursor(root, dying, entry);
    internal bool IsOriginalDyingSuspendedByRecipientCategoryMark(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByRecipientCategoryMark(dying);
    internal bool IsRecipientCategoryMarkProgramDying() => _resolve().IsRecipientCategoryMarkProgramDying();
    internal bool IsRecipientCategoryObserverEdge(int child) => _resolve().IsRecipientCategoryObserverEdge(child);
    internal ProgramSkillFrame? RecipientCategoryMarkObserverRoot() => _resolve().RecipientCategoryMarkObserverRoot();
    internal bool RecipientCategoryObserverSuffix(int rootIndex) => _resolve().RecipientCategoryObserverSuffix(rootIndex);
    internal bool RecipientCategoryPureDyingPrefix(ProgramSkillFrame f) => _resolve().RecipientCategoryPureDyingPrefix(f);
    internal bool RecipientCategoryStructuralPrefix(ProgramSkillFrame f) => _resolve().RecipientCategoryStructuralPrefix(f);
    internal bool TryAdvanceRecipientCategoryMarkSubtree() => _resolve().TryAdvanceRecipientCategoryMarkSubtree();
}
