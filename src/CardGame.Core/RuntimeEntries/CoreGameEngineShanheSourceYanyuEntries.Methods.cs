#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceYanyuEntries
{
    internal bool CanOfferShanheSourceYanyu(global::CardGame.Core.ProgramTriggerLegalityContext context) => _resolve().CanOfferShanheSourceYanyu(context);
    internal bool IsShanheSourceYanyuPreparation(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsShanheSourceYanyuPreparation(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShanheSourceYanyu(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ResolveShanheSourceYanyu(supplied, effect);
    internal bool ResumeShanheSourceYanyu(long id) => _resolve().ResumeShanheSourceYanyu(id);
    internal bool ReturnShanheSourceYanyuMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnShanheSourceYanyuMovement(frame);
    internal void SettleShanheSourceYanyuCard(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().SettleShanheSourceYanyuCard(supplied);
}
