#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangOlEntries
{
    internal void AnswerCaoShuangShanzhuan(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerCaoShuangShanzhuan(choice);
    internal void AnswerCaoShuangTuogu(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerCaoShuangTuogu(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCaoShuangShanzhuan(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginCaoShuangShanzhuan(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCaoShuangTuogu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginCaoShuangTuogu(supplied);
    internal bool CanContinuePaidCaoShuang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinuePaidCaoShuang(f);
    internal bool CanOfferCaoShuang(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferCaoShuang(c, t, w);
    internal bool CaoShuangCanPlace(int owner, int target) => _resolve().CaoShuangCanPlace(owner, target);
    internal bool CaoShuangFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().CaoShuangFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? CaoShuangObserverRoot() => _resolve().CaoShuangObserverRoot();
    internal bool CaoShuangPlaceCardEligible(int chooser, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardZoneKind zone) => _resolve().CaoShuangPlaceCardEligible(chooser, target, card, zone);
    internal string[] CaoShuangPrintedInheritanceSkills(int victim) => _resolve().CaoShuangPrintedInheritanceSkills(victim);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CaoShuangShanzhuanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoShuangShanzhuanChoices(f);
    internal global::CardGame.Core.CardConversionSource CaoShuangSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoShuangSource(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CaoShuangTuoguChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CaoShuangTuoguChoices(f);
    internal global::CardGame.Core.PromptChoice ChooseAiCaoShuangPlacement(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAiCaoShuangPlacement(p, f);
    internal global::CardGame.Core.PromptChoice ChooseAiCaoShuangTuogu(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAiCaoShuangTuogu(p, f);
    internal void CompleteCaoShuangProgram(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().CompleteCaoShuangProgram(f, completed);
    internal void ContinueCaoShuangProgram(long id) => _resolve().ContinueCaoShuangProgram(id);
    internal bool DrainCaoShuangChildren(long id) => _resolve().DrainCaoShuangChildren(id);
    internal bool HasCaoShuangDamageObserver(long id) => _resolve().HasCaoShuangDamageObserver(id);
    internal void PublishCaoShuangShanzhuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCaoShuangShanzhuan(f);
    internal void PublishCaoShuangTuogu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCaoShuangTuogu(f);
    internal bool ResumeCaoShuang(long id) => _resolve().ResumeCaoShuang(id);
    internal bool TryAdvanceCaoShuangSubtree() => _resolve().TryAdvanceCaoShuangSubtree();
}
