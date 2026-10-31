#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramParticipantPhaseOperationsEntries
{
    internal bool CanActivateTargetPhaseLedger(int owner, string skill, string usage, int target) => _resolve().CanActivateTargetPhaseLedger(owner, skill, usage, target);
    internal void CollectFullDiscardPhaseSuit(global::CardGame.Core.Card card, global::CardGame.Core.CardMovementRecord move) => _resolve().CollectFullDiscardPhaseSuit(card, move);
    internal void ConsumeProgramTargetPhaseLedger(global::CardGame.Core.ProgramSkillFrame frame, string usage) => _resolve().ConsumeProgramTargetPhaseLedger(frame, usage);
    internal void GrantProgramTurnBoundSuitUseProhibition(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().GrantProgramTurnBoundSuitUseProhibition(frame, bind);
    internal bool HasActuallyUsableHandCard(global::CardGame.Core.CharacterState owner) => _resolve().HasActuallyUsableHandCard(owner);
    internal bool IsTurnPhysicalUseForbidden(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().IsTurnPhysicalUseForbidden(seat, ids);
    internal bool IsTurnPhysicalUseForbiddenByExistingPolicies(int seat, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().IsTurnPhysicalUseForbiddenByExistingPolicies(seat, ids);
    internal bool IsTurnSuitUseForbidden(int seat, global::CardGame.Core.Suit suit) => _resolve().IsTurnSuitUseForbidden(seat, suit);
    internal bool FullDiscardPhaseSuitsAllDistinct { get => _resolve().FullDiscardPhaseSuitsAllDistinct; }
}
