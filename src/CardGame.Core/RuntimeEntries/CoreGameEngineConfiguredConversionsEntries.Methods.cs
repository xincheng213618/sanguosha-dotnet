#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConfiguredConversionsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginConfiguredCardDeclaration(global::CardGame.Core.ProgramSkillFrame frame, string bind, string state) => _resolve().BeginConfiguredCardDeclaration(frame, bind, state);
    internal bool ConfiguredConversionUnavailable(global::CardGame.Core.ProgramConfiguredViewAsQuery query) => _resolve().ConfiguredConversionUnavailable(query);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind> DeclarationNames(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramCardDeclarationDraft draft) => _resolve().DeclarationNames(frame, draft);
    internal global::System.Collections.Generic.IReadOnlyDictionary<string, int>? GetConfiguredConversionTierSnapshot(global::CardGame.Core.CharacterState owner) => _resolve().GetConfiguredConversionTierSnapshot(owner);
    internal bool IsConfiguredConversionAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool dyingUse, int? requestedUseTarget = default) => _resolve().IsConfiguredConversionAvailable(owner, card, instance, rule, dyingUse, requestedUseTarget);
    internal void PublishConfiguredCardDeclaration(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishConfiguredCardDeclaration(frame);
    internal void ResolveConfiguredCardDeclaration(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveConfiguredCardDeclaration(choice);
}
