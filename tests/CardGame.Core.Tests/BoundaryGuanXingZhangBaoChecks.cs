using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Content.Standard;
using static BoundaryLiDianChecks;

// Current ordinary OL boundary Guan Xing & Zhang Bao: docs/content/sources/boundary-guan-xing-zhang-bao-2026-10-04.json.
internal static class BoundaryGuanXingZhangBaoChecks
{
    private const string Skill="boundary:fuhun-current";
    private const string Wusheng="boundary:wusheng";
    private const string Paoxiao="boundary:paoxiao";

    public static void ConvertedSlashResponseFollowsPairColor()
    {
        var(g,r)=Start();
        var hand=OwnHand(g);
        var colors=new[]{Color.Red,Color.Black}.Where(c=>hand.Count(card=>CardColor(card)==c)>=2).ToList();
        Require(colors.Count>0,"The fixture deals the owner two same-color cards.");
        var pairColor=colors[0];
        var pair=hand.Where(card=>CardColor(card)==pairColor).Take(2).Select(card=>card.Id).ToArray();
        var targetDodges=TargetHand(g).Where(card=>card.Kind==CardKind.Dodge).ToList();
        var answerable=targetDodges.Any(card=>CardColor(card)==pairColor);
        var hpBefore=g.CreateSnapshot(1).Players[1].Hp;
        Accept(g,new UseProgramSkillCommand(0,Skill,"two-hand-cards-as-slash",pair,[1],g.Revision,P(g)!.PromptId));
        Settle(g);
        if(answerable)
        {
            Require(g.CreateSnapshot(1).Players[1].Hp==hpBefore,
                "A same-color Dodge still answers the converted slash.");
            var spent=SpentCards(g,1,targetDodges);
            Require(spent.Count==1&&CardColor(spent[0])==pairColor,
                "The answering Dodge shares the converted pair's color.");
        }
        else
        {
            Require(g.CreateSnapshot(1).Players[1].Hp==hpBefore-1,
                "Without a same-color Dodge the converted slash lands despite other Dodges.");
        }
        if(g.CreateSnapshot(1).Players[1].Hp<hpBefore)
            Require(g.CreateSnapshot(0).Players[0].Skills!.Any(s=>s.ContentId==Wusheng)&&
                g.CreateSnapshot(0).Players[0].Skills!.Any(s=>s.ContentId==Paoxiao),
                "The damaging converted slash grants both turn skills.");
        else
            Require(!g.CreateSnapshot(0).Players[0].Skills!.Any(s=>s.ContentId==Wusheng),
                "An answered converted slash grants nothing.");
        Replay(g,r);Settle(g);
    }

    public static void MixedColorConvertedSlashCannotBeDodged()
    {
        var (g,r)=StartResponseScene();
        var hand=OwnHand(g);
        var red=hand.First(card=>CardColor(card)==Color.Red);
        var black=hand.First(card=>CardColor(card)==Color.Black);
        var targetDodges=g.CreateSnapshot(3,true).Players[3].Hand.Where(card=>card.Kind==CardKind.Dodge).ToArray();
        Require(targetDodges.Length>0,"The target has a Dodge that would answer an unrestricted Slash.");
        var hpBefore=g.CreateSnapshot(3).Players[3].Hp;
        Accept(g,new UseProgramSkillCommand(0,Skill,"two-hand-cards-as-slash",[red.Id,black.Id],[3],g.Revision,P(g)!.PromptId));
        Settle(g);
        Require(g.CreateSnapshot(3).Players[3].Hp==hpBefore-1,
            "A red/black pair creates a colorless Slash: no Dodge has a matching color.");
        Require(SpentCards(g,3,targetDodges).Count==0,"The target cannot spend a red or black Dodge on the colorless Slash.");
        Require(g.CreateSnapshot(0).Players[0].Skills!.Any(s=>s.ContentId==Wusheng)&&
            g.CreateSnapshot(0).Players[0].Skills!.Any(s=>s.ContentId==Paoxiao),
            "The unanswerable Slash deals damage and grants both turn skills.");
        Replay(g,r);
    }

