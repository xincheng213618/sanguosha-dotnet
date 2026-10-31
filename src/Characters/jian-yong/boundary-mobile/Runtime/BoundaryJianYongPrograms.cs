using System.Text.Json.Serialization;
namespace CardGame.Core;
internal static class BoundaryJianYongComposition
{
    internal const string Qiaoshui="boundary:qiaoshui-mobile",Zongshi="boundary:zongshih-mobile";
    [ProgramCatalogValidation(56500)] internal static void Validate(string path,SkillProgram p)
    {
        if(p.Id==Qiaoshui && (p.Activations is not [{UsesPerPhase:1,Effects:[{Op:SkillProgramEffectOp.Pindian},{Op:SkillProgramEffectOp.GrantNextActualUseTargetAdjustment},{Op:SkillProgramEffectOp.SetBooleanState},{Op:SkillProgramEffectOp.RecordBoundaryJianYongQiaoshuiFailure}]}] ||
            p.BooleanStates is not [{Id:"failed-this-phase",ResetScope:SkillProgramStateResetScope.PlayPhase}])) throw new InvalidOperationException(path+": mobile Qiaoshui requires a single per-Play native contest and phase result.");
        if(p.Triggers.SelectMany(t=>t.Effects).Any(e=>e.Op==SkillProgramEffectOp.ResolveBoundaryJianYongZongshi) && (p.Id!=Zongshi||p.Triggers is not [{Id:"peek-after-own-pindian",Window:SkillProgramTriggerWindow.PindianResultDetermined,Subject:SkillProgramTriggerSubject.Owner,Optional:false,Effects:[{Op:SkillProgramEffectOp.ResolveBoundaryJianYongZongshi}]}])) throw new InvalidOperationException(path+": mobile Zongshi requires its exact completed own native Pindian.");
    }
}
internal interface IBoundaryJianYongHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame,SkillProgramEffectOp op); }
internal abstract class BoundaryJianYongDescriptor:ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler=>new BoundaryJianYongHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy=>Op==SkillProgramEffectOp.ResolveBoundaryJianYongZongshi?new(ProgramOperationAiSemantic.GainCards,static(_,c)=>c.PublicControlValue(1)):new(ProgramOperationAiSemantic.GrantTurnCardActionProhibition,static(_,c)=>c.PublicControlValue(-1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    { r.AllowOnly("op","target","condition"); var e=new SkillProgramEffect(Op,FilterBoundCardsProgramOperationDescriptor.Owner(r),0,r.Condition());
      if(Op==SkillProgramEffectOp.ResolveBoundaryJianYongZongshi)RequireAlways(e,r.Path);else if(e.Condition.Kind!=SkillProgramConditionKind.PindianNotWon)throw new InvalidOperationException(r.Path+": failure requires the actual not-won native result."); return e; }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e)=>Op==SkillProgramEffectOp.ResolveBoundaryJianYongZongshi?[new RequireTriggerWindow(SkillProgramTriggerWindow.PindianResultDetermined)]:[];
    public override ProgramOperationInteraction Interaction=>Op==SkillProgramEffectOp.ResolveBoundaryJianYongZongshi?ProgramOperationInteraction.Choice:ProgramOperationInteraction.Automatic;
}
internal sealed class RecordBoundaryJianYongQiaoshuiFailureDescriptor:BoundaryJianYongDescriptor{public override SkillProgramEffectOp Op=>SkillProgramEffectOp.RecordBoundaryJianYongQiaoshuiFailure;}
internal sealed class ResolveBoundaryJianYongZongshiDescriptor:BoundaryJianYongDescriptor{public override SkillProgramEffectOp Op=>SkillProgramEffectOp.ResolveBoundaryJianYongZongshi;}
internal sealed class BoundaryJianYongHandler(SkillProgramEffectOp op):ISkillProgramEffectHandler
{public SkillProgramEffectOp Op=>op;public SkillProgramStepOutcome Execute(SkillProgramEffect e,ProgramSkillFrame f,int target,ISkillProgramEffectHost host)=>host.GetCapability<IBoundaryJianYongHost>().Resolve(f,op);}
public sealed record BoundaryJianYongPhaseEvent(long ProgramFrameId,CardConversionSource Source,string Hash,int Turn,int Actor,int Phase,bool Won):IGameEvent;
public sealed record BoundaryJianYongZongshiReceipt(long PindianId,int? TopCardId,int Choice=0,int? PaidCardId=null,CardLocation? From=null,long Before=0,long After=0,int ComparisonIndex=-1);
public sealed partial record ProgramSkillFrame{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public BoundaryJianYongZongshiReceipt? BoundaryJianYongZongshi{get;init;}}
public sealed record BoundaryJianYongZongshiContestReceipt(long ParentFrameId,int ComparisonIndex,PindianResult Result)
{
    private readonly IReadOnlyList<ProgramTriggerCandidate> _candidates=[];
    public IReadOnlyList<ProgramTriggerCandidate> Candidates{get=>_candidates;init=>_candidates=Array.AsReadOnly(value.ToArray());}
    public int Cursor{get;init;}
}
public abstract partial record ResolutionFrame
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public BoundaryJianYongZongshiContestReceipt? BoundaryJianYongZongshiContest{get;init;}}
public sealed record BoundaryJianYongZongshiPaidEvent(long ProgramFrameId,long PindianId,int OwnerSeat,CardZoneKind FromZone,long Before,long After,int ComparisonIndex=-1):IGameEvent;
