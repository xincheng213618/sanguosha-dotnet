#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJiMobileShiEntries
{
    internal bool GainMobileShiFujiSuitCard(int userSeat, int usedCardId, Suit suit, long actionId) => _resolve().GainMobileShiFujiSuitCard(userSeat, usedCardId, suit, actionId);
    internal bool IsMobileShiFujiEmpoweredActive(ProgramSkillFrame f) => _resolve().IsMobileShiFujiEmpoweredActive(f);
    internal int LastMobileShiFujiEmpowerIndex(IGameEvent[] history, int ownerSeat) => _resolve().LastMobileShiFujiEmpowerIndex(history, ownerSeat);
    internal bool MobileShiDaozhuanConversionUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().MobileShiDaozhuanConversionUnavailable(query);
    internal int MobileShiFujiSlashDamageBonus(GameEngine.IDamageAttempt attack) => _resolve().MobileShiFujiSlashDamageBonus(attack);
    internal void RecordMobileShiDaozhuanUse(CardActionAcceptedEvent accepted) => _resolve().RecordMobileShiDaozhuanUse(accepted);
    internal SkillProgramStepOutcome ResolveMobileShiFujiDodgeDrawProgram(ProgramSkillFrame frame) => _resolve().ResolveMobileShiFujiDodgeDrawProgram(frame);
    internal SkillProgramStepOutcome ResolveMobileShiFujiGiftProgram(ProgramSkillFrame frame) => _resolve().ResolveMobileShiFujiGiftProgram(frame);
    internal SkillProgramStepOutcome ResolveMobileShiFujiUseEffectsProgram(ProgramSkillFrame frame) => _resolve().ResolveMobileShiFujiUseEffectsProgram(frame);
}
