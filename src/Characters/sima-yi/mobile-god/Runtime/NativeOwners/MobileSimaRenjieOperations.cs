#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MobileSimaRenjieOperations(Func<RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries> getCharacterHuaTuoAnniversaryGodRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreNativeNullificationEntries> getCoreNativeNullificationRuntime, Func<RuntimeEntries.CoreNativeNullificationFramesEntries> getCoreNativeNullificationFramesRuntime, Func<RuntimeEntries.CharacterXunYuMobileGodEntries> getCharacterXunYuMobileGodRuntime, Func<RuntimeEntries.CoreNativeBorrowedSwordEntries> getCoreNativeBorrowedSwordRuntime, BorrowedSwordHandleFactory createBorrowedSwordHandle, Func<RuntimeEntries.CoreNativeDamageApplicationEntries> getCoreNativeDamageApplicationRuntime, CardAttackHandleFactory createCardAttackHandle, Func<RuntimeEntries.CoreNativeAttackCompletionEntries> getCoreNativeAttackCompletionRuntime, Func<RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime) : IMobileSimaRenjieOperations
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
