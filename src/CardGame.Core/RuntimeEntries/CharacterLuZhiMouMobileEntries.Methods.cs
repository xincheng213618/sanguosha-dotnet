#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuZhiMouMobileEntries
{
    internal bool AdvanceMouLuZhiChildren() => _resolve().AdvanceMouLuZhiChildren();
    internal global::CardGame.Core.PromptChoice AiMouLuZhi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouLuZhi(p, f);
    internal void AnswerMouLuZhi(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouLuZhi(selected);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrivateWhirlPileSnapshot>? AppendMouLuZhiRenPiles(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PrivateWhirlPileSnapshot>? existing, int owner, int viewer, bool revealAll) => _resolve().AppendMouLuZhiRenPiles(existing, owner, viewer, revealAll);
    internal void AssertMouLuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMouLuZhi(f);
    internal bool CanOfferMouLuZhi(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouLuZhi(gate);
    internal bool CanStartMouLuZhiYang(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouLuZhiYang(owner);
    internal void CleanupMouLuZhiRen() => _resolve().CleanupMouLuZhiRen();
    internal bool ContinueAcceptedMouLuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAcceptedMouLuZhi(f);
    internal void ContinueMouLuZhi(long id) => _resolve().ContinueMouLuZhi(id);
    internal void FinishMouLuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouLuZhi(f);
    internal bool HasMouLuZhiBeforeDamageObserver(long id) => _resolve().HasMouLuZhiBeforeDamageObserver(id);
    internal bool HasMouLuZhiDamageObserver(long id) => _resolve().HasMouLuZhiDamageObserver(id);
    internal bool IsMouLuZhiDying() => _resolve().IsMouLuZhiDying();
    internal bool IsMouLuZhiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsMouLuZhiMovement(f, e, pending);
    internal bool IsMouLuZhiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouLuZhiRecoveryParent(f, recovery);
    internal bool IsMouLuZhiTrueResponseUse(global::CardGame.Core.CardActionContext action) => _resolve().IsMouLuZhiTrueResponseUse(action);
    internal global::CardGame.Core.MouLuZhiRen[] LiveMouLuZhiRen(int owner) => _resolve().LiveMouLuZhiRen(owner);
    internal global::CardGame.Core.MouLuZhiRen? MatchingMouLuZhiRen(int owner, string instance) => _resolve().MatchingMouLuZhiRen(owner, instance);
    internal global::CardGame.Core.PromptChoice MouLuZhiChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string label, int? card = default, int? target = default) => _resolve().MouLuZhiChoice(f, branch, label, card, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouLuZhiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouLuZhiChoices(f);
    internal bool MouLuZhiCompletion(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MouLuZhiCompletion(c, context);
    internal global::CardGame.Core.Card[] MouLuZhiCostCards(int owner, bool? red, string? instance = default) => _resolve().MouLuZhiCostCards(owner, red, instance);
    internal bool MouLuZhiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouLuZhiFirstChild(f, child);
    internal bool MouLuZhiObserverEdge(int n) => _resolve().MouLuZhiObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? MouLuZhiObserverRoot() => _resolve().MouLuZhiObserverRoot();
    internal bool MouLuZhiObserverSuffix(int index) => _resolve().MouLuZhiObserverSuffix(index);
    internal bool MouLuZhiOutsideTurn(int owner) => _resolve().MouLuZhiOutsideTurn(owner);
    internal int MouLuZhiPhaseUses(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().MouLuZhiPhaseUses(owner, phase);
    internal int MouLuZhiProjectedUses(int owner, string skill, int fallback) => _resolve().MouLuZhiProjectedUses(owner, skill, fallback);
    internal bool MouLuZhiSilverLionChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouLuZhiSilverLionChild(f, child);
    internal int MouLuZhiX(int owner, int target) => _resolve().MouLuZhiX(owner, target);
    internal int[] MouLuZhiYangTargets(int owner, global::CardGame.Core.MouLuZhiRen ren, string instance) => _resolve().MouLuZhiYangTargets(owner, ren, instance);
    internal void PublishMouLuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouLuZhi(f);
    internal bool ResumeMouLuZhi(long id) => _resolve().ResumeMouLuZhi(id);
    internal void ReturnMouLuZhiDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome d) => _resolve().ReturnMouLuZhiDraw(f, d);
    internal bool ReturnMouLuZhiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouLuZhiMovement(f);
    internal void SealMouLuZhiMovements(long id, long before) => _resolve().SealMouLuZhiMovements(id, before);
    internal global::CardGame.Core.SkillProgramStepOutcome StartMouLuZhi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().StartMouLuZhi(supplied, op);
    internal bool ValidMouLuZhi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouLuZhi(f);
    internal bool HasMouLuZhiCapability { get => _resolve().HasMouLuZhiCapability; }
}
