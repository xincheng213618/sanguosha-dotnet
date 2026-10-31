#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunChenMobileShiEntries
{
    internal bool AdvanceShiScChildren() => _resolve().AdvanceShiScChildren();
    internal void AfterShiScChain(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().AfterShiScChain(a);
    internal void AnswerShiSc(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShiSc(choice);
    internal global::CardGame.Core.ShiScArmedEvent? AvailableShiScArm(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().AvailableShiScArm(a);
    internal void BeforeShiScChain(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().BeforeShiScChain(a);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShiSc(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShiSc(input, op);
    internal bool CanOfferShiSc(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferShiSc(c, t, w);
    internal global::CardGame.Core.PromptChoice ChooseShiSc(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShiSc(p, f);
    internal void ConsumeShiScDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ConsumeShiScDamage(payload);
    internal bool ContinueShiSc(long id) => _resolve().ContinueShiSc(id);
    internal bool DrainShiSc(long id) => _resolve().DrainShiSc(id);
    internal void FinishShiSc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShiSc(f);
    internal bool KeepShiScPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepShiScPaid(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ShiScArmedEvent> LiveShiScArms(int owner) => _resolve().LiveShiScArms(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramPublicRuleStateSnapshot> MobileShiSunChenPublicStates(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().MobileShiSunChenPublicStates(owner, skillId);
    internal global::CardGame.Core.PendingDecision? ProjectShiScCardSelection(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ProjectShiScCardSelection(decision, f);
    internal void PublishShiSc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShiSc(f);
    internal void RequireShiSc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireShiSc(f);
    internal void ReturnShiScDraw(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShiScDraw(input, outcome);
    internal bool ReturnShiScMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShiScMovement(f);
    internal void SetShiScChainAmount(global::CardGame.Core.GameEngine.IDamageAttempt a, int amount) => _resolve().SetShiScChainAmount(a, amount);
    internal void SetShiScDamageState(long id, global::CardGame.Core.ShiScDamageReceipt? r) => _resolve().SetShiScDamageState(id, r);
    internal bool ShiScBeforeDamageObserver(long id) => _resolve().ShiScBeforeDamageObserver(id);
    internal bool ShiScChildEdge(int i) => _resolve().ShiScChildEdge(i);
    internal bool ShiScChildSuffix(int n) => _resolve().ShiScChildSuffix(n);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiScChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiScChoices(f);
    internal int ShiScChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiScChooser(f);
    internal global::CardGame.Core.GameEngine.ShiScCompletedUse? ShiScCompleted(global::CardGame.Core.ProgramSkillWindowContext w, int owner) => _resolve().ShiScCompleted(w, owner);
    internal int ShiScDamageCursor(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().ShiScDamageCursor(a);
    internal bool ShiScDamageObserver(long id) => _resolve().ShiScDamageObserver(id);
    internal global::CardGame.Core.ShiScDamageReceipt? ShiScDamageState(long id) => _resolve().ShiScDamageState(id);
    internal bool ShiScDyingObserver() => _resolve().ShiScDyingObserver();
    internal bool ShiScFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ShiScFirstChild(f, child);
    internal bool ShiScImplicitDrawTwo(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().ShiScImplicitDrawTwo(q);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiScMomentumTargets() => _resolve().ShiScMomentumTargets();
    internal bool ShiScMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ShiScMovementProof(f, effect, p);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> ShiScNextDamage(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().ShiScNextDamage(a);
    internal bool ShiScNoCategory(int owner, global::CardGame.Core.CardKind used) => _resolve().ShiScNoCategory(owner, used);
    internal global::CardGame.Core.ProgramSkillFrame? ShiScObserverRoot() => _resolve().ShiScObserverRoot();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShiScOwnedChoices(global::CardGame.Core.ProgramSkillFrame f, int owner, bool discard) => _resolve().ShiScOwnedChoices(f, owner, discard);
    internal bool ShiScRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().ShiScRecoveryParent(f, recovery);
    internal bool ShiScSourceWindow(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShiScSourceWindow(f);
    internal bool ValidShiSc(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidShiSc(f);
}
