#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextActualUseTargetAdjustmentsEntries
{
    internal void AddNextActualUseAdjustmentActions(List<LegalAction> actions, CharacterState actor) => _resolve().AddNextActualUseAdjustmentActions(actions, actor);
    internal bool ApplyNextActualUseGlobalRemoval(ProgramSkillFrame frame, IReadOnlyList<int> normal, scoped out IReadOnlyList<int> targets) => _resolve().ApplyNextActualUseGlobalRemoval(frame, normal, out targets);
    internal void BeginNextActualUseMaterialSlash(CharacterState owner, IReadOnlyList<Card> cards, CardKind kind, IReadOnlyList<int> seats, CardConversionSource? source, NextActualUseTargetAdjustmentGrantedEvent grant, ProgramSkillFrame? frame) => _resolve().BeginNextActualUseMaterialSlash(owner, cards, kind, seats, source, grant, frame);
    internal SkillProgramStepOutcome BeginNextActualUseSelectedSlash(ProgramSkillFrame frame, ProgramMultiCardViewAsSelection selection) => _resolve().BeginNextActualUseSelectedSlash(frame, selection);
    internal bool CanBeExtraTargetAdjustmentTarget(CharacterState actor, CharacterState target, LegalAction action, CardKind kind, Suit suit) => _resolve().CanBeExtraTargetAdjustmentTarget(actor, target, action, kind, suit);
    internal bool CanUseNextActualUseSelectedSlash(CharacterState owner, CharacterState target, ProgramMultiCardViewAsSelection selection) => _resolve().CanUseNextActualUseSelectedSlash(owner, target, selection);
    internal NextActualUseTargetAdjustmentGrantedEvent? CurrentNextActualUseAdjustment(int ownerSeat) => _resolve().CurrentNextActualUseAdjustment(ownerSeat);
    internal ProgramNextActualUseAdjustment? FreezeNextActualUseProgramSelection(CharacterState owner, ProgramExecutionPlan plan, IReadOnlyList<int> targets) => _resolve().FreezeNextActualUseProgramSelection(owner, plan, targets);
    internal void GrantNextActualUseTargetAdjustment(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().GrantNextActualUseTargetAdjustment(frame, effect);
    internal bool HasExactNextActualUseAdjustedProgramReturn(CardUseFrame use) => _resolve().HasExactNextActualUseAdjustedProgramReturn(use);
    internal bool HasExactNextActualUseProgramSelection(ProgramSkillFrame frame, ProgramExecutionPlan plan) => _resolve().HasExactNextActualUseProgramSelection(frame, plan);
    internal bool HasNextActualUseAdjustment(CharacterState owner) => _resolve().HasNextActualUseAdjustment(owner);
    internal bool IsNextActualUseAdjustmentAction(CharacterState actor, LegalAction action) => _resolve().IsNextActualUseAdjustmentAction(actor, action);
    internal bool IsNextActualUseCounterspellForbidden(int seat, CardKind kind, CardActionType type) => _resolve().IsNextActualUseCounterspellForbidden(seat, kind, type);
    internal bool IsNextActualUseTrickForbidden(int seat, CardKind kind) => _resolve().IsNextActualUseTrickForbidden(seat, kind);
    internal void MarkNextActualUseProgramChild(CardUseFrame use) => _resolve().MarkNextActualUseProgramChild(use);
    internal long? NextActualUseAdjustedSlashParent(NativeOwners.CardAttackHandle attack) => _resolve().NextActualUseAdjustedSlashParent(attack);
    internal IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> NextActualUseOrdinaryTrickOptions(CharacterState actor, IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> ordinary, Suit? suit, bool? color, Suit? shieldSuit, bool excludeOwner) => _resolve().NextActualUseOrdinaryTrickOptions(actor, ordinary, suit, color, shieldSuit, excludeOwner);
    internal void ObserveNextActualUseAdjustment(IGameEvent payload) => _resolve().ObserveNextActualUseAdjustment(payload);
    internal void PrepareNextActualUseSelectedRecovery(ProgramSkillFrame frame, ProgramMultiCardViewAsSelection selection) => _resolve().PrepareNextActualUseSelectedRecovery(frame, selection);
    internal void ResolveNextActualUseZhangba(CharacterState owner, IReadOnlyList<int> targets, IReadOnlyList<Card> cards) => _resolve().ResolveNextActualUseZhangba(owner, targets, cards);
    internal (CardKind Kind, Suit Suit) TargetAdjustmentAppearance(CharacterState actor, LegalAction action) => _resolve().TargetAdjustmentAppearance(actor, action);
    internal CommandError? ValidateNextActualUseZhangba(CharacterState owner, IReadOnlyList<int> ids, IReadOnlyList<int> targets) => _resolve().ValidateNextActualUseZhangba(owner, ids, targets);
}
