#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFactionRequestPaymentsEntries
{
    internal global::CardGame.Core.GameEngine.FactionProviderRewardReceipt CaptureFactionProviderReward(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending) => _resolve().CaptureFactionProviderReward(pending);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FactionRequestCostChoices(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending) => _resolve().FactionRequestCostChoices(pending);
    internal void ResolveFactionRequestCostChoice(global::CardGame.Core.PromptChoice selected, bool advanceToHumanBoundary) => _resolve().ResolveFactionRequestCostChoice(selected, advanceToHumanBoundary);
    internal void RewardFactionRequestProvider(global::CardGame.Core.GameEngine.FactionProviderRewardReceipt receipt, int providerSeat) => _resolve().RewardFactionRequestProvider(receipt, providerSeat);
    internal bool TryPublishFactionRequestCost(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending) => _resolve().TryPublishFactionRequestCost(pending);
}
