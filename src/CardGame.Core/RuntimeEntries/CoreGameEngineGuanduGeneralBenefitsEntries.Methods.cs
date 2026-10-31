#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralBenefitsEntries
{
    internal void ConsumeGuanduBeizhan(global::CardGame.Core.IGameEvent payload) => _resolve().ConsumeGuanduBeizhan(payload);
    internal void ContinueGuanduBeizhan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduBeizhan(f);
    internal void ContinueGuanduCangchu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduCangchu(f);
    internal void ContinueGuanduHengjiang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduHengjiang(f);
    internal void ContinueGuanduHengjiangReward(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduHengjiangReward(f);
    internal void ContinueGuanduJieliang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduJieliang(f);
    internal void ContinueGuanduJieliangClaim(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduJieliangClaim(f);
    internal void ContinueGuanduShicai(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduShicai(f);
    internal void ContinueGuanduSushou(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduSushou(f);
    internal void ContinueGuanduXiying(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduXiying(f);
    internal void ContinueGuanduZezhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduZezhu(f);
    internal void ContinueGuanduZhongjie(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueGuanduZhongjie(f);
    internal global::CardGame.Core.GuanduDiscardFactEvent[] GuanduClaimableDiscards(int target) => _resolve().GuanduClaimableDiscards(target);
    internal void GuanduHandLimitPenalty(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().GuanduHandLimitPenalty(f, target);
}
