using System.Runtime.CompilerServices;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Core;
using CardGame.Content.Standard;

internal static class MobileLuZhiChecks
{
    private const string Mingren="mobile:mingren",Zhenliang="mobile:zhenliang",Hero="fixture:mobile-lu-zhi-owner",Peer="fixture:mobile-lu-zhi-peer-",Driver="fixture:mobile-lu-zhi-driver",Watch="fixture:mobile-lu-zhi-watch",Mode="identity:classic-mobile-lu-zhi";
    private const string Cost="skill-program.public-pile.hp-difference-payment",Reward="skill-program.mobile:zhenliang.Draw";

    public static void OneCardOpeningPublicRenAndFrozenHpDifferencePaymentCold()
    {
        foreach(var equal in new[]{false,true})
        {
            var s=Scene.Start();s.Opening(CardKind.Dodge);
            var target=equal?3:1;var required=equal?1:2;var before=s.V(target).Hp;
            s.Yang(target);
            s.Need(s.YangFrame!.HpDifferencePileDamage is{} r&&r.Required==required&&Math.Max(1,Math.Abs(r.OwnerHp-r.TargetHp))==required,
                "Yang freezes the absolute real HP difference at target selection, including equal HP's minimum one.");
            var cards=s.V(0).Hand.Where(c=>c.Suit is Suit.Heart or Suit.Diamond).Select(c=>c.Id).Take(required).ToArray();
            s.Need(cards.Length==required,"The true same-color HE payment is fully affordable.");
            foreach(var id in cards)s.Answer(c=>c.Cards.SequenceEqual(new[]{id}));
            s.Reach(()=>s.WatchBinding=="yang-paid");
            s.Need(s.V(target).Hp==before&&cards.All(id=>s.G.CardMovements.Count(m=>m.CardId==id&&m.Reason.Value==Cost&&m.From==CardLocation.Hand(0))==1),
                "Every physical cost is discarded before the real child window; native damage has not been issued early.");
            s.Cold();s.Continue();s.PlayBoundary(0);
            s.Need(s.V(target).Hp==before-1&&s.Polarity==SkillPolarity.Yin&&s.Facts<HpDifferencePileDamagePaidEvent>().Length==1&&
                !s.G.GetLegalActions(0).Any(a=>a.ProgramSkillId==Zhenliang),
                "The paid original frame deals exactly one damage once and consumes the actual Play's Yang opportunity.");
        }
    }

