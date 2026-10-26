using System.Text.Json;
using CardGame.Core;

internal static class WushuangAiBuildBattleChecks
{
    private static readonly WushuangBuildProfile None = new(0,0,0,false,0);

    public static void CurrentInjuryBuildChangesSafeResponseAndKeepsDangerousAndLegacyDefense()
    {
        var plain = View(None, hp:3, enemyHp:3, enemyHand:0);
        var gained = Build(plain,new(2,0,0,false,0));
        var one = PublicDamagePrediction.Calculate(3,0,1,maxHp:3);
        Need(Dodge(plain,11,one).UseDodge && !Dodge(gained,11,one).UseDodge && !Dodge(gained,11,one).UseBagua &&
            Dodge(Build(gained,None),11,one).UseDodge && plain.Players[0].GeneralId == gained.Players[0].GeneralId,
            "The same general switches from defense to safe injury management after receiving the current Draw2-per-damage build, then returns to defense when that build is removed.");
        var oldPlain = Dodge(plain,10,one); var oldGained = Dodge(gained,10,one);
        Need(oldPlain.UseDodge && oldGained.UseDodge && JsonSerializer.Serialize(oldPlain.Thought)==JsonSerializer.Serialize(oldGained.Thought) &&
            Dodge(gained with{Wushuang=null},11,one).UseDodge,
            "Policy10 retains its exact response candidates/thought and non-Wushuang policy11 never uses the optional build.");
        foreach(var unknownOrHarmful in new PublicDamagePrediction?[]{null,
            PublicDamagePrediction.Calculate(3,0,3,maxHp:3),
            PublicDamagePrediction.Calculate(3,0,1,PublicDamagePaymentKind.HpLoss,3),
            PublicDamagePrediction.Calculate(3,0,1,PublicDamagePaymentKind.MaximumHpLoss,3),
            PublicDamagePrediction.Calculate(3,0,0,maxHp:3)})
            Need(Dodge(gained,11,unknownOrHarmful).UseDodge,"Unknown, lethal, direct HP loss, maximum-HP loss and zero damage cannot earn injury-draw credit.");
        var low = gained with{Players=gained.Players.Select(p=>p.Seat==0?p with{Hp=2}:p).ToArray()};
        Need(Dodge(low,11,PublicDamagePrediction.Calculate(2,0,1,maxHp:3)).UseDodge &&
            Dodge(gained with{Wushuang=gained.Wushuang! with{RemainingTurns=4}},11,one).UseDodge &&
            Dodge(gained with{Wushuang=gained.Wushuang! with{RemainingMilliseconds=60_000}},11,one).UseDodge,
            "Two-HP danger and either near-limit horizon preserve defense, including the HP tie that would otherwise become a loss.");
        var repeated = gained with{Players=gained.Players.Select(p=>p.Seat==1?p with{Equipment=[Card(30,CardKind.Crossbow)]}:p).ToArray()};
        Need(Dodge(repeated,11,one).UseDodge,"A visible repeat-Slash weapon blocks the fragile two-HP injury plan.");
    }

