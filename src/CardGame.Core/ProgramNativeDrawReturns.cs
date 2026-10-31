using System.Collections.ObjectModel;
using System.Reflection;

namespace CardGame.Core;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
internal sealed class ProgramNativeDrawReturnAttribute(params SkillProgramEffectOp[] operations) : Attribute
{
    internal IReadOnlyList<SkillProgramEffectOp> Operations { get; } = Array.AsReadOnly(operations);
}
