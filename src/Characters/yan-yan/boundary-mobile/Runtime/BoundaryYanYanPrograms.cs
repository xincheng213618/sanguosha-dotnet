using System.Text.Json.Serialization;
namespace CardGame.Core;

internal interface IBoundaryYanYanHost {SkillProgramStepOutcome Execute(ProgramSkillFrame frame,SkillProgramEffectOp op);}
internal abstract class BoundaryYanYanDescriptor:ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler=>new BoundaryYanYanHandler(Op);
    public override bool UsesConversionPolarity=>true;
    public override ProgramContextCapability RequiredCapabilities=>ProgramContextCapability.CardAction|ProgramContextCapability.TurnEffects;
    public override ProgramOperationAiPolicy AiPolicy=>new(ProgramOperationAiSemantic.GainCards,static(_,c)=>c.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)=>ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect)=>[new RequireTriggerWindows(Op==SkillProgramEffectOp.ClaimBoundaryYanYanChengshi?
        [SkillProgramTriggerWindow.CardUseCompleted]:[SkillProgramTriggerWindow.CardUseTargetsFinalized,SkillProgramTriggerWindow.CardUseBeforeTargetEffects])];
}
internal sealed class PrepareBoundaryYanYanJuzhanDescriptor:BoundaryYanYanDescriptor {public override SkillProgramEffectOp Op=>SkillProgramEffectOp.PrepareBoundaryYanYanJuzhan;}
internal sealed class FinishBoundaryYanYanJuzhanDescriptor:BoundaryYanYanDescriptor {public override SkillProgramEffectOp Op=>SkillProgramEffectOp.FinishBoundaryYanYanJuzhan;}
internal sealed class ClaimBoundaryYanYanChengshiDescriptor:BoundaryYanYanDescriptor {public override SkillProgramEffectOp Op=>SkillProgramEffectOp.ClaimBoundaryYanYanChengshi;}
internal sealed class BoundaryYanYanHandler(SkillProgramEffectOp op):ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect,ProgramSkillFrame frame,int target,ISkillProgramEffectHost host)=>host.GetCapability<IBoundaryYanYanHost>().Execute(frame,op);
}
internal static class BoundaryYanYanComposition
{
    internal const string Skill="boundary:juzhan-mobile";
    [ProgramCatalogValidation(54300)]
    internal static void Validate(string path,SkillProgram program)
    {
        var own=program.Triggers.SelectMany(t=>t.Effects).Concat(program.Activations.SelectMany(a=>a.Effects)).Any(e=>e.Op is SkillProgramEffectOp.PrepareBoundaryYanYanJuzhan or SkillProgramEffectOp.FinishBoundaryYanYanJuzhan or SkillProgramEffectOp.ClaimBoundaryYanYanChengshi);
        if(!own)return;
        if(program.Id!=Skill||program.Activations.Count!=0||program.Triggers.Count!=3)throw new InvalidOperationException(path+": mobile Juzhan requires its own two actual Slash windows and issued completed-use claim.");
        foreach(var trigger in program.Triggers)
        {
            var valid=trigger.Id switch
            {
                "yin-designated-use"=>trigger.Window==SkillProgramTriggerWindow.CardUseTargetsFinalized&&trigger.RequiredPolarity==SkillPolarity.Yin&&trigger.Optional&&trigger.Effects is
                    [{Op:SkillProgramEffectOp.PrepareBoundaryYanYanJuzhan},{Op:SkillProgramEffectOp.CommitConversionPolarity},{Op:SkillProgramEffectOp.SelectTarget,TargetKind:SkillProgramTargetKind.CurrentCardUseTargets},
                    {Op:SkillProgramEffectOp.SelectAndMoveOwnedCard,AwaitMovementTriggers:true},{Op:SkillProgramEffectOp.FinishBoundaryYanYanJuzhan}],
                "yang-became-target"=>trigger.Window==SkillProgramTriggerWindow.CardUseBeforeTargetEffects&&trigger.RequiredPolarity==SkillPolarity.Yang&&trigger.Optional&&trigger.Effects is
                    [{Op:SkillProgramEffectOp.PrepareBoundaryYanYanJuzhan},{Op:SkillProgramEffectOp.CommitConversionPolarity},{Op:SkillProgramEffectOp.Draw,Target:SkillProgramEffectTarget.Owner,Amount:1},
                    {Op:SkillProgramEffectOp.Draw,Target:SkillProgramEffectTarget.Actor,Amount:1},{Op:SkillProgramEffectOp.FinishBoundaryYanYanJuzhan}],
                "chengshi-completed-use"=>trigger.Window==SkillProgramTriggerWindow.CardUseCompleted&&!trigger.Optional&&trigger.Effects is[{Op:SkillProgramEffectOp.ClaimBoundaryYanYanChengshi}],
                _=>false
            };
            if(!valid||trigger.OwnerRelation!=SkillProgramCardActionOwnerRelation.Observer)throw new InvalidOperationException(path+": Juzhan's state/whole-use/actual-target boundaries cannot be interchanged.");
        }
    }
}
public sealed record BoundaryYanYanOccurrence(int OwnerSeat,string SkillInstanceId,string GameplayHash,long Epoch,SkillPolarity Polarity,long UseId,long ActionId,int ActorSeat,int TargetSeat,int Ordinal);
public sealed record BoundaryYanYanOccurrenceRecordedEvent(BoundaryYanYanOccurrence Occurrence):IGameEvent;
public sealed record BoundaryYanYanPreparedEvent(long FrameId,long WindowId,BoundaryYanYanOccurrence Occurrence):IGameEvent;
public sealed record BoundaryYanYanChengshiGrantedEvent(long FrameId,int OwnerSeat,string SkillInstanceId,long UseId,long ActionId):IGameEvent;
public sealed record BoundaryYanYanFinishedEvent(long FrameId,int ActorSeat,int TargetSeat,bool Chengshi):IGameEvent;
public sealed record BoundaryYanYanClaimPaidEvent(long FrameId,long UseId,int OwnerSeat,int Count,long Before,long After,long Batch):IGameEvent;
public sealed record BoundaryYanYanDraft(BoundaryYanYanOccurrence Occurrence,long WindowId);
public sealed record BoundaryYanYanClaim(long ProducerFrameId,CardConversionSource Source,string GameplayHash,long UseId,long ActionId)
{
    public bool Settled {get;init;}
    private readonly IReadOnlyList<int> _cards=Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Cards{get=>_cards;init=>_cards=Array.AsReadOnly(value.ToArray());}
    public long Before{get;init;}public long After{get;init;}public long Batch{get;init;}
}
public sealed record BoundaryYanYanClaimDraft(long UseId,long ProducerFrameId,long Before)
{
    private readonly IReadOnlyList<int> _cards=Array.AsReadOnly(Array.Empty<int>());
    private readonly IReadOnlyList<CardLocation> _from=Array.AsReadOnly(Array.Empty<CardLocation>());
    public IReadOnlyList<int> Cards{get=>_cards;init=>_cards=Array.AsReadOnly(value.ToArray());}
    public IReadOnlyList<CardLocation> From{get=>_from;init=>_from=Array.AsReadOnly(value.ToArray());}
    public long After{get;init;}public long Batch{get;init;}
}
public sealed partial record ProgramSkillWindowContext
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public BoundaryYanYanOccurrence? BoundaryYanYanOccurrence{get;init;}}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public BoundaryYanYanDraft? BoundaryYanYan{get;init;}
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public BoundaryYanYanClaimDraft? BoundaryYanYanClaim{get;init;}
}
public sealed partial record CardUseFrame
{
    private readonly IReadOnlyList<BoundaryYanYanClaim>? _boundaryYanYanClaims;
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public IReadOnlyList<BoundaryYanYanClaim>? BoundaryYanYanClaims
    {get=>_boundaryYanYanClaims;init=>_boundaryYanYanClaims=value is null?null:Array.AsReadOnly(value.ToArray());}
}
