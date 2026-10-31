#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeSkillInvocationObserversEntries
{
    internal global::CardGame.Core.GameEngine.NativeFactionSkillSuspensionProjection? FindNativeFactionSkillInvocation(long requestOwnerId, int ownerSeat, global::CardGame.Core.FactionResponsePolicySource source, string occurrenceKey, global::CardGame.Core.SkillInvocationReturnKind returnKind) => _resolve().FindNativeFactionSkillInvocation(requestOwnerId, ownerSeat, source, occurrenceKey, returnKind);
    internal bool HasFactionDefenseSkillInvocation(global::CardGame.Core.NativeOwners.FactionDefenseHandle defense) => _resolve().HasFactionDefenseSkillInvocation(defense);
    internal bool HasFactionSlashSkillInvocation(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request) => _resolve().HasFactionSlashSkillInvocation(request);
    internal bool HasNativeFactionSkillInvocation(long attackId) => _resolve().HasNativeFactionSkillInvocation(attackId);
    internal bool NativeFactionSlashInvocationHostMatches(global::CardGame.Core.NativeOwners.FactionCardRequestHandle request, global::CardGame.Core.ResolutionFrame parent) => _resolve().NativeFactionSlashInvocationHostMatches(request, parent);
}
