#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYaDanOlRuntime
{
    internal const string QingyaDiscardReason = global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ProgramDiscardTopPlacement.QingyaDiscardReason;
    internal bool HasYaDanCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveQingyaBetweenHandDiscards);

    internal static bool IsYaDanOperation(SkillProgramEffectOp op) => global::CardGame.Core.RuntimeSkills.RuntimeRules_ya_dan_ol_YaDanLedger.IsYaDanOperation(op);
    internal static CardConversionSource YaDanSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_ya_dan_ol_YaDanLedger.YaDanSource(f);
}
