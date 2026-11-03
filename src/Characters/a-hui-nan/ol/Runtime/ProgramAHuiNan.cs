using System.Text.Json.Serialization;
namespace CardGame.Core;

public sealed record AHuiNanBasicUseSnapshot(int ActorSeat,CardKind Kind);
public enum AHuiNanJuemanStage { UseChoice = 2,NativeUse,NativeChildren }
public sealed record AHuiNanJuemanUseOrigin(long ProgramFrameId,long CardUseFrameId,CardConversionSource Source,string GameplayHash,int ActualTurn,int ActorSeat,CardKind CardKind,string TargetSignature);
public sealed record AHuiNanJuemanReceipt(CardConversionSource Source,string GameplayHash,int InstructionIndex,int ActualTurn,int TurnOwner,AHuiNanJuemanStage Stage)
{
    public long Before {get;init;}
    public long After {get;init;}
    [JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public AHuiNanJuemanUseOrigin? Use {get;init;}
    private IReadOnlyList<AHuiNanBasicUseSnapshot> _history=Array.AsReadOnly(Array.Empty<AHuiNanBasicUseSnapshot>());
    public IReadOnlyList<AHuiNanBasicUseSnapshot> History {get=>_history;init=>_history=Array.AsReadOnly(value.ToArray());}
}
public sealed partial record ProgramSkillFrame
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public AHuiNanJuemanReceipt? AHuiNanJueman {get;init;}}
public sealed partial record CardUseFrame
{[JsonIgnore(Condition=JsonIgnoreCondition.WhenWritingNull)]public AHuiNanJuemanUseOrigin? AHuiNanJueman {get;init;}}
public sealed record AHuiNanJuemanStartedEvent(long FrameId,CardConversionSource Source,string GameplayHash,int ActualTurn,int TurnOwner,int BasicCount,int FirstTwoOwnerUses,CardKind? ThirdKind):IGameEvent;
public sealed record AHuiNanJuemanUseIssuedEvent(AHuiNanJuemanUseOrigin Origin):IGameEvent;
public sealed record AHuiNanJuemanUseReturnedEvent(AHuiNanJuemanUseOrigin Origin):IGameEvent;
public sealed record AHuiNanJuemanCompletedEvent(long FrameId,bool Used,string Outcome):IGameEvent;
internal interface IAHuiNanHost {SkillProgramStepOutcome Resolve(ProgramSkillFrame frame);}
internal sealed class JuemanThirdBasicUseDescriptor:ProgramOperationDescriptorBase
{
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.ActualCardUseHistory);
    public override SkillProgramEffectOp Op=>SkillProgramEffectOp.ResolveJuemanFirstBasicTurnEnd;
    public override ProgramOperationInteraction Interaction=>ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy{get;}=new(ProgramOperationAiSemantic.GainCards,static(_,c)=>c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)=>ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e)=>[new RequireTriggerWindow(SkillProgramTriggerWindow.AfterTurnEnded)];
    public override ISkillProgramEffectHandler Handler=>new AHuiNanHandler();
}
internal sealed class AHuiNanHandler:ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op=>SkillProgramEffectOp.ResolveJuemanFirstBasicTurnEnd;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e,ProgramSkillFrame f,int target,ISkillProgramEffectHost host)=>host.GetCapability<IAHuiNanHost>().Resolve(f);
}
internal static class AHuiNanComposition
{
    [ProgramCatalogValidation(420)]
    internal static void ValidateProgram(string path,SkillProgram p)
    {
        static bool Has(SkillProgramEffect e)=>e.Op==SkillProgramEffectOp.ResolveJuemanFirstBasicTurnEnd;
        if(!p.Activations.Any(a=>a.Effects.Any(Has))&&!p.Triggers.Any(t=>t.Effects.Any(Has)))return;
        if(p.Id!="ol:jueman"||p.Activations.Any(a=>a.Effects.Any(Has))||p.Triggers.Where(t=>t.Effects.Any(Has)).Any(t=>
            t.Id!="actual-end-third-basic"||t.Window!=SkillProgramTriggerWindow.AfterTurnEnded||t.Subject!=SkillProgramTriggerSubject.Owner||t.Optional||
            t.Effects is not[{Op:SkillProgramEffectOp.ResolveJuemanFirstBasicTurnEnd,Target:SkillProgramEffectTarget.Owner,Condition.Kind:SkillProgramConditionKind.Always}]))
            throw new InvalidOperationException(path+": Jueman's native third-basic use requires one mandatory owner instruction at the actual-turn-end window.");
    }
}
