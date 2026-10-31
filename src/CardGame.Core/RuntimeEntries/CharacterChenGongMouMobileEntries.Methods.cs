#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenGongMouMobileEntries
{
    internal bool AdvanceMouChenGongChildren() => _resolve().AdvanceMouChenGongChildren();
    internal global::CardGame.Core.PromptChoice AiMouChenGongGift(global::CardGame.Core.PendingDecision pending, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouChenGongGift(pending, f);
    internal global::CardGame.Core.PromptChoice AiMouChenGongRelease(global::CardGame.Core.PendingDecision pending, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouChenGongRelease(pending, f);
    internal void AnswerMouChenGongBranch(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouChenGongBranch(selected);
    internal void AnswerMouChenGongGift(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouChenGongGift(selected);
    internal void AnswerMouChenGongRelease(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouChenGongRelease(selected);
    internal global::CardGame.Core.ProgramSkillWindowContext AttachMouChenGongProtection(global::CardGame.Core.BeforeDamageProgramCandidate item, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AttachMouChenGongProtection(item, context);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouChenGongGift(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMouChenGongGift(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouChenGongRelease(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMouChenGongRelease(supplied);
    internal bool CanOfferMouChenGongRelease(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferMouChenGongRelease(c);
    internal bool CanStartMouChenGongGift(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouChenGongGift(owner);
    internal void CaptureMouChenGongLoss(global::CardGame.Core.IGameEvent fact) => _resolve().CaptureMouChenGongLoss(fact);
    internal void ChangeMouChenGongCe(global::CardGame.Core.ProgramSkillFrame f, int delta) => _resolve().ChangeMouChenGongCe(f, delta);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.BeforeDamageProgramCandidate> CollectIssuedMouChenGongZhichiCandidates(int source, int target, bool rangeChainOnly) => _resolve().CollectIssuedMouChenGongZhichiCandidates(source, target, rangeChainOnly);
    internal void ContinueMouChenGongGift(long id) => _resolve().ContinueMouChenGongGift(id);
    internal void ContinueMouChenGongRelease(long id) => _resolve().ContinueMouChenGongRelease(id);
    internal global::CardGame.Core.MouChenGongZhichiProtection? CurrentMouChenGongProtection(int owner) => _resolve().CurrentMouChenGongProtection(owner);
    internal bool DrainMouChenGongChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouChenGongChildren(f);
    internal bool ExactMouChenGongBeforeFrame(global::CardGame.Core.ProgramSkillFrame f, bool allowPrevented) => _resolve().ExactMouChenGongBeforeFrame(f, allowPrevented);
    internal void FinishMouChenGongGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouChenGongGift(f);
    internal bool GateMouChenGongZhichi(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().GateMouChenGongZhichi(gate);
    internal bool IsIssuedMouChenGongZhichiCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedMouChenGongZhichiCandidate(candidate, context);
    internal bool IsMouChenGongDying() => _resolve().IsMouChenGongDying();
    internal bool IsMouChenGongMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMouChenGongMovement(f, e, p);
    internal bool IsMouChenGongRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouChenGongRecoveryParent(f, recovery);
    internal bool IsMouChenGongZhichiBeforeCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().IsMouChenGongZhichiBeforeCandidate(candidate);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouChenGongBranchChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouChenGongBranchChoices(f);
    internal bool MouChenGongFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouChenGongFirstChild(f, child);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouChenGongGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouChenGongGiftChoices(f);
    internal bool MouChenGongGiftRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouChenGongGiftRoot(f);
    internal bool MouChenGongMovementSigned(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().MouChenGongMovementSigned(f, m);
    internal global::CardGame.Core.ProgramSkillFrame? MouChenGongObserverRoot() => _resolve().MouChenGongObserverRoot();
    internal bool MouChenGongObserverSuffix(int index) => _resolve().MouChenGongObserverSuffix(index);
    internal bool MouChenGongPhaseSpent(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().MouChenGongPhaseSpent(owner, phase);
    internal int MouChenGongProjectedPhaseUses(int owner, string skill, int fallback) => _resolve().MouChenGongProjectedPhaseUses(owner, skill, fallback);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouChenGongReleaseChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouChenGongReleaseChoices(f);
    internal bool MouChenGongReleaseParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouChenGongReleaseParent(f);
    internal bool MouChenGongReleaseRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouChenGongReleaseRoot(f);
    internal void ObserveMouChenGongZhichi(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveMouChenGongZhichi(fact);
    internal bool OriginalMouChenGongProtectionTurn(global::CardGame.Core.MouChenGongZhichiProtection p) => _resolve().OriginalMouChenGongProtectionTurn(p);
    internal bool PermitAcceptedMouChenGong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitAcceptedMouChenGong(f);
    internal bool PermitIssuedMouChenGong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitIssuedMouChenGong(f);
    internal bool PermitMouChenGongZhichiAfterLoss(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitMouChenGongZhichiAfterLoss(f);
    internal void PublishMouChenGong(global::CardGame.Core.ProgramSkillFrame f, int chooser, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, string text) => _resolve().PublishMouChenGong(f, chooser, choices, text);
    internal bool ResumeMouChenGong(long id) => _resolve().ResumeMouChenGong(id);
    internal void ReturnMouChenGongDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMouChenGongDraw(f, outcome);
    internal bool ReturnMouChenGongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouChenGongMovement(f);
    internal void SealMouChenGongMovements(long id, long before) => _resolve().SealMouChenGongMovements(id, before);
    internal bool ValidMouChenGongGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouChenGongGift(f);
    internal bool ValidMouChenGongProtection(global::CardGame.Core.MouChenGongZhichiProtection p) => _resolve().ValidMouChenGongProtection(p);
    internal bool ValidMouChenGongRelease(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouChenGongRelease(f);
    internal bool HasMouChenGongCapability { get => _resolve().HasMouChenGongCapability; }
    internal int MouChenGongActualTurnOrdinal { get => _resolve().MouChenGongActualTurnOrdinal; }
}
