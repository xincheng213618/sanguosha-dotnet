#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramActivationLedgerEntries
{
    internal void AccumulateProgramSelectedCardCount(global::CardGame.Core.ProgramSkillFrame frame, string usageId, int threshold, string resultBind) => _resolve().AccumulateProgramSelectedCardCount(frame, usageId, threshold, resultBind);
}
