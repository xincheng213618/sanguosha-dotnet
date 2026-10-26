using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileXueZongChecks
{
    private const string Hero="fixture:mobile-xue-owner",Peer="fixture:mobile-xue-peer-",Driver="fixture:mobile-xue-driver",Watch="fixture:mobile-xue-watch",Mode="identity:classic-mobile-xue";
    private const string ClaimReason="skill-program.response-entity-exchange",DiscardReason="skill-program.public-suit-escalating-discard",RewardReason="skill-program.mobile:funan.Draw";

    public static void DirectClaimHandOnlyUpgradeRetainedAndUnansweredEntityUsesCold()
    {
        var s=CreateUpgradedScene(out var acquired);
        var use=s.Play(0,CardKind.Crossbow,0,acquired[0],native:true);
        s.Reach(()=>s.WatchBinding=="reward");
        s.Need(s.Facts<MobileFunanUseEntityCapturedEvent>().Any(e=>e.ActionId==use&&e.CardId==acquired[0]) && s.RewardMoves==1,
            "An entity acquired before upgrading qualifies at actual use commitment; an unanswered equipment Use draws once.");
        s.Cold(); s.Continue(); s.PlayBoundary(0); s.Need(s.RewardMoves==1,"The paid Draw child resumes on its original use-completion frame without drawing again.");

        var replied=s.Play(0,CardKind.Duel,2,acquired[1]);
        s.Reach(()=>s.P is{PlayerSeat:2,Kind:DecisionKind.RespondSlash}); var reply=s.V(2).Hand.First().Id;
        s.Answer(c=>c.Cards.SequenceEqual(new[]{reply})); s.Reach(()=>s.P?.SkillPrompt?.SkillId=="mobile:funan"); s.Activate();
        s.Reach(()=>s.WatchBinding=="claim"); s.Continue();
        s.Reach(()=>s.P is{PlayerSeat:0,Kind:DecisionKind.RespondSlash}); s.Answer(c=>c.Cards.Count==0); s.PlayBoundary(0);
        s.Need(s.Facts<MobileFunanDirectResponseObservedEvent>().Any(e=>e.RespondedActionId==replied&&e.ResponderSeat==2)&&s.RewardMoves==1,
            "The exact real response to the acquired-entity Duel suppresses its completion reward.");

        // Retained Jiexun is offered at the next Ending with the current count 2.
        s.End(); s.Reach(()=>s.P?.SkillPrompt?.SkillId=="mobile:jiexun"); s.Activate(); s.Answer(c=>c.Targets.SequenceEqual(new[]{3}));
        s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="public-suit-choice")==true);
        s.Answer(c=>c.Parameters.GetValueOrDefault("suit")==nameof(Suit.Heart));
        s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="public-suit-discard")==true);
        s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.PublicSuitDiscard is not null).PublicSuitDiscard is{Required:2},
            "The retained skill's next actual invocation requires two real cards.");
        s.Answer(c=>c.Cards.Count==1); s.Answer(c=>c.Cards.Count==1); s.Reach(()=>s.WatchBinding=="discard"); s.Continue();

        s.PlayBoundary(2); s.Play(2,CardKind.Duel,0);
        s.Reach(()=>s.P is{PlayerSeat:0,Kind:DecisionKind.RespondSlash}); s.Answer(c=>c.Cards.SequenceEqual(new[]{acquired[2]}));
        var played=s.Facts<CardActionAcceptedEvent>().Last().Action;
        s.Reach(()=>s.P is{PlayerSeat:2,Kind:DecisionKind.RespondSlash}); s.Answer(c=>c.Cards.Count==0); s.PlayBoundary(2);
        s.Need(s.G.CardMovements.Any(m=>m.CardId==acquired[2]&&m.Reason==CardMoveReasons.Respond)&&
            s.Facts<MobileFunanUseEntityCapturedEvent>().All(e=>e.ActionId!=played.ActionId)&&s.RewardMoves==1,
            "A genuinely played Slash response in Duel is excluded, despite spending an acquired physical entity.");
        s.End(); s.PlayBoundary(3); s.Play(3,CardKind.Slash,0); s.Reach(()=>s.P is{PlayerSeat:0,Kind:DecisionKind.RespondDodge});
        s.Answer(c=>c.Cards.SequenceEqual(new[]{reply})); s.Reach(()=>s.WatchBinding=="reward");
        s.Need(s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w=>w.CompletedResponseReturn is{Kind:ProgramCompletedResponseKind.Dodge,IsCommitted:false})&&s.RewardMoves==2,
            "Using the obtained entity as a true Dodge returns through the typed native defense completion and receives its unanswered-use reward.");
        s.Cold(); s.Continue(); s.PlayBoundary(3); s.Need(s.RewardMoves==2,"Typed defense reward is paid exactly once across cold recovery.");
    }

    public static void ChosenPublicSuitCapAndHandOnlyUpgradeWithEquipmentRemaining()
    {
        var s=Scene.Start(cap:true);
        foreach(var kind in new[]{CardKind.Crossbow,CardKind.BaguaFormation,CardKind.OffensiveHorse,CardKind.DefensiveHorse})
        {s.Play(0,kind,0,native:true);s.PlayBoundary(0);}
        s.End();s.PlayBoundary(1);
        foreach(var kind in new[]{CardKind.Crossbow,CardKind.BaguaFormation}){s.Play(1,kind,1,native:true);s.PlayBoundary(1);}
        s.End();s.PlayBoundary(0);s.End();s.Reach(()=>s.P?.SkillPrompt?.SkillId=="mobile:jiexun");s.Activate();s.Answer(c=>c.Targets.SequenceEqual(new[]{2}));
        s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="public-suit-choice")==true);
        var hand=s.V(2).HandCount;s.Answer(c=>c.Parameters.GetValueOrDefault("suit")==nameof(Suit.Spade));s.Reach(()=>s.WatchBinding=="suit-draw");
        s.Need(s.V(2).HandCount==hand+5&&s.Facts<ProgramEscalatingDiscardStartedEvent>().Single().DrawCount==5&&
            s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.PublicSuitDiscard is not null).PublicSuitDiscard is{ChosenSuit:Suit.Spade,DrawCount:5,Required:1},
            "Six real Spade equipment cards produce exactly five cards and one first-invocation discard, frozen before the real gain child.");
        s.Cold();s.Continue();s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="public-suit-discard")==true);
        s.Answer(c=>c.Cards.Count==1);s.Reach(()=>s.WatchBinding=="discard");s.Continue();s.PlayBoundary(1);
        // Seat 1 retains one Hand card and two public equipment cards for the
        // second invocation: discard its Hand plus one equipment, retain one.
        while(s.V(1).HandCount>1){var id=s.V(1).Hand.First().Id;s.Submit(new UseProgramSkillCommand(1,Driver,"discard-one",[id],[],s.G.Revision,s.P!.PromptId));s.PlayBoundary(1);}
        s.End();s.PlayBoundary(0);s.End();s.Reach(()=>s.P?.SkillPrompt?.SkillId=="mobile:jiexun");s.Activate();s.Answer(c=>c.Targets.SequenceEqual(new[]{1}));
        s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="public-suit-choice")==true);s.Answer(c=>c.Parameters.GetValueOrDefault("suit")==nameof(Suit.Heart));
        s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="public-suit-discard")==true);
        var finalHand=s.V(1).Hand.Single().Id;s.Answer(c=>c.Cards.SequenceEqual(new[]{finalHand}));s.Answer(c=>c.Cards.Count==1);
        s.Reach(()=>s.WatchBinding=="discard");s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.PublicSuitDiscard is not null).PublicSuitDiscard is{Exhausted:true,PaidHandIds:{Count:1}}&&s.V(1).Equipment.Count==1,
            "The paid set covers every Hand card but deliberately leaves equipment: this still upgrades mobile Funan.");
        s.Cold();s.Continue();s.PlayBoundary(0);
        s.Need(s.Facts<ProgramResponseExchangeUpgradedEvent>().Count(e=>e.SkillId=="mobile:funan")==1&&s.V(0).Skills!.Any(k=>k.ContentId=="mobile:jiexun"),
            "Cold discard returns commit the original source-bound upgrade once without removing Jiexun.");
    }

    public static void AcquiredCounterspellDirectAnswerAndSettledRewardCold()
    {
        var s=CreateUpgradedScene(out var acquired);
        var original=s.V(0).Hand.First(c=>!acquired.Contains(c.Id)).Id;
        s.Play(0,CardKind.Duel,2,original);
        s.Reach(()=>s.P is{PlayerSeat:3,Kind:DecisionKind.Nullification});
        var first=s.V(3).Hand.First().Id;s.Answer(c=>c.Cards.SequenceEqual(new[]{first}));
        s.Reach(()=>s.P?.SkillPrompt?.SkillId=="mobile:funan");s.Activate();s.Reach(()=>s.WatchBinding=="claim");s.Continue();
        s.Reach(()=>s.P is{PlayerSeat:0,Kind:DecisionKind.Nullification});
        s.Answer(c=>c.Cards.SequenceEqual(new[]{acquired[0]}));
        var answered=s.Facts<CardActionAcceptedEvent>().Last().Action.ActionId;
        s.Reach(()=>s.P is{PlayerSeat:3,Kind:DecisionKind.Nullification});
        s.Need(s.RewardMoves==0&&s.G.ResolutionStack.OfType<NullificationWindowFrame>().Single().MobileFunanCompletions is{Cursor:0,Items:{Count:1}},
            "An acquired counterspell must await its real counter-counterspell opportunity on the original native window before rewarding.");
        s.Cold();var second=s.V(3).Hand.First().Id;s.Answer(c=>c.Cards.SequenceEqual(new[]{second}));
        var response=s.Facts<CardActionAcceptedEvent>().Last().Action.ActionId;
        s.Reach(()=>s.P?.SkillPrompt?.SkillId=="mobile:funan");s.Activate();s.Reach(()=>s.WatchBinding=="claim");s.Continue();
        s.Reach(()=>s.P is{PlayerSeat:0,Kind:DecisionKind.Nullification});s.Answer(c=>c.Cards.SequenceEqual(new[]{second}));
        var unanswered=s.Facts<CardActionAcceptedEvent>().Last().Action.ActionId;
        s.Reach(()=>s.WatchBinding=="reward");
        s.Need(s.RewardMoves==1&&s.Facts<MobileFunanDirectResponseObservedEvent>().Any(e=>e.ResponseActionId==response&&e.RespondedActionId==answered&&e.ResponderSeat==3)&&
            s.Facts<MobileFunanUseEntityCapturedEvent>().Any(e=>e.ActionId==unanswered&&e.CardId==second)&&
            s.G.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w=>w.Action.ActionId==unanswered&&w.CompletedResponseReturn is{Kind:ProgramCompletedResponseKind.Nullification,IsCommitted:false}),
            "The acquired counterspell answered by another role earns nothing; only the later actually unanswered acquired counterspell receives one typed native completion reward.");
        s.Cold();s.Continue();s.PlayBoundary(0);s.Need(s.RewardMoves==1,"The deferred response reward returns to the original counterspell window once across paid-child cold recovery.");
    }

    private static Scene CreateUpgradedScene(out int[] entities)
    {
        var s=Scene.Start(); var originals=s.V(0).Hand.Select(c=>c.Id).Take(3).ToArray(); var acquired=new List<int>();
        // These are three real paid Duels, not three copies of the existing
        // Slash/Duel/counterspell exchange matrix. They leave exactly one Hand
        // card for Jiexun's new first-invocation discard and upgrade condition.
        foreach(var original in originals)
        {
            s.Play(0,CardKind.Duel,1,original);
            s.Reach(()=>s.P is{PlayerSeat:1,Kind:DecisionKind.RespondSlash});
            var material=s.V(1).Hand.First().Id; s.Answer(c=>c.Cards.SequenceEqual(new[]{material}));
            s.Reach(()=>s.P?.SkillPrompt?.SkillId=="mobile:funan"); s.Activate();
            s.Reach(()=>s.WatchBinding=="claim");
            s.Need(s.Location(material)==CardLocation.Hand(0) && s.Location(original).Zone is(CardZoneKind.Processing or CardZoneKind.DiscardPile) &&
                s.G.CardMovements.All(m=>m.CardId!=original||m.Reason.Value!=ClaimReason) &&
                Enumerable.Range(0,4).All(seat=>s.G.CreateSnapshot(seat).TurnProhibitedPhysicalCards is null or{Count:0}),
                "Level-one mobile Funan directly takes the response entity, gives no original card and creates no old restriction.");
            if(acquired.Count==0)s.Cold(); s.Continue(); acquired.Add(material);
            s.Reach(()=>s.P is{PlayerSeat:0,Kind:DecisionKind.RespondSlash}); s.Answer(c=>c.Cards.Count==0); s.PlayBoundary(0);
            s.Need(s.Location(original)==CardLocation.DiscardPile && s.Location(material)==CardLocation.Hand(0) &&
                s.G.CardMovements.Count(m=>m.CardId==material&&m.Reason.Value==ClaimReason)==1,
                "The real original Duel finishes once without discarding or returning the acquired physical response entity.");
        }
        s.Need(s.V(1).HandCount==1,"The real response payments leave one Jiexun Hand cost.");
        s.End(); s.Reach(()=>s.P?.SkillPrompt?.SkillId=="mobile:jiexun"); s.Activate(); s.Answer(c=>c.Targets.SequenceEqual(new[]{1}));
        s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="public-suit-choice")==true);
        s.Cold(); s.Answer(c=>c.Parameters.GetValueOrDefault("suit")==nameof(Suit.Heart));
        s.Reach(()=>s.P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="public-suit-discard")==true);
        s.Need(s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.PublicSuitDiscard is not null).PublicSuitDiscard is{Required:1,ChosenSuit:Suit.Heart,DrawCount:0},
            "The chosen non-Diamond suit is frozen; first Jiexun discards one even when it draws zero.");
        s.Answer(c=>c.Cards.Count==1); s.Reach(()=>s.WatchBinding=="discard"); s.Cold(); s.Continue();
        s.PlayBoundary(0,nextTurn:true);
        s.Need(s.Facts<ProgramResponseExchangeUpgradedEvent>().Count(e=>e.SkillId=="mobile:funan")==1 &&
            s.V(0).Skills!.Any(k=>k.ContentId=="mobile:jiexun") && s.V(1).HandCount==0,
            "The exact paid Hand exhaustion upgrades the matching Funan source once and retains Jiexun.");

        entities=acquired.ToArray(); return s;
    }

    private sealed class Scene(GameEngine game,ContentRegistry content)
    {
        private readonly ContentRegistry Registry=content;
        internal GameEngine G=game;
        internal PendingDecision? P=>Enumerable.Range(0,4).Select(seat=>G.CreateSnapshot(seat,true).PendingDecision).FirstOrDefault(p=>p is not null);
        internal PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat,true).Players[seat];
        internal T[] Facts<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
        internal string? WatchBinding=>G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault() is{SkillId:Watch} f?f.TriggerId:null;
        internal int RewardMoves=>G.CardMovements.Count(m=>m.Reason.Value==RewardReason&&m.To==CardLocation.Hand(0));
        internal CardLocation Location(int id)=>G.CreateCardZoneDiagnostics().Single(c=>c.CardId==id).Location;
        internal void Need(bool value,string message){if(!value)throw new InvalidOperationException(message+Diagnostic());}
        private string Diagnostic()=>JsonSerializer.Serialize(new{G.State.TurnNumber,G.State.CurrentSeat,G.State.Phase,Pending=P,Players=G.CreateSnapshot(0,true).Players.Select(p=>new{p.Seat,p.Hp,p.HandCount,p.Equipment}),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())),
            Uses=Facts<CardActionAcceptedEvent>().Select(e=>new{e.Action.ActionId,e.Action.ParentActionId,e.Action.Type,e.Action.ActorSeat,e.Action.EffectiveKind,e.Action.PhysicalCards}),
            Captured=Facts<MobileFunanUseEntityCapturedEvent>(),Responses=Facts<MobileFunanDirectResponseObservedEvent>(),Upgrades=Facts<ProgramResponseExchangeUpgradedEvent>(),
            Facts=G.Events.TakeLast(22).Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())),Moves=G.CardMovements.TakeLast(18)});
        internal void Submit(GameCommand command){var result=G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());Need(result.Accepted,result.Error?.Message??"Native Xue Zong command rejected.");}
        internal void Answer(Func<PromptChoice,bool> pick){var choice=P!.Choices.FirstOrDefault(pick);Need(choice is not null,"Missing real Xue Zong choice.");Submit(new AnswerPromptCommand(P!.PlayerSeat,P.PromptId,choice!.Id,G.Revision));}
        internal void Activate()=>Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
        internal void Continue()=>Answer(c=>c.Parameters.GetValueOrDefault("option-id")=="continue");
        internal void End()=>Submit(new EndPlayPhaseCommand(P!.PlayerSeat,G.Revision,P.PromptId));
        internal void PlayBoundary(int seat,bool nextTurn=false){var turn=G.State.TurnNumber;Reach(()=>P is{Kind:DecisionKind.PlayCard} p&&p.PlayerSeat==seat&&(!nextTurn||G.State.TurnNumber>turn));}
        internal long Play(int seat,CardKind kind,int target,int? id=null,bool native=false)
        {
            Need(P is{Kind:DecisionKind.PlayCard} p&&p.PlayerSeat==seat,"The actor owns a real Play boundary.");
            var actions=G.GetLegalActions(seat);
            var action=actions.FirstOrDefault(a=>a.CardId is not null&&(id is null||a.CardId==id)&&(a.PlayedCardKind??V(seat).Hand.FirstOrDefault(c=>c.Id==a.CardId)?.Kind)==kind&&a.Kind!=LegalActionKind.Recast&&
                (native?a.ConversionSource is null:a.ConversionSource is not null)&&(a.TargetSeats.Count==0||a.TargetSeats.SequenceEqual(new[]{target})));
            Need(action?.CardId is not null,"Missing actual card action for "+kind+" seat "+seat);
            Submit(new PlayCardCommand(seat,action!.CardId!.Value,action.TargetSeats,G.Revision,P!.PromptId,action.PlayedCardKind,action.TargetCardId){ConversionSource=action.ConversionSource,AdditionalConversionSources=action.AdditionalConversionSources??[]});
            return G.ResolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId??Facts<CardActionAcceptedEvent>().LastOrDefault()?.Action.ActionId??0;
        }
        internal void Reach(Func<bool> done,[CallerLineNumber]int line=0)
        {
            for(var n=0;n<240;n++)
            {
                if(done())return;Need(G.State.Winner==Winner.None,"Unexpected terminal result at "+line);
                if(P is{Kind:DecisionKind.SelectGeneral} setup){Submit(new SelectGeneralCommand(setup.PlayerSeat,setup.PlayerSeat==0?Hero:Peer+setup.PlayerSeat,G.Revision,setup.PromptId));continue;}
                if(P is{Kind:DecisionKind.PlayCard}){End();continue;}
                if(P is{Kind:DecisionKind.DiscardCards} cost){Submit(new DiscardCardsCommand(cost.PlayerSeat,cost.ValidCardIds.Take(cost.RequiredCardCount).ToArray(),cost.PromptId,G.Revision));continue;}
                if(P is not null)
                {
                    if(WatchBinding is not null){Continue();continue;}
                    if(P.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="skip")){Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="skip");continue;}
                    if(P.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge or DecisionKind.Nullification or DecisionKind.RescueDying){Answer(c=>c.Cards.Count==0);continue;}
                    Need(false,"Unexpected native Xue Zong boundary at "+line);
                }
                Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false,"Missing bounded native Xue Zong flow at "+line);
        }
        private string State()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,4).Select(seat=>SnapshotJson.Serialize(G.CreateSnapshot(seat))).ToArray(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),Events=G.Events.Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())).ToArray(),G.CardMovements,Commands=CommandJson.Serialize(G.AcceptedCommands)});
        internal void Cold(){var before=State();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),content);Need(before==State(),"Cold retains the original paid entity/discard/draw frame, private chooser and typed native parent return.");}
        internal static Scene Start(bool cap=false)
        {
            var stock=Enumerable.Range(0,100).Select(n=>new ContentDeckPhysicalCard("standard:crossbow",Suit.Spade,n%13+1)).ToArray();
            if(!cap)return Boot(ContentRegistry.Build(new StandardContentPackage(),new Fixture(stock)));
            var probe=Boot(ContentRegistry.Build(new StandardContentPackage(),new Fixture(stock)));
            var hands=new[]{new[]{CardKind.Crossbow,CardKind.BaguaFormation,CardKind.OffensiveHorse,CardKind.DefensiveHorse},new[]{CardKind.Crossbow,CardKind.BaguaFormation,CardKind.Crossbow,CardKind.Crossbow}};
            for(var seat=0;seat<hands.Length;seat++)
            {
                var ids=probe.V(seat).Hand.Select(c=>c.Id).ToArray();probe.Need(ids.Length==4,"The fixed native opening has four actual Hand entities.");
                for(var i=0;i<4;i++)stock[ids[i]-1]=new(probe.Registry.Cards.Values.First(c=>c.LegacyKind==hands[seat][i]).Id,Suit.Spade,7);
            }
            return Boot(ContentRegistry.Build(new StandardContentPackage(),new Fixture(stock)));
        }
        private static Scene Boot(ContentRegistry registry)
        {
            var game=GameEngine.CreateStandard(new GameOptions{Seed=17,PlayerCount=4,HumanSeat=0,HumanSeats=[0,1,2,3],HumanRole=Role.Lord,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=true,AdvanceAfterHumanCommands=false,MaxTurns=20},registry);
            var scene=new Scene(game,registry);scene.Submit(new StartGameCommand());scene.PlayBoundary(0);return scene;
        }
    }

    private sealed class Fixture(ContentDeckPhysicalCard[] stock):IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-mobile-xue",new Version(1,0,0),[]);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,"mobile:xue-zong");
            var rules=JsonNode.Parse("""
            {"skills":[
             {"id":"fixture:mobile-xue-driver","revision":1,"activations":[{"id":"discard-one","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]}],"viewAs":[
              {"id":"duel","inputKinds":[],"inputSuits":[],"outputKind":"duel","forPlay":true,"forResponse":false,"useOnly":true,"singleCardTrickUse":true},
              {"id":"slash","inputKinds":[],"inputSuits":[],"outputKind":"slash","forPlay":true,"forResponse":true},
              {"id":"dodge","inputKinds":[],"inputSuits":[],"outputKind":"dodge","forPlay":false,"forResponse":true}
              ,{"id":"counter","inputKinds":[],"inputSuits":[],"outputKind":"nullification","forPlay":false,"forResponse":true,"extendedUse":true}
             ],"triggers":[{"id":"skipdraw","window":"drawPhaseStarting","drawPhaseMode":"replacement","subject":"owner","optional":false,"effects":[{"op":"draw","target":"owner","amount":0}]}]},
             {"id":"fixture:mobile-xue-watch","revision":1,"triggers":[
              {"id":"claim","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.response-entity-exchange"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"watch","options":[{"id":"continue"}]}]},
              {"id":"reward","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.mobile:funan.Draw"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"watch","options":[{"id":"continue"}]}]},
              {"id":"suit-draw","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.public-suit-escalating-draw"],"movementOccurrence":"perBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"watch","options":[{"id":"continue"}]}]},
              {"id":"discard","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementReasons":["skill-program.public-suit-escalating-discard"],"movementOccurrence":"perOwnerBatch","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"watch","options":[{"id":"continue"}]}]}
             ]}
            ]}
            """)!;
            rules["schemaVersion"]=SkillProgramCatalog.RulesSchemaVersion;
            var catalog=SkillProgramCatalog.Load(rules.ToJsonString(),"""
            {"schemaVersion":3,"skills":{"fixture:mobile-xue-driver":{"name":"实际用牌与打出驱动","description":"测试"},"fixture:mobile-xue-watch":{"name":"实际移牌观察","description":"测试","optionLabels":{"continue":"继续"}}}}
            """);
            foreach(var entry in catalog.Programs)b.AddSkill(new(entry.Key,entry.Key,"测试"){Program=entry.Value});
            b.AddGeneral(new(Hero,"移动薛综测试","supporter","mobile:funan","wu",20,["mobile:jiexun",Driver,Watch]));
            for(var seat=1;seat<4;seat++)b.AddGeneral(new(Peer+seat,"实体目标","supporter",Driver,"shu",20,[Watch]));
            b.AddDeck(new("fixture:mobile-xue-deck","真实固定牌堆",4,0,[]){PhysicalCards=stock});
            b.AddMode(new(Mode,"移动薛综原生规则",4,4,new Dictionary<string,int>{[nameof(Role.Lord)]=1,[nameof(Role.Loyalist)]=1,[nameof(Role.Rebel)]=2},"fixture:mobile-xue-deck",GeneralCandidateCount:4,GeneralPoolIds:[Hero,Peer+1,Peer+2,Peer+3]));
        }
    }
}
