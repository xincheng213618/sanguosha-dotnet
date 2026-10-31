#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BlackGiftSelectionQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientConsequencesReceiptsEntries> getCoreGameEngineRecipientConsequencesReceiptsRuntime) : IBlackGiftSelectionQuery
{
    public bool WasIssued(int ownerSeat, string skillId) => getCoreGameEngineRecipientConsequencesReceiptsRuntime().BlackGiftGameIssued(ownerSeat, skillId);
}
