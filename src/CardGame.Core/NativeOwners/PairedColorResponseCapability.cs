#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PairedColorResponseCapability(Func<RuntimeEntries.CoreGameEnginePairedColorResponsesEntries> getCoreGameEnginePairedColorResponsesRuntime) : IPairedColorResponseHost
{
    public SkillProgramStepOutcome OfferPairedColorCardDisposition(ProgramSkillFrame frame, string stateId) => getCoreGameEnginePairedColorResponsesRuntime().OfferPairedColorCardDisposition(frame, stateId);
}
