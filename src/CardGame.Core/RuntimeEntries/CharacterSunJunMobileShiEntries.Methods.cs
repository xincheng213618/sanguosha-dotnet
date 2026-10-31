#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunJunMobileShiEntries
{
    internal bool AdvanceShiSjChildren() => _resolve().AdvanceShiSjChildren();
    internal void AfterShiSjPublicReveal(ProgramSkillFrame f) => _resolve().AfterShiSjPublicReveal(f);
    internal bool AllowsShiSjActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsShiSjActivation(gate);
    internal bool AllowsShiSjDamageBenefit(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().AllowsShiSjDamageBenefit(c, t, context);
    internal void AnswerShiSj(PromptChoice choice) => _resolve().AnswerShiSj(choice);
    internal SkillProgramStepOutcome BeginShiSj(ProgramSkillFrame input, bool benefit) => _resolve().BeginShiSj(input, benefit);
    internal void CaptureShiSjActualHandUse(CardUseDeclaredEvent fact) => _resolve().CaptureShiSjActualHandUse(fact);
    internal void CaptureShiSjResponseHandUse(CardActionAcceptedEvent fact) => _resolve().CaptureShiSjResponseHandUse(fact);
    internal PromptChoice ChooseShiSj(PendingDecision p) => _resolve().ChooseShiSj(p);
    internal void ContinueShiSj(long id) => _resolve().ContinueShiSj(id);
    internal bool DrainShiSj(ProgramSkillFrame f) => _resolve().DrainShiSj(f);
    internal void FinishShiSj(ProgramSkillFrame f) => _resolve().FinishShiSj(f);
    internal bool HasShiSunJunFirstSuitAllowance(CharacterState actor, Card? material = default, CardConversionSource? conversion = default) => _resolve().HasShiSunJunFirstSuitAllowance(actor, material, conversion);
    internal bool KeepShiSjPaid(ProgramSkillFrame f) => _resolve().KeepShiSjPaid(f);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiSunJunPublicStates(CharacterState owner, string id) => _resolve().MobileShiSunJunPublicStates(owner, id);
    internal void PayShiSjCost(ProgramSkillFrame f) => _resolve().PayShiSjCost(f);
    internal PendingDecision? ProjectShiSjCardSelection(PendingDecision decision, ProgramSkillFrame f) => _resolve().ProjectShiSjCardSelection(decision, f);
    internal void PublishShiSj(ProgramSkillFrame f) => _resolve().PublishShiSj(f);
    internal void RecordShiSjHandAction(long useId, CardActionContext a, Suit suit) => _resolve().RecordShiSjHandAction(useId, a, suit);
    internal bool ResumeShiSj(long id) => _resolve().ResumeShiSj(id);
    internal void ReturnShiSjDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnShiSjDraw(f, outcome);
    internal bool ReturnShiSjMovement(ProgramSkillFrame f) => _resolve().ReturnShiSjMovement(f);
    internal void SealShiSj(long id) => _resolve().SealShiSj(id);
    internal bool ShiSjCanDiscardRevealed(ProgramSkillFrame f) => _resolve().ShiSjCanDiscardRevealed(f);
    internal IReadOnlyList<PromptChoice> ShiSjChoices(ProgramSkillFrame f) => _resolve().ShiSjChoices(f);
    internal bool ShiSjDamageBenefitEligible(int owner, string instance, ProgramSkillWindowContext c) => _resolve().ShiSjDamageBenefitEligible(owner, instance, c);
    internal bool ShiSjDyingChild() => _resolve().ShiSjDyingChild();
    internal bool ShiSjEdge(int i) => _resolve().ShiSjEdge(i);
    internal bool ShiSjFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ShiSjFirstChild(f, child);
    internal bool ShiSjFirstSuitSlashQuota(ProgramSlashQuotaQuery q) => _resolve().ShiSjFirstSuitSlashQuota(q);
    internal bool ShiSjHandSource(int actor, CardLocation from) => _resolve().ShiSjHandSource(actor, from);
    internal bool ShiSjIssuedFirstSuitUse(long id, int actor) => _resolve().ShiSjIssuedFirstSuitUse(id, actor);
    internal bool ShiSjMaterialAllowance(CharacterState actor, IReadOnlyList<int>? ids, Card? appearance = default, Suit? effectiveSuit = default) => _resolve().ShiSjMaterialAllowance(actor, ids, appearance, effectiveSuit);
    internal bool ShiSjMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().ShiSjMovement(f, e, p);
    internal ProgramSkillFrame? ShiSjObserverRoot() => _resolve().ShiSjObserverRoot();
    internal IReadOnlyList<PromptChoice> ShiSjOwnDiscardChoices(ProgramSkillFrame f) => _resolve().ShiSjOwnDiscardChoices(f);
    internal bool ShiSjOwnTurn(int actor) => _resolve().ShiSjOwnTurn(actor);
    internal int ShiSjPhaseLimit(int owner) => _resolve().ShiSjPhaseLimit(owner);
    internal IEnumerable<CardSnapshot> ShiSjPublicHandCard() => _resolve().ShiSjPublicHandCard();
    internal bool ShiSjRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ShiSjRecoveryParent(f, recovery);
    internal bool ShiSjSuffix(int n) => _resolve().ShiSjSuffix(n);
    internal bool ShiSjSuitSeen(int actor, Suit suit) => _resolve().ShiSjSuitSeen(actor, suit);
    internal bool ValidShiSj(ProgramSkillFrame f) => _resolve().ValidShiSj(f);
}
