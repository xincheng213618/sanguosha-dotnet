using System.Text.Json.Serialization;
namespace CardGame.Core;

internal interface IBoundaryAnjianHost {void Issue(ProgramSkillFrame frame);}
internal sealed class IssueBoundaryAnjianChoiceDescriptor:ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op=>SkillProgramEffectOp.IssueBoundaryAnjianChoice;
    public override ISkillProgramEffectHandler Handler{get;}=new BoundaryAnjianHandler();
    public override ProgramOperationInteraction Interaction=>ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy=>new(ProgramOperationAiSemantic.ProhibitCurrentResponse,static(_,c)=>c.PublicControlValue(2));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)=>ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect)=>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseTargetsFinalized),new RequireCardActionActor(),
         new RequireCardActionRelation(SkillProgramCardActionOwnerRelation.Actor,[CardKind.Slash,CardKind.FireSlash,CardKind.ThunderSlash]),new RequireChoiceOptions("anjian-choice",["no-response","damage"])];
}
internal sealed class BoundaryAnjianHandler:ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>SkillProgramEffectOp.IssueBoundaryAnjianChoice;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect,ProgramSkillFrame frame,int target,ISkillProgramEffectHost host)
    {host.GetCapability<IBoundaryAnjianHost>().Issue(frame);return SkillProgramStepOutcome.Continue;}
}
internal static class BoundaryPanZhangMaZhongComposition
{
    internal const string Duodao="boundary:duodao-mobile",Anjian="boundary:anjian-mobile";
    [ProgramCatalogValidation(54000)]
    internal static void Validate(string path,SkillProgram program)
    {
        if(!program.Triggers.SelectMany(t=>t.Effects).Concat(program.Activations.SelectMany(a=>a.Effects)).Any(e=>e.Op==SkillProgramEffectOp.IssueBoundaryAnjianChoice))return;
        if(program.Id!=Anjian||program.Activations.Count!=0||program.Triggers is not
            [{Id:"outside-target-range-choice",Window:SkillProgramTriggerWindow.CardUseTargetsFinalized,OwnerRelation:SkillProgramCardActionOwnerRelation.Actor,Optional:false,
                Effects:[{Op:SkillProgramEffectOp.ChooseOption,Target:SkillProgramEffectTarget.Owner,ResultBind:"anjian-choice"}choice,{Op:SkillProgramEffectOp.IssueBoundaryAnjianChoice}]}]||
            !choice.Options.Select(o=>o.Id).SequenceEqual(["no-response","damage"]))
            throw new InvalidOperationException(path+": mobile Anjian requires a mandatory per-target Slash choice on its exact original actor use.");
    }
}
public sealed record BoundaryAnjianReceipt(long CardUseFrameId,long ActionId,long ProducerFrameId,CardConversionSource Source,string GameplayHash,int TargetSeat,string Option);
public sealed record BoundaryAnjianChoiceIssuedEvent(BoundaryAnjianReceipt Receipt):IGameEvent;
public sealed partial record CardUseFrame
{
    private readonly IReadOnlyList<BoundaryAnjianReceipt>? _boundaryAnjianReceipts;
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<BoundaryAnjianReceipt>? BoundaryAnjianReceipts
    {get=>_boundaryAnjianReceipts;init=>_boundaryAnjianReceipts=value is null?null:Array.AsReadOnly(value.ToArray());}
}
