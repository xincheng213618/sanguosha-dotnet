#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionProofsEntries
{
    internal void AssertRandomAreaAcquisition(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRandomAreaAcquisition(frame);
    internal void RequireRandomAreaAcquisition(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequireRandomAreaAcquisition(frame);
    internal bool ValidRandomAreaAcquisition(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidRandomAreaAcquisition(frame);
    internal bool ValidRandomAreaStep(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RandomAreaAcquisitionStepReceipt step) => _resolve().ValidRandomAreaStep(frame, step);
}
