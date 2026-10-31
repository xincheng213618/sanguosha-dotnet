#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairObtainFixedRecipientReceiptsEntries
{
    internal void AssertPairBenefitState(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertPairBenefitState(f);
    internal bool IsPairBenefitMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? paid, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsPairBenefitMovement(f, paid, movement);
    internal bool ValidPairObtain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPairObtain(f);
    internal bool ValidShownPairGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidShownPairGift(f);
}
