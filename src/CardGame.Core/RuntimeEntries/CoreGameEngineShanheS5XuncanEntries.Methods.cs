#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5XuncanEntries
{
    internal void AnswerShanheS5Yunshen(PromptChoice choice) => _resolve().AnswerShanheS5Yunshen(choice);
    internal void BeginShanheS5YunshenSlash(ProgramSkillFrame f, int actorSeat, int targetSeat) => _resolve().BeginShanheS5YunshenSlash(f, actorSeat, targetSeat);
    internal bool CanOfferShanheS5Xuncan(ProgramTriggerLegalityContext c) => _resolve().CanOfferShanheS5Xuncan(c);
    internal void CaptureShanheS5SkillTarget(ProgramSkillFrame f, int targetSeat) => _resolve().CaptureShanheS5SkillTarget(f, targetSeat);
    internal bool ContinueAcceptedShanheS5Xuncan(ProgramSkillFrame f) => _resolve().ContinueAcceptedShanheS5Xuncan(f);
    internal bool IsShanheS5YunshenUse(long useId) => _resolve().IsShanheS5YunshenUse(useId);
    internal void PublishShanheS5YunshenChoice(ProgramSkillFrame f, int[] actors) => _resolve().PublishShanheS5YunshenChoice(f, actors);
    internal void RecordShanheS5FirstElementalDamage(DamageAppliedEvent damage) => _resolve().RecordShanheS5FirstElementalDamage(damage);
    internal void RecordShanheS5JudgmentEffect(ProgramSkillFrame f, ProgramJudgmentOutcomeBinding outcome, string stage, int target, bool hit, int requested) => _resolve().RecordShanheS5JudgmentEffect(f, outcome, stage, target, hit, requested);
    internal ProgramJudgmentOutcomeBinding RequireShanheS5Outcome(ProgramSkillFrame f, SkillProgramEffect effect, int target) => _resolve().RequireShanheS5Outcome(f, effect, target);
    internal ShanheS5SkillTargetEvent RequireShanheS5SkillTarget(ProgramSkillFrame f) => _resolve().RequireShanheS5SkillTarget(f);
    internal SkillProgramStepOutcome ResolveShanheS5Xuncan(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ResolveShanheS5Xuncan(supplied, effect);
    internal void ReturnShanheS5XuncanDraw(ProgramSkillFrame supplied, NativeDrawOutcome draw) => _resolve().ReturnShanheS5XuncanDraw(supplied, draw);
    internal bool ReturnShanheS5YunshenSlash(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnShanheS5YunshenSlash(completion);
    internal PromptChoice SelectAiShanheS5Yunshen(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiShanheS5Yunshen(decision, f);
    internal Suit ShanheS5FenchaiJudgmentSuit(JudgmentFrame judgment, Suit currentSuit) => _resolve().ShanheS5FenchaiJudgmentSuit(judgment, currentSuit);
    internal int[] ShanheS5YunshenDirections(int owner, int target) => _resolve().ShanheS5YunshenDirections(owner, target);
}
