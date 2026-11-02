using CardGame.Core.RuntimeEntries;

namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class HandGainCardSnapshotQueriesRuntime(CoreGameEngineAdvancedLifecycleEntries identities,
    CoreGameEngineProgramCardPoliciesEntries suits, IMatchParticipantStateAccess participants) : IHandGainCardSnapshotQueries
{
    public HandGainCardSnapshot Capture(int recipient, int cardId, int sequence, CardLocation from, bool actualGain)
    {
        var owner = participants.At(recipient);
        var card = identities.GetAdvancedCard(cardId);
        return new(card.Id, card.Kind, identities.AdvancedEffectiveHandKind(owner, card),
            suits.GetProgramEffectiveSuit(owner, card), sequence, from, actualGain);
    }
}