    public static void CurrentSlashBuildUsesSupportedBurstWithoutInventingWineMaterialsOrIgnoringRetaliation()
    {
        var plain = View(None,hp:4,enemyHp:2,enemyHand:3) with{CurrentSeat=0};
        var burst = Build(plain,new(0,0,1,true,0));
        var actions = new[]{new LegalAction(LegalActionKind.Slash,1,1,"slash"),
            new LegalAction(LegalActionKind.Alcohol,2,null,"wine"),new LegalAction(LegalActionKind.EndPlay,null,null,"end")};
        PublicDamagePrediction Predict(LegalAction _,int __) => Payment(2,2);
        var before=Choose(plain,11,actions,Predict);var acquired=Choose(burst,11,actions,Predict);var removed=Choose(Build(burst,None),11,actions,Predict);
        Need(before.Action.Kind==LegalActionKind.Alcohol&&acquired.Action.Kind==LegalActionKind.Slash&&removed.Action.Kind==LegalActionKind.Alcohol&&
            acquired.Thought.Candidates.Any(c=>c.Action.Kind==LegalActionKind.Slash&&c.Reason.Contains("当前构筑",StringComparison.Ordinal)),
            "An acquired burst/sealing build uses the actual already-finishing Slash promptly; removing it restores the old preparation ordering without changing the general identity.");
        Need(JsonSerializer.Serialize(Choose(plain,10,actions,Predict).Thought)==JsonSerializer.Serialize(Choose(burst,10,actions,Predict).Thought),
            "Historical policy10 ignores the optional build with identical seeded action scores and ordering.");
        Need(Choose(burst,11,[actions[1],actions[2]],Predict).Action.Kind==LegalActionKind.EndPlay,
            "A build cannot fabricate an independent legal Slash or remaining turn budget to justify drinking wine.");
        Need(Choose(burst,11,actions,(_,_)=>Payment(2,0)).Action.Kind==LegalActionKind.EndPlay,
            "A potential burst trait cannot override the actual fully prevented public payment.");
        var risky=burst with{Players=burst.Players.Select(p=>p.Seat==1?p with{Hp=4}:p).ToArray()};
        var reflected=PublicDamageSequencePrediction.Calculate(4,4,0,[new(PublicHealthEffectKind.Damage,4)]);
        Need(Choose(risky,11,actions,(_,_)=>Payment(4,1) with{Wushuang=new(PublicDamagePrediction.Calculate(4,0,1,maxHp:4),reflected,false,false,0,1)}).Action.Kind==LegalActionKind.EndPlay,
            "The positive build bonus never overrides a known lethal retaliation while the target survives.");
    }

    private static (bool UseDodge,bool UseBagua,AiThoughtRecord Thought) Dodge(GameSnapshot view,int policy,PublicDamagePrediction? prediction)=>
        new SimpleAiBrain(0,71,policy).ChooseDodgeResponse(view,1,1,false,true,incomingPrediction:prediction,legalBaguaAvailable:false);
    private static (LegalAction Action,AiThoughtRecord Thought) Choose(GameSnapshot view,int policy,IReadOnlyList<LegalAction> actions,Func<LegalAction,int,PublicDamagePrediction> prediction)=>
        new SimpleAiBrain(0,71,policy).ChoosePlay(view,actions,1,prediction);
    private static PublicDamagePrediction Payment(int hp,int damage)
    {var p=PublicDamagePrediction.Calculate(hp,0,damage,maxHp:4);return p with{Wushuang=new(p,null,false,false,0,1)};}
    private static GameSnapshot Build(GameSnapshot view,WushuangBuildProfile build)=>view with{Players=view.Players.Select(p=>p.Seat==0?p with{WushuangBuild=build}:p).ToArray()};
    private static CardSnapshot Card(int id,CardKind kind)=>new(id,kind,Suit.Spade,7,"visible","7");
    private static GameSnapshot View(WushuangBuildProfile build,int hp,int enemyHp,int enemyHand)=>new(null,0,EngineStatus.Running,Winner.None,3,1,TurnPhase.Play,80,0,
        [new(0,"owner",false,null,false,"fixture:unchanged-general","","",hp,hp,true,3,[Card(1,CardKind.Slash),Card(2,CardKind.Alcohol),Card(3,CardKind.Dodge)]){TeamId="blue",WushuangBuild=build},
         new(1,"opponent",false,null,false,"fixture:other","","",enemyHp,4,true,enemyHand,[]){TeamId="red",WushuangBuild=None}],null)
        {ModeKind=ContentModeKind.Team,Wushuang=new(0,1,0,0,0,[],[],false,1,0,WushuangMode.TimeLimitMilliseconds,1){RemainingTurns=12}};
    private static void Need(bool condition,string message){if(!condition)throw new InvalidOperationException(message);}
}
