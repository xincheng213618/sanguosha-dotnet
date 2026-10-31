#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheMarketEntries
{
    internal bool AllowsShanheMarketAcquisition(ProgramTriggerLegalityContext gate) => _resolve().AllowsShanheMarketAcquisition(gate);
    internal void ContinueShanheMarketPurchase(long id) => _resolve().ContinueShanheMarketPurchase(id);
    internal void InitializeShanheBattleMarket() => _resolve().InitializeShanheBattleMarket();
    internal bool IsExactShanheMarketExecution(ProgramSkillFrame program) => _resolve().IsExactShanheMarketExecution(program);
    internal bool IsIssuedShanheMarketCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedShanheMarketCandidate(candidate, context);
    internal bool KeepsPaidShanheMarketExecution(ProgramSkillFrame frame) => _resolve().KeepsPaidShanheMarketExecution(frame);
    internal void RefreshShanheMarketSuspendedPrompt(ShanheMarketDiscardQuote? discardQuote) => _resolve().RefreshShanheMarketSuspendedPrompt(discardQuote);
    internal bool ReturnShanheMarketAcquisition(ProgramSkillFrame program, bool completed) => _resolve().ReturnShanheMarketAcquisition(program, completed);
    internal string? ShanheMarketInputError(int actor) => _resolve().ShanheMarketInputError(actor);
    internal int ShanheMarketRuleBonus(int seat, SkillRuleQuery query) => _resolve().ShanheMarketRuleBonus(seat, query);
    internal CommandResult SubmitBuyShanheMarketItem(BuyShanheMarketItemCommand command) => _resolve().SubmitBuyShanheMarketItem(command);
    internal CommandResult SubmitRefreshShanheMarket(RefreshShanheMarketCommand command) => _resolve().SubmitRefreshShanheMarket(command);
    internal bool ValidShanheMarketPurchase(ShanheMarketPurchaseFrame f) => _resolve().ValidShanheMarketPurchase(f);
    internal string? ValidateLiveShanheMarketPurchase(string choiceId, string? replacement) => _resolve().ValidateLiveShanheMarketPurchase(choiceId, replacement);
}
