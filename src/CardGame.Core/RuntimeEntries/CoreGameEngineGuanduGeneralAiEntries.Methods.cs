#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralAiEntries
{
    internal global::CardGame.Core.PromptChoice SelectGuanduGeneralAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectGuanduGeneralAi(decision, f);
    internal int VaryGuanduRecipientHand(int seat) => _resolve().VaryGuanduRecipientHand(seat);
}
