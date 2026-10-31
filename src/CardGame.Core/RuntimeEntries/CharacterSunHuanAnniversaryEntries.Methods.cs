#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunHuanAnniversaryEntries
{
    internal bool AdvanceSunHuanChildren() => _resolve().AdvanceSunHuanChildren();
    internal void AnswerSunHuan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerSunHuan(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSunHuan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginSunHuan(supplied, op);
    internal void BindSunHuanNative(global::CardGame.Core.CardUseDeclaredEvent fact) => _resolve().BindSunHuanNative(fact);
    internal bool CanOfferSunHuanEnding(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferSunHuanEnding(c, t, w);
    internal global::CardGame.Core.PromptChoice ChooseSunHuan(global::CardGame.Core.PendingDecision p) => _resolve().ChooseSunHuan(p);
    internal bool ContinueSunHuan(long id) => _resolve().ContinueSunHuan(id);
    internal void DiscardSunHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DiscardSunHuan(f);
    internal bool DrainSunHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainSunHuan(f);
    internal void FinishSunHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSunHuan(f);
    internal bool IsAnniversarySunHuanExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId) => _resolve().IsAnniversarySunHuanExactCardQuery(actor, selectedPhysicalCardId);
    internal bool IsSunHuanDeclarationQuery(global::CardGame.Core.CharacterState actor, int? id) => _resolve().IsSunHuanDeclarationQuery(actor, id);
    internal bool IsSunHuanNative(global::CardGame.Core.CardUseFrame use) => _resolve().IsSunHuanNative(use);
    internal bool MatchesSunHuanDeclaration(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.CardDeclarationFrame d) => _resolve().MatchesSunHuanDeclaration(root, d);
    internal void PublishSunHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSunHuan(f);
    internal bool RetainSunHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainSunHuan(f);
    internal bool ReturnSunHuanAttack() => _resolve().ReturnSunHuanAttack();
    internal void ReturnSunHuanDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome d) => _resolve().ReturnSunHuanDraw(supplied, d);
    internal bool ReturnSunHuanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSunHuanMovement(f);
    internal void ReturnSunHuanNative(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnSunHuanNative(use);
    internal void SettleSunHuanNative(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SettleSunHuanNative(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> SunHuanActions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SunHuanActions(f);
    internal bool SunHuanBeforeDamage(long id) => _resolve().SunHuanBeforeDamage(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SunHuanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SunHuanChoices(f);
    internal bool SunHuanDamageObserver(long id) => _resolve().SunHuanDamageObserver(id);
    internal bool SunHuanDeclarationEdge(int i) => _resolve().SunHuanDeclarationEdge(i);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.AnniversarySunHuanDraw> SunHuanDraws(int seat, string instance) => _resolve().SunHuanDraws(seat, instance);
    internal bool SunHuanDying() => _resolve().SunHuanDying();
    internal global::System.Collections.Generic.IReadOnlyList<int> SunHuanExpectedTargets(global::CardGame.Core.AnniversarySunHuanNative n) => _resolve().SunHuanExpectedTargets(n);
    internal bool SunHuanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SunHuanFirstChild(f, child);
    internal global::System.Collections.Generic.IReadOnlyList<int> SunHuanHand(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SunHuanHand(f);
    internal bool SunHuanImplicitTarget(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().SunHuanImplicitTarget(q);
    internal bool SunHuanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().SunHuanMovement(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? SunHuanObserverRoot() => _resolve().SunHuanObserverRoot();
    internal bool SunHuanObserverSuffix(int i) => _resolve().SunHuanObserverSuffix(i);
    internal bool SunHuanOffTurnSlash(long id, int actor) => _resolve().SunHuanOffTurnSlash(id, actor);
    internal bool SunHuanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().SunHuanRecoveryParent(f, recovery);
    internal bool SunHuanTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery q) => _resolve().SunHuanTargetWindow(q);
    internal bool TryReturnAnniversarySunHuanDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool success) => _resolve().TryReturnAnniversarySunHuanDeclaration(d, success);
    internal bool ValidSunHuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSunHuan(f);
    internal bool ValidSunHuanDraw(global::CardGame.Core.AnniversarySunHuanDraw d) => _resolve().ValidSunHuanDraw(d);
    internal void ValidateSunHuanNativeReturn(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateSunHuanNativeReturn(use);
    internal bool HasAnniversarySunHuan { get => _resolve().HasAnniversarySunHuan; }
}
