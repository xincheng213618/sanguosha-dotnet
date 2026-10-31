#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionProofsEntries
{
    internal void RequireRandomAreaAcquisition(ProgramSkillFrame frame) => _resolve().RequireRandomAreaAcquisition(frame);
    internal bool ValidRandomAreaAcquisition(ProgramSkillFrame frame) => _resolve().ValidRandomAreaAcquisition(frame);
    internal bool ValidRandomAreaStep(ProgramSkillFrame frame, RandomAreaAcquisitionStepReceipt step) => _resolve().ValidRandomAreaStep(frame, step);
}
