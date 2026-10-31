#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheZhanhunDaoEntries
{
    internal bool CanOfferShanheZhanhunDao(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferShanheZhanhunDao(gate);
    internal bool HasActiveShanheZhanhunDao(int ownerSeat, string skillId, string skillInstanceId) => _resolve().HasActiveShanheZhanhunDao(ownerSeat, skillId, skillInstanceId);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShanheZhanhunDao(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveShanheZhanhunDao(supplied);
}
