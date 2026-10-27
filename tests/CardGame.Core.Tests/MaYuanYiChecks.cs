using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MaYuanYiChecks
{
    private static string ActiveScene="";
    private const string Owner="fixture:my-owner",Driver="fixture:my-driver",Quiet="fixture:my-quiet",Watch="fixture:my-watch",Health="fixture:my-health",Mode="identity:classic-ma-yuan-yi",Jibing="ol:jibing",Wangjing="ol:wangjing",Moucuan="ol:moucuan",Binghuo="ol:binghuo";
    public static void PublishedBingAiMaterialUsesItsExactPublicPileCold()
    {
        var (g, r) = Start("slash"); Store(g);
        foreach (var cold in new[] { false, true })
        {
            if (cold) g = Cold(g, r);
            var view = g.CreateSnapshot(0);
            var actions = g.GetHumanLegalActions();
            var bing = actions.First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([1]) &&
                action.ConversionSource is { SkillId: Jibing, BindingId: "bing-slash", OwnerSeat: 0 } source &&
                (view.Players[0].PublicPersistentPiles?.Any(pile => pile.SourceSkillId == Jibing &&
                    pile.SourceSkillInstanceId == source.SkillInstanceId && pile.Cards.Any(card => card.Id == action.CardId)) == true ||
                    view.Players[0].PublicPersistentPiles is null && view.Players[0].PublicPersistentPileSkillId == Jibing &&
                    view.Players[0].PublicPersistentPileCards?.Any(card => card.Id == action.CardId) == true));
            var end = actions.Single(action => action.Kind == LegalActionKind.EndPlay);
            var before = SnapshotJson.Serialize(view);
            Need(bing.CardId is > 0 && view.Players[0].Hand.All(card => card.Id != bing.CardId),
                "A real published Jibing Slash pays a true public pile entity that is absent from its owner's Hand.");
            foreach (var policy in new[] { AiDecisionPolicy.IdentityStrategyVersion, AiDecisionPolicy.RebelStrategyVersion,
                AiDecisionPolicy.CurrentVersion })
            {
                var choice = new SimpleAiBrain(0, 83, policy).ChoosePlay(view, [bing, end], 1);
                Need(choice.Action == bing && choice.Thought.Candidates.All(candidate => double.IsFinite(candidate.Score)),
                    "Both current identity policies must score and retain the exact published Bing conversion instead of looking for an invented Hand material.");
            }
            Need(SnapshotJson.Serialize(view) == before, "Bing material scoring must preserve the real supplied player view.");
        }
    }
    private static PlayerSnapshot V(GameEngine g,int s)=>g.CreateSnapshot(s).Players[s];
    private static PendingDecision? P(GameEngine g)=>Enumerable.Range(0,4).Select(s=>g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p=>p is not null);
    private static bool Action(GameEngine g,string s)=>P(g)?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")==s)==true;
    private static bool Optional(GameEngine g,string s)=>P(g)is{PlayerSeat:0}p&&p.SkillPrompt?.SkillId==s&&Action(g,"activate");
    private static void Need(bool yes,string why){if(!yes)throw new InvalidOperationException(why);}
    private static void Accept(GameEngine g,GameCommand c){try{var result=g.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single());Need(result.Accepted,result.Error?.Message??"Rejected real Ma Yuan Yi command.");}catch(Exception e){throw new InvalidOperationException("scene="+ActiveScene+" command="+c.GetType().Name+" cause="+e.GetType().Name+": "+e.Message+" "+Diagnostic(g),e);}}
    private static void Answer(GameEngine g,Func<PromptChoice,bool> match){var p=P(g)!;var c=p.Choices.FirstOrDefault(match);Need(p.PlayerSeat==0&&c is not null,"Missing actual human choice: "+Diagnostic(g));Accept(g,new AnswerPromptCommand(0,p.PromptId,c!.Id,g.Revision));}
    private static void Continue(GameEngine g)=>Answer(g,c=>c.Parameters.GetValueOrDefault("option-id")=="continue");
    private static void End(GameEngine g)=>Accept(g,new EndPlayPhaseCommand(0,g.Revision,P(g)!.PromptId));
    private static void Play(GameEngine g)=>Reach(g,()=>P(g)is{PlayerSeat:0,Kind:DecisionKind.PlayCard});
    private static void Store(GameEngine g){Reach(g,()=>Optional(g,Jibing));Answer(g,c=>c.Parameters.GetValueOrDefault("program-action")=="activate");Play(g);}
    private static void Reach(GameEngine g,Func<bool> done,[CallerMemberName]string caller="",[CallerLineNumber]int line=0)
    {
        for(var n=0;n<340;n++)
        {
            if(done())return;Need(g.State.Winner==Winner.None,$"Ended {caller}:{line}: "+Diagnostic(g));var p=P(g);
            if(p is null||p.PlayerSeat!=0)Accept(g,new AdvanceOneStepCommand(g.Revision));
            else if(p.Kind==DecisionKind.PlayCard)End(g);
            else if(p.Kind is DecisionKind.RespondSlash or DecisionKind.RespondDodge)Answer(g,c=>c.Cards.Count==0);
            else if(p.Kind==DecisionKind.Nullification)Answer(g,c=>c.Parameters.GetValueOrDefault("response")=="pass");
            else if(p.Kind==DecisionKind.RescueDying)Answer(g,c=>c.Parameters.GetValueOrDefault("response")is"let-die"or"pass");
            else if(p.SkillPrompt?.SkillId is Health or Watch)Continue(g);
            else if(Action(g,"skip"))Answer(g,c=>c.Parameters.GetValueOrDefault("program-action")=="skip");
            else throw new InvalidOperationException($"Unexpected {caller}:{line}: "+Diagnostic(g));
        }
        throw new InvalidOperationException($"Missing bounded {caller}:{line}: "+Diagnostic(g));
    }
    private static string Diagnostic(GameEngine g)=>JsonSerializer.Serialize(new{Scene=ActiveScene,g.State.TurnNumber,g.State.CurrentSeat,g.State.Phase,Players=Enumerable.Range(0,4).Select(s=>new{Seat=s,V(g,s).Hp,V(g,s).HandCount,V(g,s).IsAlive}),Prompt=P(g)is{}p?new{p.PlayerSeat,p.Kind,Skill=p.SkillPrompt?.SkillId,Choices=p.Choices.Take(6).Select(c=>new{c.Cards,c.Targets,c.Parameters})}:null,Frames=g.ResolutionStack.TakeLast(5).Select(f=>new{f.Id,Type=f.GetType().Name,Stage=(f as ProgramSkillFrame)?.MaYuanYi?.Stage,Program=f is ProgramSkillFrame p?new{p.SkillId,p.TriggerId,p.ActivationId,p.InstructionIndex,PlanCount=PlanCount(g,p),p.SelectedCardIds,p.SelectedTargetSeats,Bindings=p.CardSetBindings.Select(b=>new{b.Name,b.CardIds,b.SourceLocations,b.SelectionActorSeat})}:null}),Facts=g.Events.TakeLast(4).Select(e=>e.Payload.ToString())});
    private static int PlanCount(GameEngine g,ProgramSkillFrame f){try{return ProgramInstructionResolver.Default.Resolve(f,g.ContentRegistry.GetSkill(f.SkillId).Program!).Instructions.Count;}catch{return -1;}}
    private static string State(GameEngine g)=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,4).Select(s=>SnapshotJson.Serialize(g.CreateSnapshot(s))),Frames=g.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())),g.CardMovements,Events=g.Events.Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())),Commands=CommandJson.Serialize(g.AcceptedCommands)});
    private static GameEngine Cold(GameEngine g,ContentRegistry r){var restored=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())),r);Need(State(g)==State(restored),"Cold replay preserves original pile/response providers, every permitted private view and once-paid native child cursors.");return restored;}
    private static(GameEngine,ContentRegistry)Start(string scene)
    {
        ActiveScene=scene;ContentRegistry r;try{r=ContentRegistry.Build(new StandardContentPackage(),new Fixture(scene));}catch(Exception e){throw new InvalidOperationException("scene="+scene+" fixture build cause="+e.Message,e);}var g=GameEngine.CreateStandard(new GameOptions{Seed=7,PlayerCount=4,HumanSeat=0,HumanRole=scene is"awake-death"or"judgment-death"?Role.Rebel:Role.Lord,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=16},r);
        Accept(g,new StartGameCommand());Reach(g,()=>P(g)is{PlayerSeat:0,Kind:DecisionKind.SelectGeneral});Accept(g,new SelectGeneralCommand(0,Owner,g.Revision,P(g)!.PromptId));return(g,r);
    }
    private sealed class Fixture(string scene):IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-ma-yuan-yi",new(1,0,0),[]);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,"ol:ma-yuan-yi");if(scene=="provider")CharacterFixtureContent.Register(b,"classic:liu-bei");if(scene=="multi")CharacterFixtureContent.Register(b,"ol:yang-fu");if(scene=="judgment-claim")CharacterFixtureContent.Register(b,"classic:guo-jia");
            var awakening=scene.StartsWith("awake",StringComparison.Ordinal);var judgment=scene.Contains("judgment",StringComparison.Ordinal);var loss=scene is"awake-loss"or"judgment-loss";var death=scene is"awake-death"or"judgment-death";
            var defs=new List<string>{$$"""
            {"id":"{{Driver}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":30,"priority":0},{"id":"range","query":"attackRange","operation":"set","value":4,"priority":0},{"id":"quota","query":"slashLimit","operation":"add","value":4,"priority":0}],"viewAs":[
             {"id":"duel","inputKinds":[],"inputSuits":[],"outputKind":"duel","forPlay":true,"forResponse":false,"singleCardTrickUse":true,"allowSameKind":true},
             {"id":"borrowed","inputKinds":[],"inputSuits":[],"outputKind":"borrowedSword","forPlay":true,"forResponse":false,"singleCardTrickUse":true,"allowSameKind":true}],"activations":[
             {"id":"wound-peer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":2}]},
             {"id":"incoming","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"offerVirtualSlashOrDraw","target":"selectedTarget"}]},
             {"id":"trim-peer","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"selectedTarget"},"zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]},
             {"id":"chain","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"setChainedState","target":"selectedTarget","chained":true}]},
             {"id":"hand-cost","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"selectOwnedCards","target":"owner","amount":1,"zones":["hand"],"resultBind":"hand-only"},{"op":"moveBoundCards","target":"owner","sourceBind":"hand-only","destination":"discardPile","awaitMovementTriggers":true}]},
              {"id":"gain-binghuo","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["{{(scene=="judgment-loss"?"fixture:my-loss-conditional":Binghuo)}}"]}]},
             {"id":"regrant-awakening","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantSkills","target":"owner","skillIds":["ol:moucuan"]}]}]}
            """,$$"""{"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}"""};
            if(scene=="provider")defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"actual-supplied-pile","window":"cardSupplyCompleted","ownerRelation":"observer","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"chooseOption","target":"owner","resultBind":"supply","options":[{"id":"continue"}]}]}]}""");
            if(scene=="duel")defs.Add("""{"id":"fixture:my-slash-response","revision":1,"viewAs":[{"id":"slash","inputKinds":["crossbow"],"inputSuits":[],"outputKind":"slash","forPlay":false,"forResponse":true}]}""");
            if(scene=="borrowed")defs.Add("""{"id":"fixture:my-peer-weapon","revision":1,"triggers":[{"id":"actual-weapon","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"equipSampledGenerals","target":"owner","amount":1}]}]}""");
            if(scene=="reshuffle")defs.Add("""{"id":"fixture:my-prime","revision":1,"triggers":[{"id":"real-discard-stock","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"selectOwnedCards","target":"owner","numberExpression":"allOwnedZoneCards","zones":["hand"],"resultBind":"prime"},{"op":"moveBoundCards","target":"owner","sourceBind":"prime","destination":"discardPile","awaitMovementTriggers":true}]}]}""");
            if(scene=="skip")defs.Add("""{"id":"fixture:my-skip","revision":1,"triggers":[{"id":"skip","window":"turnStartBeforeNormalFlow","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["draw"]}]}]}""");
            if(loss&&awakening)defs.Add($$"""{"id":"fixture:my-conditional","revision":1,"hpSkillGrants":[{"id":"healthy-source","skillId":"{{(awakening?Moucuan:Binghuo)}}","minimumHp":5}]}""");
            if(scene=="judgment-loss")defs.Add("""{"id":"fixture:my-loss-conditional","revision":1,"hpSkillGrants":[{"id":"healthy-binghuo","skillId":"ol:binghuo","minimumHp":5}]}""");
            var lethal=death?""",{"op":"loseHp","target":"owner","amount":20}""":"";
            if(awakening)defs.Add($$"""{"id":"{{Health}}","revision":1,"triggers":[{"id":"real-maximum-child","window":"afterHealthChanged","subject":"owner","condition":{"kind":"healthChangeKindIs","hpChangeKind":"maximumHp"},"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"maximum","options":[{"id":"continue"}]}{{lethal}}]}]}""");
            if(awakening)defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-grant-child","window":"skillsChanged","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"grant","options":[{"id":"continue"}]}]}]}""");
            if(scene=="judgment")defs.Add("""{"id":"fixture:my-rejudge","revision":1,"triggers":[{"id":"replace","window":"judgmentReplacing","subject":"any","excludedReasons":[],"optional":true,"effects":[{"op":"replaceJudgment","target":"owner","zones":["hand"],"suits":["spade","club","heart","diamond"],"oldCardDestination":"discardPile"}]}]}""");
            if(scene=="judgment-handoff")defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-judgment-handoff","window":"cardsMoved","subject":"owner","sourceZones":["judgment"],"movementOccurrence":"perBatch","movementReasons":["skill-program.judgment.result"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"handoff","options":[{"id":"continue"}]}]}]}""");
            var judgmentTail=death?"""{"op":"loseHp","target":"owner","amount":20}""":"""{"op":"loseHp","target":"owner","amount":1}""";
            if(scene is"judgment-loss"or"judgment-death")defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-judgment-child","window":"judgmentFinalized","subject":"any","judgmentReasons":["binghuo"],"suits":["spade","club","heart","diamond"],"minimumRank":1,"maximumRank":13,"excludedReasons":[],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"judgment-child","options":[{"id":"continue"}]},{{judgmentTail}}]}]}""");
            var labels=defs.Select(x=>JsonNode.Parse(x)!).ToDictionary(d=>d["id"]!.GetValue<string>(),d=>new{name=d["id"]!.GetValue<string>(),description="真实原生兵牌小夹具",optionLabels=new[]{"triggers","activations"}.SelectMany(k=>d[k]?.AsArray()??new JsonArray()).SelectMany(t=>t!["effects"]!.AsArray()).SelectMany(e=>e!["options"]?.AsArray()??new JsonArray()).Select(o=>o!["id"]!.GetValue<string>()).Distinct().ToDictionary(x=>x,x=>x)});
            var catalog=SkillProgramCatalog.Load($$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{{string.Join(',',defs)}}]}""",JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=labels}));
            foreach(var(id,p)in catalog.Programs)b.AddSkill(new(id,id,"原生真实行为"){Program=p,ProgramPresentation=catalog.Presentations[id]});
            b.AddSkill(new("fixture:my-pick","实际本人","公开固定选择权重"){SelectionWeights=Enum.GetValues<Role>().ToDictionary(x=>x,_=>-100000d)});
            var own=new List<string>{Driver,Jibing,Wangjing};if(scene=="provider")own.AddRange(["classic:jijiang",Watch]);if(scene=="reshuffle")own.Add("fixture:my-prime");if(scene=="multi")own.Add("ol:bingzhong");if(awakening){own.Add(loss?"fixture:my-conditional":Moucuan);own.Add(Health);own.Add(Watch);}if(scene=="skip")own.Add("fixture:my-skip");if(scene=="judgment")own.Add("fixture:my-rejudge");if(scene=="judgment-claim")own.Add("classic:tiandu");if(scene is"judgment-loss"or"judgment-death"or"judgment-handoff")own.Add(Watch);
            b.AddGeneral(new(Owner,"实际马元义","ol-ma-yuan-yi","fixture:my-pick",scene=="provider"?"shu":"qun",scene=="provider"?12:scene=="awake-death"?5:4,own,GeneralGender.Male));
            if(scene=="provider")b.AddSkill(new("fixture:my-provider-pick","实际蜀忠臣供牌者","公开固定角色选将权重"){SelectionWeights=Enum.GetValues<Role>().ToDictionary(x=>x,x=>x==Role.Loyalist?100000d:-200000d)});
            var peers=Enumerable.Range(1,3).Select(i=>"fixture:my-peer-"+i).ToArray();for(var i=0;i<3;i++){var skills=new List<string>();if(scene=="duel")skills.Add("fixture:my-slash-response");if(scene=="borrowed")skills.AddRange([Jibing,Wangjing,"fixture:my-peer-weapon"]);if(scene=="provider"&&i==0)skills.AddRange([Quiet,Jibing,Wangjing]);b.AddGeneral(new(peers[i],"实际其他角色","supporter",scene=="provider"&&i==0?"fixture:my-provider-pick":scene=="foreign-judgment"?"standard:none":Quiet,scene=="provider"?(i==0?"shu":"wei"):awakening?(i==0?"wei":"shu"):"qun",12,skills));}
            if(scene=="borrowed"){b.AddSkill(new("fixture:my-unused","备用武器","不参与正常选将"){SelectionWeights=Enum.GetValues<Role>().ToDictionary(x=>x,_=>-200000d)});b.AddGeneral(new("fixture:my-weapon","实际武器素材","supporter","fixture:my-unused","qun",4,[]));}
            var kind=scene=="fan"?CardKind.ZhuqueFan:scene is"slash"or"duel"or"foreign-judgment"?CardKind.Slash:scene=="dodge"?CardKind.Dodge:scene is"fire"or"borrowed"?CardKind.FireSlash:CardKind.Crossbow;
            b.AddCard(new("fixture:my-card","原生"+kind,CardCatalog.Get(kind).CategoryName,"真实原实体",kind));
            b.AddDeck(new("fixture:my-deck","固定同质真实牌堆",4,2,[]){PhysicalCards=Enumerable.Range(0,scene=="empty"?16:scene is"shortage"or"reshuffle"?17:84).Select(_=>new ContentDeckPhysicalCard("fixture:my-card",scene=="red-judgment"?Suit.Heart:Suit.Spade,7)).ToArray()});
            b.AddMode(new(Mode,"马元义真实原生小局面",4,4,scene=="provider"?new Dictionary<string,int>{[nameof(Role.Lord)]=1,[nameof(Role.Loyalist)]=1,[nameof(Role.Rebel)]=2}:new Dictionary<string,int>{[nameof(Role.Lord)]=1,[nameof(Role.Rebel)]=3},"fixture:my-deck",GeneralCandidateCount:scene=="borrowed"?5:4,GeneralPoolIds:scene=="borrowed"?[Owner,..peers,"fixture:my-weapon"]:[Owner,..peers]));
        }
    }
}
