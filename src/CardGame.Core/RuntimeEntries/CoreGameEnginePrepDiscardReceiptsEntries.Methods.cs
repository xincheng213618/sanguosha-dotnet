#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardReceiptsEntries
{
    internal bool ValidPrepDiscard(ProgramSkillFrame f) => _resolve().ValidPrepDiscard(f);
    internal bool ValidPrepDiscardPayment(ProgramSkillFrame f, PrepDiscardPayment p, bool ownerCost) => _resolve().ValidPrepDiscardPayment(f, p, ownerCost);
}
