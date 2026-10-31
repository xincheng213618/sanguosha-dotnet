namespace CardGame.Core;

internal sealed record MobileSimaPlayer(int Seat, bool IsAlive, bool IsHuman);
internal sealed record MobileSimaAttack(long ResolutionId, int TargetSeat, bool IsChainPropagation, int SuccessfulDodgeResponses);
internal sealed record MobileSimaDuel(long ResolutionId, int SuccessfulSlashResponses);
internal sealed record MobileSimaSword(long ResolutionId, int WeaponOwnerSeat, int SourceSeat, int SlashTargetSeat);

internal interface IMobileSimaCommonFacts : IProgramSelectionQuery
{
    MobileSimaPlayer Player(int seat);
    bool HasSkill(int seat, string skillId);
    IReadOnlyList<SkillGrant> Grants(int seat);
    int MarkerCount(int seat, PlayerMarkerKind marker);
    int TurnNumber { get; }
    Winner Winner { get; }
    ContentSkillDefinition Skill(string skillId);
    IEnumerable<T> History<T>() where T : IGameEvent;
}

internal interface IMobileSimaYiFacts : IMobileSimaCommonFacts
{
    string? Faction(int seat);
}

internal interface IMobileSimaYiOperations : IProgramRuntimeOperations
{
    void GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds);
    void PayMarker(int owner, SkillProgramMarkerCost cost, string skillId, string bindingId);
    void ExtraTurn(ProgramSkillFrame frame, int target);
    bool DrainChildWindows(long frameId);
    void PublishState();
}

internal interface IMobileSimaRenjieFacts : IMobileSimaCommonFacts
{
    bool SkillInstalled(string skillId);
    string ContentHash { get; }
    int RoundNumber { get; }
    MobileSimaDuel? ActiveDuel { get; }
    MobileSimaAttack? ActiveAttack { get; }
    MobileSimaSword? ActiveSword { get; }
    IReadOnlyList<Card> Equipment(int seat);
    bool WenjiNullificationForbidden(NullificationWindowFrame frame, int actor);
    bool FuqiNullificationBanned(int actor);
    bool NearbyResponseProhibited(int source, int actor, CardKind kind, IReadOnlyList<int> targets);
    bool UseUnrespondable(long useId);
    bool YingboUnrespondable(long useId);
    bool WenjiResponseForbidden(long useId, int actor);
    bool LegalBorrowedSwordTarget(int owner, int target);
    int ExtraEffectOrdinal(long useId);
    bool NullificationEffectOwner(NullificationWindowFrame frame);
    int MarkerSourceCount(int owner, PlayerMarkerKind marker, int source);
    bool InvocationStructuralEdge(ResolutionFrame parent, ResolutionFrame child);
    bool InvocationObserverSuffix(long invocationFrameId);
}

internal interface IMobileSimaRenjieOperations : IProgramRuntimeOperations
{
    bool BeginInvocation(long frameId, int owner, string skillId, string occurrence, string instance);
    void SetMarkerSourceCount(int owner, PlayerMarkerKind marker, int source, int count);
    void SetMarkerCount(int owner, PlayerMarkerKind marker, int count);
    void ContinueNullification(NullificationWindowFrame frame);
    void ResolveQizheng(long useId);
    void CompleteBorrowedSword(long useId, bool transferWeapon);
    bool ApplyDamage(long attackId);
    void CompleteAttack(long attackId);
    bool AdvanceObserverSubtree();
}
