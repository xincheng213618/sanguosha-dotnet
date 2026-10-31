#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnViewAsEntries
{
    internal bool ActualTurnConversionUsageUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().ActualTurnConversionUsageUnavailable(query);
    internal bool CanUseActualTurnLimitedViewAs(global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, global::CardGame.Core.CharacterState owner) => _resolve().CanUseActualTurnLimitedViewAs(instance, rule, owner);
    internal void ConsumeActualTurnLimitedViewAs(global::CardGame.Core.CardConversionSource source) => _resolve().ConsumeActualTurnLimitedViewAs(source);
}
