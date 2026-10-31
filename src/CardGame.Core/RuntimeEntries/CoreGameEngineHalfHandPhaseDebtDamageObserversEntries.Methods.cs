#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtDamageObserversEntries
{
    internal bool HalfHandPaidDamageObserverEdge(int index) => _resolve().HalfHandPaidDamageObserverEdge(index);
    internal bool PaidObserverAttackHpLossDyingMatches(global::CardGame.Core.ProgramSkillFrame program, global::CardGame.Core.DyingFrame dying) => _resolve().PaidObserverAttackHpLossDyingMatches(program, dying);
    internal bool PaidObserverDamageDyingFaceEdge(int index) => _resolve().PaidObserverDamageDyingFaceEdge(index);
    internal bool PaidObserverDamageDyingProgramMatches(global::CardGame.Core.ProgramSkillFrame response, global::CardGame.Core.DyingFrame dying) => _resolve().PaidObserverDamageDyingProgramMatches(response, dying);
    internal bool PaidObserverDamageVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => _resolve().PaidObserverDamageVirtualAlcoholRide(dyingIndex, dying);
}
