#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuMobileGodEntries
{
    internal global::CardGame.Core.PromptChoice AiShenLsDingzhou(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiShenLsDingzhou(p, f);
    internal global::CardGame.Core.PromptChoice AiShenLsTamo(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiShenLsTamo(p, f);
    internal global::CardGame.Core.PromptChoice AiShenLsZhimeng(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiShenLsZhimeng(p, f);
    internal void AnswerShenLs(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerShenLs(chosen);
    internal void ApplyShenLsOrder(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyShenLsOrder(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShenLs(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShenLs(supplied, op);
    internal bool CompleteShenLsSetup(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().CompleteShenLsSetup(frame);
    internal void ContinueShenLs(long id) => _resolve().ContinueShenLs(id);
    internal void DivideShenLs(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().DivideShenLs(f, target);
    internal bool DrainShenLs(long id) => _resolve().DrainShenLs(id);
    internal void FinishShenLs(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShenLs(f);
    internal bool KeepShenLsPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepShenLsPaid(f);
    internal void MoveShenLs(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> cards, global::CardGame.Core.ShenLuSuStage stage, global::CardGame.Core.CardLocation destination, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? perCard) => _resolve().MoveShenLs(f, cards, stage, destination, perCard);
    internal void PublishShenLs(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShenLs(f);
    internal void RequireShenLs(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireShenLs(f);
    internal bool ResumeShenLs(long id) => _resolve().ResumeShenLs(id);
    internal bool ReturnShenLsMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenLsMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShenLsChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShenLsChoices(f);
    internal bool ShenLsDingzhouTarget(int owner, int target) => _resolve().ShenLsDingzhouTarget(owner, target);
    internal bool ShenLsEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().ShenLsEligible(c, t, w);
    internal int[] ShenLsField(int seat) => _resolve().ShenLsField(seat);
    internal int[] ShenLsGiveable(int seat) => _resolve().ShenLsGiveable(seat);
    internal bool ShenLsMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().ShenLsMovementProof(f, effect, pending);
    internal int[] ShenLsPeers(int owner, string skillId) => _resolve().ShenLsPeers(owner, skillId);
    internal bool ShenLsMode { get => _resolve().ShenLsMode; }
    internal bool ShenLsTeam { get => _resolve().ShenLsTeam; }
}
