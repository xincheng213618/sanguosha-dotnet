#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaManOlEntries
{
    internal void BeginHuaMan(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.HuaManStage stage, global::CardGame.Core.SkillProgramEffectOp op, bool paid = false) => _resolve().BeginHuaMan(f, stage, op, paid);
    internal bool CanContinueHuaMan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueHuaMan(f);
    internal bool CanOfferHuaMan(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().CanOfferHuaMan(c, t, w);
    internal void CaptureHuaManCompletedUse(long id, global::CardGame.Core.CardActionContext action) => _resolve().CaptureHuaManCompletedUse(id, action);
    internal void CaptureHuaManFinalizedUse(long id, global::CardGame.Core.CardActionContext action) => _resolve().CaptureHuaManFinalizedUse(id, action);
    internal void CompleteZhanyuan(global::CardGame.Core.ProgramSkillFrame f, int? male) => _resolve().CompleteZhanyuan(f, male);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateHuaManCardUseContext(global::CardGame.Core.ProgramCardTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CreateHuaManCardUseContext(frame, c, context);
    internal bool DrainHuaMan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainHuaMan(f);
    internal bool ExactSouyingUse(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().ExactSouyingUse(f, out use);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteHuaMan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteHuaMan(supplied, op);
    internal void FinishHuaMan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHuaMan(f);
    internal bool HasActualSouyingObserver() => _resolve().HasActualSouyingObserver();
    internal bool HasHuaManBeforeDamageObserver(long id) => _resolve().HasHuaManBeforeDamageObserver(id);
    internal bool HasHuaManDamageObserver(long id) => _resolve().HasHuaManDamageObserver(id);
    internal bool HasHuaManDiscard(int seat) => _resolve().HasHuaManDiscard(seat);
    internal bool HasHuaManSouyingCancellation(global::CardGame.Core.CardUseFrame use) => _resolve().HasHuaManSouyingCancellation(use);
    internal bool HasRuntimeXili(int seat) => _resolve().HasRuntimeXili(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HuaManChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuaManChoices(f);
    internal bool HuaManCurrent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuaManCurrent(f);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> HuaManDiscardCards(int seat) => _resolve().HuaManDiscardCards(seat);
    internal bool HuaManDiscardLocation(int seat, global::CardGame.Core.CardLocation at) => _resolve().HuaManDiscardLocation(seat, at);
    internal bool HuaManFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().HuaManFinalizedAfterCategoryTargetWindow(query);
    internal bool HuaManFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().HuaManFirstChild(root, child);
    internal bool HuaManLionCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuaManLionCost(f);
    internal global::CardGame.Core.ProgramSkillFrame? HuaManObserverRoot() => _resolve().HuaManObserverRoot();
    internal bool IsHuaManClaimedEntity(long useId, int cardId) => _resolve().IsHuaManClaimedEntity(useId, cardId);
    internal bool IsHuaManMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsHuaManMovement(f, effect, pending);
    internal bool IsHuaManSouyingMutationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SouyingNativeMutationEvent fact, global::System.Collections.Generic.IReadOnlyList<int> original) => _resolve().IsHuaManSouyingMutationFact(use, fact, original);
    internal void ObserveAcceptedHuaManUse(global::CardGame.Core.CardActionAcceptedEvent accepted) => _resolve().ObserveAcceptedHuaManUse(accepted);
    internal void ObserveHuaManAcceptedUse(global::CardGame.Core.CardActionContext action) => _resolve().ObserveHuaManAcceptedUse(action);
    internal void PayHuaManDraw(global::CardGame.Core.ProgramSkillFrame f, int subject, int amount, global::CardGame.Core.HuaManStage stage) => _resolve().PayHuaManDraw(f, subject, amount, stage);
    internal void PaySouyingMutation(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PaySouyingMutation(f);
    internal void PayXiliIncrease(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayXiliIncrease(f);
    internal void PublishHuaManChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHuaManChoice(f);
    internal void RecordHuaManMovement(global::CardGame.Core.ProgramSkillFrame f, int subject) => _resolve().RecordHuaManMovement(f, subject);
    internal void ResolveHuaManChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveHuaManChoice(choice);
    internal bool ResumeHuaMan(long id) => _resolve().ResumeHuaMan(id);
    internal bool ReturnHuaManMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHuaManMovement(f);
    internal global::CardGame.Core.PromptChoice SelectHuaManAi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectHuaManAi(f);
    internal bool TryAdvanceHuaManSubtree() => _resolve().TryAdvanceHuaManSubtree();
    internal bool TryGetSouyingCancelledVirtualPrimaryReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent, scoped out global::System.Collections.Generic.IReadOnlyList<int> primaryTarget) => _resolve().TryGetSouyingCancelledVirtualPrimaryReturn(use, parent, out primaryTarget);
    internal bool ValidSouyingMutation(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.SouyingNativeMutation r) => _resolve().ValidSouyingMutation(use, r);
    internal bool HasHuaManCapability { get => _resolve().HasHuaManCapability; }
}
