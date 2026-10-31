#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangJiMouEntries
{
    internal void AssertMouJjEffects() => _resolve().AssertMouJjEffects();
    internal void AssertMouJjNativeUse(global::CardGame.Core.CardUseFrame u) => _resolve().AssertMouJjNativeUse(u);
    internal void BindMouJjNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindMouJjNativeUse(use);
    internal void CompleteMouJjEffect(long id) => _resolve().CompleteMouJjEffect(id);
    internal bool ExecuteMouJjForeign(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ExecuteMouJjForeign(actor, action, cards, targets);
    internal bool HasUnstartedMouJjEffect(long id) => _resolve().HasUnstartedMouJjEffect(id);
    internal bool IsMouJjEffectRound(long id) => _resolve().IsMouJjEffectRound(id);
    internal bool IsMouJjEffectSubset(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().IsMouJjEffectSubset(use, targets);
    internal bool IsMouJjExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => _resolve().IsMouJjExactCardQuery(actor, selected);
    internal bool IsTargetScopedMouJjEffect(long id) => _resolve().IsTargetScopedMouJjEffect(id);
    internal void IssueMouJjForeign(int actor, int owner) => _resolve().IssueMouJjForeign(actor, owner);
    internal void IssueMouJjRepeat(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouJjRepeat(f);
    internal void IssueMouJjUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouJjUse(f);
    internal bool MouJjAdvanceChildren() => _resolve().MouJjAdvanceChildren();
    internal bool MouJjBeforeDamage(long id) => _resolve().MouJjBeforeDamage(id);
    internal bool MouJjDamage(global::CardGame.Core.GameEngine.IDamageAttempt a, long id) => _resolve().MouJjDamage(a, id);
    internal bool MouJjDamageWindow(long id) => _resolve().MouJjDamageWindow(id);
    internal bool MouJjDying() => _resolve().MouJjDying();
    internal bool MouJjEdge(int i) => _resolve().MouJjEdge(i);
    internal global::System.Collections.Generic.IReadOnlyList<int> MouJjExpectedTargets(global::CardGame.Core.MouJiangJiNativeUse n) => _resolve().MouJjExpectedTargets(n);
    internal bool MouJjFinalized(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().MouJjFinalized(q);
    internal bool MouJjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouJjFirstChild(f, child);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> MouJjForeignActions(global::CardGame.Core.CharacterState actor) => _resolve().MouJjForeignActions(actor);
    internal bool MouJjImplicit(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().MouJjImplicit(q);
    internal bool MouJjMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouJjMovement(f, effect, p);
    internal bool MouJjNativeRidesOn(global::CardGame.Core.CardUseFrame u, global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJjNativeRidesOn(u, f);
    internal global::CardGame.Core.ProgramSkillFrame? MouJjObserverRoot() => _resolve().MouJjObserverRoot();
    internal bool MouJjOffTurnSlash(long id, int actor) => _resolve().MouJjOffTurnSlash(id, actor);
    internal bool MouJjRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouJjRecoveryParent(f, recovery);
    internal bool MouJjSuffix(int n) => _resolve().MouJjSuffix(n);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> MouJjUseOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJjUseOptions(f);
    internal bool ResumeMouJjNativeUse() => _resolve().ResumeMouJjNativeUse();
    internal bool ReturnMouJjNativeUse(global::CardGame.Core.CardUseFrame u) => _resolve().ReturnMouJjNativeUse(u);
    internal bool SubmitMouJjForeign(global::CardGame.Core.UseProgramSkillCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().SubmitMouJjForeign(command, out result);
    internal bool TryBeginMouJjEffect(long id, bool afterAttack) => _resolve().TryBeginMouJjEffect(id, afterAttack);
    internal global::CardGame.Core.MouJiangJiRuntime MouJiangJi { get => _resolve().MouJiangJi; }
}
