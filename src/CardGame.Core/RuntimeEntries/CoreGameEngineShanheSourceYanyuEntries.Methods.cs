#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceYanyuEntries
{
    internal bool CanOfferShanheSourceYanyu(ProgramTriggerLegalityContext context) => _resolve().CanOfferShanheSourceYanyu(context);
    internal bool IsShanheSourceYanyuPreparation(ProgramSkillFrame frame) => _resolve().IsShanheSourceYanyuPreparation(frame);
    internal SkillProgramStepOutcome ResolveShanheSourceYanyu(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ResolveShanheSourceYanyu(supplied, effect);
    internal bool ResumeShanheSourceYanyu(long id) => _resolve().ResumeShanheSourceYanyu(id);
    internal bool ReturnShanheSourceYanyuMovement(ProgramSkillFrame frame) => _resolve().ReturnShanheSourceYanyuMovement(frame);
    internal void SettleShanheSourceYanyuCard(ProgramSkillFrame supplied) => _resolve().SettleShanheSourceYanyuCard(supplied);
}
