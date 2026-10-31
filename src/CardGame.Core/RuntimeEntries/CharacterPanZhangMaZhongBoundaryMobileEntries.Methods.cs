#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPanZhangMaZhongBoundaryMobileEntries
{
    internal void AssertBoundaryAnjianReceipts() => _resolve().AssertBoundaryAnjianReceipts();
    internal int BoundaryAnjianDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().BoundaryAnjianDamage(attack);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BoundaryAnjianReceipt> BoundaryAnjianReceipts(long frameId, int target) => _resolve().BoundaryAnjianReceipts(frameId, target);
    internal bool CanOfferBoundaryPanZhangMaZhong(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferBoundaryPanZhangMaZhong(gate);
    internal bool IsBoundaryAnjianUnrespondable(long frameId) => _resolve().IsBoundaryAnjianUnrespondable(frameId);
    internal void IssueBoundaryAnjianChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IssueBoundaryAnjianChoice(frame);
    internal void PresentBoundaryAnjianTarget() => _resolve().PresentBoundaryAnjianTarget();
    internal bool TrySelectBoundaryAnjianOption(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame, scoped out global::CardGame.Core.PromptChoice choice) => _resolve().TrySelectBoundaryAnjianOption(decision, frame, out choice);
}
