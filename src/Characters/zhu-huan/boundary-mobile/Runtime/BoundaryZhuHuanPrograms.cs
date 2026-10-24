using System.Text.Json.Serialization;
namespace CardGame.Core;

internal interface IBoundaryZhuHuanHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame); void SkipDraw(ProgramSkillFrame frame); }
internal sealed class SkipBoundaryZhuHuanDrawDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op=>SkillProgramEffectOp.SkipBoundaryZhuHuanDraw;
    public override ISkillProgramEffectHandler Handler {get;}=new BoundaryZhuHuanDrawHandler();
    public override ProgramOperationInteraction Interaction=>ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy=>new(ProgramOperationAiSemantic.InsertPhase,static(_,_)=>{});
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)=>ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect)=>[new RequireTriggerWindow(SkillProgramTriggerWindow.DrawPhaseStarting)];
}
internal sealed class BoundaryZhuHuanDrawHandler:ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>SkillProgramEffectOp.SkipBoundaryZhuHuanDraw;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect,ProgramSkillFrame frame,int target,ISkillProgramEffectHost host)
    {host.GetCapability<IBoundaryZhuHuanHost>().SkipDraw(frame);return SkillProgramStepOutcome.Continue;}
}
internal sealed class ResolveBoundaryZhuHuanPingkouDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryZhuHuanPingkou;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryZhuHuanHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.SelectTarget, static (_,c)=>c.PublicControlValue(4));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)=>ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect)=>[new RequireTriggerWindow(SkillProgramTriggerWindow.AfterTurnEnded)];
}
internal sealed class BoundaryZhuHuanHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>SkillProgramEffectOp.ResolveBoundaryZhuHuanPingkou;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect,ProgramSkillFrame frame,int target,ISkillProgramEffectHost host)=>
        host.GetCapability<IBoundaryZhuHuanHost>().Resolve(frame);
}
internal static class BoundaryZhuHuanComposition
{
    internal const string Fenli="boundary:fenli-mobile",Pingkou="boundary:pingkou-mobile";
    [ProgramCatalogValidation(53800)]
    internal static void Validate(string path,SkillProgram program)
    {
        if(program.Activations.Any(a=>a.Effects.Any(e=>e.Op==SkillProgramEffectOp.SkipBoundaryZhuHuanDraw))||
            program.Triggers.Any(t=>t.Effects.Any(e=>e.Op==SkillProgramEffectOp.SkipBoundaryZhuHuanDraw)&&
                (program.Id!=Fenli||t is not{Id:"skip-draw-most-hand",Window:SkillProgramTriggerWindow.DrawPhaseStarting,Subject:SkillProgramTriggerSubject.Owner,Optional:true,
                    Effects:[{Op:SkillProgramEffectOp.SkipBoundaryZhuHuanDraw}]})))
            throw new InvalidOperationException(path+": mobile Fenli Draw must retain its exact optional owner starting trigger.");
        if(!program.Triggers.SelectMany(t=>t.Effects).Concat(program.Activations.SelectMany(a=>a.Effects)).Any(e=>e.Op==SkillProgramEffectOp.ResolveBoundaryZhuHuanPingkou))return;
        if(program.Id!=Pingkou||program.Activations.Count!=0||program.Triggers is not
            [{Id:"actual-ended-skips",Window:SkillProgramTriggerWindow.AfterTurnEnded,Subject:SkillProgramTriggerSubject.Owner,TurnOwnerScope:SkillProgramTurnOwnerScope.Own,Optional:true,
                Effects:[{Op:SkillProgramEffectOp.ResolveBoundaryZhuHuanPingkou}]}])
            throw new InvalidOperationException(path+": mobile Pingkou requires the original owner's actual completed turn.");
    }
}
public enum BoundaryZhuHuanStage { Targets, Damage, Equipment, Movement, Complete }
public sealed record BoundaryZhuHuanReceipt(CardConversionSource Source,string GameplayHash,int ActualTurn,long EndedFrame,int Maximum,BoundaryZhuHuanStage Stage)
{
    private readonly IReadOnlyList<int> _targets=Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Targets { get=>_targets;init=>_targets=Array.AsReadOnly(value.ToArray()); }
    public bool Sealed {get;init;}
    public int Index {get;init;}
    public int EquipmentCard {get;init;}
    public long Before {get;init;}
    public long After {get;init;}
    public long Batch {get;init;}
}
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)] public BoundaryZhuHuanReceipt? BoundaryZhuHuan {get;init;} }
public sealed record BoundaryZhuHuanPhaseSkippedEvent(int OwnerSeat,int ActualTurn,SkillProgramTurnPhase Phase,long ExtraDrawFrameId):IGameEvent;
public sealed record BoundaryZhuHuanStartedEvent(long FrameId,CardConversionSource Source,string GameplayHash,int ActualTurn,long EndedFrame,int Maximum):IGameEvent;
public sealed record BoundaryZhuHuanTargetEvent(long FrameId,int Index,int Target):IGameEvent;
public sealed record BoundaryZhuHuanSealedEvent(long FrameId,int Count):IGameEvent;
public sealed record BoundaryZhuHuanDamageIssuedEvent(long FrameId,int Index,int Target):IGameEvent;
public sealed record BoundaryZhuHuanDamageReturnedEvent(long FrameId,int Index,int Target):IGameEvent;
public sealed record BoundaryZhuHuanEquipmentObtainedEvent(long FrameId,int OwnerSeat,int CardId,long Before,long After,long BatchId):IGameEvent;
public sealed record BoundaryZhuHuanCompletedEvent(long FrameId):IGameEvent;
