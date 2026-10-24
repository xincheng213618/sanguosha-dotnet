namespace CardGame.Core;
public sealed partial class GameEngine
{
    [ProgramCapability] private sealed class BoundaryJianYongHost(GameEngine e):IBoundaryJianYongHost
    {public SkillProgramStepOutcome Resolve(ProgramSkillFrame f,SkillProgramEffectOp op)=>op==SkillProgramEffectOp.ResolveBoundaryJianYongZongshi?e.BeginBoundaryJianYongZongshiChoice(f):e.RecordBoundaryJianYongFailure(f);}
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent,56500)]
    private void TrackBoundaryJianYongGrant(NextActualUseTargetAdjustmentGrantedEvent e)
    {if(e.Source.SkillId==BoundaryJianYongComposition.Qiaoshui)AdvanceEventRulesAndQueueFact(new BoundaryJianYongPhaseEvent(e.ProgramFrameId,e.Source,e.GameplayHash,_turnNumber,_currentSeat,_cardUseDebitPhaseInstanceId,true));}
    private SkillProgramStepOutcome RecordBoundaryJianYongFailure(ProgramSkillFrame f)
    {
        var paid=f.PindianResultBindings.Single(b=>b.Name==SimpleProgramPindianResultBind);
        if(f.SkillId!=BoundaryJianYongComposition.Qiaoshui||f.InstructionIndex!=4||paid.SourceWon||paid.SourceSeat!=f.OwnerSeat||_phase!=TurnPhase.Play||_currentSeat!=f.OwnerSeat)throw new InvalidOperationException("Mobile Qiaoshui needs its exact failed Play-phase Pindian.");
        AdvanceEventRulesAndQueueFact(new BoundaryJianYongPhaseEvent(f.Id,new(f.SkillId,GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId),f.GameplayHash,_turnNumber,_currentSeat,_cardUseDebitPhaseInstanceId,false));return SkillProgramStepOutcome.Continue;
    }
    private bool BoundaryJianYongPhaseCurrent(BoundaryJianYongPhaseEvent e)=>_phase==TurnPhase.Play&&e.Turn==_turnNumber&&e.Actor==_currentSeat&&e.Phase==_cardUseDebitPhaseInstanceId;
    private bool IsBoundaryJianYongAdjustmentCurrent(NextActualUseTargetAdjustmentGrantedEvent grant)=>grant.Source.SkillId!=BoundaryJianYongComposition.Qiaoshui||ProgramEventHistory<BoundaryJianYongPhaseEvent>().Any(e=>e.ProgramFrameId==grant.ProgramFrameId&&e.Source==grant.Source&&e.Hash==grant.GameplayHash&&e.Won&&BoundaryJianYongPhaseCurrent(e));
    private bool BoundaryJianYongAdjustmentConsumes(NextActualUseTargetAdjustmentGrantedEvent grant,CardKind kind)=>grant.Source.SkillId!=BoundaryJianYongComposition.Qiaoshui||GetProgramCardCategory(kind)==SkillProgramCardCategory.Basic||IsOrdinaryTrick(kind);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan,56500)]
    private bool BoundaryJianYongTrickForbidden(ProgramCardUseQuery q)=>GetProgramCardCategory(q.Kind)==SkillProgramCardCategory.Trick&&(q.ActionType==CardActionType.Use||q.Kind==CardKind.Nullification)&&
        ProgramEventHistory<BoundaryJianYongPhaseEvent>().Any(e=>!e.Won&&e.Source.OwnerSeat==q.ActorSeat&&BoundaryJianYongPhaseCurrent(e));
}
