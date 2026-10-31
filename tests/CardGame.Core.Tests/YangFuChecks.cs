using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class YangFuChecks
{
    private const string Bing="ol:bingzhong",Suyi="ol:suyi",Owner="fixture:yfu-owner",Driver="fixture:yfu-driver",Quiet="fixture:yfu-quiet",Watch="fixture:yfu-watch",Health="fixture:yfu-health",Mode="identity:classic-yang-fu-fixture";
    private static PlayerSnapshot V(GameEngine g,int seat)=>g.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? P(GameEngine g)=>Enumerable.Range(0,g.State.Players.Count).Select(s=>g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p=>p is not null);
    private static T[] E<T>(GameEngine g)where T:IGameEvent=>g.Events.Select(e=>e.Payload).OfType<T>().ToArray();
    private static int? Ordinal(GameEngine g,CardActionContext a)=>(int?)global::FixtureRuntimeAccess.Engine.GetMethod("CapturedWholeActualTurnActorUseOrdinal",BindingFlags.Instance|BindingFlags.NonPublic)!.Invoke(g,[a]);
    private static bool Action(GameEngine g,string a)=>P(g)?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")==a)==true;
    private static bool Targets(GameEngine g)=>Action(g,"bingzhong-target");
    private static void AssertBingzhongAcceptedMutation(GameEngine g)
    {
        var use=g.ResolutionStack.OfType<CardUseFrame>().Single(u=>u.BingzhongTail is{AddedTargets.Count:>0});var tail=use.BingzhongTail!;
        var history=g.Events.Select(e=>e.Payload).ToArray();var accepted=history.OfType<CardActionAcceptedEvent>().Where(e=>e.Action.ActionId==tail.ActionId).ToArray();
        Need(accepted is[var original]&&original.Action.TargetSeats.SequenceEqual([1])&&use.Action!.TargetSeats.SequenceEqual([1,3])&&
            tail.OriginalTargets.SequenceEqual([1])&&tail.AddedTargets.SequenceEqual([3]),
            "Bingzhong extends the owning native action from its frozen single-target acceptance and retains the exact typed target tail.");
        var proof=global::FixtureRuntimeAccess.Engine.GetMethod("RebuildAcceptedActualHandGainUse",BindingFlags.Instance|BindingFlags.NonPublic)!;
        bool Rebuild(CardUseFrame candidate,IGameEvent[] facts)
        {
            var at=Array.FindIndex(facts,e=>e is CardActionAcceptedEvent a&&a.Action.ActionId==tail.ActionId);
            return (bool)proof.Invoke(g,[candidate,accepted[0].Action,candidate.Action!,facts,at])!;
        }
        Need(Rebuild(use,history)&&!Rebuild(use with{BingzhongTail=tail with{OriginalTargets=[3],AddedTargets=[1]}},history)&&
            !Rebuild(use,history.Where(e=>e is not YangFuStartedEvent s||s.FrameId!=tail.FrameId).ToArray())&&
            !Rebuild(use,history.Where(e=>e is not BingzhongTargetSelectedEvent s||s.FrameId!=tail.FrameId).ToArray()),
            "Acceptance reconstruction requires the genuine preceding targets, Yang Fu start and every selected extra target; an applied summary alone cannot authorize the mutation.");
    }
    private static void Need(bool yes,string message){if(!yes)throw new InvalidOperationException(message);}
    private static void Accept(GameEngine g,GameCommand c){var result=g.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single());Need(result.Accepted,result.Error?.Message??"Rejected actual Yang Fu command.");}
    private static void Answer(GameEngine g,Func<PromptChoice,bool> pick){var p=P(g);var c=p?.Choices.FirstOrDefault(pick);Need(p?.PlayerSeat==0&&c is not null,"Missing actual human Yang Fu choice: "+Diagnostic(g));Accept(g,new AnswerPromptCommand(0,p!.PromptId,c!.Id,g.Revision));}
    private static void Continue(GameEngine g)=>Answer(g,c=>c.Parameters.GetValueOrDefault("option-id")=="continue");
    private static void FinishTargets(GameEngine g)=>Answer(g,c=>c.Parameters.GetValueOrDefault("branch")=="finish");
    private static void Use(GameEngine g,string id,int[]? targets=null)=>Accept(g,new UseProgramSkillCommand(0,Driver,id,[],targets??[],g.Revision,P(g)!.PromptId));
    private static void SuyiOn(GameEngine g,int target)=>Accept(g,new UseProgramSkillCommand(0,Suyi,"equipment-then-slash",[],[target],g.Revision,P(g)!.PromptId));
    private static void Native(GameEngine g,Func<LegalAction,bool> pick){var a=g.GetHumanLegalActions().FirstOrDefault(pick);Need(a is not null,"Missing actual native Yang Fu action: "+Diagnostic(g));Accept(g,new PlayCardCommand(0,a!.CardId!.Value,a.TargetSeats,g.Revision,P(g)!.PromptId,a.PlayedCardKind,a.TargetCardId){ConversionSource=a.ConversionSource,AdditionalConversionSources=a.AdditionalConversionSources??[]});}
    private static void Slash(GameEngine g,int target)=>Native(g,a=>a.Kind==LegalActionKind.Slash&&a.TargetSeats.SequenceEqual([target]));
    private static void End(GameEngine g)=>Accept(g,new EndPlayPhaseCommand(0,g.Revision,P(g)!.PromptId));
    private static void Play(GameEngine g,[CallerMemberName]string caller="",[CallerLineNumber]int line=0)=>Reach(g,()=>P(g)is{PlayerSeat:0,Kind:DecisionKind.PlayCard},caller,line);
    private static void Reach(GameEngine g,Func<bool> done,[CallerMemberName]string caller="",[CallerLineNumber]int line=0)
    {
        for(var i=0;i<180;i++)
        {
            if(done())return;Need(g.State.Winner==Winner.None,$"Yang Fu ended at {caller}:{line}: "+Diagnostic(g));var p=P(g);
            if(p is null||p.PlayerSeat!=0){Accept(g,new AdvanceOneStepCommand(g.Revision));continue;}
            if(Targets(g))FinishTargets(g);
            else if(p.Kind==DecisionKind.PlayCard)End(g);
            else if(p.Choices.Any(c=>c.Parameters.GetValueOrDefault("option-id")=="continue"))Continue(g);
            else if(p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash)Answer(g,c=>c.Cards.Count==0);
            else if(p.Kind==DecisionKind.Nullification)Answer(g,c=>c.Parameters.GetValueOrDefault("response")=="pass");
            else if(p.Kind==DecisionKind.RescueDying)Answer(g,c=>c.Parameters.GetValueOrDefault("response")is"let-die"or"pass");
            else if(p.Kind==DecisionKind.FireAttackDiscard)Answer(g,c=>c.Cards.Count==0);
            else if(Action(g,"skip"))Answer(g,c=>c.Parameters.GetValueOrDefault("program-action")=="skip");
            else if(p.Kind==DecisionKind.SelectTargetCard)Answer(g,c=>true);
            else throw new InvalidOperationException($"Unexpected Yang Fu {caller}:{line}: "+Diagnostic(g));
        }
        throw new InvalidOperationException($"Missing bounded Yang Fu {caller}:{line}: "+Diagnostic(g));
    }
    private static string Diagnostic(GameEngine g)=>JsonSerializer.Serialize(new{g.State.TurnNumber,g.State.CurrentSeat,g.State.Phase,Players=Enumerable.Range(0,g.State.Players.Count).Select(s=>new{Seat=s,V(g,s).Hp,V(g,s).HandCount,V(g,s).IsAlive}),Prompt=P(g)is{}p?new{p.PlayerSeat,p.Kind,Skill=p.SkillPrompt?.SkillId,Choices=p.Choices.Take(6).Select(c=>new{c.Cards,c.Targets,c.Parameters})}:null,Frames=g.ResolutionStack.TakeLast(5).Select(f=>new{f.Id,Type=f.GetType().Name,Stage=(f as ProgramSkillFrame)?.YangFu?.Stage}),Tail=g.Events.TakeLast(5).Select(e=>e.Payload.ToString())});
    private static string State(GameEngine g)=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,g.State.Players.Count).Select(s=>SnapshotJson.Serialize(g.CreateSnapshot(s))),Frames=g.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())),g.CardMovements,Events=g.Events.Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())),Commands=CommandJson.Serialize(g.AcceptedCommands)});
    private static GameEngine Cold(GameEngine g,ContentRegistry r){var copy=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())),r);Need(State(g)==State(copy),"Cold restore preserves every permitted view, declaration prefix, frozen X, native material and typed child return.");return copy;}
    private static(GameEngine,ContentRegistry)Start(string scene)
    {
        var terminal=scene=="suyi-terminal";
        var r=ContentRegistry.Build(new StandardContentPackage(),new Fixture(scene));var g=GameEngine.CreateStandard(new GameOptions{Seed=7,PlayerCount=terminal?2:4,HumanSeat=0,HumanRole=terminal?null:scene is"draw-death"or"suyi-death"?Role.Rebel:Role.Lord,HumanTeamId=terminal?"player":null,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=8},r);
        Accept(g,new StartGameCommand());Reach(g,()=>P(g)is{Kind:DecisionKind.SelectGeneral});Accept(g,new SelectGeneralCommand(0,Owner,g.Revision,P(g)!.PromptId));Play(g);return(g,r);
    }
    private sealed class Fixture(string scene):IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-yang-fu",new(1,0,0),[]);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,"ol:yang-fu");if(scene=="tail")CharacterFixtureContent.Register(b,"ol:zhang-yi");
            // The normal classic catalog tracks whole-Use damage even when its
            // observing general is not at this table. Exercise that real ledger.
            CharacterFixtureContent.Register(b,"ol:wang-guan");
            var kind=scene=="draw-two"?CardKind.DrawTwo:scene=="peach"?CardKind.Peach:scene=="alcohol"?CardKind.Alcohol:scene=="surprise"?CardKind.UnexpectedAssault:scene=="issued-distance"?CardKind.Snatch:
                scene=="suyi-lion"?CardKind.SilverLion:scene=="suyi-ox"?CardKind.WoodenOx:scene is"suyi-no-stock"or"suyi-range"?CardKind.Dodge:scene=="suyi-quota"?CardKind.Slash:CardKind.Crossbow;
            var text=char.ToLowerInvariant(kind.ToString()[0])+kind.ToString()[1..];
            static string A(string id,string effects,int targets=0)=>$$"""{"id":"{{id}}","minCards":0,"maxCards":0,"minTargets":{{targets}},"maxTargets":{{targets}},"targetKind":"otherLiving","usesPerTurn":null,"effects":[{{effects}}]}""";
            var acts=new[]{A("empty","""{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]}"""),A("hurt","""{"op":"loseHp","target":"owner","amount":2}"""),
                A("hurt-other","""{"op":"loseHp","target":"selectedTarget","amount":2}""",1),A("damage-self","""{"op":"damage","target":"owner","amount":2}"""),
                A("incoming","""{"op":"offerVirtualSlashOrDraw","target":"selectedTarget"}""",1),A("virtual","""{"op":"useVirtualSlash","target":"selectedTarget"}""",1),
                A("equip-peer","""{"op":"useRandomDeckEquipment","target":"owner","resultBind":"gear"}""",1)};
            var conversions=new List<object>();if(kind!=CardKind.Slash)conversions.Add(new{id="slash",inputKinds=new[]{text},inputSuits=Array.Empty<string>(),outputKind="slash",forPlay=true,forResponse=false});
            if(kind!=CardKind.Dodge)conversions.Add(new{id="dodge",inputKinds=new[]{text},inputSuits=Array.Empty<string>(),outputKind="dodge",forPlay=false,forResponse=true});
            var view=conversions.Count==0?"":"\"viewAs\":"+JsonSerializer.Serialize(conversions)+",";
            var extraQuota=scene=="actor"?""",{"id":"two-real-debits","query":"slashLimit","operation":"add","value":1,"priority":0}""":"";
            var defs=new List<string>{$$"""{"id":"{{Driver}}","revision":1,{{view}}"modifiers":[{"id":"keep","query":"handLimit","operation":"add","value":30,"priority":0}{{extraQuota}}],"activations":[{{string.Join(',',acts)}}]}""",
                $$"""{"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]}"""};
            if(scene=="actor")defs.Add("""{"id":"fixture:yfu-actor","revision":1,"triggers":[{"id":"real-actor","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["slash"],"priority":1000,"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLegalCurrentCardTarget"},{"op":"replaceCurrentCardUseActor","target":"selectedTarget"}]}]}""");
            if(scene=="issued-distance")defs.Add("""{"id":"fixture:yfu-first-distance","revision":1,"cardPolicies":[{"id":"true-first-use-distance","kind":"firstActualPlayUseDistanceUnlimited"}]}""");
            if(scene=="suyi-fire")defs.Add("""{"id":"fixture:yfu-fire","revision":1,"triggers":[{"id":"real-fire","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["slash"],"optional":false,"effects":[{"op":"offerCurrentSlashFireAndExtraTarget","target":"owner"}]}]}""");
            if(scene=="suyi-abolished")defs.Add("""{"id":"fixture:yfu-abolished","revision":1,"triggers":[{"id":"actual-area","window":"gameStarting","subject":"owner","optional":false,"effects":[{"op":"abolishOwnerAreas","target":"owner","zones":["equipment"]}]}]}""");
            if(scene is"draw-loss"or"suyi-loss")defs.Add($$"""{"id":"fixture:yfu-conditional","revision":1,"hpSkillGrants":[{"id":"actual-source","skillId":"{{(scene=="draw-loss"?Bing:Suyi)}}","minimumHp":{{(scene=="draw-loss"?11:13)}}}]}""");
            if(scene.StartsWith("draw",StringComparison.Ordinal))
            {
                var amount=scene=="draw-loss"?1:scene=="draw-death"?20:0;
                defs.Add($$"""{"id":"fixture:yfu-gain","revision":1,"triggers":[{"id":"actual-draw-child","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.yang-fu.bingzhong.draw"],"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}{{(amount==0?"":",{\"op\":\"loseHp\",\"target\":\"owner\",\"amount\":"+amount+"}")}}]}]}""");
            }
            if(scene is"draw-loss"or"suyi-loss")defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"actual-health-child","window":"afterHealthChanged","subject":"owner","condition":{"kind":"healthChangeKindIs","hpChangeKind":"loss"},"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"health","options":[{"id":"continue"}]}]}]}""");
            if(scene is"suyi-empty-hand"or"suyi-fire")defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-adjusted-use-child","window":"cardUseBeforeTargetEffects","ownerRelation":"actor","cardKinds":["slash","fireSlash"],"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"proof","options":[{"id":"continue"}]}]}]}""");
            if(scene=="suyi-lion")defs.Add($$"""{"id":"{{Health}}","revision":1,"triggers":[{"id":"native-recovery","window":"afterHealthChanged","subject":"owner","condition":{"kind":"healthChangeKindIs","hpChangeKind":"recovery"},"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"lion","options":[{"id":"continue"}]}]}]}""");
            if(scene=="suyi-terminal")defs.Add($$"""{"id":"{{Watch}}","revision":1,"triggers":[{"id":"real-lethal-use-child","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["slash"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"proof","options":[{"id":"continue"}]}]}]}""");
            if(scene is"suyi-loss"or"suyi-death")defs.Add($$"""{"id":"fixture:yfu-equipment-child","revision":1,"triggers":[{"id":"actual-equipment-finalization","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardCategories":["equipment"],"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},{"op":"loseHp","target":"selectedTarget","amount":{{(scene=="suyi-loss"?1:20)}}}]}]}""");
            if(scene=="suyi-target-death")defs.Add("""{"id":"fixture:yfu-equipment-child","revision":1,"triggers":[{"id":"actual-equipment-owner-death","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardCategories":["equipment"],"usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"loseHp","target":"owner","amount":20}]}]}""");
            var labels=defs.Select(d=>JsonNode.Parse(d)!).ToDictionary(d=>d["id"]!.GetValue<string>(),d=>new{name=d["id"]!.GetValue<string>(),description="杨阜原生真实命令小夹具",optionLabels=new[]{"activations","triggers"}.SelectMany(k=>d[k]?.AsArray()??new JsonArray()).SelectMany(t=>t!["effects"]!.AsArray()).SelectMany(e=>e!["options"]?.AsArray()??new JsonArray()).Select(o=>o!["id"]!.GetValue<string>()).Distinct().ToDictionary(id=>id,id=>id)});
            var catalog=SkillProgramCatalog.Load($$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{{string.Join(',',defs)}}]}""",JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=labels}));
            foreach(var(id,p)in catalog.Programs)b.AddSkill(new(id,id,"杨阜原生真实用牌与付款子链"){Program=p,ProgramPresentation=catalog.Presentations[id]});
            b.AddSkill(new("fixture:yfu-pick","真实杨阜","固定公开选将权重"){SelectionWeights=Enum.GetValues<Role>().ToDictionary(role=>role,_=>-100000d)});
            var own=new List<string>{Driver};own.Add(scene=="draw-loss"?"fixture:yfu-conditional":Bing);own.Add(scene=="suyi-loss"?"fixture:yfu-conditional":Suyi);
            if(scene.StartsWith("draw",StringComparison.Ordinal))own.Add("fixture:yfu-gain");if(scene is"draw-loss"or"suyi-loss"or"suyi-empty-hand"or"suyi-fire"or"suyi-terminal")own.Add(Watch);if(scene=="actor")own.Add("fixture:yfu-actor");if(scene=="tail")own.Add("ol:dianjun");if(scene=="issued-distance")own.Add("fixture:yfu-first-distance");if(scene=="suyi-fire")own.Add("fixture:yfu-fire");
            b.AddGeneral(new(Owner,"实际杨阜","ol-yang-fu","fixture:yfu-pick","wei",12,own,GeneralGender.Male));
            var peers=Enumerable.Range(1,3).Select(i=>$"fixture:yfu-peer-{i}").ToArray();foreach(var peer in peers)
            {
                var extra=new List<string>();if(scene=="suyi-lion")extra.Add(Health);if(scene=="suyi-abolished")extra.Add("fixture:yfu-abolished");if(scene is"suyi-loss"or"suyi-death"or"suyi-target-death")extra.Add("fixture:yfu-equipment-child");
                b.AddGeneral(new(peer,"实际其他角色","supporter",Quiet,"qun",12,extra){InitialHp=scene=="suyi-terminal"?1:null});
            }
            b.AddCard(new("fixture:yfu-card","真实"+kind,CardCatalog.Get(kind).CategoryName,"固定原生实体",kind));
            b.AddDeck(new("fixture:yfu-deck","杨阜同质真实小牌堆",4,2,[]){PhysicalCards=Enumerable.Range(0,100).Select(_=>new ContentDeckPhysicalCard("fixture:yfu-card",Suit.Spade,7)).ToArray()});
            if(scene=="suyi-terminal")
            {
                b.AddModeFeatureFactory(new SuyiTerminalSetupFactory());
                b.AddMode(new(Mode,"杨阜真实终局小局面",2,2,new Dictionary<string,int>(),"fixture:yfu-deck",2,[Owner,peers[0]],ContentModeKind.Team,new Dictionary<string,int>{["player"]=1,["other"]=1}));
            }
            else b.AddMode(new(Mode,"杨阜真实实际回合小局面",4,4,new Dictionary<string,int>{[nameof(Role.Lord)]=1,[nameof(Role.Rebel)]=3},"fixture:yfu-deck",GeneralCandidateCount:4,GeneralPoolIds:[Owner,..peers]));
        }
    }
}
