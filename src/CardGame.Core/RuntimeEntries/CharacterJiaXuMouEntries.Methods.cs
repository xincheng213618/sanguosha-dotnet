#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiaXuMouEntries
{
    internal void AssertMouJxNativeUse(CardUseFrame u) => _resolve().AssertMouJxNativeUse(u);
    internal void BindMouJxNativeUse(CardUseFrame use) => _resolve().BindMouJxNativeUse(use);
    internal bool IsMouJxExactCardQuery(CharacterState actor, int? selected) => _resolve().IsMouJxExactCardQuery(actor, selected);
    internal void IssueMouJxUse(ProgramSkillFrame f) => _resolve().IssueMouJxUse(f);
    internal bool MouJxAdvanceChildren() => _resolve().MouJxAdvanceChildren();
    internal bool MouJxBoundDistance(long id, int actor) => _resolve().MouJxBoundDistance(id, actor);
    internal bool MouJxCardDistance(ProgramCardDistanceQuery q) => _resolve().MouJxCardDistance(q);
    internal bool MouJxDying() => _resolve().MouJxDying();
    internal bool MouJxEdge(int i) => _resolve().MouJxEdge(i);
    internal IReadOnlyList<int> MouJxExpectedTargets(MouJiaXuNativeUse n) => _resolve().MouJxExpectedTargets(n);
    internal bool MouJxFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouJxFirstChild(f, child);
    internal bool MouJxHandDistanceQuery(CharacterState actor, IReadOnlyList<int>? physical) => _resolve().MouJxHandDistanceQuery(actor, physical);
    internal bool MouJxImplicitDrawTwo(ProgramTargetCardWindowQuery q) => _resolve().MouJxImplicitDrawTwo(q);
    internal bool MouJxMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouJxMovement(f, effect, p);
    internal bool MouJxNativeRidesOn(CardUseFrame u, ProgramSkillFrame f) => _resolve().MouJxNativeRidesOn(u, f);
    internal ProgramSkillFrame? MouJxObserverRoot() => _resolve().MouJxObserverRoot();
    internal bool MouJxOffTurnSlash(long id, int actor) => _resolve().MouJxOffTurnSlash(id, actor);
    internal CardSnapshot[] MouJxPrivateHandView(int viewer) => _resolve().MouJxPrivateHandView(viewer);
    internal bool MouJxRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouJxRecoveryParent(f, recovery);
    internal bool MouJxSlashDistance(ProgramSlashDistanceQuery q) => _resolve().MouJxSlashDistance(q);
    internal bool MouJxSuffix(int n) => _resolve().MouJxSuffix(n);
    internal IReadOnlyList<LegalAction> MouJxUseOptions(ProgramSkillFrame f) => _resolve().MouJxUseOptions(f);
    internal bool ResumeMouJxNativeUse() => _resolve().ResumeMouJxNativeUse();
    internal bool ReturnMouJxNativeUse(CardUseFrame u) => _resolve().ReturnMouJxNativeUse(u);
    internal bool HasMouJiaXu { get => _resolve().HasMouJiaXu; }
    internal MouJiaXuRuntime MouJiaXu { get => _resolve().MouJiaXu; }
}
