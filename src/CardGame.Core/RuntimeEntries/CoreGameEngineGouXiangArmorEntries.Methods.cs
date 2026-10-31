#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGouXiangArmorEntries
{
    internal bool CanOfferGouXiangArmor(global::CardGame.Core.ProgramTriggerLegalityContext context) => _resolve().CanOfferGouXiangArmor(context);
    internal global::CardGame.Core.CardActionContext CaptureGouXiangActionAppearance(global::CardGame.Core.CardActionContext action) => _resolve().CaptureGouXiangActionAppearance(action);
    internal void ObtainGouXiangAttackerHandCard(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ObtainGouXiangAttackerHandCard(supplied);
    internal bool TryGetGouXiangAttacker(int ownerSeat, string instanceId, global::CardGame.Core.ProgramSkillWindowContext? context, scoped out int attackerSeat) => _resolve().TryGetGouXiangAttacker(ownerSeat, instanceId, context, out attackerSeat);
}
