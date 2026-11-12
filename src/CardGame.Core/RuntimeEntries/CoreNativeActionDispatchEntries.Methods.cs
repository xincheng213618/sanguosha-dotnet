#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeActionDispatchEntries
{
    internal void ExecuteAction(CharacterState actor, LegalAction action, IReadOnlyList<int>? activeSkillCardIds = default, IReadOnlyList<int>? activeSkillTargetSeats = default, ProgramSlashRequestAcceptance? programSlashRequest = null, ProgramSlashRequestIdentity? programSlashRequestIdentity = null, PendingDecision? originalDecision = null) => _resolve().ExecuteAction(actor, action, activeSkillCardIds, activeSkillTargetSeats, programSlashRequest, programSlashRequestIdentity, originalDecision);
}