    public static void OutsideTurnNativeUseAndPlayedCompletedCategoryRewardCold()
    {
        foreach(var scene in new[]{"played","dodge","same-color-wrong-category"})
        {
            var s=Scene.Start(scene=="same-color-wrong-category");s.Opening(scene=="same-color-wrong-category"?CardKind.Dismantlement:CardKind.Dodge);
            s.Yang(1);
            var ids=s.V(0).Hand.Where(c=>scene=="same-color-wrong-category"?c.Suit is Suit.Spade or Suit.Club:c.Suit is Suit.Heart or Suit.Diamond).Select(c=>c.Id).Take(2).ToArray();
            foreach(var id in ids)s.Answer(c=>c.Cards.SequenceEqual(new[]{id}));s.Reach(()=>s.WatchBinding=="yang-paid");s.Continue();s.PlayBoundary(0);s.End();s.PlayBoundary(1);
            s.Play(1,scene=="played"?CardKind.Duel:CardKind.Slash,0);
            s.Reach(()=>s.P is{PlayerSeat:0,Kind:DecisionKind.RespondSlash or DecisionKind.RespondDodge});
            var material=s.V(0).Hand.First(c=>c.Suit is Suit.Spade or Suit.Club).Id;
            s.Answer(c=>c.Cards.SequenceEqual(new[]{material}));s.Reach(()=>s.WatchBinding=="response-paid");
            s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().All(f=>f.SkillId!=Zhenliang)&&s.Polarity==SkillPolarity.Yin,
                "Moving the actual response material does not offer mobile Yin before its native payment children finish.");
            if(scene=="played")s.Cold();s.Continue();
            s.Reach(()=>s.P?.SkillPrompt?.SkillId==Zhenliang||s.P is{PlayerSeat:1,Kind:DecisionKind.PlayCard});
            if(scene=="same-color-wrong-category")
            {
                s.Need(s.P is{PlayerSeat:1,Kind:DecisionKind.PlayCard}&&s.Polarity==SkillPolarity.Yin&&s.RewardMoves==0,
                    "A black Basic response cannot match the black Trick Ren merely by color.");continue;
            }
            var window=s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last();
            s.Need(scene=="played"?window.ResponseCompletion is{CostsDrained:true}&&window.Action.EffectiveKind==CardKind.Slash:
                window.CompletedResponseReturn is{Kind:ProgramCompletedResponseKind.Dodge,IsCommitted:false},
                "Yin qualifies only the real Played or typed actual Use completion, after original response payment and before returning to its native parent.");
            s.Activate();s.Reach(()=>s.P?.Choices.Any(c=>c.Targets.SequenceEqual(new[]{2}))==true);var hand=s.V(2).HandCount;s.Answer(c=>c.Targets.SequenceEqual(new[]{2}));
            s.Reach(()=>s.WatchBinding=="yin-paid");
            s.Need(s.V(2).HandCount==hand+1&&s.RewardMoves==1&&s.Polarity==SkillPolarity.Yang&&
                s.G.ResolutionStack.OfType<ProgramSkillFrame>().Any(f=>f.MobileLuZhiYin is{Category:SkillProgramCardCategory.Basic}),
                "A black Basic response matches red Basic Ren by category, grants exactly one selected-recipient card and commits Yin once.");
            s.Cold();s.Continue();s.PlayBoundary(1);s.Need(s.RewardMoves==1,"The paid recipient gain returns on the original response parent without drawing twice.");
        }
    }

    private sealed class Scene(GameEngine game,ContentRegistry registry)
    {
        internal GameEngine G=game;private readonly ContentRegistry Content=registry;
        internal PendingDecision? P=>Enumerable.Range(0,4).Select(seat=>G.CreateSnapshot(seat,true).PendingDecision).FirstOrDefault(p=>p is not null);
        internal PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat,true).Players[seat];
        internal T[] Facts<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
        internal SkillPolarity? Polarity=>V(0).SkillRuntimeStates!.Single(s=>s.SkillId==Zhenliang).Polarity;
        internal string? WatchBinding=>G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault() is{SkillId:Watch} f?f.TriggerId:null;
        internal ProgramSkillFrame? YangFrame=>G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f=>f.HpDifferencePileDamage is not null);
        internal int RewardMoves=>G.CardMovements.Count(m=>m.Reason.Value==Reward);
        private int[] PublicRenIds(int viewer)
        {
            var player=G.CreateSnapshot(viewer).Players[0];
            return player.PublicPersistentPiles is{} piles?piles.Where(p=>p.SourceSkillId==Mingren).SelectMany(p=>p.Cards).Select(c=>c.Id).ToArray():
                player.PublicPersistentPileSkillId==Mingren?player.PublicPersistentPileCards?.Select(c=>c.Id).ToArray()??[]:[];
        }
        internal void Need(bool ok,string text){if(!ok)throw new InvalidOperationException(text+Diag());}
        private string Diag()=>JsonSerializer.Serialize(new{G.State.TurnNumber,G.State.CurrentSeat,G.State.Phase,Pending=P,Players=G.CreateSnapshot(0,true).Players,
            Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())),Facts=G.Events.TakeLast(20).Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())),Moves=G.CardMovements.TakeLast(20)});
        internal void Submit(GameCommand c){var result=G.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single());Need(result.Accepted,result.Error?.Message??"Native mobile Lu Zhi command rejected.");}
        internal void Answer(Func<PromptChoice,bool> pick){var choice=P!.Choices.FirstOrDefault(pick);Need(choice is not null,"Missing exact native choice.");Submit(new AnswerPromptCommand(P!.PlayerSeat,P.PromptId,choice!.Id,G.Revision));}
        internal void Continue()=>Answer(c=>c.Parameters.GetValueOrDefault("option-id")=="continue");
        internal void Activate()=>Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
        internal void End()=>Submit(new EndPlayPhaseCommand(P!.PlayerSeat,G.Revision,P.PromptId));
        internal void PlayBoundary(int seat)=>Reach(()=>P is{Kind:DecisionKind.PlayCard} p&&p.PlayerSeat==seat);
        internal void Opening(CardKind renKind)
        {
            Need(WatchBinding=="initial-paid"&&V(0).HandCount==5&&G.CardMovements.Count(m=>m.Reason.Value=="skill-program.mobile:mingren.Draw")==1,
                "Mobile Mingren's real opening draw is exactly one card.");
            Cold();Continue();Reach(()=>P?.SkillPrompt?.SkillId==Mingren&&P.Choices.Any(c=>c.Cards.Count==1));var id=V(0).Hand.First(c=>c.Kind==renKind).Id;Answer(c=>c.Cards.SequenceEqual(new[]{id}));PlayBoundary(0);
            Need(V(0).HandCount==4&&Enumerable.Range(0,4).All(viewer=>PublicRenIds(viewer).SequenceEqual(new[]{id})),
                "The actual selected Hand entity becomes one publicly visible Ren, with the one-card opening draw then deposit settled.");
        }
        internal void Yang(int target)
        {
            Need(G.GetLegalActions(0).Any(a=>a.ProgramSkillId==Zhenliang&&a.ProgramActivationId=="reserve-hp-difference-damage"),
                "The precise Yang activation must be genuinely published before submission. "+ActivationDiagnostic(target));
            Submit(new UseProgramSkillCommand(0,Zhenliang,"reserve-hp-difference-damage",[],[],G.Revision,P!.PromptId));
            Answer(c=>c.Targets.SequenceEqual(new[]{target}));
        }
        private string ActivationDiagnostic(int target)
        {
            object? Call(string name,params object?[] args)=>typeof(GameEngine).GetMethods(BindingFlags.Instance|BindingFlags.NonPublic)
                .Single(m=>m.Name==name&&m.GetParameters().Length==args.Length).Invoke(G,args);
            var owner=((IEnumerable<CharacterState>)typeof(GameEngine).GetField("_players",BindingFlags.Instance|BindingFlags.NonPublic)!.GetValue(G)!).Single(p=>p.Seat==0);
            var program=Content.GetSkill(Zhenliang).Program!;var plan=ProgramInstructionResolver.Default.Resolve(program,ProgramInstructionSourceKind.Activation,"reserve-hp-difference-damage");
            var context=(PlayerSkillContext)Call("CreateSkillContext",owner,false)!;
            var router=(ProgramLegalityRouter)typeof(GameEngine).GetProperty("ProgramLegality",BindingFlags.Instance|BindingFlags.NonPublic)!.GetValue(G)!;
            var gates=new ProgramActivationLegalityContext(owner,program,plan,context);
            var instance=(string)Call("GetRuntimeSkillInstanceId",owner,Zhenliang)!;var effect=plan.Features.First(SkillProgramEffectOp.PublicPileColorDamage)!;
            return JsonSerializer.Serialize(new{Command=new{Skill=Zhenliang,Activation="reserve-hp-difference-damage",Target=target},Polarity,
                Instance=instance,EffectPolicy=effect.PublicPileDamageCostPolicy,SourceGrants=owner.SkillGrants.Grants,
                Piles=Call("SameGrantColorPiles",0,Mingren,instance),HpTargets=Call("HpDifferencePileDamageTargets",0,Zhenliang,instance,effect),
                RedPayable=Call("HpDifferencePileCosts",0,Zhenliang,instance,true),BlackPayable=Call("HpDifferencePileCosts",0,Zhenliang,instance,false),
                PublicGate=Call("CanActivatePublicPileColor",owner,Zhenliang,plan.Features),
                Phases=Enum.GetValues<ProgramActivationLegalityPhase>().Select(p=>new{Phase=p,Allowed=router.CanActivate(p,gates)})});
        }
        internal void Play(int actor,CardKind kind,int target)
        {
            var a=G.GetLegalActions(actor).FirstOrDefault(a=>a.CardId is not null&&a.Kind!=LegalActionKind.Recast&&(a.PlayedCardKind??V(actor).Hand.FirstOrDefault(c=>c.Id==a.CardId)?.Kind)==kind&&a.TargetSeats.SequenceEqual(new[]{target}));
            Need(a?.CardId is not null,"Missing legal native "+kind+" actor "+actor);
            Submit(new PlayCardCommand(actor,a!.CardId!.Value,a.TargetSeats,G.Revision,P!.PromptId,a.PlayedCardKind,a.TargetCardId){ConversionSource=a.ConversionSource,AdditionalConversionSources=a.AdditionalConversionSources??[]});
        }
        internal void Reach(Func<bool> stop,[CallerLineNumber]int line=0)
        {
            for(var n=0;n<200;n++)
            {
                if(stop())return;Need(G.State.Winner==Winner.None,"Unexpected terminal boundary at "+line);
                if(P is{Kind:DecisionKind.SelectGeneral} setup){Submit(new SelectGeneralCommand(setup.PlayerSeat,setup.PlayerSeat==0?Hero:Peer+setup.PlayerSeat,G.Revision,setup.PromptId));continue;}
                if(P is{Kind:DecisionKind.PlayCard}){End();continue;}
                if(P is{Kind:DecisionKind.DiscardCards} d){Submit(new DiscardCardsCommand(d.PlayerSeat,d.ValidCardIds.Take(d.RequiredCardCount).ToArray(),d.PromptId,G.Revision));continue;}
                if(P is not null)
                {
                    if(WatchBinding is not null){Continue();continue;}
                    if(P.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="skip")){Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="skip");continue;}
                    if(P.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying){Answer(c=>c.Cards.Count==0);continue;}
                    Need(false,"Unexpected human native boundary at "+line);
                }
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false,"Missing bounded native mobile Lu Zhi flow at "+line);
        }
        private string State()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,4).Select(seat=>SnapshotJson.Serialize(G.CreateSnapshot(seat))).ToArray(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),G.CardMovements,Events=G.Events.Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())).ToArray(),Commands=CommandJson.Serialize(G.AcceptedCommands)});
        internal void Cold(){var before=State();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),Content);Need(before==State(),"Cold preserves the original paid public Ren, frozen HP cost and native response/draw parent.");}
        internal static Scene Start(bool trickRen=false)
        {
            var stock=Enumerable.Range(0,100).Select(n=>new ContentDeckPhysicalCard("standard:slash",Suit.Spade,n%13+1)).ToArray();
            var probe=Boot(ContentRegistry.Build(new StandardContentPackage(),new Fixture(stock)));
            var hand=probe.V(0).Hand.Select(c=>c.Id).ToArray();probe.Need(hand.Length==5,"The fixed opening contains five native physical Hand entities before deposit.");
            var kinds=trickRen?new[]{CardKind.Dismantlement,CardKind.Slash,CardKind.Slash,CardKind.Crossbow,CardKind.Peach}:new[]{CardKind.Dodge,CardKind.Crossbow,CardKind.Peach,CardKind.Slash,CardKind.Slash};
            var suits=trickRen?new[]{Suit.Spade,Suit.Spade,Suit.Club,Suit.Spade,Suit.Heart}:new[]{Suit.Heart,Suit.Heart,Suit.Diamond,Suit.Spade,Suit.Club};
            for(var i=0;i<hand.Length;i++)stock[hand[i]-1]=new(probe.Content.Cards.Values.First(c=>c.LegacyKind==kinds[i]).Id,suits[i],7);
            return Boot(ContentRegistry.Build(new StandardContentPackage(),new Fixture(stock)));
        }
        private static Scene Boot(ContentRegistry registry)
        {
            var g=GameEngine.CreateStandard(new GameOptions{Seed=17,PlayerCount=4,HumanSeat=0,HumanSeats=[0,1,2,3],HumanRole=Role.Lord,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=true,AdvanceAfterHumanCommands=false,MaxTurns=12},registry);
            var s=new Scene(g,registry);s.Submit(new StartGameCommand());s.Reach(()=>s.WatchBinding=="initial-paid");return s;
        }
    }
    private sealed class Fixture(ContentDeckPhysicalCard[] stock):IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-mobile-lu-zhi",new Version(1,0,0),[]);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,"mobile:lu-zhi");
            var rules=JsonNode.Parse("""
            {"skills":[
             {"id":"fixture:mobile-lu-zhi-driver","revision":1,"viewAs":[{"id":"duel","inputKinds":[],"inputSuits":[],"outputKind":"duel","forPlay":true,"forResponse":false,"useOnly":true,"singleCardTrickUse":true},{"id":"dodge","inputKinds":[],"inputSuits":[],"outputKind":"dodge","forPlay":false,"forResponse":true}],"triggers":[{"id":"quiet","window":"drawPhaseStarting","drawPhaseMode":"replacement","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":0}]}]},
             {"id":"fixture:mobile-lu-zhi-watch","revision":1,"triggers":[
              {"id":"initial-paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile:mingren.Draw"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"watch","options":[{"id":"continue"}]}]},
              {"id":"yin-paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile:zhenliang.Draw"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"watch","options":[{"id":"continue"}]}]},
              {"id":"yang-paid","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perOwnerBatch","movementReasons":["skill-program.public-pile.hp-difference-payment"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"watch","options":[{"id":"continue"}]}]},
              {"id":"response-paid","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perOwnerBatch","movementReasons":["card.respond"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"watch","options":[{"id":"continue"}]}]}
             ]}
            ]}
            """)!;rules["schemaVersion"]=SkillProgramCatalog.RulesSchemaVersion;
            var catalog=SkillProgramCatalog.Load(rules.ToJsonString(),"""
            {"schemaVersion":3,"skills":{"fixture:mobile-lu-zhi-driver":{"name":"真实结束驱动","description":"测试"},"fixture:mobile-lu-zhi-watch":{"name":"真实付款观察","description":"测试","optionLabels":{"continue":"继续"}}}}
            """);
            foreach(var entry in catalog.Programs)b.AddSkill(new(entry.Key,entry.Key,"测试"){Program=entry.Value});
            b.AddGeneral(new(Hero,"移动卢植","supporter",Mingren,"qun",10,[Zhenliang,Driver,Watch]));
            for(var seat=1;seat<4;seat++)b.AddGeneral(new(Peer+seat,"真实目标","supporter",Driver,"shu",seat==1?13:11,[Watch]));
            b.AddDeck(new("fixture:mobile-lu-zhi-deck","固定原生牌堆",4,0,[]){PhysicalCards=stock});
            b.AddMode(new(Mode,"移动卢植真实规则",4,4,new Dictionary<string,int>{[nameof(Role.Lord)]=1,[nameof(Role.Loyalist)]=1,[nameof(Role.Rebel)]=2},"fixture:mobile-lu-zhi-deck",GeneralCandidateCount:4,GeneralPoolIds:[Hero,Peer+1,Peer+2,Peer+3]));
        }
    }
}
