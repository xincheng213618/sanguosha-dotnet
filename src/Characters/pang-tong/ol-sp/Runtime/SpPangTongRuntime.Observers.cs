using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramLifecycle;
using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards;
using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver;
namespace CardGame.Core;
internal sealed partial class SpPangTongRuntime
{
 [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 290)]
 [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 290)]
 private bool CanContinueSpPangTong(ProgramSkillFrame f)
 {if(f.SpPangTong is not{Stage:"exchange-children" or "draw-children" or "health-children" or "grant-children"})return false;return true;}
 private bool SpPangTongFirstChild(ProgramSkillFrame f,ResolutionFrame child)
 {
  if(!CanContinueSpPangTong(f)||f.SpPangTong is not{}r)return false;
  if(child is CardsMovedTriggerWindowFrame moved)
  {
   var b=moved.Batch;if(b.ParentFrameId!=f.Id||b.AwaitingProgramFrameId!=f.Id||moved.ResumeProgramFrameId is not null||b.OriginOwnerSeat!=f.OwnerSeat||b.OriginSkillId!=f.SkillId||b.OriginSkillInstanceId!=f.SkillInstanceId||b.Movements.Count==0)return false;
   bool Grain(CardMovementRecord m)=>r.Stage=="exchange-children"&&r.BatchId is{}paid&&b.ParentBatchId==paid&&r.OwnCard is{}id&&Card(id).Kind==CardKind.WoodenOx&&r.OwnFrom==CardLocation.Equipment(f.OwnerSeat)&&m.Sequence>r.Before&&m.From==CardLocation.WoodenOxGrain(f.OwnerSeat)&&m.To==CardLocation.DiscardPile&&m.Reason==CardMoveReasons.WoodenOxGrainDiscard;
   return b.Movements.All(m=>movements.Read().Contains(m)&&(r.Stage=="draw-children"&&m.Sequence>r.Before&&m.Sequence<=r.After&&m.From==CardLocation.DrawPile&&m.To==CardLocation.Hand(r.RewardSeat!.Value)&&m.Reason.Value==SpPangTongDrawReason||r.Stage=="exchange-children"&&b.Id==r.BatchId&&m.Sequence>r.Before&&m.Sequence<=r.After&&m.Reason.Value==GuolunExchangeReason&&(m.CardId==r.OwnCard&&m.From==r.OwnFrom&&m.To==facts.NormalizeEphemeralDestination(Card(r.OwnCard!.Value),r.OwnFrom!.Value,CardLocation.Hand(r.Target))||m.CardId==r.RevealedCard&&m.From==CardLocation.Hand(r.Target)&&m.To==CardLocation.Hand(f.OwnerSeat))||Grain(m)));
  }
  if(child is ProgramLifecycleTriggerWindowFrame skills&&skills.Window==SkillProgramTriggerWindow.SkillsChanged)return skills.ResumeProgramFrameId==f.Id&&skills.Continuation==ProgramLifecycleContinuation.ResumeParentProgram;
  var silver=r.Stage=="exchange-children"&&r.OwnFrom==CardLocation.Equipment(f.OwnerSeat)&&r.OwnCard is{}own&&Card(own).Kind==CardKind.SilverLion;
  var health=r.Stage=="health-children"&&r.Wounded;
  if(child is HpChangedTriggerWindowFrame hp)return (silver||r.Stage=="health-children")&&hp.Change.ParentFrameId==f.Id&&hp.ResumeFrameId==f.Id&&hp.Continuation==PostEventContinuation.AwaitedProgramMovement&&hp.Change.SourceSeat==f.OwnerSeat&&hp.Change.TargetSeat==f.OwnerSeat&&(hp.Change.Kind==HpChangeKind.Recovery&&hp.Change.Amount==1&&(silver||health)||!r.Wounded&&hp.Change.Kind==HpChangeKind.MaximumHp&&hp.Change.Amount==1);
  return (silver||health)&&child is RecoveryReplacementFrame recovery&&facts.RecoveryRidesOn(recovery,f)&&recovery.Return.Continuation==PostEventContinuation.AwaitedProgramMovement&&recovery.Attempt.SourceSeat==f.OwnerSeat&&recovery.Attempt.TargetSeat==f.OwnerSeat&&recovery.Attempt.Amount==1&&(silver?recovery.Attempt.Completion.Producer==RecoveryAttemptProducer.SilverLion&&recovery.Attempt.Completion.MoveReason?.Value==GuolunExchangeReason:recovery.Attempt.Completion.Producer==RecoveryAttemptProducer.Program&&recovery.Attempt.Completion.InstructionIndex==r.Instruction);
 }
 private ProgramSkillFrame? SpPangTongObserverRoot()
 {
  for(var index=0;index+1<nativeFrames.FrameCount;index++)
   if(nativeFrames.FrameAt(index) is ProgramSkillFrame root&&SpPangTongFirstChild(root,nativeFrames.FrameAt(index+1))&&facts.SameNameObserverSuffix(root.Id))return root;
  return null;
 }
 [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 360)]
 private bool TryAdvanceSpPangTongSubtree()=>prompts.Current is null&&SpPangTongObserverRoot() is not null&&childOperations.AdvanceObserverSubtree();
}
