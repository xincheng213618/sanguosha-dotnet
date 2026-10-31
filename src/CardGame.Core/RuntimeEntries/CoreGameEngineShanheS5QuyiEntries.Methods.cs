#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5QuyiEntries
{
    internal bool IsShanheS5FuqiResponseProhibited(int sourceSeat, int responderSeat) => _resolve().IsShanheS5FuqiResponseProhibited(sourceSeat, responderSeat);
    internal bool IsShanheS5JiaoziQualified(int ownerSeat) => _resolve().IsShanheS5JiaoziQualified(ownerSeat);
    internal bool ShanheS5FuqiNativeResponseForbidden(ProgramNativeResponseQuery query) => _resolve().ShanheS5FuqiNativeResponseForbidden(query);
    internal bool ShanheS5FuqiNullificationBanned(int responderSeat) => _resolve().ShanheS5FuqiNullificationBanned(responderSeat);
    internal int ShanheS5JiaoziModifierAmount(SkillProgramDamageModifier modifier) => _resolve().ShanheS5JiaoziModifierAmount(modifier);
    internal int ShanheS5JiaoziPublicDamageBonus(GameSnapshot view, int sourceSeat, int targetSeat, bool sourceLess, int paidHandCards = 0) => _resolve().ShanheS5JiaoziPublicDamageBonus(view, sourceSeat, targetSeat, sourceLess, paidHandCards);
}
