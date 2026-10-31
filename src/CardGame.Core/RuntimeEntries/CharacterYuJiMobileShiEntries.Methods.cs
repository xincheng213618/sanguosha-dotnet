#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJiMobileShiEntries
{
    internal bool GainMobileShiFujiSuitCard(int userSeat, int usedCardId, global::CardGame.Core.Suit suit, long actionId) => _resolve().GainMobileShiFujiSuitCard(userSeat, usedCardId, suit, actionId);
    internal bool IsMobileShiFujiEmpoweredActive(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsMobileShiFujiEmpoweredActive(f);
    internal int LastMobileShiFujiEmpowerIndex(global::CardGame.Core.IGameEvent[] history, int ownerSeat) => _resolve().LastMobileShiFujiEmpowerIndex(history, ownerSeat);
    internal bool MobileShiDaozhuanConversionUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().MobileShiDaozhuanConversionUnavailable(query);
    internal int MobileShiFujiSlashDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MobileShiFujiSlashDamageBonus(attack);
    internal void RecordMobileShiDaozhuanUse(global::CardGame.Core.CardActionAcceptedEvent accepted) => _resolve().RecordMobileShiDaozhuanUse(accepted);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMobileShiFujiDodgeDrawProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResolveMobileShiFujiDodgeDrawProgram(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMobileShiFujiGiftProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResolveMobileShiFujiGiftProgram(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMobileShiFujiUseEffectsProgram(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ResolveMobileShiFujiUseEffectsProgram(frame);
}
