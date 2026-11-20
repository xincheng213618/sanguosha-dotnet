#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoJiaMobileGodRuntime
{
    internal bool HasShenGuoJiaAwakeningReceipt(ProgramSkillFrame frame) => _awakeningOverrides.Issued(frame);
}
