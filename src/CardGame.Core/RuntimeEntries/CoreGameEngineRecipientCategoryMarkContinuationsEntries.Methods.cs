#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecipientCategoryMarkContinuationsEntries
{
    internal bool ExactRecipientCategoryDyingCursor(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.DyingFrame dying, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame entry) => _resolve().ExactRecipientCategoryDyingCursor(root, dying, entry);
    internal bool HasRecipientCategoryMarkDamageObserver(long windowId) => _resolve().HasRecipientCategoryMarkDamageObserver(windowId);
    internal bool IsOriginalDyingSuspendedByRecipientCategoryMark(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByRecipientCategoryMark(dying);
    internal bool IsRecipientCategoryMarkProgramDying() => _resolve().IsRecipientCategoryMarkProgramDying();
    internal bool IsRecipientCategoryObserverEdge(int child) => _resolve().IsRecipientCategoryObserverEdge(child);
    internal global::CardGame.Core.ProgramSkillFrame? RecipientCategoryMarkObserverRoot() => _resolve().RecipientCategoryMarkObserverRoot();
    internal bool RecipientCategoryObserverSuffix(int rootIndex) => _resolve().RecipientCategoryObserverSuffix(rootIndex);
    internal bool RecipientCategoryPureDyingPrefix(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecipientCategoryPureDyingPrefix(f);
    internal bool RecipientCategoryStructuralPrefix(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RecipientCategoryStructuralPrefix(f);
    internal bool TryAdvanceRecipientCategoryMarkSubtree() => _resolve().TryAdvanceRecipientCategoryMarkSubtree();
}
