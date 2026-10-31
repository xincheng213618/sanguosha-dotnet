#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSuFeiOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSuFei(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginSuFei(supplied, effect);
    internal bool CanContinueSuFei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueSuFei(f);
    internal bool CanGiveSuFei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanGiveSuFei(f);
    internal bool CanOfferSuFei(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferSuFei(c, t, w);
    internal void CaptureSuFeiFinalizedUse(long nativeUseId, global::CardGame.Core.CardActionContext action) => _resolve().CaptureSuFeiFinalizedUse(nativeUseId, action);
    internal bool DrainSuFei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainSuFei(f);
    internal void FinishSuFei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishSuFei(f);
    internal bool HasSuFeiBeforeDamageObserver(long id) => _resolve().HasSuFeiBeforeDamageObserver(id);
    internal bool HasSuFeiDamageObserver(long id) => _resolve().HasSuFeiDamageObserver(id);
    internal bool IsSuFeiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsSuFeiMovement(f, e, p);
    internal void ObserveSuFeiTurnOrDeath(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveSuFeiTurnOrDeath(payload);
    internal void PublishSuFeiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSuFeiChoice(f);
    internal void RequireSuFei(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireSuFei(f);
    internal void ResolveSuFeiChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSuFeiChoice(choice);
    internal bool ResumeSuFei(long id) => _resolve().ResumeSuFei(id);
    internal bool ReturnSuFeiMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSuFeiMovement(f);
    internal global::CardGame.Core.PromptChoice SuFeiAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SuFeiAiChoice(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SuFeiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SuFeiChoices(f);
    internal bool SuFeiFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().SuFeiFinalizedAfterCategoryTargetWindow(query);
    internal bool SuFeiFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().SuFeiFirstChild(root, child);
    internal bool SuFeiNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().SuFeiNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::CardGame.Core.ProgramSkillFrame? SuFeiObserverRoot() => _resolve().SuFeiObserverRoot();
    internal bool SuFeiOwnerQualified(int seat) => _resolve().SuFeiOwnerQualified(seat);
    internal int[] SuFeiPrimaryTargets(global::CardGame.Core.CardActionContext a) => _resolve().SuFeiPrimaryTargets(a);
    internal bool SuFeiUseIssued(global::CardGame.Core.SuFeiActualUse u) => _resolve().SuFeiUseIssued(u);
    internal bool TryAdvanceSuFeiSubtree() => _resolve().TryAdvanceSuFeiSubtree();
    internal bool ValidSuFeiQualification(global::CardGame.Core.SuFeiConsecutiveQualification q, int owner) => _resolve().ValidSuFeiQualification(q, owner);
    internal bool HasSuFeiCapability { get => _resolve().HasSuFeiCapability; }
}
