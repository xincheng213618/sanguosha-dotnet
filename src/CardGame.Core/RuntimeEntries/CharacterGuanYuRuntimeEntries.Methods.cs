#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanYuRuntimeEntries
{
    internal bool CanOfferGuanVariant(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferGuanVariant(candidate, trigger, context);
    internal DemonRoundFocusEvent? DemonFocus(int target) => _resolve().DemonFocus(target);
    internal bool DistancePhysicalSlash210GuanFocusAllowsSlash(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash210GuanFocusAllowsSlash(q);
    internal bool DistancePhysicalSlash220GuanFirstRedDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash220GuanFirstRedDistance(q);
    internal bool DistanceVirtualSlash40GuanFirstRedAvailable(ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash40GuanFirstRedAvailable(q);
    internal IReadOnlyList<int> ExclusiveSupportTargets(int recipient, string? skill = default) => _resolve().ExclusiveSupportTargets(recipient, skill);
    internal SkillProgramStepOutcome ExecuteGuanYuVariant(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().ExecuteGuanYuVariant(supplied, op);
    internal FactionResponsePolicySource? GetDemonDodgeRequest(CharacterState owner, CardKind kind) => _resolve().GetDemonDodgeRequest(owner, kind);
    internal bool GuanConversionAvailable(int owner, string skill) => _resolve().GuanConversionAvailable(owner, skill);
    internal bool GuanConversionUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().GuanConversionUnavailable(query);
    internal bool GuanFirstBasicAvailable(CharacterState owner, CardKind kind) => _resolve().GuanFirstBasicAvailable(owner, kind);
    internal bool GuanFirstBasicSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().GuanFirstBasicSlashQuotaAfterLimit(query);
    internal bool GuanFirstRedAvailable(CharacterState owner, Suit? suit) => _resolve().GuanFirstRedAvailable(owner, suit);
    internal bool GuanFirstRedDistance(CharacterState owner, Card card, long? useId = default) => _resolve().GuanFirstRedDistance(owner, card, useId);
    internal bool GuanFocusAllowsSlash(int owner, int target) => _resolve().GuanFocusAllowsSlash(owner, target);
    internal bool GuanFocusDirectedTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().GuanFocusDirectedTargetProhibited(query);
    internal bool GuanFocusProhibitsSlash(int owner, int target, CardKind kind) => _resolve().GuanFocusProhibitsSlash(owner, target, kind);
    internal bool GuanFocusSlashQuotaAfterLimit(ProgramSlashQuotaQuery query) => _resolve().GuanFocusSlashQuotaAfterLimit(query);
    internal int GuanFocusedSlashCount(GuanPhaseFocusEvent focus) => _resolve().GuanFocusedSlashCount(focus);
    internal bool GuanOpeningUseExists(int owner, bool red) => _resolve().GuanOpeningUseExists(owner, red);
    internal CardActionContext GuanParentAction(ProgramSkillFrame frame) => _resolve().GuanParentAction(frame);
    internal GuanPhaseFocusEvent? GuanPhaseFocus(int owner) => _resolve().GuanPhaseFocus(owner);
    internal bool GuanSparedThisTurn(int owner, int target) => _resolve().GuanSparedThisTurn(owner, target);
    internal void ReturnDemonRoundHolds() => _resolve().ReturnDemonRoundHolds();
    internal GameEngine.ProgramCardIdentityMatch? WeilinIdentity(CharacterState owner, Card card) => _resolve().WeilinIdentity(owner, card);
}
