#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongMouMobileEntries
{
    internal bool AdvanceMouPtChildren() => _resolve().AdvanceMouPtChildren();
    internal ProgramCardTriggerWindowFrame AppendMouPtLateTargets(ProgramCardTriggerWindowFrame frame) => _resolve().AppendMouPtLateTargets(frame);
    internal bool CanOfferMouPt(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouPt(gate);
    internal void CaptureMouPtConversion(CardUseDeclaredEvent e) => _resolve().CaptureMouPtConversion(e);
    internal void CaptureMouPtLateTarget(ProgramCardUseTargetAddedEvent e) => _resolve().CaptureMouPtLateTarget(e);
    internal void CollectPaidMouPtTargets(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectPaidMouPtTargets(action, window, result);
    internal bool ContinueMouPtPaidSource(ProgramSkillFrame f) => _resolve().ContinueMouPtPaidSource(f);
    internal void ContinueMouPtTargets(long id) => _resolve().ContinueMouPtTargets(id);
    internal bool DrainMouPt(ProgramSkillFrame f) => _resolve().DrainMouPt(f);
    internal void FilterMouPtConversion(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = default) => _resolve().FilterMouPtConversion(actions, actor, playable, selectedPhysicalCardId);
    internal bool FinishMouPtDeadSource(ProgramSkillFrame f) => _resolve().FinishMouPtDeadSource(f);
    internal bool HasMouPangTongExpandedTargets(CharacterState owner) => _resolve().HasMouPangTongExpandedTargets(owner);
    internal bool IsIssuedMouPtTargets(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().IsIssuedMouPtTargets(candidate, c);
    internal bool IsMouPtDying() => _resolve().IsMouPtDying();
    internal bool IsMouPtRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouPtRecoveryParent(f, recovery);
    internal bool MouPtFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouPtFirstChild(f, child);
    internal bool MouPtLateNotification(ProgramSkillWindowContext c, CardUseFrame use, CardConversionSource source, string hash) => _resolve().MouPtLateNotification(c, use, source, hash);
    internal ProgramSkillFrame? MouPtObserverRoot() => _resolve().MouPtObserverRoot();
    internal bool MouPtObserverSuffix(int i) => _resolve().MouPtObserverSuffix(i);
    internal bool MouPtPaidBySource(CardUseFrame use, CardConversionSource source, string gameplayHash) => _resolve().MouPtPaidBySource(use, source, gameplayHash);
    internal bool MouPtPaidFor(CardUseFrame use, int owner) => _resolve().MouPtPaidFor(use, owner);
    internal bool MouPtUpgraded(int owner) => _resolve().MouPtUpgraded(owner);
    internal CardUseFrame? MouPtUse(ProgramSkillWindowContext c) => _resolve().MouPtUse(c);
    internal bool MouPtVisited(long useId, int owner, int target) => _resolve().MouPtVisited(useId, owner, target);
    internal SkillProgramStepOutcome ResolveMouPt(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ResolveMouPt(supplied, op);
    internal bool ResumeMouPt(long id) => _resolve().ResumeMouPt(id);
    internal bool ReturnMouPtMovement(ProgramSkillFrame f) => _resolve().ReturnMouPtMovement(f);
    internal bool ValidMouPtTargets(ProgramSkillFrame f) => _resolve().ValidMouPtTargets(f);
    internal bool HasMouPangTongCapability { get => _resolve().HasMouPangTongCapability; }
}
