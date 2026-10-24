using System.Reflection;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class BoundaryPanZhangMaZhongMobileChecks
{
    private const string Hero="boundary:pan-zhang-ma-zhong-mobile",Owner="fixture:boundary-pan-ma-owner",Driver="fixture:boundary-pan-ma-driver",
        Defense="fixture:boundary-pan-ma-defense",Watch="fixture:boundary-pan-ma-paid",Duodao="boundary:duodao-mobile",Anjian="boundary:anjian-mobile",Mode="identity:classic-boundary-pan-ma";
    public static void MandatoryPerTargetChoicesUseRealMultiSlashAndCold()
    {
        var s=Scene.Start();s.Equip(0);s.Play();s.Trim();s.Play();
        s.Need(s.G.GetCombatDistance(0,2)==2&&s.G.GetCombatDistance(0,3)==2&&s.V(0).HandCount==1,"The real last hand entity and Fangtian weapon authorize a native three-target Slash with two targets outside their range.");
        var first=s.Slash([1,2,3]);s.Choice(2);
        s.Need(s.P!.Choices.Count==2&&s.P.Choices.All(c=>c.Parameters.GetValueOrDefault("option-id") is "no-response" or "damage"),
            "Locked Anjian offers the actor a mandatory choice for the eligible actual target, without a decline branch.");
        s.Cold();s.Option("no-response");s.Choice(3);s.Cold();s.Option("damage");
        s.Reach(()=>s.P is{Kind:DecisionKind.RespondDodge,PlayerSeat:1});
        s.Need(s.E<BoundaryAnjianChoiceIssuedEvent>().All(e=>e.Receipt.TargetSeat!=1),"The in-range target reaches its real Dodge response without an Anjian choice or receipt.");s.Dodge();
        s.Reach(()=>s.P is{Kind:DecisionKind.RespondDodge,PlayerSeat:3});
        s.Need(s.V(2).Hp==7&&s.V(2).HandCount==4&&s.Incoming(3).DamageAmount==2&&
            s.E<BoundaryAnjianChoiceIssuedEvent>().Where(e=>e.Receipt.CardUseFrameId==first).Select(e=>(e.Receipt.TargetSeat,e.Receipt.Option)).SequenceEqual([(2,"no-response"),(3,"damage")]),
            "One original multi-target Use has independent target choices: the in-range target really Dodges, the no-response target keeps all materials and takes1, and the next target still has a native Dodge prompt with public predicted damage2.");
        s.Answer(c=>c.Parameters.GetValueOrDefault("response")=="take-damage");s.Play();
        s.Need(s.V(1).Hp==8&&s.V(3).Hp==6&&s.E<DamageAppliedEvent>().Where(e=>e.SourceSeat==0).Select(e=>(e.TargetSeat,e.Amount)).SequenceEqual([(2,1),(3,2)]),
            "The original Slash applies exactly its chosen per-target bonuses and leaves the independently dodged target unharmed.");
        s.Draw();s.Play();var second=s.Slash([2]);s.Choice(2);s.Option("damage");s.Reach(()=>s.P is{Kind:DecisionKind.RespondDodge,PlayerSeat:2});s.Dodge();s.Play();
        s.Need(s.V(2).Hp==7&&s.E<CardRespondedEvent>().Count(e=>e.ResponderSeat==2&&e.EffectiveCardKind==CardKind.Dodge)==1&&
            s.E<BoundaryAnjianChoiceIssuedEvent>().Count(e=>e.Receipt.CardUseFrameId==second&&e.Receipt.TargetSeat==2&&e.Receipt.Option=="damage")==1,
            "A later actual Use obtains a fresh choice; the earlier no-response receipt does not leak into the same target's genuine later Dodge response.");
    }

    public static void AnyDamageObtainsWeaponWithoutCostAndPaidMovementCold()
    {
        var s=Scene.Start();s.Equip(1);s.Play();var weapon=s.V(1).Equipment.Single().Id;var original=s.V(0).Hand.Select(c=>c.Id).ToArray();var before=s.G.CardMovements.Count;
        s.Accept(new UseProgramSkillCommand(0,Driver,"incoming",[],[1],s.G.Revision,s.P!.PromptId));
        s.Reach(()=>s.P?.SkillPrompt?.SkillId==Duodao&&s.P.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="activate"));
        s.Need(s.V(0).Hp==4&&s.E<DamageAppliedEvent>().Last() is{SourceSeat:1,TargetSeat:0,Amount:1}&&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f=>f.SkillId==Driver&&f.AttackAttempt is{SourceSeat:1,TargetSeat:0}),
            "A real source-attributed native damage with no Slash or other card kind offers current mobile Duodao.");
        s.Cold();s.Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");s.Reach(()=>s.P?.SkillPrompt?.SkillId==Watch);
        s.Need(s.V(1).Equipment.Count==0&&s.V(0).Hand.Select(c=>c.Id).Order().SequenceEqual(original.Append(weapon).Order())&&
            s.G.CardMovements.Skip(before).All(m=>m.From!=CardLocation.Hand(0)||m.To.Zone!=CardZoneKind.DiscardPile),
            "The source's actual weapon enters the damaged owner's hand with every original hand entity retained and no discard cost.");
        s.Cold();s.Answer(_=>true);s.Play();
        s.Need(s.G.CardMovements.Count(m=>m.CardId==weapon&&m.From==CardLocation.Equipment(1)&&m.To==CardLocation.Hand(0))==1&&
            s.E<DamageAppliedEvent>().Count(e=>e.SourceSeat==1&&e.TargetSeat==0&&e.Amount==1)==1,
            "Cold returning the already-paid weapon-gain child completes the original native damage without duplicating damage, movement or a nonexistent cost.");
    }

    private sealed class Scene(GameEngine game,ContentRegistry registry)
    {
        public GameEngine G{get;private set;}=game;
        public PendingDecision? P=>Enumerable.Range(0,5).Select(i=>G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p=>p is not null);
        public PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat).Players.Single(p=>p.Seat==seat);
        public T[] E<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
        public void Play()=>Reach(()=>P is{Kind:DecisionKind.PlayCard,PlayerSeat:0});
        public void Equip(int seat)=>Accept(new UseProgramSkillCommand(0,Driver,"equip",[],[seat],G.Revision,P!.PromptId));
        public void Draw()=>Accept(new UseProgramSkillCommand(0,Driver,"draw",[],[],G.Revision,P!.PromptId));
        public void Trim()=>Accept(new UseProgramSkillCommand(0,Driver,"trim",V(0).Hand.Take(V(0).HandCount-1).Select(c=>c.Id).ToArray(),[],G.Revision,P!.PromptId));
        public long Slash(int[] targets)
        {
            var action=G.GetHumanLegalActions().First(a=>a.Kind==LegalActionKind.Slash&&a.TargetSeats.SequenceEqual(targets));
            Accept(new PlayCardCommand(0,action.CardId!.Value,targets,G.Revision,P!.PromptId,action.PlayedCardKind,action.TargetCardId)
                {ConversionSource=action.ConversionSource,AdditionalConversionSources=action.AdditionalConversionSources});
            return E<CardUseDeclaredEvent>().Last().ResolutionId;
        }
        public void Choice(int target)
        {
            Reach(()=>P?.SkillPrompt?.SkillId==Anjian&&P.Choices.Any(c=>c.Parameters.GetValueOrDefault("option-id")=="damage")&&
                G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f=>f.SkillId==Anjian)?.WindowContext?.TargetSeat==target);
            Need(P!.TargetSeat==target&&P.Prompt.Contains(V(target).Name,StringComparison.Ordinal),"The actual actor's existing choice prompt identifies the current native target in a multi-target Slash.");
        }
        public void Option(string option)=>Answer(c=>c.Parameters.GetValueOrDefault("option-id")==option);
        public void Dodge()=>Answer(c=>c.Cards.Count==1&&c.Parameters.GetValueOrDefault("response")!="take-damage");
        public void Answer(Func<PromptChoice,bool> predicate){var p=P!;Accept(new AnswerPromptCommand(p.PlayerSeat,p.PromptId,p.Choices.First(predicate).Id,G.Revision));}
        public void Accept(GameCommand command){var result=G.Submit(command);Need(result.Accepted,result.Error?.Message??"Rejected native Pan Ma fixture command.");}
        public PublicDamagePrediction Incoming(int viewer)
        {
            const BindingFlags flags=BindingFlags.Instance|BindingFlags.NonPublic;
            var attack=typeof(GameEngine).GetProperty("ActiveCardAttack",flags)!.GetValue(G);
            return(PublicDamagePrediction)typeof(GameEngine).GetMethod("PredictPublicIncomingDamage",flags)!.Invoke(G,[attack,viewer,null,null,null])!;
        }
        public void Reach(Func<bool> done)
        {
            for(var step=0;step<180&&!done();step++)
            {
                if(P is{Kind:DecisionKind.SelectGeneral}select)
                {Accept(new SelectGeneralCommand(select.PlayerSeat,select.PlayerSeat==0?Owner:select.ValidContentIds.First(),G.Revision,select.PromptId));continue;}
                if(P is{Kind:DecisionKind.ProgramTrigger}prompt&&prompt.PlayerSeat==0&&prompt.SkillPrompt?.SkillId==Duodao&&prompt.Choices.Any(c=>c.Cards.Count==1))
                {Answer(c=>c.Cards.Count==1);continue;}
                if(P is{Kind:DecisionKind.ProgramTrigger,PlayerSeat:0 or 1 or 2 or 3})throw new InvalidOperationException("Unexpected original Pan Ma boundary: "+JsonSerializer.Serialize(P));
                Accept(new AdvanceOneStepCommand(G.Revision));
            }
            Need(done(),"The bounded native Pan Ma fixture missed its expected boundary.");
        }
        private string State()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,5).Select(i=>SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),G.CardMovements});
        public void Cold(){var before=State();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),registry);Need(before==State(),"Cold preserves original Use target choice, issued per-target receipt or already-paid source weapon on its original native frames.");}
        public void Need(bool valid,string message){if(!valid)throw new InvalidOperationException(message+JsonSerializer.Serialize(new{P,Players=Enumerable.Range(0,5).Select(i=>new{Seat=i,V(i).Hp,V(i).HandCount})}));}
        public static Scene Start()
        {
            var registry=ContentRegistry.Build(new StandardContentPackage(),new Fixture());
            var game=GameEngine.CreateStandard(new(){Seed=31,PlayerCount=5,HumanSeat=0,HumanSeats=[0,1,2,3],HumanRole=Role.Lord,ModeId=Mode,UseInteractiveSetup=true,
                UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=8},registry);
            var scene=new Scene(game,registry);scene.Accept(new StartGameCommand());scene.Play();return scene;
        }
    }
    private sealed class Fixture:IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-boundary-pan-ma",new(1,0,0));
        internal static string Rules()=>$$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
          {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"native-repeat-slash","query":"slashLimit","operation":"add","value":5,"priority":0}],
            "viewAs":[{"id":"heart-as-slash","inputKinds":[],"inputSuits":["heart"],"outputKind":"slash","forPlay":true,"forResponse":true}],
            "activations":[
              {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"equipped"}]},
              {"id":"trim","minCards":1,"maxCards":8,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"trim"},{"op":"moveBoundCards","target":"owner","sourceBind":"trim","destination":"discardPile","awaitMovementTriggers":true}]},
              {"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1}]},
              {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}
            ]},
          {"id":"{{Defense}}","revision":1,"viewAs":[{"id":"heart-as-dodge","inputKinds":[],"inputSuits":["heart"],"outputKind":"dodge","forPlay":false,"forResponse":true}]},
          {"id":"{{Watch}}","revision":1,"triggers":[{"id":"weapon-paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.boundary:duodao-mobile.MoveBoundCards"],"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}
        ]}
        """;
        internal static string Presentation()=>JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=new Dictionary<string,object>{
            [Driver]=new{name="真实多目标输入",description="原生方天单手牌杀和指定来源伤害"},[Defense]=new{name="真实闪响应",description="原生红色牌转换闪"},
            [Watch]=new{name="武器已获取",description="真实装备获取后子窗",optionLabels=new Dictionary<string,string>{{"continue","继续"}}}}});
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder,Hero);var catalog=SkillProgramCatalog.Load(Rules(),Presentation());
            foreach(var(id,program)in catalog.Programs)builder.AddSkill(new(id,catalog.Presentations[id].Name,catalog.Presentations[id].Description){Program=program,ProgramPresentation=catalog.Presentations[id]});
            builder.AddGeneral(new(Owner,"潘马真实多目标","supporter",Duodao,"wu",4,AdditionalSkillIds:[Anjian,Driver,Watch]));
            var peers=Enumerable.Range(1,4).Select(i=>"fixture:boundary-pan-ma-peer-"+i).ToArray();
            foreach(var peer in peers)builder.AddGeneral(new(peer,"固定原生响应者","supporter",Defense,"wei",8));
            const string weapon="fixture:boundary-pan-ma-halberd",deck="fixture:boundary-pan-ma-deck";var card=CardCatalog.Get(CardKind.FangtianHalberd);
            builder.AddCard(new(weapon,card.DisplayName,card.CategoryName,card.Description,CardKind.FangtianHalberd));
            builder.AddDeck(new(deck,"固定原生红色装备实体",4,0,[]){PhysicalCards=Enumerable.Range(0,100).Select(i=>new ContentDeckPhysicalCard(weapon,Suit.Heart,i%13+1)).ToArray()});
            builder.AddMode(new(Mode,"潘马真实逐目标选择",5,5,new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Loyalist),1},{nameof(Role.Rebel),2},{nameof(Role.Renegade),1}},deck,
                GeneralCandidateCount:5,GeneralPoolIds:[Owner,..peers]));
        }
    }
}
