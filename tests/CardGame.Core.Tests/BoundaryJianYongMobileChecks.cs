using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
internal static class BoundaryJianYongMobileChecks
{
    private const string Hero="boundary:jian-yong-mobile", Qiao="boundary:qiaoshui-mobile", Zong="boundary:zongshih-mobile", Owner="fixture:mobile-jy-owner", Driver="fixture:mobile-jy-driver", Watch="fixture:mobile-jy-watch", Rank="fixture:mobile-jy-rank", Extra="fixture:mobile-jy-extra", Mode="identity:classic-mobile-jian-yong";
    public static void PhaseOnceEligibleNextUseAndRemoteNativeTargetsCold()
    {
        var s=Scene.Start();s.Play();s.Draw();s.Play();s.Contest(0,Qiao,1);s.Zongshi();
        var result=s.E<PindianResultDeterminedEvent>().Last().Result;var top=s.Root().BoundaryJianYongZongshi!.TopCardId;
        s.Need(result.SourceRank==13&&result.OpponentRank==1&&s.P!.Choices.Where(c=>Branch(c)=="contest").Single().Cards.SequenceEqual([result.OpponentCardId]),"Zongshi compares the native final effective ranks, not printed ranks or the winner's own card.");
        s.Cold();s.Answer(c=>Branch(c)=="contest");s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);var root=s.Root();var id=root.Id;
        s.Need(root.BoundaryJianYongZongshi is{Choice:1,From.Zone:CardZoneKind.Processing}&&s.V(0).Hand.Any(c=>c.Id==result.OpponentCardId)&&s.G.ResolutionStack.OfType<PindianFrame>().Any(),"The actual lower Pindian entity is obtained before its paid movement child and before the original contest completes.");
        s.Cold();s.Answer(_=>true);s.Play();
        s.Need(s.E<BoundaryJianYongZongshiPaidEvent>().Count(e=>e.ProgramFrameId==id)==1&&s.G.GetLegalActions(0).All(a=>a.ProgramSkillId!=Qiao||a.ProgramActivationId!="contest"),"The original native result returns once and Qiaoshui is already used for this actual Play phase.");
        var grant=s.E<NextActualUseTargetAdjustmentGrantedEvent>().Single();s.PlayKind(LegalActionKind.Equip);s.Play();s.PlayKind(LegalActionKind.Lightning);s.Play();
        s.Need(!s.E<NextActualUseTargetAdjustmentConsumedEvent>().Any(e=>e.GrantProgramFrameId==grant.ProgramFrameId),"A real equipment use and real delayed trick use do not consume the next-basic-or-ordinary-trick grant.");
        var adjusted=s.G.GetLegalActions(0).First(a=>a.Kind==LegalActionKind.Slash&&a.TargetSeats.SequenceEqual([1,2])&&a.ProgramSkillId==Qiao);var hp1=s.V(1).Hp;var hp2=s.V(2).Hp;
        s.PlayAction(adjusted);s.Play();
        s.Need(s.V(1).Hp==hp1-1&&s.V(2).Hp==hp2-1&&s.E<NextActualUseTargetAdjustmentConsumedEvent>().Count(e=>e.GrantProgramFrameId==grant.ProgramFrameId)==1,"The original Slash pays once, really hits both targets including the out-of-range added target, and consumes one eligible-use receipt.");
        var expiry=Scene.Start(extra:true);expiry.Play();expiry.Contest(0,Qiao,1);expiry.Zongshi();expiry.Answer(c=>Branch(c)=="skip");expiry.Play();var turn=expiry.G.CreateSnapshot(0).TurnNumber;
        expiry.Accept(new EndPlayPhaseCommand(0,expiry.G.Revision,expiry.P!.PromptId));expiry.Play();
        expiry.Need(expiry.G.CreateSnapshot(0).TurnNumber==turn&&expiry.G.GetLegalActions(0).Any(a=>a.ProgramSkillId==Qiao&&a.ProgramActivationId=="contest")&&expiry.G.GetLegalActions(0).All(a=>a.ProgramActivationId!="next-actual-use-target-adjustment"),"A real inserted Play ends within the same turn: the unused allowance expires, and the once-per-Play activation is available in the normal Play.");
    }
    public static void TieTopChoiceFailureExpiresAndOpponentPindianTriggersCold()
    {
        var s=Scene.Start(win:false,extra:true);s.Play();s.Draw();s.Play();s.Contest(0,Qiao,1);s.Zongshi();var r=s.E<PindianResultDeterminedEvent>().Last().Result;var viewed=s.Root().BoundaryJianYongZongshi!.TopCardId!.Value;
        s.Need(r.SourceRank==r.OpponentRank&&s.P!.Choices.Where(c=>Branch(c)=="contest").SelectMany(c=>c.Cards).Order().SequenceEqual(new[]{r.SourceCardId,r.OpponentCardId}.Order())&&s.G.CreateSnapshot(1).PendingDecision is null,"A tied actual contest offers both original entities while the viewed deck top remains chooser-private.");
        s.Cold();s.Answer(c=>Branch(c)=="top");s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);s.Need(s.V(0).Hand.Any(c=>c.Id==viewed)&&s.Root().BoundaryJianYongZongshi is{From.Zone:CardZoneKind.DrawPile,Choice:1},"Choosing the private top actually obtains that frozen top entity and leaves both contest entities for native cleanup.");s.Cold();s.Answer(_=>true);s.Play();
        s.Need(s.G.GetLegalActions(0).All(a=>a.Kind is not(LegalActionKind.Duel or LegalActionKind.Lightning or LegalActionKind.DrawTwo))&&s.E<BoundaryJianYongPhaseEvent>().Single(e=>!e.Won).Actor==0,"Not winning, including a tie, bans all trick uses for the current phase without granting any next-use adjustment.");
        var turn=s.G.CreateSnapshot(0).TurnNumber;s.Accept(new EndPlayPhaseCommand(0,s.G.Revision,s.P!.PromptId));s.Play();s.Need(s.G.CreateSnapshot(0).TurnNumber==turn&&s.G.GetLegalActions(0).Any(a=>a.Kind==LegalActionKind.Duel),"The failed contest's trick prohibition expires at the actual inserted-Play boundary, before the same turn's normal Play.");
        s.Accept(new EndPlayPhaseCommand(0,s.G.Revision,s.P!.PromptId));s.Reach(()=>s.P is{Kind:DecisionKind.PlayCard,PlayerSeat:1});s.Contest(1,Driver,0);s.Zongshi();var incoming=s.E<PindianResultDeterminedEvent>().Last().Result;
        s.Need(incoming.SourceSeat==1&&incoming.OpponentSeat==0&&s.Root().OwnerSeat==0&&s.P!.Choices.Count(c=>Branch(c)=="contest")==2,"Zongshi belongs to every real Pindian participant, including when another actor initiated the contest; it is independent of Qiaoshui's own activation.");
        s.Cold();s.Answer(c=>Branch(c)=="contest"&&c.Cards.SequenceEqual([incoming.SourceCardId]));s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);s.Cold();s.Answer(_=>true);s.Reach(()=>s.P is{Kind:DecisionKind.PlayCard,PlayerSeat:1});
        s.Need(s.V(0).Hand.Any(c=>c.Id==incoming.SourceCardId)&&s.G.CardMovements.Count(m=>m.CardId==incoming.SourceCardId&&m.From==CardLocation.Processing&&m.To==CardLocation.Hand(0)&&m.Reason.Value=="skill-program.boundary:zongshih-mobile.obtain")==1,"The opponent-owned passive trigger returns the exact chosen tied entity once after its movement child and cold restore.");
    }
    public static void SimultaneousComparisonsReuseOriginalGroupAndPaidChoiceCold()
    {
        var s=Scene.Start();s.Play();var material=s.V(0).Hand.First().Id;
        s.Accept(new UseProgramSkillCommand(0,Driver,"multi",[material],[1,2],s.G.Revision,s.P!.PromptId));s.Zongshi();
        var first=s.Root().BoundaryJianYongZongshi!;var group=s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.SimultaneousPindian is not null);var groupId=group.Id;
        var pair=s.E<PindianResultDeterminedEvent>().Last().Result;var original=s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.Id==groupId).SimultaneousPindian!.Cards.Select(c=>c.CardId).ToArray();
        s.Need(first.ComparisonIndex==0&&first.PindianId==groupId&&pair.OpponentSeat==1&&s.P!.Choices.Where(c=>Branch(c)=="contest").Single().Cards.SequenceEqual([pair.OpponentCardId]),"The first actual simultaneous comparison issues Zongshi only for its real two participants and lower final-rank entity, retaining the original group frame.");
        s.Cold();s.Answer(c=>Branch(c)=="contest");s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);s.Cold();s.Answer(_=>true);s.Zongshi();
        var second=s.Root().BoundaryJianYongZongshi!;var top=second.TopCardId!.Value;
        s.Need(second.ComparisonIndex==1&&second.PindianId==groupId&&s.E<PindianResultDeterminedEvent>().Last().Result.OpponentSeat==2&&second.TopCardId!=first.TopCardId,
            "Returning the paid first choice advances the original comparison cursor once; native consequence draw changes the deck before the second comparison's new private peek.");
        s.Cold();s.Answer(c=>Branch(c)=="top");s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);s.Cold();s.Answer(_=>true);s.Play();
        s.Need(s.E<SimultaneousPindianCompletedEvent>().Single() is{CompletedComparisons:2}&&s.E<BoundaryJianYongZongshiPaidEvent>().Where(e=>e.PindianId==groupId).Select(e=>e.ComparisonIndex).SequenceEqual([0,1])&&
            s.V(0).Hand.Any(c=>c.Id==pair.OpponentCardId)&&s.V(0).Hand.Any(c=>c.Id==top)&&original.All(id=>s.G.CardMovements.Count(m=>m.CardId==id&&m.To==CardLocation.Processing&&m.Reason==CardMoveReasons.PindianReveal)==1),
            "Both comparisons finish on the same actual simultaneous group, each original commitment pays once, and cold movement returns never repeat either claim or private top Obtain.");
    }
    private static string? Branch(PromptChoice c)=>c.Parameters.GetValueOrDefault("branch");
    private sealed class Scene(ContentRegistry registry,GameEngine game)
    {
        public GameEngine G{get;private set;}=game;public PendingDecision? P=>Enumerable.Range(0,5).Select(i=>G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p=>p is not null);
        public PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat).Players.Single(p=>p.Seat==seat);public T[] E<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
        public ProgramSkillFrame Root()=>G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.SkillId==Zong);
        public void Play()=>Reach(()=>P is{Kind:DecisionKind.PlayCard,PlayerSeat:0});public void Draw()=>Accept(new UseProgramSkillCommand(0,Driver,"draw",[],[],G.Revision,P!.PromptId));
        public void Contest(int actor,string skill,int target)=>Accept(new UseProgramSkillCommand(actor,skill,"contest",[V(actor).Hand.First().Id],[target],G.Revision,P!.PromptId));
        public void Zongshi()=>Reach(()=>P?.SkillPrompt?.SkillId==Zong&&P.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="boundary-jian-yong-zongshi"));
        public void PlayKind(LegalActionKind kind)=>PlayAction(G.GetLegalActions(0).First(a=>a.Kind==kind&&a.ProgramActivationId is null));
        public void PlayAction(LegalAction a)=>Accept(new PlayCardCommand(0,a.CardId!.Value,a.TargetSeats,G.Revision,P!.PromptId,a.PlayedCardKind,a.TargetCardId){ConversionSource=a.ConversionSource,AdditionalConversionSources=a.AdditionalConversionSources});
        public void Answer(Func<PromptChoice,bool> predicate){var p=P!;Accept(new AnswerPromptCommand(p.PlayerSeat,p.PromptId,p.Choices.First(predicate).Id,G.Revision));}
        public void Accept(GameCommand c){var r=G.Submit(c);Need(r.Accepted,r.Error?.Message??"Rejected actual mobile Jian Yong command.");}
        public void Reach(Func<bool> done)
        {
            for(var i=0;i<260&&!done();i++)
            {
                if(P is{Kind:DecisionKind.SelectGeneral}p){Accept(new SelectGeneralCommand(p.PlayerSeat,p.PlayerSeat==0?Owner:"fixture:mobile-jy-peer-"+p.PlayerSeat,G.Revision,p.PromptId));continue;}
                if(P is{Kind:DecisionKind.PlayCard,PlayerSeat:not 0}foreign){Accept(new EndPlayPhaseCommand(foreign.PlayerSeat,G.Revision,foreign.PromptId));continue;}
                if(P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("action") is "pindian-card" or "pindian-source-card" or "pindian-opponent-card")==true){Answer(c=>c.Cards.Count==1);continue;}
                if(P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="simultaneous-pindian")==true){Answer(c=>c.Parameters.GetValueOrDefault("branch") is "commit" or "draw" or "claim-decline");continue;}
                if(P?.SkillPrompt?.SkillId==Watch){Answer(_=>true);continue;}
                if(P is{Kind:DecisionKind.RespondDodge}){Answer(c=>c.Parameters.GetValueOrDefault("response")=="take-damage");continue;}
                if(P is{Kind:DecisionKind.Nullification}){Answer(c=>c.Cards.Count==0);continue;}
                if(P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="skip")==true){Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="skip");continue;}
                if(P is{Kind:DecisionKind.ProgramTrigger})throw new InvalidOperationException("Unexpected mobile Jian Yong prompt "+JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(),"The bounded actual mobile Jian Yong scene missed its expected native boundary.");
        }
        public void Cold(){var before=Dump();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),registry);Need(before==Dump(),"Cold preserves the original private peek, native final ranks, actual phase and once-paid movement child.");}
        private string Dump()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,5).Select(i=>SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),G.CardMovements});
        public void Need(bool ok,string reason){if(!ok)throw new InvalidOperationException(reason+" "+JsonSerializer.Serialize(new{P,Frames=G.ResolutionStack.Select(f=>new{f.Id,f.Kind}),Players=Enumerable.Range(0,5).Select(V).Select(p=>new{p.Seat,p.Hp,p.HandCount})}));}
        public static Scene Start(bool win=true,bool extra=false){var r=ContentRegistry.Build(new StandardContentPackage(),new Fixture(win,extra));var g=GameEngine.CreateStandard(new(){Seed=31,PlayerCount=5,HumanSeat=0,HumanSeats=[0,1,2,3,4],HumanRole=Role.Lord,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=12},r);var s=new Scene(r,g);s.Accept(new StartGameCommand());return s;}
    }
    private sealed class Fixture(bool win,bool extra):IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-mobile-jian-yong",new(1,0,0));
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,Hero);
            var rules=$$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
             {"id":"{{Driver}}","revision":1,"activations":[{"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":20}]},{"id":"contest","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":1,"maxTargets":1,"targetKind":"otherLivingWithHand","usesPerTurn":null,"effects":[{"op":"pindian","target":"selectedTarget","amount":1}]},{"id":"multi","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":1,"maxTargets":3,"targetKind":"otherLivingWithHand","usesPerTurn":null,"usesPerPhase":1,"effects":[{"op":"startSimultaneousPindian","target":"owner"}]}]},
             {"id":"{{Watch}}","revision":1,"triggers":[{"id":"native-obtain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:zongshih-mobile.obtain"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]},
             {"id":"{{Rank}}","revision":1,"cardPolicies":[{"id":"actual-heart-thirteen","kind":"pindianRankBySuit","inputSuit":"heart","value":13}]},
             {"id":"{{Extra}}","revision":1,"triggers":[{"id":"one-extra-play","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"insertPhase","target":"owner","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]}
            ]}
            """;
            var presentation=JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=new Dictionary<string,object>{[Driver]=new{name="真实摸牌拼点",description="原生固定实体命令"},[Watch]=new{name="实际获得孩子",description="真实原帧返回",optionLabels=new Dictionary<string,string>{{"continue","继续"}}},[Rank]=new{name="固定有效点数",description="真实红桃拼点13"},[Extra]=new{name="实际插入出牌",description="同一回合不同阶段"}}});
            var c=SkillProgramCatalog.Load(rules,presentation);foreach(var(id,p)in c.Programs)b.AddSkill(new(id,c.Presentations[id].Name,c.Presentations[id].Description){Program=p,ProgramPresentation=c.Presentations[id]});
            var skills=new List<string>{Zong,Driver,Watch};if(win)skills.Add(Rank);if(extra)skills.Add(Extra);b.AddGeneral(new(Owner,"移动界简雍实际身份","supporter",Qiao,"shu",3,AdditionalSkillIds:skills,Gender:GeneralGender.Male));
            var peers=Enumerable.Range(1,4).Select(i=>"fixture:mobile-jy-peer-"+i).ToArray();foreach(var peer in peers)b.AddGeneral(new(peer,"固定真实角色","supporter",Driver,"wu",8,Gender:GeneralGender.Male));
            const string deck="fixture:mobile-jy-deck";var kinds=new[]{"standard:slash","standard:crossbow","standard:lightning","standard:duel","standard:draw_two","standard:nullification"};b.AddDeck(new(deck,"真实混合实体",4,2,[]){PhysicalCards=Enumerable.Range(0,180).Select(i=>new ContentDeckPhysicalCard(kinds[i%kinds.Length],Suit.Heart,1)).ToArray()});
            b.AddMode(new(Mode,"移动界简雍原生命令",5,5,new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Loyalist),1},{nameof(Role.Rebel),2},{nameof(Role.Renegade),1}},deck,GeneralCandidateCount:5,GeneralPoolIds:[Owner,..peers]));
        }
    }
}
