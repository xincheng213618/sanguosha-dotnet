#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceLiuSandaoEntries
{
    internal bool CanOfferSourceHuchen(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferSourceHuchen(candidate, trigger, context);
    internal bool CanOfferSourceZhanshen(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferSourceZhanshen(candidate, trigger, context);
    internal SkillProgramStepOutcome ResolveShanheSourceLiuSandao(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().ResolveShanheSourceLiuSandao(supplied, operation);
    internal int SourceHuchenEnemyKills(int ownerSeat) => _resolve().SourceHuchenEnemyKills(ownerSeat);
}
