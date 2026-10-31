#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiaXuMouEntries
{
    internal void AssertMouJxNativeUse(global::CardGame.Core.CardUseFrame u) => _resolve().AssertMouJxNativeUse(u);
    internal void BindMouJxNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindMouJxNativeUse(use);
    internal bool IsMouJxExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => _resolve().IsMouJxExactCardQuery(actor, selected);
    internal void IssueMouJxUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouJxUse(f);
    internal bool MouJxAdvanceChildren() => _resolve().MouJxAdvanceChildren();
    internal bool MouJxBoundDistance(long id, int actor) => _resolve().MouJxBoundDistance(id, actor);
    internal bool MouJxCardDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().MouJxCardDistance(q);
    internal bool MouJxDying() => _resolve().MouJxDying();
    internal bool MouJxEdge(int i) => _resolve().MouJxEdge(i);
    internal global::System.Collections.Generic.IReadOnlyList<int> MouJxExpectedTargets(global::CardGame.Core.MouJiaXuNativeUse n) => _resolve().MouJxExpectedTargets(n);
    internal bool MouJxFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouJxFirstChild(f, child);
    internal bool MouJxHandDistanceQuery(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int>? physical) => _resolve().MouJxHandDistanceQuery(actor, physical);
    internal bool MouJxImplicitDrawTwo(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().MouJxImplicitDrawTwo(q);
    internal bool MouJxMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouJxMovement(f, effect, p);
    internal bool MouJxNativeRidesOn(global::CardGame.Core.CardUseFrame u, global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJxNativeRidesOn(u, f);
    internal global::CardGame.Core.ProgramSkillFrame? MouJxObserverRoot() => _resolve().MouJxObserverRoot();
    internal bool MouJxOffTurnSlash(long id, int actor) => _resolve().MouJxOffTurnSlash(id, actor);
    internal global::CardGame.Core.CardSnapshot[] MouJxPrivateHandView(int viewer) => _resolve().MouJxPrivateHandView(viewer);
    internal bool MouJxRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouJxRecoveryParent(f, recovery);
    internal bool MouJxSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().MouJxSlashDistance(q);
    internal bool MouJxSuffix(int n) => _resolve().MouJxSuffix(n);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> MouJxUseOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJxUseOptions(f);
    internal bool ResumeMouJxNativeUse() => _resolve().ResumeMouJxNativeUse();
    internal bool ReturnMouJxNativeUse(global::CardGame.Core.CardUseFrame u) => _resolve().ReturnMouJxNativeUse(u);
    internal bool HasMouJiaXu { get => _resolve().HasMouJiaXu; }
    internal global::CardGame.Core.MouJiaXuRuntime MouJiaXu { get => _resolve().MouJiaXu; }
}
