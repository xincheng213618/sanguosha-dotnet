#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrepDiscardReceiptsEntries
{
    internal bool ValidPrepDiscard(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidPrepDiscard(f);
    internal bool ValidPrepDiscardPayment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PrepDiscardPayment p, bool ownerCost) => _resolve().ValidPrepDiscardPayment(f, p, ownerCost);
}
