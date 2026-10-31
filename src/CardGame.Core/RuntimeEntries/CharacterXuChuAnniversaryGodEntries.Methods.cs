#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuChuAnniversaryGodEntries
{
    internal void AnswerShenXuChu(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerShenXuChu(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShenXuChu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginShenXuChu(supplied, op);
    internal bool CanResolveZhuangpo(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanResolveZhuangpo(g);
    internal global::CardGame.Core.PromptChoice ChooseShenXuChu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseShenXuChu(p, f);
    internal void ContinueShenXuChu(long id) => _resolve().ContinueShenXuChu(id);
    internal bool DrainShenXuChu(long id) => _resolve().DrainShenXuChu(id);
    internal void FinishShenXuChu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShenXuChu(f);
    internal void NextZhuangpoTarget(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NextZhuangpoTarget(f);
    internal void PublishShenXuChu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShenXuChu(f);
    internal bool ResumeShenXuChu(long id) => _resolve().ResumeShenXuChu(id);
    internal void ReturnShenXuChuDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShenXuChuDraw(supplied, outcome);
    internal bool ReturnShenXuChuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenXuChuMovement(f);
    internal void ReturnZhengqingDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnZhengqingDraw(f, outcome);
    internal void ReturnZhuangpoDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnZhuangpoDraw(f, outcome);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShenXuChuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShenXuChuChoices(f);
    internal (int Damage, int[] Seats) ZhengqingMaximum() => _resolve().ZhengqingMaximum();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhuangpoDiscardOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhuangpoDiscardOptions(f);
    internal int ZhuangpoDuelBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ZhuangpoDuelBonus(attack);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? ZhuangpoParent(global::CardGame.Core.ProgramSkillWindowContext? w) => _resolve().ZhuangpoParent(w);
}
