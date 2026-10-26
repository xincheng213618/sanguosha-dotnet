using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

namespace CardGame.Core.Tests;

internal static class HappyShenLuSuShopMovementChecks
{
    private const string Human="fixture:happy-ls-shop-owner",Peer="fixture:happy-ls-shop-peer",
        Driver="fixture:happy-ls-shop-driver",Watch="fixture:happy-ls-shop-watch",Quiet="fixture:happy-ls-shop-quiet";

    public static void RealShopEquipmentPaysBothDingzhouBranchesOnceCold()
    {
        // Seed227 and the owned Zhiheng exclusion match the retained native shop
        // mechanism check: the first page publishes the real free offer4186.
        var(field,fieldRegistry,fieldEquipment)=Start();
        Skill(field,Driver,"field",[1]);
        Reach(field,p=>p.Kind==DecisionKind.SelectTargetCard||p.Choices.Any(c=>c.Cards.Count==1));
        Answer(field,c=>c.Cards.Count==1);Play(field);
        var peerField=field.CreateCardZoneDiagnostics().Single(c=>c.Location==CardLocation.Equipment(1)).CardId;
        Skill(field,"happy:dingzhou","exchange",[1]);Reach(field,p=>Dingzhou(p,"field"));Answer(field,c=>Option(c)=="field");
        Reach(field,p=>Dingzhou(p,"give:"));Answer(field,c=>c.Cards.SequenceEqual([fieldEquipment]));
        Reach(field,p=>p.SkillPrompt?.SkillId==Watch);
        var fieldFrame=DingzhouFrame(field);var paidField=fieldFrame.HappyDingzhou!;
        Need(paidField is{Stage:HappyDingzhouStage.FieldGiftChildren,Paid:true,Amount:1}&&
            paidField.Chosen.SequenceEqual([fieldEquipment])&&paidField.Invoices is[var fieldInvoice]&&
            fieldInvoice.CardId==fieldEquipment&&fieldInvoice.From==CardLocation.Equipment(0)&&fieldInvoice.To==CardLocation.OutsideGame&&
            Location(field,peerField)==CardLocation.Equipment(1),
            "The real shop equipment pays the exact field gift once, then its paid child interrupts before obtaining the peer's field.");
        CheckDestroyed(field,fieldEquipment,fieldFrame.Id);field=Cold(field,fieldRegistry);Answer(field,_=>true);Play(field);
        Need(Location(field,peerField)==CardLocation.Hand(0)&&Location(field,fieldEquipment)==CardLocation.OutsideGame&&
            E<HappyDingzhouMovementPaidEvent>(field).Where(e=>e.FrameId==fieldFrame.Id).Select(e=>e.Stage)
                .SequenceEqual([HappyDingzhouStage.FieldGiftChildren,HappyDingzhouStage.FieldGainChildren])&&
            !field.GetHumanLegalActions().Any(a=>a.SkillId=="happy:dingzhou"),
            "Cold field return obtains the original field once, retains the destroyed payment, and consumes the original Play budget.");
        CheckDestroyed(field,fieldEquipment,fieldFrame.Id);

        var(hand,handRegistry,handEquipment)=Start();
        var originalHand=V(hand,1).Hand.Select(c=>c.Id).ToArray();
        Need(originalHand.Length>0,"The other real hand is nonempty before the hand exchange.");
        Skill(hand,"happy:dingzhou","exchange",[1]);Reach(hand,p=>Dingzhou(p,"hand"));Answer(hand,c=>Option(c)=="hand");
        Reach(hand,p=>p.SkillPrompt?.SkillId==Watch);
        var handFrame=DingzhouFrame(hand);
        Need(handFrame.HappyDingzhou is{Stage:HappyDingzhouStage.HandGainChildren,Paid:true}gain&&gain.Amount==originalHand.Length&&
            gain.Obtained.SequenceEqual(originalHand)&&originalHand.All(id=>Location(hand,id)==CardLocation.Hand(0)),
            "The original hand is physically acquired once before the exact native gain child.");
        hand=Cold(hand,handRegistry);Answer(hand,_=>true);Reach(hand,p=>Dingzhou(p,"give:"));
        Answer(hand,c=>c.Cards.SequenceEqual([handEquipment]));
        for(var n=1;n<originalHand.Length;n++)
        {Reach(hand,p=>Dingzhou(p,"give:"));Answer(hand,c=>c.Cards.Count==1);}
        Reach(hand,p=>p.SkillPrompt?.SkillId==Watch);
        var returnReceipt=DingzhouFrame(hand).HappyDingzhou!;
        Need(returnReceipt is{Stage:HappyDingzhouStage.HandReturnChildren,Paid:true}&&
            returnReceipt.RequiredReturnCount==originalHand.Length&&returnReceipt.Invoices.Count==originalHand.Length&&
            returnReceipt.Invoices.Count(m=>m.CardId==handEquipment&&m.From==CardLocation.Equipment(0)&&m.To==CardLocation.OutsideGame)==1&&
            returnReceipt.Invoices.Where(m=>m.CardId!=handEquipment).All(m=>m.From==CardLocation.Hand(0)&&m.To==CardLocation.Hand(1)),
            "The frozen return count is paid in one exact native batch; the purchased entity destroys itself while the other real cards reach the recipient.");
        CheckDestroyed(hand,handEquipment,handFrame.Id);hand=Cold(hand,handRegistry);Answer(hand,_=>true);Play(hand);
        Need(V(hand,1).HandCount==originalHand.Length-1&&Location(hand,handEquipment)==CardLocation.OutsideGame&&
            E<HappyDingzhouMovementPaidEvent>(hand).Where(e=>e.FrameId==handFrame.Id).Select(e=>e.Stage)
                .SequenceEqual([HappyDingzhouStage.HandGainChildren,HappyDingzhouStage.HandReturnChildren])&&
            E<HappyDingzhouMovementPaidEvent>(hand).Where(e=>e.FrameId==handFrame.Id).All(e=>e.Count==originalHand.Length)&&
            !hand.GetHumanLegalActions().Any(a=>a.SkillId=="happy:dingzhou"),
            "Cold hand return never reacquires or repays a card and does not invent a delivered copy of the destroyed shop equipment.");
        CheckDestroyed(hand,handEquipment,handFrame.Id);
    }

