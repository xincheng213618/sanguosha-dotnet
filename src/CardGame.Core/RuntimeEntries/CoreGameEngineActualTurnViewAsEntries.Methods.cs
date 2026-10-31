#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnViewAsEntries
{
    internal bool ActualTurnConversionUsageUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().ActualTurnConversionUsageUnavailable(query);
    internal bool CanUseActualTurnLimitedViewAs(IndexedSkillProgramInstance instance, SkillProgramViewAs rule, CharacterState owner) => _resolve().CanUseActualTurnLimitedViewAs(instance, rule, owner);
    internal void ConsumeActualTurnLimitedViewAs(CardConversionSource source) => _resolve().ConsumeActualTurnLimitedViewAs(source);
}
