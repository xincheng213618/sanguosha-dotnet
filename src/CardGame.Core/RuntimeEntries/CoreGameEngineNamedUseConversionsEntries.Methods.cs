#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedUseConversionsEntries
{
    internal void ConsumeNamedUseConversion(global::CardGame.Core.CardConversionSource source) => _resolve().ConsumeNamedUseConversion(source);
    internal bool IsNamedUseConversionAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule) => _resolve().IsNamedUseConversionAvailable(owner, instance, rule);
    internal bool NamedConversionUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().NamedConversionUnavailable(query);
    internal void ResetTurnProgramBooleanStates() => _resolve().ResetTurnProgramBooleanStates();
}
