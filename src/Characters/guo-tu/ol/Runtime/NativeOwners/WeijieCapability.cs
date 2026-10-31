#nullable enable
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CardGame.Core.NativeOwners;
internal sealed class WeijieCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries> getCharacterGuoTuOlRuntime) : IWeijieHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterGuoTuOlRuntime().ResolveWeijie(f);
}
