using System.Globalization;
using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramLifecycle;
using static CardGame.Core.RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards;
namespace CardGame.Core;
// One lazy module per match. The original frame remains the sole owner of all
// selections, paid native segments and their return state.
[ProgramRuntimeModule,ProgramCapability]
internal sealed partial class SpPangTongRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
 IProgramRuntimePrompts prompts, INativeFrameRoutingFacts nativeFrames, IMatchParticipantStateAccess participants, ICardZoneQueries zones,
 IProgramContentQueries content, IProgramTurnQueries turn, IProgramEventHistoryQueries history,
 ICardMovementHistoryQueries movements, IProgramBoundCardMovementOperations movement,
 IProgramBoundCardFacts boundCards, IProgramHealthStateFacts skills, IPhysicalCardDrawOperations draws,
 INativeDrawInvoiceQueries drawInvoices, IProgramCardSnapshotQueries snapshots,
 IAtomicCardMovementZones physicalZones, IAtomicCardMovementBatches batches, IAtomicCardMovementEffects effects,
 IProgramHealthChildQueries healthChildren, IProgramHealthChildOperations childOperations,
 IProgramSkillCompletion completion, IProgramChildWindows children,
 IProgramSkillAcquisitions acquisitions, ISpPangTongNativeFacts facts, ISpPangTongNativeEntries native) : ISpPangTongProgramHost
{
 private const string GuolunExchangeReason="skill-program.guolun.exchange",SpPangTongDrawReason="skill-program.sp-pang-tong.draw";
 private static CardConversionSource SpPangTongSource(ProgramSkillFrame f)=>new(f.SkillId,GetProgramBindingId(f),f.OwnerSeat,f.SkillInstanceId);
 private bool SpPangTongCurrent(ProgramSkillFrame f)=>facts.Winner==Winner.None&&participants.Players[f.OwnerSeat].IsAlive&&skills.HasRuntimeSkillInstance(participants.Players[f.OwnerSeat],f.SkillId,f.SkillInstanceId)&&content.Skill(f.SkillId).Program?.GameplayHash==f.GameplayHash;
 [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 510, SkillProgramEffectOp.ResolveSongsangLimitedOtherDeath, SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation)]
 private bool CanOfferSpPangTong(ProgramTriggerCandidate c,SkillProgramTrigger t,ProgramSkillWindowContext w)
 {
  if(!t.Effects.Any(e=>SpPangTongComposition.Own(e.Op)))return true;
  if(!participants.Players[c.OwnerSeat].IsAlive)return false;
  return t.Effects[0].Op switch{
   SkillProgramEffectOp.ResolveSongsangLimitedOtherDeath=>w.Window==SkillProgramTriggerWindow.CharacterDied&&w.TargetSeat is{}victim&&victim!=c.OwnerSeat&&ValidSeat(victim)&&!participants.Players[victim].IsAlive&&participants.Players[c.OwnerSeat].HasHpValue&&frames.OfType<ProgramKillTriggerWindowFrame>().Any(p=>p.Id==w.ParentFrameId&&p.VictimSeat==victim),
   SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation=>w.Window==SkillProgramTriggerWindow.CardsGained&&w.MovementBatch is{}b&&MatchingZhanjiDrawIndexes(b,c).Length==1,
   _=>false};
 }
 public SkillProgramStepOutcome ExecuteSpPangTong(ProgramSkillFrame supplied,SkillProgramEffectOp op)
 {
  var f=frames.ActiveProgram(supplied.Id);if(f.SpPangTong is not null||f.InstructionIndex!=1||!SpPangTongCurrent(f))throw new InvalidOperationException("SP Pang Tong requires its exact original live skill instruction.");
  var r=new SpPangTongReceipt(SpPangTongSource(f),f.GameplayHash,f.InstructionIndex,turn.TurnNumber,"reveal",ParentId:f.WindowContext?.ParentFrameId);
  events.AdvanceFact(new SpPangTongStartedEvent(f.Id,r.Source,r.GameplayHash,r.Instruction,r.Turn,r.ParentId,op));
  if(op==SkillProgramEffectOp.ResolveGuolunRevealOptionalAtomicExchange)
  {
   if(f.TriggerId is not null||f.WindowContext is not null||f.SelectedCardIds.Count!=0||f.SelectedTargetSeats is not[var target]||target==f.OwnerSeat||!ValidSeat(target)||!participants.At(target).IsAlive||zones.Count(CardLocation.Hand(target))==0)throw new InvalidOperationException("Guolun lost its real selected other hand owner.");
   frames.ReplaceTop(f=f with{SpPangTong=r with{Target=target,OriginalHand=zones.CardsAt(CardLocation.Hand(target)).Select(c=>c.Id).ToArray()}});PublishSpPangTong(f);return SkillProgramStepOutcome.AwaitChoice;
  }
  if(f.WindowContext is not{}w)throw new InvalidOperationException("SP Pang Tong needs its original native window.");
  var trigger=content.Skill(f.SkillId).Program!.Triggers.Single(t=>t.Id==f.TriggerId);
  var candidate=new ProgramTriggerCandidate(f.OwnerSeat,f.SkillId,f.TriggerId!,f.SkillInstanceId,f.GameplayHash,trigger.Priority);
  if(!CanOfferSpPangTong(candidate,trigger,w))throw new InvalidOperationException("SP Pang Tong lost its exact real death or draw invocation.");
  if(op==SkillProgramEffectOp.DrawAfterActualPlayDrawInvocation)
  {
   frames.ReplaceTop(f=f with{SpPangTong=r with{DrawInvocation=w.MovementBatch!.NativeDrawInvocation!.InvocationId,RewardSeat=f.OwnerSeat}});PaySpPangTongDraw(f);return SkillProgramStepOutcome.AwaitChild;
  }
  var owner=participants.Players[f.OwnerSeat];var wounded=facts.IsRuleWounded(owner);r=r with{Stage="health-children",Target=w.TargetSeat!.Value,Wounded=wounded,HpBefore=owner.Hp,MaximumBefore=owner.MaxHp,RuleWounded=facts.UsesTreatedWounded?wounded:null};frames.ReplaceTop(f=f with{SpPangTong=r});events.AdvanceFact(new SongsangHealthIssuedEvent(f.Id,r.Source,r.GameplayHash,r.Target,r.Wounded,r.HpBefore,r.MaximumBefore,r.RuleWounded));
  if(r.Wounded)native.Recover(f.Id,f.OwnerSeat,f.OwnerSeat,1);else native.ChangeMaximumHp(f,1);
  frames.AdvanceProgram(f.Id);return SkillProgramStepOutcome.AwaitChild;
 }
 private IReadOnlyList<PromptChoice> SpPangTongChoices(ProgramSkillFrame f)
 {
  var r=f.SpPangTong!;var choices=new List<PromptChoice>();
  Dictionary<string,string> Args(string branch)=>new(){["program-action"]="sp-pang-tong",["frame-id"]=f.Id.ToString(CultureInfo.InvariantCulture),["branch"]=branch};
  if(r.Stage=="reveal")
  {
   foreach(var choice in native.OwnedCardPaymentChoices(f.Id,f.OwnerSeat,r.Target,[CardZoneKind.Hand],OwnedCardMoveIntent.Obtain))
   {var args=choice.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);args["program-action"]="sp-pang-tong";args["branch"]="reveal";choices.Add(choice with{Parameters=args});}
  }
  else if(r.Stage=="exchange"&&r.RevealedCard is{}id&&zones.GetLocation(id)==CardLocation.Hand(r.Target))
  {
   var grain=facts.UsesFormalWoodenOx&&zones.CardsAt(CardLocation.Equipment(f.OwnerSeat)).Any(c=>c.Kind==CardKind.WoodenOx)?zones.CardsAt(CardLocation.WoodenOxGrain(f.OwnerSeat)):[];
   foreach(var card in zones.CardsAt(CardLocation.Hand(f.OwnerSeat)).Concat(zones.CardsAt(CardLocation.Equipment(f.OwnerSeat))).Concat(grain))
   {
    var at=zones.GetLocation(card.Id);if(card.IsGeneralWeapon||boundCards.ForeignDiscardPrevented(f.OwnerSeat,card,at,OwnedCardMoveIntent.Transfer)||at.Zone==CardZoneKind.Equipment&&facts.IsSourceEquipment(f.OwnerSeat,f.SkillId,f.SkillInstanceId,card))continue;
    var args=Args("exchange");args["card-id"]=card.Id.ToString(CultureInfo.InvariantCulture);choices.Add(new(new($"guolun.{f.Id}.material.{card.Id}"),$"以{card.DisplayName}交换",[card.Id],[],args));
   }
   choices.Add(new(new($"guolun.{f.Id}.decline"),"不交换",[],[],Args("decline")));
  }
  return Array.AsReadOnly(choices.ToArray());
 }
 private void PublishSpPangTong(ProgramSkillFrame f)
 {
  if(!SpPangTongCurrent(f)||!participants.Players[f.SpPangTong!.Target].IsAlive){FinishSpPangTong(f);return;}var choices=SpPangTongChoices(f);if(choices.Count==0){FinishSpPangTong(f);return;}
  var skill=content.Skill(f.SkillId);frames.ReplaceTop(f with{Step=ResolutionFrameStep.AwaitingResponse});prompts.Set(new(DecisionKind.ProgramTrigger,f.OwnerSeat,f.SpPangTong.Stage=="reveal"?"过论：选择展示其一张手牌。":"过论：选择你的牌交换，或不交换。",choices.SelectMany(c=>c.Cards).Distinct().ToArray(),[],f.OwnerSeat){PromptId=prompts.NextId(),IsPrivate=true,TargetSeat=f.OwnerSeat,Choices=choices,SkillPrompt=new(f.SkillId,skill.Name,skill.Name,skill.Description)},participants.At(f.OwnerSeat).IsHuman);native.PublishState();
 }
 [ProgramAnswer("sp-pang-tong")] private void AnswerSpPangTong(PromptChoice selected)
 {
  if(frames.Top is not ProgramSkillFrame{SpPangTong:{}r}f||!SpPangTongCurrent(f)||prompts.Current?.PlayerSeat!=f.OwnerSeat||!AssistedChoicesEqual([selected],SpPangTongChoices(f).Where(c=>c.Id==selected.Id).ToArray()))throw new InvalidOperationException("Guolun lost its actual private chooser and current material.");prompts.Clear();var branch=selected.Parameters["branch"];
  if(branch=="decline"){FinishSpPangTong(f);return;}
  if(branch=="reveal")
  {
   var slot=int.Parse(selected.Parameters["slot-index"],CultureInfo.InvariantCulture);var hand=zones.CardsAt(CardLocation.Hand(r.Target));if(slot<0||slot>=hand.Count||!hand.Select(c=>c.Id).SequenceEqual(r.OriginalHand))throw new InvalidOperationException("Guolun opaque hand slot changed before its actual reveal.");
   var card=hand[slot];frames.ReplaceTop(f=f with{SpPangTong=r with{Stage="exchange",RevealedCard=card.Id}});events.AdvanceFact(new CardsRevealedEvent(f.Id,[snapshots.Snapshot(card)]));events.AdvanceFact(new GuolunRevealedEvent(f.Id,r.Source,r.Target,card.Id));if(native.BeginPublicHandReveal(f.Id,"guolun-foreign-hand"))return;PublishSpPangTong(frames.ActiveProgram(f.Id));return;
  }
  var own=Card(int.Parse(selected.Parameters["card-id"],CultureInfo.InvariantCulture));var foreign=Card(r.RevealedCard!.Value);var from=zones.GetLocation(own.Id);var before=movement.Sequence;
  r=r with{Stage="exchange-children",OwnCard=own.Id,OwnFrom=from,OwnRank=facts.EffectiveOwnedCardRank(participants.Players[f.OwnerSeat],own),TargetRank=facts.EffectiveOwnedCardRank(participants.Players[r.Target],foreign),Before=before,After=before};r=r with{RewardSeat=r.OwnRank<r.TargetRank?f.OwnerSeat:r.TargetRank<r.OwnRank?r.Target:null};frames.ReplaceTop(f=f with{SpPangTong=r,PendingMovementContinuation=new(f.OwnerSeat,0,null)});
  var moves=new[]{(Card:own,From:from,To:facts.NormalizeEphemeralDestination(own,from,CardLocation.Hand(r.Target))),(Card:foreign,From:CardLocation.Hand(r.Target),To:CardLocation.Hand(f.OwnerSeat))};var batch=batches.Begin(moves.Select(m=>m.From),moves.Select(m=>m.To));var records=new List<CardMovementRecord>();var committed=false;
  try{
   foreach(var m in moves)physicalZones.Move(m.Card.Id,m.From,m.To);
   foreach(var m in moves){records.Add(effects.Record(m.Card,m.From,m.To,new(GuolunExchangeReason)));effects.ResolveEquipmentSkillGrant(m.Card,m.From,m.To);effects.ClearJudgmentEffectiveKind(m.Card,m.From,m.To);effects.ResolveArmorRemoval(m.Card,m.From,new(GuolunExchangeReason));effects.ResolveStorageEquipmentMove(m.Card,m.From,m.To);}committed=true;
  }finally{batches.Complete(batch,records,committed);}
  f=frames.ActiveProgram(f.Id);frames.ReplaceTop(f=f with{SpPangTong=f.SpPangTong!with{After=movement.Sequence,ActualCount=2,BatchId=batch.Id}});IssueSpPangTongSegment(f);frames.AdvanceProgram(f.Id);
 }
 private void IssueSpPangTongSegment(ProgramSkillFrame f){var r=f.SpPangTong!;events.AdvanceFact(new SpPangTongSegmentPaidEvent(f.Id,r.Source,r.GameplayHash,r.Instruction,r.Stage,r.Stage=="draw-children"?r.RewardSeat!.Value:r.Target,r.Before,r.After,r.ActualCount,r.DrawInvocation));}
 private void PaySpPangTongDraw(ProgramSkillFrame f)
 {
  var r=f.SpPangTong!;if(r.RewardSeat is not{}seat||!participants.Players[seat].IsAlive||facts.Winner!=Winner.None){FinishSpPangTong(f);return;}var before=movement.Sequence;frames.ReplaceTop(f=f with{SpPangTong=r with{Stage="draw-children",Before=before,After=before,ActualCount=0},PendingMovementContinuation=new(f.OwnerSeat,0,null)});
  var drawn=draws.DrawCards(participants.At(seat),1,true,new(SpPangTongDrawReason),r.Source);f=frames.ActiveProgram(f.Id);frames.ReplaceTop(f=f with{SpPangTong=f.SpPangTong!with{After=movement.Sequence,ActualCount=drawn.Count}});IssueSpPangTongSegment(f);frames.AdvanceProgram(f.Id);
 }
 private void FinishSpPangTong(ProgramSkillFrame f){frames.ReplaceTop(f=f with{SpPangTong=null,PendingMovementContinuation=null});completion.Finish(f,true);}
 [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 290)]
 private bool IsSpPangTongMovement(ProgramSkillFrame f,SkillProgramEffect?e,ProgramMovementContinuation p)=>e is not null&&SpPangTongComposition.Own(e.Op)&&f.SpPangTong is{Stage:"exchange-children" or "draw-children"}&&p.SubjectSeat==f.OwnerSeat&&p.BeforeCount==0&&p.CoverageResultBind is null;
 [ProgramContinuation(ProgramContinuationKind.Movement,24700)] private bool ReturnSpPangTongMovement(ProgramSkillFrame f)=>f.SpPangTong is not null&&ResumeSpPangTong(f.Id);
 [ProgramContinuation(ProgramContinuationKind.Resume,24700)] private bool ResumeSpPangTong(long id)
 {
  if(frames.Top is not ProgramSkillFrame{SpPangTong:{}r}f||f.Id!=id)return false;  if(r.Stage is "reveal" or "exchange"){if(!SpPangTongCurrent(f)){FinishSpPangTong(f);return true;}if(prompts.Current is null)PublishSpPangTong(f);return true;}
  if(children.Begin(id,PostEventContinuation.AwaitedProgramMovement))return true;
  f=frames.ActiveProgram(id);r=f.SpPangTong!;if(r.Stage=="exchange-children"){frames.ReplaceTop(f=f with{PendingMovementContinuation=null});PaySpPangTongDraw(f);return true;}
  if(r.Stage=="health-children"&&participants.Players[f.OwnerSeat].IsAlive&&facts.Winner==Winner.None){frames.ReplaceTop(f=f with{SpPangTong=r with{Stage="grant-children"}});acquisitions.Grant(f,f.OwnerSeat,["ol:zhanji"]);frames.AdvanceProgram(id);return true;}
  FinishSpPangTong(f);return true;
 }
 [ProgramAiChoice(SkillProgramEffectOp.ResolveGuolunRevealOptionalAtomicExchange)] private PromptChoice SelectAiSpPangTong(PendingDecision p,ProgramSkillFrame f)=>p.Choices[0];
 [PublicHandRevealReturn("guolun-foreign-hand")]
 private void ReturnGuolunPublicHand(ProgramSkillFrame f)
 {
  PublishSpPangTong(f);
 }
 private bool ValidSeat(int seat)=>seat>=0&&seat<participants.Players.Count;
 private Card Card(int id)=>zones.CardsAt(zones.GetLocation(id)).Single(card=>card.Id==id);
}
