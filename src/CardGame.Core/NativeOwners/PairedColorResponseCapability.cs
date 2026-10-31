#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class PairedColorResponseCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePairedColorResponsesEntries> getCoreGameEnginePairedColorResponsesRuntime) : IPairedColorResponseHost
{
    public SkillProgramStepOutcome OfferPairedColorCardDisposition(ProgramSkillFrame frame, string stateId) => getCoreGameEnginePairedColorResponsesRuntime().OfferPairedColorCardDisposition(frame, stateId);
}
