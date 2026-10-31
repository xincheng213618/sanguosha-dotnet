#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairObtainFixedRecipientReceiptsEntries
{
    internal bool ValidPairObtain(ProgramSkillFrame f) => _resolve().ValidPairObtain(f);
    internal bool ValidShownPairGift(ProgramSkillFrame f) => _resolve().ValidShownPairGift(f);
}
