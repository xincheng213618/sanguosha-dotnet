using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryLiuShanMobileChecks
{
    private const string Hero="boundary:liu-shan-mobile",Xiangle="boundary:xiangle-mobile",Fangquan="boundary:fangquan-mobile",Ruoyu="boundary:ruoyu-mobile",
        Owner="fixture:boundary-liu-shan-owner",Driver="fixture:boundary-liu-shan-driver",Prime="fixture:boundary-liu-shan-prime",
        Adjust="fixture:boundary-liu-shan-adjust",Watch="fixture:boundary-liu-shan-paid",Defense="fixture:boundary-liu-shan-defense",Mode="identity:classic-boundary-liu-shan";
    internal static (ContentRegistry Registry,GameEngine Game) CreateScene(bool singleHuman=false)
    {var s=Scene.Start("fangquan",singleHuman);s.Offer(Fangquan);return(s.Registry,s.G);}

    public static void NativeXiangleBasicPaymentAndOriginalSlashCold()
    {
        var s=Scene.Start("xiangle");s.Play(0);var hp=s.V(1).Hp;var no=s.Slash(0,1);s.Option("decline");s.Play(0);
        s.Need(s.V(1).Hp==hp&&s.E<ProgramCardEffectNullifiedEvent>().Any(e=>e.SkillId==Xiangle)&&
            s.E<CardUseFinishedEvent>().Count(e=>e.ResolutionId==no)==1&&s.E<DamageAppliedEvent>().Length==0&&s.E<CardRespondedEvent>().Length==0,
            "Declining the basic cost nullifies only the target effect of one real finished native Slash, without manufacturing a Dodge or damage.");
        var yes=s.Slash(0,1);s.Option("pay");s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="select-and-move-owned-card")==true);
        var card=s.P!.Choices.First(c=>c.Cards.Count==1).Cards.Single();s.Answer(c=>c.Cards.SequenceEqual([card]));s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);
        s.Need(s.V(0).Hand.All(c=>c.Id!=card)&&s.V(1).Hp==hp&&s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.SkillId==Xiangle).OwnerSeat==1,
            "The actual Slash actor pays a real basic hand entity while the original target-owned Xiangle frame waits for its payment child before any response or damage.");
        s.Cold();s.Answer(_=>true);s.Reach(()=>s.P is{Kind:DecisionKind.RespondDodge,PlayerSeat:1});s.Answer(c=>c.Parameters.GetValueOrDefault("response")=="take-damage");s.Play(0);
        s.Need(s.V(1).Hp==hp-1&&s.E<DamageAppliedEvent>() is[{SourceSeat:0,TargetSeat:1,Amount:1}]&&
            s.E<CardUseFinishedEvent>().Count(e=>e.ResolutionId==yes)==1&&s.G.CardMovements.Count(m=>m.CardId==card&&m.From==CardLocation.Hand(0)&&m.To==CardLocation.DiscardPile&&
                m.Reason.Value=="skill-program."+Xiangle+".SelectAndMoveOwnedCard")==1,
            "Cold returning the paid child restores a genuine native Dodge response and one actual damage without repeating its real basic cost.");
        s.End();s.Play(1);s.Need(s.V(1).MaxHp==3&&s.V(1).Hp==hp-1&&s.Limit(1)==hp-1&&s.V(1).Skills!.All(k=>k.Id!="classic:jijiang")&&
            s.E<ProgramBindingStartedEvent>().All(e=>e.SkillId!=Ruoyu),
            "Even at its own real preparation with strictly minimum current HP, the nonlord Liu Shan never awakens or acquires the lord request skill.");
    }

    public static void FangquanDynamicMaximumHandLimitPaidExtraTurnCold()
    {
        var s=Scene.Start("fangquan");s.Offer(Fangquan);s.Invoke();s.Reach(()=>s.P?.SkillPrompt?.SkillId==Adjust);
        s.Need(s.V(0).Hp==2&&s.V(0).MaxHp==4&&s.Limit(0)==4,"A real skipped Play declaration immediately changes the wounded owner's native hand limit to current maximum HP.");
        s.Cold();s.Answer(_=>true);s.Reach(()=>s.P?.SkillPrompt?.SkillId==Adjust&&s.P.Choices.Any(c=>c.Parameters.GetValueOrDefault("result-bind")=="after-change"));
        s.Need(s.V(0).Hp==1&&s.V(0).MaxHp==5&&s.Limit(0)==5,"A later real maximum-HP change updates the active hand limit, while losing HP never reduces it to the current HP or a frozen activation value.");
        s.Answer(_=>true);s.Offer(Fangquan);
        s.Need(s.V(0).HandCount==5&&s.E<PhaseChangedEvent>().All(e=>e.ActorSeat!=0||e.Phase!=TurnPhase.Play),
            "The actual discard phase retains five real hand cards at HP1 after the real Play skip, before the separate optional end-of-turn payment.");
        s.Invoke();s.Reach(()=>s.P?.Choices.Any(c=>c.Cards.Count==1)==true);var card=s.P!.Choices.First(c=>c.Cards.Count==1).Cards.Single();s.Answer(c=>c.Cards.SequenceEqual([card]));
        s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);s.Need(s.V(0).HandCount==4&&s.E<ProgramExtraTurnPendedEvent>().Length==0,
            "The exact end-turn hand cost is already discarded while its original paid frame awaits the native movement child, before selecting or queuing a beneficiary.");
        s.Cold();s.Answer(_=>true);s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="select-target")==true);s.Answer(c=>c.Targets.SequenceEqual([2]));
        s.Reach(()=>s.E<ProgramExtraTurnPendedEvent>().Any(e=>e.SkillId==Fangquan));var receipt=s.E<ProgramExtraTurnPendedEvent>().Single(e=>e.SkillId==Fangquan);s.Cold();s.Play(2);
        s.Need(receipt.Seat==2&&s.Limit(0)==1&&s.V(2).HandCount==6,"The native extra turn starts for the selected other role with its real normal draw, and the source's temporary hand-limit effect expires on the new actual turn.");
        s.End();s.Play(1);
        var after=s.G.Events.SkipWhile(e=>e.Payload is not ProgramExtraTurnPendedEvent p||p.FrameId!=receipt.FrameId).Select(e=>e.Payload).ToArray();
        s.Need(after.OfType<TurnStartedEvent>().Take(2).Select(e=>e.ActorSeat).SequenceEqual([2,1])&&
            after.OfType<CardsDrawnEvent>().Count(e=>e.PlayerSeat==2&&e.Count==2)==1&&
            new[]{TurnPhase.Play,TurnPhase.Discard}.All(phase=>after.OfType<PhaseChangedEvent>().Any(e=>e.ActorSeat==2&&e.Phase==phase))&&
            after.OfType<TurnEndedEvent>().Count(e=>e.ActorSeat==2)==1&&
            s.G.CardMovements.Count(m=>m.CardId==card&&m.Reason.Value=="skill-program."+Fangquan+".MoveBoundCards")==1,
            "The selected beneficiary executes a complete native turn and then resumes the saved natural seat; cold recovery never repeats the original exact cost.");
    }

    public static void LordRuoyuGrantsRealJijiangUseAndResponseCold()
    {
        var s=Scene.Start("ruoyu");s.Play(0);
        s.Need(s.V(0).Hp==4&&s.V(0).MaxHp==5&&s.V(0).Skills!.Any(k=>k.Id=="classic:jijiang")&&
            s.E<SkillsAcquiredEvent>().Count(e=>e.SourceSkillId==Ruoyu&&e.SkillIds.Contains("classic:jijiang"))==1&&s.E<MaximumHpChangedEvent>().Count(e=>e.PlayerSeat==0&&e.SkillId==Ruoyu&&e.Delta==1)==1,
            "The real lord ties the minimum HP3 at preparation, awakens once through native maximum HP and recovery, and gains the actual shared Jijiang skill instance.");
        var request=s.G.GetLegalActions(0).Single(a=>a.Kind==LegalActionKind.UseProgramSkill&&a.ProgramSkillId=="classic:jijiang");
        s.Accept(new UseProgramSkillCommand(0,"classic:jijiang",request.ProgramActivationId!,[],[3],s.G.Revision,s.P!.PromptId));s.Reach(()=>s.P is{Kind:DecisionKind.RespondSlash,PlayerSeat:1});
        var supply=s.P!.Choices.First(c=>c.Cards.Count==1).Cards.Single();s.Cold();s.Answer(c=>c.Cards.SequenceEqual([supply]));s.Reach(()=>s.P is{Kind:DecisionKind.RespondDodge,PlayerSeat:3});
        var use=s.G.ResolutionStack.OfType<CardUseFrame>().Last(u=>u.CardKind==CardKind.Slash);
        s.Need(use.Action is{ActorSeat:0,ProviderSeat:1,RequesterSeat:0}a&&a.PhysicalCards is[{CardId:var actual}]&&actual==supply,
            "The newly acquired native lord request really uses the other Shu role's original Slash entity with separate actual actor, provider and requester.");
        s.Answer(c=>c.Parameters.GetValueOrDefault("response")=="take-damage");s.Play(0);s.End();s.Play(1);s.Duel(1,0);
        s.Reach(()=>s.P is{Kind:DecisionKind.RespondSlash,PlayerSeat:0});s.Answer(c=>c.Parameters.GetValueOrDefault("response")=="faction-slash-request");
        s.Reach(()=>s.P is{Kind:DecisionKind.RespondSlash,PlayerSeat:1});var response=s.P!.Choices.First(c=>c.Cards.Count==1).Cards.Single();s.Cold();s.Answer(c=>c.Cards.SequenceEqual([response]));
        s.Reach(()=>s.P is{Kind:DecisionKind.RespondSlash,PlayerSeat:1});s.Answer(c=>c.Parameters.GetValueOrDefault("response")=="take-damage");s.Play(1);
        s.Need(s.E<CardRespondedEvent>().Any(e=>e.ResponderSeat==0&&e.EffectiveCardKind==CardKind.Slash)&&
            s.G.CardMovements.Count(m=>m.CardId==response&&m.From==CardLocation.Hand(1)&&m.To==CardLocation.Processing)==1&&
            s.E<SkillsAcquiredEvent>().Count(e=>e.SourceSkillId==Ruoyu)==1&&s.E<DamageAppliedEvent>().Any(e=>e.SourceSeat==0&&e.TargetSeat==1),
            "The awakened shared lord skill also really supplies a native Duel Slash response, returns to the original Duel, and never duplicates awakening or provider material after cold restore.");
    }

    private sealed class Scene(ContentRegistry registry,GameEngine game,int heroSeat)
    {
        public ContentRegistry Registry=>registry;public GameEngine G{get;private set;}=game;
        public PendingDecision? P=>Enumerable.Range(0,5).Select(i=>G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p=>p is not null);
        public PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat).Players.Single(p=>p.Seat==seat);
        public T[] E<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
        public int Limit(int seat){const BindingFlags flags=BindingFlags.Instance|BindingFlags.NonPublic;var players=(IReadOnlyList<CharacterState>)typeof(GameEngine).GetField("_players",flags)!.GetValue(G)!;return(int)typeof(GameEngine).GetMethod("GetHandLimit",flags)!.Invoke(G,[players[seat]])!;}
        public void Play(int seat)=>Reach(()=>P is{Kind:DecisionKind.PlayCard}&&P.PlayerSeat==seat);
        public void End()=>Accept(new EndPlayPhaseCommand(P!.PlayerSeat,G.Revision,P.PromptId));
        public long Slash(int actor,int target){PlayCard(actor,target,LegalActionKind.Slash);return E<CardUseDeclaredEvent>().Last().ResolutionId;}
        public void Duel(int actor,int target)=>PlayCard(actor,target,LegalActionKind.Duel);
        private void PlayCard(int actor,int target,LegalActionKind kind)
        {var a=G.GetLegalActions(actor).First(a=>a.Kind==kind&&a.TargetSeats.SequenceEqual([target]));Accept(new PlayCardCommand(actor,a.CardId!.Value,[target],G.Revision,P!.PromptId,a.PlayedCardKind,a.TargetCardId){ConversionSource=a.ConversionSource});}
        public void Offer(string skill)=>Reach(()=>P?.SkillPrompt?.SkillId==skill&&P.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="activate"));
        public void Invoke()=>Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
        public void Option(string id){Reach(()=>P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("option-id")==id)==true);Answer(c=>c.Parameters.GetValueOrDefault("option-id")==id);}
        public void Answer(Func<PromptChoice,bool> predicate){var p=P!;Accept(new AnswerPromptCommand(p.PlayerSeat,p.PromptId,p.Choices.First(predicate).Id,G.Revision));}
        public void Accept(GameCommand command){var r=G.Submit(command);Need(r.Accepted,r.Error?.Message??"Rejected native Liu Shan fixture command.");}
        public void Reach(Func<bool> done)
        {
            for(var step=0;step<220&&!done();step++)
            {
                if(P is{Kind:DecisionKind.SelectGeneral}setup)
                {Accept(new SelectGeneralCommand(setup.PlayerSeat,setup.PlayerSeat==heroSeat?Owner:"fixture:boundary-liu-shan-peer-"+setup.PlayerSeat,G.Revision,setup.PromptId));continue;}
                if(P is{Kind:DecisionKind.ProgramTrigger}prompt&&prompt.SkillPrompt?.SkillId==Fangquan&&prompt.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="skip"))
                {Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="skip");continue;}
                if(P is{Kind:DecisionKind.ProgramTrigger or DecisionKind.RespondDodge or DecisionKind.RespondSlash})throw new InvalidOperationException("Unexpected Liu Shan fixture boundary: "+JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(),"The bounded native Liu Shan fixture missed its real expected boundary.");
        }
        public void Cold(){var before=Dump();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),registry);Need(before==Dump(),"Cold retains original native Slash/basic payment, actual skipped phase, paid end-turn cost or native acquired lord request.");}
        private string Dump()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,5).Select(i=>SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),G.CardMovements});
        public void Need(bool v,string message){if(!v)throw new InvalidOperationException(message+" "+JsonSerializer.Serialize(new{Pending=P is{}p?new{p.Kind,p.PlayerSeat,p.PromptId,Skill=p.SkillPrompt?.SkillId,ChoiceCount=p.Choices.Count}:null,Players=Enumerable.Range(0,5).Select(i=>new{Seat=i,V(i).Hp,V(i).MaxHp,V(i).HandCount})}));}
        public static Scene Start(string scenario,bool singleHuman=false)
        {var hero=scenario=="xiangle"?1:0;var registry=ContentRegistry.Build(new StandardContentPackage(),new Fixture(scenario,hero));var g=GameEngine.CreateStandard(new(){Seed=31,PlayerCount=5,HumanSeat=0,HumanSeats=singleHuman?null:[0,1,2,3,4],HumanRole=Role.Lord,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=12},registry);var s=new Scene(registry,g,hero);s.Accept(new StartGameCommand());return s;}
    }
    private sealed class Fixture(string scenario,int hero):IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-boundary-liu-shan",new(1,0,0));
        internal static string Rules(string scenario)=>$$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
         {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"native-slash-range","query":"attackRange","operation":"add","value":10,"priority":0},{"id":"native-repeat-slash","query":"slashLimit","operation":"add","value":5,"priority":0}],"viewAs":[{"id":"heart-duel","inputKinds":[],"inputSuits":["heart"],"outputKind":"duel","forPlay":true,"forResponse":false,"singleCardTrickUse":true}]},
         {"id":"{{Defense}}","revision":1,"viewAs":[{"id":"heart-dodge","inputKinds":[],"inputSuits":["heart"],"outputKind":"dodge","forPlay":false,"forResponse":true}]},
         {"id":"{{Prime}}","revision":1,"triggers":[{"id":"real-opening-wound","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"loseHp","target":"owner","amount":{{(scenario=="fangquan"?2:1)}}}]}]},
         {"id":"{{Adjust}}","revision":1,"triggers":[{"id":"real-later-maximum-and-current-hp-change","window":"afterNormalDraw","subject":"owner","optional":false,"priority":-10,"effects":[{"op":"chooseOption","target":"owner","resultBind":"before-change","options":[{"id":"continue"}]},{"op":"changeMaximumHp","target":"owner","amount":1},{"op":"loseHp","target":"owner","amount":1},{"op":"chooseOption","target":"owner","resultBind":"after-change","options":[{"id":"continue"}]}]}]},
         {"id":"{{Watch}}","revision":1,"triggers":[{"id":"paid-basic-or-turn-hand","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:xiangle-mobile.SelectAndMoveOwnedCard","skill-program.boundary:fangquan-mobile.MoveBoundCards"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
        ]}
        """;
        internal static string Presentation()=>JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=new Dictionary<string,object>{
            [Driver]=new{name="真实杀决斗输入",description="原生固定实体与范围"},[Defense]=new{name="真实闪响应",description="以实际红心实体响应闪"},[Prime]=new{name="真实开局失血",description="实际事件设置当前HP"},
            [Adjust]=new{name="回合内真实体力变更",description="原生加上限与失血",optionLabels=new Dictionary<string,string>{{"continue","继续"}}},
            [Watch]=new{name="真实付款后子窗",description="原生手牌弃置后的原帧子窗",optionLabels=new Dictionary<string,string>{{"continue","继续"}}}}});
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,Hero,"classic:liu-bei");var c=SkillProgramCatalog.Load(Rules(scenario),Presentation());foreach(var(id,p)in c.Programs)b.AddSkill(new(id,c.Presentations[id].Name,c.Presentations[id].Description){Program=p,ProgramPresentation=c.Presentations[id]});
            var own=new List<string>{Fangquan,Driver,Defense};if(scenario!="fangquan")own.Add(Ruoyu);if(scenario is"fangquan"or"ruoyu")own.Add(Prime);if(scenario=="fangquan")own.AddRange([Adjust,Watch]);
            b.AddGeneral(new(Owner,"界刘禅实际原生场景","supporter",Xiangle,"shu",3,AdditionalSkillIds:own));
            var peers=Enumerable.Range(0,5).Where(i=>i!=hero).Select(i=>"fixture:boundary-liu-shan-peer-"+i).ToArray();foreach(var p in peers)b.AddGeneral(new(p,"固定蜀实体角色","supporter",Driver,"shu",scenario=="ruoyu"?3:8,AdditionalSkillIds:scenario=="xiangle"?[Defense,Watch]:[Defense]));
            const string deck="fixture:boundary-liu-shan-deck";b.AddDeck(new(deck,"固定真实基本牌",4,2,[]){PhysicalCards=Enumerable.Range(0,120).Select(i=>new ContentDeckPhysicalCard("standard:slash",Suit.Heart,i%13+1)).ToArray()});
            b.AddMode(new(Mode,"移动界刘禅实际原生边界",5,5,new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Loyalist),1},{nameof(Role.Rebel),2},{nameof(Role.Renegade),1}},deck,GeneralCandidateCount:5,GeneralPoolIds:[Owner,..peers]));
        }
    }
}
