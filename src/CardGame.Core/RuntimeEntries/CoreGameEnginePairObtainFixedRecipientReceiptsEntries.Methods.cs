#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairObtainFixedRecipientReceiptsEntries
{
    internal bool IsPairBenefitMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? paid, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsPairBenefitMovement(f, paid, movement);
    internal bool ValidPairObtain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPairObtain(f);
    internal bool ValidShownPairGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidShownPairGift(f);
}
