#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlShenCaoPiOperations(Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, Func<RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, Func<RuntimeEntries.CoreGameEngineProgramContinuationsEntries> getCoreGameEngineProgramContinuationsRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineProgramSkillChangesEntries> getCoreGameEngineProgramSkillChangesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<Dictionary<int, long>> getDictionary) : IOlShenCaoPiOperations
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
