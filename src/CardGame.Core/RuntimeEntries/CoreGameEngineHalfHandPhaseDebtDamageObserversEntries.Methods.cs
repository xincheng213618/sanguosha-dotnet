#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtDamageObserversEntries
{
    internal bool HalfHandPaidDamageObserverEdge(int index) => _resolve().HalfHandPaidDamageObserverEdge(index);
    internal bool PaidObserverAttackHpLossDyingMatches(ProgramSkillFrame program, DyingFrame dying) => _resolve().PaidObserverAttackHpLossDyingMatches(program, dying);
    internal bool PaidObserverDamageDyingFaceEdge(int index) => _resolve().PaidObserverDamageDyingFaceEdge(index);
    internal bool PaidObserverDamageDyingProgramMatches(ProgramSkillFrame response, DyingFrame dying) => _resolve().PaidObserverDamageDyingProgramMatches(response, dying);
    internal bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, DyingFrame dying) => _resolve().PaidObserverDamageVirtualAlcoholRide(dyingIndex, dying);
}
