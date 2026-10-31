#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMarketEntries
{
    internal bool AllowsShanheMarketAcquisition(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsShanheMarketAcquisition(gate);
    internal void ContinueShanheMarketPurchase(long id) => _resolve().ContinueShanheMarketPurchase(id);
    internal void InitializeShanheBattleMarket() => _resolve().InitializeShanheBattleMarket();
    internal bool IsExactShanheMarketExecution(global::CardGame.Core.ProgramSkillFrame program) => _resolve().IsExactShanheMarketExecution(program);
    internal bool IsIssuedShanheMarketCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedShanheMarketCandidate(candidate, context);
    internal bool KeepsPaidShanheMarketExecution(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().KeepsPaidShanheMarketExecution(frame);
    internal void RefreshShanheMarketSuspendedPrompt(global::CardGame.Core.ShanheMarketDiscardQuote? discardQuote) => _resolve().RefreshShanheMarketSuspendedPrompt(discardQuote);
    internal bool ReturnShanheMarketAcquisition(global::CardGame.Core.ProgramSkillFrame program, bool completed) => _resolve().ReturnShanheMarketAcquisition(program, completed);
    internal string? ShanheMarketInputError(int actor) => _resolve().ShanheMarketInputError(actor);
    internal int ShanheMarketRuleBonus(int seat, global::CardGame.Core.SkillRuleQuery query) => _resolve().ShanheMarketRuleBonus(seat, query);
    internal global::CardGame.Core.CommandResult SubmitBuyShanheMarketItem(global::CardGame.Core.BuyShanheMarketItemCommand command) => _resolve().SubmitBuyShanheMarketItem(command);
    internal global::CardGame.Core.CommandResult SubmitRefreshShanheMarket(global::CardGame.Core.RefreshShanheMarketCommand command) => _resolve().SubmitRefreshShanheMarket(command);
    internal bool ValidShanheMarketPurchase(global::CardGame.Core.ShanheMarketPurchaseFrame f) => _resolve().ValidShanheMarketPurchase(f);
    internal string? ValidateLiveShanheMarketPurchase(string choiceId, string? replacement) => _resolve().ValidateLiveShanheMarketPurchase(choiceId, replacement);
}
