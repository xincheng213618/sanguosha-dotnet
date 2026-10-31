namespace CardGame.Core;

internal sealed record OlCaoPiPlayer(int Seat, bool IsAlive, bool IsHuman, int MaxHp);

internal interface IOlShenCaoPiFacts : IProgramSelectionQuery
{
    OlCaoPiPlayer Player(int seat);
    long MovementSequence { get; }
    Winner Winner { get; }
    ContentSkillDefinition Skill(string skillId);
    IEnumerable<T> History<T>() where T : IGameEvent;
    IReadOnlyList<CardMovementRecord> Movements { get; }
    IReadOnlyList<SkillGrant> Grants(int seat);
    IReadOnlyList<PublicPersistentPileSource> ReferencedPiles(int owner, string skillId, string instance);
    IReadOnlyList<Card> PileCards(PublicPersistentPileSource source);
    IReadOnlyList<Card> Hand(int seat);
    bool StorageCard(int owner, string skillId, string instance, Card card, CardLocation location);
    bool ExactAwakeningParent(ProgramSkillFrame frame);
    int KeepValue(int owner, Card card);
}

internal interface IOlShenCaoPiOperations : IProgramRuntimeOperations
{
    void MoveCards(IReadOnlyList<Card> cards, CardLocation from, CardLocation to, CardMoveReason reason);
    void MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason, CardMovementDisclosure disclosure);
    PublicPersistentPileSource EnsurePileSource(ProgramSkillFrame frame, int capacity);
    bool DrainChildWindows(long frameId);
    void PublishState();
    void GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds);
    void ObserveSkillRevisions();
    bool RemoveGrant(int owner, string grantId);
}
