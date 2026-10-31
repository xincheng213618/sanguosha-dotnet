#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvQianOlEntries
{
    internal bool CanContinueLvQian(ProgramSkillFrame f) => _resolve().CanContinueLvQian(f);
    internal bool CanOfferLvQian(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferLvQian(c, t, w);
    internal WeiluPhaseKey? CaptureWeiluEndingPhase(CharacterState owner) => _resolve().CaptureWeiluEndingPhase(owner);
    internal WeiluPhaseKey? CaptureWeiluStartingPhase(CharacterState owner) => _resolve().CaptureWeiluStartingPhase(owner);
    internal IEnumerable<BeforeDamageProgramCandidate> CollectZengdaoDamageCandidates(int source, int target, bool rangeChainOnly) => _resolve().CollectZengdaoDamageCandidates(source, target, rangeChainOnly);
    internal void ConsumeUnreturnedWeiluDebt(CharacterState owner, WeiluPaidDebt debt) => _resolve().ConsumeUnreturnedWeiluDebt(owner, debt);
    internal ProgramSkillWindowContext CreateLvQianBeforeDamageContext(BeforeDamageProgramCandidate item, ProgramSkillWindowContext context) => _resolve().CreateLvQianBeforeDamageContext(item, context);
    internal ProgramSkillWindowContext CreateWeiluEndingContext(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().CreateWeiluEndingContext(frame, candidate, context);
    internal ProgramSkillWindowContext CreateWeiluStartingContext(PlayPhaseStartingBoundaryFrame frame, ProgramSkillWindowContext context) => _resolve().CreateWeiluStartingContext(frame, context);
    internal bool DrainLvQian(ProgramSkillFrame f) => _resolve().DrainLvQian(f);
    internal bool ExactWeiluDamageParent(ProgramSkillFrame f, long damageId, int attacker, int amount) => _resolve().ExactWeiluDamageParent(f, damageId, attacker, amount);
    internal SkillProgramStepOutcome ExecuteLvQian(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ExecuteLvQian(supplied, op);
    internal void FinishLvQian(ProgramSkillFrame f) => _resolve().FinishLvQian(f);
    internal bool HasCurrentIssuedWeiluDebt(CharacterState owner) => _resolve().HasCurrentIssuedWeiluDebt(owner);
    internal bool HasIssuedWeiluDebtCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().HasIssuedWeiluDebtCandidate(c, w);
    internal bool IsDurableZengdaoGrant(ZengdaoPublicGrant g) => _resolve().IsDurableZengdaoGrant(g);
    internal bool IsDurableZengdaoPublicPile(PublicPersistentPileSource pile) => _resolve().IsDurableZengdaoPublicPile(pile);
    internal bool IsIssuedZengdaoCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedZengdaoCandidate(c, w);
    internal bool IsLvQianRecoveryCompletion(ProgramSkillFrame f, RecoveryAttempt attempt) => _resolve().IsLvQianRecoveryCompletion(f, attempt);
    internal void IssueLvQianSegment(ProgramSkillFrame f) => _resolve().IssueLvQianSegment(f);
    internal void IssueLvQianStarted(ProgramSkillFrame f) => _resolve().IssueLvQianStarted(f);
    internal IEnumerable<ProgramTriggerCandidate> IssuedWeiluEndingCandidates(CharacterState owner) => _resolve().IssuedWeiluEndingCandidates(owner);
    internal bool LvQianFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().LvQianFirstChild(root, child);
    internal ProgramSkillFrame? LvQianObserverRoot() => _resolve().LvQianObserverRoot();
    internal bool LvQianSourceCurrent(CardConversionSource source, string hash) => _resolve().LvQianSourceCurrent(source, hash);
    internal ZengdaoPublicGrant? MatchingZengdaoGrant(ProgramTriggerCandidate c) => _resolve().MatchingZengdaoGrant(c);
    internal void ObserveLvQianActualPlayEntry(int ownerSeat, int phaseInstance) => _resolve().ObserveLvQianActualPlayEntry(ownerSeat, phaseInstance);
    internal void ObserveLvQianTurnBoundary(IGameEvent payload) => _resolve().ObserveLvQianTurnBoundary(payload);
    internal void PayNextWeiluTarget(ProgramSkillFrame f) => _resolve().PayNextWeiluTarget(f);
    internal void PayZengdaoDamage(ProgramSkillFrame f, ZengdaoEntityChoice choice) => _resolve().PayZengdaoDamage(f, choice);
    internal void PublishZengdaoChoice(ProgramSkillFrame f) => _resolve().PublishZengdaoChoice(f);
    internal void ResolveZengdaoDamageChoice(PromptChoice choice) => _resolve().ResolveZengdaoDamageChoice(choice);
    internal bool ResumeLvQian(long id) => _resolve().ResumeLvQian(id);
    internal bool ReturnLvQianMovement(ProgramSkillFrame f) => _resolve().ReturnLvQianMovement(f);
    internal PromptChoice SelectZengdaoDamageChoice(ProgramSkillFrame f) => _resolve().SelectZengdaoDamageChoice(f);
    internal bool TryAdvanceLvQianSubtree() => _resolve().TryAdvanceLvQianSubtree();
    internal bool TryBeginForcedWeiluPhaseEnd() => _resolve().TryBeginForcedWeiluPhaseEnd();
    internal IReadOnlyList<PromptChoice> ZengdaoDamageChoices(ProgramSkillFrame f) => _resolve().ZengdaoDamageChoices(f);
    internal bool HasLvQianCapability { get => _resolve().HasLvQianCapability; }
}
