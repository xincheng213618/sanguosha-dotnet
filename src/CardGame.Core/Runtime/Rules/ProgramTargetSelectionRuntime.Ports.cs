namespace CardGame.Core;

internal interface IProgramTargetResolvers : IProgramSelectionQuery
{
    bool TryResolve(ProgramTargetResolverContext context, out IReadOnlyList<int> targets);
    int MinimumTargets(SkillProgramTargetKind kind);
    string EmptySelectionLabel(SkillProgramTargetKind kind);
}

internal interface IProgramTargetFacts : IProgramSelectionQuery
{
    IReadOnlyList<int> VirtualDuelTargets(CharacterState owner);
    bool IsWithinAttackRange(int sourceSeat, int targetSeat);
    ProgramSkillWindowContext CreateCardActionProgramContext(ProgramCardTriggerWindowFrame window, ProgramCardTriggerCandidate candidate);
    ProgramSkillWindowContext CreateTurnEndingProgramContext(TurnEndingBoundaryFrame window, ProgramTriggerCandidate candidate, SkillProgramTriggerFacts? facts);
    IReadOnlyList<int>? CaoSongSourceTargetSeats(int ownerSeat, SkillProgramTargetKind kind, ProgramSkillWindowContext? context);
    bool IsExactHellPreparationTargetContext(int ownerSeat, ProgramSkillWindowContext? context);
    DyingFrame? ActiveDying { get; }
    IReadOnlyList<int> GetProgramCardUseRoleTargets(int ownerSeat, ProgramSkillWindowContext? context);
    IReadOnlyList<int> GetCurrentArrowBarrageTargets(int ownerSeat, ProgramSkillWindowContext? context);
    int ResolveProgramParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference);
    int GetMarkerSourceCount(CharacterState player, PlayerMarkerKind marker, int sourceSeat);
    bool PublicTeamEnemy(int ownerSeat, CharacterState target);
    bool SamePublicTeam(CharacterState owner, CharacterState target);
    bool IsRuleWounded(CharacterState target);
    bool OtherRevealedPublicTeamAlly(CharacterState owner, CharacterState target);
    bool IsShanheS5JixianTarget(int ownerSeat, CharacterState target);
    bool HasWeaponAbility(CharacterState target, CardKind kind);
    bool CanUseProvidedSlashTarget(CharacterState owner, CharacterState target);
    bool IsSlashRedirectTarget(int ownerSeat, int targetSeat);
    bool IsCardUseForbidden(int ownerSeat, CardKind kind, CardActionType type);
    bool HasTurnCardTargetRestriction(int ownerSeat, SkillProgramCardTargetRestriction restriction);
    bool IsDirectedCardTargetProhibited(int ownerSeat, int targetSeat, CardKind kind);
    bool IsSlashProhibited(CharacterState target);
    bool IsProgramDiscardedDelayedTrickTarget(int ownerSeat, CharacterState target, ProgramSkillWindowContext? context);
    string? GetEffectiveFactionId(CharacterState target);
    bool HasDiscardableHeBy(int ownerSeat, int targetSeat);
    bool IsEquipmentExchangePair(int ownerSeat, int firstSeat, int secondSeat);
    int GetCombatDistance(int ownerSeat, int targetSeat);
    int PlannedNormalDrawCount(ProgramSkillFrame frame);
    bool IsPindianSourceSelectionInstruction(IReadOnlyList<SkillProgramEffect> instructions, int index);
}

internal interface IProgramTargetSelectionProtocols : IProgramRuntimeOperations
{
    void Present(PendingDecision decision, int ownerSeat);
    void Cancel(ProgramSkillFrame frame, string reason);
    SkillProgramStepOutcome SelectNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect);
    void ResolveNamedBoundTargets(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected);
    int[] OrderSupportTargets(ProgramSkillFrame frame, int[] targets);
}

public sealed partial class GameEngine
{
}
