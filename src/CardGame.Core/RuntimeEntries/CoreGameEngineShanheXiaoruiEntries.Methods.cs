#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheXiaoruiEntries
{
    internal bool CanOfferShanheXiaorui(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheXiaorui(candidate, trigger, context);
    internal SkillProgramStepOutcome GrantShanheXiaoruiQuota(ProgramSkillFrame supplied) => _resolve().GrantShanheXiaoruiQuota(supplied);
    internal bool IsShanheXiaoruiDamage(int ownerSeat, ProgramSkillWindowContext context) => _resolve().IsShanheXiaoruiDamage(ownerSeat, context);
}
