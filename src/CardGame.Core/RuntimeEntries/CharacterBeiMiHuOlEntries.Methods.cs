#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBeiMiHuOlEntries
{
    internal void ApplyBeiMiHuTargetMutation(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::System.Collections.Generic.IReadOnlyList<int> targets, global::System.Collections.Generic.IReadOnlyList<int> paid, string kind) => _resolve().ApplyBeiMiHuTargetMutation(f, use, window, targets, paid, kind);
    internal void AssertBeiMiHu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertBeiMiHu(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBeiMiHuAwakening(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginBeiMiHuAwakening(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBeiMiHuCardTargets(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginBeiMiHuCardTargets(f, op);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBeiMiHuGuju(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginBeiMiHuGuju(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBeiMiHuMark(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginBeiMiHuMark(f, op);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginBeiMiHuOperation(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginBeiMiHuOperation(supplied, op);
    internal int BeiMiHuActualCards(int owner) => _resolve().BeiMiHuActualCards(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BeiMiHuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeiMiHuChoices(f);
    internal bool BeiMiHuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BeiMiHuFirstChild(f, child);
    internal int[] BeiMiHuLowestUnmarkedOthers(int owner) => _resolve().BeiMiHuLowestUnmarkedOthers(owner);
    internal global::CardGame.Core.ProgramSkillFrame? BeiMiHuObserverRoot() => _resolve().BeiMiHuObserverRoot();
    internal global::CardGame.Core.SkillProgramEffect? BeiMiHuPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeiMiHuPausedEffect(f);
    internal int BeiMiHuReceiptTurn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeiMiHuReceiptTurn(f);
    internal global::CardGame.Core.CardConversionSource BeiMiHuSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeiMiHuSource(f);
    internal bool BeiMiHuTargetGroupLegal(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> group) => _resolve().BeiMiHuTargetGroupLegal(use, group);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<int>> BeiMiHuTargetGroups(global::CardGame.Core.CardUseFrame use) => _resolve().BeiMiHuTargetGroups(use);
    internal int[] BeiMiHuUnmarkedOthers(int owner) => _resolve().BeiMiHuUnmarkedOthers(owner);
    internal bool CanContinueBeiMiHu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueBeiMiHu(f);
    internal bool CanDrawAfterKuiDamage(int owner, global::CardGame.Core.SkillProgramTrigger trigger, int target) => _resolve().CanDrawAfterKuiDamage(owner, trigger, target);
    internal bool CanOfferBeiMiHuCardAction(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferBeiMiHuCardAction(candidate, trigger, context);
    internal bool CanOfferBeiMiHuLifecycle(int owner, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferBeiMiHuLifecycle(owner, trigger, context);
    internal void CompleteBeiMiHuTargetAddition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteBeiMiHuTargetAddition(f);
    internal void ContinueBeiMiHuTurnPrelude() => _resolve().ContinueBeiMiHuTurnPrelude();
    internal bool DrainBeiMiHuChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainBeiMiHuChildren(f);
    internal bool ExactBeiMiHuLifecycleParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactBeiMiHuLifecycleParent(f);
    internal void FinishBeiMiHu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishBeiMiHu(f);
    internal bool HasBeiMiHuBeforeDamageObserver(long id) => _resolve().HasBeiMiHuBeforeDamageObserver(id);
    internal bool HasBeiMiHuDamageObserver(long id) => _resolve().HasBeiMiHuDamageObserver(id);
    internal bool HasBeiMiHuPreludeBindings(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().HasBeiMiHuPreludeBindings(player, window, operation);
    internal bool IsBeiMiHuMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsBeiMiHuMovement(f, effect, pending);
    internal bool KuiAppendTargetFinalizedInstantTrickRequirementTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().KuiAppendTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal bool KuiAppendTargetNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().KuiAppendTargetNormalizeImplicitDrawTwoTargetWindow(query);
    internal bool KuiCancelTargetFinalizedInstantTrickRequirementTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().KuiCancelTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal bool KuiCancelTargetNormalizeImplicitDrawTwoTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().KuiCancelTargetNormalizeImplicitDrawTwoTargetWindow(query);
    internal global::CardGame.Core.BeiMiHuReceipt NewBeiMiHuReceipt(global::CardGame.Core.ProgramSkillFrame f, string stage) => _resolve().NewBeiMiHuReceipt(f, stage);
    internal void PayBeiMiHuKui(global::CardGame.Core.ProgramSkillFrame f, int target, int delta, string kind) => _resolve().PayBeiMiHuKui(f, target, delta, kind);
    internal bool ProveBeiMiHuTurnPrelude(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveBeiMiHuTurnPrelude(owner, context);
    internal void PublishBeiMiHuChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishBeiMiHuChoice(f);
    internal global::CardGame.Core.CardUseFrame RequireBeiMiHuCardParent(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().RequireBeiMiHuCardParent(f, out window);
    internal void ResolveBeiMiHuChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveBeiMiHuChoice(selected);
    internal bool ResumeBeiMiHu(long id) => _resolve().ResumeBeiMiHu(id);
    internal bool ReturnBeiMiHuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBeiMiHuMovement(f);
    internal void ReturnBeiMiHuTurnPreludeWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().ReturnBeiMiHuTurnPreludeWindow(window);
    internal bool TryAdvanceBeiMiHuSubtree() => _resolve().TryAdvanceBeiMiHuSubtree();
    internal bool TryBeginBeiMiHuActualTurnPrelude(global::CardGame.Core.CharacterState owner) => _resolve().TryBeginBeiMiHuActualTurnPrelude(owner);
    internal bool TryGetBeiMiHuCardUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.SkillProgramEffectOp op, scoped out global::CardGame.Core.CardUseFrame use, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().TryGetBeiMiHuCardUse(owner, context, op, out use, out window);
    internal bool ValidBeiMiHu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidBeiMiHu(f);
    internal bool ValidBeiMiHuBeforeTurnPrelude(global::CardGame.Core.BeiMiHuBeforeTurnProof proof) => _resolve().ValidBeiMiHuBeforeTurnPrelude(proof);
    internal bool ValidBeiMiHuPreludeLifecycleWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().ValidBeiMiHuPreludeLifecycleWindow(window);
    internal bool ValidBeiMiHuTargetMutation(global::CardGame.Core.CardUseFrame use) => _resolve().ValidBeiMiHuTargetMutation(use);
    internal int BeiMiHuRound { get => _resolve().BeiMiHuRound; }
}
