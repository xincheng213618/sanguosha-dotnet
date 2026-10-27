using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MaChengChecks
{
    private const string Skill="ol:chenglie",Owner="fixture:mc-owner",Driver="fixture:mc-driver",Quiet="fixture:mc-quiet",Issuer="fixture:mc-issuer",Watch="fixture:mc-watch",Mode="identity:classic-ma-cheng";
    private static string Scene="";
    public static void ChenglieTargetsNativeMultiAndZeroUsesCold()
    {
        foreach(var scene in new[]{"add-one","add-two","prepared","fire","zero","decline","no-extra"})
        {
            var(g,r)=Start(scene);Use(g,"stock");Play(g);
            if(scene is"prepared"or"no-extra"){Native(g,a=>a.Kind==LegalActionKind.Equip);Play(g);Use(g,"give",V(g,0).Hand.Take(V(g,0).HandCount-1).Select(c=>c.Id).ToArray(),[1]);Play(g);Need(V(g,0).HandCount==1&&V(g,0).Equipment.Any(c=>c.Kind==CardKind.FangtianHalberd),"A real equipped Fangtian and last Hand card create the prepared multi-target native Slash.");}
            var hp=Enumerable.Range(0,4).Select(s=>V(g,s).Hp).ToArray();
            if(scene=="zero"){Use(g,"zero");Reach(g,()=>Action(g,"select-target"));Answer(g,c=>c.Targets.SequenceEqual([1]));}
            else Native(g,a=>Kind(g,a)==CardKind.Slash&&a.TargetSeats.SequenceEqual(scene=="prepared"?[1,2]:scene=="no-extra"?[1,2,3]:[1]));
            if(scene=="fire"){Reach(g,()=>Action(g,"current-slash-fire"));Answer(g,c=>c.Parameters.GetValueOrDefault("branch")=="convert"&&c.Targets.Count==0);}
            if(scene=="decline"){Reach(g,()=>Optional(g));g=Cold(g,r);Answer(g,c=>c.Parameters.GetValueOrDefault("program-action")=="skip");Play(g);Need(E<ChenglieStartedEvent>(g).Length==0&&E<ChenglieAttachmentPlacedEvent>(g).Length==0,"Declining the true optional window never adds targets, exchanges or deposits cards.");continue;}
            if(scene=="no-extra"){Play(g);Need(E<ChenglieStartedEvent>(g).Length==0,"Already designating every legal other character offers no zero-addition Chenglie exchange.");continue;}
            Activate(g);var start=Current(g)!;var extra=scene=="add-two"?new[]{2,3}:scene=="prepared"?new[]{3}:new[]{2};foreach(var seat in extra)Pick(g,"target",targets:[seat]);if(Current(g)is{Stage:ChenglieStage.Targets})Pick(g,"targets-done");
            Reach(g,()=>Current(g)?.Stage is ChenglieStage.Exchange or ChenglieStage.Assignment||E<ChenglieSetupReadyEvent>(g).Length>0);g=Cold(g,r);FinishSetup(g);Play(g);
            var applied=E<ChenglieTargetsAppliedEvent>(g).Single();var finished=E<CardUseFinishedEvent>(g).Where(e=>e.ResolutionId==applied.UseId).ToArray();
            Need(applied.AddedCount==extra.Length&&finished.Length==1&&E<ChenglieAttachmentPlacedEvent>(g).Length==(scene=="prepared"?3:1+extra.Length)&&E<ChenglieLeaseSettledEvent>(g).Count(e=>e.UseId==applied.UseId)==1,"One original Use/action receives one or two new targets and owns every exact temporary entity until its one whole-use settlement.");
            Need(E<DamageAppliedEvent>(g).Count(e=>e.Amount>0)==(scene=="prepared"?3:1+extra.Length)&&Enumerable.Range(1,3).All(s=>V(g,s).Hp==hp[s]),"Actual red targets first take native damage and only then recover; all selected target/effect cursors return through the same use.");
            if(scene=="zero")Need(E<CardUseDeclaredEvent>(g).Single(e=>e.ResolutionId==applied.UseId).CardId==0,"The full secret-card and reward protocol also follows a genuine zero-entity native Slash.");
            if(scene=="fire")Need(E<ProgramCurrentSlashFireChangedEvent>(g).Length==1&&E<DamageAppliedEvent>(g).All(e=>e.Nature==DamageNature.Fire),"A real committed fire conversion survives the subsequent Chenglie target rewrite.");
            if(scene is"add-one"or"fire")Need(E<CompletedUndamagedUseDamageRecordedEvent>(g).Count(e=>e.CardUseFrameId==applied.UseId)==1+extra.Length,"The retained whole-use tracker replays the original accepted action through Chenglie's true target mutation.");_=Cold(g,r);
        }
        {
            var(g,r)=Start("aud-liuli");var issuer=IssuerSeat(g);var first=Enumerable.Range(1,3).First(s=>s!=issuer);Use(g,"give",V(g,0).Hand.Take(V(g,0).HandCount-1).Select(c=>c.Id).ToArray(),[first]);Play(g);Use(g,"hurt",targets:[issuer]);Reach(g,()=>P(g)is{PlayerSeat:0,SkillPrompt.SkillId:"classic:liuli"}p&&p.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="activate"));var original=g.ResolutionStack.OfType<CardUseFrame>().SingleOrDefault(u=>u.ChenglieLease is not null);Need(original is not null,"The late added-target Liuli window must follow the real Chenglie setup: "+Diagnostic(g));var lease=original!.ChenglieLease!;
            Need(!lease.Original.Contains(0)&&lease.Added.Contains(0)&&lease.Cards.Any(c=>c.HolderSeat==0),"The human recipient is genuinely added by Chenglie and already owns its secretly assigned original attachment before its late Liuli window.");Answer(g,c=>c.Parameters.GetValueOrDefault("program-action")=="activate");Reach(g,()=>Action(g,"select-target"));var next=P(g)!.Choices.First(c=>c.Targets.Count==1).Targets.Single();Answer(g,c=>c.Targets.SequenceEqual([next]));Reach(g,()=>Action(g,"select-and-move-owned-card"));g=Cold(g,r);Answer(g,c=>c.Cards.Count==1);Play(g);
            Need(E<ChenglieRedirectedEvent>(g).Single()is{PreviousSeat:0}redirect&&redirect.Seat==next&&E<CardUseFinishedEvent>(g).Count(e=>e.ResolutionId==lease.UseId)==1&&E<ChenglieLeaseSettledEvent>(g).Count(e=>e.UseId==lease.UseId)==1&&E<ChenglieAttachmentPlacedEvent>(g).Any(e=>e.HolderSeat==0)&&!E<ChenglieResponseRecordedEvent>(g).Any(e=>e.HolderSeat==0)&&E<ChenglieOutcomeIssuedEvent>(g).Any(e=>e.HolderSeat==0&&!e.Responded),"Real classic Liuli redirects the native effect without rewriting the designated Action or moving the frozen original holder's attachment; the same whole Use still pays and cleans every original lease once.");_=Cold(g,r);
        }
    }
    private static PlayerSnapshot V(GameEngine g,int seat)=>g.CreateSnapshot(seat).Players[seat];
    private static PendingDecision? P(GameEngine g)=>Enumerable.Range(0,4).Select(s=>g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p=>p is not null);
    private static T[] E<T>(GameEngine g)where T:IGameEvent=>g.Events.Select(e=>e.Payload).OfType<T>().ToArray();
    private static ChenglieReceipt? Current(GameEngine g)=>g.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f=>f.Chenglie is not null)?.Chenglie;
    private static int IssuerSeat(GameEngine g)=>Enumerable.Range(1,3).Single(s=>V(g,s).GeneralId==Issuer);
    private static bool Action(GameEngine g,string action)=>P(g)?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")==action)==true;
    private static bool Optional(GameEngine g)=>P(g)is{PlayerSeat:0,SkillPrompt.SkillId:Skill}p&&p.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
    private static CardKind? Kind(GameEngine g,LegalAction a)=>a.PlayedCardKind??V(g,0).Hand.FirstOrDefault(c=>c.Id==a.CardId)?.Kind;
    private static void Need(bool yes,string why){if(!yes)throw new InvalidOperationException("scene="+Scene+" "+why);}
    private static void Accept(GameEngine g,GameCommand c){try{var result=g.Submit(CommandJson.Deserialize(CommandJson.Serialize([c])).Single());Need(result.Accepted,result.Error?.Message??"Rejected real MaCheng command");}catch(Exception e){throw new InvalidOperationException("scene="+Scene+" command="+c.GetType().Name+" cause="+e.Message+" "+Diagnostic(g),e);}}
    private static void Answer(GameEngine g,Func<PromptChoice,bool> match){var p=P(g)!;var c=p.Choices.FirstOrDefault(match);Need(p.PlayerSeat==0&&c is not null,"Missing real human choice "+Diagnostic(g));Accept(g,new AnswerPromptCommand(0,p.PromptId,c!.Id,g.Revision));}
    private static void Pick(GameEngine g,string branch,int[]? cards=null,int[]? targets=null)=>Answer(g,c=>c.Parameters.GetValueOrDefault("program-action")=="chenglie"&&c.Parameters.GetValueOrDefault("branch")==branch&&(cards is null||c.Cards.SequenceEqual(cards))&&(targets is null||c.Targets.SequenceEqual(targets)));
    private static void Activate(GameEngine g){Reach(g,()=>Optional(g));Answer(g,c=>c.Parameters.GetValueOrDefault("program-action")=="activate");Reach(g,()=>Current(g)is{Stage:ChenglieStage.Targets});}
    private static void FinishSetup(GameEngine g)
    {
        for(var i=0;i<25;i++)
        {
            if(E<ChenglieSetupReadyEvent>(g).Length>0)return;
            Reach(g,()=>Current(g)?.Stage is ChenglieStage.Exchange or ChenglieStage.Assignment||E<ChenglieSetupReadyEvent>(g).Length>0);
            if(E<ChenglieSetupReadyEvent>(g).Length>0)return;var stage=Current(g)!.Stage;Pick(g,stage==ChenglieStage.Exchange?"exchange":"assign");
        }
        throw new InvalidOperationException("Chenglie failed to assign the bounded original pool "+Diagnostic(g));
    }
    private static void Use(GameEngine g,string id,int[]? cards=null,int[]? targets=null)=>Accept(g,new UseProgramSkillCommand(0,Driver,id,cards??[],targets??[],g.Revision,P(g)!.PromptId));
    private static void Native(GameEngine g,Func<LegalAction,bool> match){var a=g.GetHumanLegalActions().FirstOrDefault(match);Need(a is not null,"Missing native action "+Diagnostic(g));Accept(g,new PlayCardCommand(0,a!.CardId!.Value,a.TargetSeats,g.Revision,P(g)!.PromptId,a.PlayedCardKind,a.TargetCardId){ConversionSource=a.ConversionSource,AdditionalConversionSources=a.AdditionalConversionSources??[]});}
    private static void End(GameEngine g)=>Accept(g,new EndPlayPhaseCommand(0,g.Revision,P(g)!.PromptId));
    private static void Play(GameEngine g)=>Reach(g,()=>P(g)is{PlayerSeat:0,Kind:DecisionKind.PlayCard});
    private static void Reach(GameEngine g,Func<bool> done,[CallerMemberName]string caller="",[CallerLineNumber]int line=0)
    {
        for(var i=0;i<350;i++)
        {
            if(done())return;Need(g.State.Winner==Winner.None,$"Ended before {caller}:{line} "+Diagnostic(g));var p=P(g);
            if(p is null||p.PlayerSeat!=0){Accept(g,new AdvanceOneStepCommand(g.Revision));continue;}
            if(p.Kind==DecisionKind.PlayCard)End(g);
            else if(p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification)Answer(g,c=>c.Cards.Count==0);
            else if(p.Kind==DecisionKind.RescueDying)Answer(g,c=>c.Parameters.GetValueOrDefault("response")is"let-die"or"pass");
            else if(p.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="skip"))Answer(g,c=>c.Parameters.GetValueOrDefault("program-action")=="skip");
            else throw new InvalidOperationException($"Unexpected {caller}:{line} "+Diagnostic(g));
        }
        throw new InvalidOperationException($"Missing bounded {caller}:{line} "+Diagnostic(g));
    }
    private static string Diagnostic(GameEngine g)=>JsonSerializer.Serialize(new{Scene,g.State.TurnNumber,g.State.CurrentSeat,g.State.Phase,Players=Enumerable.Range(0,4).Select(s=>new{Seat=s,V(g,s).GeneralId,V(g,s).Hp,V(g,s).HandCount,V(g,s).IsAlive}),Prompt=P(g)is{}p?new{p.PlayerSeat,p.Kind,Skill=p.SkillPrompt?.SkillId,Choices=p.Choices.Take(5).Select(c=>new{c.Cards,c.Targets,c.Parameters})}:null,Frames=g.ResolutionStack.TakeLast(7).Select(f=>new{f.Id,Type=f.GetType().Name,Skill=(f as ProgramSkillFrame)?.SkillId,Instruction=(f as ProgramSkillFrame)?.InstructionIndex,Stage=(f as ProgramSkillFrame)?.Chenglie?.Stage}),Facts=g.Events.TakeLast(5).Select(e=>e.Payload.ToString())});
    private static string State(GameEngine g)=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,4).Select(s=>SnapshotJson.Serialize(g.CreateSnapshot(s))),Frames=g.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())),Events=g.Events.Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())),g.CardMovements,Zones=g.CreateCardZoneDiagnostics(),Commands=CommandJson.Serialize(g.AcceptedCommands)});
    private static GameEngine Cold(GameEngine g,ContentRegistry r){var restored=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())),r);Need(State(g)==State(restored),"Cold replay preserves every filtered view, original native Use/action, source/hash, private original entity mapping, response bool and once-paid child cursor.");return restored;}
    private static(GameEngine,ContentRegistry)Start(string scene)
    {
        Scene=scene;ContentRegistry r;try{r=ContentRegistry.Build(new StandardContentPackage(),new Fixture(scene));}catch(Exception e){throw new InvalidOperationException("scene="+scene+" fixture cause="+e.Message,e);}var g=GameEngine.CreateStandard(new GameOptions{Seed=7,PlayerCount=4,HumanSeat=0,HumanRole=Role.Lord,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=false,AdvanceAfterHumanCommands=false,MaxTurns=12},r);Accept(g,new StartGameCommand());Reach(g,()=>P(g)is{PlayerSeat:0,Kind:DecisionKind.SelectGeneral});Accept(g,new SelectGeneralCommand(0,Owner,g.Revision,P(g)!.PromptId));Play(g);return(g,r);
    }
    private sealed class Fixture(string scene):IGameContentPackage
    {
        public PackageManifest Manifest {get;}=new("fixture-ma-cheng",new(1,0,0),[]);
        public void Register(IContentRegistryBuilder b)
        {
            CharacterFixtureContent.Register(b,"ol:ma-cheng","classic:ma-chao");if(scene is"add-one"or"fire")CharacterFixtureContent.Register(b,"ol:pei-yuan-shao");if(scene=="aud-double")CharacterFixtureContent.Register(b,"classic:lu-bu");if(scene=="aud-faction")CharacterFixtureContent.Register(b,"classic:cao-cao");if(scene=="aud-liuli")CharacterFixtureContent.Register(b,"classic:da-qiao");
            var audience=scene.StartsWith("aud-",StringComparison.Ordinal);var gear=scene is"aud-lion"or"aud-loss"or"aud-death"?CardKind.SilverLion:scene=="aud-ox"?CardKind.WoodenOx:scene is"prepared"or"no-extra"?CardKind.FangtianHalberd:audience?CardKind.Dodge:CardKind.Crossbow;var input=char.ToLowerInvariant(gear.ToString()[0])+gear.ToString()[1..];var defs=new List<JsonNode>();
            static JsonNode N(string s)=>JsonNode.Parse(s)!;
            void S(string id,string body)=>defs.Add(N("{\"id\":\""+id+"\",\"revision\":1,"+body+"}"));
            JsonNode A(string id,string effects,int cards=0,int targets=0)=>new JsonObject{["id"]=id,["minCards"]=cards,["maxCards"]=cards,["minTargets"]=targets,["maxTargets"]=targets,["targetKind"]="anyLiving",["usesPerTurn"]=null,["effects"]=N("["+effects+"]")};
            var give=A("give","""{"op":"giveSelected","target":"selectedTarget"}""",1,1);give["maxCards"]=30;
            var acts=new JsonArray(A("stock","""{"op":"draw","target":"owner","amount":8}"""),give,A("wound","""{"op":"loseHp","target":"owner","amount":1}"""),A("hurt","""{"op":"loseHp","target":"selectedTarget","amount":1}""",0,1),A("fatal-target","""{"op":"loseHp","target":"selectedTarget","amount":11}""",0,1),A("discard-two","""{"op":"selectOwnedCards","target":"owner","amount":2,"zones":["hand"],"resultBind":"discarded"},{"op":"moveBoundCards","target":"owner","sourceBind":"discarded","destination":"discardPile","awaitMovementTriggers":true}"""),A("zero","""{"op":"selectTarget","target":"owner","targetKind":"otherLivingVirtualSlashTarget"},{"op":"useVirtualSlash","target":"selectedTarget"}"""));
            var driver=new JsonObject{["id"]=Driver,["revision"]=1,["modifiers"]=N("""[{"id":"keep","query":"handLimit","operation":"add","value":40,"priority":0},{"id":"range","query":"attackRange","operation":"set","value":4,"priority":0},{"id":"slashes","query":"slashLimit","operation":"add","value":3,"priority":0}]"""),["activations"]=acts,["viewAs"]=new JsonArray(JsonSerializer.SerializeToNode(new{id="slash",inputKinds=new[]{input},inputSuits=Array.Empty<string>(),outputKind="slash",forPlay=true,forResponse=true,allowSameKind=true}),JsonSerializer.SerializeToNode(new{id="dodge",inputKinds=new[]{input},inputSuits=Array.Empty<string>(),outputKind="dodge",forPlay=false,forResponse=true,allowSameKind=true}))};defs.Add(driver);
            S(Quiet,"\"triggers\":[{\"id\":\"quiet\",\"window\":\"afterNormalDraw\",\"subject\":\"owner\",\"optional\":false,\"effects\":[{\"op\":\"skipTurnPhases\",\"target\":\"owner\",\"phases\":[\"play\"]}]}]");
            if(scene=="fire")S("fixture:mc-fire","\"triggers\":[{\"id\":\"real-fire\",\"window\":\"cardUseCommitted\",\"ownerRelation\":\"actor\",\"cardKinds\":[\"slash\"],\"optional\":false,\"effects\":[{\"op\":\"offerCurrentSlashFireAndExtraTarget\",\"target\":\"owner\"}]}]");
            if(audience)S("fixture:mc-incoming","\"modifiers\":[{\"id\":\"real-range\",\"query\":\"attackRange\",\"operation\":\"set\",\"value\":4,\"priority\":0}],\"triggers\":[{\"id\":\"real-health-triggered-slash\",\"window\":\"afterHealthChanged\",\"subject\":\"owner\",\"condition\":{\"kind\":\"healthChangeKindIs\",\"hpChangeKind\":\"loss\"},\"optional\":false,\"usageScope\":\"game\",\"usageLimit\":1,\"effects\":[{\"op\":\"selectTarget\",\"target\":\"owner\",\"targetKind\":\"otherLivingHighestHand\"},{\"op\":\"useVirtualSlash\",\"target\":\"selectedTarget\"}]}]");
            if(scene is"aud-loss"or"aud-death")S("fixture:mc-conditional","\"hpSkillGrants\":[{\"id\":\"living-chenglie\",\"skillId\":\"ol:chenglie\",\"minimumHp\":11}]");
            if(scene is"aud-lion"or"aud-ox"or"aud-loss"or"aud-death")
            {
                var effects=new JsonArray(N("""{"op":"chooseOption","target":"owner","resultBind":"child","options":[{"id":"continue"}]}"""));if(scene is"aud-loss"or"aud-death"){effects.Add(N("""{"op":"selectTarget","target":"owner","targetKind":"otherLiving"}"""));effects.Add(JsonSerializer.SerializeToNode(new{op="loseHp",target="selectedTarget",amount=scene=="aud-death"?20:1}));}
                var trigger=new JsonObject{["id"]="real-paid-child",["window"]=scene=="aud-ox"?"cardsMoved":"afterHealthChanged",["subject"]="owner",["optional"]=false,["effects"]=effects};if(scene=="aud-ox"){trigger["sourceZones"]=N("[\"woodenOxGrain\"]");trigger["movementOccurrence"]="perBatch";trigger["movementReasons"]=N("[\"equipment.wooden-ox.grain-discard\"]");}else trigger["condition"]=N("""{"kind":"healthChangeKindIs","hpChangeKind":"recovery"}""");defs.Add(new JsonObject{["id"]=Watch,["revision"]=1,["triggers"]=new JsonArray(trigger)});
            }
            var labels=defs.ToDictionary(n=>n["id"]!.GetValue<string>(),n=>new{name=n["id"]!.GetValue<string>(),description="马承原生真实小夹具",optionLabels=new[]{"activations","triggers"}.SelectMany(k=>n[k]?.AsArray()??new JsonArray()).SelectMany(t=>t!["effects"]!.AsArray()).SelectMany(e=>e!["options"]?.AsArray()??new JsonArray()).Select(o=>o!["id"]!.GetValue<string>()).Distinct().ToDictionary(id=>id,id=>id)});var catalog=SkillProgramCatalog.Load(JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.RulesSchemaVersion,skills=defs}),JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=labels}));foreach(var(id,p)in catalog.Programs)b.AddSkill(new(id,id,"实际马承边界"){Program=p,ProgramPresentation=catalog.Presentations[id]});
            b.AddSkill(new("fixture:mc-own-pick","本人","固定公开评分"){SelectionWeights=Enum.GetValues<Role>().ToDictionary(role=>role,_=>-100000000d)});var own=new List<string>{Driver};if(!audience)own.Add(Skill);if(scene=="fire")own.Add("fixture:mc-fire");if(scene is"aud-lion"or"aud-ox"or"aud-loss"or"aud-death")own.Add(Watch);if(scene=="aud-faction")own.Add("classic:hujia");if(scene=="aud-liuli")own.Add("classic:liuli");b.AddGeneral(new(Owner,"马承真实本人","ol-ma-cheng","fixture:mc-own-pick","wei",12,own,GeneralGender.Male));
            var peers=audience?new[]{Issuer,"fixture:mc-provider","fixture:mc-peer"}:new[]{"fixture:mc-peer-1","fixture:mc-peer-2","fixture:mc-peer-3"};
            foreach(var peer in peers)
            {
                var skills=new List<string>();var pick=Quiet;
                if(peer==Issuer){pick="fixture:mc-issuer-pick";b.AddSkill(new(pick,"真实发动者","固定公开身份评分"){SelectionWeights=Enum.GetValues<Role>().ToDictionary(role=>role,role=>role==Role.Rebel?1000000d:-1000000d)});skills.AddRange([Quiet,"fixture:mc-incoming",scene is"aud-loss"or"aud-death"?"fixture:mc-conditional":Skill]);if(scene=="aud-double")skills.Add("classic:wushuang");}
                if(peer=="fixture:mc-provider"&&scene=="aud-faction"){pick="fixture:mc-provider-pick";b.AddSkill(new(pick,"真实护驾者","固定公开身份评分"){SelectionWeights=Enum.GetValues<Role>().ToDictionary(role=>role,role=>role==Role.Loyalist?1000000d:-1000000d)});skills.Add(Quiet);}
                b.AddGeneral(new(peer,"真实其他角色","supporter",pick,"wei",12,skills,GeneralGender.Male));
            }
            b.AddCard(new("fixture:mc-card","真实"+gear,CardCatalog.Get(gear).CategoryName,"同质原生实体",gear));b.AddDeck(new("fixture:mc-deck","马承小牌堆",4,2,[]){PhysicalCards=Enumerable.Range(0,scene is"shortage"or"reshuffle"?18:128).Select(_=>new ContentDeckPhysicalCard("fixture:mc-card",scene=="black"?Suit.Spade:Suit.Heart,7)).ToArray()});b.AddMode(new(Mode,"马承真实行为",4,4,scene=="aud-faction"?new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Loyalist),1},{nameof(Role.Rebel),2}}:new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Rebel),3}},"fixture:mc-deck",GeneralCandidateCount:4,GeneralPoolIds:[Owner,..peers]));
        }
    }
}
