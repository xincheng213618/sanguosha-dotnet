#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DynamicDiscardDamageCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDynamicDiscardDamagePaymentsEntries> getCoreGameEngineDynamicDiscardDamagePaymentsRuntime) : IDynamicDiscardDamageHost
{
    public SkillProgramStepOutcome DiscardTargetHpCardsAndDamage(ProgramSkillFrame f) => getCoreGameEngineDynamicDiscardDamagePaymentsRuntime().BeginDynamicDiscardDamage(f);
}
