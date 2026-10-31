#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuMouEntries
{
    internal void AddMouZyFireTargets(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor) => _resolve().AddMouZyFireTargets(actions, actor);
    internal void AssertMouZyUse(global::CardGame.Core.CardUseFrame u) => _resolve().AssertMouZyUse(u);
    internal void BindMouZyNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindMouZyNativeUse(use);
    internal bool FinishMouZyZeroFireAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouZyZeroFireAttack(attack, out paused);
    internal bool HasMouZyFireTargets(global::CardGame.Core.CharacterState actor) => _resolve().HasMouZyFireTargets(actor);
    internal bool IsMouZyExactCardQuery(global::CardGame.Core.CharacterState actor, int? id) => _resolve().IsMouZyExactCardQuery(actor, id);
    internal bool IsMouZyTargetAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action) => _resolve().IsMouZyTargetAction(actor, action);
    internal bool IsMouZyZeroFireUse(long id) => _resolve().IsMouZyZeroFireUse(id);
    internal void IssueMouZyUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouZyUse(f);
    internal bool MouZyAdvanceChildren() => _resolve().MouZyAdvanceChildren();
    internal bool MouZyBeforeDamage(long id) => _resolve().MouZyBeforeDamage(id);
    internal bool MouZyDamageApplied(global::CardGame.Core.GameEngine.IDamageAttempt a, long id) => _resolve().MouZyDamageApplied(a, id);
    internal bool MouZyDying() => _resolve().MouZyDying();
    internal bool MouZyEdge(int i) => _resolve().MouZyEdge(i);
    internal global::System.Collections.Generic.IReadOnlyList<int> MouZyExpectedTargets(int actor, global::CardGame.Core.LegalAction action) => _resolve().MouZyExpectedTargets(actor, action);
    internal int MouZyFireBonus(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().MouZyFireBonus(a);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<int>> MouZyFireOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouZyFireOptions(f);
    internal bool MouZyFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouZyFirstChild(f, child);
    internal bool MouZyMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouZyMovement(f, effect, p);
    internal bool MouZyNativeRidesOn(global::CardGame.Core.CardUseFrame u, global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouZyNativeRidesOn(u, f);
    internal global::CardGame.Core.ProgramSkillFrame? MouZyObserverRoot() => _resolve().MouZyObserverRoot();
    internal bool MouZyOffTurnUse(long id, int actor) => _resolve().MouZyOffTurnUse(id, actor);
    internal bool MouZyPaidDamage(long id) => _resolve().MouZyPaidDamage(id);
    internal bool MouZyRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouZyRecoveryParent(f, recovery);
    internal bool MouZySuffix(int i) => _resolve().MouZySuffix(i);
    internal int MouZyTargetLimit(global::CardGame.Core.CharacterState actor) => _resolve().MouZyTargetLimit(actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> MouZyUseOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouZyUseOptions(f);
    internal bool ResumeMouZyUse() => _resolve().ResumeMouZyUse();
    internal bool ReturnMouZyUse(global::CardGame.Core.CardUseFrame u) => _resolve().ReturnMouZyUse(u);
    internal global::CardGame.Core.MouZhouYuRuntime MouZyAnniversary { get => _resolve().MouZyAnniversary; }
}
