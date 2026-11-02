using System.Text.Json.Serialization;
namespace CardGame.Core;
internal sealed class OfferSidaoCompletedPairSnatchDescriptor:GuoSiDescriptor
{
 public override SkillProgramEffectOp Op=>SkillProgramEffectOp.OfferSidaoCompletedPairSnatch;
 public override ProgramFactRequirements FactRequirements=>new(Triggered:ProgramTrackedFact.ActualCardUseHistory);
 public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
 {
  r.AllowOnly("op","target","targetSetExpr","condition");
  var effect=new SkillProgramEffect(Op,FilterBoundCardsProgramOperationDescriptor.Owner(r),0,r.Condition())
  {TargetSetExpression=r.ParticipantSetExpression("targetSetExpr")??throw new InvalidOperationException(r.Path+": Sidao requires its shared target-set expression.")};
  RequireAlways(effect,r.Path);return effect;
 }
 public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e)=>[new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCompleted)];
}
