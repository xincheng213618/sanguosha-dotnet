#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCuiYanOlEntries
{
    internal bool AdvanceCuiYanChildren() => _resolve().AdvanceCuiYanChildren();
    internal global::CardGame.Core.PromptChoice AiCuiYanDamage(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiCuiYanDamage(p, f);
    internal void AnswerCuiYanDamage(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerCuiYanDamage(choice);
    internal void AssertCuiYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertCuiYan(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginCuiYan(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginCuiYan(supplied, op);
    internal bool CanOfferCuiYan(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferCuiYan(c, t, context);
    internal bool CaptureCuiYanFinalUse(global::CardGame.Core.CardUseFrame use) => _resolve().CaptureCuiYanFinalUse(use);
    internal void CaptureCuiYanNativeHistory(global::CardGame.Core.IGameEvent payload) => _resolve().CaptureCuiYanNativeHistory(payload);
    internal void CaptureCuiYanUseTargets(global::CardGame.Core.CardUseFrame use) => _resolve().CaptureCuiYanUseTargets(use);
    internal void ConsumeShutongForActualUse(long useId, global::CardGame.Core.CardActionContext? a, int actor, bool response) => _resolve().ConsumeShutongForActualUse(useId, a, actor, response);
    internal void ContinueCuiYan(long id) => _resolve().ContinueCuiYan(id);
    internal void ContinueCuiYanLegacyAlcohol(long id) => _resolve().ContinueCuiYanLegacyAlcohol(id);
    internal bool ContinuePaidCuiYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidCuiYan(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CuiYanDamageChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CuiYanDamageChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> CuiYanDamageTargets(int owner) => _resolve().CuiYanDamageTargets(owner);
    internal bool CuiYanDrawMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().CuiYanDrawMovement(f, m);
    internal bool CuiYanFinalizedAfterCategoryTargetWindow(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().CuiYanFinalizedAfterCategoryTargetWindow(query);
    internal bool CuiYanFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().CuiYanFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? CuiYanObserverRoot() => _resolve().CuiYanObserverRoot();
    internal bool CuiYanTrueResponse(global::CardGame.Core.CardActionContext a, scoped out long parentId) => _resolve().CuiYanTrueResponse(a, out parentId);
    internal bool CuiYanUsedOrPlayed(int seat) => _resolve().CuiYanUsedOrPlayed(seat);
    internal bool CurrentCuiYanEnd(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentCuiYanEnd(c, context);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? CurrentCuiYanUseWindow(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentCuiYanUseWindow(c, context);
    internal void FinishCuiYan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishCuiYan(f);
    internal bool HasCuiYanBeforeDamageObserver(long id) => _resolve().HasCuiYanBeforeDamageObserver(id);
    internal bool HasCuiYanDamageObserver(long id) => _resolve().HasCuiYanDamageObserver(id);
    internal bool IsCuiYanDying() => _resolve().IsCuiYanDying();
    internal bool IsCuiYanLegacyAlcoholUse(long id) => _resolve().IsCuiYanLegacyAlcoholUse(id);
    internal bool IsCuiYanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsCuiYanMovement(f, effect, p);
    internal bool IsCuiYanNativeResponseForbidden(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind required, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().IsCuiYanNativeResponseForbidden(owner, required, cards);
    internal bool IsCuiYanNextUseForbidden(int actorSeat, global::System.Collections.Generic.IReadOnlyList<int> materialIds, global::CardGame.Core.Suit? nativeEffectiveSuit) => _resolve().IsCuiYanNextUseForbidden(actorSeat, materialIds, nativeEffectiveSuit);
    internal bool IsCuiYanOriginalDyingRide() => _resolve().IsCuiYanOriginalDyingRide();
    internal void ObserveCuiYanCardHistory(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveCuiYanCardHistory(payload);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ShutongNextUseGrantedEvent> OutstandingShutong(int seat) => _resolve().OutstandingShutong(seat);
    internal void PublishCuiYanDamage(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishCuiYanDamage(f);
    internal bool ResumeCuiYan(long id) => _resolve().ResumeCuiYan(id);
    internal bool ReturnCuiYanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnCuiYanMovement(f);
    internal bool TryBeginCuiYanLegacyAlcoholUse(long id, long parentId, long dyingId) => _resolve().TryBeginCuiYanLegacyAlcoholUse(id, parentId, dyingId);
    internal bool TryReturnCuiYanLegacyAlcohol(global::CardGame.Core.ProgramCardTriggerWindowFrame w) => _resolve().TryReturnCuiYanLegacyAlcohol(w);
    internal void ValidateCuiYanLegacyAlcohol(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateCuiYanLegacyAlcohol(use);
    internal bool CuiYanActualTurnOpen { get => _resolve().CuiYanActualTurnOpen; }
    internal bool HasCuiYanCapability { get => _resolve().HasCuiYanCapability; }
}
