#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5HuashenEntries
{
    internal SkillProgramStepOutcome ChooseOrExchangeShanheS5Avatar(SkillProgramEffect effect, ProgramSkillFrame frame) => _resolve().ChooseOrExchangeShanheS5Avatar(effect, frame);
    internal void FinishShanheS5AvatarChoice(ProgramSkillFrame frame) => _resolve().FinishShanheS5AvatarChoice(frame);
    internal void PublishShanheS5AvatarChoice(ProgramSkillFrame frame) => _resolve().PublishShanheS5AvatarChoice(frame);
    internal void ResolveShanheS5AvatarChoice(PromptChoice choice) => _resolve().ResolveShanheS5AvatarChoice(choice);
    internal bool ResumeShanheS5AvatarChoice(long id) => _resolve().ResumeShanheS5AvatarChoice(id);
    internal PrivateGeneralLibrary? ShanheS5AvatarChoiceLibrary(ProgramSkillFrame frame) => _resolve().ShanheS5AvatarChoiceLibrary(frame);
    internal IReadOnlyList<PromptChoice> ShanheS5AvatarChoices(ProgramSkillFrame frame) => _resolve().ShanheS5AvatarChoices(frame);
}
