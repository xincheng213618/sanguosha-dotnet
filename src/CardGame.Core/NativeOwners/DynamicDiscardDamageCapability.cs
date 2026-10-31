#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DynamicDiscardDamageCapability(Func<RuntimeEntries.CoreGameEngineDynamicDiscardDamagePaymentsEntries> getCoreGameEngineDynamicDiscardDamagePaymentsRuntime) : IDynamicDiscardDamageHost
{
    public SkillProgramStepOutcome DiscardTargetHpCardsAndDamage(ProgramSkillFrame f) => getCoreGameEngineDynamicDiscardDamagePaymentsRuntime().BeginDynamicDiscardDamage(f);
}
