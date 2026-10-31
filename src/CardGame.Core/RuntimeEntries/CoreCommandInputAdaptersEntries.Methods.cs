#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCommandInputAdaptersEntries
{
    internal CommandResult SubmitAdvance(GameCommand command, bool singleStep) => _resolve().SubmitAdvance(command, singleStep);
    internal CommandResult SubmitCixiongDoubleSwordsPromptAnswer(PromptChoice selected) => _resolve().SubmitCixiongDoubleSwordsPromptAnswer(selected);
    internal CommandResult SubmitDyingPromptAnswer(PromptChoice selected) => _resolve().SubmitDyingPromptAnswer(selected);
    internal CommandResult SubmitEndPlay(EndPlayPhaseCommand command) => _resolve().SubmitEndPlay(command);
    internal CommandResult SubmitFactionDefensePromptAnswer(PromptChoice selected, string response) => _resolve().SubmitFactionDefensePromptAnswer(selected, response);
    internal CommandResult SubmitFactionSlashPromptAnswer(PromptChoice selected, string response) => _resolve().SubmitFactionSlashPromptAnswer(selected, response);
    internal CommandResult SubmitFireAttackPromptAnswer(PromptChoice selected) => _resolve().SubmitFireAttackPromptAnswer(selected);
    internal CommandResult SubmitHarvestPromptAnswer(PromptChoice selected) => _resolve().SubmitHarvestPromptAnswer(selected);
    internal CommandResult SubmitIceSwordPromptAnswer(PromptChoice selected) => _resolve().SubmitIceSwordPromptAnswer(selected);
    internal CommandResult SubmitNullificationPromptAnswer(PromptChoice selected) => _resolve().SubmitNullificationPromptAnswer(selected);
    internal CommandResult SubmitPlayCard(PlayCardCommand command) => _resolve().SubmitPlayCard(command);
    internal CommandResult SubmitPromptAnswer(int actorSeat, PromptId prompt, ChoiceId choice) => _resolve().SubmitPromptAnswer(actorSeat, prompt, choice);
    internal CommandResult SubmitQilinBowPromptAnswer(PromptChoice selected) => _resolve().SubmitQilinBowPromptAnswer(selected);
    internal CommandResult SubmitQinglongCrescentBladePromptAnswer(PromptChoice selected) => _resolve().SubmitQinglongCrescentBladePromptAnswer(selected);
    internal CommandResult SubmitSelectGeneral(SelectGeneralCommand command) => _resolve().SubmitSelectGeneral(command);
    internal CommandResult SubmitSkipDiscardPolicyAnswer(PromptChoice selected) => _resolve().SubmitSkipDiscardPolicyAnswer(selected);
    internal CommandResult SubmitSlashPromptAnswer(PromptChoice selected) => _resolve().SubmitSlashPromptAnswer(selected);
    internal CommandResult SubmitStart(StartGameCommand command) => _resolve().SubmitStart(command);
    internal CommandResult SubmitStoneAxePromptAnswer(PromptChoice selected) => _resolve().SubmitStoneAxePromptAnswer(selected);
    internal CommandResult SubmitTargetCardPromptAnswer(PromptChoice selected) => _resolve().SubmitTargetCardPromptAnswer(selected);
    internal CommandResult SubmitUseEquipmentEffect(UseEquipmentEffectCommand command) => _resolve().SubmitUseEquipmentEffect(command);
    internal CommandResult SubmitZhuqueFanPromptAnswer(PromptChoice selected) => _resolve().SubmitZhuqueFanPromptAnswer(selected);
}
