using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryZhongHuiMobileChecks
{
    private const string Hero="boundary:zhong-hui-mobile",Quanji="boundary:quanji-mobile",Zili="boundary:zili-mobile",Paiyi="boundary:paiyi-mobile",
        Owner="fixture:boundary-zhong-hui-owner",Driver="fixture:boundary-zhong-hui-driver",Stock="fixture:boundary-zhong-hui-stock",
        Watch="fixture:boundary-zhong-hui-paid",Mode="identity:classic-boundary-zhong-hui";

    public static void PerDamagePointAndOwnPlayEndAuthorityAwakeningPaiyiPaidCold()
    {
        var s=Scene.Start();s.Play(0);s.Need(s.V(0).Hp==5&&s.V(0).HandCount==6&&s.V(1).HandCount==10,"The real fixed fixture starts from native lord HP5, six owner hand entities and a stocked target.");
        s.Accept(new UseProgramSkillCommand(0,Driver,"real-two-damage",[],[1],s.G.Revision,s.P!.PromptId));
        var stored=new List<int>();
        for(var point=0;point<2;point++)
        {
            s.Offer(Quanji);s.Invoke();var card=s.SelectSource();stored.Add(card);s.AtPayment();
            s.Need(s.V(0).AuthorityCount==point+1&&s.V(0).AuthorityCards!.Any(c=>c.Id==card)&&s.V(0).HandCount==6&&s.V(0).Hp==3,
                "Each actual damage point draws then stores one real selected hand entity, without changing hand count or reapplying the original two damage.");
            if(point==0)s.Cold();s.Answer(_=>true);
        }
        s.Play(0);s.Need(s.E<DamageAppliedEvent>() is[{SourceSeat:1,TargetSeat:0,Amount:2}]&&s.E<ProgramBindingStartedEvent>().Count(e=>e.SkillId==Quanji&&e.Window==SkillProgramTriggerWindow.AfterDamageApplied)==2&&s.Limit()==5,
            "One genuine amount2 damage issues two optional Quanji occurrences, and the original Authority pile contributes exactly two to the native HP3 hand limit.");
        s.End();s.Offer(Quanji);s.Need(s.V(0).HandCount>s.V(0).Hp,"The extra mobile opportunity is the owner's actual ending Play with hand greater than HP.");
        s.Invoke();stored.Add(s.SelectSource());s.AtPayment();s.Answer(_=>true);s.Play(1);
        s.Need(s.V(0).AuthorityCount==3&&s.V(0).HandCount==6&&s.Limit()==6&&s.E<ProgramBindingStartedEvent>().Count(e=>e.SkillId==Quanji&&e.Window==SkillProgramTriggerWindow.PlayEnding)==1&&
            s.E<SkillAwakenedEvent>().All(e=>e.SkillId!=Zili),"The own-Play ending adds the third exact Authority entity and native discard retains six, while awakening waits for the next real preparation.");
        for(var actor=1;actor<5;actor++){s.Play(actor);s.End();}
        s.Reach(()=>s.P?.SkillPrompt?.SkillId==Zili&&s.P.Choices.Any(c=>c.Parameters.GetValueOrDefault("binding-id")=="draw"));
        s.Need(s.P!.Choices.Any(c=>c.Parameters.GetValueOrDefault("binding-id")=="recover"),"At the actual preparation with three Authority and wounded HP, shared awakening offers both real benefits.");
        s.Answer(c=>c.Parameters.GetValueOrDefault("binding-id")=="draw");s.Play(0);
        s.Need(s.V(0).MaxHp==4&&s.V(0).Hp==3&&s.V(0).HandCount==10&&s.V(0).Skills!.Any(k=>k.Id==Paiyi)&&
            s.E<SkillAwakenedEvent>().Count(e=>e.SkillId==Zili&&e.AcquiredSkillIds.SequenceEqual([Paiyi]))==1&&
            s.E<MaximumHpChangedEvent>().Count(e=>e.PlayerSeat==0&&e.SkillId==Zili&&e.Delta==-1)==1,
            "The real shared awakening draws2, reduces maximum HP once and grants this verified mobile Paiyi, before the same turn's normal draw2.");
        var targetHand=s.V(1).HandCount;var hp=s.V(1).Hp;var cost=stored[1];
        s.Accept(new UseProgramSkillCommand(0,Paiyi,"remove-authority",[cost],[1],s.G.Revision,s.P!.PromptId));s.AtPayment();
        var frame=s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.SkillId==Paiyi);
        s.Need(s.V(0).AuthorityCount==2&&s.V(1).HandCount==targetHand&&s.V(1).Hp==hp&&frame.SelectedCardPayment is{MovementCommitted:true},
            "The activated native ability pays exactly its chosen original Authority card and waits on the original selected-card payment before drawing or damaging the selected target.");
        s.Cold();s.Answer(_=>true);s.Play(0);
        s.Need(s.V(1).HandCount==targetHand+2&&s.V(1).Hp==hp-1&&s.V(0).AuthorityCount==2&&s.Limit()==5&&
            s.E<DamageAppliedEvent>().Count(e=>e.SourceSeat==0&&e.TargetSeat==1&&e.Amount==1)==1&&
            s.G.CardMovements.Count(m=>m.CardId==cost&&m.From==CardLocation.Authority(0)&&m.To==CardLocation.DiscardPile&&m.Reason.Value=="skill-program."+Paiyi+".DiscardSelected")==1&&
            s.G.GetLegalActions(0).All(a=>a.ProgramSkillId!=Paiyi)&&
            stored.All(id=>s.G.CardMovements.Count(m=>m.CardId==id&&m.To==CardLocation.Authority(0)&&m.Reason.Value=="skill-program."+Quanji+".MoveBoundCards")==1),
            "Paid cold returns without paying Authority twice: the identity edition draws exactly2 and conditionally deals one real damage, consumes its one Play-phase use, and updates the Authority-derived hand limit.");
    }

    private sealed class Scene(ContentRegistry registry,GameEngine game)
    {
        public GameEngine G{get;private set;}=game;
        public PendingDecision? P=>Enumerable.Range(0,5).Select(i=>G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p=>p is not null);
        public PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat).Players.Single(p=>p.Seat==seat);
        public T[] E<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
        public int Limit(){const BindingFlags f=BindingFlags.Instance|BindingFlags.NonPublic;var p=(IReadOnlyList<CharacterState>)typeof(GameEngine).GetField("_players",f)!.GetValue(G)!;return(int)typeof(GameEngine).GetMethod("GetHandLimit",f)!.Invoke(G,[p[0]])!;}
        public void Play(int actor)=>Reach(()=>P is{Kind:DecisionKind.PlayCard}&&P.PlayerSeat==actor);
        public void End()=>Accept(new EndPlayPhaseCommand(P!.PlayerSeat,G.Revision,P.PromptId));
        public void Offer(string skill)=>Reach(()=>P?.SkillPrompt?.SkillId==skill&&P.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="activate"));
        public void Invoke()=>Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
        public int SelectSource(){Reach(()=>P?.SkillPrompt?.SkillId==Quanji&&P.Choices.Any(c=>c.Cards.Count==1)==true);var card=P!.Choices.First(c=>c.Cards.Count==1).Cards.Single();Answer(c=>c.Cards.SequenceEqual([card]));return card;}
        public void AtPayment()=>Reach(()=>P?.SkillPrompt?.SkillId==Watch);
        public void Answer(Func<PromptChoice,bool> predicate){var p=P!;Accept(new AnswerPromptCommand(p.PlayerSeat,p.PromptId,p.Choices.First(predicate).Id,G.Revision));}
        public void Accept(GameCommand c){var r=G.Submit(c);Need(r.Accepted,r.Error?.Message??"Rejected native Zhong Hui scene command.");}
        public void Reach(Func<bool> done)
        {
            for(var i=0;i<160&&!done();i++)
            {
                if(P is{Kind:DecisionKind.SelectGeneral}select){Accept(new SelectGeneralCommand(select.PlayerSeat,select.PlayerSeat==0?Owner:"fixture:boundary-zhong-hui-peer-"+select.PlayerSeat,G.Revision,select.PromptId));continue;}
                if(P is{Kind:DecisionKind.ProgramTrigger})throw new InvalidOperationException("Unexpected Zhong Hui original boundary: "+JsonSerializer.Serialize(new{P.Kind,P.PlayerSeat,Skill=P.SkillPrompt?.SkillId,P.Prompt}));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(),"The bounded native Zhong Hui fixture missed its actual boundary.");
        }
        public void Cold(){var before=Dump();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),registry);Need(before==Dump(),"Cold keeps the original damage-point or paid Authority activation on its original native frames.");}
        private string Dump()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,5).Select(i=>SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),G.CardMovements});
        public void Need(bool condition,string text){if(!condition)throw new InvalidOperationException(text+" "+JsonSerializer.Serialize(new{Pending=P is{}p?new{p.Kind,p.PlayerSeat,Skill=p.SkillPrompt?.SkillId}:null,Players=Enumerable.Range(0,5).Select(i=>new{Seat=i,V(i).Hp,V(i).MaxHp,V(i).HandCount,V(i).AuthorityCount})}));}
        public static Scene Start(){var r=ContentRegistry.Build(new StandardContentPackage(),new Fixture());var g=GameEngine.CreateStandard(new(){Seed=31,PlayerCount=5,HumanSeat=0,HumanSeats=[0,1,2,3,4],HumanRole=Role.Lord,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=12},r);var s=new Scene(r,g);s.Accept(new StartGameCommand());return s;}
    }

    private sealed class Fixture:IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-boundary-zhong-hui",new(1,0,0));
        internal static string Rules()=>$$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
         {"id":"{{Driver}}","revision":1,"activations":[{"id":"real-two-damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":2}]}]},
         {"id":"{{Stock}}","revision":1,"triggers":[{"id":"real-starting-stock","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":6}]}]},
         {"id":"{{Watch}}","revision":1,"triggers":[
          {"id":"stored-authority-movement","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:quanji-mobile.MoveBoundCards"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]},
          {"id":"paid-authority-movement","window":"cardsMoved","subject":"owner","sourceZones":["authority"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:paiyi-mobile.DiscardSelected"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
         ]}
        ]}
        """;
        internal static string Presentation()=>JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=new Dictionary<string,object>{
            [Driver]=new{name="真实2点伤害",description="由选定角色造成一次原生2点伤害"},[Stock]=new{name="真实起手补给",description="开局实际摸6张牌"},[Watch]=new{name="权的真实付款子窗",description="在原始实体移动后返回原帧",optionLabels=new Dictionary<string,string>{{"continue","继续"}}}}});
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,Hero);var c=SkillProgramCatalog.Load(Rules(),Presentation());foreach(var(id,p)in c.Programs)b.AddSkill(new(id,c.Presentations[id].Name,c.Presentations[id].Description){Program=p,ProgramPresentation=c.Presentations[id]});
            b.AddGeneral(new(Owner,"界钟会真实权计与觉醒","supporter",Quanji,"wei",4,AdditionalSkillIds:[Zili,Driver,Watch]));var peers=Enumerable.Range(1,4).Select(i=>"fixture:boundary-zhong-hui-peer-"+i).ToArray();
            foreach(var p in peers)b.AddGeneral(new(p,"固定实体角色","supporter","standard:none","wei",12,AdditionalSkillIds:p.EndsWith("-1",StringComparison.Ordinal)?[Stock]:[]));
            const string deck="fixture:boundary-zhong-hui-deck";b.AddDeck(new(deck,"固定真实实体",4,2,[]){PhysicalCards=Enumerable.Range(0,120).Select(i=>new ContentDeckPhysicalCard("standard:slash",Suit.Heart,i%13+1)).ToArray()});
            b.AddMode(new(Mode,"界钟会实际身份主流程",5,5,new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Loyalist),1},{nameof(Role.Rebel),2},{nameof(Role.Renegade),1}},deck,GeneralCandidateCount:5,GeneralPoolIds:[Owner,..peers]));
        }
    }
}
