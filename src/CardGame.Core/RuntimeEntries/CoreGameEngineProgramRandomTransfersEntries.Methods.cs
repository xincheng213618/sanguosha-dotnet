#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRandomTransfersEntries
{
    internal void ContinueRandomTransferAfterPublicReveal(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueRandomTransferAfterPublicReveal(frame);
    internal bool DoesProgramFrozenSuitMatchChoice(global::CardGame.Core.ProgramSkillFrame frame, string cardBind, string choiceBind) => _resolve().DoesProgramFrozenSuitMatchChoice(frame, cardBind, choiceBind);
    internal global::CardGame.Core.SkillProgramStepOutcome TransferProgramRandomOwnedCard(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string resultBind) => _resolve().TransferProgramRandomOwnedCard(frame, targetSeat, resultBind);
}
