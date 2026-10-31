#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDiaoChanMouMobileEntries
{
    internal bool AdvanceMouDiaoChanChildren() => _resolve().AdvanceMouDiaoChanChildren();
    internal global::CardGame.Core.PromptChoice AiMouDiaoChan(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiMouDiaoChan(p, f);
    internal void AnswerMouDiaoChan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouDiaoChan(choice);
    internal void AssertMouDiaoChan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMouDiaoChan(f);
    internal void AssertMouDiaoChanRingUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSelectedActorDuelOrigin origin) => _resolve().AssertMouDiaoChanRingUse(use, origin);
    internal bool CanStartMouDiaoChanLijian(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouDiaoChanLijian(owner);
    internal void CompleteMouDiaoChanRingDuel(long frameId, int index, long useId) => _resolve().CompleteMouDiaoChanRingDuel(frameId, index, useId);
    internal bool ContinueAcceptedMouDiaoChan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAcceptedMouDiaoChan(f);
    internal void ContinueMouDiaoChan(long id) => _resolve().ContinueMouDiaoChan(id);
    internal bool DrainMouDiaoChanChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouDiaoChanChildren(f);
    internal void FinishMouDiaoChan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouDiaoChan(f);
    internal bool HasMouDiaoChanBeforeDamageObserver(long id) => _resolve().HasMouDiaoChanBeforeDamageObserver(id);
    internal bool HasMouDiaoChanDamageObserver(long id) => _resolve().HasMouDiaoChanDamageObserver(id);
    internal bool IsMouDiaoChanDying() => _resolve().IsMouDiaoChanDying();
    internal bool IsMouDiaoChanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsMouDiaoChanMovement(f, e, p);
    internal bool IsMouDiaoChanRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouDiaoChanRecoveryParent(f, recovery);
    internal bool IsMouDiaoChanSilverLionChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().IsMouDiaoChanSilverLionChild(f, child);
    internal void IssueMouDiaoChanDuel(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().IssueMouDiaoChanDuel(supplied);
    internal global::CardGame.Core.PromptChoice MouDiaoChanChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string text, int? target = default, int? card = default) => _resolve().MouDiaoChanChoice(f, branch, text, target, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouDiaoChanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDiaoChanChoices(f);
    internal global::CardGame.Core.Card[] MouDiaoChanCosts(int owner) => _resolve().MouDiaoChanCosts(owner);
    internal bool MouDiaoChanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouDiaoChanFirstChild(f, child);
    internal bool MouDiaoChanObserverEdge(int n) => _resolve().MouDiaoChanObserverEdge(n);
    internal global::CardGame.Core.ProgramSkillFrame? MouDiaoChanObserverRoot() => _resolve().MouDiaoChanObserverRoot();
    internal bool MouDiaoChanObserverSuffix(int index) => _resolve().MouDiaoChanObserverSuffix(index);
    internal int MouDiaoChanPhaseUses(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().MouDiaoChanPhaseUses(owner, phase);
    internal int ProjectMouDiaoChanPhaseUsed(int seat, string skill, int fallback) => _resolve().ProjectMouDiaoChanPhaseUsed(seat, skill, fallback);
    internal void PublishMouDiaoChan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMouDiaoChan(f);
    internal bool ResumeMouDiaoChan(long id) => _resolve().ResumeMouDiaoChan(id);
    internal void ReturnMouDiaoChanBiyue(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMouDiaoChanBiyue(f, outcome);
    internal bool ReturnMouDiaoChanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouDiaoChanMovement(f);
    internal bool ReturnMouDiaoChanRingAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnMouDiaoChanRingAttack(completion);
    internal bool ReturnMouDiaoChanRingUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouDiaoChanRingUse(use);
    internal void SealMouDiaoChanMovement(long id, long before) => _resolve().SealMouDiaoChanMovement(id, before);
    internal global::CardGame.Core.SkillProgramStepOutcome StartMouDiaoChanMobile(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().StartMouDiaoChanMobile(supplied, op);
    internal bool ValidMouDiaoChan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouDiaoChan(f);
}
