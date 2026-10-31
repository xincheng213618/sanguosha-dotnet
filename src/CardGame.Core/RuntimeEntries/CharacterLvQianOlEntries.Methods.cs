#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvQianOlEntries
{
    internal void AssertLvQian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertLvQian(f);
    internal bool CanContinueLvQian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueLvQian(f);
    internal bool CanOfferLvQian(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferLvQian(c, t, w);
    internal global::CardGame.Core.WeiluPhaseKey? CaptureWeiluEndingPhase(global::CardGame.Core.CharacterState owner) => _resolve().CaptureWeiluEndingPhase(owner);
    internal global::CardGame.Core.WeiluPhaseKey? CaptureWeiluStartingPhase(global::CardGame.Core.CharacterState owner) => _resolve().CaptureWeiluStartingPhase(owner);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectZengdaoDamageCandidates(int source, int target, bool rangeChainOnly) => _resolve().CollectZengdaoDamageCandidates(source, target, rangeChainOnly);
    internal void ConsumeUnreturnedWeiluDebt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.WeiluPaidDebt debt) => _resolve().ConsumeUnreturnedWeiluDebt(owner, debt);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateLvQianBeforeDamageContext(global::CardGame.Core.BeforeDamageProgramCandidate item, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateLvQianBeforeDamageContext(item, context);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateWeiluEndingContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateWeiluEndingContext(frame, candidate, context);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateWeiluStartingContext(global::CardGame.Core.PlayPhaseStartingBoundaryFrame frame, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateWeiluStartingContext(frame, context);
    internal bool DrainLvQian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainLvQian(f);
    internal bool ExactWeiluDamageParent(global::CardGame.Core.ProgramSkillFrame f, long damageId, int attacker, int amount) => _resolve().ExactWeiluDamageParent(f, damageId, attacker, amount);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteLvQian(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteLvQian(supplied, op);
    internal void FinishLvQian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishLvQian(f);
    internal bool HasCurrentIssuedWeiluDebt(global::CardGame.Core.CharacterState owner) => _resolve().HasCurrentIssuedWeiluDebt(owner);
    internal bool HasIssuedWeiluDebtCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HasIssuedWeiluDebtCandidate(c, w);
    internal bool HasLvQianBeforeDamageObserver(long id) => _resolve().HasLvQianBeforeDamageObserver(id);
    internal bool HasLvQianDamageObserver(long id) => _resolve().HasLvQianDamageObserver(id);
    internal bool IsDurableZengdaoGrant(global::CardGame.Core.ZengdaoPublicGrant g) => _resolve().IsDurableZengdaoGrant(g);
    internal bool IsDurableZengdaoPublicPile(global::CardGame.Core.PublicPersistentPileSource pile) => _resolve().IsDurableZengdaoPublicPile(pile);
    internal bool IsIssuedZengdaoCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedZengdaoCandidate(c, w);
    internal bool IsLvQianDying() => _resolve().IsLvQianDying();
    internal bool IsLvQianMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsLvQianMovement(f, e, p);
    internal bool IsLvQianRecoveryCompletion(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryAttempt attempt) => _resolve().IsLvQianRecoveryCompletion(f, attempt);
    internal void IssueLvQianSegment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueLvQianSegment(f);
    internal void IssueLvQianStarted(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueLvQianStarted(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> IssuedWeiluEndingCandidates(global::CardGame.Core.CharacterState owner) => _resolve().IssuedWeiluEndingCandidates(owner);
    internal bool LvQianFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().LvQianFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? LvQianObserverRoot() => _resolve().LvQianObserverRoot();
    internal bool LvQianSourceCurrent(global::CardGame.Core.CardConversionSource source, string hash) => _resolve().LvQianSourceCurrent(source, hash);
    internal global::CardGame.Core.ZengdaoPublicGrant? MatchingZengdaoGrant(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().MatchingZengdaoGrant(c);
    internal void ObserveLvQianActualPlayEntry(int ownerSeat, int phaseInstance) => _resolve().ObserveLvQianActualPlayEntry(ownerSeat, phaseInstance);
    internal void ObserveLvQianTurnBoundary(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveLvQianTurnBoundary(payload);
    internal void PayNextWeiluTarget(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayNextWeiluTarget(f);
    internal void PayZengdaoDamage(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ZengdaoEntityChoice choice) => _resolve().PayZengdaoDamage(f, choice);
    internal void PublishZengdaoChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZengdaoChoice(f);
    internal void ResolveZengdaoDamageChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveZengdaoDamageChoice(choice);
    internal bool ResumeLvQian(long id) => _resolve().ResumeLvQian(id);
    internal bool ReturnLvQianMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLvQianMovement(f);
    internal global::CardGame.Core.PromptChoice SelectZengdaoDamageChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectZengdaoDamageChoice(f);
    internal bool TryAdvanceLvQianSubtree() => _resolve().TryAdvanceLvQianSubtree();
    internal bool TryBeginForcedWeiluPhaseEnd() => _resolve().TryBeginForcedWeiluPhaseEnd();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZengdaoDamageChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZengdaoDamageChoices(f);
    internal bool HasLvQianCapability { get => _resolve().HasLvQianCapability; }
}
