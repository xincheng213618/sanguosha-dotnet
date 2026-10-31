#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongMouMobileEntries
{
    internal bool AdvanceMouPtChildren() => _resolve().AdvanceMouPtChildren();
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame AppendMouPtLateTargets(global::CardGame.Core.ProgramCardTriggerWindowFrame frame) => _resolve().AppendMouPtLateTargets(frame);
    internal bool CanOfferMouPt(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouPt(gate);
    internal void CaptureMouPtConversion(global::CardGame.Core.CardUseDeclaredEvent e) => _resolve().CaptureMouPtConversion(e);
    internal void CaptureMouPtLateTarget(global::CardGame.Core.ProgramCardUseTargetAddedEvent e) => _resolve().CaptureMouPtLateTarget(e);
    internal void CollectPaidMouPtTargets(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectPaidMouPtTargets(action, window, result);
    internal bool ContinueMouPtPaidSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueMouPtPaidSource(f);
    internal void ContinueMouPtTargets(long id) => _resolve().ContinueMouPtTargets(id);
    internal bool DrainMouPt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouPt(f);
    internal void FilterMouPtConversion(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> playable, int? selectedPhysicalCardId = default) => _resolve().FilterMouPtConversion(actions, actor, playable, selectedPhysicalCardId);
    internal bool FinishMouPtDeadSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouPtDeadSource(f);
    internal bool HasMouPangTongExpandedTargets(global::CardGame.Core.CharacterState owner) => _resolve().HasMouPangTongExpandedTargets(owner);
    internal bool IsIssuedMouPtTargets(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsIssuedMouPtTargets(candidate, c);
    internal bool IsMouPtDying() => _resolve().IsMouPtDying();
    internal bool IsMouPtRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouPtRecoveryParent(f, recovery);
    internal bool MouPtFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouPtFirstChild(f, child);
    internal bool MouPtLateNotification(global::CardGame.Core.ProgramSkillWindowContext c, global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardConversionSource source, string hash) => _resolve().MouPtLateNotification(c, use, source, hash);
    internal global::CardGame.Core.ProgramSkillFrame? MouPtObserverRoot() => _resolve().MouPtObserverRoot();
    internal bool MouPtObserverSuffix(int i) => _resolve().MouPtObserverSuffix(i);
    internal bool MouPtPaidBySource(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardConversionSource source, string gameplayHash) => _resolve().MouPtPaidBySource(use, source, gameplayHash);
    internal bool MouPtPaidFor(global::CardGame.Core.CardUseFrame use, int owner) => _resolve().MouPtPaidFor(use, owner);
    internal bool MouPtUpgraded(int owner) => _resolve().MouPtUpgraded(owner);
    internal global::CardGame.Core.CardUseFrame? MouPtUse(global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().MouPtUse(c);
    internal bool MouPtVisited(long useId, int owner, int target) => _resolve().MouPtVisited(useId, owner, target);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouPt(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ResolveMouPt(supplied, op);
    internal bool ResumeMouPt(long id) => _resolve().ResumeMouPt(id);
    internal bool ReturnMouPtMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouPtMovement(f);
    internal bool ValidMouPtTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouPtTargets(f);
    internal bool HasMouPangTongCapability { get => _resolve().HasMouPangTongCapability; }
}
