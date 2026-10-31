#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGainGiftObserversEntries
{
    internal bool GainGiftFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().GainGiftFirstChild(root, child);
    internal ProgramSkillFrame? GainGiftObserverRoot() => _resolve().GainGiftObserverRoot();
    internal bool IsGainGiftProgramDying() => _resolve().IsGainGiftProgramDying();
    internal bool ValidGainGiftReceipt(ProgramSkillFrame f) => _resolve().ValidGainGiftReceipt(f);
}
