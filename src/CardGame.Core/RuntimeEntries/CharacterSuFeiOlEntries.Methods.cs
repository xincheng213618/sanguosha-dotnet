#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSuFeiOlEntries
{
    internal SkillProgramStepOutcome BeginSuFei(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginSuFei(supplied, effect);
    internal bool CanContinueSuFei(ProgramSkillFrame f) => _resolve().CanContinueSuFei(f);
    internal bool CanGiveSuFei(ProgramSkillFrame f) => _resolve().CanGiveSuFei(f);
    internal bool CanOfferSuFei(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferSuFei(c, t, w);
    internal bool DrainSuFei(ProgramSkillFrame f) => _resolve().DrainSuFei(f);
    internal void FinishSuFei(ProgramSkillFrame f) => _resolve().FinishSuFei(f);
    internal void PublishSuFeiChoice(ProgramSkillFrame f) => _resolve().PublishSuFeiChoice(f);
    internal void RequireSuFei(ProgramSkillFrame f) => _resolve().RequireSuFei(f);
    internal void ResolveSuFeiChoice(PromptChoice choice) => _resolve().ResolveSuFeiChoice(choice);
    internal bool ResumeSuFei(long id) => _resolve().ResumeSuFei(id);
    internal bool ReturnSuFeiMovement(ProgramSkillFrame f) => _resolve().ReturnSuFeiMovement(f);
    internal PromptChoice SuFeiAiChoice(ProgramSkillFrame f) => _resolve().SuFeiAiChoice(f);
    internal IReadOnlyList<PromptChoice> SuFeiChoices(ProgramSkillFrame f) => _resolve().SuFeiChoices(f);
    internal bool SuFeiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().SuFeiFirstChild(root, child);
    internal ProgramSkillFrame? SuFeiObserverRoot() => _resolve().SuFeiObserverRoot();
    internal bool TryAdvanceSuFeiSubtree() => _resolve().TryAdvanceSuFeiSubtree();
}
