#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGouXiangArmorEntries
{
    internal bool CanOfferGouXiangArmor(ProgramTriggerLegalityContext context) => _resolve().CanOfferGouXiangArmor(context);
    internal CardActionContext CaptureGouXiangActionAppearance(CardActionContext action) => _resolve().CaptureGouXiangActionAppearance(action);
    internal void ObtainGouXiangAttackerHandCard(ProgramSkillFrame supplied) => _resolve().ObtainGouXiangAttackerHandCard(supplied);
    internal bool TryGetGouXiangAttacker(int ownerSeat, string instanceId, ProgramSkillWindowContext? context, scoped out int attackerSeat) => _resolve().TryGetGouXiangAttacker(ownerSeat, instanceId, context, out attackerSeat);
}
