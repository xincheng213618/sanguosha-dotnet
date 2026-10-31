#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYanOlRuntime
{
    internal CardUseFrame? LiuYanTusheUse(int owner, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || context.OwnerSeat != owner || context.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != identity.ParentCardUseFrameId || CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != context.Window || _coreCardUseLifecycleOperations.LifecycleCardUse(identity.ParentCardUseFrameId)is not { Action: { Type: CardActionType.Use } a } use || a.ActionId != identity.CardActionId || a.ActorSeat != owner || use.SourceSeat != owner || a.EffectiveKind != use.CardKind || CardCatalog.Get(use.CardKind).CategoryName == "装备牌" || a.EffectiveDesignatedTargetSeats.Count == 0)
            return null;
        return use;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27230, SkillProgramEffectOp.DrawTusheForActualNonEquipmentTargets)]
    internal bool QualifyLiuYanTushe(CharacterState owner, ProgramSkillWindowContext context) => LiuYanTusheUse(owner.Seat, context)is not null && LiuYanNoBasicHand(owner);
}
