namespace CardGame.Core;
public sealed record WenjiGiftPaidEvent(long FrameId,CardConversionSource Source,string GameplayHash,int ActualTurn,int RecipientSeat,int CardId,CardLocation From,CardKind Name,long Before,long After):IGameEvent;
internal sealed class ReceiveWenjiGiftAndArmTurnNameDescriptor:LiuQiDescriptor
{
 public override SkillProgramEffectOp Op=>SkillProgramEffectOp.ReceiveWenjiGiftAndArmTurnName;
 public override ISkillProgramEffectHandler Handler{get;}=new LiuQiHandler(SkillProgramEffectOp.ReceiveWenjiGiftAndArmTurnName);
 public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e)=>[new RequireTriggerWindow(SkillProgramTriggerWindow.PlayPhaseStarting)];
}
