using Il2CppSLZ.Marrow.Warehouse;

[RegisterTypeInIl2Cpp]
public class Flask : ScriptableObject
{
    public Flask(IntPtr ptr) : base(ptr) { }

    public string[] elixirGUIDs;
    public bool useDefaultIngredients = true;
    public string[] ingredients;
    public string[] additionalIngredients;
}