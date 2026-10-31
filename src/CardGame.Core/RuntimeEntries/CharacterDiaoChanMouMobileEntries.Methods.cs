#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDiaoChanMouMobileEntries
{
    internal bool AdvanceMouDiaoChanChildren() => _resolve().AdvanceMouDiaoChanChildren();
    internal PromptChoice AiMouDiaoChan(PendingDecision p, ProgramSkillFrame f) => _resolve().AiMouDiaoChan(p, f);
    internal void AnswerMouDiaoChan(PromptChoice choice) => _resolve().AnswerMouDiaoChan(choice);
    internal bool CanStartMouDiaoChanLijian(CharacterState owner) => _resolve().CanStartMouDiaoChanLijian(owner);
    internal void CompleteMouDiaoChanRingDuel(long frameId, int index, long useId) => _resolve().CompleteMouDiaoChanRingDuel(frameId, index, useId);
    internal bool ContinueAcceptedMouDiaoChan(ProgramSkillFrame f) => _resolve().ContinueAcceptedMouDiaoChan(f);
    internal void ContinueMouDiaoChan(long id) => _resolve().ContinueMouDiaoChan(id);
    internal bool DrainMouDiaoChanChildren(ProgramSkillFrame f) => _resolve().DrainMouDiaoChanChildren(f);
    internal void FinishMouDiaoChan(ProgramSkillFrame f) => _resolve().FinishMouDiaoChan(f);
    internal bool IsMouDiaoChanDying() => _resolve().IsMouDiaoChanDying();
    internal bool IsMouDiaoChanMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsMouDiaoChanMovement(f, e, p);
    internal bool IsMouDiaoChanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouDiaoChanRecoveryParent(f, recovery);
    internal bool IsMouDiaoChanSilverLionChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().IsMouDiaoChanSilverLionChild(f, child);
    internal void IssueMouDiaoChanDuel(ProgramSkillFrame supplied) => _resolve().IssueMouDiaoChanDuel(supplied);
    internal PromptChoice MouDiaoChanChoice(ProgramSkillFrame f, string branch, string text, int? target = default, int? card = default) => _resolve().MouDiaoChanChoice(f, branch, text, target, card);
    internal IReadOnlyList<PromptChoice> MouDiaoChanChoices(ProgramSkillFrame f) => _resolve().MouDiaoChanChoices(f);
    internal Card[] MouDiaoChanCosts(int owner) => _resolve().MouDiaoChanCosts(owner);
    internal bool MouDiaoChanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouDiaoChanFirstChild(f, child);
    internal bool MouDiaoChanObserverEdge(int n) => _resolve().MouDiaoChanObserverEdge(n);
    internal ProgramSkillFrame? MouDiaoChanObserverRoot() => _resolve().MouDiaoChanObserverRoot();
    internal bool MouDiaoChanObserverSuffix(int index) => _resolve().MouDiaoChanObserverSuffix(index);
    internal int MouDiaoChanPhaseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().MouDiaoChanPhaseUses(owner, phase);
    internal int ProjectMouDiaoChanPhaseUsed(int seat, string skill, int fallback) => _resolve().ProjectMouDiaoChanPhaseUsed(seat, skill, fallback);
    internal void PublishMouDiaoChan(ProgramSkillFrame f) => _resolve().PublishMouDiaoChan(f);
    internal bool ResumeMouDiaoChan(long id) => _resolve().ResumeMouDiaoChan(id);
    internal void ReturnMouDiaoChanBiyue(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnMouDiaoChanBiyue(f, outcome);
    internal bool ReturnMouDiaoChanMovement(ProgramSkillFrame f) => _resolve().ReturnMouDiaoChanMovement(f);
    internal bool ReturnMouDiaoChanRingAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnMouDiaoChanRingAttack(completion);
    internal bool ReturnMouDiaoChanRingUse(CardUseFrame use) => _resolve().ReturnMouDiaoChanRingUse(use);
    internal void SealMouDiaoChanMovement(long id, long before) => _resolve().SealMouDiaoChanMovement(id, before);
    internal SkillProgramStepOutcome StartMouDiaoChanMobile(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().StartMouDiaoChanMobile(supplied, op);
    internal bool ValidMouDiaoChan(ProgramSkillFrame f) => _resolve().ValidMouDiaoChan(f);
}
