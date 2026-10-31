#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYunMobileShiEntries
{
    internal bool CanOfferMobileShiCuifeng(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileShiCuifeng(gate);
    internal bool IsExactMobileShiCuifengProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsExactMobileShiCuifengProgram(frame);
    internal int MobileShiCuifengDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MobileShiCuifengDamageBonus(attack);
    internal bool MobileShiWuyiAtLeast(global::CardGame.Core.CharacterState owner, int level) => _resolve().MobileShiWuyiAtLeast(owner, level);
    internal bool MobileShiWuyiBasicDistance(global::CardGame.Core.ProgramCardDistanceQuery query) => _resolve().MobileShiWuyiBasicDistance(query);
    internal bool MobileShiWuyiSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery query) => _resolve().MobileShiWuyiSlashDistance(query);
    internal bool QualifyMobileShiWuyiUnlock(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().QualifyMobileShiWuyiUnlock(gate);
    internal void RecordMobileShiWuyiHandBasic(global::CardGame.Core.CardActionAcceptedEvent accepted) => _resolve().RecordMobileShiWuyiHandBasic(accepted);
    internal bool HasMobileShiCuifengCapability { get => _resolve().HasMobileShiCuifengCapability; }
}
