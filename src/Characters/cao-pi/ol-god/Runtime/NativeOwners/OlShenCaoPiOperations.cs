#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlShenCaoPiOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramSkillChangesEntries> getCoreGameEngineProgramSkillChangesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::System.Collections.Generic.Dictionary<int, long>> getDictionary) : IOlShenCaoPiOperations
{
    public void MoveCards(IReadOnlyList<Card> cards, CardLocation from, CardLocation to, CardMoveReason reason) => getCorePhysicalMovementFacadesRuntime().MoveCards(cards, from, to, reason);
    public void MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason, CardMovementDisclosure disclosure) => getCorePhysicalMovementFacadesRuntime().MoveCard(card, from, to, reason, disclosure: disclosure);
    public PublicPersistentPileSource EnsurePileSource(ProgramSkillFrame frame, int capacity) => getCoreGameEnginePublicPileSourcesRuntime().EnsurePublicPileSource(frame, capacity);
    public bool DrainChildWindows(long frameId) => getCoreGameEngineProgramContinuationsRuntime().TryBeginProgramChildWindows(frameId, PostEventContinuation.Program);
    public void PublishState() => advanceRulesAndPublishState();
    public void GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => getCoreGameEngineProgramSkillChangesRuntime().GrantProgramSkills(frame, skillIds);
    public void ObserveSkillRevisions()
    {
        foreach (var player in matchParticipantsState.ReferenceToPlayers)
            getDictionary().TryAdd(player.Seat, player.SkillGrants.Revision);
    }

    public bool RemoveGrant(int owner, string grantId) => matchParticipantsState.ReferenceToPlayers[owner].SkillGrants.RemoveGrant(grantId);
}