    public static void OtherConversionsAndPhysicalSlashesUseCorrectResponseColors()
    {
        foreach(var converted in new[]{false,true})
        {
            var (g,r)=StartResponseScene(wusheng:true,blackDodgeOnly:true);
            var material=OwnHand(g).Single(card=>converted?card.Kind==CardKind.Peach:
                card.Kind==CardKind.Slash&&card.Suit==Suit.Club&&card.Rank==5);
            var action=g.GetHumanLegalActions().Single(a=>a.Kind==LegalActionKind.Slash&&a.CardId==material.Id&&
                a.TargetSeats.SequenceEqual([3])&&(a.ConversionSource?.SkillId==Wusheng)==converted);
            var dodges=g.CreateSnapshot(3,true).Players[3].Hand.Where(card=>card.Kind==CardKind.Dodge).ToArray();
            var hp=g.CreateSnapshot(3).Players[3].Hp;
            Accept(g,new PlayCardCommand(0,material.Id,[3],g.Revision,P(g)!.PromptId,action.PlayedCardKind)
                {ConversionSource=action.ConversionSource});
            Settle(g);
            Require(g.CreateSnapshot(3).Players[3].Hp==hp-(converted?1:0),
                "Fuhun restricts a red Wusheng Slash, while a physical Slash retains its ordinary Dodge response.");
            Require(SpentCards(g,3,dodges).Count==(converted?0:1),
                "A black hand Dodge answers the physical Slash but cannot answer the red Wusheng conversion.");
            Replay(g,r);
        }
    }

    private enum Color { Red, Black }

    private static Color CardColor(CardSnapshot card)=>card.Suit is Suit.Heart or Suit.Diamond?Color.Red:Color.Black;

    private static IReadOnlyList<CardSnapshot> OwnHand(GameEngine g)=>g.CreateSnapshot(0,true).Players[0].Hand!;

    private static IReadOnlyList<CardSnapshot> TargetHand(GameEngine g)=>g.CreateSnapshot(1,true).Players[1].Hand!;

    private static IReadOnlyList<CardSnapshot> SpentCards(GameEngine g,int seat,IEnumerable<CardSnapshot> before)=>before
        .Where(card=>g.CreateSnapshot(seat,true).Players[seat].Hand!.All(now=>now.Id!=card.Id))
        .ToList();


    private static void Accept(GameEngine g,GameCommand command){var result=g.Submit(command);Require(result.Accepted,result.Error?.Message??"Command rejected.");}

    private const string DriverSkill="fixture:gxb-driver";

    private static (GameEngine,ContentRegistry) StartResponseScene(bool wusheng=false,bool blackDodgeOnly=false)
    {
        var r=ContentRegistry.Build(new StandardContentPackage(),new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(),new StandardClassicGeneralPackage(),new Fixture(false));
        var setup=new ShanheBattleSetup(31,1,"boundary-fuhun-response","fixture:gxb",4,4,0,0,0,wusheng?[Wusheng]:[],
            [new(CardKind.Peach,Suit.Heart,4),new(CardKind.Slash,Suit.Club,5)],[],
            [new("fixture:gxb-1",Hp:5){StartingHand=[]},
             new("fixture:gxb-2",Hp:5){StartingHand=blackDodgeOnly?[new(CardKind.Dodge,Suit.Spade,2)]:
                [new(CardKind.Dodge,Suit.Diamond,2),new(CardKind.Dodge,Suit.Spade,2)]}],
            [new("fixture:gxb-3",Hp:5){StartingHand=[]}],new string('c',64));
        var g=GameEngine.CreateStandard(new GameOptions{Seed=setup.BattleSeed,PlayerCount=4,HumanSeat=0,HumanRole=null,
            HumanTeamId=ShanheBattleSetup.PlayerTeamId,ModeId=ShanheBattleSetup.BattleModeId,ShanheBattle=setup,
            AdvanceAfterHumanCommands=false,UseInteractiveSetup=false,UseInteractiveDiscard=false,MaxTurns=4},r);
        Accept(g,new StartGameCommand());Settle(g);return(g,r);
    }

