#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramBooleanStateExpirationEntries
{
    internal void ExpireProgramBooleanStateLeases(global::System.Func<global::CardGame.Core.GameEngine.ProgramBooleanStateLease, bool> expires) => _resolve().ExpireProgramBooleanStateLeases(expires);
    internal void ExpireProgramBooleanStateLeases2(int turnNumber, int turnSeat) => _resolve().ExpireProgramBooleanStateLeases(turnNumber, turnSeat);
    internal void ExpireProgramBooleanStateLeasesAtOwnerTurnStart(int ownerSeat, bool ownerDied = false) => _resolve().ExpireProgramBooleanStateLeasesAtOwnerTurnStart(ownerSeat, ownerDied);
    internal void ScheduleProgramBooleanStateExpiration(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> stateIds, global::CardGame.Core.SkillProgramDamageModifierExpiration expiration = (global::CardGame.Core.SkillProgramDamageModifierExpiration)0) => _resolve().ScheduleProgramBooleanStateExpiration(frame, stateIds, expiration);
}
