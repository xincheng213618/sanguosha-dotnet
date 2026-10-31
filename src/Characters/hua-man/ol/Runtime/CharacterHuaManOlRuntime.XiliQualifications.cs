#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaManOlRuntime
{
    internal bool HasRuntimeXili(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ProgramInstances.Any(i => i.Program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.PayXiliActualOtherOwnerDamage)));
}
