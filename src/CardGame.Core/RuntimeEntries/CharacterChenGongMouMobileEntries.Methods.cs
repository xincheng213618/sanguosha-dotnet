#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenGongMouMobileEntries
{
    internal bool AdvanceMouChenGongChildren() => _resolve().AdvanceMouChenGongChildren();
    internal PromptChoice AiMouChenGongGift(PendingDecision pending, ProgramSkillFrame f) => _resolve().AiMouChenGongGift(pending, f);
    internal PromptChoice AiMouChenGongRelease(PendingDecision pending, ProgramSkillFrame f) => _resolve().AiMouChenGongRelease(pending, f);
    internal void AnswerMouChenGongGift(PromptChoice selected) => _resolve().AnswerMouChenGongGift(selected);
    internal void AnswerMouChenGongRelease(PromptChoice selected) => _resolve().AnswerMouChenGongRelease(selected);
    internal ProgramSkillWindowContext AttachMouChenGongProtection(BeforeDamageProgramCandidate item, ProgramSkillWindowContext context) => _resolve().AttachMouChenGongProtection(item, context);
    internal SkillProgramStepOutcome BeginMouChenGongGift(ProgramSkillFrame supplied) => _resolve().BeginMouChenGongGift(supplied);
    internal SkillProgramStepOutcome BeginMouChenGongRelease(ProgramSkillFrame supplied) => _resolve().BeginMouChenGongRelease(supplied);
    internal bool CanOfferMouChenGongRelease(ProgramTriggerLegalityContext c) => _resolve().CanOfferMouChenGongRelease(c);
    internal bool CanStartMouChenGongGift(CharacterState owner) => _resolve().CanStartMouChenGongGift(owner);
    internal void ChangeMouChenGongCe(ProgramSkillFrame f, int delta) => _resolve().ChangeMouChenGongCe(f, delta);
    internal IEnumerable<BeforeDamageProgramCandidate> CollectIssuedMouChenGongZhichiCandidates(int source, int target, bool rangeChainOnly) => _resolve().CollectIssuedMouChenGongZhichiCandidates(source, target, rangeChainOnly);
    internal void ContinueMouChenGongGift(long id) => _resolve().ContinueMouChenGongGift(id);
    internal void ContinueMouChenGongRelease(long id) => _resolve().ContinueMouChenGongRelease(id);
    internal MouChenGongZhichiProtection? CurrentMouChenGongProtection(int owner) => _resolve().CurrentMouChenGongProtection(owner);
    internal bool DrainMouChenGongChildren(ProgramSkillFrame f) => _resolve().DrainMouChenGongChildren(f);
    internal bool ExactMouChenGongBeforeFrame(ProgramSkillFrame f, bool allowPrevented) => _resolve().ExactMouChenGongBeforeFrame(f, allowPrevented);
    internal bool GateMouChenGongZhichi(ProgramTriggerLegalityContext gate) => _resolve().GateMouChenGongZhichi(gate);
    internal bool IsIssuedMouChenGongZhichiCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedMouChenGongZhichiCandidate(candidate, context);
    internal bool IsMouChenGongDying() => _resolve().IsMouChenGongDying();
    internal bool IsMouChenGongMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsMouChenGongMovement(f, e, p);
    internal bool IsMouChenGongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouChenGongRecoveryParent(f, recovery);
    internal bool IsMouChenGongZhichiBeforeCandidate(ProgramTriggerCandidate candidate) => _resolve().IsMouChenGongZhichiBeforeCandidate(candidate);
    internal bool MouChenGongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouChenGongFirstChild(f, child);
    internal IReadOnlyList<PromptChoice> MouChenGongGiftChoices(ProgramSkillFrame f) => _resolve().MouChenGongGiftChoices(f);
    internal bool MouChenGongGiftRoot(ProgramSkillFrame f) => _resolve().MouChenGongGiftRoot(f);
    internal bool MouChenGongMovementSigned(ProgramSkillFrame f, CardMovementRecord m) => _resolve().MouChenGongMovementSigned(f, m);
    internal ProgramSkillFrame? MouChenGongObserverRoot() => _resolve().MouChenGongObserverRoot();
    internal bool MouChenGongObserverSuffix(int index) => _resolve().MouChenGongObserverSuffix(index);
    internal bool MouChenGongPhaseSpent(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().MouChenGongPhaseSpent(owner, phase);
    internal int MouChenGongProjectedPhaseUses(int owner, string skill, int fallback) => _resolve().MouChenGongProjectedPhaseUses(owner, skill, fallback);
    internal IReadOnlyList<PromptChoice> MouChenGongReleaseChoices(ProgramSkillFrame f) => _resolve().MouChenGongReleaseChoices(f);
    internal bool MouChenGongReleaseParent(ProgramSkillFrame f) => _resolve().MouChenGongReleaseParent(f);
    internal bool MouChenGongReleaseRoot(ProgramSkillFrame f) => _resolve().MouChenGongReleaseRoot(f);
    internal void ObserveMouChenGongZhichi(IGameEvent fact) => _resolve().ObserveMouChenGongZhichi(fact);
    internal bool OriginalMouChenGongProtectionTurn(MouChenGongZhichiProtection p) => _resolve().OriginalMouChenGongProtectionTurn(p);
    internal bool PermitAcceptedMouChenGong(ProgramSkillFrame f) => _resolve().PermitAcceptedMouChenGong(f);
    internal bool PermitIssuedMouChenGong(ProgramSkillFrame f) => _resolve().PermitIssuedMouChenGong(f);
    internal bool PermitMouChenGongZhichiAfterLoss(ProgramSkillFrame f) => _resolve().PermitMouChenGongZhichiAfterLoss(f);
    internal void PublishMouChenGong(ProgramSkillFrame f, int chooser, IReadOnlyList<PromptChoice> choices, string text) => _resolve().PublishMouChenGong(f, chooser, choices, text);
    internal bool ResumeMouChenGong(long id) => _resolve().ResumeMouChenGong(id);
    internal bool ReturnMouChenGongMovement(ProgramSkillFrame f) => _resolve().ReturnMouChenGongMovement(f);
    internal void SealMouChenGongMovements(long id, long before) => _resolve().SealMouChenGongMovements(id, before);
    internal bool ValidMouChenGongGift(ProgramSkillFrame f) => _resolve().ValidMouChenGongGift(f);
    internal bool ValidMouChenGongProtection(MouChenGongZhichiProtection p) => _resolve().ValidMouChenGongProtection(p);
    internal bool ValidMouChenGongRelease(ProgramSkillFrame f) => _resolve().ValidMouChenGongRelease(f);
    internal bool HasMouChenGongCapability { get => _resolve().HasMouChenGongCapability; }
    internal int MouChenGongActualTurnOrdinal { get => _resolve().MouChenGongActualTurnOrdinal; }
}
