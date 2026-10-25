using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class MobileSunHaoChecks
{
    private const string Can = "mobile:sun-hao-canshi", Gui = "mobile:guiming", Driver = "fixture:mobile-sun-hao-driver", Watch = "fixture:mobile-sun-hao-watch", Foreign = "fixture:mobile-sun-hao-foreign";
    private const string Mode = "identity:classic-mobile-sun-hao", Owner = "fixture:mobile-sun-hao-owner";
    private const string Draw = "skill-program.mobile:sun-hao-canshi.draw", Discard = "skill-program.sun-hao.canshi.discard";

    public static void ReplacementZeroNarrowGuimingAndNativeDrawLossCold()
    {
        foreach (var scene in new[] { "replace", "decline", "zero", "lord", "nonlord", "draw-loss" })
        {
            var (game, registry) = Start(scene);
            var before = View(game,0).HandCount;
            var expected = scene is "zero" or "nonlord" ? 0 : scene == "lord" ? 3 : 1;
            if (scene == "decline")
            {
                Answer(game,c => c.Parameters.GetValueOrDefault("program-action") == "skip"); Play(game);
                Need(View(game,0).HandCount == before + 3 && Facts<SunHaoCanshiGrantedEvent>(game).Length == 0 && Facts<MobileSunHaoDrawIssuedEvent>(game).Length == 0,
                    "Declining mobile Canshi retains the complete normal two-card draw plus its real +1 modifier.");
                continue;
            }
            Activate(game);
            if (expected > 0)
            {
                Reach(game,() => WatchBinding(game) == "draw-paid");
                Need(game.ResolutionStack.OfType<ProgramSkillFrame>().Any(f => f.SkillId == Can && f.MobileSunHaoDraw?.Outcome?.ActualDrawIds.Count == expected) &&
                    Facts<SunHaoCanshiGrantedEvent>(game).Length == 0,
                    "The actual replacement draw pauses above its original typed outcome before issuing the turn obligation.");
                game = Cold(game,registry); Continue(game);
            }
            Play(game);
            var grant = Facts<SunHaoCanshiGrantedEvent>(game).Single();
            Need(grant.X == expected && View(game,0).HandCount == before + expected && game.CardMovements.Count(m => m.Reason.Value == Draw && m.To == CardLocation.Hand(0)) == expected,
                "Mobile Canshi replaces the complete normal draw with exactly frozen X, including zero, rather than adding X or retaining modifiers. " + Diag(game));
            Need(Facts<MobileSunHaoDrawIssuedEvent>(game).Length == 1 && Facts<MobileSunHaoDrawReturnedEvent>(game).Length == 1,
                "Cold native draw return never reissues the same replacement or obligation.");
            if (scene is "lord" or "nonlord")
                Need(Enumerable.Range(1,3).All(seat => !RuleWounded(game,seat) && View(game,seat).Hp == View(game,seat).MaxHp) && grant.X == (scene == "lord" ? 3 : 0),
                    "Only the qualified Lord counts full-HP Wu peers for Canshi; mobile Guiming never alters their global wounded queries, real HP or recovery space.");
            if (scene == "draw-loss")
                Need(View(game,0).Skills!.All(s => s.ContentId != Can), "The real draw-gain child removes the HP-qualified source before the issued obligation resumes.");
            if (scene is "zero" or "draw-loss")
            {
                Native(game,a => a.Kind == LegalActionKind.Alcohol);
                Reach(game,() => Cost(game)); Pay(game); Reach(game,() => WatchBinding(game) == "discard-paid");
                game = Cold(game,registry); Continue(game); Play(game);
                Need(Facts<SunHaoCanshiDiscardedEvent>(game).Length == 1,
                    "Even an X=0 replacement, or a source lost during the real draw child, leaves one genuine Basic Use obligation.");
            }
            _ = Cold(game,registry);
        }
    }

    public static void BasicDelayedAndTypedResponseUsesPaidColdAfterSourceLoss()
    {
        foreach (var scene in new[] { "peach", "alcohol", "lightning", "dodge", "nullification" })
        {
            var (game, registry) = Start(scene); Activate(game);
            Reach(game,() => WatchBinding(game) == "draw-paid"); Continue(game); Play(game);
            if (scene == "dodge") IncomingSlash(game);
            else if (scene == "nullification")
            {
                ForeignTrick(game);
                Reach(game,() => Pending(game) is { PlayerSeat: 0, Kind: DecisionKind.Nullification });
                Answer(game,c => c.Cards.Count == 1);
            }
            else Native(game,a => a.Kind == (scene == "peach" ? LegalActionKind.Peach : scene == "alcohol" ? LegalActionKind.Alcohol : LegalActionKind.Lightning));
            Reach(game,() => Cost(game));
            var invoice = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == Can && f.SunHaoCanshi is not null).SunHaoCanshi!;
            if (scene is "dodge" or "nullification")
                Need(invoice.UseId is null && game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(w => w.Id == invoice.WindowId &&
                    w.CompletedResponseReturn is { IsCommitted: true } typed && typed.Kind == (scene == "dodge" ? ProgramCompletedResponseKind.Dodge : ProgramCompletedResponseKind.Nullification)),
                    "Real Dodge and Nullification uses retain the native committed typed response, rather than manufacturing a separate Use frame.");
            Pay(game); Reach(game,() => WatchBinding(game) == "discard-paid");
            var paid = Facts<SunHaoCanshiDiscardedEvent>(game).Single();
            var restored = Cold(game,registry);
            foreach (var branch in new[] { game, restored })
            {
                Continue(branch); Play(branch);
                Need(Facts<SunHaoCanshiDiscardedEvent>(branch).Single() == paid && branch.CardMovements.Count(m => m.CardId == paid.CardId && m.Reason.Value == Discard) == 1,
                    "A paid Basic, delayed trick or response-use discard returns to its original native action once after cold recovery.");
            }
            Equal(game,restored);
            if (scene == "dodge")
            {
                Need(View(game,0).Skills!.All(s => s.ContentId != Can), "The first real paid-discard child must actually remove the HP-qualified Canshi source.");
                IncomingSlash(game); Reach(game,() => Cost(game)); Pay(game); Play(game);
                Need(Facts<SunHaoCanshiDiscardedEvent>(game).Length == 2 && Facts<SunHaoCanshiStartedEvent>(game).Select(e => e.ActionId).Distinct().Count() == 2,
                    "A second real Dodge still opens the issued response-use obligation after source loss, without duplicating either physical response payment.");
            }
            _ = Cold(game,registry);
        }
    }

    public static void PlayedResponsesDoNotPayAndActualTurnReturnExpires()
    {
        foreach (var scene in new[] { "played-slash", "played-dodge" })
        {
            var (game, _) = Start(scene); Activate(game); Reach(game,() => WatchBinding(game) == "draw-paid"); Continue(game); Play(game);
            if(scene=="played-slash")Use(game,"duel",1);
            else ForeignArrowFromDiscard(game);
            var kind = scene == "played-slash" ? DecisionKind.RespondSlash : DecisionKind.RespondDodge;
            Reach(game,() => Pending(game) is { PlayerSeat: 0 } prompt && prompt.Kind == kind);
            var card = Pending(game)!.Choices.First(c => c.Cards.Count == 1).Cards.Single();
            Answer(game,c => c.Cards.SequenceEqual(new[] { card })); Play(game);
            Need(game.CardMovements.Any(m => m.CardId == card && m.From == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Respond) &&
                Facts<SunHaoCanshiStartedEvent>(game).Length == 0 && Facts<SunHaoCanshiDiscardedEvent>(game).Length == 0,
                "A precisely selected physical Slash in Duel or Dodge in Arrow Barrage is really played and never charged as a mobile Canshi Use. " + Diag(game));
        }
        var (expired, registry) = Start("dodge"); Activate(expired); Reach(expired,() => WatchBinding(expired) == "draw-paid"); Continue(expired); Play(expired);
        var turn = expired.State.TurnNumber;
        Accept(expired,new EndPlayPhaseCommand(0,expired.Revision,Pending(expired)!.PromptId));
        Reach(expired,() => expired.State.CurrentSeat == 1 && expired.State.TurnNumber > turn && Pending(expired) is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        Need(Facts<ActualTurnCursorReturnedEvent>(expired).Any(e => e.ActualTurn == turn && e.OwnerSeat == 0), "The old obligation expires only after the actual source turn cursor returns.");
        expired = Cold(expired,registry);
        Accept(expired,new UseProgramSkillCommand(1,Driver,"self-slash",[],[0],expired.Revision,Pending(expired)!.PromptId));
        Reach(expired,() => Pending(expired) is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge });
        Answer(expired,c => c.Cards.Count == 1);
        Reach(expired,() => expired.ResolutionStack.Count == 0 && Pending(expired) is { PlayerSeat: 1, Kind: DecisionKind.PlayCard });
        Need(Facts<SunHaoCanshiStartedEvent>(expired).Length == 0 && Facts<SunHaoCanshiDiscardedEvent>(expired).Length == 0,
            "The next actual turn's genuine Dodge pays no expired mobile obligation after cold restoration.");
    }

    private static void IncomingSlash(GameEngine game)
    {
        Use(game,"incoming",1);
        Reach(game,() => Pending(game)?.Choices.Any(c => c.Parameters.GetValueOrDefault("offer-option") == "slash") == true);
        Answer(game,c => c.Parameters.GetValueOrDefault("offer-option") == "slash");
        Reach(game,() => Pending(game)?.Choices.Any(c => c.Parameters.GetValueOrDefault("offer-option") == "target-0") == true);
        Answer(game,c => c.Parameters.GetValueOrDefault("offer-option") == "target-0");
        Reach(game,() => Pending(game) is { PlayerSeat: 0, Kind: DecisionKind.RespondDodge });
        Answer(game,c => c.Cards.Count == 1);
    }
    private static void ForeignTrick(GameEngine game)
    {
        Accept(game,new EndPlayPhaseCommand(0,game.Revision,Pending(game)!.PromptId));
        Reach(game,() => Pending(game)?.SkillPrompt?.SkillId == Foreign && Pending(game)!.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
        Activate(game);
        Reach(game,() => Pending(game)?.SkillPrompt?.SkillId == Foreign && Pending(game)!.Choices.Any(c => c.Targets.SequenceEqual(new[] { 1 })));
        Answer(game,c => c.Targets.SequenceEqual(new[] { 1 }));
    }
    private static void ForeignArrowFromDiscard(GameEngine game)
    {
        var arrow=View(game,0).Hand.Single(c=>c.Kind==CardKind.ArrowBarrage).Id;
        Accept(game,new UseProgramSkillCommand(0,Driver,"drop",[arrow],[],game.Revision,Pending(game)!.PromptId));
        Play(game);
        Accept(game,new EndPlayPhaseCommand(0,game.Revision,Pending(game)!.PromptId));
        Reach(game,()=>Pending(game)?.Choices.Any(c=>c.Cards.SequenceEqual(new[]{arrow})&&c.Targets.SequenceEqual(new[]{1}))==true);
        Answer(game,c=>c.Cards.SequenceEqual(new[]{arrow})&&c.Targets.SequenceEqual(new[]{1}));
        Reach(game,()=>Pending(game) is{PlayerSeat:0,Kind:DecisionKind.RespondDodge});
        Need(game.ResolutionStack.OfType<CardUseFrame>().Any(use=>use.CardId==arrow&&use.CardKind==CardKind.ArrowBarrage&&use.SourceSeat==1&&
            use.Action is{Type:CardActionType.Use,ActorSeat:1,ProviderSeat:1} action&&action.PhysicalCards.Any(c=>c.CardId==arrow&&c.From==CardLocation.DiscardPile)),
            "The existing Zhiwang producer must issue the exact discarded physical Arrow Barrage as the foreign actor's own native Use.");
    }
    private static PlayerSnapshot View(GameEngine game,int seat) => game.CreateSnapshot(seat,true).Players[seat];
    private static PendingDecision? Pending(GameEngine game) => Enumerable.Range(0,4).Select(s => game.CreateSnapshot(s,true).PendingDecision).FirstOrDefault(p => p is not null);
    private static T[] Facts<T>(GameEngine game) where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static bool Cost(GameEngine game) => Pending(game)?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "sun-hao-canshi") == true;
    private static string? WatchBinding(GameEngine game) => game.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault() is { SkillId: Watch } f ? f.TriggerId : null;
    private static void Activate(GameEngine game) => Answer(game,c => c.Parameters.GetValueOrDefault("program-action") == "activate");
    private static void Continue(GameEngine game) => Answer(game,c => c.Parameters.GetValueOrDefault("option-id") == "continue");
    private static void Pay(GameEngine game) => Answer(game,c => c.Cards.Count == 1);
    private static void Use(GameEngine game,string id,int target) => Accept(game,new UseProgramSkillCommand(0,Driver,id,[],[target],game.Revision,Pending(game)!.PromptId));
    private static void Answer(GameEngine game,Func<PromptChoice,bool> pick)
    { var prompt = Pending(game)!; var choice = prompt.Choices.FirstOrDefault(pick); Need(choice is not null,"Missing actual native choice: "+Diag(game)); Accept(game,new AnswerPromptCommand(prompt.PlayerSeat,prompt.PromptId,choice!.Id,game.Revision)); }
    private static void Native(GameEngine game,Func<LegalAction,bool> pick)
    { var action = game.GetHumanLegalActions().FirstOrDefault(pick); Need(action?.CardId is not null,"Missing native card action: "+Diag(game)); Accept(game,new PlayCardCommand(0,action!.CardId!.Value,action.TargetSeats,game.Revision,Pending(game)!.PromptId,action.PlayedCardKind,action.TargetCardId){ConversionSource=action.ConversionSource,AdditionalConversionSources=action.AdditionalConversionSources??[]}); }
    private static void Accept(GameEngine game,GameCommand command)
    { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted,result.Error?.Message??"Rejected mobile Sun Hao command."); }
    private static void Play(GameEngine game) => Reach(game,() => Pending(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
    private static void Reach(GameEngine game,Func<bool> done)
    {
        for (var step=0;step<240;step++)
        {
            if(done())return;
            var prompt=Pending(game);
            if(prompt is null||!View(game,prompt.PlayerSeat).IsHuman){Accept(game,new AdvanceOneStepCommand(game.Revision));continue;}
            if(Cost(game))Pay(game);
            else if(prompt.Kind==DecisionKind.SelectGeneral)Accept(game,new SelectGeneralCommand(prompt.PlayerSeat,prompt.PlayerSeat==0?Owner:"fixture:mobile-sun-hao-peer-"+prompt.PlayerSeat,game.Revision,prompt.PromptId));
            else if(prompt.Choices.Any(c=>c.Parameters.GetValueOrDefault("option-id")=="continue"))Continue(game);
            else if(prompt.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="skip"))Answer(game,c=>c.Parameters.GetValueOrDefault("program-action")=="skip");
            else if(prompt.Kind==DecisionKind.PlayCard)Accept(game,new EndPlayPhaseCommand(prompt.PlayerSeat,game.Revision,prompt.PromptId));
            else if(prompt.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge or DecisionKind.Nullification or DecisionKind.RescueDying)Answer(game,c=>c.Cards.Count==0);
            else if(prompt.Kind==DecisionKind.SelectTargetCard)Answer(game,_=>true);
            else Accept(game,new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Missing mobile Sun Hao native boundary: "+Diag(game));
    }
    private static string Diag(GameEngine game) => JsonSerializer.Serialize(new { game.State.TurnNumber,game.State.CurrentSeat,game.State.Phase,Pending=Pending(game),
        Frames=game.ResolutionStack.TakeLast(5).Select(f=>new{f.Id,Kind=f.GetType().Name,Draw=(f as ProgramSkillFrame)?.MobileSunHaoDraw,Cost=(f as ProgramSkillFrame)?.SunHaoCanshi}),
        Grants=Facts<SunHaoCanshiGrantedEvent>(game),Payments=Facts<SunHaoCanshiDiscardedEvent>(game) });
    private static string State(GameEngine game) => JsonSerializer.Serialize(new { Views=Enumerable.Range(0,4).Select(s=>SnapshotJson.Serialize(game.CreateSnapshot(s))),
        Frames=game.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())),game.CardMovements,Events=game.Events.Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())),Commands=CommandJson.Serialize(game.AcceptedCommands) });
    private static void Equal(GameEngine first,GameEngine second) => Need(State(first)==State(second),"Cold mobile Canshi retains all views, original parents, typed draws, accepted response uses and once-paid card movements.");
    private static GameEngine Cold(GameEngine game,ContentRegistry registry)
    { var copy=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())),registry); Equal(game,copy); return copy; }
    private static bool RuleWounded(GameEngine game,int seat)
    { var players=(System.Collections.IList)typeof(GameEngine).GetField("_players",BindingFlags.Instance|BindingFlags.NonPublic)!.GetValue(game)!;
      return (bool)typeof(GameEngine).GetMethod("IsRuleWounded",BindingFlags.Instance|BindingFlags.NonPublic)!.Invoke(game,[players[seat]])!; }
    private static void Need(bool condition,string message) { if(!condition)throw new InvalidOperationException(message); }
    private static (GameEngine,ContentRegistry) Start(string scene)
    {
        var registry=ContentRegistry.Build(new StandardContentPackage(),new Fixture(scene));
        var options=new GameOptions{Seed=7,PlayerCount=4,HumanSeat=0,HumanSeats=[0,1,2,3],HumanRole=scene=="nonlord"?Role.Rebel:Role.Lord,
            ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=8};
        if(scene=="played-dodge")
        {
            var setup=new ShanheBattleSetup(7,1,"mobile-sun-hao-native-played-dodge",Owner,11,0,0,0,0,[],
                [new(CardKind.Dodge,Suit.Club,2),new(CardKind.Dodge,Suit.Club,3),new(CardKind.Dodge,Suit.Club,4),new(CardKind.ArrowBarrage,Suit.Heart,1)],
                [new(CardKind.Crossbow,Suit.Club,1)],
                [new("fixture:mobile-sun-hao-peer-1",Hp:12){StartingHand=[]},new("fixture:mobile-sun-hao-peer-2",Hp:12){StartingHand=[]}],
                [new("fixture:mobile-sun-hao-peer-3",Hp:12){StartingHand=[]}],new string('a',64));
            options=options with{Seed=setup.BattleSeed,HumanSeats=null,HumanRole=null,HumanTeamId=ShanheBattleSetup.PlayerTeamId,ModeId=ShanheBattleSetup.BattleModeId,ShanheBattle=setup,UseInteractiveSetup=false};
        }
        var game=GameEngine.CreateStandard(options,registry);
        Accept(game,new StartGameCommand()); Reach(game,()=>Pending(game)?.SkillPrompt?.SkillId==Can&&Pending(game)!.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="activate"));
        return(game,registry);
    }
    private sealed class Fixture(string scene) : IGameContentPackage
    {
        public PackageManifest Manifest { get; }=new("fixture-mobile-sun-hao",new(1,0,0),[]);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder,"mobile:sun-hao","ol:sun-hao");
            if(scene=="played-dodge")CharacterFixtureContent.Register(builder,"mou:cao-ang");
            var sourceLoss=scene is "draw-loss" or "dodge";
            var kind=scene=="peach"?CardKind.Peach:scene=="lightning"?CardKind.Lightning:scene is "dodge" or "played-dodge"?CardKind.Dodge:scene=="nullification"?CardKind.Nullification:scene=="played-slash"?CardKind.Slash:CardKind.Alcohol;
            var definitions=new List<JsonNode>();
            void Add(string json)=>definitions.Add(JsonNode.Parse(json)!);
            Add("""
            {"id":"fixture:mobile-sun-hao-driver","revision":1,"modifiers":[{"id":"normal-plus-one","query":"drawCount","operation":"add","value":1,"priority":0},{"id":"keep","query":"handLimit","operation":"add","value":20,"priority":0}],"activations":[
              {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"offerVirtualSlashOrDraw","target":"selectedTarget"}]},
              {"id":"drop","minCards":1,"maxCards":1,"sourceZones":["hand"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"captureSelectedCards","target":"owner","resultBind":"drop"},{"op":"moveBoundCards","target":"owner","sourceBind":"drop","destination":"discardPile","awaitMovementTriggers":true}]},
              {"id":"self-slash","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"useVirtualSlash","target":"selectedTarget"}]}]}
            """);
            if(scene=="played-slash")definitions[0]!["activations"]!.AsArray().Add(JsonNode.Parse("""{"id":"duel","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"usesPerPhase":2,"effects":[{"op":"useSelectedActorDuel","target":"owner"}]}"""));
            Add("""{"id":"fixture:mobile-sun-hao-hurt","revision":1,"triggers":[{"id":"actual-injury","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"loseHp","target":"owner","amount":1}]}]}""");
            if(sourceLoss)Add("""{"id":"fixture:mobile-sun-hao-qualified","revision":1,"hpSkillGrants":[{"id":"actual-paid-source","skillId":"mobile:sun-hao-canshi","minimumHp":13}]}""");
            if(scene=="nullification")
            {
                var foreign=JsonNode.Parse("""
                {"id":"fixture:mobile-sun-hao-foreign","revision":1,"triggers":[{"id":"native-other-use-at-play-ending","window":"playEnding","subject":"owner","optional":true,"usageScope":"game","usageLimit":1,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLiving"},{"op":"useVirtualOrdinaryTrick","target":"owner","actorRef":{"kind":"selectedTarget"},"targetRef":{"kind":"owner"},"outputKind":"dismantlement"}]}]}
                """)!;
                definitions.Add(foreign);
            }
            var watch=JsonNode.Parse("""
            {"id":"fixture:mobile-sun-hao-watch","revision":1,"triggers":[
              {"id":"draw-paid","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementReasons":["skill-program.mobile:sun-hao-canshi.draw"],"movementOccurrence":"perBatch","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"draw-paid","options":[{"id":"continue"}]}]},
              {"id":"discard-paid","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementReasons":["skill-program.sun-hao.canshi.discard"],"movementOccurrence":"perBatch","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"discard-paid","options":[{"id":"continue"}]}]}]}
            """)!;
            if(sourceLoss)watch["triggers"]![scene=="draw-loss"?0:1]!["effects"]!.AsArray().Insert(0,JsonNode.Parse("""{"op":"loseHp","target":"owner","amount":1}"""));
            definitions.Add(watch);
            var labels=definitions.ToDictionary(n=>n["id"]!.GetValue<string>(),n=>
            {
                var label=new Dictionary<string,object>{{"name","真实原生驱动"},{"description","移动孙皓差异检查"}};
                if(n["id"]!.GetValue<string>()==Watch)label["optionLabels"]=new Dictionary<string,string>{{"continue","继续"}};
                return label;
            });
            var catalog=SkillProgramCatalog.Load(JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.RulesSchemaVersion,skills=definitions}),JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=labels}));
            foreach(var(id,program)in catalog.Programs)builder.AddSkill(new(id,id,id){Program=program,ProgramPresentation=catalog.Presentations[id]});
            var ownerSkills=new List<string>{Driver,Watch,sourceLoss?"fixture:mobile-sun-hao-qualified":Can};
            if(scene is "lord" or "nonlord")ownerSkills.Add(Gui);
            if(scene=="nullification")ownerSkills.Add(Foreign);
            if(scene=="played-dodge")ownerSkills.Add("mou:zhiwang");
            if(scene=="peach")ownerSkills.Add("fixture:mobile-sun-hao-hurt");
            builder.AddGeneral(new(Owner,"移动孙皓实际规则","supporter","standard:none","wu",12,ownerSkills));
            for(var seat=1;seat<4;seat++)builder.AddGeneral(new("fixture:mobile-sun-hao-peer-"+seat,"实际其他角色","supporter",scene=="played-dodge"?"standard:none":Driver,"wu",12,
                seat==1&&scene is not("zero"or"lord"or"nonlord")?["fixture:mobile-sun-hao-hurt"]:[]));
            builder.AddCard(new("fixture:mobile-sun-hao-card","原生"+kind,CardCatalog.Get(kind).CategoryName,"真实固定同质实体",kind));
            builder.AddDeck(new("fixture:mobile-sun-hao-deck","真实同质牌堆",6,2,[]){PhysicalCards=Enumerable.Range(0,160).Select(_=>new ContentDeckPhysicalCard("fixture:mobile-sun-hao-card",Suit.Spade,7)).ToArray()});
            builder.AddMode(new(Mode,"移动孙皓真实规则",4,4,new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Rebel),3}},"fixture:mobile-sun-hao-deck",GeneralCandidateCount:4,
                GeneralPoolIds:[Owner,"fixture:mobile-sun-hao-peer-1","fixture:mobile-sun-hao-peer-2","fixture:mobile-sun-hao-peer-3"]));
            if(scene=="played-dodge")builder.AddMode(new(ShanheBattleSetup.BattleModeId,"真实外来万箭打出闪",4,4,new Dictionary<string,int>(),"fixture:mobile-sun-hao-deck",
                GeneralPoolIds:[Owner,"fixture:mobile-sun-hao-peer-1","fixture:mobile-sun-hao-peer-2","fixture:mobile-sun-hao-peer-3"],ModeKind:ContentModeKind.Team,
                TeamCounts:new Dictionary<string,int>{{ShanheBattleSetup.PlayerTeamId,2},{ShanheBattleSetup.EnemyTeamId,2}}));
        }
    }
}
