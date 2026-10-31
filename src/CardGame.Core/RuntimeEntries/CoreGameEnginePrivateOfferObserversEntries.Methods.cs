#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateOfferObserversEntries
{
    internal bool IsPrivateOfferProgramDying() => _resolve().IsPrivateOfferProgramDying();
    internal bool PrivateOfferFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().PrivateOfferFirstChild(root, child);
    internal ProgramSkillFrame? PrivateOfferObserverRoot() => _resolve().PrivateOfferObserverRoot();
}
