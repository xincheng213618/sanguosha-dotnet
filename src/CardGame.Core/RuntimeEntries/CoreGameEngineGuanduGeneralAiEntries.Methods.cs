#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGuanduGeneralAiEntries
{
    internal PromptChoice SelectGuanduGeneralAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectGuanduGeneralAi(decision, f);
    internal int VaryGuanduRecipientHand(int seat) => _resolve().VaryGuanduRecipientHand(seat);
}
