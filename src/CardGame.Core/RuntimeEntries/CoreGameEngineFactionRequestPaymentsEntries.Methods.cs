#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFactionRequestPaymentsEntries
{
    internal GameEngine.FactionProviderRewardReceipt CaptureFactionProviderReward(NativeOwners.FactionCardRequestHandle pending) => _resolve().CaptureFactionProviderReward(pending);
    internal IReadOnlyList<PromptChoice> FactionRequestCostChoices(NativeOwners.FactionCardRequestHandle pending) => _resolve().FactionRequestCostChoices(pending);
    internal void ResolveFactionRequestCostChoice(PromptChoice selected, bool advanceToHumanBoundary) => _resolve().ResolveFactionRequestCostChoice(selected, advanceToHumanBoundary);
    internal void RewardFactionRequestProvider(GameEngine.FactionProviderRewardReceipt receipt, int providerSeat) => _resolve().RewardFactionRequestProvider(receipt, providerSeat);
    internal bool TryPublishFactionRequestCost(NativeOwners.FactionCardRequestHandle pending) => _resolve().TryPublishFactionRequestCost(pending);
}
