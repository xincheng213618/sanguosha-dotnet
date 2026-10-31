#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOffTurnUsedCardObserversEntries
{
    internal bool CanContinueOffTurnUsedCardGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueOffTurnUsedCardGift(frame);
    internal bool IsOffTurnUsedCardGiftProgramDying() => _resolve().IsOffTurnUsedCardGiftProgramDying();
    internal bool OffTurnUsedCardGiftFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().OffTurnUsedCardGiftFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? OffTurnUsedCardGiftObserverRoot(long id) => _resolve().OffTurnUsedCardGiftObserverRoot(id);
    internal bool OffTurnUsedCardGiftSkillsChangedEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().OffTurnUsedCardGiftSkillsChangedEdge(parent, child);
    internal bool OffTurnUsedCardGiftStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().OffTurnUsedCardGiftStructuralEdge(parent, child);
}
