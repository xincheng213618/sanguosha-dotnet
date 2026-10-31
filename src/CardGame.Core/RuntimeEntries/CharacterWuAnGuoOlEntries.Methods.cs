#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWuAnGuoOlEntries
{
    internal bool AdvanceWuAnGuoSubtree() => _resolve().AdvanceWuAnGuoSubtree();
    internal void AssertWuAnGuoLiyong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertWuAnGuoLiyong(f);
    internal void AssertWuAnGuoLiyongNativeUses(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertWuAnGuoLiyongNativeUses(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWuAnGuoLiyong(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginWuAnGuoLiyong(input);
    internal void BeginWuAnGuoLiyongDuel(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.WuAnGuoLiyongDuelOrigin origin) => _resolve().BeginWuAnGuoLiyongDuel(frame, origin);
    internal bool CanActivateWuAnGuoLiyong(global::CardGame.Core.CharacterState owner) => _resolve().CanActivateWuAnGuoLiyong(owner);
    internal bool CanIssueWuAnGuoLiyongDuel(int actor, int target) => _resolve().CanIssueWuAnGuoLiyongDuel(actor, target);
    internal bool ContinuePaidWuAnGuoLiyong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidWuAnGuoLiyong(f);
    internal void ContinueWuAnGuoLiyong(long id) => _resolve().ContinueWuAnGuoLiyong(id);
    internal void FinishWuAnGuoLiyong(global::CardGame.Core.ProgramSkillFrame f, bool issued) => _resolve().FinishWuAnGuoLiyong(f, issued);
    internal bool HasWuAnGuoBeforeDamageObserver(long id) => _resolve().HasWuAnGuoBeforeDamageObserver(id);
    internal bool HasWuAnGuoDamageObserver(long id) => _resolve().HasWuAnGuoDamageObserver(id);
    internal bool HasWuAnGuoLiyongCompletedUseParent(long useId) => _resolve().HasWuAnGuoLiyongCompletedUseParent(useId);
    internal bool IsWuAnGuoDying() => _resolve().IsWuAnGuoDying();
    internal bool IsWuAnGuoLiyongAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsWuAnGuoLiyongAttackConsistent(attack, processing);
    internal bool IsWuAnGuoLiyongDuelUse(long id) => _resolve().IsWuAnGuoLiyongDuelUse(id);
    internal bool IsWuAnGuoYangConversionAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource source, bool forResponse) => _resolve().IsWuAnGuoYangConversionAvailable(owner, card, source, forResponse);
    internal bool MatchesWuAnGuoLiyong(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.WuAnGuoLiyongDuelOrigin origin) => _resolve().MatchesWuAnGuoLiyong(f, origin);
    internal void ObserveWuAnGuoActualUses(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveWuAnGuoActualUses(payload);
    internal bool ResumeWuAnGuoLiyong(long id) => _resolve().ResumeWuAnGuoLiyong(id);
    internal bool ReturnWuAnGuoLiyongAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnWuAnGuoLiyongAttack(completion);
    internal void ReturnWuAnGuoLiyongUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnWuAnGuoLiyongUse(use);
    internal bool ValidWuAnGuoLiyong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWuAnGuoLiyong(f);
    internal bool WuAnGuoConversionUnavailable(global::CardGame.Core.ProgramViewAsSourceQuery query) => _resolve().WuAnGuoConversionUnavailable(query);
    internal bool WuAnGuoFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().WuAnGuoFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? WuAnGuoObserverRoot() => _resolve().WuAnGuoObserverRoot();
    internal int WuAnGuoUsedSuitMask(int seat) => _resolve().WuAnGuoUsedSuitMask(seat);
    internal bool HasWuAnGuoCapability { get => _resolve().HasWuAnGuoCapability; }
}
