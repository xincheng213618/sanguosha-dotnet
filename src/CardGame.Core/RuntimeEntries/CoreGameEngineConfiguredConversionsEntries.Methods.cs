#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConfiguredConversionsEntries
{
    internal SkillProgramStepOutcome BeginConfiguredCardDeclaration(ProgramSkillFrame frame, string bind, string state) => _resolve().BeginConfiguredCardDeclaration(frame, bind, state);
    internal bool ConfiguredConversionUnavailable(ProgramConfiguredViewAsQuery query) => _resolve().ConfiguredConversionUnavailable(query);
    internal IReadOnlyList<CardKind> DeclarationNames(ProgramSkillFrame frame, ProgramCardDeclarationDraft draft) => _resolve().DeclarationNames(frame, draft);
    internal IReadOnlyDictionary<string, int>? GetConfiguredConversionTierSnapshot(CharacterState owner) => _resolve().GetConfiguredConversionTierSnapshot(owner);
    internal bool IsConfiguredConversionAvailable(CharacterState owner, Card card, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool dyingUse, int? requestedUseTarget = default) => _resolve().IsConfiguredConversionAvailable(owner, card, instance, rule, dyingUse, requestedUseTarget);
    internal void PublishConfiguredCardDeclaration(ProgramSkillFrame frame) => _resolve().PublishConfiguredCardDeclaration(frame);
    internal void ResolveConfiguredCardDeclaration(PromptChoice choice) => _resolve().ResolveConfiguredCardDeclaration(choice);
}
