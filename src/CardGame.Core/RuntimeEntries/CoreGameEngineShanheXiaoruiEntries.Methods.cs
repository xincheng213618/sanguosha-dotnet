#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheXiaoruiEntries
{
    internal bool CanOfferShanheXiaorui(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheXiaorui(candidate, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome GrantShanheXiaoruiQuota(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().GrantShanheXiaoruiQuota(supplied);
    internal bool IsShanheXiaoruiDamage(int ownerSeat, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsShanheXiaoruiDamage(ownerSeat, context);
}
