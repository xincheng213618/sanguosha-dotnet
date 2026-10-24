using CardGame.Core;
namespace CardGame.Wpf.ViewModels;
public sealed partial class MainViewModel
{
    private HumanSkillViewModel ApplyBoundaryYanYanProgress(HumanSkillViewModel skill)
    {
        if(skill.ContentId!="boundary:juzhan-mobile"||_snapshot?.Players.SingleOrDefault(p=>p.Seat==_snapshot.HumanSeat)?.SkillRuntimeStates?
            .SingleOrDefault(s=>s.SkillId==skill.ContentId)is not{}runtime)return skill;
        var states=runtime.PublicRuleStates??[];var count=states.FirstOrDefault(s=>s.StateId=="juzhan-state-count")?.Value??0;
        var bonus=states.FirstOrDefault(s=>s.StateId=="juzhan-permanent-slash")?.Value??0;var yin=runtime.Polarity==SkillPolarity.Yin;
        return skill with{ProgressText=$"{(yin?"阴":"阳")} {count}次 · 杀+{bonus}",
            StateText=$"当前为{(yin?"阴：按每次使用杀指定目标计数":"阳：按每名实际杀目标计数")}，已累计 {count} 次。\n"+
                "自己满足对应身份，或本状态恰好第3次时可发动；发动后转换并重新计数。\n"+
                $"两项条件同时满足时乘势；持久出杀次数 +{bonus}，此杀结算后取得其实体牌。"};
    }
}
