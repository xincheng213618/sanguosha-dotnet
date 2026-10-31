#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateOfferObserversEntries
{
    internal bool IsPrivateOfferProgramDying() => _resolve().IsPrivateOfferProgramDying();
    internal bool PrivateOfferFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().PrivateOfferFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? PrivateOfferObserverRoot() => _resolve().PrivateOfferObserverRoot();
}
