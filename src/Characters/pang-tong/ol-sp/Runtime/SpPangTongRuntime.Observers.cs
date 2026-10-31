using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramLifecycle;
using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards;
using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver;
namespace CardGame.Core;
internal sealed partial class SpPangTongRuntime
{
 private bool SpPangTongPaidMovementMatches(ProgramSkillFrame f,SpPangTongReceipt r)
 {
  var moves=movements.Read().Where(m=>m.Sequence>r.Before&&m.Sequence<=r.After).ToArray();
  if(r.Stage=="draw-children")return r.RewardSeat is{}seat&&ValidSeat(seat)&&r.ActualCount is >=0 and <=1&&moves.Count(m=>m.From==CardLocation.DrawPile&&m.To==CardLocation.Hand(seat)&&m.Reason.Value==SpPangTongDrawReason)==r.ActualCount;
  return r.Stage=="exchange-children"&&r.ActualCount==2&&r.BatchId is>0&&r.RevealedCard is{}foreign&&r.OwnCard is{}own&&own!=foreign&&r.OwnFrom is{}from&&from.OwnerSeat==f.OwnerSeat&&from.Zone is(CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.WoodenOxGrain)&&(r.OwnRank is >=1 and <=13||r.OwnRank==0&&facts.IsDiouCarrier(own))&&r.TargetRank is >=1 and <=13&&r.RewardSeat==(r.OwnRank<r.TargetRank?f.OwnerSeat:r.TargetRank<r.OwnRank?r.Target:null)&&moves.Count(m=>m.Reason.Value==GuolunExchangeReason)==2&&moves.Count(m=>m.CardId==own&&m.From==from&&m.To==facts.NormalizeEphemeralDestination(Card(own),from,CardLocation.Hand(r.Target))&&m.Reason.Value==GuolunExchangeReason)==1&&moves.Count(m=>m.CardId==foreign&&m.From==CardLocation.Hand(r.Target)&&m.To==CardLocation.Hand(f.OwnerSeat)&&m.Reason.Value==GuolunExchangeReason)==1;
 }
 [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 330)]
 private void AssertSpPangTong(ProgramSkillFrame f)
 {
  if(f.SpPangTong is not{}r)return;
  var effect=PausedEffect(f);
  if(r.Source!=SpPangTongSource(f)||r.GameplayHash!=f.GameplayHash||r.Instruction!=f.InstructionIndex||r.Turn!=turn.TurnNumber||r.ParentId!=f.WindowContext?.ParentFrameId||effect is null||!SpPangTongComposition.Own(effect.Op)||r.Before>r.After||r.After>movement.Sequence||r.Stage is not("reveal" or "exchange" or "exchange-children" or "draw-children" or "health-children" or "grant-children")||history.Read<SpPangTongStartedEvent>().Count(e=>e.FrameId==f.Id&&e.Source==r.Source&&e.GameplayHash==r.GameplayHash&&e.Instruction==r.Instruction&&e.Turn==r.Turn&&e.ParentId==r.ParentId&&e.Operation==effect.Op)!=1)throw new InvalidOperationException("SP Pang Tong lost its original source, instruction, actual window or owning receipt.");
  if(effect.Op==SkillProgramEffectOp.ResolveGuolunRevealOptionalAtomicExchange)
  {
   if(f.TriggerId is not null||f.SelectedTargetSeats is not[var target]||target!=r.Target||target==f.OwnerSeat||!ValidSeat(target)||r.OriginalHand.Count==0||r.OriginalHand.Distinct().Count()!=r.OriginalHand.Count)throw new InvalidOperationException("Guolun has no original actual selected hand owner.");
   if(r.Stage!="reveal"&&(r.RevealedCard is not{}id||!r.OriginalHand.Contains(id)||history.Read<GuolunRevealedEvent>().Count(e=>e.FrameId==f.Id&&e.Source==r.Source&&e.TargetSeat==r.Target&&e.CardId==id)!=1))throw new InvalidOperationException("Guolun lost the single actual public reveal.");
  }
  if(effect.Op==SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation)
  {
   if(f.WindowContext?.MovementBatch is not{NativeDrawInvocation:{}draw}batch||r.DrawInvocation!=draw.InvocationId||draw.OwnerSeat!=f.OwnerSeat||draw.ParentFrameId!=batch.ParentFrameId||draw.LastBatchId!=batch.Id||draw.ActualTurnNumber!=r.Turn||draw.ActualTurnOwnerSeat!=f.OwnerSeat||draw.PhaseActorSeat!=f.OwnerSeat||draw.Phase?.Kind!=ActualDiscardRecoveryPhaseKind.Play||!drawInvoices.IsValid(draw)||!ZhanjiDrawEntitlementRuntime.HasEntitlement(draw,f.OwnerSeat,f.SkillId,GetProgramBindingId(f),f.SkillInstanceId,f.GameplayHash)||draw.DirectProducer is{}producer&&producer.OwnerSeat==f.OwnerSeat&&producer.SkillId==f.SkillId||frames.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w=>w.Id==r.ParentId) is not{}parent||parent.Batch.Id!=batch.Id||parent.CandidateIndex<0||parent.CandidateIndex>=parent.Candidates.Count||!MountObserverCandidateMatches(f,parent.Candidates[parent.CandidateIndex]))throw new InvalidOperationException("Zhanji cannot borrow a gift or another draw invocation.");
  }
  if(r.Stage is "exchange-children" or "draw-children")
  {
   if(f.PendingMovementContinuation is not{SubjectSeat:var subject,BeforeCount:0,CoverageResultBind:null}||subject!=f.OwnerSeat||!SpPangTongPaidMovementMatches(f,r)||history.Read<SpPangTongSegmentPaidEvent>().Count(e=>e.FrameId==f.Id&&e.Source==r.Source&&e.GameplayHash==r.GameplayHash&&e.Instruction==r.Instruction&&e.Stage==r.Stage&&e.Before==r.Before&&e.After==r.After&&e.Count==r.ActualCount&&e.TargetSeat==(r.Stage=="draw-children"?r.RewardSeat:r.Target)&&e.DrawInvocation==r.DrawInvocation)!=1)throw new InvalidOperationException("SP Pang Tong needs one exact already-paid native movement invoice.");
  }
  if(r.Stage is "health-children" or "grant-children")
  {
   if(effect.Op!=SkillProgramEffectOp.ResolveSongsangLimitedOtherDeath||f.WindowContext is not{Window:SkillProgramTriggerWindow.CharacterDied,TargetSeat:{}victim}||victim!=r.Target||victim==f.OwnerSeat||participants.Players[victim].IsAlive||r.Wounded!=(r.RuleWounded??(r.HpBefore<r.MaximumBefore))||history.Read<SongsangHealthIssuedEvent>().Count(e=>e.FrameId==f.Id&&e.Source==r.Source&&e.GameplayHash==r.GameplayHash&&e.VictimSeat==r.Target&&e.Wounded==r.Wounded&&e.HpBefore==r.HpBefore&&e.MaximumBefore==r.MaximumBefore&&e.RuleWounded==r.RuleWounded)!=1)throw new InvalidOperationException("Songsang lost its actual other death and frozen native health branch.");
  }
  if(frames.Top?.Id==f.Id&&prompts.Current is{}pending&&r.Stage is "reveal" or "exchange"&&(pending.PlayerSeat!=f.OwnerSeat||!pending.IsPrivate||!AssistedChoicesEqual(pending.Choices,SpPangTongChoices(f))))throw new InvalidOperationException("Guolun exposes a foreign private decision.");
 }
 [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 290)]
 [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 290)]
 private bool CanContinueSpPangTong(ProgramSkillFrame f)
 {if(f.SpPangTong is not{Stage:"exchange-children" or "draw-children" or "health-children" or "grant-children"})return false;AssertSpPangTong(f);return true;}
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
 private int FrameIndex(long id)=>nativeFrames.LastFrameIndex(id);
 [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 360)]
 private bool TryAdvanceSpPangTongSubtree()=>prompts.Current is null&&SpPangTongObserverRoot() is not null&&childOperations.AdvanceObserverSubtree();
 [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 250)]
 [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 360)]
 private bool HasSpPangTongDamageObserver(long id)=>SpPangTongObserverRoot() is{}root&&frames.OfType<ResolutionFrame>().Any(f=>f.Id==id&&f is DamageTriggerWindowFrame)&&FrameIndex(id)>FrameIndex(root.Id);
 [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 450)]
 private bool HasSpPangTongBeforeDamageObserver(long id)=>SpPangTongObserverRoot() is{}root&&frames.OfType<ResolutionFrame>().Any(f=>f.Id==id&&f is BeforeDamageProgramWindowFrame)&&FrameIndex(id)>FrameIndex(root.Id);
 [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 220)]
 private bool IsSpPangTongDying()=>healthChildren.ActiveDying is{}d&&SpPangTongObserverRoot() is{}root&&FrameIndex(d.Id)>FrameIndex(root.Id);
}
