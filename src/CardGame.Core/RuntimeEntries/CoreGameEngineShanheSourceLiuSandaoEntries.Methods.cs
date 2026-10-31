#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceLiuSandaoEntries
{
    internal bool CanOfferSourceHuchen(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSourceHuchen(candidate, trigger, context);
    internal bool CanOfferSourceZhanshen(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSourceZhanshen(candidate, trigger, context);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShanheSourceLiuSandao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().ResolveShanheSourceLiuSandao(supplied, operation);
    internal int SourceHuchenEnemyKills(int ownerSeat) => _resolve().SourceHuchenEnemyKills(ownerSeat);
}
