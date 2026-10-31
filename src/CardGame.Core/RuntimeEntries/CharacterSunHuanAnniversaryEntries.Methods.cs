#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunHuanAnniversaryEntries
{
    internal bool AdvanceSunHuanChildren() => _resolve().AdvanceSunHuanChildren();
    internal void AnswerSunHuan(PromptChoice choice) => _resolve().AnswerSunHuan(choice);
    internal SkillProgramStepOutcome BeginSunHuan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginSunHuan(supplied, op);
    internal void BindSunHuanNative(CardUseDeclaredEvent fact) => _resolve().BindSunHuanNative(fact);
    internal bool CanOfferSunHuanEnding(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferSunHuanEnding(c, t, w);
    internal PromptChoice ChooseSunHuan(PendingDecision p) => _resolve().ChooseSunHuan(p);
    internal bool ContinueSunHuan(long id) => _resolve().ContinueSunHuan(id);
    internal void DiscardSunHuan(ProgramSkillFrame f) => _resolve().DiscardSunHuan(f);
    internal bool DrainSunHuan(ProgramSkillFrame f) => _resolve().DrainSunHuan(f);
    internal void FinishSunHuan(ProgramSkillFrame f) => _resolve().FinishSunHuan(f);
    internal bool IsAnniversarySunHuanExactCardQuery(CharacterState actor, int? selectedPhysicalCardId) => _resolve().IsAnniversarySunHuanExactCardQuery(actor, selectedPhysicalCardId);
    internal bool IsSunHuanDeclarationQuery(CharacterState actor, int? id) => _resolve().IsSunHuanDeclarationQuery(actor, id);
    internal bool IsSunHuanNative(CardUseFrame use) => _resolve().IsSunHuanNative(use);
    internal bool MatchesSunHuanDeclaration(ProgramSkillFrame root, CardDeclarationFrame d) => _resolve().MatchesSunHuanDeclaration(root, d);
    internal void PublishSunHuan(ProgramSkillFrame f) => _resolve().PublishSunHuan(f);
    internal bool RetainSunHuan(ProgramSkillFrame f) => _resolve().RetainSunHuan(f);
    internal bool ReturnSunHuanAttack() => _resolve().ReturnSunHuanAttack();
    internal void ReturnSunHuanDraw(ProgramSkillFrame supplied, NativeDrawOutcome d) => _resolve().ReturnSunHuanDraw(supplied, d);
    internal bool ReturnSunHuanMovement(ProgramSkillFrame f) => _resolve().ReturnSunHuanMovement(f);
    internal void ReturnSunHuanNative(CardUseFrame use) => _resolve().ReturnSunHuanNative(use);
    internal void SettleSunHuanNative(ProgramSkillFrame f) => _resolve().SettleSunHuanNative(f);
    internal IReadOnlyList<LegalAction> SunHuanActions(ProgramSkillFrame f) => _resolve().SunHuanActions(f);
    internal IReadOnlyList<PromptChoice> SunHuanChoices(ProgramSkillFrame f) => _resolve().SunHuanChoices(f);
    internal bool SunHuanDeclarationEdge(int i) => _resolve().SunHuanDeclarationEdge(i);
    internal IEnumerable<AnniversarySunHuanDraw> SunHuanDraws(int seat, string instance) => _resolve().SunHuanDraws(seat, instance);
    internal bool SunHuanDying() => _resolve().SunHuanDying();
    internal IReadOnlyList<int> SunHuanExpectedTargets(AnniversarySunHuanNative n) => _resolve().SunHuanExpectedTargets(n);
    internal bool SunHuanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SunHuanFirstChild(f, child);
    internal IReadOnlyList<int> SunHuanHand(ProgramSkillFrame f) => _resolve().SunHuanHand(f);
    internal bool SunHuanImplicitTarget(ProgramTargetCardWindowQuery q) => _resolve().SunHuanImplicitTarget(q);
    internal bool SunHuanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().SunHuanMovement(f, effect, p);
    internal ProgramSkillFrame? SunHuanObserverRoot() => _resolve().SunHuanObserverRoot();
    internal bool SunHuanObserverSuffix(int i) => _resolve().SunHuanObserverSuffix(i);
    internal bool SunHuanOffTurnSlash(long id, int actor) => _resolve().SunHuanOffTurnSlash(id, actor);
    internal bool SunHuanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().SunHuanRecoveryParent(f, recovery);
    internal bool SunHuanTargetWindow(ProgramTargetCardWindowQuery q) => _resolve().SunHuanTargetWindow(q);
    internal bool TryReturnAnniversarySunHuanDeclaration(CardDeclarationFrame d, bool success) => _resolve().TryReturnAnniversarySunHuanDeclaration(d, success);
    internal bool ValidSunHuan(ProgramSkillFrame f) => _resolve().ValidSunHuan(f);
    internal bool ValidSunHuanDraw(AnniversarySunHuanDraw d) => _resolve().ValidSunHuanDraw(d);
    internal void ValidateSunHuanNativeReturn(CardUseFrame use) => _resolve().ValidateSunHuanNativeReturn(use);
    internal bool HasAnniversarySunHuan { get => _resolve().HasAnniversarySunHuan; }
}
