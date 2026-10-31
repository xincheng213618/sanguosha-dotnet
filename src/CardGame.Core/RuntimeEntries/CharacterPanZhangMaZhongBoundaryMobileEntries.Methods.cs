#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPanZhangMaZhongBoundaryMobileEntries
{
    internal void AssertBoundaryAnjianReceipts() => _resolve().AssertBoundaryAnjianReceipts();
    internal int BoundaryAnjianDamage(GameEngine.IDamageAttempt attack) => _resolve().BoundaryAnjianDamage(attack);
    internal IEnumerable<BoundaryAnjianReceipt> BoundaryAnjianReceipts(long frameId, int target) => _resolve().BoundaryAnjianReceipts(frameId, target);
    internal bool CanOfferBoundaryPanZhangMaZhong(ProgramTriggerLegalityContext gate) => _resolve().CanOfferBoundaryPanZhangMaZhong(gate);
    internal bool IsBoundaryAnjianUnrespondable(long frameId) => _resolve().IsBoundaryAnjianUnrespondable(frameId);
    internal void IssueBoundaryAnjianChoice(ProgramSkillFrame frame) => _resolve().IssueBoundaryAnjianChoice(frame);
    internal void PresentBoundaryAnjianTarget() => _resolve().PresentBoundaryAnjianTarget();
    internal bool TrySelectBoundaryAnjianOption(PendingDecision decision, ProgramSkillFrame frame, scoped out PromptChoice choice) => _resolve().TrySelectBoundaryAnjianOption(decision, frame, out choice);
}
