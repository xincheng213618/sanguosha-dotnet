#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangGuanOlEntries
{
    internal bool AdvanceWangGuanSubtree() => _resolve().AdvanceWangGuanSubtree();
    internal void AnswerWangGuan(PromptChoice choice) => _resolve().AnswerWangGuan(choice);
    internal void AssertShiluIdentity(ShiluHandIdentityGrantedEvent e) => _resolve().AssertShiluIdentity(e);
    internal SkillProgramStepOutcome BeginWangGuan(ProgramSkillFrame input, SkillProgramEffectOp op) => _resolve().BeginWangGuan(input, op);
    internal bool CanRunIssuedMiuyanCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunIssuedMiuyanCandidate(c, context);
    internal long CaptureMiuyanQualificationStamp(long underlying) => _resolve().CaptureMiuyanQualificationStamp(underlying);
    internal PromptChoice ChooseWangGuanAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseWangGuanAi(p, f);
    internal void CollectIssuedMiuyanCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedMiuyanCandidates(action, window, result);
    internal bool ContinueIssuedWangGuan(ProgramSkillFrame f) => _resolve().ContinueIssuedWangGuan(f);
    internal void ContinueWangGuan(long id) => _resolve().ContinueWangGuan(id);
    internal bool? DecideIssuedTrigger630(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().DecideIssuedTrigger630(candidate, context);
    internal void FinishWangGuan(ProgramSkillFrame f) => _resolve().FinishWangGuan(f);
    internal bool IsIssuedMiuyanCandidate(ProgramTriggerCandidate c) => _resolve().IsIssuedMiuyanCandidate(c);
    internal bool IsMiuyanCategoryGiftMovementReveal(IGameEvent fact) => _resolve().IsMiuyanCategoryGiftMovementReveal(fact);
    internal bool IsMiuyanGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsMiuyanGrantQualified(owner, grant);
    internal bool IsMiuyanShandaoMovementReveal(IGameEvent fact) => _resolve().IsMiuyanShandaoMovementReveal(fact);
    internal void IssueMiuyanUse(long useId, CardActionContext? action) => _resolve().IssueMiuyanUse(useId, action);
    internal CardUseFrame? MiuyanCandidateUse(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().MiuyanCandidateUse(c, context);
    internal IReadOnlyList<WangGuanHandEntity> MiuyanClaimEntities(MiuyanUseInvoice use, int owner) => _resolve().MiuyanClaimEntities(use, owner);
    internal IEnumerable<int> MiuyanDraftHandIds(ProgramHandDraftRevealedEvent e) => _resolve().MiuyanDraftHandIds(e);
    internal IEnumerable<int> MiuyanJuanxiaHandIds(ProgramJuanxiaTrickUsedEvent e) => _resolve().MiuyanJuanxiaHandIds(e);
    internal IEnumerable<int> MiuyanQingliangHandIds(ProgramQingliangRevealEvent e) => _resolve().MiuyanQingliangHandIds(e);
    internal bool MiuyanUseProof(CardUseFrame use) => _resolve().MiuyanUseProof(use);
    internal void ObserveMiuyanPhaseHandReveal(IGameEvent fact) => _resolve().ObserveMiuyanPhaseHandReveal(fact);
    internal void PublishWangGuan(ProgramSkillFrame f) => _resolve().PublishWangGuan(f);
    internal bool QualifyWangGuan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyWangGuan(c, t, context);
    internal void RecordWangGuanMovement(long id, long before, int count) => _resolve().RecordWangGuanMovement(id, before, count);
    internal bool ResumeWangGuan(long id) => _resolve().ResumeWangGuan(id);
    internal bool ReturnWangGuanMovement(ProgramSkillFrame f) => _resolve().ReturnWangGuanMovement(f);
    internal GameEngine.ProgramCardIdentityMatch? ShiluIdentity(CharacterState owner, Card card) => _resolve().ShiluIdentity(owner, card);
    internal IReadOnlyList<int> ShiluTargets(int owner) => _resolve().ShiluTargets(owner);
    internal IReadOnlyList<PromptChoice> WangGuanChoices(ProgramSkillFrame f) => _resolve().WangGuanChoices(f);
    internal bool WangGuanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WangGuanFirstChild(f, child);
    internal ProgramSkillFrame? WangGuanObserverRoot() => _resolve().WangGuanObserverRoot();
    internal bool HasMiuyanCapability { get => _resolve().HasMiuyanCapability; }
    internal bool HasShiluCapability { get => _resolve().HasShiluCapability; }
}
