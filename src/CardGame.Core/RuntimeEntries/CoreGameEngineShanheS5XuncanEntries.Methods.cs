#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5XuncanEntries
{
    internal void AnswerShanheS5Yunshen(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShanheS5Yunshen(choice);
    internal void AssertShanheS5XuncanNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertShanheS5XuncanNativeUse(use);
    internal void AssertShanheS5Yunshen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertShanheS5Yunshen(f);
    internal void BeginShanheS5YunshenSlash(global::CardGame.Core.ProgramSkillFrame f, int actorSeat, int targetSeat) => _resolve().BeginShanheS5YunshenSlash(f, actorSeat, targetSeat);
    internal bool CanOfferShanheS5Xuncan(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShanheS5Xuncan(c);
    internal void CaptureShanheS5SkillTarget(global::CardGame.Core.ProgramSkillFrame f, int targetSeat) => _resolve().CaptureShanheS5SkillTarget(f, targetSeat);
    internal bool ContinueAcceptedShanheS5Xuncan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueAcceptedShanheS5Xuncan(f);
    internal bool IsShanheS5YunshenUse(long useId) => _resolve().IsShanheS5YunshenUse(useId);
    internal void PublishShanheS5YunshenChoice(global::CardGame.Core.ProgramSkillFrame f, int[] actors) => _resolve().PublishShanheS5YunshenChoice(f, actors);
    internal void RecordShanheS5FirstElementalDamage(global::CardGame.Core.DamageAppliedEvent damage) => _resolve().RecordShanheS5FirstElementalDamage(damage);
    internal void RecordShanheS5JudgmentEffect(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramJudgmentOutcomeBinding outcome, string stage, int target, bool hit, int requested) => _resolve().RecordShanheS5JudgmentEffect(f, outcome, stage, target, hit, requested);
    internal global::CardGame.Core.ProgramJudgmentOutcomeBinding RequireShanheS5Outcome(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect, int target) => _resolve().RequireShanheS5Outcome(f, effect, target);
    internal global::CardGame.Core.ShanheS5SkillTargetEvent RequireShanheS5SkillTarget(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireShanheS5SkillTarget(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShanheS5Xuncan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ResolveShanheS5Xuncan(supplied, effect);
    internal void ReturnShanheS5XuncanDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome draw) => _resolve().ReturnShanheS5XuncanDraw(supplied, draw);
    internal bool ReturnShanheS5YunshenSlash(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnShanheS5YunshenSlash(completion);
    internal global::CardGame.Core.PromptChoice SelectAiShanheS5Yunshen(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiShanheS5Yunshen(decision, f);
    internal global::CardGame.Core.Suit ShanheS5FenchaiJudgmentSuit(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Suit currentSuit) => _resolve().ShanheS5FenchaiJudgmentSuit(judgment, currentSuit);
    internal int[] ShanheS5YunshenDirections(int owner, int target) => _resolve().ShanheS5YunshenDirections(owner, target);
}
