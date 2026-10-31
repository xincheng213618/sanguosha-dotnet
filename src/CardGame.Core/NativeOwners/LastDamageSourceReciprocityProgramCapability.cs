#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class LastDamageSourceReciprocityProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineLastDamageSourceReciprocityEntries> getCoreGameEngineLastDamageSourceReciprocityRuntime) : ILastDamageSourceReciprocityProgramHost
{
    public SkillProgramStepOutcome LastDamageSourceReciprocity(ProgramSkillFrame f, string stateId) => getCoreGameEngineLastDamageSourceReciprocityRuntime().BeginLastDamageSourceReciprocity(f, stateId);
}
