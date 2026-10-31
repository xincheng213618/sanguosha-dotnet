#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramBooleanStateExpirationEntries
{
    internal void ExpireProgramBooleanStateLeases(Func<GameEngine.ProgramBooleanStateLease, bool> expires) => _resolve().ExpireProgramBooleanStateLeases(expires);
    internal void ExpireProgramBooleanStateLeases2(int turnNumber, int turnSeat) => _resolve().ExpireProgramBooleanStateLeases(turnNumber, turnSeat);
    internal void ExpireProgramBooleanStateLeasesAtOwnerTurnStart(int ownerSeat, bool ownerDied = false) => _resolve().ExpireProgramBooleanStateLeasesAtOwnerTurnStart(ownerSeat, ownerDied);
    internal void ScheduleProgramBooleanStateExpiration(ProgramSkillFrame frame, IReadOnlyList<string> stateIds, SkillProgramDamageModifierExpiration expiration = (SkillProgramDamageModifierExpiration)0) => _resolve().ScheduleProgramBooleanStateExpiration(frame, stateIds, expiration);
}
