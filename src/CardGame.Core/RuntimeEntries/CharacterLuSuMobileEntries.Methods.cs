#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuMobileEntries
{
    internal bool KeepMobileLuSuDimengPaid(ProgramSkillFrame f) => _resolve().KeepMobileLuSuDimengPaid(f);
    internal bool MobileLuSuDimengPaid(ProgramSkillFrame f) => _resolve().MobileLuSuDimengPaid(f);
    internal bool ReturnMobileLuSuDimengPaid(ProgramSkillFrame f) => _resolve().ReturnMobileLuSuDimengPaid(f);
}
