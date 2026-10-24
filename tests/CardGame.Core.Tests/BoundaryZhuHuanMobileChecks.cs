using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class BoundaryZhuHuanMobileChecks
{
    private const string Hero="boundary:zhu-huan-mobile",Fenli="boundary:fenli-mobile",Pingkou="boundary:pingkou-mobile",
        Quiet="fixture:boundary-zhu-huan-quiet",Watch="fixture:boundary-zhu-huan-paid",External="fixture:boundary-zhu-huan-external-skip",Deck="fixture:boundary-zhu-huan-deck";
    public static void TiedFenliActualQuotaSequentialDamageDyingAndOneEquipmentCold()
    {
        var s=Scene.Start(equipment:true,external:false);
        s.Reach(()=>s.P?.SkillPrompt?.SkillId==Fenli);
        s.Need(s.V(0).HandCount==s.V(1).HandCount&&s.V(0).HandCount==s.V(2).HandCount&&s.V(0).Hp==s.V(2).Hp&&s.V(0).Equipment.Count==1,
            "The native draw offer starts from tied maximum hands and tied maximum HP, with real nonempty maximum equipment.");
        for(var phase=0;phase<3;phase++)
        {s.Reach(()=>s.P?.SkillPrompt?.SkillId==Fenli);s.Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");}
        s.Reach(()=>s.P?.SkillPrompt?.SkillId==Pingkou);s.Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
        s.Reach(()=>s.Root?.BoundaryZhuHuan?.Stage==BoundaryZhuHuanStage.Targets);
        s.Need(s.Root!.BoundaryZhuHuan!.Maximum==3&&s.E<BoundaryZhuHuanPhaseSkippedEvent>().Select(e=>e.Phase).SequenceEqual([SkillProgramTurnPhase.Draw,SkillProgramTurnPhase.Play,SkillProgramTurnPhase.Discard])&&
            s.E<TurnEndedEvent>().Any(e=>e.ActorSeat==0&&e.TurnNumber==s.G.State.TurnNumber),
            "Each accepted Fenli choice reaches one actual native skip, and Pingkou starts only after the real TurnEnded event with quota3.");
        var root=s.Root.Id;
        foreach(var target in new[]{2,1})s.Answer(c=>Branch(c)=="target"&&c.Targets.SequenceEqual([target]));
        s.Answer(c=>Branch(c)=="finish");s.Reach(()=>s.P is{Kind:DecisionKind.RescueDying,PlayerSeat:0});
        var peach=s.P!.Choices.First(c=>c.Parameters.GetValueOrDefault("response")=="peach").Cards.Single();
        s.Need(s.V(1).Hp==0&&s.V(2).Hp==4&&s.Root!.BoundaryZhuHuan!.Targets.SequenceEqual([1,2])&&s.Root.AttackAttempt is{TargetSeat:1}&&
            s.E<BoundaryZhuHuanEquipmentObtainedEvent>().Length==0,
            "Targets resolve in actual action order; first native damage and its live dying child finish before the second target or equipment reward.");
        s.Cold();s.Answer(c=>c.Parameters.GetValueOrDefault("response")=="peach"&&c.Cards.SequenceEqual([peach]));
        s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);
        var gain=s.E<BoundaryZhuHuanEquipmentObtainedEvent>().Single();
        s.Need(s.V(1).Hp==1&&s.V(2).Hp==3&&s.V(0).Hand.Any(c=>c.Id==gain.CardId)&&s.Root?.BoundaryZhuHuan?.Stage==BoundaryZhuHuanStage.Movement&&
            s.E<BoundaryZhuHuanDamageReturnedEvent>().Where(e=>e.FrameId==root).Select(e=>e.Target).SequenceEqual([1,2])&&
            s.G.CardMovements.Count(m=>m.Reason.Value=="skill.boundary-zhu-huan.pingkou.equipment")==1,
            "After both native damage returns, the whole invocation obtains exactly one real random equipment entity and pauses its paid gain observer.");
        s.Cold();s.Answer(_=>true);s.Reach(()=>s.E<BoundaryZhuHuanCompletedEvent>().Any(e=>e.FrameId==root));
        s.Need(s.E<BoundaryZhuHuanEquipmentObtainedEvent>().Length==1&&s.E<BoundaryZhuHuanDamageIssuedEvent>().Length==2&&
            s.E<DamageAppliedEvent>().Count(e=>e.SourceSeat==0&&e.TargetSeat is 1 or 2&&e.Amount==1)==2&&
            s.G.CardMovements.Count(m=>m.CardId==gain.CardId&&m.From==CardLocation.DrawPile&&m.To==CardLocation.Hand(0)&&m.Reason.Value=="skill.boundary-zhu-huan.pingkou.equipment")==1,
            "Cold restoring dying and already-paid equipment children does not reissue damage, reroll equipment or acquire one item per target.");
    }

    public static void ExternalActualJudgmentSkipCountsAndEmptyEquipmentDoesNotQualify()
    {
        foreach(var external in new[]{false,true})
        {
            var s=Scene.Start(equipment:false,external:external);
            s.Reach(()=>s.P?.SkillPrompt?.SkillId==Fenli);s.Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="skip");
            s.Reach(()=>s.P?.SkillPrompt?.SkillId==Fenli);s.Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="skip");
            s.Reach(()=>s.P is{Kind:DecisionKind.PlayCard,PlayerSeat:0});
            s.Need(s.V(0).Equipment.Count==0&&s.E<BoundaryZhuHuanPhaseSkippedEvent>().Length==(external?1:0),
                "Declining the two tied-maximum choices does not fabricate a skip; the independent native judgment skip counts only when actually reached.");
            s.Accept(new EndPlayPhaseCommand(0,s.G.Revision,s.P!.PromptId));
            if(external)
            {
                s.Reach(()=>s.P?.SkillPrompt?.SkillId==Pingkou);s.Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
                s.Reach(()=>s.Root?.BoundaryZhuHuan?.Stage==BoundaryZhuHuanStage.Targets);
                s.Need(s.Root!.BoundaryZhuHuan!.Maximum==1&&s.E<BoundaryZhuHuanPhaseSkippedEvent>() is[{Phase:SkillProgramTurnPhase.Judgment}],
                    "Pingkou counts an actual phase skipped by another native skill, while zero equipment never offers Fenli's discard substitution.");
                s.Answer(c=>Branch(c)=="target"&&c.Targets.SequenceEqual([2]));
                s.Need(s.P!.Choices.All(c=>Branch(c)!="target"),"The real one-phase quota offers no second target.");
                s.Answer(c=>Branch(c)=="finish");s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);s.Answer(_=>true);
                s.Reach(()=>s.E<BoundaryZhuHuanCompletedEvent>().Length==1);
                s.Need(s.V(2).Hp==3&&s.E<BoundaryZhuHuanEquipmentObtainedEvent>().Length==1,"A non-Fenli actual skip still earns one complete native damage and one equipment reward.");
            }
            else
            {
                s.Reach(()=>s.E<TurnStartedEvent>().Any(e=>e.ActorSeat!=0));
                s.Need(s.E<BoundaryZhuHuanStartedEvent>().Length==0&&s.E<BoundaryZhuHuanEquipmentObtainedEvent>().Length==0&&s.E<BoundaryZhuHuanPhaseSkippedEvent>().All(e=>e.OwnerSeat!=0),
                    "A normally completed turn without actual skips never offers Pingkou or obtains equipment; tied zero equipment is insufficient for Fenli.");
            }
        }
    }
    private static string? Branch(PromptChoice c)=>c.Parameters.GetValueOrDefault("branch");
    private sealed class Scene(GameEngine game,ContentRegistry registry)
    {
        public GameEngine G{get;private set;}=game;
        public PendingDecision? P=>Enumerable.Range(0,3).Select(i=>G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p=>p is not null);
        public PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat).Players.Single(p=>p.Seat==seat);
        public ProgramSkillFrame? Root=>G.ResolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f=>f.BoundaryZhuHuan is not null);
        public T[] E<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
        public void Answer(Func<PromptChoice,bool> predicate){var p=P!;Accept(new AnswerPromptCommand(p.PlayerSeat,p.PromptId,p.Choices.First(predicate).Id,G.Revision));}
        public void Accept(GameCommand command){var result=G.Submit(command);Need(result.Accepted,result.Error?.Message??"Rejected Zhu Huan fixture command.");}
        public void Reach(Func<bool> done)
        {
            for(var step=0;step<150&&!done();step++)
            {
                if(P is{Kind:DecisionKind.ProgramTrigger,PlayerSeat:0}prompt)
                    throw new InvalidOperationException("Unexpected native Zhu Huan boundary: "+JsonSerializer.Serialize(prompt));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(),"The bounded real Zhu Huan fixture missed its expected native boundary.");
        }
        private string State()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,3).Select(i=>SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),
            Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),G.CardMovements});
        public void Cold(){var before=State();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),registry);Need(before==State(),"Cold preserves the actual skipped ledger, exact target damage/dying return and once-paid equipment on the original frame.");}
        public void Need(bool valid,string message){if(!valid)throw new InvalidOperationException(message+JsonSerializer.Serialize(new{P,Players=Enumerable.Range(0,3).Select(i=>new{Seat=i,V(i).Hp,V(i).HandCount}),Root=Root?.BoundaryZhuHuan}));}
        public static Scene Start(bool equipment,bool external)
        {
            var registry=ContentRegistry.Build(new StandardContentPackage(),new Fixture());
            var setup=new ShanheBattleSetup(46,1,"boundary-zhu-huan-native",Hero,4,0,0,0,0,external?[Watch,External]:[Watch],
                [new(CardKind.Peach,Suit.Heart,3)],equipment?[new(CardKind.Crossbow,Suit.Club,1)]:[],
                [new(Peer(1),Hp:1){StartingHand=Enumerable.Range(0,5).Select(i=>new ShanheStartingCard(CardKind.Crossbow,Suit.Club,i+1)).ToArray()},
                 new(Peer(2),Hp:4){StartingHand=Enumerable.Range(0,5).Select(i=>new ShanheStartingCard(CardKind.Crossbow,Suit.Club,i+1)).ToArray()}],[],new string('b',64));
            var game=GameEngine.CreateStandard(new(){Seed=setup.BattleSeed,PlayerCount=3,HumanSeat=0,HumanRole=null,HumanTeamId=ShanheBattleSetup.PlayerTeamId,ModeId=ShanheBattleSetup.BattleModeId,ShanheBattle=setup,
                UseInteractiveSetup=false,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=8},registry);
            var scene=new Scene(game,registry);scene.Accept(new StartGameCommand());return scene;
        }
    }
    private static string Peer(int seat)=>"fixture:boundary-zhu-huan-peer-"+seat;
    private sealed class Fixture:IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-boundary-zhu-huan",new(1,0,0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder,Hero);
            var catalog=SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{External}}","revision":1,"triggers":[{"id":"real-judgment","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["judgment"]}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[{"id":"equipment-paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill.boundary-zhu-huan.pingkou.equipment"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
            ]}
            """,JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=new Dictionary<string,object>{
                [Quiet]=new{name="安静对手",description="真实原生行动顺序"},[External]=new{name="其他实际跳阶段",description="原生判断阶段被实际跳过"},
                [Watch]=new{name="平寇装备已付款",description="原生移动后续接",optionLabels=new Dictionary<string,string>{{"continue","继续"}}}}}));
            foreach(var(id,program)in catalog.Programs)builder.AddSkill(new(id,catalog.Presentations[id].Name,catalog.Presentations[id].Description){Program=program,ProgramPresentation=catalog.Presentations[id]});
            foreach(var seat in new[]{1,2})builder.AddGeneral(new(Peer(seat),"固定同桌","supporter",Quiet,"wei",4));
            builder.AddDeck(new(Deck,"固定原生装备库存",4,0,[]){PhysicalCards=Enumerable.Range(0,80).Select(i=>new ContentDeckPhysicalCard("standard:crossbow",Suit.Club,i%13+1)).ToArray()});
            builder.AddMode(new(ShanheBattleSetup.BattleModeId,"界朱桓真实跳阶段",3,3,new Dictionary<string,int>(),Deck,GeneralPoolIds:[Hero,Peer(1),Peer(2)],ModeKind:ContentModeKind.Team,
                TeamCounts:new Dictionary<string,int>{{ShanheBattleSetup.PlayerTeamId,1},{ShanheBattleSetup.EnemyTeamId,2}}));
        }
    }
}
