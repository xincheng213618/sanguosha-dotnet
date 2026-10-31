#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedUseConversionsEntries
{
    internal void ConsumeNamedUseConversion(CardConversionSource source) => _resolve().ConsumeNamedUseConversion(source);
    internal bool IsNamedUseConversionAvailable(CharacterState owner, IndexedSkillProgramInstance instance, SkillProgramViewAs rule) => _resolve().IsNamedUseConversionAvailable(owner, instance, rule);
    internal bool NamedConversionUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().NamedConversionUnavailable(query);
    internal void ResetTurnProgramBooleanStates() => _resolve().ResetTurnProgramBooleanStates();
}
