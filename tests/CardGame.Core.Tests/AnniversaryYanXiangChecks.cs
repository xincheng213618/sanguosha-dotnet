using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class AnniversaryYanXiangChecks
{
 private const string Hero="anniversary:yan-xiang",Yuzheng="anniversary:yuzheng",Suishi="anniversary:suishi",Peer="fixture:yan-xiang-peer-",NoDraw="fixture:yan-xiang-no-draw",Watch="fixture:yan-xiang-native-watch",Deck="fixture:yan-xiang-deck",Mode="identity:classic-yan-xiang";
 public static void EndingNativePlansRespectExhaustedTrueUseQuotaCold()
 {
  var s=Scene.Start(false);s.UseYuzheng(0);s.Menu(AnniversaryYanXiangStage.Option);s.Answer(c=>Value(c)=="2");s.Play(0);
  var quota=s.E<AnniversaryYanXiangLeaseEvent>().Single(e=>e.Option==2);
  s.NativeKind(0,CardKind.Crossbow);s.Play(0);
  for(var i=1;i<quota.Amount;i++){s.NativeKind(0,CardKind.Slash,3);s.Play(0);}
  s.Need(s.E<CurrentTurnCardUseKindsRecordedEvent>().Skip(quota.UseCutoff).Count(e=>e.ActorSeat==0)==quota.Amount,
   "The native ending test first spends the recipient's exact admitted true-use quota through real cards.");
  s.End(0);s.Offer();s.Activate();s.Menu(AnniversaryYanXiangStage.DamageName);s.Answer(c=>Value(c)==CardKind.Slash.ToString());s.Menu(AnniversaryYanXiangStage.UseChoice);
  var frame=s.Root!.Id;var issued=s.E<AnniversaryYanXiangUseIssuedEvent>().Length;
  s.Need(s.P!.Choices is[{Parameters:var choice}]&&choice.GetValueOrDefault("verb")=="decline",
   "Suishi publishes only decline when this actor cannot begin another true Use; its custom HE conversion cannot bypass the quota.");
  s.Cold();s.Answer(c=>c.Parameters.GetValueOrDefault("verb")=="decline");
  s.Reach(()=>s.E<AnniversaryYanXiangCompletedEvent>().Any(e=>e.FrameId==frame));
  s.Need(s.E<AnniversaryYanXiangUseIssuedEvent>().Length==issued,"Cold unpaid decline never invents another accepted action or material payment.");
 }
 private static string? Value(PromptChoice c)=>c.Parameters.GetValueOrDefault("value");
 private sealed class Scene(GameEngine game,ContentRegistry registry)
 {
  public GameEngine G{get;private set;}=game;
  public PendingDecision? P=>Enumerable.Range(0,4).Select(i=>G.CreateSnapshot(i).PendingDecision).FirstOrDefault(p=>p is not null);
  public ProgramSkillFrame? Root=>G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(f=>f.AnniversaryYanXiang is not null);
  public PlayerSnapshot V(int seat)=>G.CreateSnapshot(seat).Players.Single(p=>p.Seat==seat);
  public T[] E<T>()=>G.Events.Select(e=>e.Payload).OfType<T>().ToArray();
  public CardLocation Zone(int id)=>G.CreateCardZoneDiagnostics().Single(c=>c.CardId==id).Location;
  public void Play(int actor)=>Reach(()=>P is{Kind:DecisionKind.PlayCard}p&&p.PlayerSeat==actor);
  public void End(int actor)=>Accept(new EndPlayPhaseCommand(actor,G.Revision,P!.PromptId));
  public void Discard(IReadOnlyList<int> ids){var p=P!;Accept(new DiscardCardsCommand(p.PlayerSeat,ids,p.PromptId,G.Revision));}
  public void UseYuzheng(int target)=>Accept(new UseProgramSkillCommand(0,Yuzheng,"admonish",[],[target],G.Revision,P!.PromptId));
  public void Offer()=>Reach(()=>P?.SkillPrompt?.SkillId==Suishi&&P.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="activate"));
  public void Activate()=>Answer(c=>c.Parameters.GetValueOrDefault("program-action")=="activate");
  public void Menu(AnniversaryYanXiangStage stage)=>Reach(()=>Root?.AnniversaryYanXiang?.Stage==stage&&P?.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="anniversary-yan-xiang")==true);
  public void Continue()=>Answer(c=>c.Parameters.GetValueOrDefault("option-id")=="continue");
  public void NativeKind(int actor,CardKind kind,int? target=null)=>Native(actor,V(actor).Hand.First(c=>c.Kind==kind).Id,target);
  public void Native(int actor,int id,int? target=null)
  {
   var a=G.GetLegalActions(actor).First(a=>a.CardId==id&&a.ConversionSource is null&&(target is null||a.TargetSeats.SequenceEqual([target.Value])));
   Accept(new PlayCardCommand(actor,id,a.TargetSeats,G.Revision,P!.PromptId,a.PlayedCardKind,a.TargetCardId));
  }
  public void Answer(Func<PromptChoice,bool> predicate)
  {var p=P!;var c=p.Choices.FirstOrDefault(predicate);Need(c is not null,"The actual current human must receive the intended private recipient/native choice.");Accept(new AnswerPromptCommand(p.PlayerSeat,p.PromptId,c!.Id,G.Revision));}
  public void Reach(Func<bool> ready)
  {
   for(var step=0;step<500;step++)
   {
    if(ready())return;
    if(P is{}p)
    {
     if(p.Kind==DecisionKind.PlayCard){End(p.PlayerSeat);continue;}
     if(p.Kind==DecisionKind.DiscardCards){var ids=p.ValidCardIds.OrderBy(id=>V(p.PlayerSeat).Hand.Single(c=>c.Id==id).Kind==CardKind.Dodge?1:0).Take(p.RequiredCardCount).ToArray();Accept(new DiscardCardsCommand(p.PlayerSeat,ids,p.PromptId,G.Revision));continue;}
     if(p.Kind==DecisionKind.SelectFaction){Answer(_=>true);continue;}
     if(p.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification or DecisionKind.RescueDying){Answer(c=>c.Cards.Count==0);continue;}
     if(p.Kind is DecisionKind.FireAttackReveal or DecisionKind.FireAttackDiscard){Answer(c=>c.Cards.Count>0);continue;}
     if(p.SkillPrompt?.SkillId==Watch){Continue();continue;}
     if(p.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="activate")){Answer(c=>c.Parameters.GetValueOrDefault("program-action")!="activate");continue;}
     Need(false,"Unexpected Yan Xiang native boundary.");
    }
    Need(G.State.Winner==Winner.None,"The short native scene retains living participants.");Accept(new AdvanceOneStepCommand(G.Revision));
   }
   Need(false,"The bounded native scene missed its requested original continuation.");
  }
  private string State()=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,4).Select(i=>SnapshotJson.Serialize(G.CreateSnapshot(i))).ToArray(),Frames=G.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),Events=G.Events.Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())).ToArray(),G.CardMovements,Zones=G.CreateCardZoneDiagnostics(),Commands=CommandJson.Serialize(G.AcceptedCommands)});
  public void Cold(){var before=State();G=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())),registry);Need(before==State(),"Cold preserves the original paid child, recipient and native action identically.");}
  public void Replay(){var before=State();G=GameReplay.Replay(G.CreateCheckpoint().Options,G.AcceptedCommands,registry);Need(before==State(),"The accepted journal replays actual branch payments, issued effects and native damage returns exactly.");}
  public void Need(bool okay,string message){if(!okay)throw new InvalidOperationException(message+" "+JsonSerializer.Serialize(new{G.State.TurnNumber,G.State.CurrentSeat,P,Receipt=Root?.AnniversaryYanXiang,Last=G.Events.TakeLast(7).Select(e=>JsonSerializer.Serialize(e.Payload,e.Payload.GetType())).ToArray()}));}
  private void Accept(GameCommand command){var result=G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());Need(result.Accepted,result.Error?.Message??"Rejected original Yan Xiang command.");}
  public static Scene Start(bool damage)
  {
   var hands=Hands(damage);var stock=Enumerable.Range(0,80).Select(_=>new ContentDeckPhysicalCard("standard:slash",Suit.Club,7)).ToArray();
   // One fixed-seed native deal determines entity positions. Replacing the recipe
   // at those IDs preserves the same native shuffle, setup, deal and replay.
   var probe=Boot(ContentRegistry.Build(new StandardContentPackage(),new Fixture(damage,stock)));
   foreach(var seat in Enumerable.Range(0,4))
   {
    var ids=probe.V(seat).Hand.Select(c=>c.Id).ToArray();probe.Need(ids.Length==hands[seat].Length,"The fixed identity table deals its exact sparse initial hand counts.");
    for(var i=0;i<ids.Length;i++)stock[ids[i]-1]=new(probe.registryCard(hands[seat][i].Kind),hands[seat][i].Suit,hands[seat][i].Rank);
   }
   return Boot(ContentRegistry.Build(new StandardContentPackage(),new Fixture(damage,stock)));
  }
  private string registryCard(CardKind kind)=>registry.Cards.Values.First(c=>c.LegacyKind==kind).Id;
  private static Scene Boot(ContentRegistry registry)
  {
   var g=GameEngine.CreateStandard(new GameOptions{Seed=47,PlayerCount=4,HumanSeat=0,HumanSeats=[0,1,2,3],HumanRole=Role.Lord,AiPolicyVersion=AiDecisionPolicy.CurrentVersion,ModeId=Mode,UseInteractiveSetup=true,UseInteractiveDiscard=true,AdvanceAfterHumanCommands=false,MaxTurns=12},registry);
   var s=new Scene(g,registry);s.Accept(new StartGameCommand());
   for(var n=0;n<30;n++)
   {
    if(s.P is{Kind:DecisionKind.PlayCard,PlayerSeat:0})return s;
    if(s.P is{Kind:DecisionKind.SelectGeneral}p){s.Accept(new SelectGeneralCommand(p.PlayerSeat,p.PlayerSeat==0?Hero:Peer+p.PlayerSeat,g.Revision,p.PromptId));continue;}
    if(s.P is{Kind:DecisionKind.SelectFaction}){s.Answer(_=>true);continue;}
    s.Need(s.P is null,"The fixed four-general identity opening reaches only its actual selection and deal boundaries.");s.Accept(new AdvanceOneStepCommand(g.Revision));
   }
   throw new InvalidOperationException("The fixed identity opening did not reach its first actual Play phase.");
  }
 }
 private static ShanheStartingCard[][] Hands(bool damage)=>damage?
 [
  [new(CardKind.Crossbow,Suit.Spade,1),new(CardKind.Peach,Suit.Heart,2),new(CardKind.Slash,Suit.Club,7),new(CardKind.Dodge,Suit.Heart,2)],
  [new(CardKind.Peach,Suit.Heart,2),new(CardKind.Peach,Suit.Club,3),new(CardKind.Dodge,Suit.Heart,2)],
  [new(CardKind.Peach,Suit.Heart,2),new(CardKind.Peach,Suit.Club,3),new(CardKind.Dodge,Suit.Heart,2)],
  [new(CardKind.Peach,Suit.Heart,2),new(CardKind.Peach,Suit.Club,3),new(CardKind.Dodge,Suit.Heart,2)]
 ]:
 [
  [new(CardKind.Crossbow,Suit.Spade,1),new(CardKind.Dodge,Suit.Heart,2),new(CardKind.Dodge,Suit.Heart,3),new(CardKind.Dodge,Suit.Club,3),new(CardKind.Peach,Suit.Heart,2)],
  [new(CardKind.Peach,Suit.Heart,2),new(CardKind.Peach,Suit.Club,3)],
  [new(CardKind.Peach,Suit.Heart,2),new(CardKind.Peach,Suit.Club,3)],
  [new(CardKind.Crossbow,Suit.Spade,1),new(CardKind.ArrowBarrage,Suit.Spade,7),new(CardKind.Dodge,Suit.Heart,2)]
 ];
 private static string HandSkill(int seat)=>"fixture:yan-xiang-hand-"+seat;
 private sealed class Fixture(bool damage,IReadOnlyList<ContentDeckPhysicalCard> stock):IGameContentPackage
 {
  public PackageManifest Manifest{get;}=new("fixture-anniversary-yan-xiang",new(1,0,0));
  public void Register(IContentRegistryBuilder b)
  {
   CharacterFixtureContent.Register(new OwnerBuilder(b),Hero);
   var rules=JsonNode.Parse($$"""
   {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
    {"id":"{{NoDraw}}","revision":1,"triggers":[{"id":"no-normal-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"isSkillInvocation":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
    {"id":"{{Watch}}","revision":1,"triggers":[
     {"id":"paid-discard","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementReasons":["skill-program.anniversary:yuzheng.discard"],"movementOccurrence":"perBatch","usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]},
     {"id":"native-damage-use","window":"cardUseBeforeTargetEffects","ownerRelation":"actor","cardKinds":["slash","duel","barbarianAssault","arrowBarrage","fireAttack"],"usageScope":"game","usageLimit":1,"optional":false,"isSkillInvocation":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"native","options":[{"id":"continue"}]}]}
    ]}
   ]}
   """)!;
   var names=new Dictionary<string,object>{[NoDraw]=new{name="固定真实手牌",description="保留原生阶段，正常摸牌调整为零。"},[Watch]=new{name="原生付款与使用暂停",description="在原帧真实子窗暂停。",optionLabels=new Dictionary<string,string>{["continue"]="继续"}}};
   for(var seat=0;seat<4;seat++)
   {
    rules["skills"]!.AsArray().Add(JsonNode.Parse($$"""{"id":"{{HandSkill(seat)}}","revision":1,"modifiers":[{"id":"sparse-opening","query":"initialHandSize","operation":"add","value":{{Hands(damage)[seat].Length}},"priority":0}]}"""));
    names[HandSkill(seat)]=new{name="固定稀疏发牌",description="保留原生实体发牌及洗牌。"};
   }
   var catalog=SkillProgramCatalog.Load(rules.ToJsonString(),JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=names}));
   foreach(var(id,p)in catalog.Programs)b.AddSkill(new(id,catalog.Presentations[id].Name,catalog.Presentations[id].Description){Program=p,ProgramPresentation=catalog.Presentations[id]});
   for(var seat=1;seat<4;seat++)b.AddGeneral(new(Peer+seat,"固定真实同桌","",NoDraw,"wei",12,[Watch,HandSkill(seat)]){InitialHp=damage?3:12});
   b.AddDeck(new(Deck,"固定实际实体库存",0,2,[]){PhysicalCards=stock});
   b.AddMode(new(Mode,"阎象真实分支和原生伤害",4,4,new Dictionary<string,int>{[nameof(Role.Lord)]=1,[nameof(Role.Rebel)]=3},Deck,GeneralCandidateCount:4,GeneralPoolIds:[Hero,Peer+1,Peer+2,Peer+3]));
  }
 }
 private sealed class OwnerBuilder(IContentRegistryBuilder b):IContentRegistryBuilder
 {
  public void AddGeneral(ContentGeneralDefinition d)=>b.AddGeneral(d.Id==Hero?d with{InitialHp=2,AdditionalSkillIds=[..d.AdditionalSkillIds??[],NoDraw,Watch,HandSkill(0)]}:d);
  public void AddSkill(ContentSkillDefinition d)=>b.AddSkill(d);public void AddCard(ContentCardDefinition d)=>b.AddCard(d);public void AddDeck(ContentDeckRecipe d)=>b.AddDeck(d);public void AddMode(ContentModeDefinition d)=>b.AddMode(d);
 }
}
