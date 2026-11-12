using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum MouHqStage { Cards, Recipient, GiveChildren }
public sealed record MouHqEntity(int CardId,CardLocation From);
public sealed record MouHqReceipt(CardConversionSource Source,string GameplayHash,int Turn,int TurnOwner,long WindowId)
{
    public MouHqStage Stage{get;init;}
    public long UseId{get;init;}
    public long ActionId{get;init;}
    public bool Outgoing{get;init;}
    public bool Paid{get;init;}
    public int Recipient{get;init;}=-1;
    public int Count{get;init;}
    public long Before{get;init;}
    public long After{get;init;}
    private IReadOnlyList<MouHqEntity> _selected=[];
    public IReadOnlyList<MouHqEntity> Selected{get=>_selected;init=>_selected=Array.AsReadOnly(value.ToArray());}
}
public sealed partial record ProgramSkillFrame
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public MouHqReceipt? MouHuangQuan{get;init;}}
public sealed record MouHqEffectGrant(long FrameId,CardConversionSource Source,string GameplayHash,long WindowId,long UseId,long ActionId,int Recipient,int GivenCount);
public sealed record MouHqStartedEvent(long FrameId,CardConversionSource Source,string GameplayHash,int Turn,int TurnOwner,long WindowId,long UseId,long ActionId):IGameEvent;
public sealed record MouHqCommittedEvent(long FrameId,int OwnerSeat,int Recipient,int Count,int Turn,int TurnOwner):IGameEvent;
public sealed record MouHqEffectIssuedEvent(MouHqEffectGrant Grant,bool Repeats):IGameEvent;
public sealed record MouHqCompletedEvent(long FrameId):IGameEvent;
internal interface IMouHuangQuanHost{SkillProgramStepOutcome Resolve(ProgramSkillFrame f,SkillProgramEffectOp op);}
internal sealed class MouHuangQuanHandler(SkillProgramEffectOp op):ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e,ProgramSkillFrame f,int target,ISkillProgramEffectHost host)=>host.GetCapability<IMouHuangQuanHost>().Resolve(f,op);
}
internal abstract class MouHuangQuanDescriptor:ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)=>ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler=>new MouHuangQuanHandler(Op);
    public override ProgramOperationInteraction Interaction=>ProgramOperationInteraction.Choice;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e)=>[];
    public override ProgramOperationAiPolicy AiPolicy{get;}=new(ProgramOperationAiSemantic.GainCards,static(_,c)=>c.PublicControlValue(3));
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c)=>c.State.CanContinueAfterOwnerDeath(c.Frame);
}
internal sealed class ResolveMouHuangQuanQiaoduiDescriptor:MouHuangQuanDescriptor{public override SkillProgramEffectOp Op=>SkillProgramEffectOp.ResolveMouHuangQuanQiaodui;}
