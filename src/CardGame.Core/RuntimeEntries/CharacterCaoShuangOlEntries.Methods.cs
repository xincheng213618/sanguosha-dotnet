#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangOlEntries
{
    internal void AnswerCaoShuangShanzhuan(PromptChoice choice) => _resolve().AnswerCaoShuangShanzhuan(choice);
    internal void AnswerCaoShuangTuogu(PromptChoice choice) => _resolve().AnswerCaoShuangTuogu(choice);
    internal SkillProgramStepOutcome BeginCaoShuangShanzhuan(ProgramSkillFrame supplied) => _resolve().BeginCaoShuangShanzhuan(supplied);
    internal SkillProgramStepOutcome BeginCaoShuangTuogu(ProgramSkillFrame supplied) => _resolve().BeginCaoShuangTuogu(supplied);
    internal bool CanContinuePaidCaoShuang(ProgramSkillFrame f) => _resolve().CanContinuePaidCaoShuang(f);
    internal bool CanOfferCaoShuang(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferCaoShuang(c, t, w);
    internal bool CaoShuangCanPlace(int owner, int target) => _resolve().CaoShuangCanPlace(owner, target);
    internal bool CaoShuangFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().CaoShuangFirstChild(root, child);
    internal ProgramSkillFrame? CaoShuangObserverRoot() => _resolve().CaoShuangObserverRoot();
    internal bool CaoShuangPlaceCardEligible(int chooser, int target, Card card, CardZoneKind zone) => _resolve().CaoShuangPlaceCardEligible(chooser, target, card, zone);
    internal string[] CaoShuangPrintedInheritanceSkills(int victim) => _resolve().CaoShuangPrintedInheritanceSkills(victim);
    internal IReadOnlyList<PromptChoice> CaoShuangShanzhuanChoices(ProgramSkillFrame f) => _resolve().CaoShuangShanzhuanChoices(f);
    internal CardConversionSource CaoShuangSource(ProgramSkillFrame f) => _resolve().CaoShuangSource(f);
    internal IReadOnlyList<PromptChoice> CaoShuangTuoguChoices(ProgramSkillFrame f) => _resolve().CaoShuangTuoguChoices(f);
    internal PromptChoice ChooseAiCaoShuangPlacement(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAiCaoShuangPlacement(p, f);
    internal PromptChoice ChooseAiCaoShuangTuogu(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAiCaoShuangTuogu(p, f);
    internal void CompleteCaoShuangProgram(ProgramSkillFrame f, bool completed) => _resolve().CompleteCaoShuangProgram(f, completed);
    internal void ContinueCaoShuangProgram(long id) => _resolve().ContinueCaoShuangProgram(id);
    internal bool DrainCaoShuangChildren(long id) => _resolve().DrainCaoShuangChildren(id);
    internal void PublishCaoShuangShanzhuan(ProgramSkillFrame f) => _resolve().PublishCaoShuangShanzhuan(f);
    internal void PublishCaoShuangTuogu(ProgramSkillFrame f) => _resolve().PublishCaoShuangTuogu(f);
    internal bool ResumeCaoShuang(long id) => _resolve().ResumeCaoShuang(id);
    internal bool TryAdvanceCaoShuangSubtree() => _resolve().TryAdvanceCaoShuangSubtree();
}
