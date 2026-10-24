using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryYanYanMobileChecks
{
    private const string Hero="boundary:yan-yan-mobile",Skill="boundary:juzhan-mobile",Owner="fixture:boundary-yan-yan-owner",
        Driver="fixture:boundary-yan-yan-driver",Watch="fixture:boundary-yan-yan-paid",Mode="identity:classic-boundary-yan-yan";
    internal static (ContentRegistry Registry,GameEngine Game) CreateScene(bool singleHuman=false){var s=Scene.Start(singleHuman);return(s.Registry,s.G);}
    public static void StateEpochThirdSelfChengshiOriginalUseClaimCold()
    {
        var s=Scene.Start();s.Need(s.Polarity==SkillPolarity.Yin&&s.Count==0&&s.Limit()==11,"Actual initial Shu faction registers structured Yin and the normal native quota.");
        s.Equip();s.Play(0);s.Trim();s.Play(0);var first=s.Slash(0,[1,2,3]);s.Offer();s.Skip();s.Play(0);
        s.Need(s.Count==1&&s.E<BoundaryYanYanOccurrenceRecordedEvent>().Count(e=>e.Occurrence.UseId==first&&e.Occurrence.Polarity==SkillPolarity.Yin)==1,
            "One actual three-target Slash increments Yin once for its designated Use, rather than three targets.");
        s.Draw();s.Play(0);s.Slash(0,[1]);s.Offer();s.Skip();s.Play(0);
        var third=s.Slash(0,[2]);var material=s.E<CardUseDeclaredEvent>().Last().CardId;s.Offer();s.Cold();s.Invoke();s.SelectTarget(2);s.SelectOwned();
        s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);
        var paid=s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.SkillId==Skill);
        s.Need(paid.BoundaryYanYan is{Occurrence:{Ordinal:3,Polarity:SkillPolarity.Yin}}&&s.Polarity==SkillPolarity.Yang&&
            s.E<BoundaryYanYanChengshiGrantedEvent>().Length==0,"The original third-self frame commits Yin once and waits for its actual paid obtain child before the bonus.");
        s.Cold();s.Answer(_=>true);s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);
        s.Need(s.E<BoundaryYanYanChengshiGrantedEvent>() is[{UseId:var granted}]&&granted==third&&s.V(0).Hand.Any(c=>c.Id==material)&&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.SkillId==Skill).BoundaryYanYanClaim is{Cards.Count:1},
            "Self and exactly third together issue persistent quota and obtain the original Slash only after its real whole Use completed.");
        s.Cold();s.Answer(_=>true);s.Play(0);
        s.Need(s.Limit()==12&&s.Bonus==1&&s.E<BoundaryYanYanChengshiGrantedEvent>().Length==1&&
            s.E<CardUseFinishedEvent>().Count(e=>e.ResolutionId==third)==1&&
            s.G.CardMovements.Count(m=>m.CardId==material&&m.To==CardLocation.Hand(0)&&m.Reason.Value=="skill.boundary-yan-yan.chengshi")==1&&
            !s.CanSlash(0,2)&&s.CanSlash(0,1),"Cold returns each already-paid child without paying, converting, increasing quota or claiming twice; Yin prohibits only its actual selected target.");
        s.End();s.Play(1);s.Need(s.Bonus==1&&s.Limit()==12,"The official undelimited extra Slash allowance remains after the original turn ends.");s.Cold();
    }

    public static void YangTargetOccurrencesAndDirectedTurnBanCold()
    {
        var s=Scene.Start();s.EnterYang();s.Slash(0,[2]);s.Play(0);s.Slash(0,[3]);s.Offer();
        s.Need(s.Count==3&&s.Polarity==SkillPolarity.Yang,"The actual third target occurrence globally offers Yang even when the owner is not that target.");
        s.Skip();s.Play(0);var preparations=s.E<BoundaryYanYanPreparedEvent>().Length;s.Slash(0,[4]);s.Play(0);
        s.Need(s.Count==4&&s.E<BoundaryYanYanPreparedEvent>().Length==preparations&&s.Bonus==0,
            "Declining exactly third keeps Yang and fourth nonself has no new offer; the count is not a modulo-three clock.");
        s.End();s.Play(1);var hp=s.V(0).Hp;var ownerHand=s.V(0).HandCount;var actorHand=s.V(1).HandCount;
        s.Slash(1,[0]);s.Offer();s.Cold();s.Invoke();s.Play(1);
        s.Need(s.Polarity==SkillPolarity.Yin&&s.Count==0&&s.Bonus==0&&s.V(0).HandCount==ownerHand+1&&s.V(1).HandCount==actorHand&&s.V(0).Hp==hp-1&&
            !s.CanSlash(1,0)&&s.CanSlash(1,2),"A later actual self target can invoke outside third, draws one for each participant, and prohibits only that source's cards toward the owner for the current turn.");
        s.Cold();s.End();s.Play(2);s.End();s.Play(3);s.End();s.Play(4);s.End();s.Play(0);s.End();s.Play(1);
        s.Need(s.CanSlash(1,0),"The directed prohibition expires with its issuing turn rather than permanently banning the source.");

        var both=Scene.Start();both.EnterYang();both.Slash(0,[2]);both.Play(0);both.End();both.Play(1);
        var use=both.Slash(1,[0]);var card=both.E<CardUseDeclaredEvent>().Last().CardId;both.Offer();both.Invoke();
        both.Reach(()=>both.P?.SkillPrompt?.SkillId==Watch);both.Cold();both.Answer(_=>true);both.Play(1);
        both.Need(both.Bonus==1&&both.Limit()==12&&both.E<BoundaryYanYanChengshiGrantedEvent>() is[{UseId:var exact}]&&exact==use&&
            both.V(0).Hand.Any(c=>c.Id==card)&&both.G.CardMovements.Count(m=>m.CardId==card&&m.To==CardLocation.Hand(0)&&m.Reason.Value=="skill.boundary-yan-yan.chengshi")==1,
            "Yang's simultaneous self-and-third branch also grants quota and, after full native resolution, obtains the opponent's original physical Slash exactly once.");
    }

    private sealed class Scene(ContentRegistry registry,GameEngine game)
    {
        public ContentRegistry Registry=>registry;
        public GameEngine G{get;private set;}=game;
        public PendingDecision? P=>Enumerable.Range(0,5).Select(i=>G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p=>p is not null);
        public PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat).Players.Single(p=>p.Seat==seat);
        public T[] E<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
        public SkillRuntimeStateSnapshot State=>V(0).SkillRuntimeStates!.Single(s=>s.SkillId==Skill);
        public SkillPolarity? Polarity=>State.Polarity;
        public int Count=>State.PublicRuleStates!.Single(s=>s.StateId=="juzhan-state-count").Value;
        public int Bonus=>State.PublicRuleStates!.Single(s=>s.StateId=="juzhan-permanent-slash").Value;
        public int Limit()
        {const BindingFlags flags=BindingFlags.Instance|BindingFlags.NonPublic;var players=(IReadOnlyList<CharacterState>)typeof(GameEngine).GetField("_players",flags)!.GetValue(G)!;return(int)typeof(GameEngine).GetMethod("GetSlashUseLimit",flags)!.Invoke(G,[players[0],null,null])!;}
        public void Play(int actor)=>Reach(()=>P is{Kind:DecisionKind.PlayCard}&&P.PlayerSeat==actor);
        public void End()=>Accept(new EndPlayPhaseCommand(P!.PlayerSeat,G.Revision,P.PromptId));
        public void Draw()=>Accept(new UseProgramSkillCommand(0,Driver,"draw",[],[],G.Revision,P!.PromptId));
        public void Trim()=>Accept(new UseProgramSkillCommand(0,Driver,"trim",V(0).Hand.Take(V(0).HandCount-1).Select(c=>c.Id).ToArray(),[],G.Revision,P!.PromptId));
        public void Equip()
        {var a=G.GetLegalActions(0).First(a=>a.Kind==LegalActionKind.Equip);Accept(new PlayCardCommand(0,a.CardId!.Value,a.TargetSeats,G.Revision,P!.PromptId,a.PlayedCardKind,a.TargetCardId));}
        public bool CanSlash(int actor,int target)=>G.GetLegalActions(actor).Any(a=>a.Kind==LegalActionKind.Slash&&a.TargetSeats.SequenceEqual([target]));
        public long Slash(int actor,int[] targets)
        {var a=G.GetLegalActions(actor).First(a=>a.Kind==LegalActionKind.Slash&&a.TargetSeats.SequenceEqual(targets));Accept(new PlayCardCommand(actor,a.CardId!.Value,targets,G.Revision,P!.PromptId,a.PlayedCardKind,a.TargetCardId){ConversionSource=a.ConversionSource,AdditionalConversionSources=a.AdditionalConversionSources});return E<CardUseDeclaredEvent>().Last().ResolutionId;}
        public void Offer()=>Reach(()=>P?.SkillPrompt?.SkillId==Skill&&P.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="activate"));
        public void Invoke()=>Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
        public void Skip()=>Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="skip");
        public void SelectTarget(int target)
        {Reach(()=>P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="select-target")==true);Answer(c=>c.Targets.SequenceEqual([target]));}
        public void SelectOwned()
        {Reach(()=>P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="select-and-move-owned-card"&&c.Parameters.ContainsKey("source-zone"))==true);
            Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="select-and-move-owned-card"&&c.Parameters.ContainsKey("source-zone"));}
        public void EnterYang()
        {Slash(0,[1]);Offer();Invoke();SelectTarget(1);SelectOwned();Reach(()=>P?.SkillPrompt?.SkillId==Watch);Answer(_=>true);Play(0);Draw();Play(0);Need(Polarity==SkillPolarity.Yang&&Count==1&&Bonus==0,"A real first-self Yin invocation establishes Yang; its same Slash contributes one actual target occurrence without Chengshi.");}
        public void Answer(Func<PromptChoice,bool> predicate)
        {var p=P!;Accept(new AnswerPromptCommand(p.PlayerSeat,p.PromptId,p.Choices.First(predicate).Id,G.Revision));}
        public void Accept(GameCommand command){var r=G.Submit(command);Need(r.Accepted,r.Error?.Message??"Rejected original Yan Yan fixture command.");}
        public void Reach(Func<bool> done)
        {
            for(var step=0;step<240&&!done();step++)
            {
                if(P is{Kind:DecisionKind.SelectGeneral}setup)
                {Accept(new SelectGeneralCommand(setup.PlayerSeat,setup.PlayerSeat==0?Owner:setup.ValidContentIds.First(),G.Revision,setup.PromptId));continue;}
                if(P is{Kind:DecisionKind.RespondDodge}){Answer(c=>c.Parameters.GetValueOrDefault("response")=="take-damage");continue;}
                if(P is{Kind:DecisionKind.ProgramTrigger})throw new InvalidOperationException("Unexpected real Yan Yan boundary: "+JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(),"The bounded real Yan Yan fixture missed its expected boundary.");
        }
        public void Cold()
        {var before=Dump();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),registry);Need(before==Dump(),"Cold preserves original occurrence, state epoch, paid movement and completed Use claim on their original frames.");}
        private string Dump()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,5).Select(i=>SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),G.CardMovements});
        public void Need(bool value,string message){if(!value)throw new InvalidOperationException(message+" "+JsonSerializer.Serialize(new{P,Players=Enumerable.Range(0,5).Select(i=>new{Seat=i,V(i).Hp,V(i).HandCount})}));}
        public static Scene Start(bool singleHuman=false)
        {var registry=ContentRegistry.Build(new StandardContentPackage(),new Fixture());var game=GameEngine.CreateStandard(new(){Seed=31,PlayerCount=5,HumanSeat=0,HumanSeats=singleHuman?null:[0,1,2,3,4],HumanRole=Role.Lord,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=20},registry);var s=new Scene(registry,game);s.Accept(new StartGameCommand());s.Play(0);return s;}
    }
    private sealed class Fixture:IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-boundary-yan-yan",new(1,0,0));
        internal static string Rules()=>$$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
        {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"native-multiple-use","query":"slashLimit","operation":"add","value":10,"priority":0},{"id":"native-distance","query":"attackRange","operation":"add","value":10,"priority":0}],
         "viewAs":[{"id":"heart-as-slash","inputKinds":[],"inputSuits":["heart"],"outputKind":"slash","forPlay":true,"forResponse":true}],
         "activations":[{"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":4}]},
         {"id":"trim","minCards":1,"maxCards":20,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"trim"},{"op":"moveBoundCards","target":"owner","sourceBind":"trim","destination":"discardPile","awaitMovementTriggers":true}]}]},
        {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-original-entity","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:juzhan-mobile.SelectAndMoveOwnedCard","skill.boundary-yan-yan.chengshi"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"continue","options":[{"id":"continue"}]}]}]}
        ]}
        """;
        internal static string Presentation()=>JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=new Dictionary<string,object>{
            [Driver]=new{name="真实杀输入",description="原生实体转换杀、阶段次数和固定距离"},[Watch]=new{name="已支付实体移动",description="取得真实实体后的原帧子窗",optionLabels=new Dictionary<string,string>{{"continue","继续"}}}}});
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,Hero);var c=SkillProgramCatalog.Load(Rules(),Presentation());
            foreach(var(id,p)in c.Programs)b.AddSkill(new(id,c.Presentations[id].Name,c.Presentations[id].Description){Program=p,ProgramPresentation=c.Presentations[id]});
            b.AddGeneral(new(Owner,"严颜真实状态拥有者","supporter",Skill,"shu",4,AdditionalSkillIds:[Driver,Watch]));
            var peers=Enumerable.Range(1,4).Select(i=>"fixture:boundary-yan-yan-peer-"+i).ToArray();foreach(var p in peers)b.AddGeneral(new(p,"固定实体角色","supporter",Driver,"wei",12));
            const string weapon="fixture:boundary-yan-yan-halberd",deck="fixture:boundary-yan-yan-deck";var card=CardCatalog.Get(CardKind.FangtianHalberd);
            b.AddCard(new(weapon,card.DisplayName,card.CategoryName,card.Description,CardKind.FangtianHalberd));
            b.AddDeck(new(deck,"固定真实红色装备实体",4,2,[]){PhysicalCards=Enumerable.Range(0,160).Select(i=>new ContentDeckPhysicalCard(weapon,Suit.Heart,i%13+1)).ToArray()});
            b.AddMode(new(Mode,"界严颜实际状态与乘势",5,5,new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Loyalist),1},{nameof(Role.Rebel),2},{nameof(Role.Renegade),1}},deck,GeneralCandidateCount:5,GeneralPoolIds:[Owner,..peers]));
        }
    }
}
