#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramParticipantPhaseOperationsEntries
{
    internal bool CanActivateTargetPhaseLedger(int owner, string skill, string usage, int target) => _resolve().CanActivateTargetPhaseLedger(owner, skill, usage, target);
    internal void CollectFullDiscardPhaseSuit(Card card, CardMovementRecord move) => _resolve().CollectFullDiscardPhaseSuit(card, move);
    internal void ConsumeProgramTargetPhaseLedger(ProgramSkillFrame frame, string usage) => _resolve().ConsumeProgramTargetPhaseLedger(frame, usage);
    internal void GrantProgramTurnBoundSuitUseProhibition(ProgramSkillFrame frame, string bind) => _resolve().GrantProgramTurnBoundSuitUseProhibition(frame, bind);
    internal bool HasActuallyUsableHandCard(CharacterState owner) => _resolve().HasActuallyUsableHandCard(owner);
    internal bool IsTurnPhysicalUseForbidden(int seat, IReadOnlyList<int> ids) => _resolve().IsTurnPhysicalUseForbidden(seat, ids);
    internal bool IsTurnPhysicalUseForbiddenByExistingPolicies(int seat, IReadOnlyList<int> ids) => _resolve().IsTurnPhysicalUseForbiddenByExistingPolicies(seat, ids);
    internal bool IsTurnSuitUseForbidden(int seat, Suit suit) => _resolve().IsTurnSuitUseForbidden(seat, suit);
    internal bool FullDiscardPhaseSuitsAllDistinct { get => _resolve().FullDiscardPhaseSuitsAllDistinct; }
}
