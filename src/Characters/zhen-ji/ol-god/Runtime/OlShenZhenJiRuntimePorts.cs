namespace CardGame.Core;

internal sealed record OlShenfuPlayer(int Seat, string Name, bool IsAlive, bool IsHuman, int Hp);

internal interface IOlShenZhenJiFacts : IProgramSelectionQuery
{
    int CurrentSeat { get; }
    int TurnNumber { get; }
    long MovementSequence { get; }
    Winner Winner { get; }
    IReadOnlyList<OlShenfuPlayer> Players { get; }
    OlShenfuPlayer Player(int seat);
    IReadOnlyList<Card> Hand(int seat);
    bool HasSkillInstance(int seat, string skillId, string instance);
    int RelativeOrder(int from, int target);
    ContentSkillDefinition Skill(string skillId);
    IReadOnlyList<PromptChoice> HandDiscardChoices(long frameId, int chooser, int owner);
}

internal interface IOlShenZhenJiOperations : IProgramRuntimeOperations
{
    void PublishState();
    void LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason);
    void BeginDamage(ProgramSkillFrame frame, int target, int amount, DamageNature nature);
    bool DrainChildWindows(long frameId);
}
