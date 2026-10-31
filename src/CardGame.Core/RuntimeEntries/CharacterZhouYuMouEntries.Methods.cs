#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuMouEntries
{
    internal void AddMouZyFireTargets(List<LegalAction> actions, CharacterState actor) => _resolve().AddMouZyFireTargets(actions, actor);
    internal void AssertMouZyUse(CardUseFrame u) => _resolve().AssertMouZyUse(u);
    internal void BindMouZyNativeUse(CardUseFrame use) => _resolve().BindMouZyNativeUse(use);
    internal bool FinishMouZyZeroFireAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouZyZeroFireAttack(attack, out paused);
    internal bool HasMouZyFireTargets(CharacterState actor) => _resolve().HasMouZyFireTargets(actor);
    internal bool IsMouZyExactCardQuery(CharacterState actor, int? id) => _resolve().IsMouZyExactCardQuery(actor, id);
    internal bool IsMouZyTargetAction(CharacterState actor, LegalAction action) => _resolve().IsMouZyTargetAction(actor, action);
    internal bool IsMouZyZeroFireUse(long id) => _resolve().IsMouZyZeroFireUse(id);
    internal void IssueMouZyUse(ProgramSkillFrame f) => _resolve().IssueMouZyUse(f);
    internal bool MouZyAdvanceChildren() => _resolve().MouZyAdvanceChildren();
    internal bool MouZyDamageApplied(GameEngine.IDamageAttempt a, long id) => _resolve().MouZyDamageApplied(a, id);
    internal bool MouZyDying() => _resolve().MouZyDying();
    internal bool MouZyEdge(int i) => _resolve().MouZyEdge(i);
    internal IReadOnlyList<int> MouZyExpectedTargets(int actor, LegalAction action) => _resolve().MouZyExpectedTargets(actor, action);
    internal int MouZyFireBonus(GameEngine.IDamageAttempt a) => _resolve().MouZyFireBonus(a);
    internal IReadOnlyList<IReadOnlyList<int>> MouZyFireOptions(ProgramSkillFrame f) => _resolve().MouZyFireOptions(f);
    internal bool MouZyFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouZyFirstChild(f, child);
    internal bool MouZyMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouZyMovement(f, effect, p);
    internal bool MouZyNativeRidesOn(CardUseFrame u, ProgramSkillFrame f) => _resolve().MouZyNativeRidesOn(u, f);
    internal ProgramSkillFrame? MouZyObserverRoot() => _resolve().MouZyObserverRoot();
    internal bool MouZyOffTurnUse(long id, int actor) => _resolve().MouZyOffTurnUse(id, actor);
    internal bool MouZyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouZyRecoveryParent(f, recovery);
    internal bool MouZySuffix(int i) => _resolve().MouZySuffix(i);
    internal int MouZyTargetLimit(CharacterState actor) => _resolve().MouZyTargetLimit(actor);
    internal IReadOnlyList<LegalAction> MouZyUseOptions(ProgramSkillFrame f) => _resolve().MouZyUseOptions(f);
    internal bool ResumeMouZyUse() => _resolve().ResumeMouZyUse();
    internal bool ReturnMouZyUse(CardUseFrame u) => _resolve().ReturnMouZyUse(u);
    internal MouZhouYuRuntime MouZyAnniversary { get => _resolve().MouZyAnniversary; }
}
