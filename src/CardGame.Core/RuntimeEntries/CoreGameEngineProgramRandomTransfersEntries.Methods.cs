#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRandomTransfersEntries
{
    internal void ContinueRandomTransferAfterPublicReveal(ProgramSkillFrame frame) => _resolve().ContinueRandomTransferAfterPublicReveal(frame);
    internal bool DoesProgramFrozenSuitMatchChoice(ProgramSkillFrame frame, string cardBind, string choiceBind) => _resolve().DoesProgramFrozenSuitMatchChoice(frame, cardBind, choiceBind);
    internal SkillProgramStepOutcome TransferProgramRandomOwnedCard(ProgramSkillFrame frame, int targetSeat, string resultBind) => _resolve().TransferProgramRandomOwnedCard(frame, targetSeat, resultBind);
}
