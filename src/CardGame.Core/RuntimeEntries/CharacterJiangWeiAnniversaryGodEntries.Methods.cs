#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiAnniversaryGodEntries
{
    internal global::CardGame.Core.PromptChoice AiShenJw(global::CardGame.Core.PendingDecision d, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiShenJw(d, f);
    internal void AnswerShenJw(global::CardGame.Core.PromptChoice c) => _resolve().AnswerShenJw(c);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShenJw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShenJw(supplied, op);
    internal bool CanRunShenJw(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanRunShenJw(c, t, w);
    internal void CaptureShenJwCompletedName(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> candidates) => _resolve().CaptureShenJwCompletedName(action, window, candidates);
    internal void CaptureShenJwDiscardOrigins(global::CardGame.Core.CardMovementBatchContext b) => _resolve().CaptureShenJwDiscardOrigins(b);
    internal bool CompleteShenJwAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().CompleteShenJwAttack(attack, out paused);
    internal void CompleteShenJwPingxiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteShenJwPingxiang(f);
    internal void CompleteShenJwSlash(long id) => _resolve().CompleteShenJwSlash(id);
    internal void ContinueShenJw(long id) => _resolve().ContinueShenJw(id);
    internal bool DrainShenJw(long id) => _resolve().DrainShenJw(id);
    internal void FinishShenJw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().FinishShenJw(supplied);
    internal bool HasShenJwPaidPingxiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasShenJwPaidPingxiang(f);
    internal bool IsShenJwNativeSlash(global::CardGame.Core.CardUseFrame use) => _resolve().IsShenJwNativeSlash(use);
    internal void IssueShenJwDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueShenJwDraw(f);
    internal void IssueShenJwSlash(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().IssueShenJwSlash(f, targetSeat);
    internal void MoveShenJwJiufa(global::CardGame.Core.ProgramSkillFrame f, bool gain) => _resolve().MoveShenJwJiufa(f, gain);
    internal bool ProveShenJwMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().ProveShenJwMovement(f, e, pending);
    internal void PublishShenJw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShenJw(f);
    internal bool ResumeShenJw(long id) => _resolve().ResumeShenJw(id);
    internal bool ReturnShenJwAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnShenJwAttack(completion);
    internal void ReturnShenJwDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome o) => _resolve().ReturnShenJwDraw(supplied, o);
    internal bool ReturnShenJwMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenJwMovement(f);
    internal void ReturnShenJwSlash(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnShenJwSlash(use);
    internal int[]? SelectShenJwDiscards(global::CardGame.Core.CardMovementBatchContext b, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().SelectShenJwDiscards(b, c, features);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> ShenJiangWeiPingxiangHandLimitContributions(global::CardGame.Core.CharacterState p) => _resolve().ShenJiangWeiPingxiangHandLimitContributions(p);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? ShenJwActionParent(global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().ShenJwActionParent(w);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShenJwChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShenJwChoices(f);
    internal int[] ShenJwDiscardIndexes(global::CardGame.Core.CardMovementBatchContext b) => _resolve().ShenJwDiscardIndexes(b);
    internal int[] ShenJwSlashTargets(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShenJwSlashTargets(f);
    internal void ValidateShenJwSlash(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateShenJwSlash(use);
    internal bool HasShenJw { get => _resolve().HasShenJw; }
}
