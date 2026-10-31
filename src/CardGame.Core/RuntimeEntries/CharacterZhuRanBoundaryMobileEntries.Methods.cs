#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRanBoundaryMobileEntries
{
    internal bool AdvanceBoundaryDanshouObservers() => _resolve().AdvanceBoundaryDanshouObservers();
    internal void AnswerBoundaryDanshou(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerBoundaryDanshou(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BoundaryDanshouChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryDanshouChoices(f);
    internal int BoundaryDanshouCount(int owner, int actor, int turn, int turnOwner, long cutoff) => _resolve().BoundaryDanshouCount(owner, actor, turn, turnOwner, cutoff);
    internal bool BoundaryDanshouDamageObserver(long id) => _resolve().BoundaryDanshouDamageObserver(id);
    internal bool BoundaryDanshouDyingObserver() => _resolve().BoundaryDanshouDyingObserver();
    internal bool BoundaryDanshouFinalizedTargetWindows(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().BoundaryDanshouFinalizedTargetWindows(query);
    internal bool BoundaryDanshouFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BoundaryDanshouFirstChild(f, child);
    internal global::CardGame.Core.Card[] BoundaryDanshouMaterials(int owner) => _resolve().BoundaryDanshouMaterials(owner);
    internal global::CardGame.Core.TurnEndingBoundaryFrame? BoundaryDanshouMother(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().BoundaryDanshouMother(c, context);
    internal bool BoundaryDanshouMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().BoundaryDanshouMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? BoundaryDanshouObserverRoot() => _resolve().BoundaryDanshouObserverRoot();
    internal bool BoundaryDanshouRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().BoundaryDanshouRecovery(f, recovery);
    internal void CaptureBoundaryDanshouUse(global::CardGame.Core.CardUseFrame use) => _resolve().CaptureBoundaryDanshouUse(use);
    internal void ContinueBoundaryDanshou(long id) => _resolve().ContinueBoundaryDanshou(id);
    internal bool KeepBoundaryDanshouPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryDanshouPaid(f);
    internal void ObserveBoundaryDanshouHistory(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveBoundaryDanshouHistory(payload);
    internal void PayBoundaryDanshou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayBoundaryDanshou(f);
    internal global::CardGame.Core.SkillProgramStepOutcome PrepareBoundaryDanshou(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PrepareBoundaryDanshou(supplied);
    internal void PublishBoundaryDanshou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBoundaryDanshou(f);
    internal bool QualifyBoundaryDanshou(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyBoundaryDanshou(c, context);
    internal bool ResumeBoundaryDanshou(long id) => _resolve().ResumeBoundaryDanshou(id);
    internal bool ReturnBoundaryDanshouMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryDanshouMovement(f);
    internal bool SealBoundaryDanshouCompletedUse(global::CardGame.Core.CardUseFrame use) => _resolve().SealBoundaryDanshouCompletedUse(use);
    internal bool HasBoundaryDanshou { get => _resolve().HasBoundaryDanshou; }
}
