#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOffTurnUsedCardObserversEntries
{
    internal bool CanContinueOffTurnUsedCardGift(ProgramSkillFrame frame) => _resolve().CanContinueOffTurnUsedCardGift(frame);
    internal bool IsOffTurnUsedCardGiftProgramDying() => _resolve().IsOffTurnUsedCardGiftProgramDying();
    internal bool OffTurnUsedCardGiftFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().OffTurnUsedCardGiftFirstChild(root, child);
    internal ProgramSkillFrame? OffTurnUsedCardGiftObserverRoot(long id) => _resolve().OffTurnUsedCardGiftObserverRoot(id);
    internal bool OffTurnUsedCardGiftSkillsChangedEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().OffTurnUsedCardGiftSkillsChangedEdge(parent, child);
    internal bool OffTurnUsedCardGiftStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().OffTurnUsedCardGiftStructuralEdge(parent, child);
}
