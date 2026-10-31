#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangGuanOlEntries
{
    internal bool AdvanceWangGuanSubtree() => _resolve().AdvanceWangGuanSubtree();
    internal void AnswerWangGuan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerWangGuan(choice);
    internal void AssertShiluIdentity(global::CardGame.Core.ShiluHandIdentityGrantedEvent e) => _resolve().AssertShiluIdentity(e);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginWangGuan(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginWangGuan(input, op);
    internal bool CanRunIssuedMiuyanCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunIssuedMiuyanCandidate(c, context);
    internal long CaptureMiuyanQualificationStamp(long underlying) => _resolve().CaptureMiuyanQualificationStamp(underlying);
    internal global::CardGame.Core.PromptChoice ChooseWangGuanAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseWangGuanAi(p, f);
    internal void CollectIssuedMiuyanCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedMiuyanCandidates(action, window, result);
    internal bool ContinueIssuedWangGuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedWangGuan(f);
    internal void ContinueWangGuan(long id) => _resolve().ContinueWangGuan(id);
    internal bool? DecideIssuedTrigger630(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger630(candidate, context);
    internal void FinishWangGuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWangGuan(f);
    internal bool IsIssuedMiuyanCandidate(global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().IsIssuedMiuyanCandidate(c);
    internal bool IsMiuyanCategoryGiftMovementReveal(global::CardGame.Core.IGameEvent fact) => _resolve().IsMiuyanCategoryGiftMovementReveal(fact);
    internal bool IsMiuyanGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsMiuyanGrantQualified(owner, grant);
    internal bool IsMiuyanShandaoMovementReveal(global::CardGame.Core.IGameEvent fact) => _resolve().IsMiuyanShandaoMovementReveal(fact);
    internal void IssueMiuyanUse(long useId, global::CardGame.Core.CardActionContext? action) => _resolve().IssueMiuyanUse(useId, action);
    internal global::CardGame.Core.CardUseFrame? MiuyanCandidateUse(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MiuyanCandidateUse(c, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.WangGuanHandEntity> MiuyanClaimEntities(global::CardGame.Core.MiuyanUseInvoice use, int owner) => _resolve().MiuyanClaimEntities(use, owner);
    internal global::System.Collections.Generic.IEnumerable<int> MiuyanDraftHandIds(global::CardGame.Core.ProgramHandDraftRevealedEvent e) => _resolve().MiuyanDraftHandIds(e);
    internal global::System.Collections.Generic.IEnumerable<int> MiuyanJuanxiaHandIds(global::CardGame.Core.ProgramJuanxiaTrickUsedEvent e) => _resolve().MiuyanJuanxiaHandIds(e);
    internal global::System.Collections.Generic.IEnumerable<int> MiuyanQingliangHandIds(global::CardGame.Core.ProgramQingliangRevealEvent e) => _resolve().MiuyanQingliangHandIds(e);
    internal bool MiuyanUseProof(global::CardGame.Core.CardUseFrame use) => _resolve().MiuyanUseProof(use);
    internal void ObserveMiuyanPhaseHandReveal(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveMiuyanPhaseHandReveal(fact);
    internal void PublishWangGuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWangGuan(f);
    internal bool QualifyWangGuan(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyWangGuan(c, t, context);
    internal void RecordWangGuanMovement(long id, long before, int count) => _resolve().RecordWangGuanMovement(id, before, count);
    internal bool ResumeWangGuan(long id) => _resolve().ResumeWangGuan(id);
    internal bool ReturnWangGuanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWangGuanMovement(f);
    internal global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? ShiluIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().ShiluIdentity(owner, card);
    internal global::System.Collections.Generic.IReadOnlyList<int> ShiluTargets(int owner) => _resolve().ShiluTargets(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WangGuanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangGuanChoices(f);
    internal bool WangGuanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().WangGuanFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? WangGuanObserverRoot() => _resolve().WangGuanObserverRoot();
    internal bool HasMiuyanCapability { get => _resolve().HasMiuyanCapability; }
    internal bool HasShiluCapability { get => _resolve().HasShiluCapability; }
}
