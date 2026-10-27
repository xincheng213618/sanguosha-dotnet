using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class LiYiChecks
{
    private const string Owner="fixture:ly-owner",Mode="identity:classic-li-yi",Driver="fixture:ly-driver",Quiet="fixture:ly-quiet",Watch="fixture:ly-watch",Response="fixture:ly-response",Zero="fixture:ly-zero",Conditional="fixture:ly-conditional";
    private const string Chanshuang="ol:chanshuang",Zhanjin="ol:zhanjin";
    private static string Scene="";

    public static void ChanshuangPrivateSameChoicesRecastLionOxCold()
    {
        var(g,r)=Start("same");UseSkill(g,Chanshuang,"simultaneous-options",targets:[1]);
        Need(Stage(g)==ChanshuangStage.Votes&&P(g)is{PlayerSeat:0,IsPrivate:true},"The first actual participant receives a private independent choice.");Answer(g,c=>c.Parameters.GetValueOrDefault("option")=="recast");
        var f=Current(g)!;Need(f.Chanshuang is{Votes.Count:1,Stage:ChanshuangStage.Votes}&&E<ChanshuangVoteRevealedEvent>(g).Length==0&&E<ChanshuangCostIssuedEvent>(g).Length==0&&g.CreateSnapshot(0).PendingDecision is null&&g.CreateSnapshot(1).PendingDecision is{IsPrivate:true},"The first sealed vote is not revealed to the other chooser, and no cost is executed before both independently submit.");
        g=Cold(g,r);Reach(g,()=>Stage(g)==ChanshuangStage.RecastCard&&P(g)?.PlayerSeat==0);var id=Current(g)!.Id;
        Need(E<ChanshuangVoteRevealedEvent>(g).Where(e=>e.FrameId==id).Select(e=>e.Option).SequenceEqual([ChanshuangOption.Recast,ChanshuangOption.Recast]),"Identical choices are revealed twice and remain two independent participant executions.");
        var primary=P(g)!.Choices.First(c=>c.Cards.Count==1).Cards.Single();Answer(g,c=>c.Cards.SequenceEqual([primary]));Finish(g);Play(g);
        var facts=E<NativeRecastInvocationRecordedEvent>(g).Where(e=>e.ParentFrameId==id).ToArray();
        Need(facts.Length==2&&facts.Select(e=>e.ExecutorSeat).SequenceEqual([0,1])&&facts.All(e=>e.PrimaryCount==1&&e.ActualRecastCount==1&&e.ActualDrawCount==1)&&E<ChanshuangCompletedEvent>(g).Single(e=>e.FrameId==id).Count==2,"Both identical votes perform their own actual one-card HE recast and one draw, exactly once.");_=Cold(g,r);

        foreach(var scene in new[]{"lion-loss","lion-death","ox"})
        {
            (g,r)=Start(scene);Native(g,a=>a.Kind==LegalActionKind.Equip);Play(g);var eq=V(g,0).Equipment.Single();int[] grain=[];
            if(scene=="ox")
            {
                grain=[V(g,0).Hand[0].Id];Accept(g,new UseEquipmentEffectCommand(0,CardKind.WoodenOx,grain,[],g.Revision,P(g)!.PromptId));Play(g);
                Need(V(g,0).WoodenOxGrain is{Count:1}&&Enumerable.Range(1,3).All(s=>g.CreateSnapshot(s).Players[0].WoodenOxGrain is null or{Count:0}),"Only the actual Ox owner sees its stored entity; other prepared views reveal none of its identity.");
            }
            else {Use(g,"wound");Play(g);}
            Begin(g,1,"recast");Reach(g,()=>Stage(g)==ChanshuangStage.RecastCard&&P(g)?.PlayerSeat==0);Answer(g,c=>c.Cards.SequenceEqual([eq.Id]));
            Reach(g,()=>P(g)is{PlayerSeat:0}p&&p.SkillPrompt?.SkillId==Watch&&p.Choices.Any(c=>c.Parameters.GetValueOrDefault("option-id")=="continue"));
            f=Current(g)!;id=f.Id;
            Need(f.Chanshuang is{Stage:ChanshuangStage.CostChildren,Cursor:0,Materials.Count:1}&&E<NativeRecastInvocationRecordedEvent>(g).All(e=>e.ParentFrameId!=id)&&E<ChanshuangDrawIssuedEvent>(g).All(e=>e.FrameId!=id),"The actual paid equipment owns its native child pause before reward or peer execution.");
            if(scene=="ox")Need(grain.All(c=>g.CardMovements.Any(m=>m.CardId==c&&m.From==CardLocation.WoodenOxGrain(0)&&m.To==CardLocation.DiscardPile))&&f.Chanshuang!.Grain.SequenceEqual(grain),"Ox spill remains an independently proven native child, outside the original recast primary group.");
            else Need(E<RecoveryAppliedEvent>(g).Any(e=>e.TargetSeat==0&&e.Amount==1),"Removing the genuinely equipped Silver Lion preserves its real recovery before continuing the recast.");
            g=Cold(g,r);Continue(g);Reach(g,()=>E<ChanshuangCompletedEvent>(g).Any(e=>e.FrameId==id));
            facts=E<NativeRecastInvocationRecordedEvent>(g).Where(e=>e.ParentFrameId==id).ToArray();
            Need(facts.Length==2&&facts.All(e=>e.PrimaryCount==1&&e.ActualRecastCount==1)&&facts[0].ActualDrawCount==(scene=="lion-death"?0:1)&&facts[1].ActualDrawCount==1&&E<ChanshuangMaterialPaidEvent>(g).Count(e=>e.FrameId==id&&e.CardId==eq.Id)==1,"After all paid children, the original equipment and living peer execute once; neither grain nor a replay is another primary recast.");
            if(scene=="lion-loss")Need(V(g,0).IsAlive&&!V(g,0).Skills!.Any(s=>s.Id==Chanshuang),"Loss of the real HP-conditional source does not cancel the already-issued second participant.");
            if(scene=="lion-death")Need(!V(g,0).IsAlive&&E<PlayerDiedEvent>(g).Any(e=>e.VictimSeat==0),"Actual non-Lord issuer death completes the paid owner's zero-draw recast and the living peer's independent recast.");_=Cold(g,r);
        }
    }

    private static PlayerSnapshot V(GameEngine g,int seat)=>g.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? P(GameEngine g)=>Enumerable.Range(0,4).Select(s=>g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p=>p is not null);
    private static T[] E<T>(GameEngine g)where T:IGameEvent=>g.Events.Select(e=>e.Payload).OfType<T>().ToArray();
    private static ProgramSkillFrame? Current(GameEngine g)=>g.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f=>f.Chanshuang is not null);
    private static ChanshuangStage? Stage(GameEngine g)=>Current(g)?.Chanshuang?.Stage;
    private static void Need(bool ok,string why){if(!ok)throw new InvalidOperationException("scene="+Scene+" "+why);}
    private static void Accept(GameEngine g,GameCommand c){try{var result=g.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single());Need(result.Accepted,result.Error?.Message??"Rejected native Li Yi command.");}catch(Exception e){throw new InvalidOperationException("scene="+Scene+" "+c.GetType().Name+" cause="+e.Message+" "+Diagnostic(g),e);}}
    private static void Answer(GameEngine g,Func<PromptChoice,bool> choose){var p=P(g)!;var c=p.Choices.FirstOrDefault(choose);Need(p.PlayerSeat==0&&c is not null,"Missing actual human choice: "+Diagnostic(g));Accept(g,new AnswerPromptCommand(0,p.PromptId,c!.Id,g.Revision));}
    private static void Continue(GameEngine g)=>Answer(g,c=>c.Parameters.GetValueOrDefault("option-id")=="continue");
    private static void UseSkill(GameEngine g,string skill,string activation,int[]? cards=null,int[]? targets=null)=>Accept(g,new UseProgramSkillCommand(0,skill,activation,cards??[],targets??[],g.Revision,P(g)!.PromptId));
    private static void Use(GameEngine g,string activation,int[]? cards=null,int[]? targets=null)=>UseSkill(g,Driver,activation,cards,targets);
    private static void Native(GameEngine g,Func<LegalAction,bool> match){var a=g.GetHumanLegalActions().FirstOrDefault(match);Need(a is not null,"Missing real native action: "+Diagnostic(g));Accept(g,new PlayCardCommand(0,a!.CardId!.Value,a.TargetSeats,g.Revision,P(g)!.PromptId,a.PlayedCardKind,a.TargetCardId){ConversionSource=a.ConversionSource,AdditionalConversionSources=a.AdditionalConversionSources??[]});}
    private static void Begin(GameEngine g,int peer,string option){UseSkill(g,Chanshuang,"simultaneous-options",targets:[peer]);Answer(g,c=>c.Parameters.GetValueOrDefault("option")==option);}
    private static void Discard(GameEngine g,int count){Use(g,"discard-"+count);Play(g);}
    private static void End(GameEngine g)=>Accept(g,new EndPlayPhaseCommand(0,g.Revision,P(g)!.PromptId));
    private static void Play(GameEngine g)=>Reach(g,()=>P(g)is{PlayerSeat:0,Kind:DecisionKind.PlayCard});
    private static void Finish(GameEngine g)=>Reach(g,()=>Current(g)is null);
    private static void Reach(GameEngine g,Func<bool> done,[CallerMemberName]string caller="",[CallerLineNumber]int line=0)
    {
        for(var step=0;step<240;step++)
        {
            if(done())return;var p=P(g);Need(g.State.Winner==Winner.None,$"Unexpected terminal before {caller}:{line}: "+Diagnostic(g));
            if(p is null||p.PlayerSeat!=0){Accept(g,new AdvanceOneStepCommand(g.Revision));continue;}
            if(p.Kind==DecisionKind.PlayCard)End(g);
            else if(p.SkillPrompt?.SkillId==Watch)Continue(g);
            else if(Current(g)is{}f&&p.SkillPrompt?.SkillId==Chanshuang)
            {
                if(f.Chanshuang!.Stage is ChanshuangStage.RecastCard or ChanshuangStage.DiscardCards)Answer(g,c=>c.Cards.Count==1);
                else if(f.Chanshuang.Stage==ChanshuangStage.SlashTarget)Answer(g,c=>c.Targets.SequenceEqual([1])||c.Targets.Count==1);
                else if(f.Chanshuang.Stage==ChanshuangStage.SlashChoice)Answer(g,c=>c.Parameters.GetValueOrDefault("option")=="use"&&c.Cards.Count==1||c.Parameters.GetValueOrDefault("program-action")=="round-once-cardless-request");
                else throw new InvalidOperationException("Unexpected Chanshuang stage: "+Diagnostic(g));
            }
            else if(p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification)Answer(g,c=>c.Cards.Count==0);
            else if(p.Kind==DecisionKind.RescueDying)Answer(g,c=>c.Parameters.GetValueOrDefault("response")is"let-die"or"pass");
            else if(p.Kind==DecisionKind.StoneAxe)Answer(g,c=>c.Parameters.GetValueOrDefault("action")=="stone-axe-skip");
            else if(p.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="skip"))Answer(g,c=>c.Parameters.GetValueOrDefault("program-action")=="skip");
            else if(p.Choices.Any(c=>c.Cards.Count==1))Answer(g,c=>c.Cards.Count==1);
            else throw new InvalidOperationException($"Unexpected {caller}:{line}: "+Diagnostic(g));
        }
        throw new InvalidOperationException($"Missing boundary {caller}:{line}: "+Diagnostic(g));
    }
    private static string Diagnostic(GameEngine g)=>JsonSerializer.Serialize(new{Scene,g.State.TurnNumber,g.State.CurrentSeat,g.State.Phase,Players=Enumerable.Range(0,4).Select(s=>new{Seat=s,V(g,s).Hp,V(g,s).IsAlive,V(g,s).HandCount}),Prompt=P(g)is{}p?new{p.PlayerSeat,p.Kind,Skill=p.SkillPrompt?.SkillId,Choices=p.Choices.Take(5).Select(c=>new{c.Cards,c.Targets,c.Parameters})}:null,Frames=g.ResolutionStack.TakeLast(6).Select(f=>new{f.Id,Type=f.GetType().Name,Receipt=(f as ProgramSkillFrame)?.Chanshuang}),Facts=g.Events.TakeLast(4).Select(e=>e.Payload.ToString())});
    private static string State(GameEngine g)=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,4).Select(s=>SnapshotJson.Serialize(g.CreateSnapshot(s))),Frames=g.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())),g.CardMovements,Commands=CommandJson.Serialize(g.AcceptedCommands),Events=g.Events.Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType()))});
    private static GameEngine Cold(GameEngine g,ContentRegistry r){var copy=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())),r);Need(State(g)==State(copy),"Cold replay preserves all private choices, immutable primary groups, native supplier source and once-paid progress.");return copy;}
    private static(GameEngine,ContentRegistry)Start(string scene)
    {
        Scene=scene;ContentRegistry r;try{r=ContentRegistry.Build(new StandardContentPackage(),new Fixture(scene));}catch(Exception e){throw new InvalidOperationException("scene="+scene+" fixture cause="+e.Message,e);}var g=GameEngine.CreateStandard(new GameOptions{Seed=7,PlayerCount=4,HumanSeat=0,HumanRole=scene=="lion-death"?Role.Rebel:Role.Lord,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=10},r);Accept(g,new StartGameCommand());Reach(g,()=>P(g)is{PlayerSeat:0,Kind:DecisionKind.SelectGeneral});Accept(g,new SelectGeneralCommand(0,Owner,g.Revision,P(g)!.PromptId));Play(g);return(g,r);
    }
    private sealed class Fixture(string scene):IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-li-yi",new(1,0,0),[]);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,"ol:li-yi");if(scene=="faction")CharacterFixtureContent.Register(b,"classic:liu-bei");
            static JsonNode N(string value)=>JsonNode.Parse(value)!;
            static JsonNode A(string id,string effects,int targets=0)=>N($$$"""{"id":"{{{id}}}","minCards":0,"maxCards":0,"minTargets":{{{targets}}},"maxTargets":{{{targets}}},"targetKind":"otherLiving","usesPerTurn":null,"effects":[{{{effects}}}]}""");
            var acts=new JsonArray(A("wound","""{"op":"loseHp","target":"owner","amount":1}"""),A("near-death","""{"op":"loseHp","target":"selectedTarget","amount":11}""",1),A("grant","""{"op":"grantSkills","target":"owner","skillIds":["ol:chanshuang"]}"""));
            for(var count=1;count<=3;count++)acts.Add(A("discard-"+count,$$$"""{"op":"selectOwnedCards","target":"owner","amount":{{{count}}},"zones":["hand"],"resultBind":"paid"},{"op":"moveBoundCards","target":"owner","sourceBind":"paid","destination":"discardPile","awaitMovementTriggers":true}"""));
            acts.Add(N("""{"id":"recast-equipment","minCards":1,"maxCards":1,"sourceZones":["hand","equipment"],"cardCategories":["equipment"],"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"recastSelectedEquipment","target":"owner"}]}"""));
            var driver=N($$$"""{"id":"{{{Driver}}}","revision":1,"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":30,"priority":0}],"viewAs":[{"id":"slash","inputKinds":[],"inputSuits":[],"sourceZones":["hand"],"outputKind":"slash","forPlay":true,"forResponse":false,"allowSameKind":true}]}""");driver["activations"]=acts;
            if(scene=="weapon-conversion")driver["viewAs"]!.AsArray().Add(N("""{"id":"equipment-slash","inputKinds":["crossbow"],"inputSuits":[],"sourceZones":["equipment"],"outputKind":"slash","forPlay":true,"forResponse":false}"""));
            var defs=new JsonArray(driver,N($$$"""{"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}"""));
            if(scene=="axe")defs.Add(N($$$"""{"id":"{{{Response}}}","revision":1,"viewAs":[{"id":"dodge","inputKinds":[],"inputSuits":[],"outputKind":"dodge","forPlay":false,"forResponse":true,"allowSameKind":true}]}"""));
            if(scene=="zero")defs.Add(N($$$"""{"id":"{{{Zero}}}","revision":1,"viewAs":[{"id":"round-slash","inputKinds":[],"inputSuits":[],"inputCount":0,"sourceZones":[],"outputKind":"slash","forPlay":true,"forResponse":false,"roundOnceCardlessBasic":{"usageGroup":"ly-round","usesPerRound":1}}]}"""));
            if(scene=="lion-loss")defs.Add(N($$$"""{"id":"{{{Conditional}}}","revision":1,"hpSkillGrants":[{"id":"healthy-chanshuang","skillId":"ol:chanshuang","minimumHp":12}]}"""));
            if(scene is"lion-loss"or"lion-death"or"ox")
            {
                var trigger=N(scene=="ox"?"""{"id":"native-cost","window":"cardsMoved","subject":"owner","sourceZones":["woodenOxGrain"],"movementOccurrence":"perBatch","movementReasons":["equipment.wooden-ox.grain-discard"],"optional":false,"effects":[]}""":"""{"id":"native-cost","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["card.recast.discard"],"optional":false,"effects":[]}""");
                trigger["effects"]!.AsArray().Add(N("""{"op":"chooseOption","target":"owner","resultBind":"continue","options":[{"id":"continue"}]}"""));if(scene!="ox")trigger["effects"]!.AsArray().Add(N($$$"""{"op":"loseHp","target":"owner","amount":{{{(scene=="lion-death"?20:2)}}}}"""));defs.Add(new JsonObject{["id"]=Watch,["revision"]=1,["triggers"]=new JsonArray(trigger)});
            }
            if(scene=="faction")defs.Add(N($$$"""{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"supply","window":"cardSupplyCompleted","ownerRelation":"actor","cardKinds":["slash","fireSlash","thunderSlash"],"optional":false,"effects":[{"op":"draw","target":"owner","amount":1}]}]}"""));
            if(scene=="history-group-recast")defs.Add(N("""{"id":"fixture:ly-match","revision":1,"activations":[{"id":"match","minCards":2,"maxCards":null,"sourceZones":["hand","equipment"],"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"targetPhaseLedgerId":"match","effects":[{"op":"recastMatchingCardsThenPeerChoice","target":"selectedTarget","stateId":"match"}]}]}"""));
            if(scene=="history-played")defs.Add(N("""{"id":"fixture:ly-duel","revision":1,"activations":[{"id":"duel","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"usesPerPhase":2,"effects":[{"op":"useSelectedActorDuel","target":"owner"}]}]}"""));
            if(scene=="weapon-conversion")defs.Add(N("""{"id":"fixture:ly-weapon-conversion","revision":1,"triggers":[{"id":"real-equipment-slash","window":"turnEnding","subject":"owner","turnOwnerScope":"otherLiving","optional":true,"effects":[{"op":"selectOwnedCards","target":"owner","minimumCards":1,"maximumCards":1,"zones":["equipment"],"resultBind":"weapon-material"},{"op":"useBoundCardsAsSlash","target":"owner","sourceBind":"weapon-material"}]}]}"""));
            var labels=defs.ToDictionary(n=>n!["id"]!.GetValue<string>(),n=>new{name=n!["id"]!.GetValue<string>(),description="李异真实原生行为夹具",optionLabels=n!["id"]!.GetValue<string>()==Watch&&scene!="faction"?new Dictionary<string,string>{{"continue","继续"}}:new Dictionary<string,string>()});var catalog=SkillProgramCatalog.Load(new JsonObject{["schemaVersion"]=SkillProgramCatalog.RulesSchemaVersion,["skills"]=defs}.ToJsonString(),JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=labels}));foreach(var(id,p)in catalog.Programs)b.AddSkill(new(id,id,"真实原生缠双夹具"){Program=p,ProgramPresentation=catalog.Presentations[id]});
            b.AddSkill(new("fixture:ly-pick","固定本人","公开选将评分"){SelectionWeights=Enum.GetValues<Role>().ToDictionary(role=>role,_=>-100000d)});
            var own=new List<string>{Driver,Zhanjin};if(scene!="history-before")own.Add(scene=="lion-loss"?Conditional:Chanshuang);if(scene=="zero")own.Add(Zero);if(scene is"lion-loss"or"lion-death"or"ox"or"faction")own.Add(Watch);if(scene=="faction")own.Add("classic:jijiang");if(scene=="history-group-recast")own.Add("fixture:ly-match");if(scene=="history-played")own.Add("fixture:ly-duel");
            if(scene=="weapon-conversion")own.Add("fixture:ly-weapon-conversion");
            b.AddGeneral(new(Owner,"实际李异","ol-li-yi","fixture:ly-pick",scene=="faction"?"shu":"wu",12,own,GeneralGender.Male));
            var peers=Enumerable.Range(1,3).Select(i=>"fixture:ly-peer-"+i).ToArray();foreach(var peer in peers)b.AddGeneral(new(peer,"真实其他角色","supporter",Quiet,scene=="faction"?"shu":"wei",12,scene=="axe"?[Response]:[]));
            var kind=scene switch{"weapon"or"weapon-conversion"=>CardKind.Crossbow,"lion-loss"or"lion-death"=>CardKind.SilverLion,"ox"=>CardKind.WoodenOx,"converted"or"zero"or"axe"=>CardKind.Dodge,"history-two"or"history-three"or"history-group-recast"=>CardKind.IronChain,_=>CardKind.Slash};b.AddCard(new("fixture:ly-card","真实"+kind,CardCatalog.Get(kind).CategoryName,"原生实体",kind));b.AddDeck(new("fixture:ly-deck","真实固定同质牌堆",6,2,[]){PhysicalCards=Enumerable.Range(0,128).Select(i=>new ContentDeckPhysicalCard("fixture:ly-card",Suit.Club,i%13+1)).ToArray()});
            b.AddMode(new(Mode,"李异真实行为",4,4,scene=="faction"?new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Rebel),1},{nameof(Role.Loyalist),2}}:new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Rebel),3}},"fixture:ly-deck",GeneralCandidateCount:4,GeneralPoolIds:[Owner,..peers]));
        }
    }
}
