namespace CardGame.Core;
public enum LiuQiStage { ChooseRecipient, ChooseGift, GiftChildren }
public sealed record LiuQiReceipt(CardConversionSource Source,string GameplayHash,int InstructionIndex,int ActualTurn,long WindowId,LiuQiStage Stage,int? RecipientSeat=null,int? CardId=null,CardLocation? From=null,CardKind? Name=null,long Before=0,long After=0);
internal interface ILiuQiProgramHost {SkillProgramStepOutcome BeginLiuQi(ProgramSkillFrame frame,SkillProgramEffect effect);}
internal abstract class LiuQiDescriptor:ProgramOperationDescriptorBase
{
 public override ProgramOperationInteraction Interaction=>ProgramOperationInteraction.Choice;
 public override ProgramOperationAiPolicy AiPolicy{get;}=new(ProgramOperationAiSemantic.GainCards,static(_,_)=>{});
 public override SkillProgramEffect Parse(ProgramOperationNodeReader r)=>ParseOwnerAlways(r);
}
internal sealed class LiuQiHandler(SkillProgramEffectOp op):ISkillProgramEffectHandler
{
 public SkillProgramEffectOp Op=>op;
 public SkillProgramStepOutcome Execute(SkillProgramEffect e,ProgramSkillFrame f,int s,ISkillProgramEffectHost h)=>h.GetCapability<ILiuQiProgramHost>().BeginLiuQi(f,e);
}
internal static class LiuQiComposition
{
 internal static bool IsOp(SkillProgramEffectOp op)=>op == SkillProgramEffectOp.ReceiveWenjiGiftAndArmTurnName;
 internal static void Activation(string path,SkillProgramActivation a){if(a.Effects.Any(e=>IsOp(e.Op)))throw new InvalidOperationException(path+": LiuQi requires actual own native phase triggers.");}
 internal static void Trigger(string path,SkillProgramTrigger t)
 {
  if(!t.Effects.Any(e=>IsOp(e.Op)))return;
  if(t.Effects is not[var e]||e.Condition.Kind!=SkillProgramConditionKind.Always||t.TurnOwnerScope!=SkillProgramTurnOwnerScope.Own||!t.Optional||t.Window!=SkillProgramTriggerWindow.PlayPhaseStarting)throw new InvalidOperationException(path+": Wenji requires one unconditional optional own PlayStart effect.");
 }
}

public sealed partial record ProgramSkillFrame
{
 [System.Text.Json.Serialization.JsonIgnore(Condition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] public LiuQiReceipt? LiuQi {get;init;}
}
public sealed partial class CharacterState
{
 private IReadOnlyList<WenjiTurnNameGrant>? _wenjiTurnNameGrants;
 internal IReadOnlyList<WenjiTurnNameGrant>? WenjiTurnNameGrants {get=>_wenjiTurnNameGrants;set=>_wenjiTurnNameGrants=value is null?null:Array.AsReadOnly(value.ToArray());}
}
public sealed partial record NullificationWindowFrame
{
 [System.Text.Json.Serialization.JsonIgnore(Condition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)] public WenjiCounterspellQualification? WenjiCounterspell {get;init;}
}
