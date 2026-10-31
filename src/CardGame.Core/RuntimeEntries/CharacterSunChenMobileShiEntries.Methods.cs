#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunChenMobileShiEntries
{
    internal bool AdvanceShiScChildren() => _resolve().AdvanceShiScChildren();
    internal void AfterShiScChain(GameEngine.IDamageAttempt a) => _resolve().AfterShiScChain(a);
    internal void AnswerShiSc(PromptChoice choice) => _resolve().AnswerShiSc(choice);
    internal ShiScArmedEvent? AvailableShiScArm(GameEngine.IDamageAttempt a) => _resolve().AvailableShiScArm(a);
    internal void BeforeShiScChain(GameEngine.IDamageAttempt a) => _resolve().BeforeShiScChain(a);
    internal SkillProgramStepOutcome BeginShiSc(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginShiSc(input, op);
    internal bool CanOfferShiSc(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferShiSc(c, t, w);
    internal PromptChoice ChooseShiSc(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShiSc(p, f);
    internal void ConsumeShiScDamage(IGameEvent payload) => _resolve().ConsumeShiScDamage(payload);
    internal bool ContinueShiSc(long id) => _resolve().ContinueShiSc(id);
    internal bool DrainShiSc(long id) => _resolve().DrainShiSc(id);
    internal void FinishShiSc(ProgramSkillFrame f) => _resolve().FinishShiSc(f);
    internal bool KeepShiScPaid(ProgramSkillFrame f) => _resolve().KeepShiScPaid(f);
    internal IEnumerable<ShiScArmedEvent> LiveShiScArms(int owner) => _resolve().LiveShiScArms(owner);
    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiSunChenPublicStates(CharacterState owner, string skillId) => _resolve().MobileShiSunChenPublicStates(owner, skillId);
    internal PendingDecision? ProjectShiScCardSelection(PendingDecision decision, ProgramSkillFrame f) => _resolve().ProjectShiScCardSelection(decision, f);
    internal void PublishShiSc(ProgramSkillFrame f) => _resolve().PublishShiSc(f);
    internal void ReturnShiScDraw(ProgramSkillFrame input, NativeDrawOutcome outcome) => _resolve().ReturnShiScDraw(input, outcome);
    internal bool ReturnShiScMovement(ProgramSkillFrame f) => _resolve().ReturnShiScMovement(f);
    internal void SetShiScChainAmount(GameEngine.IDamageAttempt a, int amount) => _resolve().SetShiScChainAmount(a, amount);
    internal void SetShiScDamageState(long id, ShiScDamageReceipt? r) => _resolve().SetShiScDamageState(id, r);
    internal bool ShiScChildEdge(int i) => _resolve().ShiScChildEdge(i);
    internal bool ShiScChildSuffix(int n) => _resolve().ShiScChildSuffix(n);
    internal IReadOnlyList<PromptChoice> ShiScChoices(ProgramSkillFrame f) => _resolve().ShiScChoices(f);
    internal int ShiScChooser(ProgramSkillFrame f) => _resolve().ShiScChooser(f);
    internal GameEngine.ShiScCompletedUse? ShiScCompleted(ProgramSkillWindowContext w, int owner) => _resolve().ShiScCompleted(w, owner);
    internal int ShiScDamageCursor(GameEngine.IDamageAttempt a) => _resolve().ShiScDamageCursor(a);
    internal ShiScDamageReceipt? ShiScDamageState(long id) => _resolve().ShiScDamageState(id);
    internal bool ShiScDyingObserver() => _resolve().ShiScDyingObserver();
    internal bool ShiScFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ShiScFirstChild(f, child);
    internal bool ShiScImplicitDrawTwo(ProgramTargetCardWindowQuery q) => _resolve().ShiScImplicitDrawTwo(q);
    internal IReadOnlyList<int> ShiScMomentumTargets() => _resolve().ShiScMomentumTargets();
    internal bool ShiScMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().ShiScMovementProof(f, effect, p);
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> ShiScNextDamage(GameEngine.IDamageAttempt a) => _resolve().ShiScNextDamage(a);
    internal bool ShiScNoCategory(int owner, CardKind used) => _resolve().ShiScNoCategory(owner, used);
    internal ProgramSkillFrame? ShiScObserverRoot() => _resolve().ShiScObserverRoot();
    internal IReadOnlyList<PromptChoice> ShiScOwnedChoices(ProgramSkillFrame f, int owner, bool discard) => _resolve().ShiScOwnedChoices(f, owner, discard);
    internal bool ShiScRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().ShiScRecoveryParent(f, recovery);
    internal bool ShiScSourceWindow(ProgramSkillFrame f) => _resolve().ShiScSourceWindow(f);
    internal bool ValidShiSc(ProgramSkillFrame f) => _resolve().ValidShiSc(f);
}
