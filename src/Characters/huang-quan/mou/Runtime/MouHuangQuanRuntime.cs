using System.Globalization;
namespace CardGame.Core;
[ProgramRuntimeModule,ProgramCapability]
internal sealed partial class MouHuangQuanRuntime(IMouHuangQuanFacts facts,IMouHuangQuanNative native,IProgramRuntimeFrames frames,IProgramRuntimeEvents events,IProgramRuntimePrompts prompts,IProgramHealthChildQueries health,IProgramHealthChildOperations healthOperations):IMouHuangQuanHost
{
    internal const string MouHqQiaodui="mou:qiaodui",MouHqGiveReason="skill-program.mou:qiaodui.give";
    internal static CardConversionSource MouHqSource(ProgramSkillFrame f)=>new(f.SkillId,f.TriggerId??f.ActivationId,f.OwnerSeat,f.SkillInstanceId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification,35700,SkillProgramEffectOp.ResolveMouHuangQuanQiaodui)]
    private bool OfferMouHq(ProgramTriggerCandidate c,SkillProgramTrigger t,ProgramSkillWindowContext context)
    {
        if(t.Effects is[{Op:SkillProgramEffectOp.ResolveMouHuangQuanQiaodui}])return facts.Alive(c.OwnerSeat)&&facts.OriginalUse(c.OwnerSeat,context) is not null&&
            facts.Living.Any(p=>p!=c.OwnerSeat)&&!facts.History<MouHqCommittedEvent>().Any(e=>e.OwnerSeat==c.OwnerSeat&&e.Turn==facts.Turn&&e.TurnOwner==facts.TurnOwner);
        return true;
    }
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame supplied,SkillProgramEffectOp op)
    {
        var f=frames.ActiveProgram(supplied.Id);if(f.MouHuangQuan is not null){ContinueMouHq(f.Id);return SkillProgramStepOutcome.AwaitChild;}
        if(!facts.ParentMatches(f)||f.SelectedCardIds.Count!=0||f.SelectedTargetSeats.Count!=0)throw new InvalidOperationException("Qiaodui lost the original real-use parent.");
        var r=new MouHqReceipt(MouHqSource(f),f.GameplayHash,facts.Turn,facts.TurnOwner,f.WindowContext!.ParentFrameId);
        if(facts.OriginalUse(f.OwnerSeat,f.WindowContext) is not{}use||facts.History<MouHqCommittedEvent>().Any(e=>e.OwnerSeat==f.OwnerSeat&&e.Turn==r.Turn&&e.TurnOwner==r.TurnOwner))throw new InvalidOperationException("Qiaodui already spent this actual turn.");
        r=r with{Stage=MouHqStage.Cards,UseId=use.UseId,ActionId=use.ActionId,Outgoing=use.Outgoing};
        frames.ReplaceTop(f=f with{MouHuangQuan=r});events.AdvanceFact(new MouHqStartedEvent(f.Id,r.Source,r.GameplayHash,r.Turn,r.TurnOwner,r.WindowId,r.UseId,r.ActionId));PublishMouHq(f);return SkillProgramStepOutcome.AwaitChoice;
    }
    internal bool Valid(ProgramSkillFrame f)=>f.MouHuangQuan is{}r&&facts.ParentMatches(f)&&r.Source==MouHqSource(f)&&r.GameplayHash==f.GameplayHash&&r.Turn==facts.Turn&&r.TurnOwner==facts.TurnOwner&&
        r.WindowId==f.WindowContext!.ParentFrameId&&r.Before>=0&&r.After>=r.Before&&r.After<=facts.MovementSequence&&r.Selected.Select(x=>x.CardId).Distinct().Count()==r.Selected.Count&&
        facts.History<MouHqStartedEvent>().Count(e=>e.FrameId==f.Id&&e.Source==r.Source&&e.GameplayHash==r.GameplayHash&&e.WindowId==r.WindowId&&e.UseId==r.UseId&&e.ActionId==r.ActionId)==1;
    private IReadOnlyList<PromptChoice> MouHqChoices(ProgramSkillFrame f)
    {
        var r=f.MouHuangQuan!;var choices=new List<PromptChoice>();
        PromptChoice C(string branch,string label,int? id=null,int? target=null)=>new(new($"mou-hq.{f.Id}.{r.Stage}.{r.Selected.Count}.{branch}.{id??target??-1}"),label,id is{}card?[card]:[],target is{}seat?[seat]:[],
            new Dictionary<string,string>{{"program-action","mou-hq"},{"branch",branch},{"frame-id",f.Id.ToString(CultureInfo.InvariantCulture)}});
        if(r.Stage==MouHqStage.Recipient){choices.AddRange(facts.Living.Where(p=>p!=f.OwnerSeat).Select(p=>C("recipient",facts.Name(p),target:p)));return Array.AsReadOnly(choices.ToArray());}
        if(r.Stage!=MouHqStage.Cards||!facts.Alive(f.OwnerSeat))return [];
        var legal=facts.TransferEntities(f,f.OwnerSeat);var validSelected=r.Selected.All(x=>legal.Contains(x));
        if(validSelected&&r.Selected.Count<2)choices.AddRange(legal.Where(x=>!r.Selected.Contains(x)).Select(x=>C("card",$"选择【{facts.CardName(x.CardId)}】",x.CardId)));
        if(validSelected&&r.Selected.Count<=2)choices.Add(C("confirm","确认交牌并发动巧对"));
        if(r.Selected.Count>0)choices.Add(C("reset","重新选牌"));
        choices.Add(C("decline","不发动巧对"));return Array.AsReadOnly(choices.ToArray());
    }
    private void PublishMouHq(ProgramSkillFrame f)
    {
        var r=f.MouHuangQuan!;var choices=MouHqChoices(f);
        if(choices.Count==0){FinishMouHq(f,false);return;}
        frames.ReplaceTop(f with{Step=ResolutionFrameStep.AwaitingResponse});var s=facts.Skill(f.SkillId);
        prompts.Set(new(DecisionKind.ProgramTrigger,f.OwnerSeat,"巧对：选择至多两张牌及一名其他角色。",choices.SelectMany(c=>c.Cards).Distinct().ToArray(),choices.SelectMany(c=>c.Targets).Distinct().ToArray(),f.OwnerSeat)
        {PromptId=prompts.NextId(),IsPrivate=r.Stage!=MouHqStage.Recipient,TargetSeat=r.Recipient>=0?r.Recipient:null,Choices=choices,SkillPrompt=new(s.Id,s.Name,s.Name,s.Description)},facts.Human(f.OwnerSeat));native.Publish();
    }
    private static bool Equal(PromptChoice a,PromptChoice b)=>a.Id==b.Id&&a.Cards.SequenceEqual(b.Cards)&&a.Targets.SequenceEqual(b.Targets)&&a.Parameters.Count==b.Parameters.Count&&a.Parameters.All(p=>b.Parameters.GetValueOrDefault(p.Key)==p.Value);
    [ProgramAnswer("mou-hq")]internal void AnswerMouHq(PromptChoice supplied)
    {
        if(frames.Top is not ProgramSkillFrame{MouHuangQuan:{}r}f||!Valid(f)||prompts.Current?.PlayerSeat!=f.OwnerSeat||!MouHqChoices(f).Any(c=>Equal(c,supplied)))throw new InvalidOperationException("Huang Quan accepts only the current payer's canonical dynamic physical choice.");
        prompts.Clear();frames.ReplaceTop(f=f with{Step=ResolutionFrameStep.ResolvingEffect});
        switch(supplied.Parameters["branch"])
        {
            case "decline":FinishMouHq(f,false);return;
            case "card":var entity=facts.TransferEntities(f,f.OwnerSeat).Single(x=>x.CardId==supplied.Cards.Single());frames.ReplaceTop(f=f with{MouHuangQuan=r with{Selected=r.Selected.Append(entity).ToArray()}});PublishMouHq(f);return;
            case "reset":frames.ReplaceTop(f=f with{MouHuangQuan=r with{Selected=[]}});PublishMouHq(f);return;
            case "confirm":frames.ReplaceTop(f=f with{MouHuangQuan=r with{Stage=MouHqStage.Recipient}});PublishMouHq(f);return;
            case "recipient":frames.ReplaceTop(f=f with{MouHuangQuan=r with{Recipient=supplied.Targets.Single()}});GiveMouHq(f);return;
            default:throw new InvalidOperationException("Unknown Huang Quan choice.");
        }
    }
    private void GiveMouHq(ProgramSkillFrame f)
    {
        var r=f.MouHuangQuan!;var recipient=r.Recipient;
        var legal=facts.TransferEntities(f,f.OwnerSeat);if(r.Selected.Any(x=>!legal.Contains(x))||r.Selected.Count>2||recipient==f.OwnerSeat||!facts.Alive(recipient))throw new InvalidOperationException("Huang Quan's actual gift lost its exact current sources, count or other recipient.");
        if(facts.History<MouHqCommittedEvent>().Any(e=>e.OwnerSeat==f.OwnerSeat&&e.Turn==r.Turn&&e.TurnOwner==r.TurnOwner))throw new InvalidOperationException("Qiaodui cannot spend one actual-turn budget twice.");
        var before=facts.MovementSequence;frames.ReplaceTop(f=f with{MouHuangQuan=r with{Paid=true,Count=r.Selected.Count,Stage=MouHqStage.GiveChildren,Before=before,After=before},PendingMovementContinuation=new(f.OwnerSeat,0,null)});
        events.AdvanceFact(new MouHqCommittedEvent(f.Id,f.OwnerSeat,recipient,r.Selected.Count,r.Turn,r.TurnOwner));
        native.Give(f,f.OwnerSeat,recipient,r.Selected.Select(x=>x.CardId).ToArray(),before);
        f=frames.ActiveProgram(f.Id);frames.ReplaceTop(f with{MouHuangQuan=f.MouHuangQuan! with{After=facts.MovementSequence}});ContinueMouHq(f.Id);
    }
    private void ContinueMouHq(long id)
    {
        if(prompts.Current is not null||frames.Top is not ProgramSkillFrame{MouHuangQuan:{}r}f||f.Id!=id)return;
        if(!Valid(f))throw new InvalidOperationException("Huang Quan lost its exact owning continuation.");
        if(r.Stage is MouHqStage.Cards or MouHqStage.Recipient){PublishMouHq(f);return;}
        if(native.Drain(f))return;f=frames.ActiveProgram(id);r=f.MouHuangQuan!;frames.ReplaceTop(f=f with{PendingMovementContinuation=null});
        if(r.Stage==MouHqStage.GiveChildren){native.OriginalEffect(f);FinishMouHq(f,true);return;}FinishMouHq(f,true);
    }
    private void FinishMouHq(ProgramSkillFrame f,bool accepted)
    {events.AdvanceFact(new MouHqCompletedEvent(f.Id));frames.ReplaceTop(f=f with{MouHuangQuan=null,PendingMovementContinuation=null});native.Finish(f,accepted);}
    [ProgramContinuation(ProgramContinuationKind.Resume,35700)]internal bool ResumeMouHq(long id){if(frames.Top is not ProgramSkillFrame{MouHuangQuan:not null}f||f.Id!=id)return false;ContinueMouHq(id);return true;}
    [ProgramContinuation(ProgramContinuationKind.Movement,35700)]internal bool MovementMouHq(ProgramSkillFrame f)=>f.MouHuangQuan is not null&&f.PendingMovementContinuation is not null&&ResumeMouHq(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath,35700)]internal bool KeepMouHqAfterOwnerDeath(ProgramSkillFrame f)=>f.MouHuangQuan is{Paid:true}&&Valid(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss,35700)]internal bool KeepPaidQiaodui(ProgramSkillFrame f)=>f.SkillId==MouHqQiaodui&&f.MouHuangQuan is{Paid:true}&&Valid(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouHuangQuanQiaodui)]internal PromptChoice ChooseMouHq(PendingDecision p,ProgramSkillFrame f)
    {
        var r=f.MouHuangQuan!;
        if(r.Stage==MouHqStage.Recipient)return p.Choices.OrderByDescending(c=>facts.HelpValue(f.OwnerSeat,c.Targets.Single())).First();
        if(p.Choices.FirstOrDefault(c=>c.Parameters["branch"]=="confirm")is{}confirm)return confirm;
        return p.Choices.Last();
    }
}
