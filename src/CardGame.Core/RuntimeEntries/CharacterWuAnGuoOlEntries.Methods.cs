#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuAnGuoOlEntries
{
    internal bool AdvanceWuAnGuoSubtree() => _resolve().AdvanceWuAnGuoSubtree();
    internal SkillProgramStepOutcome BeginWuAnGuoLiyong(ProgramSkillFrame input) => _resolve().BeginWuAnGuoLiyong(input);
    internal void BeginWuAnGuoLiyongDuel(ProgramSkillFrame frame, WuAnGuoLiyongDuelOrigin origin) => _resolve().BeginWuAnGuoLiyongDuel(frame, origin);
    internal bool CanActivateWuAnGuoLiyong(CharacterState owner) => _resolve().CanActivateWuAnGuoLiyong(owner);
    internal bool CanIssueWuAnGuoLiyongDuel(int actor, int target) => _resolve().CanIssueWuAnGuoLiyongDuel(actor, target);
    internal bool ContinuePaidWuAnGuoLiyong(ProgramSkillFrame f) => _resolve().ContinuePaidWuAnGuoLiyong(f);
    internal void ContinueWuAnGuoLiyong(long id) => _resolve().ContinueWuAnGuoLiyong(id);
    internal void FinishWuAnGuoLiyong(ProgramSkillFrame f, bool issued) => _resolve().FinishWuAnGuoLiyong(f, issued);
    internal bool HasWuAnGuoLiyongCompletedUseParent(long useId) => _resolve().HasWuAnGuoLiyongCompletedUseParent(useId);
    internal bool IsWuAnGuoLiyongDuelUse(long id) => _resolve().IsWuAnGuoLiyongDuelUse(id);
    internal bool IsWuAnGuoYangConversionAvailable(CharacterState owner, Card card, CardConversionSource source, bool forResponse) => _resolve().IsWuAnGuoYangConversionAvailable(owner, card, source, forResponse);
    internal bool MatchesWuAnGuoLiyong(ProgramSkillFrame f, WuAnGuoLiyongDuelOrigin origin) => _resolve().MatchesWuAnGuoLiyong(f, origin);
    internal void ObserveWuAnGuoActualUses(IGameEvent payload) => _resolve().ObserveWuAnGuoActualUses(payload);
    internal bool ResumeWuAnGuoLiyong(long id) => _resolve().ResumeWuAnGuoLiyong(id);
    internal bool ReturnWuAnGuoLiyongAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnWuAnGuoLiyongAttack(completion);
    internal void ReturnWuAnGuoLiyongUse(CardUseFrame use) => _resolve().ReturnWuAnGuoLiyongUse(use);
    internal bool ValidWuAnGuoLiyong(ProgramSkillFrame f) => _resolve().ValidWuAnGuoLiyong(f);
    internal bool WuAnGuoConversionUnavailable(ProgramViewAsSourceQuery query) => _resolve().WuAnGuoConversionUnavailable(query);
    internal bool WuAnGuoFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WuAnGuoFirstChild(f, child);
    internal ProgramSkillFrame? WuAnGuoObserverRoot() => _resolve().WuAnGuoObserverRoot();
    internal int WuAnGuoUsedSuitMask(int seat) => _resolve().WuAnGuoUsedSuitMask(seat);
    internal bool HasWuAnGuoCapability { get => _resolve().HasWuAnGuoCapability; }
}
