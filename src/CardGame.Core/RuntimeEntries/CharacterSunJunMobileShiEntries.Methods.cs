#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunJunMobileShiEntries
{
    internal bool AdvanceShiSjChildren() => _resolve().AdvanceShiSjChildren();
    internal void AfterShiSjPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AfterShiSjPublicReveal(f);
    internal bool AllowsShiSjActivation(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsShiSjActivation(gate);
    internal bool AllowsShiSjDamageBenefit(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AllowsShiSjDamageBenefit(c, t, context);
    internal void AnswerShiSj(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShiSj(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiSj(global::CardGame.Core.ProgramSkillFrame input, bool benefit) => _resolve().BeginShiSj(input, benefit);
    internal void CaptureShiSjActualHandUse(global::CardGame.Core.CardUseDeclaredEvent fact) => _resolve().CaptureShiSjActualHandUse(fact);
    internal void CaptureShiSjResponseHandUse(global::CardGame.Core.CardActionAcceptedEvent fact) => _resolve().CaptureShiSjResponseHandUse(fact);
    internal global::CardGame.Core.PromptChoice ChooseShiSj(global::CardGame.Core.PendingDecision p) => _resolve().ChooseShiSj(p);
    internal void ContinueShiSj(long id) => _resolve().ContinueShiSj(id);
    internal bool DrainShiSj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainShiSj(f);
    internal void FinishShiSj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiSj(f);
    internal bool HasShiSunJunFirstSuitAllowance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card? material = default, global::CardGame.Core.CardConversionSource? conversion = default) => _resolve().HasShiSunJunFirstSuitAllowance(actor, material, conversion);
    internal bool KeepShiSjPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepShiSjPaid(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiSunJunPublicStates(global::CardGame.Core.CharacterState owner, string id) => _resolve().MobileShiSunJunPublicStates(owner, id);
    internal void PayShiSjCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayShiSjCost(f);
    internal global::CardGame.Core.PendingDecision? ProjectShiSjCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ProjectShiSjCardSelection(decision, f);
    internal void PublishShiSj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiSj(f);
    internal void RecordShiSjHandAction(long useId, global::CardGame.Core.CardActionContext a, global::CardGame.Core.Suit suit) => _resolve().RecordShiSjHandAction(useId, a, suit);
    internal bool ResumeShiSj(long id) => _resolve().ResumeShiSj(id);
    internal void ReturnShiSjDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShiSjDraw(f, outcome);
    internal bool ReturnShiSjMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiSjMovement(f);
    internal void SealShiSj(long id) => _resolve().SealShiSj(id);
    internal bool ShiSjCanDiscardRevealed(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiSjCanDiscardRevealed(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiSjChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiSjChoices(f);
    internal bool ShiSjDamageBenefitEligible(int owner, string instance, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().ShiSjDamageBenefitEligible(owner, instance, c);
    internal bool ShiSjDyingChild() => _resolve().ShiSjDyingChild();
    internal bool ShiSjEdge(int i) => _resolve().ShiSjEdge(i);
    internal bool ShiSjFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ShiSjFirstChild(f, child);
    internal bool ShiSjFirstSuitSlashQuota(global::CardGame.Core.ProgramSlashQuotaQuery q) => _resolve().ShiSjFirstSuitSlashQuota(q);
    internal bool ShiSjHandSource(int actor, global::CardGame.Core.CardLocation from) => _resolve().ShiSjHandSource(actor, from);
    internal bool ShiSjIssuedFirstSuitUse(long id, int actor) => _resolve().ShiSjIssuedFirstSuitUse(id, actor);
    internal bool ShiSjMaterialAllowance(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int>? ids, global::CardGame.Core.Card? appearance = default, global::CardGame.Core.Suit? effectiveSuit = default) => _resolve().ShiSjMaterialAllowance(actor, ids, appearance, effectiveSuit);
    internal bool ShiSjMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShiSjMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? ShiSjObserverRoot() => _resolve().ShiSjObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiSjOwnDiscardChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiSjOwnDiscardChoices(f);
    internal bool ShiSjOwnTurn(int actor) => _resolve().ShiSjOwnTurn(actor);
    internal int ShiSjPhaseLimit(int owner) => _resolve().ShiSjPhaseLimit(owner);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> ShiSjPublicHandCard() => _resolve().ShiSjPublicHandCard();
    internal bool ShiSjRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ShiSjRecoveryParent(f, recovery);
    internal bool ShiSjSuffix(int n) => _resolve().ShiSjSuffix(n);
    internal bool ShiSjSuitSeen(int actor, global::CardGame.Core.Suit suit) => _resolve().ShiSjSuitSeen(actor, suit);
    internal bool ValidShiSj(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidShiSj(f);
}
