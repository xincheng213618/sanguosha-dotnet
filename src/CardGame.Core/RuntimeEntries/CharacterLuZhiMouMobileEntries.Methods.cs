#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuZhiMouMobileEntries
{
    internal bool AdvanceMouLuZhiChildren() => _resolve().AdvanceMouLuZhiChildren();
    internal PromptChoice AiMouLuZhi(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouLuZhi(p, f);
    internal void AnswerMouLuZhi(PromptChoice selected) => _resolve().AnswerMouLuZhi(selected);
    internal IReadOnlyList<PrivateWhirlPileSnapshot>? AppendMouLuZhiRenPiles(IReadOnlyList<PrivateWhirlPileSnapshot>? existing, int owner, int viewer, bool revealAll) => _resolve().AppendMouLuZhiRenPiles(existing, owner, viewer, revealAll);
    internal bool CanOfferMouLuZhi(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouLuZhi(gate);
    internal bool CanStartMouLuZhiYang(CharacterState owner) => _resolve().CanStartMouLuZhiYang(owner);
    internal void CleanupMouLuZhiRen() => _resolve().CleanupMouLuZhiRen();
    internal bool ContinueAcceptedMouLuZhi(ProgramSkillFrame f) => _resolve().ContinueAcceptedMouLuZhi(f);
    internal void ContinueMouLuZhi(long id) => _resolve().ContinueMouLuZhi(id);
    internal void FinishMouLuZhi(ProgramSkillFrame f) => _resolve().FinishMouLuZhi(f);
    internal bool IsMouLuZhiDying() => _resolve().IsMouLuZhiDying();
    internal bool IsMouLuZhiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) => _resolve().IsMouLuZhiMovement(f, e, pending);
    internal bool IsMouLuZhiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouLuZhiRecoveryParent(f, recovery);
    internal bool IsMouLuZhiTrueResponseUse(CardActionContext action) => _resolve().IsMouLuZhiTrueResponseUse(action);
    internal MouLuZhiRen[] LiveMouLuZhiRen(int owner) => _resolve().LiveMouLuZhiRen(owner);
    internal MouLuZhiRen? MatchingMouLuZhiRen(int owner, string instance) => _resolve().MatchingMouLuZhiRen(owner, instance);
    internal PromptChoice MouLuZhiChoice(ProgramSkillFrame f, string branch, string label, int? card = default, int? target = default) => _resolve().MouLuZhiChoice(f, branch, label, card, target);
    internal IReadOnlyList<PromptChoice> MouLuZhiChoices(ProgramSkillFrame f) => _resolve().MouLuZhiChoices(f);
    internal bool MouLuZhiCompletion(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().MouLuZhiCompletion(c, context);
    internal Card[] MouLuZhiCostCards(int owner, bool? red, string? instance = default) => _resolve().MouLuZhiCostCards(owner, red, instance);
    internal bool MouLuZhiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouLuZhiFirstChild(f, child);
    internal bool MouLuZhiObserverEdge(int n) => _resolve().MouLuZhiObserverEdge(n);
    internal ProgramSkillFrame? MouLuZhiObserverRoot() => _resolve().MouLuZhiObserverRoot();
    internal bool MouLuZhiObserverSuffix(int index) => _resolve().MouLuZhiObserverSuffix(index);
    internal bool MouLuZhiOutsideTurn(int owner) => _resolve().MouLuZhiOutsideTurn(owner);
    internal int MouLuZhiPhaseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().MouLuZhiPhaseUses(owner, phase);
    internal int MouLuZhiProjectedUses(int owner, string skill, int fallback) => _resolve().MouLuZhiProjectedUses(owner, skill, fallback);
    internal bool MouLuZhiSilverLionChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouLuZhiSilverLionChild(f, child);
    internal int MouLuZhiX(int owner, int target) => _resolve().MouLuZhiX(owner, target);
    internal int[] MouLuZhiYangTargets(int owner, MouLuZhiRen ren, string instance) => _resolve().MouLuZhiYangTargets(owner, ren, instance);
    internal void PublishMouLuZhi(ProgramSkillFrame f) => _resolve().PublishMouLuZhi(f);
    internal bool ResumeMouLuZhi(long id) => _resolve().ResumeMouLuZhi(id);
    internal void ReturnMouLuZhiDraw(ProgramSkillFrame f, NativeDrawOutcome d) => _resolve().ReturnMouLuZhiDraw(f, d);
    internal bool ReturnMouLuZhiMovement(ProgramSkillFrame f) => _resolve().ReturnMouLuZhiMovement(f);
    internal void SealMouLuZhiMovements(long id, long before) => _resolve().SealMouLuZhiMovements(id, before);
    internal SkillProgramStepOutcome StartMouLuZhi(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().StartMouLuZhi(supplied, op);
    internal bool ValidMouLuZhi(ProgramSkillFrame f) => _resolve().ValidMouLuZhi(f);
    internal bool HasMouLuZhiCapability { get => _resolve().HasMouLuZhiCapability; }
}
