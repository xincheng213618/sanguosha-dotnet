#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangShiPingOlEntries
{
    internal bool AdvanceZhangShiPingSubtree() => _resolve().AdvanceZhangShiPingSubtree();
    internal global::CardGame.Core.PromptChoice AiHongji(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiHongji(p, f);
    internal global::CardGame.Core.PromptChoice AiXingGu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiXingGu(p, f);
    internal void AnswerHongji(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHongji(choice);
    internal void AnswerXingGu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerXingGu(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHongjiExtraPhase(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHongjiExtraPhase(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHongjiPromise(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginHongjiPromise(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginXingGu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginXingGu(supplied, operation);
    internal bool CanOfferZhangShiPing(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferZhangShiPing(c, t, context);
    internal bool CanPlaceZhangShiPingMount(int owner, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().CanPlaceZhangShiPingMount(owner, target, card, from);
    internal bool ContinueIssuedZhangShiPing(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedZhangShiPing(f);
    internal void ContinueXingGu(long id) => _resolve().ContinueXingGu(id);
    internal bool? DecideIssuedTrigger670(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger670(candidate, context);
    internal bool DrainXingGu(long id) => _resolve().DrainXingGu(id);
    internal void FinishXingGu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishXingGu(f);
    internal bool HasIssuedHongjiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedHongjiCandidate(c, context);
    internal bool HasOutstandingHongjiPhase(global::CardGame.Core.CharacterState recipient, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().HasOutstandingHongjiPhase(recipient, window);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HongjiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HongjiChoices(f);
    internal (bool Draw, bool Play) HongjiEligibility(int owner, int recipient, long? parentId) => _resolve().HongjiEligibility(owner, recipient, parentId);
    internal bool HongjiPromiseReservedByPhaseParent(global::CardGame.Core.ZhangShiPingHongjiGrantedEvent promise) => _resolve().HongjiPromiseReservedByPhaseParent(promise);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(global::CardGame.Core.CharacterState turnOwner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().IncludeHongjiPhaseCandidates(turnOwner, window, candidates);
    internal bool IsHongjiPhaseCandidate(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().IsHongjiPhaseCandidate(c);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ZhangShiPingHongjiGrantedEvent> OutstandingHongjiPromises(int recipient, global::CardGame.Core.SkillProgramTriggerWindow window) => _resolve().OutstandingHongjiPromises(recipient, window);
    internal void PlaceZhangShiPingMount(global::CardGame.Core.ProgramSkillFrame frame, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().PlaceZhangShiPingMount(frame, target, card, from);
    internal void PublishHongji(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHongji(f);
    internal void PublishXingGu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishXingGu(f);
    internal bool ResumeXingGu(long id) => _resolve().ResumeXingGu(id);
    internal void ResumeZhangShiPingHongjiPhase(long frameId) => _resolve().ResumeZhangShiPingHongjiPhase(frameId);
    internal bool TryBeginZhangShiPingHongjiPhase(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.TurnPhase phase, int recipientSeat) => _resolve().TryBeginZhangShiPingHongjiPhase(frame, phase, recipientSeat);
    internal bool TryCancelZhangShiPingHongjiPhase() => _resolve().TryCancelZhangShiPingHongjiPhase();
    internal bool TryResumeZhangShiPingHongjiPhase() => _resolve().TryResumeZhangShiPingHongjiPhase();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> XingGuPlacementChoices(int owner, string instance) => _resolve().XingGuPlacementChoices(owner, instance);
    internal bool ZhangShiPingFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhangShiPingFirstChild(f, child);
    internal bool ZhangShiPingHasMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardsMovedTriggerWindowFrame moved) => _resolve().ZhangShiPingHasMovement(f, moved);
    internal global::CardGame.Core.ProgramSkillFrame? ZhangShiPingObserverRoot() => _resolve().ZhangShiPingObserverRoot();
    internal bool ZhangShiPingObserverSuffix(int root) => _resolve().ZhangShiPingObserverSuffix(root);
    internal bool HasHongjiCapability { get => _resolve().HasHongjiCapability; }
}
