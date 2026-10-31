#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCuiYanOlEntries
{
    internal bool AdvanceCuiYanChildren() => _resolve().AdvanceCuiYanChildren();
    internal PromptChoice AiCuiYanDamage(PendingDecision p, ProgramSkillFrame f) => _resolve().AiCuiYanDamage(p, f);
    internal void AnswerCuiYanDamage(PromptChoice choice) => _resolve().AnswerCuiYanDamage(choice);
    internal SkillProgramStepOutcome BeginCuiYan(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginCuiYan(supplied, op);
    internal bool CanOfferCuiYan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferCuiYan(c, t, context);
    internal bool CaptureCuiYanFinalUse(CardUseFrame use) => _resolve().CaptureCuiYanFinalUse(use);
    internal void CaptureCuiYanNativeHistory(IGameEvent payload) => _resolve().CaptureCuiYanNativeHistory(payload);
    internal void CaptureCuiYanUseTargets(CardUseFrame use) => _resolve().CaptureCuiYanUseTargets(use);
    internal void ConsumeShutongForActualUse(long useId, CardActionContext? a, int actor, bool response) => _resolve().ConsumeShutongForActualUse(useId, a, actor, response);
    internal void ContinueCuiYan(long id) => _resolve().ContinueCuiYan(id);
    internal void ContinueCuiYanLegacyAlcohol(long id) => _resolve().ContinueCuiYanLegacyAlcohol(id);
    internal bool ContinuePaidCuiYan(ProgramSkillFrame f) => _resolve().ContinuePaidCuiYan(f);
    internal IReadOnlyList<PromptChoice> CuiYanDamageChoices(ProgramSkillFrame f) => _resolve().CuiYanDamageChoices(f);
    internal IReadOnlyList<int> CuiYanDamageTargets(int owner) => _resolve().CuiYanDamageTargets(owner);
    internal bool CuiYanDrawMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().CuiYanDrawMovement(f, m);
    internal bool CuiYanFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _resolve().CuiYanFinalizedAfterCategoryTargetWindow(query);
    internal bool CuiYanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CuiYanFirstChild(f, child);
    internal ProgramSkillFrame? CuiYanObserverRoot() => _resolve().CuiYanObserverRoot();
    internal bool CuiYanTrueResponse(CardActionContext a, scoped out long parentId) => _resolve().CuiYanTrueResponse(a, out parentId);
    internal bool CuiYanUsedOrPlayed(int seat) => _resolve().CuiYanUsedOrPlayed(seat);
    internal bool CurrentCuiYanEnd(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentCuiYanEnd(c, context);
    internal ProgramCardTriggerWindowFrame? CurrentCuiYanUseWindow(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentCuiYanUseWindow(c, context);
    internal void FinishCuiYan(ProgramSkillFrame f) => _resolve().FinishCuiYan(f);
    internal bool IsCuiYanLegacyAlcoholUse(long id) => _resolve().IsCuiYanLegacyAlcoholUse(id);
    internal bool IsCuiYanNativeResponseForbidden(CharacterState owner, CardKind required, IReadOnlyList<Card> cards) => _resolve().IsCuiYanNativeResponseForbidden(owner, required, cards);
    internal bool IsCuiYanNextUseForbidden(int actorSeat, IReadOnlyList<int> materialIds, Suit? nativeEffectiveSuit) => _resolve().IsCuiYanNextUseForbidden(actorSeat, materialIds, nativeEffectiveSuit);
    internal void ObserveCuiYanCardHistory(IGameEvent payload) => _resolve().ObserveCuiYanCardHistory(payload);
    internal IEnumerable<ShutongNextUseGrantedEvent> OutstandingShutong(int seat) => _resolve().OutstandingShutong(seat);
    internal void PublishCuiYanDamage(ProgramSkillFrame f) => _resolve().PublishCuiYanDamage(f);
    internal bool ResumeCuiYan(long id) => _resolve().ResumeCuiYan(id);
    internal bool ReturnCuiYanMovement(ProgramSkillFrame f) => _resolve().ReturnCuiYanMovement(f);
    internal bool TryBeginCuiYanLegacyAlcoholUse(long id, long parentId, long dyingId) => _resolve().TryBeginCuiYanLegacyAlcoholUse(id, parentId, dyingId);
    internal bool TryReturnCuiYanLegacyAlcohol(ProgramCardTriggerWindowFrame w) => _resolve().TryReturnCuiYanLegacyAlcohol(w);
    internal void ValidateCuiYanLegacyAlcohol(CardUseFrame use) => _resolve().ValidateCuiYanLegacyAlcohol(use);
    internal bool CuiYanActualTurnOpen { get => _resolve().CuiYanActualTurnOpen; }
    internal bool HasCuiYanCapability { get => _resolve().HasCuiYanCapability; }
}