    private static void CheckDestroyed(GameEngine g,int id,long parent)
    {
        var destroyed=E<WushuangShopEquipmentDestroyedEvent>(g).Where(e=>e.CardId==id).ToArray();
        Need(E<WushuangShopEquipmentCreatedEvent>(g).Count(e=>e.CardId==id)==1&&destroyed is[var fact]&&fact.ParentFrameId==parent&&
            g.CardMovements.Count(m=>m.Sequence==fact.MovementSequence&&m.CardId==id&&m.From==CardLocation.Equipment(0)&&m.To==CardLocation.OutsideGame)==1&&
            !g.CardMovements.Any(m=>m.CardId==id&&m.To==CardLocation.Hand(1)),
            "The paid invoice is backed by its one real shop creation and one exact native destruction movement on the original Dingzhou frame.");
    }
    private static (GameEngine,ContentRegistry,int) Start()
    {
        var registry=ComposedSkillContentRegistry.CreateShowcase(additionalPackages:[new Fixture()]);
        var g=GameEngine.CreateStandard(StandardWushuangContentRegistry.CreateOptions(227) with
        {AiPolicyVersion=AiDecisionPolicy.RebelCoordinationVersion,ConfirmOpeningHand=false,AdvanceAfterHumanCommands=false,UseInteractiveDiscard=false,MaxTurns=12},registry);
        Accept(g,new StartGameCommand());Reach(g,p=>p.Kind==DecisionKind.SelectGeneral);
        Accept(g,new SelectGeneralCommand(0,Human,g.Revision,P(g)!.PromptId));
        Drive(g,()=>Shop(g) is{CanShop:true,Phase:WushuangShopPhase.OpeningTactic});
        Accept(g,new BuyWushuangOfferCommand(0,Shop(g).Offers[0].Id,null,g.Revision));
        Drive(g,()=>Shop(g) is{CanShop:true,Phase:WushuangShopPhase.Shopping});
        var offer=Shop(g).Offers.Single(o=>o.CardOfferId==4186);
        Need(offer.Cost==0,"The retained native seed publishes the real free equipment offer.");
        Accept(g,new BuyWushuangOfferCommand(0,offer.Id,null,g.Revision));
        var created=E<WushuangShopEquipmentCreatedEvent>(g).Single(e=>e.OfferId==offer.Id);
        Need(Location(g,created.CardId)==CardLocation.Equipment(0),"The native purchase really equips its newly registered physical entity.");
        Accept(g,new FinishWushuangShopCommand(0,g.Revision));Play(g);return(g,registry,created.CardId);
    }
    private static ProgramSkillFrame DingzhouFrame(GameEngine g)=>g.ResolutionStack.OfType<ProgramSkillFrame>().Single(f=>f.SkillId=="happy:dingzhou");
    private static PlayerSnapshot V(GameEngine g,int seat=0)=>g.CreateSnapshot(seat).Players.Single(p=>p.Seat==seat);
    private static PendingDecision? P(GameEngine g)=>g.CreateSnapshot(0).PendingDecision;
    private static WushuangSnapshot Shop(GameEngine g)=>g.CreateWushuangSnapshot(0)!;
    private static T[] E<T>(GameEngine g)=>g.Events.Select(e=>e.Payload).OfType<T>().ToArray();
    private static string? Option(PromptChoice c)=>c.Parameters.GetValueOrDefault("option");
    private static bool Dingzhou(PendingDecision p,string option)=>p.Choices.Any(c=>c.Parameters.GetValueOrDefault("program-action")=="happy-dingzhou"&&Option(c)?.StartsWith(option,StringComparison.Ordinal)==true);
    private static CardLocation Location(GameEngine g,int id)=>g.CreateCardZoneDiagnostics().Single(c=>c.CardId==id).Location;
    private static void Skill(GameEngine g,string skill,string activation,int[] targets)=>Accept(g,new UseProgramSkillCommand(0,skill,activation,[],targets,g.Revision,P(g)!.PromptId));
    private static void Answer(GameEngine g,Func<PromptChoice,bool> choose)
    {var p=P(g)!;Accept(g,new AnswerPromptCommand(0,p.PromptId,p.Choices.First(choose).Id,g.Revision));}
    private static void Play(GameEngine g)=>Reach(g,p=>p.Kind==DecisionKind.PlayCard);
    private static void Reach(GameEngine g,Func<PendingDecision,bool> done)=>Drive(g,()=>P(g) is{}p&&done(p));
    private static void Drive(GameEngine g,Func<bool> done)
    {
        for(var step=0;step<220;step++)
        {
            if(done())return;
            if(Shop(g) is{CanShop:true,Phase:WushuangShopPhase.OpeningTactic})
                Accept(g,new BuyWushuangOfferCommand(0,Shop(g).Offers[0].Id,null,g.Revision));
            else if(Shop(g) is{CanShop:true,Phase:WushuangShopPhase.Shopping})Accept(g,new FinishWushuangShopCommand(0,g.Revision));
            else if(P(g) is{Kind:DecisionKind.PlayCard}play)Accept(g,new EndPlayPhaseCommand(0,g.Revision,play.PromptId));
            else if(P(g) is{}p&&p.Kind!=DecisionKind.SelectGeneral)
            {
                var choice=p.Choices.FirstOrDefault(c=>c.Parameters.GetValueOrDefault("program-action")=="skip")??
                    p.Choices.FirstOrDefault(c=>c.Parameters.GetValueOrDefault("response")=="pass")??p.Choices.First();
                Answer(g,c=>c.Id==choice.Id);
            }
            else Accept(g,new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Happy Lu Su shop movement missed its bounded native boundary: "+JsonSerializer.Serialize(new{Prompt=P(g),Shop=Shop(g),Frames=g.ResolutionStack.Select(f=>new{f.Id,f.Kind,f.Step})}));
    }
    private static GameEngine Cold(GameEngine g,ContentRegistry registry)
    {
        var copy=GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())),registry);
        string State(GameEngine e)=>JsonSerializer.Serialize(new{Views=Enumerable.Range(0,2).Select(s=>SnapshotJson.Serialize(e.CreateSnapshot(s))).ToArray(),
            Frames=e.ResolutionStack.Select(f=>JsonSerializer.Serialize(f,f.GetType())).ToArray(),e.CardMovements,Zones=e.CreateCardZoneDiagnostics(),
            Facts=e.Events.Select(f=>JsonSerializer.Serialize(f.Payload,f.Payload.GetType())).ToArray()});
        Need(State(g)==State(copy),"The real paid movement, original source/frame, private choice, destruction evidence and views cold-restore exactly.");return copy;
    }
    private static void Accept(GameEngine g,GameCommand command)
    {var result=g.Submit(command);Need(result.Accepted,result.Error?.Message??"Native Happy Lu Su shop command rejected.");}
    private static void Need(bool condition,string message){if(!condition)throw new InvalidOperationException(message);}

    private sealed class Fixture:IGameContentPackage
    {
        public PackageManifest Manifest{get;}=new("fixture-happy-ls-shop",new(1,0,0));
        public void Register(IContentRegistryBuilder b)
        {
            new StandardWushuangContentPackage().Register(new ModeBuilder(b));
            var catalog=SkillProgramCatalog.Load($$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
              {"id":"{{Driver}}","revision":1,"activations":[{"id":"field","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},"zones":["hand"],"cardCategories":["equipment"],"count":1,"destination":"selectedTargetEquipment"}]}]},
              {"id":"{{Quiet}}","revision":1,"triggers":[{"id":"quiet","window":"afterNormalDraw","subject":"owner","optional":false,"effects":[{"op":"skipTurnPhases","target":"owner","phases":["play"]}]}]},
              {"id":"{{Watch}}","revision":1,"triggers":[
                {"id":"given","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["skill-program.happy-shen-lu-su.FieldGiftChildren","skill-program.happy-shen-lu-su.HandReturnChildren"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]},
                {"id":"gained","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.happy-shen-lu-su.HandGainChildren"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"gain","options":[{"id":"continue"}]}]}]}
            ]}
            """,JsonSerializer.Serialize(new{schemaVersion=SkillProgramCatalog.PresentationSchemaVersion,skills=new[]{Driver,Quiet,Watch}.ToDictionary(id=>id,id=>new{name=id,description="Real native Happy Lu Su shop movement.",
                optionLabels=id==Watch?new Dictionary<string,string>{{"continue","继续"}}:new Dictionary<string,string>()})}));
            foreach(var(id,program)in catalog.Programs)b.AddSkill(new(id,id,"Real native fixture"){Program=program,ProgramPresentation=catalog.Presentations[id]});
            b.AddGeneral(new(Human,"Happy Dingzhou shop owner","","happy:dingzhou","wu",30,[Driver,Watch,"boundary:zhiheng"]));
            b.AddGeneral(new(Peer,"Quiet shop peer","",Quiet,"qun",30));
            b.AddDeck(new("fixture:happy-ls-shop-deck","Real field material",4,0,[])
            {PhysicalCards=Enumerable.Range(0,240).Select(_=>new ContentDeckPhysicalCard("standard:crossbow",Suit.Spade,1)).ToArray()});
        }
    }
    private sealed class ModeBuilder(IContentRegistryBuilder b):IContentRegistryBuilder
    {
        public void AddMode(ContentModeDefinition d)=>b.AddMode(d.Id==WushuangMode.ModeId?d with{DeckId="fixture:happy-ls-shop-deck",GeneralCandidateCount=2,GeneralPoolIds=[Human,Peer]}:d);
        public void AddGeneral(ContentGeneralDefinition d)=>b.AddGeneral(d);public void AddSkill(ContentSkillDefinition d)=>b.AddSkill(d);
        public void AddCard(ContentCardDefinition d)=>b.AddCard(d);public void AddDeck(ContentDeckRecipe d)=>b.AddDeck(d);
        public void AddEquipmentContent(EquipmentContentPackage package)=>b.AddEquipmentContent(package);
        public void AddModeFeatureFactory(IGameModeFeatureFactory factory)=>b.AddModeFeatureFactory(factory);
        public bool HasModeFeatureFactory(string id)=>b.HasModeFeatureFactory(id);
    }
}
