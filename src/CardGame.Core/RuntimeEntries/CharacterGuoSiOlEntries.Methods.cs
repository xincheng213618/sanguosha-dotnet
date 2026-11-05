#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoSiOlEntries
{
    internal void AssertGuoSiNativeUse(CardUseFrame use) => _resolve().AssertGuoSiNativeUse(use);
    internal bool CanContinueIssuedGuoSi(ProgramSkillFrame f) => _resolve().CanContinueIssuedGuoSi(f);
    internal bool CanOfferGuoSi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferGuoSi(c, t, w);
    internal bool CompleteGuoSiUseReturn(CardUseFrame use) => _resolve().CompleteGuoSiUseReturn(use);
    internal void ContinueGuoSiNativePayment(long id) => _resolve().ContinueGuoSiNativePayment(id);
    internal bool DrainGuoSi(ProgramSkillFrame f) => _resolve().DrainGuoSi(f);
    internal SkillProgramStepOutcome ExecuteGuoSi(SkillProgramEffect effect, ProgramSkillFrame supplied) => _resolve().ExecuteGuoSi(effect, supplied);
    internal void FinishGuoSi(ProgramSkillFrame f) => _resolve().FinishGuoSi(f);
    internal PromptChoice GuoSiAiChoice(ProgramSkillFrame f) => _resolve().GuoSiAiChoice(f);
    internal IReadOnlyList<PromptChoice> GuoSiChoices(ProgramSkillFrame f) => _resolve().GuoSiChoices(f);
    internal bool GuoSiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().GuoSiFirstChild(root, child);
    internal bool GuoSiNativePaymentEdge(int index) => _resolve().GuoSiNativePaymentEdge(index);
    internal bool GuoSiNativeRidesOn(ProgramSkillFrame f, CardUseFrame use) => _resolve().GuoSiNativeRidesOn(f, use);
    internal ProgramSkillFrame? GuoSiObserverRoot() => _resolve().GuoSiObserverRoot();
    internal bool GuoSiObserverSuffix(int index) => _resolve().GuoSiObserverSuffix(index);
    internal bool IsGuoSiNativeUse(CardUseFrame use) => _resolve().IsGuoSiNativeUse(use);
    internal void IssueSidao(ProgramSkillFrame f, LegalAction action) => _resolve().IssueSidao(f, action);
    internal void PublishGuoSiChoice(ProgramSkillFrame f) => _resolve().PublishGuoSiChoice(f);
    internal void RequireIssuedGuoSi(ProgramSkillFrame f) => _resolve().RequireIssuedGuoSi(f);
    internal void ResolveGuoSiChoice(PromptChoice choice) => _resolve().ResolveGuoSiChoice(choice);
    internal bool ResumeGuoSi(long id) => _resolve().ResumeGuoSi(id);
    internal bool ReturnGuoSiMovement(ProgramSkillFrame f) => _resolve().ReturnGuoSiMovement(f);
    internal void ReturnGuoSiNativeUse(CardUseFrame use) => _resolve().ReturnGuoSiNativeUse(use);
    internal IReadOnlyList<LegalAction> SidaoActions(int owner, IReadOnlyList<int> commonTargets) => _resolve().SidaoActions(owner, commonTargets);
    internal bool TryAdvanceGuoSiSubtree() => _resolve().TryAdvanceGuoSiSubtree();
    internal void ValidateGuoSiNativeUseReturn(CardUseFrame use) => _resolve().ValidateGuoSiNativeUseReturn(use);
}
