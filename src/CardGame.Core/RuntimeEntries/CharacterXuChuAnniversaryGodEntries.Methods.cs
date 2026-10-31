#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuChuAnniversaryGodEntries
{
    internal void AnswerShenXuChu(PromptChoice selected) => _resolve().AnswerShenXuChu(selected);
    internal SkillProgramStepOutcome BeginShenXuChu(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginShenXuChu(supplied, op);
    internal bool CanResolveZhuangpo(ProgramTriggerLegalityContext g) => _resolve().CanResolveZhuangpo(g);
    internal PromptChoice ChooseShenXuChu(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseShenXuChu(p, f);
    internal void ContinueShenXuChu(long id) => _resolve().ContinueShenXuChu(id);
    internal bool DrainShenXuChu(long id) => _resolve().DrainShenXuChu(id);
    internal void FinishShenXuChu(ProgramSkillFrame f) => _resolve().FinishShenXuChu(f);
    internal void NextZhuangpoTarget(ProgramSkillFrame f) => _resolve().NextZhuangpoTarget(f);
    internal void PublishShenXuChu(ProgramSkillFrame f) => _resolve().PublishShenXuChu(f);
    internal bool ResumeShenXuChu(long id) => _resolve().ResumeShenXuChu(id);
    internal void ReturnShenXuChuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShenXuChuDraw(supplied, outcome);
    internal bool ReturnShenXuChuMovement(ProgramSkillFrame f) => _resolve().ReturnShenXuChuMovement(f);
    internal void ReturnZhengqingDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnZhengqingDraw(f, outcome);
    internal void ReturnZhuangpoDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnZhuangpoDraw(f, outcome);
    internal IReadOnlyList<PromptChoice> ShenXuChuChoices(ProgramSkillFrame f) => _resolve().ShenXuChuChoices(f);
    internal (int Damage, int[] Seats) ZhengqingMaximum() => _resolve().ZhengqingMaximum();
    internal IReadOnlyList<PromptChoice> ZhuangpoDiscardOptions(ProgramSkillFrame f) => _resolve().ZhuangpoDiscardOptions(f);
    internal int ZhuangpoDuelBonus(GameEngine.IDamageAttempt attack) => _resolve().ZhuangpoDuelBonus(attack);
    internal ProgramCardTriggerWindowFrame? ZhuangpoParent(ProgramSkillWindowContext? w) => _resolve().ZhuangpoParent(w);
}
