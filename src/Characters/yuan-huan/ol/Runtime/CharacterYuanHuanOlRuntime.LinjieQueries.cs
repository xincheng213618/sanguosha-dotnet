#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanHuanOlRuntime
{
    internal IReadOnlyList<PromptChoice> LinjieCards(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.YuanHuan!.TargetSeat, f.YuanHuan.TargetSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard);
}
