using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Core;
using CardGame.Content.Standard;

internal static class MobileGuoHuanghouChecks
{
    private const string Skill="mobile:jiaozhao",Hero="fixture:mobile-guo-huanghou-owner",Peer="fixture:mobile-guo-huanghou-peer-",Driver="fixture:mobile-guo-huanghou-driver",Mode="identity:classic-mobile-guo-huanghou";

    public static void TwiceNativeUpgradedSelfDeclarationKeepsFixedEntityAndNameCold()
    {
        var s=Scene.Start();var startingHp=s.V(0).Hp;
        for(var tier=1;tier<=2;tier++)
        {
            s.Submit(new UseProgramSkillCommand(0,Driver,"damage",[],[0],s.G.Revision,s.P!.PromptId));
            s.Reach(()=>s.P?.SkillPrompt?.SkillId=="classic:danxin");s.Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
            s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("option-id")=="upgrade")==true);s.Answer(c=>c.Parameters.GetValueOrDefault("option-id")=="upgrade");s.PlayBoundary();
            s.Need(s.V(0).ConfiguredConversionTiers!.GetValueOrDefault("declared-card-tier")==tier&&s.V(0).Hp==startingHp-tier,
                "Both real damage/after-damage returns use the shared Danxin upgrade mutation and advance the same original conversion state once.");
        }
        s.Need(!s.G.GetLegalActions(0).Any(a=>a.ConversionSource?.SkillId==Skill),
            "At final tier, undeclared arbitrary Hand entities still have no mobile Jiaozhao conversion.");
        var original=s.V(0).Hand[0];var twin=s.V(0).Hand.First(c=>c.Id!=original.Id&&c.Kind==original.Kind);
        s.Submit(new UseProgramSkillCommand(0,Skill,"declare-card",[original.Id],[],s.G.Revision,s.P!.PromptId));
        var declaration=s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.CardDeclarationDraft is not null);
        s.Need(declaration.CardDeclarationDraft is{Tier:2,DeclarerSeat:0} draft&&draft.CardId==original.Id&&draft.NearestSeats.SequenceEqual(new[]{0})&&s.P?.PlayerSeat==0&&
            s.P.Choices.All(c=>c.Targets.Count==0)&&s.P.Choices.Any(c=>c.Parameters.GetValueOrDefault("choice")==nameof(CardKind.Duel)),
            "Final-tier mobile Jiaozhao reveals its exact original Hand entity and asks its owner for a fixed name on the same declaration frame.");
        s.Need(Enumerable.Range(0,4).All(viewer=>s.G.CreateSnapshot(viewer).PublicRevealedCards.Select(c=>c.Id).SequenceEqual(new[]{original.Id})),
            "The real final-tier displayed material remains publicly visible to all four viewers.");
        s.Cold();s.Answer(c=>c.Parameters.GetValueOrDefault("choice")==nameof(CardKind.Duel));s.PlayBoundary();
        var conversions=s.G.GetLegalActions(0).Where(a=>a.ConversionSource?.SkillId==Skill).ToArray();
        s.Need(conversions.Length>0&&conversions.All(a=>a.CardId==original.Id&&a.PlayedCardKind==CardKind.Duel&&!a.TargetSeats.Contains(0)),
            "All three tiers retain the fixed entity, fixed declared name and no-self-target rule, even with another physically identical Hand card.");
        var action=conversions.First(a=>a.TargetSeats.SequenceEqual(new[]{1}));var before=s.State();
        var reject=s.G.Submit(new PlayCardCommand(0,twin.Id,action.TargetSeats,s.G.Revision,s.P!.PromptId,CardKind.Duel,action.TargetCardId){ConversionSource=action.ConversionSource});
        s.Need(!reject.Accepted&&before==s.State(),"A different same-name entity cannot pay the declared conversion or consume its original native cost.");
        s.Submit(new PlayCardCommand(0,original.Id,action.TargetSeats,s.G.Revision,s.P!.PromptId,CardKind.Duel,action.TargetCardId){ConversionSource=action.ConversionSource});s.PlayBoundary();
        s.Need(s.G.CardMovements.Count(m=>m.CardId==original.Id&&m.From==CardLocation.Hand(0)&&m.To==CardLocation.Processing)==1&&
            s.G.CardMovements.Last(m=>m.CardId==original.Id).To==CardLocation.DiscardPile&&s.V(0).Hand.Any(c=>c.Id==twin.Id)&&
            s.G.Events.Select(e=>e.Payload).OfType<CardUseFinishedEvent>().Count(e=>e.CardId==original.Id&&e.CardKind==CardKind.Duel)==1&&
            !s.G.GetLegalActions(0).Any(a=>a.ProgramSkillId==Skill||a.ConversionSource?.SkillId==Skill),
            "The fixed original entity pays and finishes the genuine declared Duel once; the declaration phase allowance and conversion cannot be paid again.");
    }

    private sealed class Scene(GameEngine game,ContentRegistry registry)
    {
        internal GameEngine G=game;
        internal PendingDecision? P=>Enumerable.Range(0,4).Select(seat=>G.CreateSnapshot(seat).PendingDecision).FirstOrDefault(p=>p is not null);
        internal PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat,true).Players[seat];
        internal void Need(bool ok,string message){if(!ok)throw new InvalidOperationException(message+JsonSerializer.Serialize(new{G.State.TurnNumber,G.State.CurrentSeat,G.State.Phase,Pending=P,
            Players=G.CreateSnapshot(0,true).Players,Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())),Facts=G.Events.TakeLast(18).Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType()))}));}
        internal void Submit(GameCommand command){var result=G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());Need(result.Accepted,result.Error?.Message??"Native Guo Huanghou command rejected.");}
        internal void Answer(Func<PromptChoice,bool> pick){var choice=P!.Choices.FirstOrDefault(pick);Need(choice is not null,"Missing native declaration or upgrade choice.");Submit(new AnswerPromptCommand(P!.PlayerSeat,P.PromptId,choice!.Id,G.Revision));}
        internal void PlayBoundary()=>Reach(()=>P is{PlayerSeat:0,Kind:DecisionKind.PlayCard});
        internal void Reach(Func<bool> ready,[CallerLineNumber]int line=0)
        {
            for(var n=0;n<160;n++)
            {
                if(ready())return;Need(G.State.Winner==Winner.None,"Unexpected terminal result at "+line);
                if(P is{Kind:DecisionKind.SelectGeneral} setup){Submit(new SelectGeneralCommand(setup.PlayerSeat,setup.PlayerSeat==0?Hero:Peer+setup.PlayerSeat,G.Revision,setup.PromptId));continue;}
                if(P is{Kind:DecisionKind.PlayCard} play){Submit(new EndPlayPhaseCommand(play.PlayerSeat,G.Revision,play.PromptId));continue;}
                if(P is{Kind:DecisionKind.DiscardCards} d){Submit(new DiscardCardsCommand(d.PlayerSeat,d.ValidCardIds.Take(d.RequiredCardCount).ToArray(),d.PromptId,G.Revision));continue;}
                if(P is not null)
                {
                    if(P.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="skip")){Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="skip");continue;}
                    if(P.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge or DecisionKind.Nullification or DecisionKind.RescueDying){Answer(c=>c.Cards.Count==0);continue;}
                    Need(false,"Unexpected native human boundary at "+line);
                }
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false,"Missing bounded real declaration flow at "+line);
        }
        internal string State()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,4).Select(seat=>SnapshotJson.Serialize(G.CreateSnapshot(seat))).ToArray(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),
            G.CardMovements,Events=G.Events.Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())).ToArray(),Commands=CommandJson.Serialize(G.AcceptedCommands)});
        internal void Cold(){var before=State();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),registry);Need(before==State(),"Cold retains the original public entity, frozen final tier, self declarer and exact native declaration cursor.");}
        internal static Scene Start()
        {
            var registry=ContentRegistry.Build(new StandardContentPackage(),new Fixture());
            var game=GameEngine.CreateStandard(new GameOptions{Seed=17,PlayerCount=4,HumanSeat=0,HumanSeats=[0,1,2,3],HumanRole=Role.Lord,ModeId=Mode,
                UseInteractiveSetup=true,UseInteractiveDiscard=true,AdvanceAfterHumanCommands=false,MaxTurns=8},registry);
            var s=new Scene(game,registry);s.Submit(new StartGameCommand());s.PlayBoundary();return s;
        }
    }
    private sealed class Fixture:IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-mobile-guo-huanghou",new Version(1,0,0),[]);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,"mobile:guo-huanghou","classic:guo-huanghou");
            var rules=JsonNode.Parse("""
            {"skills":[{"id":"fixture:mobile-guo-huanghou-driver","revision":1,"activations":[{"id":"damage","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"selectedTarget","amount":1}]}],"triggers":[{"id":"quiet","window":"drawPhaseStarting","drawPhaseMode":"replacement","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":0}]}]}]}
            """)!;rules["schemaVersion"]=SkillProgramCatalog.RulesSchemaVersion;
            var catalog=SkillProgramCatalog.Load(rules.ToJsonString(),"""
            {"schemaVersion":3,"skills":{"fixture:mobile-guo-huanghou-driver":{"name":"真实受伤驱动","description":"测试"}}}
            """);
            foreach(var entry in catalog.Programs)b.AddSkill(new(entry.Key,entry.Key,"测试"){Program=entry.Value});
            b.AddGeneral(new(Hero,"移动郭皇后","supporter",Skill,"wei",9,["classic:danxin",Driver]));
            for(var seat=1;seat<4;seat++)b.AddGeneral(new(Peer+seat,"原生目标","supporter",Driver,"wei",9));
            b.AddDeck(new("fixture:mobile-guo-huanghou-deck","真实同名实体牌堆",4,0,[]){PhysicalCards=Enumerable.Range(0,100).Select(n=>new ContentDeckPhysicalCard("standard:crossbow",n%2==0?Suit.Spade:Suit.Heart,n%13+1)).ToArray()});
            b.AddMode(new(Mode,"移动郭皇后真实声明",4,4,new Dictionary<string,int>{[nameof(Role.Lord)]=1,[nameof(Role.Loyalist)]=1,[nameof(Role.Rebel)]=2},"fixture:mobile-guo-huanghou-deck",GeneralCandidateCount:4,GeneralPoolIds:[Hero,Peer+1,Peer+2,Peer+3]));
        }
    }
}