    private static (GameEngine,ContentRegistry) Start(bool inheritance=false)
    {
        ContentRegistry r;
        try
        {
            r=ContentRegistry.Build(new StandardContentPackage(),new StandardActiveSkillExpansionPackage(true),
                new StandardRescueSkillExpansionPackage(),new StandardClassicGeneralPackage(),new Fixture(inheritance));
        }
        catch(Exception diagnostic)
        {
            for(var cause=(Exception?)diagnostic;cause is not null;cause=cause.InnerException)
                Console.WriteLine("DIAG-CHAIN: "+cause.GetType().Name+": "+cause.Message);
            throw;
        }
        var g=GameEngine.CreateStandard(new GameOptions{Seed=31,PlayerCount=4,HumanSeat=0,HumanRole=Role.Lord,
            ModeId="identity:gxb",UseInteractiveSetup=true,AdvanceAfterHumanCommands=false,MaxTurns=12},r);
        Accept(g,new StartGameCommand());Reach(g,p=>p.Kind==DecisionKind.SelectGeneral);
        Accept(g,new SelectGeneralCommand(0,"fixture:gxb",g.Revision,P(g)!.PromptId));
        Settle(g);return(g,r);
    }

    private sealed class Fixture(bool inheritance):IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-gxb", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            b.AddGeneral(new("fixture:gxb","界关兴张苞","boundary_guan_xing_zhang_bao","boundary:fuhun-current","shu",4,
                [DriverSkill]));
            for(var i=1;i<4;i++)b.AddGeneral(new($"fixture:gxb-{i}","其他"+i,"supporter","standard:none","wei",5,null));
            b.AddDeck(new("fixture:gxb-deck","固定",4,1,[]){PhysicalCards=Enumerable.Range(0,96).Select(i=>new ContentDeckPhysicalCard(
                inheritance?"standard:dodge":i%2==0?"standard:slash":"standard:dodge",
                inheritance?Suit.Diamond:i%4==1?Suit.Spade:Suit.Heart,i%13+1)).ToArray()});
            b.AddSkill(new(DriverSkill,"驱动","测试"){Program=Load(DriverSkill,
                "\"activations\":["+Activation("dump","[{\"op\":\"discardOwnedZoneCards\",\"target\":\"owner\",\"zones\":[\"hand\"]}]")+","+
                Activation("strip","["+string.Join(",",Enumerable.Repeat(
                    "{\"op\":\"selectAndMoveOwnedCard\",\"target\":\"owner\",\"chooserRef\":{\"kind\":\"selectedTarget\"},\"cardOwnerRef\":{\"kind\":\"selectedTarget\"},\"zones\":[\"hand\"],\"count\":1,\"destination\":\"discardPile\",\"skipIfNoCards\":true}",4))+"]",1)+"]")});
            b.AddMode(new("identity:gxb","固定",4,4,new Dictionary<string,int>{{nameof(Role.Lord),1},{nameof(Role.Loyalist),1},{nameof(Role.Rebel),2}},
                "fixture:gxb-deck",GeneralCandidateCount:4,
                GeneralPoolIds:["fixture:gxb","fixture:gxb-1","fixture:gxb-2","fixture:gxb-3"]));
            b.AddMode(new(ShanheBattleSetup.BattleModeId,"父魂固定响应",4,4,new Dictionary<string,int>(),"fixture:gxb-deck",
                GeneralPoolIds:["fixture:gxb","fixture:gxb-1","fixture:gxb-2","fixture:gxb-3"],ModeKind:ContentModeKind.Team,
                TeamCounts:new Dictionary<string,int>{[ShanheBattleSetup.PlayerTeamId]=2,[ShanheBattleSetup.EnemyTeamId]=2}));
        }
    }

    private static string Activation(string id,string effects,int targets=0)=>"{\"id\":\""+id+"\",\"usesPerTurn\":null,\"minCards\":0,\"maxCards\":0,\"minTargets\":"+targets+",\"maxTargets\":"+targets+",\"targetKind\":\"anyLiving\",\"effects\":"+effects+"}";

    private static SkillProgram Load(string id,string members)=>SkillProgramCatalog.Load("{\"schemaVersion\":"+SkillProgramCatalog.RulesSchemaVersion+",\"skills\":[{\"id\":\""+id+"\",\"revision\":1,"+members+"}]}","{\"schemaVersion\":3,\"skills\":{\""+id+"\":{\"name\":\"fixture\",\"description\":\"fixture\"}}}").Programs[id];
}
