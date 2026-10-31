#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangShiPingOlEntries
{
    internal bool AdvanceZhangShiPingSubtree() => _resolve().AdvanceZhangShiPingSubtree();
    internal PromptChoice AiHongji(PendingDecision p, ProgramSkillFrame f) => _resolve().AiHongji(p, f);
    internal PromptChoice AiXingGu(PendingDecision p, ProgramSkillFrame f) => _resolve().AiXingGu(p, f);
    internal void AnswerHongji(PromptChoice choice) => _resolve().AnswerHongji(choice);
    internal void AnswerXingGu(PromptChoice choice) => _resolve().AnswerXingGu(choice);
    internal SkillProgramStepOutcome BeginHongjiExtraPhase(ProgramSkillFrame supplied) => _resolve().BeginHongjiExtraPhase(supplied);
    internal SkillProgramStepOutcome BeginHongjiPromise(ProgramSkillFrame supplied) => _resolve().BeginHongjiPromise(supplied);
    internal SkillProgramStepOutcome BeginXingGu(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginXingGu(supplied, operation);
    internal bool CanOfferZhangShiPing(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferZhangShiPing(c, t, context);
    internal bool CanPlaceZhangShiPingMount(int owner, int target, Card card, CardLocation from) => _resolve().CanPlaceZhangShiPingMount(owner, target, card, from);
    internal bool ContinueIssuedZhangShiPing(ProgramSkillFrame f) => _resolve().ContinueIssuedZhangShiPing(f);
    internal void ContinueXingGu(long id) => _resolve().ContinueXingGu(id);
    internal bool? DecideIssuedTrigger670(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger670(candidate, context);
    internal bool DrainXingGu(long id) => _resolve().DrainXingGu(id);
    internal void FinishXingGu(ProgramSkillFrame f) => _resolve().FinishXingGu(f);
    internal bool HasIssuedHongjiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().HasIssuedHongjiCandidate(c, context);
    internal bool HasOutstandingHongjiPhase(CharacterState recipient, SkillProgramTriggerWindow window) => _resolve().HasOutstandingHongjiPhase(recipient, window);
    internal IReadOnlyList<PromptChoice> HongjiChoices(ProgramSkillFrame f) => _resolve().HongjiChoices(f);
    internal (bool Draw, bool Play) HongjiEligibility(int owner, int recipient, long? parentId) => _resolve().HongjiEligibility(owner, recipient, parentId);
    internal bool HongjiPromiseReservedByPhaseParent(ZhangShiPingHongjiGrantedEvent promise) => _resolve().HongjiPromiseReservedByPhaseParent(promise);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeHongjiPhaseCandidates(CharacterState turnOwner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> candidates) => _resolve().IncludeHongjiPhaseCandidates(turnOwner, window, candidates);
    internal bool IsHongjiPhaseCandidate(ProgramTriggerCandidate c) => _resolve().IsHongjiPhaseCandidate(c);
    internal IEnumerable<ZhangShiPingHongjiGrantedEvent> OutstandingHongjiPromises(int recipient, SkillProgramTriggerWindow window) => _resolve().OutstandingHongjiPromises(recipient, window);
    internal void PlaceZhangShiPingMount(ProgramSkillFrame frame, int target, Card card, CardLocation from) => _resolve().PlaceZhangShiPingMount(frame, target, card, from);
    internal void PublishHongji(ProgramSkillFrame f) => _resolve().PublishHongji(f);
    internal void PublishXingGu(ProgramSkillFrame f) => _resolve().PublishXingGu(f);
    internal bool ResumeXingGu(long id) => _resolve().ResumeXingGu(id);
    internal void ResumeZhangShiPingHongjiPhase(long frameId) => _resolve().ResumeZhangShiPingHongjiPhase(frameId);
    internal bool TryBeginZhangShiPingHongjiPhase(ProgramSkillFrame frame, TurnPhase phase, int recipientSeat) => _resolve().TryBeginZhangShiPingHongjiPhase(frame, phase, recipientSeat);
    internal bool TryCancelZhangShiPingHongjiPhase() => _resolve().TryCancelZhangShiPingHongjiPhase();
    internal bool TryResumeZhangShiPingHongjiPhase() => _resolve().TryResumeZhangShiPingHongjiPhase();
    internal IReadOnlyList<PromptChoice> XingGuPlacementChoices(int owner, string instance) => _resolve().XingGuPlacementChoices(owner, instance);
    internal bool ZhangShiPingFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhangShiPingFirstChild(f, child);
    internal bool ZhangShiPingHasMovement(ProgramSkillFrame f, CardsMovedTriggerWindowFrame moved) => _resolve().ZhangShiPingHasMovement(f, moved);
    internal ProgramSkillFrame? ZhangShiPingObserverRoot() => _resolve().ZhangShiPingObserverRoot();
    internal bool ZhangShiPingObserverSuffix(int root) => _resolve().ZhangShiPingObserverSuffix(root);
    internal bool HasHongjiCapability { get => _resolve().HasHongjiCapability; }
}
