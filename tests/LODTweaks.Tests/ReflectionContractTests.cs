using Mono.Cecil;

namespace LODTweaks.Tests;

public sealed class ReflectionContractTests
{
    [Fact]
    public void LodGroupShouldExposeInheritedOnInitTarget()
    {
        using AssemblyDefinition frooxEngineAssembly = AssemblyDefinition.ReadAssembly(GetAssemblyPath("FrooxEngine.dll"));
        TypeDefinition componentBase = GetRequiredType(frooxEngineAssembly, "FrooxEngine.ComponentBase`1");

        Assert.Contains(componentBase.Methods, static method => method.Name == "OnInit" && !method.HasParameters);
    }

    [Fact]
    public void LodGroupPatchShouldSetLateUpdateOrder()
    {
        using AssemblyDefinition modAssembly = AssemblyDefinition.ReadAssembly(GetAssemblyPath("LODTweaks.dll"));
        TypeDefinition patchType = GetRequiredType(modAssembly, "LODTweaks.LODGroupOnInitPatch");
        MethodDefinition targetMethod = Assert.Single(patchType.Methods, static method => method.Name == "TargetMethod");
        MethodDefinition postfix = Assert.Single(patchType.Methods, static method => method.Name == "Postfix");

        Assert.Equal("System.Reflection.MethodInfo", targetMethod.ReturnType.FullName);
        Assert.Contains(
            postfix.Body.Instructions,
            static instruction => instruction.OpCode.Code == Mono.Cecil.Cil.Code.Ldc_I4 && Equals(instruction.Operand, 1000));
    }

    [Theory]
    [InlineData("LODTweaks.LODGroupOnInitPatch")]
    [InlineData("LODTweaks.WorkerInspectorBuildInspectorUIPatch")]
    public void HarmonyPatchTypesShouldDeclarePatchMetadata(string typeName)
    {
        using AssemblyDefinition modAssembly = AssemblyDefinition.ReadAssembly(GetAssemblyPath("LODTweaks.dll"));
        TypeDefinition patchType = GetRequiredType(modAssembly, typeName);

        Assert.Contains(
            patchType.CustomAttributes,
            static attribute => attribute.AttributeType.FullName is "HarmonyLib.HarmonyPatch");
    }

    private static string GetAssemblyPath(string assemblyFileName)
    {
        return Path.Combine(AppContext.BaseDirectory, assemblyFileName);
    }

    private static TypeDefinition GetRequiredType(AssemblyDefinition assembly, string fullName)
    {
        return assembly.MainModule.GetType(fullName)
            ?? throw new InvalidOperationException($"Type '{fullName}' was not found in '{assembly.MainModule.FileName}'.");
    }
}
