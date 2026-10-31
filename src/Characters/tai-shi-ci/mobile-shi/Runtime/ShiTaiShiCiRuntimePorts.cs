namespace CardGame.Core;

internal sealed record ShiTaiShiCiPlayer(int Seat, string Name, bool IsAlive, bool IsHuman, int Hp, int MaxHp);
internal sealed record ShiTaiShiCiAttack(long ResolutionId, int TargetSeat, int CardUserSeat, CardKind? EffectiveCardKind);

internal interface IShiTaiShiCiFacts : IProgramSelectionQuery
{
    ShiTaiShiCiPlayer Player(int seat);
    int LivingCount { get; }
    bool ValidSeat(int seat);
    IReadOnlyList<Card> Hand(int seat);
    int TurnNumber { get; }
    int CurrentSeat { get; }
    long MovementSequence { get; }
    Winner Winner { get; }
    bool HasActivationOperation(SkillProgramEffectOp operation);
    ContentSkillDefinition Skill(string skillId);
    IEnumerable<T> History<T>() where T : IGameEvent;
    IReadOnlyList<CardMovementRecord> Movements { get; }
    int LieScanSequence { get; }
    int AttackRange(int seat);
    ShiTaiShiCiAttack Attack(long frameId);
    long? ActiveAttackId { get; }
    IReadOnlyList<int> Targets(int owner, SkillProgramTargetKind kind);
    IReadOnlyList<PromptChoice> HandDiscardChoices(long frameId, int chooser, int owner);
    bool Allies(int owner, int target);
    int KeepValue(int owner, Card card);
    IReadOnlyList<IndexedSkillProgramInstance> ProgramInstances(int owner);
}

internal interface IShiTaiShiCiOperations : IProgramRuntimeOperations
{
    void SetLieScanSequence(int sequence);
    void MarkEffectIneffective(long frameId, int target);
    void PublishState();
    void LogicalDiscard(ProgramSkillFrame frame, int actor, IReadOnlyList<int> ids, CardMoveReason reason);
    SkillProgramStepOutcome BeginVirtualCardUse(ProgramSkillFrame frame, int target, CardKind kind, bool ignoreDistance);
    void DrawCards(long frameId, int recipient, int amount, CardMoveReason reason);
    void Recover(long frameId, int owner, int target, int amount);
    bool DrainChildWindows(long frameId);
    void Finish(ProgramSkillFrame frame, bool completed);
}
