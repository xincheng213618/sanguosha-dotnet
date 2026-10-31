#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramPhaseColorRestrictionsEntries
{
    internal void GrantProgramPlayPhaseColorRestriction(ProgramSkillFrame frame, string sourceBind, int targetSeat) => _resolve().GrantProgramPlayPhaseColorRestriction(frame, sourceBind, targetSeat);
    internal bool IsPlayPhasePhysicalCardRestricted(CharacterState player, Card card) => _resolve().IsPlayPhasePhysicalCardRestricted(player, card);
    internal void ObservePlayPhaseColorRestriction(IGameEvent payload) => _resolve().ObservePlayPhaseColorRestriction(payload);
    internal IReadOnlyList<string>? UnfulfilledPlayPhaseColorRestrictions(int ownerSeat) => _resolve().UnfulfilledPlayPhaseColorRestrictions(ownerSeat);
}
