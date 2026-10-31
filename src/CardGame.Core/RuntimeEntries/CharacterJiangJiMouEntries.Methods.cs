#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangJiMouEntries
{
    internal void AssertMouJjNativeUse(CardUseFrame u) => _resolve().AssertMouJjNativeUse(u);
    internal void BindMouJjNativeUse(CardUseFrame use) => _resolve().BindMouJjNativeUse(use);
    internal void CompleteMouJjEffect(long id) => _resolve().CompleteMouJjEffect(id);
    internal bool ExecuteMouJjForeign(CharacterState actor, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().ExecuteMouJjForeign(actor, action, cards, targets);
    internal bool HasUnstartedMouJjEffect(long id) => _resolve().HasUnstartedMouJjEffect(id);
    internal bool IsMouJjEffectRound(long id) => _resolve().IsMouJjEffectRound(id);
    internal bool IsMouJjExactCardQuery(CharacterState actor, int? selected) => _resolve().IsMouJjExactCardQuery(actor, selected);
    internal bool IsTargetScopedMouJjEffect(long id) => _resolve().IsTargetScopedMouJjEffect(id);
    internal void IssueMouJjForeign(int actor, int owner) => _resolve().IssueMouJjForeign(actor, owner);
    internal void IssueMouJjRepeat(ProgramSkillFrame f) => _resolve().IssueMouJjRepeat(f);
    internal void IssueMouJjUse(ProgramSkillFrame f) => _resolve().IssueMouJjUse(f);
    internal bool MouJjAdvanceChildren() => _resolve().MouJjAdvanceChildren();
    internal bool MouJjDamage(GameEngine.IDamageAttempt a, long id) => _resolve().MouJjDamage(a, id);
    internal bool MouJjDying() => _resolve().MouJjDying();
    internal bool MouJjEdge(int i) => _resolve().MouJjEdge(i);
    internal IReadOnlyList<int> MouJjExpectedTargets(MouJiangJiNativeUse n) => _resolve().MouJjExpectedTargets(n);
    internal bool MouJjFinalized(ProgramTargetCardWindowQuery q) => _resolve().MouJjFinalized(q);
    internal bool MouJjFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouJjFirstChild(f, child);
    internal IEnumerable<LegalAction> MouJjForeignActions(CharacterState actor) => _resolve().MouJjForeignActions(actor);
    internal bool MouJjImplicit(ProgramTargetCardWindowQuery q) => _resolve().MouJjImplicit(q);
    internal bool MouJjMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouJjMovement(f, effect, p);
    internal bool MouJjNativeRidesOn(CardUseFrame u, ProgramSkillFrame f) => _resolve().MouJjNativeRidesOn(u, f);
    internal ProgramSkillFrame? MouJjObserverRoot() => _resolve().MouJjObserverRoot();
    internal bool MouJjOffTurnSlash(long id, int actor) => _resolve().MouJjOffTurnSlash(id, actor);
    internal bool MouJjRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouJjRecoveryParent(f, recovery);
    internal bool MouJjSuffix(int n) => _resolve().MouJjSuffix(n);
    internal IReadOnlyList<LegalAction> MouJjUseOptions(ProgramSkillFrame f) => _resolve().MouJjUseOptions(f);
    internal bool ResumeMouJjNativeUse() => _resolve().ResumeMouJjNativeUse();
    internal bool ReturnMouJjNativeUse(CardUseFrame u) => _resolve().ReturnMouJjNativeUse(u);
    internal bool SubmitMouJjForeign(UseProgramSkillCommand command, scoped out CommandResult result) => _resolve().SubmitMouJjForeign(command, out result);
    internal bool TryBeginMouJjEffect(long id, bool afterAttack) => _resolve().TryBeginMouJjEffect(id, afterAttack);
    internal MouJiangJiRuntime MouJiangJi { get => _resolve().MouJiangJi; }
}
