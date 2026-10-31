namespace CardGame.Core;

internal sealed record ShiZhouYuPlayer(int Seat, string Name, bool IsAlive, bool IsHuman, bool IsChained);
internal sealed record ShiZhouYuDamage(long ResolutionId, int TargetSeat, DamageNature Nature);

internal interface IShiZhouYuFacts : IProgramSelectionQuery
{
    ShiZhouYuPlayer Player(int seat);
    IReadOnlyList<ShiZhouYuPlayer> Players { get; }
    bool ValidSeat(int seat);
    IReadOnlyList<Card> Hand(int seat);
    IReadOnlyList<Card> Equipment(int seat);
    int TurnNumber { get; }
    int CurrentSeat { get; }
    int Phase { get; }
    Winner Winner { get; }
    bool HasTriggerOperation(SkillProgramEffectOp operation);
    ContentSkillDefinition Skill(string skillId);
    IEnumerable<T> History<T>() where T : IGameEvent;
    CardConversionSource Source(ProgramSkillFrame frame);
    CardLocation Location(int cardId);
    IReadOnlyList<Card> CardsAt(CardLocation location);
    bool IsRedCard(int owner, Card card);
    bool Allies(int owner, int target);
    int KeepValue(int owner, Card card);
    bool ChoicesEqual(IReadOnlyList<PromptChoice> left, IReadOnlyList<PromptChoice> right);
    ShiZhouYuDamage? Damage { get; }
}

internal interface IShiZhouYuOperations : IProgramRuntimeOperations
{
    void PublishState();
    bool DrainChildWindows(long frameId);
    void Finish(ProgramSkillFrame frame, bool completed);
    void MoveCards(IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason,
        CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified);
    void LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason);
    void RevealHand(long frameId, int owner, string name, IReadOnlyList<int> ids);
    void Damage(ProgramSkillFrame frame, int target, int amount, DamageNature nature);
    void DrawCards(long frameId, int recipient, int amount, CardMoveReason reason);
    void SetChained(ProgramSkillFrame frame, bool chained, int target);
    void Log(string category, string message, int actor, int target);
}
