#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePersistentPrivateWhirlRuntime
{
    internal static bool IsPrivateWhirlDeposit(SkillProgramEffectOp op) => op is SkillProgramEffectOp.DepositPrivateWhirlUntilNextPlayEnding or SkillProgramEffectOp.DepositPersistentPrivateWhirl;
    internal int PrivateWhirlMaximum(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program!.Triggers.Any(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.DepositPersistentPrivateWhirl)) ? int.MaxValue : 5;
}
