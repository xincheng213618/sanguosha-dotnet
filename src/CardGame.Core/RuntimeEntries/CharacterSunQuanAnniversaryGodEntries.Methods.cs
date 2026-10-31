#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanAnniversaryGodEntries
{
    internal bool AnniversaryChouxiAvailable(CharacterState owner, CardKind kind) => _resolve().AnniversaryChouxiAvailable(owner, kind);
    internal bool AnniversaryChouxiCountExemption(long id, int actor) => _resolve().AnniversaryChouxiCountExemption(id, actor);
    internal bool AnniversaryChouxiIssuedDistance(ProgramCardDistanceQuery q) => _resolve().AnniversaryChouxiIssuedDistance(q);
    internal bool AnniversaryChouxiIssuedUse(long id, int actor) => _resolve().AnniversaryChouxiIssuedUse(id, actor);
    internal bool AnniversaryChouxiSlashDistance(ProgramSlashConversionDistanceQuery q) => _resolve().AnniversaryChouxiSlashDistance(q);
    internal int AnniversaryDamageTotal(int owner) => _resolve().AnniversaryDamageTotal(owner);
    internal int[] AnniversaryJichaoPeers(int owner) => _resolve().AnniversaryJichaoPeers(owner);
    internal bool AnniversaryJichaoUnlocked(int owner, string instance) => _resolve().AnniversaryJichaoUnlocked(owner, instance);
    internal int AnniversaryMingBatchColors(CardMovementBatchContext batch) => _resolve().AnniversaryMingBatchColors(batch);
    internal IReadOnlyList<Card> AnniversaryMingCards(int holder) => _resolve().AnniversaryMingCards(holder);
    internal bool AnniversaryMingHolderHasCards(int holder) => _resolve().AnniversaryMingHolderHasCards(holder);
    internal IEnumerable<AnniversaryMingPileCreatedEvent> AnniversaryMingPiles() => _resolve().AnniversaryMingPiles();
    internal ProgramTriggerCandidate AnniversaryMingReturnCandidate(int holder, string binding) => _resolve().AnniversaryMingReturnCandidate(holder, binding);
    internal bool AnniversaryMingTriggerEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().AnniversaryMingTriggerEligible(c, t, w);
    internal IReadOnlyList<PromptChoice> AnniversarySunQuanChoices(ProgramSkillFrame f) => _resolve().AnniversarySunQuanChoices(f);
    internal void AnswerAnniversarySunQuan(PromptChoice selected) => _resolve().AnswerAnniversarySunQuan(selected);
    internal void AppendAnniversarySunQuanMingMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> candidates) => _resolve().AppendAnniversarySunQuanMingMovementCandidates(batch, candidates);
    internal bool BanUnavailableAnniversaryChouxi(ProgramConfiguredViewAsQuery q) => _resolve().BanUnavailableAnniversaryChouxi(q);
    internal bool BanUnavailableAnniversaryChouxiSource(ProgramViewAsLegalityQuery q) => _resolve().BanUnavailableAnniversaryChouxiSource(q);
    internal SkillProgramStepOutcome BeginAnniversarySunQuan(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginAnniversarySunQuan(supplied, operation);
    internal bool CanActivateAnniversaryJichao(ProgramActivationLegalityContext q) => _resolve().CanActivateAnniversaryJichao(q);
    internal void CaptureAnniversaryChouxiUse(IGameEvent payload) => _resolve().CaptureAnniversaryChouxiUse(payload);
    internal PromptChoice ChooseAnniversarySunQuan(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAnniversarySunQuan(p, f);
    internal bool CompleteAnniversaryCangmingActualTurnStart(ProgramLifecycleTriggerWindowFrame frame) => _resolve().CompleteAnniversaryCangmingActualTurnStart(frame);
    internal ProgramSkillWindowContext CreateAnniversaryMingColorContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().CreateAnniversaryMingColorContext(frame, candidate);
    internal IReadOnlyList<int> DiscardAnniversarySunQuanMingOnDeath(DeathFrame death) => _resolve().DiscardAnniversarySunQuanMingOnDeath(death);
    internal bool DrainAnniversarySunQuan(ProgramSkillFrame f) => _resolve().DrainAnniversarySunQuan(f);
    internal void FinishAnniversarySunQuan(ProgramSkillFrame f) => _resolve().FinishAnniversarySunQuan(f);
    internal bool HasAnniversaryMingStartObligation(int seat) => _resolve().HasAnniversaryMingStartObligation(seat);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeAnniversaryMingReturns(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeAnniversaryMingReturns(owner, window, ordinary);
    internal IReadOnlyList<NamedAmbushPileSnapshot>? IncludeAnniversaryMingViews(IReadOnlyList<NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll) => _resolve().IncludeAnniversaryMingViews(ordinary, holder, viewer, revealAll);
    internal bool IsAnniversaryChouxiSource(CharacterState owner, CardConversionSource? source) => _resolve().IsAnniversaryChouxiSource(owner, source);
    internal bool IsIssuedAnniversaryMingColors(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedAnniversaryMingColors(candidate, context);
    internal bool IsIssuedAnniversaryMingReturn(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedAnniversaryMingReturn(candidate, context);
    internal bool KeepPaidAnniversarySunQuan(ProgramSkillFrame f) => _resolve().KeepPaidAnniversarySunQuan(f);
    internal void MoveAnniversaryMing(ProgramSkillFrame f, IReadOnlyList<int> cards, bool deposit) => _resolve().MoveAnniversaryMing(f, cards, deposit);
    internal void PublishAnniversarySunQuan(ProgramSkillFrame f) => _resolve().PublishAnniversarySunQuan(f);
    internal bool ResumeAnniversarySunQuan(long id) => _resolve().ResumeAnniversarySunQuan(id);
    internal void ReturnAnniversarySunQuanColors(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnAnniversarySunQuanColors(f, outcome);
    internal bool ReturnAnniversarySunQuanMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversarySunQuanMovement(f);
    internal bool TryBeginAnniversaryCangmingActualTurnStart(CharacterState current) => _resolve().TryBeginAnniversaryCangmingActualTurnStart(current);
    internal ProgramSkillWindowContext? TryCreateAnniversaryMingColorContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateAnniversaryMingColorContext(frame, candidate);
    internal bool ValidAnniversarySunQuan(ProgramSkillFrame f) => _resolve().ValidAnniversarySunQuan(f);
    internal void ValidateSubmittedAnniversaryChouxi(ProgramSubmittedUseQuery q) => _resolve().ValidateSubmittedAnniversaryChouxi(q);
    internal bool HasAnniversaryMing { get => _resolve().HasAnniversaryMing; }
}
