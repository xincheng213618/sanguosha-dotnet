#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileSimaRenjieOperations(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries> getCharacterHuaTuoAnniversaryGodRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries> getCoreNativeNullificationRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeNullificationFramesEntries> getCoreNativeNullificationFramesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries> getCharacterXunYuMobileGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeBorrowedSwordEntries> getCoreNativeBorrowedSwordRuntime, global::CardGame.Core.NativeOwners.BorrowedSwordHandleFactory createBorrowedSwordHandle, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries> getCoreNativeDamageApplicationRuntime, global::CardGame.Core.NativeOwners.CardAttackHandleFactory createCardAttackHandle, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries> getCoreNativeAttackCompletionRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime) : IMobileSimaRenjieOperations
{
    public bool BeginInvocation(long frameId, int owner, string skillId, string occurrence, string instance) => getCharacterHuaTuoAnniversaryGodRuntime().TryBeginSkillInvocation(frameId, owner, skillId, occurrence, instance);
    public void SetMarkerSourceCount(int owner, PlayerMarkerKind marker, int source, int count) => matchParticipantsState.ReferenceToPlayers[owner].MarkerSourceCounts[(marker, source)] = count;
    public void SetMarkerCount(int owner, PlayerMarkerKind marker, int count) => matchParticipantsState.ReferenceToPlayers[owner].Markers[marker] = count;
    public void ContinueNullification(NullificationWindowFrame frame) => getCoreNativeNullificationRuntime().ContinueNullificationWindow(getCoreNativeNullificationFramesRuntime().ReplaceNullificationWindowFrame(frame));
    public void ResolveQizheng(long useId) => getCharacterXunYuMobileGodRuntime().ResolveQizhengAfterResponse(useId);
    public void CompleteBorrowedSword(long useId, bool transferWeapon) => getCoreNativeBorrowedSwordRuntime().CompleteBorrowedSwordWithoutSlash(createBorrowedSwordHandle(useId), transferWeapon);
    public bool ApplyDamage(long attackId) => getCoreNativeDamageApplicationRuntime().ApplyAttackDamage(createCardAttackHandle(attackId));
    public void CompleteAttack(long attackId) => getCoreNativeAttackCompletionRuntime().CompleteAttack(createCardAttackHandle(attackId));
    public bool AdvanceObserverSubtree() => getCoreGameEngineObserverSubtreesRuntime().TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
