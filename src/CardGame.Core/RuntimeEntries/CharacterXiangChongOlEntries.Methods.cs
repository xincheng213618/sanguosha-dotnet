#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiangChongOlEntries
{
    internal bool AdvanceXiangSubtree() => _resolve().AdvanceXiangSubtree();
    internal PromptChoice AiXiang(PendingDecision p, ProgramSkillFrame f) => _resolve().AiXiang(p, f);
    internal bool AllowsMuzhen(ProgramActivationLegalityContext gate) => _resolve().AllowsMuzhen(gate);
    internal void AnswerXiang(PromptChoice choice) => _resolve().AnswerXiang(choice);
    internal SkillProgramStepOutcome BeginXiangChong(ProgramSkillFrame input, SkillProgramEffect e) => _resolve().BeginXiangChong(input, e);
    internal void BeginXiangChongGuyingEquipment(ProgramSkillFrame frame, XiangChongEquipmentOrigin origin) => _resolve().BeginXiangChongGuyingEquipment(frame, origin);
    internal bool CanIssueXiangChongGuyingEquipment(ProgramSkillFrame frame, int cardId, CardLocation from) => _resolve().CanIssueXiangChongGuyingEquipment(frame, cardId, from);
    internal void CaptureXiangChongEquipmentMaterialEntry(ProgramSkillFrame f) => _resolve().CaptureXiangChongEquipmentMaterialEntry(f);
    internal void CaptureXiangChongEquipmentResult(CardUseFrame use) => _resolve().CaptureXiangChongEquipmentResult(use);
    internal bool ContinuePaidXiang(ProgramSkillFrame f) => _resolve().ContinuePaidXiang(f);
    internal void ContinueXiang(long id) => _resolve().ContinueXiang(id);
    internal bool DrainXiang(long id) => _resolve().DrainXiang(id);
    internal void FinishXiang(ProgramSkillFrame f, string reason) => _resolve().FinishXiang(f, reason);
    internal void FinishXiangChongEquipment(ProgramSkillFrame f, bool issued) => _resolve().FinishXiangChongEquipment(f, issued);
    internal bool IsXiangChongClaimedUseMaterial(long useId, int cardId) => _resolve().IsXiangChongClaimedUseMaterial(useId, cardId);
    internal bool IsXiangChongEquipmentMaterialClaim(long useId, int cardId, CardActionContext action) => _resolve().IsXiangChongEquipmentMaterialClaim(useId, cardId, action);
    internal bool IsXiangChongGuyingEquipmentRide(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().IsXiangChongGuyingEquipmentRide(ride, beneath);
    internal bool IsXiangChongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsXiangChongRecoveryParent(f, recovery);
    internal bool MatchesXiangChongEquipment(ProgramSkillFrame f, XiangChongEquipmentOrigin origin) => _resolve().MatchesXiangChongEquipment(f, origin);
    internal bool MuzhenTarget(ProgramSkillFrame? f, int owner, int target, string branch) => _resolve().MuzhenTarget(f, owner, target, branch);
    internal bool MuzhenUsed(int owner, string branch, long token) => _resolve().MuzhenUsed(owner, branch, token);
    internal void PayXiang(ProgramSkillFrame f, IReadOnlyList<XiangChongCard> cards, CardLocation to, string reason, string segment) => _resolve().PayXiang(f, cards, to, reason, segment);
    internal void PlaceMuzhen(ProgramSkillFrame f) => _resolve().PlaceMuzhen(f);
    internal void PublishXiang(ProgramSkillFrame f) => _resolve().PublishXiang(f);
    internal bool QualifyGuying(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyGuying(c, t, context);
    internal void RecordXiangChongEquipmentMaterialClaim(ProgramSkillFrame frame, XiangChongEquipmentOrigin origin) => _resolve().RecordXiangChongEquipmentMaterialClaim(frame, origin);
    internal bool ResumeXiang(long id) => _resolve().ResumeXiang(id);
    internal void ReturnXiangEquipment(CardUseFrame use) => _resolve().ReturnXiangEquipment(use);
    internal bool ValidXiang(ProgramSkillFrame f) => _resolve().ValidXiang(f);
    internal bool ValidXiangEquipmentMaterialInvoice(XiangChongEquipmentMaterialClaim c) => _resolve().ValidXiangEquipmentMaterialInvoice(c);
    internal bool ValidXiangEquipmentResult(XiangChongEquipmentResult result) => _resolve().ValidXiangEquipmentResult(result);
    internal bool ValidXiangEquipmentTermination(XiangChongEquipmentResult result, XiangChongEquipmentTermination terminal) => _resolve().ValidXiangEquipmentTermination(result, terminal);
    internal void ValidateXiangChongGuyingEquipment(CardUseFrame use) => _resolve().ValidateXiangChongGuyingEquipment(use);
    internal IReadOnlyList<PromptChoice> XiangChoices(ProgramSkillFrame f) => _resolve().XiangChoices(f);
    internal int XiangDebtCount(int owner) => _resolve().XiangDebtCount(owner);
    internal bool XiangEquipmentActualActor(XiangChongEquipmentOrigin origin, int finalActor) => _resolve().XiangEquipmentActualActor(origin, finalActor);
    internal bool XiangEquipmentMaterialLiveLocation(XiangChongEquipmentMaterialClaim claim, CardUseFrame inner) => _resolve().XiangEquipmentMaterialLiveLocation(claim, inner);
    internal bool XiangEquipmentObserverEdge(int child) => _resolve().XiangEquipmentObserverEdge(child);
    internal bool XiangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().XiangFirstChild(f, child);
    internal CardMovementRecord? XiangLoss(int owner, ProgramSkillWindowContext context) => _resolve().XiangLoss(owner, context);
    internal bool XiangLostStillAvailable(ProgramSkillFrame f) => _resolve().XiangLostStillAvailable(f);
    internal ProgramSkillFrame? XiangObserverRoot() => _resolve().XiangObserverRoot();
    internal IReadOnlyList<XiangChongCard> XiangOwned(int owner, ProgramSkillFrame? f = default, bool discard = false) => _resolve().XiangOwned(owner, f, discard);
    internal long XiangPlayToken(int owner) => _resolve().XiangPlayToken(owner);
}
