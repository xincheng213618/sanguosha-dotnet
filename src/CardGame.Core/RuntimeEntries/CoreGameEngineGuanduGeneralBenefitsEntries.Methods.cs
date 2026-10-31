#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralBenefitsEntries
{
    internal void ConsumeGuanduBeizhan(IGameEvent payload) => _resolve().ConsumeGuanduBeizhan(payload);
    internal void ContinueGuanduBeizhan(ProgramSkillFrame f) => _resolve().ContinueGuanduBeizhan(f);
    internal void ContinueGuanduCangchu(ProgramSkillFrame f) => _resolve().ContinueGuanduCangchu(f);
    internal void ContinueGuanduHengjiang(ProgramSkillFrame f) => _resolve().ContinueGuanduHengjiang(f);
    internal void ContinueGuanduHengjiangReward(ProgramSkillFrame f) => _resolve().ContinueGuanduHengjiangReward(f);
    internal void ContinueGuanduJieliang(ProgramSkillFrame f) => _resolve().ContinueGuanduJieliang(f);
    internal void ContinueGuanduJieliangClaim(ProgramSkillFrame f) => _resolve().ContinueGuanduJieliangClaim(f);
    internal void ContinueGuanduShicai(ProgramSkillFrame f) => _resolve().ContinueGuanduShicai(f);
    internal void ContinueGuanduSushou(ProgramSkillFrame f) => _resolve().ContinueGuanduSushou(f);
    internal void ContinueGuanduXiying(ProgramSkillFrame f) => _resolve().ContinueGuanduXiying(f);
    internal void ContinueGuanduZezhu(ProgramSkillFrame f) => _resolve().ContinueGuanduZezhu(f);
    internal void ContinueGuanduZhongjie(ProgramSkillFrame f) => _resolve().ContinueGuanduZhongjie(f);
    internal GuanduDiscardFactEvent[] GuanduClaimableDiscards(int target) => _resolve().GuanduClaimableDiscards(target);
    internal void GuanduHandLimitPenalty(ProgramSkillFrame f, int target) => _resolve().GuanduHandLimitPenalty(f, target);
}
