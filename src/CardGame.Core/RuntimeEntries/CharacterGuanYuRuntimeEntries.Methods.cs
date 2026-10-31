#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuanYuRuntimeEntries
{
    internal bool CanOfferGuanVariant(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferGuanVariant(candidate, trigger, context);
    internal global::CardGame.Core.DemonRoundFocusEvent? DemonFocus(int target) => _resolve().DemonFocus(target);
    internal bool DistancePhysicalSlash210GuanFocusAllowsSlash(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash210GuanFocusAllowsSlash(q);
    internal bool DistancePhysicalSlash220GuanFirstRedDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash220GuanFirstRedDistance(q);
    internal bool DistanceVirtualSlash40GuanFirstRedAvailable(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistanceVirtualSlash40GuanFirstRedAvailable(q);
    internal global::System.Collections.Generic.IReadOnlyList<int> ExclusiveSupportTargets(int recipient, string? skill = default) => _resolve().ExclusiveSupportTargets(recipient, skill);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteGuanYuVariant(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().ExecuteGuanYuVariant(supplied, op);
    internal global::CardGame.Core.FactionResponsePolicySource? GetDemonDodgeRequest(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind) => _resolve().GetDemonDodgeRequest(owner, kind);
    internal bool GuanConversionAvailable(int owner, string skill) => _resolve().GuanConversionAvailable(owner, skill);
    internal bool GuanConversionUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().GuanConversionUnavailable(query);
    internal bool GuanFirstBasicAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind) => _resolve().GuanFirstBasicAvailable(owner, kind);
    internal bool GuanFirstBasicSlashQuotaAfterLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().GuanFirstBasicSlashQuotaAfterLimit(query);
    internal bool GuanFirstRedAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Suit? suit) => _resolve().GuanFirstRedAvailable(owner, suit);
    internal bool GuanFirstRedDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, long? useId = default) => _resolve().GuanFirstRedDistance(owner, card, useId);
    internal bool GuanFocusAllowsSlash(int owner, int target) => _resolve().GuanFocusAllowsSlash(owner, target);
    internal bool GuanFocusDirectedTargetProhibited(global::CardGame.Core.ProgramDirectedTargetQuery query) => _resolve().GuanFocusDirectedTargetProhibited(query);
    internal bool GuanFocusProhibitsSlash(int owner, int target, global::CardGame.Core.CardKind kind) => _resolve().GuanFocusProhibitsSlash(owner, target, kind);
    internal bool GuanFocusSlashQuotaAfterLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().GuanFocusSlashQuotaAfterLimit(query);
    internal int GuanFocusedSlashCount(global::CardGame.Core.GuanPhaseFocusEvent focus) => _resolve().GuanFocusedSlashCount(focus);
    internal bool GuanOpeningUseExists(int owner, bool red) => _resolve().GuanOpeningUseExists(owner, red);
    internal global::CardGame.Core.CardActionContext GuanParentAction(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().GuanParentAction(frame);
    internal global::CardGame.Core.GuanPhaseFocusEvent? GuanPhaseFocus(int owner) => _resolve().GuanPhaseFocus(owner);
    internal bool GuanSparedThisTurn(int owner, int target) => _resolve().GuanSparedThisTurn(owner, target);
    internal void ReturnDemonRoundHolds() => _resolve().ReturnDemonRoundHolds();
    internal global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? WeilinIdentity(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().WeilinIdentity(owner, card);
}
