#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiAnniversaryGodEntries
{
    internal PromptChoice AiShenJw(PendingDecision d, ProgramSkillFrame f) => _resolve().AiShenJw(d, f);
    internal void AnswerShenJw(PromptChoice c) => _resolve().AnswerShenJw(c);
    internal SkillProgramStepOutcome BeginShenJw(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginShenJw(supplied, op);
    internal bool CanRunShenJw(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanRunShenJw(c, t, w);
    internal void CaptureShenJwCompletedName(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> candidates) => _resolve().CaptureShenJwCompletedName(action, window, candidates);
    internal void CaptureShenJwDiscardOrigins(CardMovementBatchContext b) => _resolve().CaptureShenJwDiscardOrigins(b);
    internal bool CompleteShenJwAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteShenJwAttack(attack, out paused);
    internal void CompleteShenJwPingxiang(ProgramSkillFrame f) => _resolve().CompleteShenJwPingxiang(f);
    internal void CompleteShenJwSlash(long id) => _resolve().CompleteShenJwSlash(id);
    internal void ContinueShenJw(long id) => _resolve().ContinueShenJw(id);
    internal bool DrainShenJw(long id) => _resolve().DrainShenJw(id);
    internal void FinishShenJw(ProgramSkillFrame supplied) => _resolve().FinishShenJw(supplied);
    internal bool HasShenJwPaidPingxiang(ProgramSkillFrame f) => _resolve().HasShenJwPaidPingxiang(f);
    internal bool IsShenJwNativeSlash(CardUseFrame use) => _resolve().IsShenJwNativeSlash(use);
    internal void IssueShenJwDraw(ProgramSkillFrame f) => _resolve().IssueShenJwDraw(f);
    internal void IssueShenJwSlash(ProgramSkillFrame f, int targetSeat) => _resolve().IssueShenJwSlash(f, targetSeat);
    internal void MoveShenJwJiufa(ProgramSkillFrame f, bool gain) => _resolve().MoveShenJwJiufa(f, gain);
    internal void PublishShenJw(ProgramSkillFrame f) => _resolve().PublishShenJw(f);
    internal bool ResumeShenJw(long id) => _resolve().ResumeShenJw(id);
    internal bool ReturnShenJwAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnShenJwAttack(completion);
    internal void ReturnShenJwDraw(ProgramSkillFrame supplied, NativeDrawOutcome o) => _resolve().ReturnShenJwDraw(supplied, o);
    internal bool ReturnShenJwMovement(ProgramSkillFrame f) => _resolve().ReturnShenJwMovement(f);
    internal void ReturnShenJwSlash(CardUseFrame use) => _resolve().ReturnShenJwSlash(use);
    internal int[]? SelectShenJwDiscards(CardMovementBatchContext b, ProgramTriggerCandidate c, ProgramInstructionFeatures features) => _resolve().SelectShenJwDiscards(b, c, features);
    internal IEnumerable<RuleQueryContribution> ShenJiangWeiPingxiangHandLimitContributions(CharacterState p) => _resolve().ShenJiangWeiPingxiangHandLimitContributions(p);
    internal ProgramCardTriggerWindowFrame? ShenJwActionParent(ProgramSkillWindowContext w) => _resolve().ShenJwActionParent(w);
    internal IReadOnlyList<PromptChoice> ShenJwChoices(ProgramSkillFrame f) => _resolve().ShenJwChoices(f);
    internal int[] ShenJwDiscardIndexes(CardMovementBatchContext b) => _resolve().ShenJwDiscardIndexes(b);
    internal int[] ShenJwSlashTargets(ProgramSkillFrame f) => _resolve().ShenJwSlashTargets(f);
    internal void ValidateShenJwSlash(CardUseFrame use) => _resolve().ValidateShenJwSlash(use);
    internal bool HasShenJw { get => _resolve().HasShenJw; }
}
