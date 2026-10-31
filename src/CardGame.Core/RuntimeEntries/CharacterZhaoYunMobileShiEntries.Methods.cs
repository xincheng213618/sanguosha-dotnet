#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhaoYunMobileShiEntries
{
    internal bool CanOfferMobileShiCuifeng(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMobileShiCuifeng(gate);
    internal bool IsExactMobileShiCuifengProgram(ProgramSkillFrame frame) => _resolve().IsExactMobileShiCuifengProgram(frame);
    internal int MobileShiCuifengDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().MobileShiCuifengDamageBonus(attack);
    internal bool MobileShiWuyiAtLeast(CharacterState owner, int level) => _resolve().MobileShiWuyiAtLeast(owner, level);
    internal bool MobileShiWuyiBasicDistance(ProgramCardDistanceQuery query) => _resolve().MobileShiWuyiBasicDistance(query);
    internal bool MobileShiWuyiSlashDistance(ProgramSlashDistanceQuery query) => _resolve().MobileShiWuyiSlashDistance(query);
    internal bool QualifyMobileShiWuyiUnlock(ProgramTriggerLegalityContext gate) => _resolve().QualifyMobileShiWuyiUnlock(gate);
    internal void RecordMobileShiWuyiHandBasic(CardActionAcceptedEvent accepted) => _resolve().RecordMobileShiWuyiHandBasic(accepted);
    internal bool HasMobileShiCuifengCapability { get => _resolve().HasMobileShiCuifengCapability; }
}
