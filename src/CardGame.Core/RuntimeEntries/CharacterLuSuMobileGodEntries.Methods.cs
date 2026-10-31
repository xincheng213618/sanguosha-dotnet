#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuMobileGodEntries
{
    internal PromptChoice AiShenLsDingzhou(PendingDecision p, ProgramSkillFrame f) => _resolve().AiShenLsDingzhou(p, f);
    internal PromptChoice AiShenLsTamo(PendingDecision p, ProgramSkillFrame f) => _resolve().AiShenLsTamo(p, f);
    internal PromptChoice AiShenLsZhimeng(PendingDecision p, ProgramSkillFrame f) => _resolve().AiShenLsZhimeng(p, f);
    internal void AnswerShenLs(PromptChoice chosen) => _resolve().AnswerShenLs(chosen);
    internal void ApplyShenLsOrder(ProgramSkillFrame f) => _resolve().ApplyShenLsOrder(f);
    internal SkillProgramStepOutcome BeginShenLs(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginShenLs(supplied, op);
    internal bool CompleteShenLsSetup(ProgramLifecycleTriggerWindowFrame frame) => _resolve().CompleteShenLsSetup(frame);
    internal void ContinueShenLs(long id) => _resolve().ContinueShenLs(id);
    internal void DivideShenLs(ProgramSkillFrame f, int target) => _resolve().DivideShenLs(f, target);
    internal bool DrainShenLs(long id) => _resolve().DrainShenLs(id);
    internal void FinishShenLs(ProgramSkillFrame f) => _resolve().FinishShenLs(f);
    internal bool KeepShenLsPaid(ProgramSkillFrame f) => _resolve().KeepShenLsPaid(f);
    internal void MoveShenLs(ProgramSkillFrame f, IReadOnlyList<int> cards, ShenLuSuStage stage, CardLocation destination, Func<Card, CardLocation, CardLocation>? perCard) => _resolve().MoveShenLs(f, cards, stage, destination, perCard);
    internal void PublishShenLs(ProgramSkillFrame f) => _resolve().PublishShenLs(f);
    internal bool ResumeShenLs(long id) => _resolve().ResumeShenLs(id);
    internal bool ReturnShenLsMovement(ProgramSkillFrame f) => _resolve().ReturnShenLsMovement(f);
    internal IReadOnlyList<PromptChoice> ShenLsChoices(ProgramSkillFrame f) => _resolve().ShenLsChoices(f);
    internal bool ShenLsDingzhouTarget(int owner, int target) => _resolve().ShenLsDingzhouTarget(owner, target);
    internal bool ShenLsEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().ShenLsEligible(c, t, w);
    internal int[] ShenLsField(int seat) => _resolve().ShenLsField(seat);
    internal int[] ShenLsGiveable(int seat) => _resolve().ShenLsGiveable(seat);
    internal int[] ShenLsPeers(int owner, string skillId) => _resolve().ShenLsPeers(owner, skillId);
    internal bool ShenLsMode { get => _resolve().ShenLsMode; }
    internal bool ShenLsTeam { get => _resolve().ShenLsTeam; }
}
