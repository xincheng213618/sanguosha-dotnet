#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaManOlEntries
{
    internal void BeginHuaMan(ProgramSkillFrame f, HuaManStage stage, SkillProgramEffectOp op, bool paid = false) => _resolve().BeginHuaMan(f, stage, op, paid);
    internal bool CanContinueHuaMan(ProgramSkillFrame f) => _resolve().CanContinueHuaMan(f);
    internal bool CanOfferHuaMan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().CanOfferHuaMan(c, t, w);
    internal void CaptureHuaManCompletedUse(long id, CardActionContext action) => _resolve().CaptureHuaManCompletedUse(id, action);
    internal void CaptureHuaManFinalizedUse(long id, CardActionContext action) => _resolve().CaptureHuaManFinalizedUse(id, action);
    internal void CompleteZhanyuan(ProgramSkillFrame f, int? male) => _resolve().CompleteZhanyuan(f, male);
    internal ProgramSkillWindowContext CreateHuaManCardUseContext(ProgramCardTriggerWindowFrame frame, ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CreateHuaManCardUseContext(frame, c, context);
    internal bool DrainHuaMan(ProgramSkillFrame f) => _resolve().DrainHuaMan(f);
    internal bool ExactSouyingUse(ProgramSkillFrame f, scoped out CardUseFrame use) => _resolve().ExactSouyingUse(f, out use);
    internal SkillProgramStepOutcome ExecuteHuaMan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ExecuteHuaMan(supplied, op);
    internal void FinishHuaMan(ProgramSkillFrame f) => _resolve().FinishHuaMan(f);
    internal bool HasActualSouyingObserver() => _resolve().HasActualSouyingObserver();
    internal bool HasHuaManDiscard(int seat) => _resolve().HasHuaManDiscard(seat);
    internal bool HasHuaManSouyingCancellation(CardUseFrame use) => _resolve().HasHuaManSouyingCancellation(use);
    internal bool HasRuntimeXili(int seat) => _resolve().HasRuntimeXili(seat);
    internal IReadOnlyList<PromptChoice> HuaManChoices(ProgramSkillFrame f) => _resolve().HuaManChoices(f);
    internal bool HuaManCurrent(ProgramSkillFrame f) => _resolve().HuaManCurrent(f);
    internal IEnumerable<Card> HuaManDiscardCards(int seat) => _resolve().HuaManDiscardCards(seat);
    internal bool HuaManDiscardLocation(int seat, CardLocation at) => _resolve().HuaManDiscardLocation(seat, at);
    internal bool HuaManFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().HuaManFinalizedAfterCategoryTargetWindow(query);
    internal bool HuaManFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().HuaManFirstChild(root, child);
    internal bool HuaManLionCost(ProgramSkillFrame f) => _resolve().HuaManLionCost(f);
    internal ProgramSkillFrame? HuaManObserverRoot() => _resolve().HuaManObserverRoot();
    internal bool IsHuaManClaimedEntity(long useId, int cardId) => _resolve().IsHuaManClaimedEntity(useId, cardId);
    internal bool IsHuaManSouyingMutationFact(CardUseFrame use, SouyingNativeMutationEvent fact, IReadOnlyList<int> original) => _resolve().IsHuaManSouyingMutationFact(use, fact, original);
    internal void ObserveAcceptedHuaManUse(CardActionAcceptedEvent accepted) => _resolve().ObserveAcceptedHuaManUse(accepted);
    internal void ObserveHuaManAcceptedUse(CardActionContext action) => _resolve().ObserveHuaManAcceptedUse(action);
    internal void PayHuaManDraw(ProgramSkillFrame f, int subject, int amount, HuaManStage stage) => _resolve().PayHuaManDraw(f, subject, amount, stage);
    internal void PaySouyingMutation(ProgramSkillFrame f) => _resolve().PaySouyingMutation(f);
    internal void PayXiliIncrease(ProgramSkillFrame f) => _resolve().PayXiliIncrease(f);
    internal void PublishHuaManChoice(ProgramSkillFrame f) => _resolve().PublishHuaManChoice(f);
    internal void RecordHuaManMovement(ProgramSkillFrame f, int subject) => _resolve().RecordHuaManMovement(f, subject);
    internal void ResolveHuaManChoice(PromptChoice choice) => _resolve().ResolveHuaManChoice(choice);
    internal bool ResumeHuaMan(long id) => _resolve().ResumeHuaMan(id);
    internal bool ReturnHuaManMovement(ProgramSkillFrame f) => _resolve().ReturnHuaManMovement(f);
    internal PromptChoice SelectHuaManAi(ProgramSkillFrame f) => _resolve().SelectHuaManAi(f);
    internal bool TryAdvanceHuaManSubtree() => _resolve().TryAdvanceHuaManSubtree();
    internal bool TryGetSouyingCancelledVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, scoped out IReadOnlyList<int> primaryTarget) => _resolve().TryGetSouyingCancelledVirtualPrimaryReturn(use, parent, out primaryTarget);
    internal bool ValidSouyingMutation(CardUseFrame use, SouyingNativeMutation r) => _resolve().ValidSouyingMutation(use, r);
    internal bool HasHuaManCapability { get => _resolve().HasHuaManCapability; }
}
