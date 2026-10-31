#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSuFeiOlEntries
{
    internal SkillProgramStepOutcome BeginSuFei(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().BeginSuFei(supplied, effect);
    internal bool CanContinueSuFei(ProgramSkillFrame f) => _resolve().CanContinueSuFei(f);
    internal bool CanGiveSuFei(ProgramSkillFrame f) => _resolve().CanGiveSuFei(f);
    internal bool CanOfferSuFei(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferSuFei(c, t, w);
    internal void CaptureSuFeiFinalizedUse(long nativeUseId, CardActionContext action) => _resolve().CaptureSuFeiFinalizedUse(nativeUseId, action);
    internal bool DrainSuFei(ProgramSkillFrame f) => _resolve().DrainSuFei(f);
    internal void FinishSuFei(ProgramSkillFrame f) => _resolve().FinishSuFei(f);
    internal void ObserveSuFeiTurnOrDeath(IGameEvent payload) => _resolve().ObserveSuFeiTurnOrDeath(payload);
    internal void PublishSuFeiChoice(ProgramSkillFrame f) => _resolve().PublishSuFeiChoice(f);
    internal void RequireSuFei(ProgramSkillFrame f) => _resolve().RequireSuFei(f);
    internal void ResolveSuFeiChoice(PromptChoice choice) => _resolve().ResolveSuFeiChoice(choice);
    internal bool ResumeSuFei(long id) => _resolve().ResumeSuFei(id);
    internal bool ReturnSuFeiMovement(ProgramSkillFrame f) => _resolve().ReturnSuFeiMovement(f);
    internal PromptChoice SuFeiAiChoice(ProgramSkillFrame f) => _resolve().SuFeiAiChoice(f);
    internal IReadOnlyList<PromptChoice> SuFeiChoices(ProgramSkillFrame f) => _resolve().SuFeiChoices(f);
    internal bool SuFeiFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().SuFeiFinalizedAfterCategoryTargetWindow(query);
    internal bool SuFeiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().SuFeiFirstChild(root, child);
    internal bool SuFeiNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().SuFeiNormalizeImplicitDrawTwoTargetWindow(query);
    internal ProgramSkillFrame? SuFeiObserverRoot() => _resolve().SuFeiObserverRoot();
    internal bool SuFeiOwnerQualified(int seat) => _resolve().SuFeiOwnerQualified(seat);
    internal int[] SuFeiPrimaryTargets(CardActionContext a) => _resolve().SuFeiPrimaryTargets(a);
    internal bool SuFeiUseIssued(SuFeiActualUse u) => _resolve().SuFeiUseIssued(u);
    internal bool TryAdvanceSuFeiSubtree() => _resolve().TryAdvanceSuFeiSubtree();
    internal bool ValidSuFeiQualification(SuFeiConsecutiveQualification q, int owner) => _resolve().ValidSuFeiQualification(q, owner);
    internal bool HasSuFeiCapability { get => _resolve().HasSuFeiCapability; }
}
