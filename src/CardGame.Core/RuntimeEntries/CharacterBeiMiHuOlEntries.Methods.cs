#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBeiMiHuOlEntries
{
    internal void ApplyBeiMiHuTargetMutation(ProgramSkillFrame f, CardUseFrame use, ProgramCardTriggerWindowFrame window, IReadOnlyList<int> targets, IReadOnlyList<int> paid, string kind) => _resolve().ApplyBeiMiHuTargetMutation(f, use, window, targets, paid, kind);
    internal void AssertBeiMiHu(ProgramSkillFrame f) => _resolve().AssertBeiMiHu(f);
    internal SkillProgramStepOutcome BeginBeiMiHuAwakening(ProgramSkillFrame f) => _resolve().BeginBeiMiHuAwakening(f);
    internal SkillProgramStepOutcome BeginBeiMiHuCardTargets(ProgramSkillFrame f, SkillProgramEffectOp op) => _resolve().BeginBeiMiHuCardTargets(f, op);
    internal SkillProgramStepOutcome BeginBeiMiHuGuju(ProgramSkillFrame f) => _resolve().BeginBeiMiHuGuju(f);
    internal SkillProgramStepOutcome BeginBeiMiHuMark(ProgramSkillFrame f, SkillProgramEffectOp op) => _resolve().BeginBeiMiHuMark(f, op);
    internal SkillProgramStepOutcome BeginBeiMiHuOperation(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginBeiMiHuOperation(supplied, op);
    internal int BeiMiHuActualCards(int owner) => _resolve().BeiMiHuActualCards(owner);
    internal IReadOnlyList<PromptChoice> BeiMiHuChoices(ProgramSkillFrame f) => _resolve().BeiMiHuChoices(f);
    internal bool BeiMiHuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BeiMiHuFirstChild(f, child);
    internal int[] BeiMiHuLowestUnmarkedOthers(int owner) => _resolve().BeiMiHuLowestUnmarkedOthers(owner);
    internal ProgramSkillFrame? BeiMiHuObserverRoot() => _resolve().BeiMiHuObserverRoot();
    internal SkillProgramEffect? BeiMiHuPausedEffect(ProgramSkillFrame f) => _resolve().BeiMiHuPausedEffect(f);
    internal int BeiMiHuReceiptTurn(ProgramSkillFrame f) => _resolve().BeiMiHuReceiptTurn(f);
    internal CardConversionSource BeiMiHuSource(ProgramSkillFrame f) => _resolve().BeiMiHuSource(f);
    internal bool BeiMiHuTargetGroupLegal(CardUseFrame use, IReadOnlyList<int> group) => _resolve().BeiMiHuTargetGroupLegal(use, group);
    internal IReadOnlyList<IReadOnlyList<int>> BeiMiHuTargetGroups(CardUseFrame use) => _resolve().BeiMiHuTargetGroups(use);
    internal int[] BeiMiHuUnmarkedOthers(int owner) => _resolve().BeiMiHuUnmarkedOthers(owner);
    internal bool CanContinueBeiMiHu(ProgramSkillFrame f) => _resolve().CanContinueBeiMiHu(f);
    internal bool CanDrawAfterKuiDamage(int owner, SkillProgramTrigger trigger, int target) => _resolve().CanDrawAfterKuiDamage(owner, trigger, target);
    internal bool CanOfferBeiMiHuCardAction(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferBeiMiHuCardAction(candidate, trigger, context);
    internal bool CanOfferBeiMiHuLifecycle(int owner, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferBeiMiHuLifecycle(owner, trigger, context);
    internal void CompleteBeiMiHuTargetAddition(ProgramSkillFrame f) => _resolve().CompleteBeiMiHuTargetAddition(f);
    internal bool DrainBeiMiHuChildren(ProgramSkillFrame f) => _resolve().DrainBeiMiHuChildren(f);
    internal bool ExactBeiMiHuLifecycleParent(ProgramSkillFrame f) => _resolve().ExactBeiMiHuLifecycleParent(f);
    internal void FinishBeiMiHu(ProgramSkillFrame f) => _resolve().FinishBeiMiHu(f);
    internal bool IsBeiMiHuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().IsBeiMiHuMovement(f, effect, pending);
    internal bool KuiAppendTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().KuiAppendTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal bool KuiAppendTargetNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().KuiAppendTargetNormalizeImplicitDrawTwoTargetWindow(query);
    internal bool KuiCancelTargetFinalizedInstantTrickRequirementTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().KuiCancelTargetFinalizedInstantTrickRequirementTargetWindow(query);
    internal bool KuiCancelTargetNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().KuiCancelTargetNormalizeImplicitDrawTwoTargetWindow(query);
    internal BeiMiHuReceipt NewBeiMiHuReceipt(ProgramSkillFrame f, string stage) => _resolve().NewBeiMiHuReceipt(f, stage);
    internal void PayBeiMiHuKui(ProgramSkillFrame f, int target, int delta, string kind) => _resolve().PayBeiMiHuKui(f, target, delta, kind);
    internal void PublishBeiMiHuChoice(ProgramSkillFrame f) => _resolve().PublishBeiMiHuChoice(f);
    internal CardUseFrame RequireBeiMiHuCardParent(ProgramSkillFrame f, scoped out ProgramCardTriggerWindowFrame window) => _resolve().RequireBeiMiHuCardParent(f, out window);
    internal void ResolveBeiMiHuChoice(PromptChoice selected) => _resolve().ResolveBeiMiHuChoice(selected);
    internal bool ResumeBeiMiHu(long id) => _resolve().ResumeBeiMiHu(id);
    internal bool ReturnBeiMiHuMovement(ProgramSkillFrame f) => _resolve().ReturnBeiMiHuMovement(f);
    internal bool TryAdvanceBeiMiHuSubtree() => _resolve().TryAdvanceBeiMiHuSubtree();
    internal bool TryGetBeiMiHuCardUse(int owner, ProgramSkillWindowContext context, SkillProgramEffectOp op, scoped out CardUseFrame use, scoped out ProgramCardTriggerWindowFrame window) => _resolve().TryGetBeiMiHuCardUse(owner, context, op, out use, out window);
    internal bool ValidBeiMiHu(ProgramSkillFrame f) => _resolve().ValidBeiMiHu(f);
    internal bool ValidBeiMiHuTargetMutation(CardUseFrame use) => _resolve().ValidBeiMiHuTargetMutation(use);
    internal int BeiMiHuRound { get => _resolve().BeiMiHuRound; }
}
