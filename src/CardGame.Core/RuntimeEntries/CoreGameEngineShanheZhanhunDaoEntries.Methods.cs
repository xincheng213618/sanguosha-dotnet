#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheZhanhunDaoEntries
{
    internal bool CanOfferShanheZhanhunDao(ProgramTriggerLegalityContext gate) => _resolve().CanOfferShanheZhanhunDao(gate);
    internal bool HasActiveShanheZhanhunDao(int ownerSeat, string skillId, string skillInstanceId) => _resolve().HasActiveShanheZhanhunDao(ownerSeat, skillId, skillInstanceId);
    internal SkillProgramStepOutcome ResolveShanheZhanhunDao(ProgramSkillFrame supplied) => _resolve().ResolveShanheZhanhunDao(supplied);
}
