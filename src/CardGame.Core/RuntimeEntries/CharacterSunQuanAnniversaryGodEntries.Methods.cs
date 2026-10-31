#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanAnniversaryGodEntries
{
    internal bool AnniversaryChouxiAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind) => _resolve().AnniversaryChouxiAvailable(owner, kind);
    internal bool AnniversaryChouxiCountExemption(long id, int actor) => _resolve().AnniversaryChouxiCountExemption(id, actor);
    internal bool AnniversaryChouxiIssuedDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().AnniversaryChouxiIssuedDistance(q);
    internal bool AnniversaryChouxiIssuedUse(long id, int actor) => _resolve().AnniversaryChouxiIssuedUse(id, actor);
    internal bool AnniversaryChouxiSlashDistance(global::CardGame.Core.ProgramSlashConversionDistanceQuery q) => _resolve().AnniversaryChouxiSlashDistance(q);
    internal int AnniversaryDamageTotal(int owner) => _resolve().AnniversaryDamageTotal(owner);
    internal int[] AnniversaryJichaoPeers(int owner) => _resolve().AnniversaryJichaoPeers(owner);
    internal bool AnniversaryJichaoUnlocked(int owner, string instance) => _resolve().AnniversaryJichaoUnlocked(owner, instance);
    internal int AnniversaryMingBatchColors(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().AnniversaryMingBatchColors(batch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> AnniversaryMingCards(int holder) => _resolve().AnniversaryMingCards(holder);
    internal bool AnniversaryMingHolderHasCards(int holder) => _resolve().AnniversaryMingHolderHasCards(holder);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.AnniversaryMingPileCreatedEvent> AnniversaryMingPiles() => _resolve().AnniversaryMingPiles();
    internal global::CardGame.Core.ProgramTriggerCandidate AnniversaryMingReturnCandidate(int holder, string binding) => _resolve().AnniversaryMingReturnCandidate(holder, binding);
    internal bool AnniversaryMingTriggerEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().AnniversaryMingTriggerEligible(c, t, w);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AnniversarySunQuanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversarySunQuanChoices(f);
    internal bool AnniversarySunQuanMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().AnniversarySunQuanMovementProof(f, effect, pending);
    internal void AnswerAnniversarySunQuan(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerAnniversarySunQuan(selected);
    internal void AppendAnniversarySunQuanMingMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> candidates) => _resolve().AppendAnniversarySunQuanMingMovementCandidates(batch, candidates);
    internal bool BanUnavailableAnniversaryChouxi(global::CardGame.Core.ProgramConfiguredViewAsQuery q) => _resolve().BanUnavailableAnniversaryChouxi(q);
    internal bool BanUnavailableAnniversaryChouxiSource(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().BanUnavailableAnniversaryChouxiSource(q);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversarySunQuan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginAnniversarySunQuan(supplied, operation);
    internal bool CanActivateAnniversaryJichao(global::CardGame.Core.ProgramActivationLegalityContext q) => _resolve().CanActivateAnniversaryJichao(q);
    internal void CaptureAnniversaryChouxiUse(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureAnniversaryChouxiUse(payload);
    internal global::CardGame.Core.PromptChoice ChooseAnniversarySunQuan(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAnniversarySunQuan(p, f);
    internal bool CompleteAnniversaryCangmingActualTurnStart(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().CompleteAnniversaryCangmingActualTurnStart(frame);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateAnniversaryMingColorContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateAnniversaryMingColorContext(frame, candidate);
    internal global::System.Collections.Generic.IReadOnlyList<int> DiscardAnniversarySunQuanMingOnDeath(global::CardGame.Core.DeathFrame death) => _resolve().DiscardAnniversarySunQuanMingOnDeath(death);
    internal bool DrainAnniversarySunQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainAnniversarySunQuan(f);
    internal void FinishAnniversarySunQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishAnniversarySunQuan(f);
    internal bool HasAnniversaryMingStartObligation(int seat) => _resolve().HasAnniversaryMingStartObligation(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeAnniversaryMingReturns(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => _resolve().IncludeAnniversaryMingReturns(owner, window, ordinary);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NamedAmbushPileSnapshot>? IncludeAnniversaryMingViews(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NamedAmbushPileSnapshot>? ordinary, int holder, int viewer, bool revealAll) => _resolve().IncludeAnniversaryMingViews(ordinary, holder, viewer, revealAll);
    internal bool IsAnniversaryChouxiSource(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source) => _resolve().IsAnniversaryChouxiSource(owner, source);
    internal bool IsIssuedAnniversaryMingColors(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedAnniversaryMingColors(candidate, context);
    internal bool IsIssuedAnniversaryMingReturn(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedAnniversaryMingReturn(candidate, context);
    internal bool KeepPaidAnniversarySunQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidAnniversarySunQuan(f);
    internal void MoveAnniversaryMing(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> cards, bool deposit) => _resolve().MoveAnniversaryMing(f, cards, deposit);
    internal void PublishAnniversarySunQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishAnniversarySunQuan(f);
    internal void RequireAnniversarySunQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireAnniversarySunQuan(f);
    internal bool ResumeAnniversarySunQuan(long id) => _resolve().ResumeAnniversarySunQuan(id);
    internal void ReturnAnniversarySunQuanColors(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnAnniversarySunQuanColors(f, outcome);
    internal bool ReturnAnniversarySunQuanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversarySunQuanMovement(f);
    internal bool TryBeginAnniversaryCangmingActualTurnStart(global::CardGame.Core.CharacterState current) => _resolve().TryBeginAnniversaryCangmingActualTurnStart(current);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateAnniversaryMingColorContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateAnniversaryMingColorContext(frame, candidate);
    internal bool ValidAnniversarySunQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidAnniversarySunQuan(f);
    internal void ValidateSubmittedAnniversaryChouxi(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().ValidateSubmittedAnniversaryChouxi(q);
    internal bool HasAnniversaryMing { get => _resolve().HasAnniversaryMing; }
}
