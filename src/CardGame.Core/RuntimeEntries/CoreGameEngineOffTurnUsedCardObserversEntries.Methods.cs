#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOffTurnUsedCardObserversEntries
{
    internal bool CanContinueOffTurnUsedCardGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueOffTurnUsedCardGift(frame);
    internal bool HasOffTurnUsedCardGiftBeforeDamageObserver(long windowId) => _resolve().HasOffTurnUsedCardGiftBeforeDamageObserver(windowId);
    internal bool HasOffTurnUsedCardGiftDamageObserver(long windowId) => _resolve().HasOffTurnUsedCardGiftDamageObserver(windowId);
    internal bool IsOffTurnUsedCardGiftMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsOffTurnUsedCardGiftMovement(frame, effect, pending);
    internal bool IsOffTurnUsedCardGiftProgramDying() => _resolve().IsOffTurnUsedCardGiftProgramDying();
    internal bool OffTurnUsedCardGiftDamageObserver(long windowId, bool before) => _resolve().OffTurnUsedCardGiftDamageObserver(windowId, before);
    internal bool OffTurnUsedCardGiftFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().OffTurnUsedCardGiftFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? OffTurnUsedCardGiftObserverRoot(long id) => _resolve().OffTurnUsedCardGiftObserverRoot(id);
    internal bool OffTurnUsedCardGiftSkillsChangedEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().OffTurnUsedCardGiftSkillsChangedEdge(parent, child);
    internal bool OffTurnUsedCardGiftStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().OffTurnUsedCardGiftStructuralEdge(parent, child);
}
