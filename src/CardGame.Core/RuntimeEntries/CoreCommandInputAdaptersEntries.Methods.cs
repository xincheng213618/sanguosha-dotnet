#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreCommandInputAdaptersEntries
{
    internal global::CardGame.Core.CommandResult SubmitAdvance(global::CardGame.Core.GameCommand command, bool singleStep) => _resolve().SubmitAdvance(command, singleStep);
    internal global::CardGame.Core.CommandResult SubmitCixiongDoubleSwordsPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitCixiongDoubleSwordsPromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitDyingPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitDyingPromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitEndPlay(global::CardGame.Core.EndPlayPhaseCommand command) => _resolve().SubmitEndPlay(command);
    internal global::CardGame.Core.CommandResult SubmitFactionDefensePromptAnswer(global::CardGame.Core.PromptChoice selected, string response) => _resolve().SubmitFactionDefensePromptAnswer(selected, response);
    internal global::CardGame.Core.CommandResult SubmitFactionSlashPromptAnswer(global::CardGame.Core.PromptChoice selected, string response) => _resolve().SubmitFactionSlashPromptAnswer(selected, response);
    internal global::CardGame.Core.CommandResult SubmitFireAttackPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitFireAttackPromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitHarvestPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitHarvestPromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitIceSwordPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitIceSwordPromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitNullificationPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitNullificationPromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitPlayCard(global::CardGame.Core.PlayCardCommand command) => _resolve().SubmitPlayCard(command);
    internal global::CardGame.Core.CommandResult SubmitPromptAnswer(int actorSeat, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice) => _resolve().SubmitPromptAnswer(actorSeat, prompt, choice);
    internal global::CardGame.Core.CommandResult SubmitQilinBowPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitQilinBowPromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitQinglongCrescentBladePromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitQinglongCrescentBladePromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitSelectGeneral(global::CardGame.Core.SelectGeneralCommand command) => _resolve().SubmitSelectGeneral(command);
    internal global::CardGame.Core.CommandResult SubmitSkipDiscardPolicyAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitSkipDiscardPolicyAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitSlashPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitSlashPromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitStart(global::CardGame.Core.StartGameCommand command) => _resolve().SubmitStart(command);
    internal global::CardGame.Core.CommandResult SubmitStoneAxePromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitStoneAxePromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitTargetCardPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitTargetCardPromptAnswer(selected);
    internal global::CardGame.Core.CommandResult SubmitUseEquipmentEffect(global::CardGame.Core.UseEquipmentEffectCommand command) => _resolve().SubmitUseEquipmentEffect(command);
    internal global::CardGame.Core.CommandResult SubmitZhuqueFanPromptAnswer(global::CardGame.Core.PromptChoice selected) => _resolve().SubmitZhuqueFanPromptAnswer(selected);
}
