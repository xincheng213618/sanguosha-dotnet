#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiMobileGodRuntime
{
    internal void MarkMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int responderSeat, CardKind requiredKind) => _mobileSimaRenjieOperations.Get().MarkMobileSimaRenjieResponse(ProjectMobileSimaAttack(attack), responderSeat, requiredKind);
    internal void ClearMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _mobileSimaRenjieOperations.Get().ClearMobileSimaRenjieResponse(ProjectMobileSimaAttack(attack));
    internal bool TryBeginMobileSimaRenjieFailedAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _mobileSimaRenjieOperations.Get().TryBeginMobileSimaRenjieFailedAttack(ProjectMobileSimaAttack(attack));
    internal bool TryBeginMobileSimaRenjieNullification(NullificationWindowFrame supplied, bool explicitPass) => _mobileSimaRenjieOperations.Get().TryBeginMobileSimaRenjieNullification(supplied, explicitPass);
    internal bool TryBeginMobileSimaRenjieBorrowedSword(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => _mobileSimaRenjieOperations.Get().TryBeginMobileSimaRenjieBorrowedSword(ProjectMobileSimaSword(pending));
    internal bool TryBeginMobileSimaRenjieQizheng(CardUseFrame use) => _mobileSimaRenjieOperations.Get().TryBeginMobileSimaRenjieQizheng(use);
    internal bool MobileSimaRenjieStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _mobileSimaRenjieOperations.Get().MobileSimaRenjieStructuralEdge(parent, child);
    internal bool HasMobileSimaRenjieNativeSubtree(long useId) => _mobileSimaRenjieOperations.Get().HasMobileSimaRenjieNativeSubtree(useId);
    bool global::CardGame.Core.IMobileSimaRenjieOperations.BeginInvocation(long frameId, int owner, string skillId, string occurrence, string instance) => _characterHuaTuoAnniversaryGodOperations.TryBeginSkillInvocation(frameId, owner, skillId, occurrence, instance);
    void global::CardGame.Core.IMobileSimaRenjieOperations.SetMarkerSourceCount(int owner, PlayerMarkerKind marker, int source, int count) => _coreMatchParticipantsPlayersAccess._players[owner].MarkerSourceCounts[(marker, source)] = count;
    void global::CardGame.Core.IMobileSimaRenjieOperations.SetMarkerCount(int owner, PlayerMarkerKind marker, int count) => _coreMatchParticipantsPlayersAccess._players[owner].Markers[marker] = count;
    void global::CardGame.Core.IMobileSimaRenjieOperations.ContinueNullification(NullificationWindowFrame frame) => _coreNativeNullificationOperations.ContinueNullificationWindow(_coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(frame));
    void global::CardGame.Core.IMobileSimaRenjieOperations.ResolveQizheng(long useId) => _characterXunYuMobileGodOperations.ResolveQizhengAfterResponse(useId);
    void global::CardGame.Core.IMobileSimaRenjieOperations.CompleteBorrowedSword(long useId, bool transferWeapon) => _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordWithoutSlash(_kernelCoreMainOperations.CreateBorrowedSwordHandle(useId), transferWeapon);
    bool global::CardGame.Core.IMobileSimaRenjieOperations.ApplyDamage(long attackId) => _coreNativeDamageApplicationOperations.ApplyAttackDamage(_kernelCoreMainOperations.CreateCardAttackHandle(attackId));
    void global::CardGame.Core.IMobileSimaRenjieOperations.CompleteAttack(long attackId) => _coreNativeAttackCompletionOperations.CompleteAttack(_kernelCoreMainOperations.CreateCardAttackHandle(attackId));
    bool global::CardGame.Core.IMobileSimaRenjieOperations.AdvanceObserverSubtree() => _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
