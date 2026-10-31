#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramPhaseColorRestrictionsEntries
{
    internal void GrantProgramPlayPhaseColorRestriction(global::CardGame.Core.ProgramSkillFrame frame, string sourceBind, int targetSeat) => _resolve().GrantProgramPlayPhaseColorRestriction(frame, sourceBind, targetSeat);
    internal bool IsPlayPhasePhysicalCardRestricted(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card) => _resolve().IsPlayPhasePhysicalCardRestricted(player, card);
    internal void ObservePlayPhaseColorRestriction(global::CardGame.Core.IGameEvent payload) => _resolve().ObservePlayPhaseColorRestriction(payload);
    internal global::System.Collections.Generic.IReadOnlyList<string>? UnfulfilledPlayPhaseColorRestrictions(int ownerSeat) => _resolve().UnfulfilledPlayPhaseColorRestrictions(ownerSeat);
}
