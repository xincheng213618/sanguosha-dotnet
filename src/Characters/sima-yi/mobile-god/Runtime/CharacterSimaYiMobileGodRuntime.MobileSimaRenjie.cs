#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiMobileGodRuntime
{
    internal void MarkMobileSimaRenjieResponse(NativeOwners.CardAttackHandle attack, int responderSeat, CardKind requiredKind) => _mobileSimaRenjieOperations.Get().MarkMobileSimaRenjieResponse(ProjectMobileSimaAttack(attack), responderSeat, requiredKind);
    internal void ClearMobileSimaRenjieResponse(NativeOwners.CardAttackHandle attack) => _mobileSimaRenjieOperations.Get().ClearMobileSimaRenjieResponse(ProjectMobileSimaAttack(attack));
    internal bool TryBeginMobileSimaRenjieFailedAttack(NativeOwners.CardAttackHandle attack) => _mobileSimaRenjieOperations.Get().TryBeginMobileSimaRenjieFailedAttack(ProjectMobileSimaAttack(attack));
    internal bool TryBeginMobileSimaRenjieNullification(NullificationWindowFrame supplied, bool explicitPass) => _mobileSimaRenjieOperations.Get().TryBeginMobileSimaRenjieNullification(supplied, explicitPass);
    internal bool TryBeginMobileSimaRenjieBorrowedSword(NativeOwners.BorrowedSwordHandle pending) => _mobileSimaRenjieOperations.Get().TryBeginMobileSimaRenjieBorrowedSword(ProjectMobileSimaSword(pending));
    internal bool TryBeginMobileSimaRenjieQizheng(CardUseFrame use) => _mobileSimaRenjieOperations.Get().TryBeginMobileSimaRenjieQizheng(use);
    internal bool MobileSimaRenjieStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _mobileSimaRenjieOperations.Get().MobileSimaRenjieStructuralEdge(parent, child);
    bool IMobileSimaRenjieOperations.BeginInvocation(long frameId, int owner, string skillId, string occurrence, string instance) => _characterHuaTuoAnniversaryGodOperations.TryBeginSkillInvocation(frameId, owner, skillId, occurrence, instance);
    void IMobileSimaRenjieOperations.SetMarkerSourceCount(int owner, PlayerMarkerKind marker, int source, int count) => _coreMatchParticipantsPlayersAccess._players[owner].MarkerSourceCounts[(marker, source)] = count;
    void IMobileSimaRenjieOperations.SetMarkerCount(int owner, PlayerMarkerKind marker, int count) => _coreMatchParticipantsPlayersAccess._players[owner].Markers[marker] = count;
    void IMobileSimaRenjieOperations.ContinueNullification(NullificationWindowFrame frame) => _coreNativeNullificationOperations.ContinueNullificationWindow(_coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(frame));
    void IMobileSimaRenjieOperations.ResolveQizheng(long useId) => _characterXunYuMobileGodOperations.ResolveQizhengAfterResponse(useId);
    void IMobileSimaRenjieOperations.CompleteBorrowedSword(long useId, bool transferWeapon) => _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordWithoutSlash(_kernelCoreMainOperations.CreateBorrowedSwordHandle(useId), transferWeapon);
    bool IMobileSimaRenjieOperations.ApplyDamage(long attackId) => _coreNativeDamageApplicationOperations.ApplyAttackDamage(_kernelCoreMainOperations.CreateCardAttackHandle(attackId));
    void IMobileSimaRenjieOperations.CompleteAttack(long attackId) => _coreNativeAttackCompletionOperations.CompleteAttack(_kernelCoreMainOperations.CreateCardAttackHandle(attackId));
    bool IMobileSimaRenjieOperations.AdvanceObserverSubtree() => _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
