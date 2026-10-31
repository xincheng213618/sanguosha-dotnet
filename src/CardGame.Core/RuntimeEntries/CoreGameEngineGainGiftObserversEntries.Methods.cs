#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGainGiftObserversEntries
{
    internal bool GainGiftFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().GainGiftFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? GainGiftObserverRoot() => _resolve().GainGiftObserverRoot();
    internal bool IsGainGiftProgramDying() => _resolve().IsGainGiftProgramDying();
    internal bool ValidGainGiftReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidGainGiftReceipt(f);
}
