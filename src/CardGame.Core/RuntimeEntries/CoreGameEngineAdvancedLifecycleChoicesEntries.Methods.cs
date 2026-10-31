#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdvancedLifecycleChoicesEntries
{
    internal bool AllowsDamageFarthestCharacterActivation(ProgramActivationLegalityContext gate) => _resolve().AllowsDamageFarthestCharacterActivation(gate);
    internal void PromptAdvancedSelection(long frameId) => _resolve().PromptAdvancedSelection(frameId);
    internal void ResolveAdvancedLifecycleChoice(PromptChoice choice) => _resolve().ResolveAdvancedLifecycleChoice(choice);
    internal bool TryContinueAdvancedDamage(long frameId) => _resolve().TryContinueAdvancedDamage(frameId);
}
