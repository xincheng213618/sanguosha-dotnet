#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageTriggerCompletion(Func<RuntimeEntries.CoreNativeCardClassificationsEntries> getCoreNativeCardClassificationsRuntime, Func<RuntimeEntries.CoreNativeDyingEntries> getCoreNativeDyingRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreNativeDamageWindowsEntries> getCoreNativeDamageWindowsRuntime, Func<RuntimeEntries.CoreNativeDamageCompletionEntries> getCoreNativeDamageCompletionRuntime, GameEngine.IDamageAttempt attack, CharacterState target) : IDamageTriggerCompletion
{
    public int Hp => target.Hp;
    public bool IsAlive => target.IsAlive;
    public int SourceSeat => attack.SourceSeat;
    public int TargetSeat => attack.TargetSeat;
    public int DamageAmount => attack.DamageAmount;
    public DamageNature Nature => getCoreNativeCardClassificationsRuntime().GetDamageNature(attack);
    public bool IsSourceLess => attack.IsSourceLess;

    public bool HasResolvedDying(long damageFrameId) => attack.HasResolvedDyingForDamage(damageFrameId);
    public void BeginDying(long damageFrameId) => getCoreNativeDyingRuntime().BeginDying(attack, damageFrameId, target, matchParticipantsState.ReferenceToPlayers[attack.SourceSeat]);
    public bool TryBeginAfterDamage(long damageFrameId) => getCoreNativeDamageWindowsRuntime().TryBeginDamageTriggerWindow(attack, damageFrameId, SkillProgramTriggerWindow.AfterDamageApplied);
    public void CompleteAttack() => getCoreNativeDamageCompletionRuntime().CompleteDamageAttack(attack);
}
